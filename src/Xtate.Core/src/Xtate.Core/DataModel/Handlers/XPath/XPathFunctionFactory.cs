﻿using System.Collections.Generic;
using Xtate.DataModel.XPath.Functions;

namespace Xtate.DataModel.XPath
{
	internal class XPathFunctionFactory
	{
		public static readonly XPathFunctionFactory Instance = new XPathFunctionFactory();

		private readonly Dictionary<(string Namespace, string Name), XPathFunctionDescriptorBase> _functionDescriptors = new Dictionary<(string Namespace, string Name), XPathFunctionDescriptorBase>();

		private XPathFunctionFactory()
		{
			RegisterFunction<In>();
		}

		private void RegisterFunction<T>() where T : XPathFunctionDescriptorBase, new()
		{
			var descriptor = new T();

			_functionDescriptors.Add((descriptor.Namespace, descriptor.Name), descriptor);
		}

		public XPathFunctionDescriptorBase ResolveFunction(string ns, string name)
		{
			if (_functionDescriptors.TryGetValue((ns, name), out var descriptor))
			{
				return descriptor;
			}

			throw new XPathDataModelException(Res.Format(Resources.Exception_Unknown_XPath_function, ns, name));
		}
	}
}