using Csg.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csg.Data
{
    public static class DapperDbScopeExtensions
    {
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
