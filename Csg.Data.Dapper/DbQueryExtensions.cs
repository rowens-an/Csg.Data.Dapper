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
        public static IEnumerable<T> ExecuteMap<T>(this IDbQueryBuilder query)
        {
            var stmt = query.Render();
            var parameters = new Dapper.DynamicParameters();
            var cmd = new CommandDefinition(stmt.CommandText,
                commandType: System.Data.CommandType.Text,
                parameters: parameters,
                transaction: query.Transaction,
                commandTimeout: query.CommandTimeout
            );

            foreach (var param in stmt.Parameters)
            {
                parameters.Add(param.ParameterName, param.Value, param.DbType, System.Data.ParameterDirection.Input, param.Size > 0 ? (int?)param.Size : null);
            }

            return Dapper.SqlMapper.Query<T>(query.Connection, cmd);
        }

        public static Task<IEnumerable<T>> ExecuteMapAsync<T>(this IDbQueryBuilder query)
        {
            var stmt = query.Render();
            var parameters = new Dapper.DynamicParameters();
            var cmd = new CommandDefinition(stmt.CommandText,
                commandType: System.Data.CommandType.Text,
                parameters: parameters,
                transaction: query.Transaction,
                commandTimeout: query.CommandTimeout
            );

            foreach (var param in stmt.Parameters)
            {
                parameters.Add(param.ParameterName, param.Value, param.DbType, System.Data.ParameterDirection.Input, param.Size > 0 ? (int?)param.Size : null);
            }

            return Dapper.SqlMapper.QueryAsync<T>(query.Connection, cmd);
        }

        public static IDbQueryWhereClause FieldEquals(this IDbQueryWhereClause query, string fieldName, DbString equalsValue)
        {
            return query.FieldMatch(fieldName, SqlOperator.Equal, equalsValue);
        }

        public static IDbQueryWhereClause FieldMatch(this IDbQueryWhereClause query, string fieldName, Csg.Data.Sql.SqlOperator @operator, DbString value)
        {
            var filter = new Csg.Data.Sql.SqlCompareFilter<string>(query.Root, fieldName, @operator, value.Value);
                        
            if (value.IsAnsi && value.IsFixedLength)
            {
                filter.DataType = DbType.AnsiStringFixedLength;
            }
            else if (value.IsAnsi)
            {
                filter.DataType = DbType.AnsiString;
            }
            else if (value.IsFixedLength)
            {
                filter.DataType = DbType.StringFixedLength;
            }
            else
            {
                filter.DataType = DbType.String;
            }

            if (value.Length >= 0)
            {
                filter.Size = value.Length;
            }

            query.AddFilter(filter);

            return query;
        }

        public static IDbQueryWhereClause StringMatch(this IDbQueryWhereClause query, string fieldName, Csg.Data.Sql.SqlWildcardDecoration @operator, DbString value)
        {
            var filter = new Csg.Data.Sql.SqlStringMatchFilter(query.Root, fieldName, @operator, value.Value);

            if (value.IsAnsi && value.IsFixedLength)
            {
                filter.DataType = DbType.AnsiStringFixedLength;
            }
            else if (value.IsAnsi)
            {
                filter.DataType = DbType.AnsiString;
            }
            else if (value.IsFixedLength)
            {
                filter.DataType = DbType.StringFixedLength;
            }
            else
            {
                filter.DataType = DbType.String;
            }

            if (value.Length >= 0)
            {
                filter.Size = value.Length;
            }

            query.AddFilter(filter);

            return query;
        }

        public static IDbQueryWhereClause FieldMatch<T>(this IDbQueryWhereClause query, string fieldName, SqlOperator @operator, DbDate<T> date) where T: struct
        {
            query.AddFilter(new Csg.Data.Sql.SqlCompareFilter(query.Root, fieldName, @operator, date.GetDbType(), date.Value));

            return query;
        }

        public static IDbQueryWhereClause FieldBetween<T>(this IDbQueryWhereClause query, string fieldName, DbDate<T> begin, DbDate<T> end) where T : struct
        {
            query.FieldMatch(fieldName, SqlOperator.GreaterThanOrEqual, begin).FieldMatch(fieldName, SqlOperator.LessThanOrEqual, end);

            return query;
        }
       
    }

}