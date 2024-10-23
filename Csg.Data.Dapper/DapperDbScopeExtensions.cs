using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace Csg.Data;

/// <summary>
///     Extension methods for using Dapper with a <see cref="Csg.Data.DbScope"/.>
/// </summary>
public static class DapperDbScopeExtensions
{
    /// <summary>
    ///     Executes the given query text against the connection associated with <see cref="DbScope" />.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="scope"></param>
    /// <param name="commandText"></param>
    /// <param name="commandType"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static IEnumerable<T> Query<T>(this DbScope scope, string commandText,
        CommandType commandType = CommandType.Text,
        int? commandTimeout = null,
        object parameters = null
    )
    {
        var cmd = new CommandDefinition(commandText,
            commandType: commandType,
            parameters: parameters,
            transaction: scope.Transaction,
            commandTimeout: commandTimeout
        );

        return SqlMapper.Query<T>(scope.Connection, cmd);
    }

    /// <summary>
    ///     Executes the given query text against the connection associated with <see cref="DbScope" />.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="scope"></param>
    /// <param name="commandText"></param>
    /// <param name="commandType"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static async Task<IEnumerable<T>> QueryAsync<T>(this DbScope scope, string commandText,
        CommandType commandType = CommandType.Text,
        int? commandTimeout = null,
        object parameters = null
    )
    {
        var cmd = new CommandDefinition(commandText,
            commandType: commandType,
            parameters: parameters,
            transaction: scope.Transaction,
            commandTimeout: commandTimeout
        );

        return await SqlMapper.QueryAsync<T>(scope.Connection, cmd);
    }

    /// <summary>
    ///     Executes the given command text against the connection associated with <see cref="DbScope" />.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="scope"></param>
    /// <param name="commandText"></param>
    /// <param name="commandType"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static int Execute<T>(this DbScope scope, string commandText,
        CommandType commandType = CommandType.Text,
        int? commandTimeout = null,
        object parameters = null
    )
    {
        var cmd = new CommandDefinition(commandText,
            commandType: commandType,
            parameters: parameters,
            transaction: scope.Transaction,
            commandTimeout: commandTimeout
        );

        return SqlMapper.Execute(scope.Connection, cmd);
    }
}