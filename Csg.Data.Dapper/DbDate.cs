using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csg.Data
{


    /// <summary>
    /// Represents a date and/or time value and it's associated database data type.
    /// </summary>
    public sealed class DbDate<TValue> : Dapper.SqlMapper.ICustomQueryParameter, Csg.Data.IDbTypeProvider where TValue: struct
    {
        public static DbDateType DefaultDateType = DbDateType.DateTime;

        /// <summary>
        /// Gets or sets the database data type.
        /// </summary>
        public DbDateType DateTimeType { get; set; }

        /// <summary>
        /// Create a new DbDate
        /// </summary>
        public DbDate()
        {
            this.DateTimeType = DefaultDateType;
        }

        /// <summary>
        /// Gets or sets the date/time value.
        /// </summary>
        public TValue? Value { get; set; }

        /// <summary>
        /// Adds the parameter to the command object using this is as a parameter value.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        public void AddParameter(IDbCommand command, string name)
        {
            var param = command.CreateParameter();
            param.ParameterName = name;

            if (this.Value == null)
            {
                param.Value = DBNull.Value;
            }
            else
            {
                param.Value = this.Value;
            }

            param.DbType = this.GetDbType();

            
            command.Parameters.Add(param);
        }

        /// <summary>
        /// Gets the database data type associated with the value.
        /// </summary>
        /// <returns></returns>
        public DbType GetDbType()
        {
            if (this.DateTimeType == DbDateType.Date)
            {
                return DbType.Date;
            }
            else if (this.DateTimeType == DbDateType.DateTime2)
            {
                return DbType.DateTime2;
            }
            else if (this.DateTimeType == DbDateType.DateTimeOffset)
            {
                return DbType.DateTimeOffset;
            }
            else
            {
                throw new NotSupportedException("Unsupported date type");
            }            
        }
    }



}
