﻿using System;
using System.IO;
using System.Reflection;
using System.Security;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TSSArt.StateMachine.EcmaScript;

namespace TSSArt.StateMachine.Test
{
	[TestClass]
	public class DataModelTest
	{
		private ChannelReader<IEvent> _eventChannel;
		private Mock<ILogger>         _logger;
		private InterpreterOptions    _options;

		private IStateMachine GetStateMachine(string scxml)
		{
			using (var textReader = new StringReader(scxml))
			using (var reader = XmlReader.Create(textReader))
			{
				return new ScxmlDirector(reader, new BuilderFactory()).ConstructStateMachine();
			}
		}

		private IStateMachine NoneDataModel(string xml) => GetStateMachine("<scxml xmlns='http://www.w3.org/2005/07/scxml' version='1.0' datamodel='none'>" + xml + "</scxml>");

		private IStateMachine EcmaScriptDataModel(string xml) => GetStateMachine("<scxml xmlns='http://www.w3.org/2005/07/scxml' version='1.0' datamodel='ecmascript'>" + xml + "</scxml>");

		private IStateMachine NoNameOnEntry(string xml) =>
				GetStateMachine("<scxml xmlns='http://www.w3.org/2005/07/scxml' version='1.0' datamodel='ecmascript'><datamodel><data id='my'/></datamodel><state><onentry>" + xml +
								"</onentry></state></scxml>");

		private IStateMachine WithNameOnEntry(string xml) =>
				GetStateMachine("<scxml xmlns='http://www.w3.org/2005/07/scxml' version='1.0' datamodel='ecmascript' name='MyName'><datamodel><data id='my'/></datamodel><state><onentry>" + xml +
								"</onentry></state></scxml>");

		private async Task RunStateMachine(Func<string, IStateMachine> getter, string innerXml)
		{
			var stateMachine = getter(innerXml);

			await StateMachineInterpreter.RunAsync(IdGenerator.NewSessionId(), stateMachine, _eventChannel, _options);
		}

		[TestInitialize]
		public void Init()
		{
			var channel = Channel.CreateUnbounded<IEvent>();
			channel.Writer.Complete();
			_eventChannel = channel.Reader;

			_options = new InterpreterOptions();
			_logger = new Mock<ILogger>();
			_logger.Setup(e => e.Log(It.IsNotNull<string>(), "MyName", It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()))
				   .Callback((string sessionid, string name, string lbl, object prm, CancellationToken _) => Console.WriteLine(lbl + ":" + prm));
			_options.DataModelHandlerFactories.Add(EcmaScriptDataModelHandler.Factory);
			_options.Logger = _logger.Object;
		}

		[TestMethod]
		public async Task LogWriteTest()
		{
			await RunStateMachine(NoNameOnEntry, innerXml: "<log label='output'/>");

			_logger.Verify(l => l.Log(It.IsNotNull<string>(), null, "output", null, default), Times.Once);
		}

		[TestMethod]
		public async Task LogExpressionWriteTest()
		{
			await RunStateMachine(NoNameOnEntry, innerXml: "<log expr=\"'output'\"/>");

			_logger.Verify(l => l.Log(It.IsNotNull<string>(), null, null, "output", default), Times.Once);
		}

		[TestMethod]
		public async Task LogSessionIdWriteTest()
		{
			await RunStateMachine(NoNameOnEntry, innerXml: "<log expr='_sessionid'/>");

			_logger.Verify(l => l.Log(It.IsNotNull<string>(), null, null, It.IsNotNull<string>(), default), Times.Once);
		}

		[TestMethod]
		public async Task LogNameWriteTest()
		{
			await RunStateMachine(WithNameOnEntry, innerXml: "<log expr='_name'/>");

			_logger.Verify(l => l.Log(It.IsNotNull<string>(), "MyName", null, "MyName", default), Times.Once);
		}

		[TestMethod]
		public async Task LogNullNameWriteTest()
		{
			await RunStateMachine(NoNameOnEntry, innerXml: "<log expr='_name'/>");

			_logger.Verify(l => l.Log(It.IsNotNull<string>(), null, null, null, default), Times.Once);
		}

		[TestMethod]
		public async Task LogNonExistedTest()
		{
			await RunStateMachine(NoNameOnEntry, innerXml: "<log expr='_not_existed'/>");

			_logger.Verify(l => l.Error(ErrorType.Execution1, It.IsNotNull<string>(), null, "(#7)", It.IsAny<Exception>(), It.IsAny<CancellationToken>()), Times.Once);
			_logger.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ExecutionBlockWithErrorInsideTest()
		{
			await RunStateMachine(WithNameOnEntry, innerXml: "<log expr='_name'/><log expr='_not_existed'/><log expr='_name'/>");

			_logger.Verify(l => l.Log(It.IsNotNull<string>(), "MyName", null, "MyName", default), Times.Once);
			_logger.Verify(l => l.Error(ErrorType.Execution1, It.IsNotNull<string>(), "MyName", "(#9)", It.IsNotNull<Exception>(), default), Times.Once);
			_logger.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task InterpreterVersionVariableTest()
		{
			await RunStateMachine(WithNameOnEntry, innerXml: "<log expr='_x.interpreter.version'/>");

			var version = typeof(StateMachineInterpreter).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

			_logger.Verify(l => l.Log(It.IsNotNull<string>(), "MyName", null, version, default), Times.Once);
			_logger.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task InterpreterNameVariableTest()
		{
			await RunStateMachine(WithNameOnEntry, innerXml: "<log expr='_x.interpreter.name'/>");

			_logger.Verify(l => l.Log(It.IsNotNull<string>(), "MyName", null, "TSSArt.StateMachine.StateMachineInterpreter", default), Times.Once);
			_logger.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task DataModelNameVariableTest()
		{
			await RunStateMachine(WithNameOnEntry, innerXml: "<log expr='_x.datamodel.name'/>");

			_logger.Verify(l => l.Log(It.IsNotNull<string>(), "MyName", null, "TSSArt.StateMachine.EcmaScript.EcmaScriptDataModelHandler", default), Times.Once);
			_logger.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task DataModelAssemblyVariableTest()
		{
			await RunStateMachine(WithNameOnEntry, innerXml: "<log expr='_x.datamodel.assembly'/>");

			_logger.Verify(l => l.Log(It.IsNotNull<string>(), "MyName", null, "TSSArt.StateMachine.EcmaScript", default), Times.Once);
			_logger.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task DataModelVersionVariableTest()
		{
			await RunStateMachine(WithNameOnEntry, innerXml: "<log expr='_x.datamodel.version'/>");

			_logger.Verify(l => l.Log(It.IsNotNull<string>(), "MyName", null, "1.0.0", default), Times.Once);
			_logger.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task JintVersionVariableTest()
		{
			await RunStateMachine(WithNameOnEntry, innerXml: "<log expr='_x.datamodel.vars.JintVersion'/>");

			_logger.Verify(l => l.Log(It.IsNotNull<string>(), "MyName", null, "2.11.58", default), Times.Once);
			_logger.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ExecutionScriptTest()
		{
			await RunStateMachine(WithNameOnEntry, innerXml: "<script>my='1'+'a';</script><log expr='my'/>");

			_logger.Verify(l => l.Log(It.IsNotNull<string>(), "MyName", null, "1a", default), Times.Once);
			_logger.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task SimpleAssignTest()
		{
			await RunStateMachine(WithNameOnEntry, innerXml: "<assign location='x' expr='\"Hello World\"'/><log expr='x'/>");

			_logger.Verify(l => l.Log(It.IsNotNull<string>(), "MyName", null, "Hello World", default), Times.Once);
			_logger.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ComplexAssignTest()
		{
			await RunStateMachine(WithNameOnEntry, innerXml: "<script>my=[]; my[3]={};</script><assign location='my[3].yy' expr=\"'Hello World'\"/><log expr='my[3].yy'/>");

			_logger.Verify(l => l.Log(It.IsNotNull<string>(), "MyName", null, "Hello World", default), Times.Once);
			_logger.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task UserAssignTest()
		{
			await RunStateMachine(WithNameOnEntry, innerXml: "<assign location='_name1' expr=\"'Hello World'\"/><log expr='_name1'/>");

			_logger.Verify(l => l.Log(It.IsNotNull<string>(), "MyName", null, "Hello World", default), Times.Once);
			_logger.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task SystemAssignTest()
		{
			await RunStateMachine(WithNameOnEntry, innerXml: "<assign location='_name' expr=\"'Hello World'\"/>");

			_logger.Verify(l => l.Error(ErrorType.Execution1, It.IsNotNull<string>(), "MyName", "(#7)", It.IsNotNull<SecurityException>(), default), Times.Once);
			_logger.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task IfTrueTest()
		{
			await RunStateMachine(WithNameOnEntry, innerXml: "<if cond='1==1'><log expr=\"'Hello World'\"/></if>");

			_logger.Verify(l => l.Log(It.IsNotNull<string>(), "MyName", null, "Hello World", default), Times.Once);
			_logger.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task IfFalseTest()
		{
			await RunStateMachine(WithNameOnEntry, innerXml: "<if cond='1==0'><log expr=\"'Hello World'\"/></if>");

			_logger.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task IfElseTrueTest()
		{
			await RunStateMachine(WithNameOnEntry, innerXml: "<if cond='true'><log expr=\"'Hello World'\"/><else/><log expr=\"'Bye World'\"/></if>");

			_logger.Verify(l => l.Log(It.IsNotNull<string>(), "MyName", null, "Hello World", default), Times.Once);
			_logger.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task IfElseFalseTest()
		{
			await RunStateMachine(WithNameOnEntry, innerXml: "<if cond='false'><log expr=\"'Hello World'\"/><else/><log expr=\"'Bye World'\"/></if>");

			_logger.Verify(l => l.Log(It.IsNotNull<string>(), "MyName", null, "Bye World", default), Times.Once);
			_logger.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task SwitchTest()
		{
			await RunStateMachine(WithNameOnEntry, innerXml: "<if cond='false'><log expr=\"'Hello World'\"/><elseif cond='true'/><log expr=\"'Maybe World'\"/><else/><log expr=\"'Bye World'\"/></if>");

			_logger.Verify(l => l.Log(It.IsNotNull<string>(), "MyName", null, "Maybe World", default), Times.Once);
			_logger.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ForeachNoIndexTest()
		{
			await RunStateMachine(WithNameOnEntry, "<script>my=[]; my[0]='aaa'; my[1]='bbb'</script><foreach array='my' item='itm'>"
												   + "<log expr=\"itm\"/></foreach><log expr='typeof(itm)'/>");

			_logger.Verify(l => l.Log(It.IsNotNull<string>(), "MyName", null, "aaa", default), Times.Once);
			_logger.Verify(l => l.Log(It.IsNotNull<string>(), "MyName", null, "bbb", default), Times.Once);
			_logger.Verify(l => l.Log(It.IsNotNull<string>(), "MyName", null, "undefined", default), Times.Once);
			_logger.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ForeachWithIndexTest()
		{
			await RunStateMachine(WithNameOnEntry, "<script>my=[]; my[0]='aaa'; my[1]='bbb'</script><foreach array='my' item='itm' index='idx'>"
												   + "<log expr=\"idx + '-' + itm\"/></foreach><log expr='typeof(itm)'/><log expr='typeof(idx)'/>");

			_logger.Verify(l => l.Log(It.IsNotNull<string>(), "MyName", null, "0-aaa", default), Times.Once);
			_logger.Verify(l => l.Log(It.IsNotNull<string>(), "MyName", null, "1-bbb", default), Times.Once);
			_logger.Verify(l => l.Log(It.IsNotNull<string>(), "MyName", null, "undefined", default), Times.Exactly(2));
			_logger.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task NoneDataModelTransitionWithConditionTrueTest()
		{
			await RunStateMachine(NoneDataModel, innerXml: "<state id='s1'><transition cond='In(s1)' target='s2'/></state><state id='s2'><onentry><log label='Hello'/></onentry></state>");

			_logger.Verify(l => l.Log(It.IsNotNull<string>(), null, "Hello", null, default), Times.Once);
			_logger.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task NoneDataModelTransitionWithConditionFalseTest()
		{
			await RunStateMachine(NoneDataModel, innerXml: "<state id='s1'><transition cond='In(s2)' target='s2'/></state><state id='s2'><onentry><log label='Hello'/></onentry></state>");

			_logger.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task EcmaScriptDataModelTransitionWithConditionTrueTest()
		{
			await RunStateMachine(EcmaScriptDataModel, innerXml: "<state id='s1'><transition cond=\"In('s1')\" target='s2'/></state><state id='s2'><onentry><log label='Hello'/></onentry></state>");

			_logger.Verify(l => l.Log(It.IsNotNull<string>(), null, "Hello", null, default), Times.Once);
			_logger.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task EcmaScriptDataModelTransitionWithConditionFalseTest()
		{
			await RunStateMachine(EcmaScriptDataModel, innerXml: "<state id='s1'><transition cond=\"In('s2')\" target='s2'/></state><state id='s2'><onentry><log label='Hello'/></onentry></state>");

			_logger.VerifyNoOtherCalls();
		}
	}
}