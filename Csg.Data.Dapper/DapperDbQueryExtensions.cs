using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csg.Data;
using Csg.Data.Sql;
using Dapper;

namespace Csg.Data
{
    public static class DapperDbQueryExtensions
    {
        /// <summary>
        /// Creates a dapper command definition from the given query.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        public static Dapper.CommandDefinition ToDapperCommand(this IDbQueryBuilder query, CommandFlags commandFlags = CommandFlags.Buffered)
        {
            return query.Render().ToDapperCommand();
        }

        /// <summary>
        /// Creates a <see cref="Dapper.CommandDefinition"/> from the given SQL statement.
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        public static Dapper.CommandDefinition ToDapperCommand(this Sql.SqlStatement statement, System.Data.IDbTransaction transaction = null, int? commandTimeout = null, Dapper.CommandFlags commandFlags = Dapper.CommandFlags.Buffered)
        {
            var cmd = new Dapper.CommandDefinition(statement.CommandText,
                commandType: System.Data.CommandType.Text,
                parameters: statement.Parameters.ToDynamicParameters(),
                transaction: transaction,
                commandTimeout: commandTimeout,
                flags: commandFlags
            );

            return cmd;
        }

        /// <summary>
        /// Creates a Dapper <see cref="Dapper.DynamicParameters"/> object from the given set of parameter names and values.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static Dapper.DynamicParameters ToDynamicParameters(this IEnumerable<DbParameterValue> parameters)
        {
            var dynpar = new Dapper.DynamicParameters();

            foreach (var param in parameters)
            {
                dynpar.Add(param.ParameterName, param.Value, param.DbType, System.Data.ParameterDirection.Input, param.Size > 0 ? (int?)param.Size : null);
            }

            return dynpar;
        }

        /// <summary>
        /// Renders the given query and executes it using <see cref="Dapper.SqlMapper.Query{T}(IDbConnection, CommandDefinition)"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(this IDbQueryBuilder query, CommandFlags commandFlags = CommandFlags.Buffered)
        {
            return Dapper.SqlMapper.Query<T>(query.Connection, ToDapperCommand(query, commandFlags));
        }

        /// <summary>
        /// Renders the given query and executes it using <see cref="Dapper.SqlMapper.QueryAsync{T}(IDbConnection, CommandDefinition)"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        public static Task<IEnumerable<T>> QueryAsync<T>(this IDbQueryBuilder query, CommandFlags commandFlags = CommandFlags.Buffered)
        {
            return Dapper.SqlMapper.QueryAsync<T>(query.Connection, ToDapperCommand(query, commandFlags));
        }

        /// <summary>
        /// Renders the given query and executes it using <see cref="Dapper.SqlMapper.QuerySingle(IDbConnection, CommandDefinition)"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        public static T QuerySingle<T>(this IDbQueryBuilder query, CommandFlags commandFlags = CommandFlags.Buffered)
        {
            return Dapper.SqlMapper.QuerySingle<T>(query.Connection, ToDapperCommand(query, commandFlags));
        }

        /// <summary>
        /// Renders the given query and executes it using <see cref="Dapper.SqlMapper.QuerySingleAsync(IDbConnection, CommandDefinition)"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        public static Task<T> QuerySingleAsync<T>(this IDbQueryBuilder query, CommandFlags commandFlags = CommandFlags.Buffered)
        {
            return Dapper.SqlMapper.QuerySingleAsync<T>(query.Connection, ToDapperCommand(query, commandFlags));
        }

        /// <summary>
        /// Renders the given query and executes it using <see cref="Dapper.SqlMapper.QuerySingleOrDefault(IDbConnection, CommandDefinition)"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        public static T QuerySingleOrDefault<T>(this IDbQueryBuilder query, CommandFlags commandFlags = CommandFlags.Buffered)
        {
            return Dapper.SqlMapper.QuerySingleOrDefault<T>(query.Connection, ToDapperCommand(query, commandFlags));
        }

        /// <summary>
        /// Renders the given query and executes it using <see cref="Dapper.SqlMapper.QuerySingleOrDefaultAsync(IDbConnection, CommandDefinition)"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        public static Task<T> QuerySingleOrDefaultAsync<T>(this IDbQueryBuilder query, CommandFlags commandFlags = CommandFlags.Buffered)
        {
            return Dapper.SqlMapper.QuerySingleOrDefaultAsync<T>(query.Connection, ToDapperCommand(query, commandFlags));
        }

        /// <summary>
        /// Renders the given query and executes it using <see cref="Dapper.SqlMapper.QueryFirst{T}(IDbConnection, CommandDefinition)"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        public static T QueryFirst<T>(this IDbQueryBuilder query, CommandFlags commandFlags = CommandFlags.Buffered)
        {
            return Dapper.SqlMapper.QueryFirst<T>(query.Connection, ToDapperCommand(query, commandFlags));
        }

        /// <summary>
        /// Renders the given query and executes it using <see cref="Dapper.SqlMapper.QueryFirstAsync{T}(IDbConnection, CommandDefinition)"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        public static Task<T> QueryFirstAsync<T>(this IDbQueryBuilder query, CommandFlags commandFlags = CommandFlags.Buffered)
        {
            return Dapper.SqlMapper.QueryFirstAsync<T>(query.Connection, ToDapperCommand(query, commandFlags));
        }

        /// <summary>
        /// Renders the given query and executes it using <see cref="Dapper.SqlMapper.QueryFirstOrDefault{T}(IDbConnection, CommandDefinition)"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        public static T QueryFirstOrDefault<T>(this IDbQueryBuilder query, CommandFlags commandFlags = CommandFlags.Buffered)
        {
            return Dapper.SqlMapper.QueryFirstOrDefault<T>(query.Connection, ToDapperCommand(query, commandFlags));
        }

        /// <summary>
        /// Renders the given query and executes it using <see cref="Dapper.SqlMapper.QueryFirstOrDefaultAsync{T}(IDbConnection, CommandDefinition)"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        public static Task<T> QueryFirstOrDefaultAsync<T>(this IDbQueryBuilder query, CommandFlags commandFlags = CommandFlags.Buffered)
        {
            return Dapper.SqlMapper.QueryFirstOrDefaultAsync<T>(query.Connection, ToDapperCommand(query, commandFlags));
        }
    }

}