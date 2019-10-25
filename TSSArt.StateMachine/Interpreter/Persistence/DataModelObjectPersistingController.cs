﻿using System;

namespace TSSArt.StateMachine
{
	public class DataModelObjectPersistingController : DataModelPersistingController
	{
		private readonly Bucket                    _bucket;
		private readonly DataModelObject           _dataModelObject;
		private readonly DataModelReferenceTracker _referenceTracker;
		private          int                       _record;

		public DataModelObjectPersistingController(Bucket bucket, DataModelReferenceTracker referenceTracker, DataModelObject dataModelObject)
		{
			_bucket = bucket;
			_referenceTracker = referenceTracker ?? throw new ArgumentNullException(nameof(referenceTracker));
			_dataModelObject = dataModelObject ?? throw new ArgumentNullException(nameof(dataModelObject));

			var shrink = dataModelObject.Properties.Count > 0;
			while (true)
			{
				var recordBucket = bucket.Nested(_record);

				if (!recordBucket.TryGet(Key.Operation, out Key operation))
				{
					break;
				}

				switch (operation)
				{
					case Key.Set when recordBucket.TryGet(Key.Property, out string property):
					{
						var dataModelValue = recordBucket.GetDataModelValue(referenceTracker, dataModelObject[property]);
						dataModelObject.SetInternal(property, dataModelValue);
						referenceTracker.AddReference(dataModelValue);
						break;
					}

					case Key.Remove when recordBucket.TryGet(Key.Property, out string property):
					{
						shrink = true;
						referenceTracker.RemoveReference(dataModelObject[property]);
						dataModelObject.RemoveInternal(property);
						break;
					}

					default: throw new ArgumentOutOfRangeException();
				}

				_record ++;
			}

			if (shrink)
			{
				bucket.RemoveSubtree(Bucket.RootKey);
				if (dataModelObject.IsReadOnly)
				{
					bucket.Add(Key.ReadOnly, value: true);
				}

				_record = 0;
				foreach (var property in dataModelObject.Properties)
				{
					var recordBucket = bucket.Nested(_record ++);
					recordBucket.Add(Key.Operation, Key.Set);
					recordBucket.Add(Key.Property, property);
					recordBucket.SetDataModelValue(referenceTracker, dataModelObject[property]);
				}
			}

			dataModelObject.Changed += OnChanged;
		}

		private void OnChanged(DataModelObject.ChangedAction action, string property, DataModelValue value)
		{
			switch (action)
			{
				case DataModelObject.ChangedAction.Set:
				{
					var recordBucket = _bucket.Nested(_record ++);
					recordBucket.Add(Key.Operation, Key.Set);
					recordBucket.Add(Key.Property, property);
					_referenceTracker.AddReference(value);
					recordBucket.SetDataModelValue(_referenceTracker, value);
					break;
				}
				case DataModelObject.ChangedAction.Remove:
				{
					_referenceTracker.RemoveReference(value);
					if (_dataModelObject.Properties.Count > 1)
					{
						var recordBucket = _bucket.Nested(_record ++);
						recordBucket.Add(Key.Operation, Key.Remove);
						recordBucket.Add(Key.Property, property);
					}
					else
					{
						_record = 0;
						_bucket.RemoveSubtree(Bucket.RootKey);
					}

					break;
				}
				default: throw new ArgumentOutOfRangeException(nameof(action), action, message: null);
			}
		}

		public override void Dispose() => _dataModelObject.Changed -= OnChanged;
	}
}