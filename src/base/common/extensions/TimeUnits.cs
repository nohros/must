﻿using System;

namespace Nohros.Extensions.Time
{
  /// <summary>
  /// Extension methods for the <see cref="TimeUnit"/>.
  /// </summary>
  public static class TimeUnits
  {
    /// <summary>
    /// Converts the specified datetime to the unix time unit.
    /// </summary>
    /// <returns>
    /// The number of seconds since unix epoch.
    /// </returns>
    /// <remarks>
    /// If <see cref="DateTime.Kind"/> property of the given
    /// <see cref="duration"/> is <see cref="DateTimeKind.Unspecified"/>
    /// <paramref name="duration"/> is assumed to be a UTC time.
    /// </remarks>
    [Obsolete("Use ToUnixEpoch instead", true)]
    public static long ToUnixTime(this DateTime duration) {
      return TimeUnitHelper.ToUnixTime(duration);
    }

    /// <summary>
    /// Converts the specified datetime to the unix time unit.
    /// </summary>
    /// <returns>
    /// The number of seconds since unix epoch.
    /// </returns>
    /// <remarks>
    /// If <see cref="DateTime.Kind"/> property of the given
    /// <see cref="duration"/> is <see cref="DateTimeKind.Unspecified"/>
    /// <paramref name="duration"/> is assumed to be a UTC time.
    /// </remarks>
    public static long ToUnixEpoch(this DateTime duration) {
      return TimeUnitHelper.ToUnixEpoch(duration);
    }

    /// <summary>
    /// Converts the specified datetime to the unix time unit in the given
    /// time unit.
    /// </summary>
    /// <returns>
    /// The number of seconds since unix epoch.
    /// </returns>
    /// <remarks>
    /// If <see cref="DateTime.Kind"/> property of the given
    /// <see cref="duration"/> is <see cref="DateTimeKind.Unspecified"/>
    /// <paramref name="duration"/> is assumed to be a UTC time.
    /// </remarks>
    public static long ToUnixEpoch(this DateTime duration, TimeUnit unit) {
      long epoch = TimeUnitHelper.ToUnixEpoch(duration);
      switch (unit) {
        case TimeUnit.Milliseconds:
          return epoch.ToMilliseconds(TimeUnit.Seconds);
        case TimeUnit.Nanoseconds:
          return epoch.ToNanoseconds(TimeUnit.Seconds);
        case TimeUnit.Seconds:
          return epoch;
        default:
          throw new NotSupportedException();
      }
    }

    /// <summary>
    /// Converts the specified epoch time to its corresponding local date
    /// and time.
    /// </summary>
    /// <param name="timestamp">
    /// A Unix epoch time.
    /// </param>
    /// <returns>
    /// The local time representation of the specified unix epoch
    /// time.
    /// </returns>
    /// <remarks>
    /// The returned date time represents a local time. To convert to a
    /// Coordinated Universal Time (UTC) use the
    /// <see cref="FromUnixEpoch(long, DateTimeKind)"/> overload
    /// </remarks>
    public static DateTime FromUnixEpoch(this long timestamp) {
      return FromUnixEpoch(timestamp, DateTimeKind.Local);
    }

    /// <summary>
    /// Converts the specified epoch time to its corresponding date and time
    /// using the given date time kind.
    /// </summary>
    /// <param name="timestamp">
    /// A Unix epoch time.
    /// </param>
    /// <returns>
    /// The <see cref="DateTime"/> representation of the specified unix epoch
    /// time.
    /// </returns>
    /// <remarks>
    /// If <paramref name="kind"/> is set to
    /// <see cref="DateTimeKind.Unspecified"/> the
    /// <see cref="DateTimeKind.Local"/> will be used.
    /// </remarks>
    public static DateTime FromUnixEpoch(this long timestamp, DateTimeKind kind) {
      return TimeUnitHelper.FromUnixEpoch(timestamp, kind);
    }

    /// <summary>
    /// Convert the specified time duration in the given unit to the
    /// nanoseconds units.
    /// </summary>
    /// <returns></returns>
    public static long ToNanoseconds(this long duration, TimeUnit unit) {
      return TimeUnitHelper.ToNanos(duration, unit);
    }


    /// <summary>
    /// Convert the specified time duration in the given unit to the
    /// milliseconds units.
    /// </summary>
    /// <returns></returns>
    public static long ToMilliseconds(this long duration, TimeUnit unit) {
      return TimeUnitHelper.ToMilliseconds(duration, unit);
    }

    /// <summary>
    /// Convert the specified time duration in the given unit to the
    /// seconds units.
    /// </summary>
    /// <returns></returns>
    public static long ToSeconds(this long duration, TimeUnit unit) {
      return TimeUnitHelper.ToSeconds(duration, unit);
    }
  }
}