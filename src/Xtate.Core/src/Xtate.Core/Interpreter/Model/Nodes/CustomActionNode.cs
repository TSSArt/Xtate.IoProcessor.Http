﻿using System.Collections.Immutable;

namespace TSSArt.StateMachine
{
	internal sealed class CustomActionNode : ExecutableEntityNode, ICustomAction, IAncestorProvider
	{
		private readonly CustomAction _entity;

		public CustomActionNode(in DocumentIdRecord documentIdNode, in CustomAction entity) : base(documentIdNode, (ICustomAction?) entity.Ancestor)
		{
			Infrastructure.Assert(entity.Xml != null);

			_entity = entity;
		}

	#region Interface IAncestorProvider

		object? IAncestorProvider.Ancestor => _entity.Ancestor;

	#endregion

	#region Interface ICustomAction

		public string Xml => _entity.Xml!;

		public ImmutableArray<ILocationExpression> Locations => _entity.Locations;

		public ImmutableArray<IValueExpression> Values => _entity.Values;

	#endregion

		protected override void Store(Bucket bucket)
		{
			bucket.Add(Key.TypeInfo, TypeInfo.CustomActionNode);
			bucket.Add(Key.DocumentId, DocumentId);
			bucket.Add(Key.Content, Xml);
			bucket.AddEntityList(Key.LocationList, Locations);
			bucket.AddEntityList(Key.ValueList, Values);
		}
	}
}