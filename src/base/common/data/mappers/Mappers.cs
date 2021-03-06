﻿using System;

namespace Nohros.Data
{
  /// <summary>
  /// Factory class for implementations of the
  /// <see cref="IDataReaderMapper{T}"/> interface that maps to value types.
  /// </summary>
  [Obsolete("This class is obsolete and was replaced by the ValueTypeMappers")]
  public static class Mappers
  {
    /// <summary>
    /// Creates an instance of a class that implements the
    /// <see cref="IDataReaderMapper{T}"/> where T is <see cref="bool"/>.
    /// </summary>
    public static IDataReaderMapper<bool> Boolean() {
      return ValueTypeMappers.Boolean();
    }

    /// <summary>
    /// Creates an instance of a class that implements the
    /// <see cref="IDataReaderMapper{T}"/> where T is <see cref="byte"/>.
    /// </summary>
    public static IDataReaderMapper<byte> Byte() {
      return ValueTypeMappers.Byte();
    }

    /// <summary>
    /// Creates an instance of a class that implements the
    /// <see cref="IDataReaderMapper{T}"/> where T is <see cref="char"/>.
    /// </summary>
    public static IDataReaderMapper<char> Char() {
      return ValueTypeMappers.Char();
    }

    /// <summary>
    /// Creates an instance of a class that implements the
    /// <see cref="IDataReaderMapper{T}"/> where T is <see cref="DateTime"/>.
    /// </summary>
    public static IDataReaderMapper<DateTime> DateTime() {
      return ValueTypeMappers.DateTime();
    }

    /// <summary>
    /// Creates an instance of a class that implements the
    /// <see cref="IDataReaderMapper{T}"/> where T is <see cref="decimal"/>.
    /// </summary>
    public static IDataReaderMapper<decimal> Decimal() {
      return ValueTypeMappers.Decimal();
    }

    /// <summary>
    /// Creates an instance of a class that implements the
    /// <see cref="IDataReaderMapper{T}"/> where T is <see cref="double"/>.
    /// </summary>
    public static IDataReaderMapper<double> Double() {
      return ValueTypeMappers.Double();
    }

    /// <summary>
    /// Creates an instance of a class that implements the
    /// <see cref="IDataReaderMapper{T}"/> where T is <see cref="float"/>.
    /// </summary>
    public static IDataReaderMapper<float> Float() {
      return ValueTypeMappers.Float();
    }

    /// <summary>
    /// Creates an instance of a class that implements the
    /// <see cref="IDataReaderMapper{T}"/> where T is <see cref="Guid"/>.
    /// </summary>
    public static IDataReaderMapper<Guid> Guid() {
      return ValueTypeMappers.Guid();
    }

    /// <summary>
    /// Creates an instance of a class that implements the
    /// <see cref="IDataReaderMapper{T}"/> where T is <see cref="short"/>.
    /// </summary>
    public static IDataReaderMapper<short> Int16() {
      return ValueTypeMappers.Int16();
    }

    /// <summary>
    /// Creates an instance of a class that implements the
    /// <see cref="IDataReaderMapper{T}"/> where T is <see cref="int"/>.
    /// </summary>
    public static IDataReaderMapper<int> Int32() {
      return ValueTypeMappers.Int32();
    }

    /// <summary>
    /// Creates an instance of a class that implements the
    /// <see cref="IDataReaderMapper{T}"/> where T is <see cref="long"/>.
    /// </summary>
    public static IDataReaderMapper<long> Long() {
      return ValueTypeMappers.Long();
    }

    /// <summary>
    /// Creates an instance of a class that implements the
    /// <see cref="IDataReaderMapper{T}"/> where T is <see cref="string"/>.
    /// </summary>
    public static IDataReaderMapper<string> String() {
      return ValueTypeMappers.String();
    }
  }
}
