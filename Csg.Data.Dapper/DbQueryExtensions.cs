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
        public static Dapper.CommandDefinition CreateDapperCommand(this IDbQueryBuilder query, CommandFlags commandFlags = CommandFlags.Buffered)
        {
            var stmt = query.Render();
            var parameters = new Dapper.DynamicParameters();
            var cmd = new CommandDefinition(stmt.CommandText,
                commandType: System.Data.CommandType.Text,
                parameters: parameters,
                transaction: query.Transaction,
                commandTimeout: query.CommandTimeout,
                flags: commandFlags
            );

            foreach (var param in stmt.Parameters)
            {
                parameters.Add(param.ParameterName, param.Value, param.DbType, System.Data.ParameterDirection.Input, param.Size > 0 ? (int?)param.Size : null);
            }

            return cmd;
        }

        /// <summary>
        /// Renders the given query and executes it using Dapper.SqlMapper.Query().
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(this IDbQueryBuilder query, CommandFlags commandFlags = CommandFlags.Buffered)
        {
            return Dapper.SqlMapper.Query<T>(query.Connection, CreateDapperCommand(query, commandFlags));
        }

        /// <summary>
        /// Renders the given query and executes it using Dapper.SqlMapper.QueryAsync().
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        public static Task<IEnumerable<T>> QueryAsync<T>(this IDbQueryBuilder query, CommandFlags commandFlags = CommandFlags.Buffered)
        {
            return Dapper.SqlMapper.QueryAsync<T>(query.Connection, CreateDapperCommand(query, commandFlags));
        }

    }

}