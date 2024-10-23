using System;
using System.Data;
using Dapper;

namespace Csg.Data;

/// <summary>
///     Represents a date and/or time value and it's associated database data type.
/// </summary>
public sealed class DbDate<TValue> : SqlMapper.ICustomQueryParameter, IDbTypeProvider
    where TValue : struct
{
    public static DbDateType DefaultDateType = DbDateType.DateTime;

    /// <summary>
    ///     Create a new DbDate
    /// </summary>
    public DbDate()
    {
        DateTimeType = DefaultDateType;
    }

    /// <summary>
    ///     Gets or sets the database data type.
    /// </summary>
    public DbDateType DateTimeType { get; set; }

    /// <summary>
    ///     Gets or sets the date/time value.
    /// </summary>
    public TValue? Value { get; set; }

    /// <summary>
    ///     Adds the parameter to the command object using this is as a parameter value.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="name"></param>
    public void AddParameter(IDbCommand command, string name)
    {
        var param = command.CreateParameter();
        param.ParameterName = name;

        if (Value == null)
            param.Value = DBNull.Value;
        else
            param.Value = Value;

        param.DbType = GetDbType();


        command.Parameters.Add(param);
    }

    /// <summary>
    ///     Gets the database data type associated with the value.
    /// </summary>
    /// <returns></returns>
    public DbType GetDbType()
    {
        if (DateTimeType == DbDateType.Date)
            return DbType.Date;
        if (DateTimeType == DbDateType.DateTime2)
            return DbType.DateTime2;
        if (DateTimeType == DbDateType.DateTimeOffset)
            return DbType.DateTimeOffset;
        throw new NotSupportedException("Unsupported date type");
    }
}