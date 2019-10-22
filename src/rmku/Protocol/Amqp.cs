using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using rmku.Protocol.Primitives;
using rmku.Protocol.Connection;

namespace rmku.Protocol
{
	internal static class Amqp
	{
		internal delegate T ReadDelegate<T>(ReadOnlySpan<byte> bytes);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe T Read<T>(ref ReadOnlySequence<byte> data, ReadDelegate<T> reader) where T : unmanaged
		{
			int length = sizeof(T);
			if (data.FirstSpan.Length >= length)
			{
				var val = reader(data.FirstSpan);
				data = data.Slice(length);
				return val;
			}

			Span<byte> bytes = stackalloc byte[length];
			data.Slice(0, length).CopyTo(bytes);
			data = data.Slice(length);
			return reader(bytes);
		}

		public static ShortUInt ReadShortUint(ref ReadOnlySequence<byte> data)
		{
			return new ShortUInt(Read<ushort>(ref data, BinaryPrimitives.ReadUInt16BigEndian));
		}

		public static byte ReadOctet(ref ReadOnlySequence<byte> data)
		{
			byte octet = data.FirstSpan[0];
			data = data.Slice(1);
			return octet;
		}

		public static uint ReadShortString(ref ReadOnlySequence<byte> data, out ShortString shortString)
		{
			byte length = ReadOctet(ref data);

			byte[] content = new byte[length];
			data.Slice(0, length).CopyTo(content);
			data = data.Slice(length);

			shortString = new ShortString(content);
			return sizeof(byte) + (uint)length;
		}

		public static uint ReadLongString(ref ReadOnlySequence<byte> data, out LongString longString)
		{
			uint length = Read<uint>(ref data, BinaryPrimitives.ReadUInt32BigEndian);

			byte[] content = new byte[length];
			data.Slice(0, length).CopyTo(content);
			data = data.Slice(length);

			longString = new LongString(content);
			return sizeof(uint) + length;
		}

		public static uint ReadTable(ref ReadOnlySequence<byte> data, out Table table)
		{
			uint length = Read<uint>(ref data, BinaryPrimitives.ReadUInt32BigEndian);

			var values = new List<object>(32);
			uint readSoFar = 0;

			while(length > readSoFar)
			{
				readSoFar += ReadFieldValuePair(ref data, out var value);
				values.Add(value);
			}

			table = new Table(values.ToArray());
			return sizeof(uint) + length;
		}

		private static Primitives.Boolean ReadBoolean(ref ReadOnlySequence<byte> data)
		{
			var octet = ReadOctet(ref data);
			return new Primitives.Boolean(octet);
		}

		private static ShortShortInt ReadShortShortInt(ref ReadOnlySequence<byte> data)
		{
			byte octet = data.FirstSpan[0];
			data = data.Slice(1);
			return new ShortShortInt((sbyte)octet);
		}

		private static ShortShortUInt ReadShortShortUInt(ref ReadOnlySequence<byte> data)
		{
			byte octet = data.FirstSpan[0];
			data = data.Slice(1);
			return new ShortShortUInt(octet);
		}

		private static ShortInt ReadShortInt(ref ReadOnlySequence<byte> data)
		{
			return new ShortInt(Read<short>(ref data, BinaryPrimitives.ReadInt16BigEndian));
		}

		private static ShortUInt ReadShortUInt(ref ReadOnlySequence<byte> data)
		{
			return new ShortUInt(Read<ushort>(ref data, BinaryPrimitives.ReadUInt16BigEndian));
		}

		private static LongInt ReadLongInt(ref ReadOnlySequence<byte> data)
		{
			return new LongInt(Read<int>(ref data, BinaryPrimitives.ReadInt32BigEndian));
		}

		private static LongUInt ReadLongUInt(ref ReadOnlySequence<byte> data)
		{
			return new LongUInt(Read<uint>(ref data, BinaryPrimitives.ReadUInt32BigEndian));
		}

		private static LongLongInt ReadLongLongInt(ref ReadOnlySequence<byte> data)
		{
			return new LongLongInt(Read<long>(ref data, BinaryPrimitives.ReadInt64BigEndian));
		}

		private static LongLongUInt ReadLongLongUInt(ref ReadOnlySequence<byte> data)
		{
			return new LongLongUInt(Read<ulong>(ref data, BinaryPrimitives.ReadUInt64BigEndian));
		}

		private static Primitives.Float ReadFloat(ref ReadOnlySequence<byte> data)
		{
			return new Primitives.Float(BitConverter.Int32BitsToSingle(Read(ref data, BinaryPrimitives.ReadInt32BigEndian)));
		}

		private static Primitives.Double ReadDouble(ref ReadOnlySequence<byte> data)
		{
			return new Primitives.Double(BitConverter.Int64BitsToDouble(Read(ref data, BinaryPrimitives.ReadInt64BigEndian)));
		}

		private static Primitives.Decimal ReadDecimal(ref ReadOnlySequence<byte> data)
		{
			//TODO think anout how I want to implement decimals
			throw new NotImplementedException();
		}

		private static Timestamp ReadTimestamp(ref ReadOnlySequence<byte> data)
		{
			return new Timestamp(Read(ref data, BinaryPrimitives.ReadUInt64BigEndian));
		}

		private static uint ReadFieldArray(ref ReadOnlySequence<byte> data, out FieldArray<object> values)
		{
			int length = Read<int>(ref data, BinaryPrimitives.ReadInt32BigEndian);

			var readValues = new List<FieldValue<object>>(32);
			uint readSoFar = 0;

			while (length > readSoFar)
			{
				readSoFar += ReadFieldValuePair(ref data, out var value);
				readValues.Add(new FieldValue<object>(value));
			}

			values = new FieldArray<object>(readValues.ToArray());
			return sizeof(int) + (uint)length;
		}

		private static uint ReadFieldValuePair(ref ReadOnlySequence<byte> data, out object value)
		{
			value = null;
			uint size = ReadShortString(ref data, out ShortString name);

			byte fieldType = ReadOctet(ref data);
			size += sizeof(byte);

			switch (fieldType)
			{
				case (byte)'t':
					size += sizeof(byte);
					value = new FieldValuePair<Primitives.Boolean>(name, ReadBoolean(ref data));
					break;

				case (byte)'b':
					size += sizeof(byte);
					value = new FieldValuePair<ShortShortInt>(name, ReadShortShortInt(ref data));
					break;

				case (byte)'B':
					size += sizeof(byte);
					value = new FieldValuePair<ShortShortUInt>(name, ReadShortShortUInt(ref data));
					break;

				case (byte)'U':
					size += sizeof(Int16);
					value = new FieldValuePair<ShortInt>(name, ReadShortInt(ref data));
					break;

				case (byte)'u':
					size += sizeof(UInt16);
					value = new FieldValuePair<ShortUInt>(name, ReadShortUInt(ref data));
					break;

				case (byte)'I':
					size += sizeof(Int32);
					value = new FieldValuePair<LongInt>(name, ReadLongInt(ref data));
					break;

				case (byte)'i':
					size += sizeof(Int32);
					value = new FieldValuePair<LongUInt>(name, ReadLongUInt(ref data));
					break;

				case (byte)'L':
					size += sizeof(Int64);
					value = new FieldValuePair<LongLongInt>(name, ReadLongLongInt(ref data));
					break;

				case (byte)'l':
					size += sizeof(UInt64);
					value = new FieldValuePair<LongLongUInt>(name, ReadLongLongUInt(ref data));
					break;

				case (byte)'f':
					size += sizeof(float);
					value = new FieldValuePair<Primitives.Float>(name, ReadFloat(ref data));
					break;

				case (byte)'d':
					size += sizeof(double);
					value = new FieldValuePair<Primitives.Double>(name, ReadDouble(ref data));
					break;

				case (byte)'D':
					size += sizeof(byte) + 4;
					value = new FieldValuePair<Primitives.Decimal>(name, ReadDecimal(ref data));
					break;

				case (byte)'s':
					size += ReadShortString(ref data, out var shortString);
					value = new FieldValuePair<ShortString>(name, shortString);
					break;

				case (byte)'S':
					size += ReadLongString(ref data, out var longString);
					value = new FieldValuePair<LongString>(name, longString);
					break;

				case (byte)'A':
					size += ReadFieldArray(ref data, out FieldArray<object> fieldArray);
					value = new FieldValuePair<FieldArray<object>>(name, fieldArray);
					break;

				case (byte)'T':
					size += 8;
					value = new FieldValuePair<Timestamp>(name, new FieldValue<Timestamp>(ReadTimestamp(ref data)));
					break;

				case (byte)'F':
					size += ReadTable(ref data, out Table table);
					value = new FieldValuePair<Table>(name, table);
					break;

				case (byte)'V':
					size += 0;
					value = null;
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(fieldType));
			}
			
			return size;
		}

		public static Start ReadStart(ref ReadOnlySequence<byte> data)
		{
			byte versionMajor = ReadOctet(ref data);
			byte versionMinor = ReadOctet(ref data);
			ReadTable(ref data, out Table serverProperties);
			ReadLongString(ref data, out LongString mechanisms);
			ReadLongString(ref data, out LongString locales);

			return new Start(versionMajor, versionMinor, serverProperties, mechanisms, locales);
		}
	}
}
