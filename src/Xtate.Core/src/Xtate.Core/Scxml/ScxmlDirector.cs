﻿using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Xml;
using TSSArt.StateMachine.Annotations;

namespace TSSArt.StateMachine
{
	[PublicAPI]
	public class ScxmlDirector : XmlDirector<ScxmlDirector>
	{
		private const string ScxmlNs       = "http://www.w3.org/2005/07/scxml";
		private const string TSSArtScxmlNs = "http://tssart.com/scxml";
		private const char   Space         = ' ';

		private static readonly string[] Keywords =
		{
				"array", "assign", "autoforward", "binding", "cancel", "cond", "content", "data", "datamodel", "delay", "delayexpr", "donedata", "else", "elseif", "event",
				"eventexpr", "expr", "final", "finalize", "foreach", "history", "id", "idlocation", "if", "index", "initial", "invoke", "item", "label", "location", "log",
				"name", "namelist", "onentry", "onexit", "parallel", "param", "persistence", "queueSize", "raise", "script", "scxml", "send", "sendid", "sendidexpr", "src",
				"srcexpr", "state", "synchronous", "target", "targetexpr", "transition", "type", "typeexpr", "version"
		};

		private static readonly char[] SpaceSplitter = { Space };

		private static readonly Policy<IStateMachineBuilder> StateMachinePolicy = BuildPolicy<IStateMachineBuilder>(StateMachineBuildPolicy);
		private static readonly Policy<IStateBuilder>        StatePolicy        = BuildPolicy<IStateBuilder>(StateBuildPolicy);
		private static readonly Policy<IParallelBuilder>     ParallelPolicy     = BuildPolicy<IParallelBuilder>(ParallelBuildPolicy);
		private static readonly Policy<IFinalBuilder>        FinalPolicy        = BuildPolicy<IFinalBuilder>(FinalBuildPolicy);
		private static readonly Policy<IInitialBuilder>      InitialPolicy      = BuildPolicy<IInitialBuilder>(InitialBuildPolicy);
		private static readonly Policy<IHistoryBuilder>      HistoryPolicy      = BuildPolicy<IHistoryBuilder>(HistoryBuildPolicy);
		private static readonly Policy<ITransitionBuilder>   TransitionPolicy   = BuildPolicy<ITransitionBuilder>(TransitionBuildPolicy);
		private static readonly Policy<ILogBuilder>          LogPolicy          = BuildPolicy<ILogBuilder>(LogBuildPolicy);
		private static readonly Policy<ISendBuilder>         SendPolicy         = BuildPolicy<ISendBuilder>(SendBuildPolicy);
		private static readonly Policy<IParamBuilder>        ParamPolicy        = BuildPolicy<IParamBuilder>(ParamBuildPolicy);
		private static readonly Policy<IContentBuilder>      ContentPolicy      = BuildPolicy<IContentBuilder>(ContentBuildPolicy);
		private static readonly Policy<IOnEntryBuilder>      OnEntryPolicy      = BuildPolicy<IOnEntryBuilder>(OnEntryBuildPolicy);
		private static readonly Policy<IOnExitBuilder>       OnExitPolicy       = BuildPolicy<IOnExitBuilder>(OnExitBuildPolicy);
		private static readonly Policy<IInvokeBuilder>       InvokePolicy       = BuildPolicy<IInvokeBuilder>(InvokeBuildPolicy);
		private static readonly Policy<IFinalizeBuilder>     FinalizePolicy     = BuildPolicy<IFinalizeBuilder>(FinalizeBuildPolicy);
		private static readonly Policy<IScriptBuilder>       ScriptPolicy       = BuildPolicy<IScriptBuilder>(ScriptBuildPolicy);
		private static readonly Policy<IDataModelBuilder>    DataModelPolicy    = BuildPolicy<IDataModelBuilder>(DataModelBuildPolicy);
		private static readonly Policy<IDataBuilder>         DataPolicy         = BuildPolicy<IDataBuilder>(DataBuildPolicy);
		private static readonly Policy<IDoneDataBuilder>     DoneDataPolicy     = BuildPolicy<IDoneDataBuilder>(DoneDataBuildPolicy);
		private static readonly Policy<IForEachBuilder>      ForEachPolicy      = BuildPolicy<IForEachBuilder>(ForEachBuildPolicy);
		private static readonly Policy<IIfBuilder>           IfPolicy           = BuildPolicy<IIfBuilder>(IfBuildPolicy);
		private static readonly Policy<IElseBuilder>         ElsePolicy         = BuildPolicy<IElseBuilder>(ElseBuildPolicy);
		private static readonly Policy<IElseIfBuilder>       ElseIfPolicy       = BuildPolicy<IElseIfBuilder>(ElseIfBuildPolicy);
		private static readonly Policy<IRaiseBuilder>        RaisePolicy        = BuildPolicy<IRaiseBuilder>(RaiseBuildPolicy);
		private static readonly Policy<IAssignBuilder>       AssignPolicy       = BuildPolicy<IAssignBuilder>(AssignBuildPolicy);
		private static readonly Policy<ICancelBuilder>       CancelPolicy       = BuildPolicy<ICancelBuilder>(CancelBuildPolicy);

		private readonly IErrorProcessor _errorProcessor;
		private readonly IBuilderFactory _factory;

		public ScxmlDirector(XmlReader xmlReader, IBuilderFactory factory, IErrorProcessor errorProcessor) : base(xmlReader, errorProcessor)
		{
			_factory = factory;
			_errorProcessor = errorProcessor;
		}

		public static void FillXmlNameTable(XmlNameTable xmlNameTable)
		{
			if (xmlNameTable == null) throw new ArgumentNullException(nameof(xmlNameTable));

			xmlNameTable.Add(ScxmlNs);
			xmlNameTable.Add(TSSArtScxmlNs);

			foreach (var keyword in Keywords)
			{
				xmlNameTable.Add(keyword);
			}
		}

		public IStateMachine ConstructStateMachine(IStateMachineValidator? stateMachineValidator = default)
		{
			var stateMachine = ReadStateMachine();

			stateMachineValidator?.Validate(stateMachine, _errorProcessor);

			return stateMachine;
		}

		private static IIdentifier AsIdentifier(string val)
		{
			if (val == null) throw new ArgumentNullException(nameof(val));

			return (Identifier) val;
		}

		private static IOutgoingEvent AsEvent(string val)
		{
			if (val == null) throw new ArgumentNullException(nameof(val));

			return new EventEntity(val) { Target = EventEntity.InternalTarget };
		}

		private static ImmutableArray<IIdentifier> AsIdentifierList(string val)
		{
			if (string.IsNullOrEmpty(val))
			{
				throw new ArgumentException(Resources.Exception_ListOfIdentifiersCannotBeEmpty, nameof(val));
			}

			if (val.IndexOf(Space) < 0)
			{
				return ImmutableArray.Create<IIdentifier>((Identifier) val);
			}

			var identifiers = val.Split(SpaceSplitter, StringSplitOptions.RemoveEmptyEntries);

			if (identifiers.Length == 0)
			{
				throw new ArgumentException(Resources.Exception_ListOfIdentifiersCannotBeEmpty, nameof(val));
			}

			var builder = ImmutableArray.CreateBuilder<IIdentifier>(identifiers.Length);

			foreach (var identifier in identifiers)
			{
				builder.Add((Identifier) identifier);
			}

			return builder.MoveToImmutable();
		}

		private static ImmutableArray<IEventDescriptor> AsEventDescriptorList(string val)
		{
			if (string.IsNullOrEmpty(val))
			{
				throw new ArgumentException(Resources.Exception_ListOfEventsCannotBeEmpty, nameof(val));
			}

			if (val.IndexOf(Space) < 0)
			{
				return ImmutableArray.Create<IEventDescriptor>((EventDescriptor) val);
			}

			var eventDescriptors = val.Split(SpaceSplitter, StringSplitOptions.RemoveEmptyEntries);

			if (eventDescriptors.Length == 0)
			{
				throw new ArgumentException(Resources.Exception_ListOfEventsCannotBeEmpty, nameof(val));
			}

			var builder = ImmutableArray.CreateBuilder<IEventDescriptor>(eventDescriptors.Length);

			foreach (var identifier in eventDescriptors)
			{
				builder.Add((EventDescriptor) identifier);
			}

			return builder.MoveToImmutable();
		}

		private static IConditionExpression AsConditionalExpression(string expression)
		{
			if (string.IsNullOrEmpty(expression))
			{
				throw new ArgumentException(Resources.Exception_ConditionDoesNotSpecified, nameof(expression));
			}

			return new ConditionExpression { Expression = expression };
		}

		private static ILocationExpression AsLocationExpression(string expression)
		{
			if (string.IsNullOrEmpty(expression))
			{
				throw new ArgumentException(Resources.Exception_LocationDoesNotSpecified, nameof(expression));
			}

			return new LocationExpression { Expression = expression };
		}

		private static ImmutableArray<ILocationExpression> AsLocationExpressionList(string expression)
		{
			if (string.IsNullOrEmpty(expression))
			{
				throw new ArgumentException(Resources.Exception_ListOfLocationsCannotBeEmpty, nameof(expression));
			}

			if (expression.IndexOf(Space) < 0)
			{
				return ImmutableArray.Create<ILocationExpression>(new LocationExpression { Expression = expression });
			}

			var locationExpressions = expression.Split(SpaceSplitter, StringSplitOptions.RemoveEmptyEntries);

			if (locationExpressions.Length == 0)
			{
				throw new ArgumentException(Resources.Exception_ListOfLocationsCannotBeEmpty, nameof(expression));
			}

			var builder = ImmutableArray.CreateBuilder<ILocationExpression>(locationExpressions.Length);

			foreach (var locationExpression in locationExpressions)
			{
				builder.Add(new LocationExpression { Expression = locationExpression });
			}

			return builder.MoveToImmutable();
		}

		private static IValueExpression AsValueExpression(string expression)
		{
			if (expression == null) throw new ArgumentNullException(nameof(expression));

			return new ValueExpression { Expression = expression };
		}

		private static IScriptExpression AsScriptExpression(string expression)
		{
			if (expression == null) throw new ArgumentNullException(nameof(expression));

			return new ScriptExpression { Expression = expression };
		}

		private static IExternalScriptExpression AsExternalScriptExpression(string uri)
		{
			if (uri == null) throw new ArgumentNullException(nameof(uri));

			return new ExternalScriptExpression { Uri = new Uri(uri, UriKind.RelativeOrAbsolute) };
		}

		private static IExternalDataExpression AsExternalDataExpression(string uri)
		{
			if (uri == null) throw new ArgumentNullException(nameof(uri));

			return new ExternalDataExpression { Uri = new Uri(uri, UriKind.RelativeOrAbsolute) };
		}

		private static Uri AsUri(string uri)
		{
			if (uri == null) throw new ArgumentNullException(nameof(uri));

			return new Uri(uri, UriKind.RelativeOrAbsolute);
		}

		private static T AsEnum<T>(string val) where T : struct
		{
			if (val == null) throw new ArgumentNullException(nameof(val));

			if (!Enum.TryParse(val, ignoreCase: true, out T result) || val.ToLowerInvariant() != val)
			{
				throw new ArgumentException(Res.Format(Resources.Exception_ValueCannotBeParsed, typeof(T).Name));
			}

			return result;
		}

		private static int AsMilliseconds(string val)
		{
			if (string.IsNullOrEmpty(val))
			{
				throw new ArgumentException(Resources.Exception_ValueCantBeEmpty, nameof(val));
			}

			if (val == @"0")
			{
				return 0;
			}

			const string ms = "ms";
			if (val.EndsWith(ms, StringComparison.Ordinal))
			{
				return int.Parse(val.Substring(startIndex: 0, val.Length - ms.Length), NumberFormatInfo.InvariantInfo);
			}

			const string s = "s";
			if (val.EndsWith(s, StringComparison.Ordinal))
			{
				return int.Parse(val.Substring(startIndex: 0, val.Length - s.Length), NumberFormatInfo.InvariantInfo) * 1000;
			}

			throw new ArgumentException(Resources.Exception_DelayParsingError);
		}

		private static void CheckScxmlVersion(string version)
		{
			if (version == @"1.0")
			{
				return;
			}

			throw new ArgumentException(Resources.Exception_UnsupportedScxmlVersion);
		}

		private object? CreateAncestor() => _errorProcessor.LineInfoRequired && HasLineInfo() ? new XmlLineInfo(LineNumber, LinePosition) : null;

		private IStateMachine ReadStateMachine() => Populate(_factory.CreateStateMachineBuilder(CreateAncestor()), StateMachinePolicy).Build();

		private static void StateMachineBuildPolicy(IPolicyBuilder<IStateMachineBuilder> pb) =>
				pb.ValidateElementName(ScxmlNs, name: "scxml")
				  .RequiredAttribute(name: "version", (dr, b) => CheckScxmlVersion(dr.Current))
				  .OptionalAttribute(name: "initial", (dr, b) => b.SetInitial(AsIdentifierList(dr.Current)))
				  .OptionalAttribute(name: "datamodel", (dr, b) => b.SetDataModelType(dr.Current))
				  .OptionalAttribute(name: "binding", (dr, b) => b.SetBindingType(AsEnum<BindingType>(dr.Current)))
				  .OptionalAttribute(name: "name", (dr, b) => b.SetName(dr.Current))
				  .MultipleElements(ScxmlNs, name: "state", (dr, b) => b.AddState(dr.ReadState()))
				  .MultipleElements(ScxmlNs, name: "parallel", (dr, b) => b.AddParallel(dr.ReadParallel()))
				  .MultipleElements(ScxmlNs, name: "final", (dr, b) => b.AddFinal(dr.ReadFinal()))
				  .OptionalElement(ScxmlNs, name: "datamodel", (dr, b) => b.SetDataModel(dr.ReadDataModel()))
				  .OptionalElement(ScxmlNs, name: "script", (dr, b) => b.SetScript(dr.ReadScript()))
				  .OptionalAttribute(TSSArtScxmlNs, name: "synchronous", (dr, b) => b.SetSynchronousEventProcessing(XmlConvert.ToBoolean(dr.Current)))
				  .OptionalAttribute(TSSArtScxmlNs, name: "queueSize", (dr, b) => b.SetExternalQueueSize(XmlConvert.ToInt32(dr.Current)))
				  .OptionalAttribute(TSSArtScxmlNs, name: "persistence", (dr, b) => b.SetPersistenceLevel((PersistenceLevel) Enum.Parse(typeof(PersistenceLevel), dr.Current)));

		private IState ReadState() => Populate(_factory.CreateStateBuilder(CreateAncestor()), StatePolicy).Build();

		private static void StateBuildPolicy(IPolicyBuilder<IStateBuilder> pb) =>
				pb.OptionalAttribute(name: "id", (dr, b) => b.SetId(AsIdentifier(dr.Current)))
				  .OptionalAttribute(name: "initial", (dr, b) => b.SetInitial(AsIdentifierList(dr.Current)))
				  .MultipleElements(ScxmlNs, name: "state", (dr, b) => b.AddState(dr.ReadState()))
				  .MultipleElements(ScxmlNs, name: "parallel", (dr, b) => b.AddParallel(dr.ReadParallel()))
				  .MultipleElements(ScxmlNs, name: "final", (dr, b) => b.AddFinal(dr.ReadFinal()))
				  .MultipleElements(ScxmlNs, name: "history", (dr, b) => b.AddHistory(dr.ReadHistory()))
				  .MultipleElements(ScxmlNs, name: "invoke", (dr, b) => b.AddInvoke(dr.ReadInvoke()))
				  .MultipleElements(ScxmlNs, name: "transition", (dr, b) => b.AddTransition(dr.ReadTransition()))
				  .MultipleElements(ScxmlNs, name: "onentry", (dr, b) => b.AddOnEntry(dr.ReadOnEntry()))
				  .MultipleElements(ScxmlNs, name: "onexit", (dr, b) => b.AddOnExit(dr.ReadOnExit()))
				  .OptionalElement(ScxmlNs, name: "initial", (dr, b) => b.SetInitial(dr.ReadInitial()))
				  .OptionalElement(ScxmlNs, name: "datamodel", (dr, b) => b.SetDataModel(dr.ReadDataModel()));

		private IParallel ReadParallel() => Populate(_factory.CreateParallelBuilder(CreateAncestor()), ParallelPolicy).Build();

		private static void ParallelBuildPolicy(IPolicyBuilder<IParallelBuilder> pb) =>
				pb.OptionalAttribute(name: "id", (dr, b) => b.SetId(AsIdentifier(dr.Current)))
				  .MultipleElements(ScxmlNs, name: "state", (dr, b) => b.AddState(dr.ReadState()))
				  .MultipleElements(ScxmlNs, name: "parallel", (dr, b) => b.AddParallel(dr.ReadParallel()))
				  .MultipleElements(ScxmlNs, name: "history", (dr, b) => b.AddHistory(dr.ReadHistory()))
				  .MultipleElements(ScxmlNs, name: "invoke", (dr, b) => b.AddInvoke(dr.ReadInvoke()))
				  .MultipleElements(ScxmlNs, name: "transition", (dr, b) => b.AddTransition(dr.ReadTransition()))
				  .MultipleElements(ScxmlNs, name: "onentry", (dr, b) => b.AddOnEntry(dr.ReadOnEntry()))
				  .MultipleElements(ScxmlNs, name: "onexit", (dr, b) => b.AddOnExit(dr.ReadOnExit()))
				  .OptionalElement(ScxmlNs, name: "datamodel", (dr, b) => b.SetDataModel(dr.ReadDataModel()));

		private IFinal ReadFinal() => Populate(_factory.CreateFinalBuilder(CreateAncestor()), FinalPolicy).Build();

		private static void FinalBuildPolicy(IPolicyBuilder<IFinalBuilder> pb) =>
				pb.OptionalAttribute(name: "id", (dr, b) => b.SetId(AsIdentifier(dr.Current)))
				  .MultipleElements(ScxmlNs, name: "onentry", (dr, b) => b.AddOnEntry(dr.ReadOnEntry()))
				  .MultipleElements(ScxmlNs, name: "onexit", (dr, b) => b.AddOnExit(dr.ReadOnExit()))
				  .OptionalElement(ScxmlNs, name: "donedata", (dr, b) => b.SetDoneData(dr.ReadDoneData()));

		private IInitial ReadInitial() => Populate(_factory.CreateInitialBuilder(CreateAncestor()), InitialPolicy).Build();

		private static void InitialBuildPolicy(IPolicyBuilder<IInitialBuilder> pb) => pb.SingleElement(ScxmlNs, name: "transition", (dr, b) => b.SetTransition(dr.ReadTransition()));

		private IHistory ReadHistory() => Populate(_factory.CreateHistoryBuilder(CreateAncestor()), HistoryPolicy).Build();

		private static void HistoryBuildPolicy(IPolicyBuilder<IHistoryBuilder> pb) =>
				pb.OptionalAttribute(name: "id", (dr, b) => b.SetId(AsIdentifier(dr.Current)))
				  .OptionalAttribute(name: "type", (dr, b) => b.SetType(AsEnum<HistoryType>(dr.Current)))
				  .SingleElement(ScxmlNs, name: "transition", (dr, b) => b.SetTransition(dr.ReadTransition()));

		private ITransition ReadTransition() => Populate(_factory.CreateTransitionBuilder(CreateAncestor()), TransitionPolicy).Build();

		private static void TransitionBuildPolicy(IPolicyBuilder<ITransitionBuilder> pb) =>
				pb.OptionalAttribute(name: "event", (dr, b) => b.SetEvent(AsEventDescriptorList(dr.Current)))
				  .OptionalAttribute(name: "cond", (dr, b) => b.SetCondition(AsConditionalExpression(dr.Current)))
				  .OptionalAttribute(name: "target", (dr, b) => b.SetTarget(AsIdentifierList(dr.Current)))
				  .OptionalAttribute(name: "type", (dr, b) => b.SetType(AsEnum<TransitionType>(dr.Current)))
				  .MultipleElements(ScxmlNs, name: "assign", (dr, b) => b.AddAction(dr.ReadAssign()))
				  .MultipleElements(ScxmlNs, name: "foreach", (dr, b) => b.AddAction(dr.ReadForEach()))
				  .MultipleElements(ScxmlNs, name: "if", (dr, b) => b.AddAction(dr.ReadIf()))
				  .MultipleElements(ScxmlNs, name: "log", (dr, b) => b.AddAction(dr.ReadLog()))
				  .MultipleElements(ScxmlNs, name: "raise", (dr, b) => b.AddAction(dr.ReadRaise()))
				  .MultipleElements(ScxmlNs, name: "send", (dr, b) => b.AddAction(dr.ReadSend()))
				  .MultipleElements(ScxmlNs, name: "cancel", (dr, b) => b.AddAction(dr.ReadCancel()))
				  .MultipleElements(ScxmlNs, name: "script", (dr, b) => b.AddAction(dr.ReadScript()))
				  .UnknownElement((dr, b) => b.AddAction(dr.ReadCustomAction()));

		private ILog ReadLog() => Populate(_factory.CreateLogBuilder(CreateAncestor()), LogPolicy).Build();

		private static void LogBuildPolicy(IPolicyBuilder<ILogBuilder> pb) =>
				pb.OptionalAttribute(name: "label", (dr, b) => b.SetLabel(dr.Current))
				  .OptionalAttribute(name: "expr", (dr, b) => b.SetExpression(AsValueExpression(dr.Current)));

		private ISend ReadSend() => Populate(_factory.CreateSendBuilder(CreateAncestor()), SendPolicy).Build();

		private static void SendBuildPolicy(IPolicyBuilder<ISendBuilder> pb) =>
				pb.OptionalAttribute(name: "event", (dr, b) => b.SetEvent(dr.Current))
				  .OptionalAttribute(name: "eventexpr", (dr, b) => b.SetEventExpression(AsValueExpression(dr.Current)))
				  .OptionalAttribute(name: "target", (dr, b) => b.SetTarget(AsUri(dr.Current)))
				  .OptionalAttribute(name: "targetexpr", (dr, b) => b.SetTargetExpression(AsValueExpression(dr.Current)))
				  .OptionalAttribute(name: "type", (dr, b) => b.SetType(AsUri(dr.Current)))
				  .OptionalAttribute(name: "typeexpr", (dr, b) => b.SetTypeExpression(AsValueExpression(dr.Current)))
				  .OptionalAttribute(name: "id", (dr, b) => b.SetId(dr.Current))
				  .OptionalAttribute(name: "idlocation", (dr, b) => b.SetIdLocation(AsLocationExpression(dr.Current)))
				  .OptionalAttribute(name: "delay", (dr, b) => b.SetDelay(AsMilliseconds(dr.Current)))
				  .OptionalAttribute(name: "delayexpr", (dr, b) => b.SetDelayExpression(AsValueExpression(dr.Current)))
				  .OptionalAttribute(name: "namelist", (dr, b) => b.SetNameList(AsLocationExpressionList(dr.Current)))
				  .MultipleElements(ScxmlNs, name: "param", (dr, b) => b.AddParameter(dr.ReadParam()))
				  .OptionalElement(ScxmlNs, name: "content", (dr, b) => b.SetContent(dr.ReadContent()));

		private IParam ReadParam() => Populate(_factory.CreateParamBuilder(CreateAncestor()), ParamPolicy).Build();

		private static void ParamBuildPolicy(IPolicyBuilder<IParamBuilder> pb) =>
				pb.RequiredAttribute(name: "name", (dr, b) => b.SetName(dr.Current))
				  .OptionalAttribute(name: "expr", (dr, b) => b.SetExpression(AsValueExpression(dr.Current)))
				  .OptionalAttribute(name: "location", (dr, b) => b.SetLocation(AsLocationExpression(dr.Current)));

		private IContent ReadContent() => Populate(_factory.CreateContentBuilder(CreateAncestor()), ContentPolicy).Build();

		private static void ContentBuildPolicy(IPolicyBuilder<IContentBuilder> pb) =>
				pb.OptionalAttribute(name: "expr", (dr, b) => b.SetExpression(AsValueExpression(dr.Current)))
				  .RawContent((dr, b) => b.SetBody(dr.Current));

		private IOnEntry ReadOnEntry() => Populate(_factory.CreateOnEntryBuilder(CreateAncestor()), OnEntryPolicy).Build();

		private static void OnEntryBuildPolicy(IPolicyBuilder<IOnEntryBuilder> pb) =>
				pb.MultipleElements(ScxmlNs, name: "assign", (dr, b) => b.AddAction(dr.ReadAssign()))
				  .MultipleElements(ScxmlNs, name: "foreach", (dr, b) => b.AddAction(dr.ReadForEach()))
				  .MultipleElements(ScxmlNs, name: "if", (dr, b) => b.AddAction(dr.ReadIf()))
				  .MultipleElements(ScxmlNs, name: "log", (dr, b) => b.AddAction(dr.ReadLog()))
				  .MultipleElements(ScxmlNs, name: "raise", (dr, b) => b.AddAction(dr.ReadRaise()))
				  .MultipleElements(ScxmlNs, name: "send", (dr, b) => b.AddAction(dr.ReadSend()))
				  .MultipleElements(ScxmlNs, name: "cancel", (dr, b) => b.AddAction(dr.ReadCancel()))
				  .MultipleElements(ScxmlNs, name: "script", (dr, b) => b.AddAction(dr.ReadScript()))
				  .UnknownElement((dr, b) => b.AddAction(dr.ReadCustomAction()));

		private IOnExit ReadOnExit() => Populate(_factory.CreateOnExitBuilder(CreateAncestor()), OnExitPolicy).Build();

		private static void OnExitBuildPolicy(IPolicyBuilder<IOnExitBuilder> pb) =>
				pb.MultipleElements(ScxmlNs, name: "assign", (dr, b) => b.AddAction(dr.ReadAssign()))
				  .MultipleElements(ScxmlNs, name: "foreach", (dr, b) => b.AddAction(dr.ReadForEach()))
				  .MultipleElements(ScxmlNs, name: "if", (dr, b) => b.AddAction(dr.ReadIf()))
				  .MultipleElements(ScxmlNs, name: "log", (dr, b) => b.AddAction(dr.ReadLog()))
				  .MultipleElements(ScxmlNs, name: "raise", (dr, b) => b.AddAction(dr.ReadRaise()))
				  .MultipleElements(ScxmlNs, name: "send", (dr, b) => b.AddAction(dr.ReadSend()))
				  .MultipleElements(ScxmlNs, name: "cancel", (dr, b) => b.AddAction(dr.ReadCancel()))
				  .MultipleElements(ScxmlNs, name: "script", (dr, b) => b.AddAction(dr.ReadScript()))
				  .UnknownElement((dr, b) => b.AddAction(dr.ReadCustomAction()));

		private IInvoke ReadInvoke() => Populate(_factory.CreateInvokeBuilder(CreateAncestor()), InvokePolicy).Build();

		private static void InvokeBuildPolicy(IPolicyBuilder<IInvokeBuilder> pb) =>
				pb.OptionalAttribute(name: "type", (dr, b) => b.SetType(AsUri(dr.Current)))
				  .OptionalAttribute(name: "typeexpr", (dr, b) => b.SetTypeExpression(AsValueExpression(dr.Current)))
				  .OptionalAttribute(name: "src", (dr, b) => b.SetSource(AsUri(dr.Current)))
				  .OptionalAttribute(name: "srcexpr", (dr, b) => b.SetSourceExpression(AsValueExpression(dr.Current)))
				  .OptionalAttribute(name: "id", (dr, b) => b.SetId(dr.Current))
				  .OptionalAttribute(name: "idlocation", (dr, b) => b.SetIdLocation(AsLocationExpression(dr.Current)))
				  .OptionalAttribute(name: "namelist", (dr, b) => b.SetNameList(AsLocationExpressionList(dr.Current)))
				  .OptionalAttribute(name: "autoforward", (dr, b) => b.SetAutoForward(XmlConvert.ToBoolean(dr.Current)))
				  .MultipleElements(ScxmlNs, name: "param", (dr, b) => b.AddParam(dr.ReadParam()))
				  .OptionalElement(ScxmlNs, name: "finalize", (dr, b) => b.SetFinalize(dr.ReadFinalize()))
				  .OptionalElement(ScxmlNs, name: "content", (dr, b) => b.SetContent(dr.ReadContent()));

		private IFinalize ReadFinalize() => Populate(_factory.CreateFinalizeBuilder(CreateAncestor()), FinalizePolicy).Build();

		private static void FinalizeBuildPolicy(IPolicyBuilder<IFinalizeBuilder> pb) =>
				pb
						.MultipleElements(ScxmlNs, name: "assign", (dr, b) => b.AddAction(dr.ReadAssign()))
						.MultipleElements(ScxmlNs, name: "foreach", (dr, b) => b.AddAction(dr.ReadForEach()))
						.MultipleElements(ScxmlNs, name: "if", (dr, b) => b.AddAction(dr.ReadIf()))
						.MultipleElements(ScxmlNs, name: "log", (dr, b) => b.AddAction(dr.ReadLog()))
						.MultipleElements(ScxmlNs, name: "cancel", (dr, b) => b.AddAction(dr.ReadCancel()))
						.MultipleElements(ScxmlNs, name: "script", (dr, b) => b.AddAction(dr.ReadScript()))
						.UnknownElement((dr, b) => b.AddAction(dr.ReadCustomAction()));

		private IScript ReadScript() => Populate(_factory.CreateScriptBuilder(CreateAncestor()), ScriptPolicy).Build();

		private static void ScriptBuildPolicy(IPolicyBuilder<IScriptBuilder> pb) =>
				pb.OptionalAttribute(name: "src", (dr, b) => b.SetSource(AsExternalScriptExpression(dr.Current)))
				  .RawContent((dr, b) => b.SetBody(AsScriptExpression(dr.Current)));

		private IDataModel ReadDataModel() => Populate(_factory.CreateDataModelBuilder(CreateAncestor()), DataModelPolicy).Build();

		private static void DataModelBuildPolicy(IPolicyBuilder<IDataModelBuilder> pb) => pb.MultipleElements(ScxmlNs, name: "data", (dr, b) => b.AddData(dr.ReadData()));

		private IData ReadData() => Populate(_factory.CreateDataBuilder(CreateAncestor()), DataPolicy).Build();

		private static void DataBuildPolicy(IPolicyBuilder<IDataBuilder> pb) =>
				pb.RequiredAttribute(name: "id", (dr, b) => b.SetId(dr.Current))
				  .OptionalAttribute(name: "src", (dr, b) => b.SetSource(AsExternalDataExpression(dr.Current)))
				  .OptionalAttribute(name: "expr", (dr, b) => b.SetExpression(AsValueExpression(dr.Current)))
				  .RawContent((dr, b) => b.SetInlineContent(dr.Current));

		private IDoneData ReadDoneData() => Populate(_factory.CreateDoneDataBuilder(CreateAncestor()), DoneDataPolicy).Build();

		private static void DoneDataBuildPolicy(IPolicyBuilder<IDoneDataBuilder> pb) =>
				pb.OptionalElement(ScxmlNs, name: "content", (dr, b) => b.SetContent(dr.ReadContent()))
				  .MultipleElements(ScxmlNs, name: "param", (dr, b) => b.AddParameter(dr.ReadParam()));

		private IForEach ReadForEach() => Populate(_factory.CreateForEachBuilder(CreateAncestor()), ForEachPolicy).Build();

		private static void ForEachBuildPolicy(IPolicyBuilder<IForEachBuilder> pb) =>
				pb.RequiredAttribute(name: "array", (dr, b) => b.SetArray(AsValueExpression(dr.Current)))
				  .RequiredAttribute(name: "item", (dr, b) => b.SetItem(AsLocationExpression(dr.Current)))
				  .OptionalAttribute(name: "index", (dr, b) => b.SetIndex(AsLocationExpression(dr.Current)))
				  .MultipleElements(ScxmlNs, name: "assign", (dr, b) => b.AddAction(dr.ReadAssign()))
				  .MultipleElements(ScxmlNs, name: "foreach", (dr, b) => b.AddAction(dr.ReadForEach()))
				  .MultipleElements(ScxmlNs, name: "if", (dr, b) => b.AddAction(dr.ReadIf()))
				  .MultipleElements(ScxmlNs, name: "log", (dr, b) => b.AddAction(dr.ReadLog()))
				  .MultipleElements(ScxmlNs, name: "raise", (dr, b) => b.AddAction(dr.ReadRaise()))
				  .MultipleElements(ScxmlNs, name: "send", (dr, b) => b.AddAction(dr.ReadSend()))
				  .MultipleElements(ScxmlNs, name: "cancel", (dr, b) => b.AddAction(dr.ReadCancel()))
				  .MultipleElements(ScxmlNs, name: "script", (dr, b) => b.AddAction(dr.ReadScript()))
				  .UnknownElement((dr, b) => b.AddAction(dr.ReadCustomAction()));

		private IIf ReadIf() => Populate(_factory.CreateIfBuilder(CreateAncestor()), IfPolicy).Build();

		private static void IfBuildPolicy(IPolicyBuilder<IIfBuilder> pb) =>
				pb.RequiredAttribute(name: "cond", (dr, b) => b.SetCondition(AsConditionalExpression(dr.Current)))
				  .MultipleElements(ScxmlNs, name: "elseif", (dr, b) => b.AddAction(dr.ReadElseIf()))
				  .MultipleElements(ScxmlNs, name: "else", (dr, b) => b.AddAction(dr.ReadElse()))
				  .MultipleElements(ScxmlNs, name: "assign", (dr, b) => b.AddAction(dr.ReadAssign()))
				  .MultipleElements(ScxmlNs, name: "foreach", (dr, b) => b.AddAction(dr.ReadForEach()))
				  .MultipleElements(ScxmlNs, name: "if", (dr, b) => b.AddAction(dr.ReadIf()))
				  .MultipleElements(ScxmlNs, name: "log", (dr, b) => b.AddAction(dr.ReadLog()))
				  .MultipleElements(ScxmlNs, name: "raise", (dr, b) => b.AddAction(dr.ReadRaise()))
				  .MultipleElements(ScxmlNs, name: "send", (dr, b) => b.AddAction(dr.ReadSend()))
				  .MultipleElements(ScxmlNs, name: "cancel", (dr, b) => b.AddAction(dr.ReadCancel()))
				  .MultipleElements(ScxmlNs, name: "script", (dr, b) => b.AddAction(dr.ReadScript()))
				  .UnknownElement((dr, b) => b.AddAction(dr.ReadCustomAction()));

		private IElse ReadElse() => Populate(_factory.CreateElseBuilder(CreateAncestor()), ElsePolicy).Build();

		private static void ElseBuildPolicy(IPolicyBuilder<IElseBuilder> pb) { }

		private IElseIf ReadElseIf() => Populate(_factory.CreateElseIfBuilder(CreateAncestor()), ElseIfPolicy).Build();

		private static void ElseIfBuildPolicy(IPolicyBuilder<IElseIfBuilder> pb) => pb.RequiredAttribute(name: "cond", (dr, b) => b.SetCondition(AsConditionalExpression(dr.Current)));

		private IRaise ReadRaise() => Populate(_factory.CreateRaiseBuilder(CreateAncestor()), RaisePolicy).Build();

		private static void RaiseBuildPolicy(IPolicyBuilder<IRaiseBuilder> pb) => pb.RequiredAttribute(name: "event", (dr, b) => b.SetEvent(AsEvent(dr.Current)));

		private IAssign ReadAssign() => Populate(_factory.CreateAssignBuilder(CreateAncestor()), AssignPolicy).Build();

		private static void AssignBuildPolicy(IPolicyBuilder<IAssignBuilder> pb) =>
				pb.RequiredAttribute(name: "location", (dr, b) => b.SetLocation(AsLocationExpression(dr.Current)))
				  .OptionalAttribute(name: "expr", (dr, b) => b.SetExpression(AsValueExpression(dr.Current)))
				  .RawContent((dr, b) => b.SetInlineContent(dr.Current));

		private ICancel ReadCancel() => Populate(_factory.CreateCancelBuilder(CreateAncestor()), CancelPolicy).Build();

		private static void CancelBuildPolicy(IPolicyBuilder<ICancelBuilder> pb) =>
				pb.OptionalAttribute(name: "sendid", (dr, b) => b.SetSendId(dr.Current))
				  .OptionalAttribute(name: "sendidexpr", (dr, b) => b.SetSendIdExpression(AsValueExpression(dr.Current)));

		private ICustomAction ReadCustomAction()
		{
			var builder = _factory.CreateCustomActionBuilder(CreateAncestor());
			builder.SetXml(ReadOuterXml());
			return builder.Build();
		}

		private class XmlLineInfo : IXmlLineInfo
		{
			public XmlLineInfo(int lineNumber, int linePosition)
			{
				LineNumber = lineNumber;
				LinePosition = linePosition;
			}

		#region Interface IXmlLineInfo

			public bool HasLineInfo() => true;

			public int LineNumber   { get; }
			public int LinePosition { get; }

		#endregion
		}
	}
}