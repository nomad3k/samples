using System;

namespace CheeseShop.Common
{
    public class DateUtil
    {
        /// <summary>
        /// Have any calls to Now() or Today() return fixed values,
        /// to help with testing.
        /// </summary>
        /// <param name="dateTime"></param>
        public static void FixForTesting(DateTime? dateTime = null)
        {
            var dt = dateTime ?? DateTime.Now;
            Now = () => dt;
            Today = () => dt.Date;
        }

        public static Func<DateTime> Now = () => DateTime.Now;
        public static Func<DateTime> Today = () => DateTime.Today;
    }
}