using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Csg.Data.Sql;
using Dapper;

namespace Csg.Data;

public static class DapperDbQueryExtensions
{
    /// <summary>
    ///     Creates a dapper command definition from the given query.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="commandFlags"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static CommandDefinition ToDapperCommand(this IDbQueryBuilder query,
        CommandFlags commandFlags = CommandFlags.Buffered,
        CancellationToken cancellationToken = default)
    {
        return query.Render().ToDapperCommand(query.Transaction, query.CommandTimeout, commandFlags, cancellationToken);
    }

    /// <summary>
    ///     Creates a <see cref="Dapper.CommandDefinition" /> from the given SQL statement.
    /// </summary>
    /// <param name="statement"></param>
    /// <param name="transaction"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandFlags"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static CommandDefinition ToDapperCommand(this SqlStatement statement,
        IDbTransaction transaction = null, int? commandTimeout = null,
        CommandFlags commandFlags = CommandFlags.Buffered,
        CancellationToken cancellationToken = default)
    {
        var cmd = new CommandDefinition(statement.CommandText,
            commandType: CommandType.Text,
            parameters: statement.Parameters.ToDynamicParameters(),
            transaction: transaction,
            commandTimeout: commandTimeout,
            flags: commandFlags,
            cancellationToken: cancellationToken
        );

        return cmd;
    }

    /// <summary>
    ///     Creates a Dapper <see cref="Dapper.DynamicParameters" /> object from the given set of parameter names and values.
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static DynamicParameters ToDynamicParameters(this IEnumerable<DbParameterValue> parameters)
    {
        var dynpar = new DynamicParameters();

        foreach (var param in parameters)
            dynpar.Add(param.ParameterName, param.Value, param.DbType, ParameterDirection.Input,
                param.Size > 0 ? param.Size : null);

        return dynpar;
    }

    /// <summary>
    ///     Renders the given query and executes it using
    ///     <see cref="Dapper.SqlMapper.Query{T}(IDbConnection, CommandDefinition)" />.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="commandFlags"></param>
    /// <returns></returns>
    public static IEnumerable<T> Query<T>(this IDbQueryBuilder query, CommandFlags commandFlags = CommandFlags.Buffered)
    {
        return query.Connection.Query<T>(ToDapperCommand(query, commandFlags));
    }

    /// <summary>
    ///     Renders the given query and executes it using
    ///     <see cref="Dapper.SqlMapper.QueryAsync{T}(IDbConnection, CommandDefinition)" />.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="commandFlags"></param>
    /// <returns></returns>
    public static Task<IEnumerable<T>> QueryAsync<T>(this IDbQueryBuilder query,
        CommandFlags commandFlags = CommandFlags.Buffered,
        CancellationToken cancellationToken = default)
    {
        return query.Connection.QueryAsync<T>(ToDapperCommand(query, commandFlags, cancellationToken));
    }

    /// <summary>
    ///     Renders the given query and executes it using
    ///     <see cref="Dapper.SqlMapper.QuerySingle(IDbConnection, CommandDefinition)" />.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="commandFlags"></param>
    /// <returns></returns>
    public static T QuerySingle<T>(this IDbQueryBuilder query, CommandFlags commandFlags = CommandFlags.Buffered)
    {
        return query.Connection.QuerySingle<T>(ToDapperCommand(query, commandFlags));
    }

    /// <summary>
    ///     Renders the given query and executes it using
    ///     <see cref="Dapper.SqlMapper.QuerySingleAsync(IDbConnection, CommandDefinition)" />.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="commandFlags"></param>
    /// <returns></returns>
    public static Task<T> QuerySingleAsync<T>(this IDbQueryBuilder query,
        CommandFlags commandFlags = CommandFlags.Buffered,
        CancellationToken cancellationToken = default)
    {
        return query.Connection.QuerySingleAsync<T>(ToDapperCommand(query, commandFlags, cancellationToken));
    }

    /// <summary>
    ///     Renders the given query and executes it using
    ///     <see cref="Dapper.SqlMapper.QuerySingleOrDefault(IDbConnection, CommandDefinition)" />.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="commandFlags"></param>
    /// <returns></returns>
    public static T QuerySingleOrDefault<T>(this IDbQueryBuilder query,
        CommandFlags commandFlags = CommandFlags.Buffered)
    {
        return query.Connection.QuerySingleOrDefault<T>(ToDapperCommand(query, commandFlags));
    }

    /// <summary>
    ///     Renders the given query and executes it using
    ///     <see cref="Dapper.SqlMapper.QuerySingleOrDefaultAsync(IDbConnection, CommandDefinition)" />.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="commandFlags"></param>
    /// <returns></returns>
    public static Task<T> QuerySingleOrDefaultAsync<T>(this IDbQueryBuilder query,
        CommandFlags commandFlags = CommandFlags.Buffered,
        CancellationToken cancellationToken = default)
    {
        return query.Connection.QuerySingleOrDefaultAsync<T>(ToDapperCommand(query, commandFlags, cancellationToken));
    }

    /// <summary>
    ///     Renders the given query and executes it using
    ///     <see cref="Dapper.SqlMapper.QueryFirst{T}(IDbConnection, CommandDefinition)" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="commandFlags"></param>
    /// <returns></returns>
    public static T QueryFirst<T>(this IDbQueryBuilder query, CommandFlags commandFlags = CommandFlags.Buffered)
    {
        return query.Connection.QueryFirst<T>(ToDapperCommand(query, commandFlags));
    }

    /// <summary>
    ///     Renders the given query and executes it using
    ///     <see cref="Dapper.SqlMapper.QueryFirstAsync{T}(IDbConnection, CommandDefinition)" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="commandFlags"></param>
    /// <returns></returns>
    public static Task<T> QueryFirstAsync<T>(this IDbQueryBuilder query,
        CommandFlags commandFlags = CommandFlags.Buffered,
        CancellationToken cancellationToken = default)
    {
        return query.Connection.QueryFirstAsync<T>(ToDapperCommand(query, commandFlags, cancellationToken));
    }

    /// <summary>
    ///     Renders the given query and executes it using
    ///     <see cref="Dapper.SqlMapper.QueryFirstOrDefault{T}(IDbConnection, CommandDefinition)" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="commandFlags"></param>
    /// <returns></returns>
    public static T QueryFirstOrDefault<T>(this IDbQueryBuilder query,
        CommandFlags commandFlags = CommandFlags.Buffered)
    {
        return query.Connection.QueryFirstOrDefault<T>(ToDapperCommand(query, commandFlags));
    }

    /// <summary>
    ///     Renders the given query and executes it using
    ///     <see cref="Dapper.SqlMapper.QueryFirstOrDefaultAsync{T}(IDbConnection, CommandDefinition)" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="commandFlags"></param>
    /// <returns></returns>
    public static Task<T> QueryFirstOrDefaultAsync<T>(this IDbQueryBuilder query,
        CommandFlags commandFlags = CommandFlags.Buffered,
        CancellationToken cancellationToken = default)
    {
        return query.Connection.QueryFirstOrDefaultAsync<T>(ToDapperCommand(query, commandFlags, cancellationToken));
    }
}