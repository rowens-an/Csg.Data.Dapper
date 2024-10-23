using System;
using System.Data;
using Dapper;

namespace Csg.Data;

/// <summary>
///     Provides a string type that has additional database string type information.
/// </summary>
public sealed class DbString : SqlMapper.ICustomQueryParameter, IDbTypeProvider
{
    /// <summary>
    ///     A value to set the default value of strings
    ///     going through Dapper. Default is 4000, any value larger than this
    ///     field will not have the default value applied.
    /// </summary>
    public const int DefaultLength = Dapper.DbString.DefaultLength;

    /// <summary>
    ///     Create a new DbString
    /// </summary>
    public DbString()
    {
        Length = -1;
        IsAnsi = IsAnsiDefault;
    }

    /// <summary>
    ///     Default value for IsAnsi.
    /// </summary>
    public static bool IsAnsiDefault { get; set; }

    /// <summary>
    ///     Ansi vs Unicode
    /// </summary>
    public bool IsAnsi { get; set; }

    /// <summary>
    ///     Fixed length
    /// </summary>
    public bool IsFixedLength { get; set; }

    /// <summary>
    ///     Length of the string -1 for max
    /// </summary>
    public int Length { get; set; }

    /// <summary>
    ///     The value of the string
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    ///     Add the parameter to the command... internal use only
    /// </summary>
    /// <param name="command"></param>
    /// <param name="name"></param>
    public void AddParameter(IDbCommand command, string name)
    {
        if (IsFixedLength && Length == -1)
            throw new InvalidOperationException("If specifying IsFixedLength, a Length must also be specified");

        var param = command.CreateParameter();

        param.ParameterName = name;

        if (Value == null)
            param.Value = DBNull.Value;
        else
            param.Value = Value;

        if (Length == -1 && Value != null && Value.Length <= DefaultLength)
            param.Size = DefaultLength;
        else
            param.Size = Length;

        param.DbType = IsAnsi
            ? IsFixedLength ? DbType.AnsiStringFixedLength : DbType.AnsiString
            : IsFixedLength
                ? DbType.StringFixedLength
                : DbType.String;

        command.Parameters.Add(param);
    }

    DbType IDbTypeProvider.GetDbType()
    {
        return IsAnsi
            ? IsFixedLength ? DbType.AnsiStringFixedLength : DbType.AnsiString
            : IsFixedLength
                ? DbType.StringFixedLength
                : DbType.String;
    }
}