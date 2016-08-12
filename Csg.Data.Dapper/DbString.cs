using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csg.Data
{
    public sealed class DbString : Dapper.SqlMapper.ICustomQueryParameter, Csg.Data.IDbTypeProvider
    {
        /// <summary>
        /// Default value for IsAnsi.
        /// </summary>
        public static bool IsAnsiDefault
        {
            get; set;
        }

        /// <summary>
        /// A value to set the default value of strings
        /// going through Dapper. Default is 4000, any value larger than this
        /// field will not have the default value applied.
        /// </summary>
        public const int DefaultLength = Dapper.DbString.DefaultLength;

        /// <summary>
        /// Create a new DbString
        /// </summary>
        public DbString()
        {
            Length = -1;
            IsAnsi = IsAnsiDefault;
        }

        /// <summary>
        /// Ansi vs Unicode 
        /// </summary>
        public bool IsAnsi { get; set; }

        /// <summary>
        /// Fixed length 
        /// </summary>
        public bool IsFixedLength { get; set; }

        /// <summary>
        /// Length of the string -1 for max
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// The value of the string
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Add the parameter to the command... internal use only
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        public void AddParameter(IDbCommand command, string name)
        {
            if (IsFixedLength && Length == -1)
            {
                throw new InvalidOperationException("If specifying IsFixedLength,  a Length must also be specified");
            }

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

            if (this.Length == -1 && this.Value != null && this.Value.Length <= DefaultLength)
            {
                param.Size = DefaultLength;
            }
            else
            {
                param.Size = this.Length;
            }

            param.DbType = this.IsAnsi ? (this.IsFixedLength ? DbType.AnsiStringFixedLength : DbType.AnsiString) : (this.IsFixedLength ? DbType.StringFixedLength : DbType.String);

            command.Parameters.Add(param);
        }

        DbType IDbTypeProvider.GetDbType()
        {
            return this.IsAnsi ? (this.IsFixedLength ? DbType.AnsiStringFixedLength : DbType.AnsiString) : (this.IsFixedLength ? DbType.StringFixedLength : DbType.String);
        }
    }
}
