using System;

namespace Csg.Data;

/// <summary>
///     Extension methods for the DbDate type
/// </summary>
public static class DbDateExtensions
{
    /// <summary>
    ///     Converts a <see cref="DateTime" /> value into a <see cref="DbDate{DateTime}" />.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static DbDate<DateTime> ToDbDate(this DateTime date, DbDateType type)
    {
        return new DbDate<DateTime>
        {
            DateTimeType = type,
            Value = date
        };
    }

    /// <summary>
    ///     Converts a <see cref="DateTimeOffset" /> value into a <see cref="DbDate{DateTimeOffset}" />.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static DbDate<DateTimeOffset> ToDbDate(this DateTimeOffset date, DbDateType type)
    {
        return new DbDate<DateTimeOffset>
        {
            DateTimeType = type,
            Value = date
        };
    }
}