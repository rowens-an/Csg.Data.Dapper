using Csg.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csg.Data
{
    /// <summary>
    /// Extension methods for using Dapper with a <see cref="Csg.Data.DbScope"/.>
    /// </summary>
    public static class DapperDbScopeExtensions
    {
        /// <summary>
        /// Executes the given query text against the connection associated with <see cref="DbScope"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="scope"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IEnumerable<T> QueryMap<T>(this DbScope scope, string commandText,
            System.Data.CommandType commandType = System.Data.CommandType.Text,
            int? commandTimeout = null,
            object parameters = null
)
        {
            var cmd = new Dapper.CommandDefinition(commandText,
                commandType: commandType,
                parameters: parameters,
                transaction: scope.Transaction,
                commandTimeout: commandTimeout
            );

            return Dapper.SqlMapper.Query<T>(scope.Connection, cmd);
        }

        /// <summary>
        /// Executes the given query text against the connection associated with <see cref="DbScope"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="scope"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> QueryMapAsync<T>(this DbScope scope, string commandText,
            System.Data.CommandType commandType = System.Data.CommandType.Text,
            int? commandTimeout = null,
            object parameters = null
        )
        {
            var cmd = new Dapper.CommandDefinition(commandText,
                commandType: commandType,
                parameters: parameters,
                transaction: scope.Transaction,
                commandTimeout: commandTimeout
            );

            return await Dapper.SqlMapper.QueryAsync<T>(scope.Connection, cmd);
        }

        /// <summary>
        /// Executes the given command text against the connection associated with <see cref="DbScope"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="scope"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int Execute<T>(this DbScope scope, string commandText,
            System.Data.CommandType commandType = System.Data.CommandType.Text,
            int? commandTimeout = null,
            object parameters = null
)
        {
            var cmd = new Dapper.CommandDefinition(commandText,
                commandType: commandType,
                parameters: parameters,
                transaction: scope.Transaction,
                commandTimeout: commandTimeout
            );

            return Dapper.SqlMapper.Execute(scope.Connection, cmd);
        }
    }
}
