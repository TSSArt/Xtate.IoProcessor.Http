﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Xtate.Core.Test
{
	public class Record
	{
		private DataModelValue     _value;
		public  object?            ConstructorArg;
		public  string             Line;
		public  DataModelValue     OriginalValue;
		public  DataModelValueType Type;

		public Record([CallerLineNumber] int lineNumber = 0) => Line = $"#{lineNumber}";

		public DataModelValue Value
		{
			get => _value;
			set
			{
				_value = value;
				OriginalValue = value;
			}
		}

		public Record WrapToLazy()
		{
			var lazyMock = new Mock<ILazyValue>();
			lazyMock.Setup(l => l.Value).Returns(Value);

			return new Record
				   {
						   Line = $"LZ({Line})",
						   ConstructorArg = ConstructorArg,
						   Type = Type,
						   Value = new DataModelValue(lazyMock.Object),
						   OriginalValue = OriginalValue
				   };
		}
	}

	[TestClass]
	public class DataModelValueTest
	{
		private DataModelArray   _emptyDataModelArray          = null!;
		private DataModelObject  _emptyDataModelObject         = null!;
		private Mock<ILazyValue> _lazyValueArrayMock           = null!;
		private Mock<ILazyValue> _lazyValueNullMock            = null!;
		private Mock<ILazyValue> _lazyValueObjectMock          = null!;
		private Mock<ILazyValue> _lazyValueOfLazyValueNullMock = null!;

		private static IEnumerable<Record> SimpleRecords
		{
			get
			{
				yield return new Record
							 {
									 Type = DataModelValueType.Undefined,
									 Value = new DataModelValue()
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.Null,
									 Value = new DataModelValue((DataModelObject?) null)
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.Null,
									 Value = new DataModelValue((DataModelArray?) null)
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.Null,
									 Value = new DataModelValue((string?) null)
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.Null,
									 Value = new DataModelValue((ILazyValue?) null)
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.Boolean,
									 Value = new DataModelValue(false),
									 ConstructorArg = false
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.Boolean,
									 Value = new DataModelValue(true),
									 ConstructorArg = true
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.Number,
									 Value = new DataModelValue(0.0),
									 ConstructorArg = 0.0
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.Number,
									 Value = new DataModelValue(1.0),
									 ConstructorArg = 1.0
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.Number,
									 Value = new DataModelValue(double.MinValue),
									 ConstructorArg = double.MinValue
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.Number,
									 Value = new DataModelValue(double.MaxValue),
									 ConstructorArg = double.MaxValue
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.Number,
									 Value = new DataModelValue(double.NegativeInfinity),
									 ConstructorArg = double.NegativeInfinity
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.Number,
									 Value = new DataModelValue(double.PositiveInfinity),
									 ConstructorArg = double.PositiveInfinity
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.Number,
									 Value = new DataModelValue(double.NaN),
									 ConstructorArg = double.NaN
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.Number,
									 Value = new DataModelValue(double.Epsilon),
									 ConstructorArg = double.Epsilon
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.Number,
									 Value = new DataModelValue(9007199254740991), // JavaScript Number.MAX_SAFE_INTEGER
									 ConstructorArg = (double) 9007199254740991
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.Number,
									 Value = new DataModelValue(-9007199254740991), // JavaScript Number.MIN_SAFE_INTEGER
									 ConstructorArg = (double) -9007199254740991
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.Number,
									 Value = new DataModelValue(int.MinValue),
									 ConstructorArg = (double) int.MinValue
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.Number,
									 Value = new DataModelValue(int.MaxValue),
									 ConstructorArg = (double) int.MaxValue
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.String,
									 Value = new DataModelValue(""),
									 ConstructorArg = ""
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.String,
									 Value = new DataModelValue(" "),
									 ConstructorArg = " "
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.String,
									 Value = new DataModelValue("Text"),
									 ConstructorArg = "Text"
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.DateTime,
									 Value = new DataModelValue(new DateTime(ticks: 1, DateTimeKind.Utc)),
									 ConstructorArg = new DateTime(ticks: 1, DateTimeKind.Utc)
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.DateTime,
									 Value = new DataModelValue(new DateTime(TimeSpan.TicksPerDay, DateTimeKind.Unspecified)),
									 ConstructorArg = new DateTime(TimeSpan.TicksPerDay, DateTimeKind.Unspecified)
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.DateTime,
									 Value = new DataModelValue(new DateTime(year: 2000, month: 1, day: 1, hour: 0, minute: 0, second: 0, DateTimeKind.Utc)),
									 ConstructorArg = new DateTime(year: 2000, month: 1, day: 1, hour: 0, minute: 0, second: 0, DateTimeKind.Utc)
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.DateTime,
									 Value = new DataModelValue(new DateTime(year: 1, month: 1, day: 1, hour: 0, minute: 0, second: 0, DateTimeKind.Utc)),
									 ConstructorArg = new DateTime(year: 1, month: 1, day: 1, hour: 0, minute: 0, second: 0, DateTimeKind.Utc)
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.DateTime,
									 Value = new DataModelValue(new DateTime(year: 9999, month: 12, day: 31, hour: 23, minute: 59, second: 59, DateTimeKind.Utc)),
									 ConstructorArg = new DateTime(year: 9999, month: 12, day: 31, hour: 23, minute: 59, second: 59, DateTimeKind.Utc)
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.DateTime,
									 Value = new DataModelValue(new DateTime(year: 2000, month: 1, day: 1, hour: 0, minute: 0, second: 0, DateTimeKind.Unspecified)),
									 ConstructorArg = new DateTime(year: 2000, month: 1, day: 1, hour: 0, minute: 0, second: 0, DateTimeKind.Unspecified)
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.DateTime, // DateTimeOffset
									 Value = new DataModelValue(new DateTime(TimeSpan.TicksPerDay, DateTimeKind.Local)),
									 ConstructorArg = new DateTime(TimeSpan.TicksPerDay, DateTimeKind.Local)
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.DateTime, // DateTimeOffset
									 Value = new DataModelValue(new DateTimeOffset()),
									 ConstructorArg = new DateTimeOffset()
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.DateTime, // DateTimeOffset
									 Value = new DataModelValue(new DateTimeOffset(year: 2000, month: 1, day: 1, hour: 0, minute: 0, second: 0, TimeSpan.FromHours(-14))),
									 ConstructorArg = new DateTimeOffset(year: 2000, month: 1, day: 1, hour: 0, minute: 0, second: 0, TimeSpan.FromHours(-14))
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.DateTime, // DateTimeOffset
									 Value = new DataModelValue(new DateTimeOffset(year: 2000, month: 1, day: 1, hour: 0, minute: 0, second: 0, TimeSpan.FromHours(14))),
									 ConstructorArg = new DateTimeOffset(year: 2000, month: 1, day: 1, hour: 0, minute: 0, second: 0, TimeSpan.FromHours(14))
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.DateTime, // DateTimeOffset
									 Value = new DataModelValue(new DateTimeOffset(year: 2000, month: 1, day: 1, hour: 0, minute: 0, second: 0, TimeSpan.FromMinutes(0))),
									 ConstructorArg = new DateTimeOffset(year: 2000, month: 1, day: 1, hour: 0, minute: 0, second: 0, TimeSpan.FromMinutes(0))
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.DateTime, // DateTimeOffset
									 Value = new DataModelValue(new DateTimeOffset(year: 2000, month: 1, day: 1, hour: 0, minute: 0, second: 0, TimeSpan.FromMinutes(5))),
									 ConstructorArg = new DateTimeOffset(year: 2000, month: 1, day: 1, hour: 0, minute: 0, second: 0, TimeSpan.FromMinutes(5))
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.DateTime, // DateTimeOffset
									 Value = new DataModelValue(new DateTimeOffset(year: 2000, month: 1, day: 1, hour: 0, minute: 0, second: 0, TimeSpan.FromMinutes(15))),
									 ConstructorArg = new DateTimeOffset(year: 2000, month: 1, day: 1, hour: 0, minute: 0, second: 0, TimeSpan.FromMinutes(15))
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.DateTime, // DateTimeOffset
									 Value = new DataModelValue(new DateTimeOffset(year: 2000, month: 1, day: 1, hour: 0, minute: 0, second: 0, TimeSpan.FromMinutes(30))),
									 ConstructorArg = new DateTimeOffset(year: 2000, month: 1, day: 1, hour: 0, minute: 0, second: 0, TimeSpan.FromMinutes(30))
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.DateTime, // DateTimeOffset
									 Value = new DataModelValue(new DateTimeOffset(year: 1, month: 1, day: 1, hour: 0, minute: 0, second: 0, TimeSpan.FromMinutes(0))),
									 ConstructorArg = new DateTimeOffset(year: 1, month: 1, day: 1, hour: 0, minute: 0, second: 0, TimeSpan.FromMinutes(0))
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.DateTime, // DateTimeOffset
									 Value = new DataModelValue(new DateTimeOffset(year: 1, month: 1, day: 1, hour: 0, minute: 0, second: 0, TimeSpan.FromMinutes(-1))),
									 ConstructorArg = new DateTimeOffset(year: 1, month: 1, day: 1, hour: 0, minute: 0, second: 0, TimeSpan.FromMinutes(-1))
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.DateTime, // DateTimeOffset
									 Value = new DataModelValue(new DateTimeOffset(year: 9999, month: 12, day: 31, hour: 23, minute: 59, second: 59, TimeSpan.FromMinutes(0))),
									 ConstructorArg = new DateTimeOffset(year: 9999, month: 12, day: 31, hour: 23, minute: 59, second: 59, TimeSpan.FromMinutes(0))
							 };
				yield return new Record
							 {
									 Type = DataModelValueType.DateTime, // DateTimeOffset
									 Value = new DataModelValue(new DateTimeOffset(year: 9999, month: 12, day: 31, hour: 23, minute: 59, second: 59, TimeSpan.FromMinutes(1))),
									 ConstructorArg = new DateTimeOffset(year: 9999, month: 12, day: 31, hour: 23, minute: 59, second: 59, TimeSpan.FromMinutes(1))
							 };

				var dataModelObject = new DataModelObject();
				yield return new Record
							 {
									 Type = DataModelValueType.Object,
									 Value = new DataModelValue(dataModelObject),
									 ConstructorArg = dataModelObject
							 };

				var dataModelArray = new DataModelArray();
				yield return new Record
							 {
									 Type = DataModelValueType.Array,
									 Value = new DataModelValue(dataModelArray),
									 ConstructorArg = dataModelArray
							 };
			}
		}

		private static IEnumerable<Record> LazyRecords => from r in SimpleRecords select r.WrapToLazy();

		private static IEnumerable<Record> LazyInLazyRecords => from r in LazyRecords select r.WrapToLazy();

		private static IEnumerable<Record> Records => SimpleRecords.Concat(LazyRecords).Concat(LazyInLazyRecords);

		private static string M(string line) => $"Failure in record at line {line}";

		public static IEnumerable<object?[]> RecordsConstructorArgValueType() => from r in Records select new[] { r.Line, r.ConstructorArg, r.Value, r.Type };

		public static IEnumerable<object[]> RecordsTypeValue() => from r in Records select new object[] { r.Line, r.Type, r.Value };

		public static IEnumerable<object[]> RecordsValueOriginalValue() => from r in Records select new object[] { r.Line, r.Value, r.OriginalValue };

		[TestMethod]
		[DynamicData(nameof(RecordsConstructorArgValueType), DynamicDataSourceType.Method)]
		public void CtrArg1_ShouldBeEqualToObject(string line, object ctrArg, DataModelValue value, DataModelValueType type)
		{
			var obj = value.ToObject();

			switch (type)
			{
				case DataModelValueType.Undefined:
				case DataModelValueType.Null:
				case DataModelValueType.String:
				case DataModelValueType.Boolean:
				case DataModelValueType.Number:
				case DataModelValueType.DateTime when ctrArg.GetType() == obj?.GetType():
					Assert.AreEqual(ctrArg, obj, M(line));
					break;

				case DataModelValueType.Object:
				case DataModelValueType.Array:
					Assert.AreSame(ctrArg, obj, M(line));
					break;

				case DataModelValueType.DateTime:
					Assert.AreEqual(DataModelDateTime.FromDateTime((DateTime) ctrArg).ToDateTimeOffset(), obj, M(line));
					break;

				default:
					Assert.Fail(M(line));
					break;
			}
		}

		[TestMethod]
		[DynamicData(nameof(RecordsTypeValue), DynamicDataSourceType.Method)]
		public void Type_ShouldBeArg1(string line, DataModelValueType type, DataModelValue value)
		{
			// assert
			Assert.AreEqual(type, value.Type, M(line));
		}

		[TestMethod]
		[DynamicData(nameof(RecordsTypeValue), DynamicDataSourceType.Method)]
		public void IsUndefined_ShouldBeCorrectValue(string line, DataModelValueType type, DataModelValue value)
		{
			// assert
			Assert.AreEqual(type == DataModelValueType.Undefined, value.IsUndefined(), M(line));
		}

		[TestMethod]
		[DynamicData(nameof(RecordsTypeValue), DynamicDataSourceType.Method)]
		public void IsUndefinedOrNull_ShouldBeCorrectValue(string line, DataModelValueType type, DataModelValue value)
		{
			// assert
			Assert.AreEqual(type == DataModelValueType.Undefined || type == DataModelValueType.Null, value.IsUndefinedOrNull(), M(line));
		}

		[TestMethod]
		[DynamicData(nameof(RecordsValueOriginalValue), DynamicDataSourceType.Method)]
		public void IEquatableEquals_ValueShouldBeEqualToOriginalValue(string line, DataModelValue value, DataModelValue originalValue)
		{
			// assert
			Assert.IsTrue(value.Equals(originalValue), M(line));
		}

		[TestMethod]
		[DynamicData(nameof(RecordsValueOriginalValue), DynamicDataSourceType.Method)]
		public void Equals_ValueShouldBeEqualToOriginalValue(string line, DataModelValue value, DataModelValue originalValue)
		{
			// assert
			Assert.IsTrue(value.Equals((object) originalValue), M(line));
		}

		[TestMethod]
		[DynamicData(nameof(RecordsValueOriginalValue), DynamicDataSourceType.Method)]
		public void EqualityOps_ValueShouldBeEqualToOriginalValue(string line, DataModelValue value, DataModelValue originalValue)
		{
			// assert
			Assert.IsTrue(value == originalValue, M(line));
			Assert.IsFalse(value != originalValue, M(line));
		}

		[TestMethod]
		[DynamicData(nameof(RecordsValueOriginalValue), DynamicDataSourceType.Method)]
		public void GetHashCode_HashCodeShouldMatchWithOriginalHashCode(string line, DataModelValue value, DataModelValue originalValue)
		{
			// assert
			Assert.IsTrue(value.GetHashCode() == originalValue.GetHashCode(), M(line));
		}

		[TestMethod]
		[DynamicData(nameof(RecordsConstructorArgValueType), DynamicDataSourceType.Method)]
		public void AsObject_ShouldReturnCorrectValueOrThrow(string line, object ctrArg, DataModelValue value, DataModelValueType type)
		{
			// assert
			if (type == DataModelValueType.Object)
			{
				Assert.AreEqual(ctrArg, value.AsObject(), M(line));
			}
			else
			{
				Assert.ThrowsException<ArgumentException>(value.AsObject, M(line));
			}
		}

		[TestMethod]
		[DynamicData(nameof(RecordsConstructorArgValueType), DynamicDataSourceType.Method)]
		public void AsNullableObject_ShouldReturnCorrectValueOrThrow(string line, object ctrArg, DataModelValue value, DataModelValueType type)
		{
			// assert
			if (type == DataModelValueType.Object)
			{
				Assert.AreSame(ctrArg, value.AsNullableObject(), M(line));
			}
			else if (type == DataModelValueType.Null)
			{
				Assert.IsNull(value.AsNullableObject(), M(line));
			}
			else
			{
				Assert.ThrowsException<ArgumentException>(value.AsNullableObject, M(line));
			}
		}

		[TestMethod]
		[DynamicData(nameof(RecordsConstructorArgValueType), DynamicDataSourceType.Method)]
		public void AsObjectOrEmpty_ShouldReturnCorrectValue(string line, object ctrArg, DataModelValue value, DataModelValueType type)
		{
			// assert
			if (type == DataModelValueType.Object)
			{
				Assert.AreEqual(ctrArg, value.AsObjectOrEmpty(), M(line));
			}
			else
			{
				Assert.AreSame(DataModelObject.Empty, value.AsObjectOrEmpty(), M(line));
			}
		}

		[TestMethod]
		[DynamicData(nameof(RecordsConstructorArgValueType), DynamicDataSourceType.Method)]
		public void AsArray_ShouldReturnCorrectValueOrThrow(string line, object ctrArg, DataModelValue value, DataModelValueType type)
		{
			// assert
			if (type == DataModelValueType.Array)
			{
				Assert.AreEqual(ctrArg, value.AsArray(), M(line));
			}
			else
			{
				Assert.ThrowsException<ArgumentException>(value.AsArray, M(line));
			}
		}

		[TestMethod]
		[DynamicData(nameof(RecordsConstructorArgValueType), DynamicDataSourceType.Method)]
		public void AsNullableArray_ShouldReturnCorrectValueOrThrow(string line, object ctrArg, DataModelValue value, DataModelValueType type)
		{
			// assert
			if (type == DataModelValueType.Array)
			{
				Assert.AreSame(ctrArg, value.AsNullableArray(), M(line));
			}
			else if (type == DataModelValueType.Null)
			{
				Assert.IsNull(value.AsNullableArray(), M(line));
			}
			else
			{
				Assert.ThrowsException<ArgumentException>(value.AsNullableArray, M(line));
			}
		}

		[TestMethod]
		[DynamicData(nameof(RecordsConstructorArgValueType), DynamicDataSourceType.Method)]
		public void AsArrayOrEmpty_ShouldReturnCorrectValue(string line, object ctrArg, DataModelValue value, DataModelValueType type)
		{
			// assert
			if (type == DataModelValueType.Array)
			{
				Assert.AreEqual(ctrArg, value.AsArrayOrEmpty(), M(line));
			}
			else
			{
				Assert.AreSame(DataModelArray.Empty, value.AsArrayOrEmpty(), M(line));
			}
		}

		[TestMethod]
		[DynamicData(nameof(RecordsConstructorArgValueType), DynamicDataSourceType.Method)]
		public void AsString_ShouldReturnCorrectValueOrThrow(string line, object ctrArg, DataModelValue value, DataModelValueType type)
		{
			// assert
			if (type == DataModelValueType.String)
			{
				Assert.AreEqual(ctrArg, value.AsString(), M(line));
			}
			else
			{
				Assert.ThrowsException<ArgumentException>(value.AsString, M(line));
			}
		}

		[TestMethod]
		[DynamicData(nameof(RecordsConstructorArgValueType), DynamicDataSourceType.Method)]
		public void AsNullableString_ShouldReturnCorrectValueOrThrow(string line, object ctrArg, DataModelValue value, DataModelValueType type)
		{
			// assert
			if (type == DataModelValueType.String)
			{
				Assert.AreSame(ctrArg, value.AsNullableString(), M(line));
			}
			else if (type == DataModelValueType.Null)
			{
				Assert.IsNull(value.AsNullableString(), M(line));
			}
			else
			{
				Assert.ThrowsException<ArgumentException>(value.AsNullableString, M(line));
			}
		}

		[TestMethod]
		[DynamicData(nameof(RecordsConstructorArgValueType), DynamicDataSourceType.Method)]
		public void AsStringOrDefault_ShouldReturnCorrectValue(string line, object ctrArg, DataModelValue value, DataModelValueType type)
		{
			// assert
			if (type == DataModelValueType.String)
			{
				Assert.AreEqual(ctrArg, value.AsStringOrDefault(), M(line));
			}
			else
			{
				Assert.IsNull(value.AsStringOrDefault(), M(line));
			}
		}

		[TestMethod]
		[DynamicData(nameof(RecordsConstructorArgValueType), DynamicDataSourceType.Method)]
		public void AsNumber_ShouldReturnCorrectValueOrThrow(string line, object ctrArg, DataModelValue value, DataModelValueType type)
		{
			// assert
			if (type == DataModelValueType.Number)
			{
				Assert.AreEqual(Convert.ToDouble(ctrArg), value.AsNumber(), M(line));
			}
			else
			{
				Assert.ThrowsException<ArgumentException>(() => value.AsNumber(), M(line));
			}
		}

		[TestMethod]
		[DynamicData(nameof(RecordsConstructorArgValueType), DynamicDataSourceType.Method)]
		public void AsNumberOrDefault_ShouldReturnCorrectValueOrThrow(string line, object ctrArg, DataModelValue value, DataModelValueType type)
		{
			// assert
			if (type == DataModelValueType.Number)
			{
				Assert.AreEqual(Convert.ToDouble(ctrArg), value.AsNumberOrDefault(), M(line));
			}
			else
			{
				Assert.IsNull(value.AsNumberOrDefault(), M(line));
			}
		}

		[TestInitialize]
		public void LazyValueInit()
		{
			_emptyDataModelObject = new DataModelObject();
			_emptyDataModelArray = new DataModelArray();

			_lazyValueNullMock = new Mock<ILazyValue>();
			_lazyValueNullMock.Setup(lv => lv.Value).Returns(DataModelValue.Null);

			_lazyValueObjectMock = new Mock<ILazyValue>();
			_lazyValueObjectMock.Setup(lv => lv.Value).Returns(new DataModelValue(_emptyDataModelObject));

			_lazyValueArrayMock = new Mock<ILazyValue>();
			_lazyValueArrayMock.Setup(lv => lv.Value).Returns(new DataModelValue(_emptyDataModelArray));

			_lazyValueOfLazyValueNullMock = new Mock<ILazyValue>();
			_lazyValueOfLazyValueNullMock.Setup(lv => lv.Value).Returns(new DataModelValue(_lazyValueNullMock.Object));
		}

		[TestMethod]
		public void CtrLazyValue_ShouldNotCallValuePropertyInConstructor()
		{
			// act
			var _ = new DataModelValue(_lazyValueNullMock.Object);

			// assert
			_lazyValueNullMock.Verify(lv => lv.Value, Times.Never);
		}

		[TestMethod]
		public void CloneAsWritable_ShouldCloneValueOfLazyValue()
		{
			// arrange
			var v = new DataModelValue(_lazyValueNullMock.Object);

			// act
			var c = v.CloneAsWritable();

			// assert
			Assert.IsFalse(c.IsLazyValue);
		}

		[TestMethod]
		public void CloneAsWritable_ShouldCloneValueOfNestedLazyValue()
		{
			// arrange
			var v = new DataModelValue(_lazyValueOfLazyValueNullMock.Object);

			// act
			var c = v.CloneAsWritable();

			// assert
			Assert.IsFalse(c.IsLazyValue);
		}

		[TestMethod]
		public void CloneAsReadOnly_ShouldCloneValueOfLazyValue()
		{
			// arrange
			var v = new DataModelValue(_lazyValueNullMock.Object);

			// act
			var c = v.CloneAsReadOnly();

			// assert
			Assert.IsFalse(c.IsLazyValue);
		}

		[TestMethod]
		public void CloneAsReadOnly_ShouldCloneValueOfNestedLazyValue()
		{
			// arrange
			var v = new DataModelValue(_lazyValueOfLazyValueNullMock.Object);

			// act
			var c = v.CloneAsReadOnly();

			// assert
			Assert.IsFalse(c.IsLazyValue);
		}

		[TestMethod]
		public void AsConstant_ShouldCloneValueOfLazyValue()
		{
			// arrange
			var v = new DataModelValue(_lazyValueNullMock.Object);

			// act
			var c = v.AsConstant();

			// assert
			Assert.IsFalse(c.IsLazyValue);
		}

		[TestMethod]
		public void AsConstant_ShouldCloneValueOfNestedLazyValue()
		{
			// arrange
			var v = new DataModelValue(_lazyValueOfLazyValueNullMock.Object);

			// act
			var c = v.AsConstant();

			// assert
			Assert.IsFalse(c.IsLazyValue);
		}

		[TestMethod]
		public void MakeDeepConstant_ShouldMakeConstantObjectOfLazyValue()
		{
			// arrange
			var v = new DataModelValue(_lazyValueObjectMock.Object);

			// act
			v.MakeDeepConstant();

			// assert
			Assert.IsFalse(_emptyDataModelObject.CanSet("prop"));
		}

		[TestMethod]
		public void MakeDeepConstant_ShouldMakeConstantArrayOfLazyValue()
		{
			// arrange
			var v = new DataModelValue(_lazyValueArrayMock.Object);

			// act
			v.MakeDeepConstant();

			// assert
			Assert.IsFalse(_emptyDataModelArray.CanSet(0));
		}
	}
}