using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Csg.Data
{
    /// <summary>
    /// Represents the database data type of a date/time field.
    /// </summary>
    public enum DbDateType
    {
        Date,
        DateTime,
        DateTime2,
        DateTimeOffset
    }
}
