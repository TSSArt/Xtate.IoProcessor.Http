﻿using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TSSArt.StateMachine.EcmaScript;

namespace TSSArt.StateMachine.Test
{
	[TestClass]
	public class ScxmlDirectorTest
	{
		private IStateMachine _stateMachine;

		[TestInitialize]
		public void Initialize()
		{
			var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TSSArt.StateMachine.Test.Resources.Main.xml");
			var xmlReader = XmlReader.Create(stream);

			var director = new ScxmlDirector(xmlReader, new BuilderFactory());

			_stateMachine = director.ConstructStateMachine();
		}

		[TestMethod]
		public async Task ReadScxmlTest()
		{
			var channel = Channel.CreateUnbounded<IEvent>();

			await channel.Writer.WriteAsync(new EventObject(EventType.External, "Event1"));
			await channel.Writer.WriteAsync(new EventObject(EventType.External, "Test1.done"));
			await channel.Writer.WriteAsync(new EventObject(EventType.External, "Event2"));
			await channel.Writer.WriteAsync(new EventObject(EventType.External, "done.state.Test2"));
			await channel.Writer.WriteAsync(new EventObject(EventType.External, "Timer"));
			channel.Writer.Complete(new ArgumentException("333"));

			var options = new InterpreterOptions();
			options.DataModelHandlerFactories.Add(EcmaScriptDataModelHandler.Factory);

			await StateMachineInterpreter.RunAsync(IdGenerator.NewSessionId(), _stateMachine, channel.Reader, options);
		}

		[TestMethod]
		public async Task ReadAllScxmlTest()
		{
			var channel = Channel.CreateUnbounded<IEvent>();

			channel.Writer.Complete(new ArgumentException("333"));

			var options = new InterpreterOptions();
			options.DataModelHandlerFactories.Add(EcmaScriptDataModelHandler.Factory);

			await StateMachineInterpreter.RunAsync(IdGenerator.NewSessionId(), _stateMachine, channel.Reader, options);
		}

		[TestMethod]
		public void ScxmlSerializerTest()
		{
			var interpreterModelBuilder = new InterpreterModelBuilder();
			var dataModelHandler = EcmaScriptDataModelHandler.Factory.CreateHandler(interpreterModelBuilder);
			var interpreterModel = interpreterModelBuilder.Build(_stateMachine, dataModelHandler);
			var serializer = new ScxmlSerializer();
			var text = new StringWriter();
			using (var xmlWriter = XmlWriter.Create(text, new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true }))
			{
				serializer.Serialize(interpreterModel.Root, xmlWriter);
			}

			Console.WriteLine(text);
		}
	}
}