﻿#region Copyright © 2019-2020 Sergii Artemenko

// This file is part of the Xtate project. <https://xtate.net/>
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

#endregion

// @formatter:off

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(category:"Design", checkId:"CA1000:Do not declare static members on generic types", Justification = "<Pending>", Scope = "member", Target = "~P:Xtate.Core.TypeInfo`1.Instance")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1001:Types that own disposable fields should be disposable", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.StateMachineController")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1001:Types that own disposable fields should be disposable", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.StateMachineHostContext")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1001:Types that own disposable fields should be disposable", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Service.ServiceBase")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1010:Generic interface should also be implemented", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.DeferredFinalizer")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1030:Use events where appropriate", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Builder.FinalFluentBuilder`1")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1030:Use events where appropriate", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Builder.IFinalBuilder")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1030:Use events where appropriate", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Builder.IParallelBuilder")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1030:Use events where appropriate", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Builder.IStateBuilder")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1030:Use events where appropriate", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Builder.ParallelFluentBuilder`1")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1030:Use events where appropriate", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Builder.StateFluentBuilder`1")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1030:Use events where appropriate", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Builder.TransitionFluentBuilder`1")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Core.CustomActionDispatcher.SetupExecutor(System.Collections.Immutable.ImmutableArray{Xtate.CustomAction.ICustomActionFactory},System.Threading.CancellationToken)~System.Threading.Tasks.ValueTask")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Core.IoBoundTaskScheduler.WorkerThread")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Core.StateMachineController.DelayedFire(Xtate.Core.StateMachineController.ScheduledEvent,System.Int32)~System.Threading.Tasks.ValueTask")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Core.StateMachineHostContext.DestroyStateMachine(Xtate.SessionId,System.Threading.CancellationToken)~System.Threading.Tasks.ValueTask")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Core.StateMachineHostContext.WaitAllAsync(System.Threading.CancellationToken)~System.Threading.Tasks.ValueTask")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Core.StateMachineInterpreter.Error(System.Object,System.Exception,System.Boolean)~System.Threading.Tasks.ValueTask")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.IoProcessor.NamedIoProcessor.StartListener~System.Threading.Tasks.ValueTask")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.IoProcessor.NamedIoProcessorFactory.GetHostName~System.String")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Persistence.StreamStorage.DisposeAsync~System.Threading.Tasks.ValueTask")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Scxml.XmlDirector`1.Populate``1(``0,Xtate.Scxml.XmlDirector`1.Policy{``0})~System.Threading.Tasks.ValueTask{``0}")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Scxml.XmlDirector`1.PopulateAttributes``1(``0,Xtate.Scxml.XmlDirector`1.Policy{``0},Xtate.Scxml.XmlDirector`1.Policy{``0}.ValidationContext)")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Scxml.XmlDirector`1.PopulateElements``1(``0,Xtate.Scxml.XmlDirector`1.Policy{``0},Xtate.Scxml.XmlDirector`1.Policy{``0}.ValidationContext)")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Service.ServiceBase.DisposeAsync~System.Threading.Tasks.ValueTask")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Service.ServiceBase.Start(System.Uri,Xtate.InvokeData,Xtate.Service.IServiceCommunication)")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.StateMachineHost.Xtate#Core#IStateMachineHost#StartInvoke(Xtate.SessionId,Xtate.InvokeData,Xtate.Core.SecurityContext,System.Threading.CancellationToken)~System.Threading.Tasks.ValueTask")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1032:Implement standard exception constructors", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.StateMachineValidationException")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1033:Interface methods should be callable by child types", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Core.LazyId.Xtate#Core#IObject#ToObject~System.Object")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1033:Interface methods should be callable by child types", Justification = "<Pending>", Scope = "member", Target = "~P:Xtate.DataModel.DefaultDoneDataEvaluator.Xtate#Core#IAncestorProvider#Ancestor")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1033:Interface methods should be callable by child types", Justification = "<Pending>", Scope = "member", Target = "~P:Xtate.DataModel.DefaultExternalDataExpressionEvaluator.Xtate#Core#IAncestorProvider#Ancestor")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1033:Interface methods should be callable by child types", Justification = "<Pending>", Scope = "member", Target = "~P:Xtate.DataModel.DefaultInlineContentEvaluator.Xtate#Core#IAncestorProvider#Ancestor")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1033:Interface methods should be callable by child types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.DataModel.DataModelHandlerBase")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1033:Interface methods should be callable by child types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.DataModel.DefaultAssignEvaluator")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1033:Interface methods should be callable by child types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.DataModel.DefaultCancelEvaluator")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1033:Interface methods should be callable by child types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.DataModel.DefaultContentBodyEvaluator")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1033:Interface methods should be callable by child types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.DataModel.DefaultCustomActionEvaluator")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1033:Interface methods should be callable by child types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.DataModel.DefaultForEachEvaluator")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1033:Interface methods should be callable by child types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.DataModel.DefaultIfEvaluator")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1033:Interface methods should be callable by child types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.DataModel.DefaultInvokeEvaluator")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1033:Interface methods should be callable by child types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.DataModel.DefaultLogEvaluator")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1033:Interface methods should be callable by child types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.DataModel.DefaultRaiseEvaluator")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1033:Interface methods should be callable by child types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.DataModel.DefaultScriptEvaluator")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1033:Interface methods should be callable by child types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.DataModel.DefaultSendEvaluator")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1033:Interface methods should be callable by child types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.IoProcessor.IoProcessorBase")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1033:Interface methods should be callable by child types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Service.ServiceBase")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1034:Nested types should not be visible", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.CancellationTokenRegistrationExtensions.ConfiguredAwaitable")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1034:Nested types should not be visible", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.StreamExtensions.ConfiguredAwaitable")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1034:Nested types should not be visible", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Scxml.XmlDirector`1.Policy`1.ValidationContext")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1040:Avoid empty interfaces", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.IEntity")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1040:Avoid empty interfaces", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.IExecutableEntity")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1040:Avoid empty interfaces", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.IStateEntity")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1040:Avoid empty interfaces", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.DataModel.IValueEvaluator")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1040:Avoid empty interfaces", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.IElse")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1040:Avoid empty interfaces", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.IIdentifier")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1062:Validate arguments of public methods", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Core.ErrorProcessorExtensions.AddError(Xtate.Core.IErrorProcessor,System.Type,System.Object,System.String,System.Exception)")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1062:Validate arguments of public methods", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Scxml.ScxmlSerializer.Build(Xtate.IStateMachine@,Xtate.Core.StateMachineEntity@)")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1067:Override Object.Equals(object) when implementing IEquatable<T>", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.InvokeId")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1067:Override Object.Equals(object) when implementing IEquatable<T>", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.SendId")]
[assembly: SuppressMessage(category:"Design", checkId:"CA1067:Override Object.Equals(object) when implementing IEquatable<T>", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.SessionId")]
[assembly: SuppressMessage(category:"Globalization", checkId:"CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Core.IdGenerator.NewGuidWithHash(System.Int32)~System.String")]
[assembly: SuppressMessage(category:"Globalization", checkId:"CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Core.IdGenerator.NewInvokeId(System.String,System.Int32)~System.String")]
[assembly: SuppressMessage(category:"Globalization", checkId:"CA1307:Specify StringComparison", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Core.FullUriComparer.GetHashCode(System.Uri)~System.Int32")]
[assembly: SuppressMessage(category:"Globalization", checkId:"CA1307:Specify StringComparison", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Core.LazyId.GetHashCode~System.Int32")]
[assembly: SuppressMessage(category:"Globalization", checkId:"CA1307:Specify StringComparison", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Core.StateMachineHostContext.GetService(Xtate.SessionId,System.String)~Xtate.Service.IService")]
[assembly: SuppressMessage(category:"Globalization", checkId:"CA1307:Specify StringComparison", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.DataModelList.Entry.GetHashCode~System.Int32")]
[assembly: SuppressMessage(category:"Globalization", checkId:"CA1307:Specify StringComparison", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.DataModelList.KeyValue.GetHashCode~System.Int32")]
[assembly: SuppressMessage(category:"Globalization", checkId:"CA1307:Specify StringComparison", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.DataModelValue.TryFromAnonymousType(System.Object,System.Collections.Generic.Dictionary{System.Object,Xtate.DataModelList}@,Xtate.DataModelValue@)~System.Boolean")]
[assembly: SuppressMessage(category:"Globalization", checkId:"CA1307:Specify StringComparison", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.InvokeId.InvokeUniqueIdEqualityComparer.GetHashCode(Xtate.InvokeId)~System.Int32")]
[assembly: SuppressMessage(category:"Globalization", checkId:"CA1307:Specify StringComparison", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.IoProcessor.NamedIoProcessor.ExtractSessionId(System.Uri)~Xtate.SessionId")]
[assembly: SuppressMessage(category:"Globalization", checkId:"CA1307:Specify StringComparison", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Scxml.PrefixNamespace.GetHashCode~System.Int32")]
[assembly: SuppressMessage(category:"Globalization", checkId:"CA1307:Specify StringComparison", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Scxml.ScxmlDirector.AsEventDescriptorList(System.String)~System.Collections.Immutable.ImmutableArray{Xtate.IEventDescriptor}")]
[assembly: SuppressMessage(category:"Globalization", checkId:"CA1307:Specify StringComparison", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Scxml.ScxmlDirector.AsIdentifierList(System.String)~System.Collections.Immutable.ImmutableArray{Xtate.IIdentifier}")]
[assembly: SuppressMessage(category:"Globalization", checkId:"CA1307:Specify StringComparison", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Scxml.ScxmlDirector.AsLocationExpressionList(System.String)~System.Collections.Immutable.ImmutableArray{Xtate.ILocationExpression}")]
[assembly: SuppressMessage(category:"Globalization", checkId:"CA1307:Specify StringComparison", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.SessionId.#ctor(System.String)")]
[assembly: SuppressMessage(category:"Globalization", checkId:"CA1308:Normalize strings to uppercase", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Scxml.ScxmlDirector.AsEnum``1(System.String)~``0")]
[assembly: SuppressMessage(category:"Microsoft.Naming", checkId:"CA1724:TypeNamesShouldNotMatchNamespaces", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Scxml.XmlDirector`1.Policy`1")]
[assembly: SuppressMessage(category:"Naming", checkId:"CA1710:Identifiers should have correct suffix", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.DeferredFinalizer")]
[assembly: SuppressMessage(category:"Naming", checkId:"CA1710:Identifiers should have correct suffix", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.DataModelList")]
[assembly: SuppressMessage(category:"Naming", checkId:"CA1720:Identifier contains type name", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.CustomAction.ExpectedValueType")]
[assembly: SuppressMessage(category:"Naming", checkId:"CA1720:Identifier contains type name", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.DataModelValueType")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.AssignEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.CancelEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.CancellationTokenRegistrationExtensions.ConfiguredAwaitable")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.ConditionExpression")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.ContentBody")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.ContentEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.CustomActionEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.DataEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.DataModelEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.DoneDataEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.ElseEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.ElseIfEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.EventEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.ExternalDataExpression")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.ExternalScriptExpression")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.FinalEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.FinalizeEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.ForEachEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.HistoryEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.IfEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.InitialEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.InlineContent")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.InterpreterOptions")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.InvokeEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.LocationExpression")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.LogEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.OnEntryEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.OnExitEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.ParallelEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.ParamEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.RaiseEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.ScriptEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.ScriptExpression")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.SendEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.StateEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.StateMachineEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.StateMachineOptions")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.StateMachineOrigin")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.StateMachineVisitor.TrackList`1")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.StreamExtensions.ConfiguredAwaitable")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.TransitionEntity")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.Core.ValueExpression")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1815:Override equals and operator equals on value types", Justification = "<Pending>", Scope = "type", Target = "~T:Xtate.InvokeData")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1822:Mark members as static", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.DataModel.XPath.XPathResolver.GetName(Xtate.DataModel.XPath.XPathCompiledExpression)~System.String")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1835:Prefer the 'Memory'-based overloads for 'ReadAsync' and 'WriteAsync'", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Core.InjectedCancellationStream.ReadAsyncInternal(System.Byte[],System.Int32,System.Int32,System.Threading.CancellationToken)~System.Threading.Tasks.Task{System.Int32}")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1835:Prefer the 'Memory'-based overloads for 'ReadAsync' and 'WriteAsync'", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Core.InjectedCancellationStream.WriteAsyncInternal(System.Byte[],System.Int32,System.Int32,System.Threading.CancellationToken)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1835:Prefer the 'Memory'-based overloads for 'ReadAsync' and 'WriteAsync'", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Core.StreamExtensions.ReadToEndAsync(System.IO.Stream,System.Threading.CancellationToken)~System.Threading.Tasks.ValueTask{System.Byte[]}")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1835:Prefer the 'Memory'-based overloads for 'ReadAsync' and 'WriteAsync'", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Persistence.StreamStorage.CheckPoint(System.Int32,System.Threading.CancellationToken)~System.Threading.Tasks.ValueTask")]
[assembly: SuppressMessage(category:"Performance", checkId:"CA1835:Prefer the 'Memory'-based overloads for 'ReadAsync' and 'WriteAsync'", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Persistence.StreamStorage.ReadStream(System.IO.Stream,System.Int32,System.Boolean,System.Threading.CancellationToken)~System.Threading.Tasks.ValueTask{Xtate.Persistence.InMemoryStorage}")]
[assembly: SuppressMessage(category:"Reliability", checkId:"CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Core.StreamExtensions.ReadToEndAsync(System.IO.Stream,System.Threading.CancellationToken)~System.Threading.Tasks.ValueTask{System.Byte[]}")]
[assembly: SuppressMessage(category:"Reliability", checkId:"CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.IoProcessor.NamedIoProcessor.StartListener~System.Threading.Tasks.ValueTask")]
[assembly: SuppressMessage(category:"Reliability", checkId:"CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.StateMachineHost.Xtate#Core#IStateMachineHost#IsInvokeActive(Xtate.SessionId,Xtate.InvokeId)~System.Boolean")]
[assembly: SuppressMessage(category:"Reliability", checkId:"CA2012:Use ValueTasks correctly", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Core.LocalCache`2.TryGetValue(`0,`1@)~System.Boolean")]
[assembly: SuppressMessage(category:"Reliability", checkId:"CA2016:Forward the 'CancellationToken' parameter to methods that take one", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Service.ServiceBase.Xtate#Service#IService#Destroy(System.Threading.CancellationToken)~System.Threading.Tasks.ValueTask")]
[assembly: SuppressMessage(category:"Style", checkId:"IDE0016:Use 'throw' expression", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.DataModelList.CanSet(System.String,System.Boolean)~System.Boolean")]
[assembly: SuppressMessage(category:"Style", checkId:"IDE0016:Use 'throw' expression", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.DataModelList.Set(System.String,System.Boolean,Xtate.DataModelValue@,Xtate.DataModelList)")]
[assembly: SuppressMessage(category:"Style", checkId:"IDE0016:Use 'throw' expression", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.DataModelList.SetInternal(System.String,System.Boolean,Xtate.DataModelValue@,Xtate.Core.DataModelAccess,Xtate.DataModelList,System.Boolean)~System.Boolean")]
[assembly: SuppressMessage(category:"Style", checkId:"IDE0057:Use range operator", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.EventName.SetParts(System.Span{Xtate.IIdentifier},System.String)")]
[assembly: SuppressMessage(category:"Style", checkId:"IDE0057:Use range operator", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Persistence.Bucket.StringKeyConverter`1.Write(System.String,System.Span{System.Byte})")]
[assembly: SuppressMessage(category:"Style", checkId:"IDE0066:Convert switch statement to expression", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.Persistence.Encode.Decode(System.ReadOnlySpan{System.Byte})~System.Int32")]
[assembly: SuppressMessage(category:"Style", checkId:"IDE1006:Naming Styles", Justification = "<Pending>", Scope = "member", Target = "~P:Xtate.DataModelList.DebugIndexKeyValue.__ItemInfo__")]
[assembly: SuppressMessage(category:"Style", checkId:"IDE1006:Naming Styles", Justification = "<Pending>", Scope = "member", Target = "~P:Xtate.DataModelList.DebugView.__ListInfo__")]
[assembly: SuppressMessage(category:"Usage", checkId:"CA2213:Disposable fields should be disposed", Justification = "<Pending>", Scope = "member", Target = "~F:Xtate.StateMachineHost._context")]
[assembly: SuppressMessage(category:"Usage", checkId:"CA2225:Operator overloads have named alternates", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.DataModelDateTime.op_Explicit(Xtate.DataModelDateTime)~System.DateTime")]
[assembly: SuppressMessage(category:"Usage", checkId:"CA2225:Operator overloads have named alternates", Justification = "<Pending>", Scope = "member", Target = "~M:Xtate.DataModelDateTime.op_Explicit(Xtate.DataModelDateTime)~System.DateTimeOffset")]
[assembly: SuppressMessage(category:"Usage", checkId:"CA2227:Collection properties should be read only", Justification = "<Pending>", Scope = "member", Target = "~P:Xtate.Core.InterpreterOptions.Configuration")]
[assembly: SuppressMessage(category:"Usage", checkId:"CA2227:Collection properties should be read only", Justification = "<Pending>", Scope = "member", Target = "~P:Xtate.Core.InterpreterOptions.Host")]