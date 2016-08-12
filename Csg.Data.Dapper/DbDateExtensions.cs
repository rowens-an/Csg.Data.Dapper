using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Csg.Data
{
    public static class DbDateExtensions
    {
        public static DbDate<DateTime> ToDbDate(this DateTime date, DbDateType type)
        {
            return new DbDate<DateTime>()
            {
                DateTimeType = type,
                Value = date
            };
        }

        public static DbDate<DateTimeOffset> ToDbDate(this DateTimeOffset date, DbDateType type)
        {
            return new DbDate<DateTimeOffset>()
            {
                DateTimeType = type,
                Value = date
            };
        }
    }
}
