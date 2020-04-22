using System;
using System.Globalization;

namespace DPool.Utility
{
    /// <summary>时间工具类
    /// </summary>
    public static class DateTimeUtil
    {

        /// <summary>将时间转换成int32类型时间戳(从1970-01-01 00:00:00 开始计算)
        /// </summary>
        /// <param name="datetime">时间</param>
        /// <returns></returns>
        public static int ToInt32(DateTime datetime)
        {
            //默认情况下以1970.01.01为开始时间计算
            var timeSpan = datetime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt32(timeSpan.TotalSeconds);
        }

        /// <summary>将时间转换成long类型时间戳,以毫秒为单位(从1970-01-01 00:00:00 开始计算)
        /// </summary>
        /// <param name="datetime">时间</param>
        /// <returns></returns>
        public static long ToInt64(DateTime datetime)
        {
            //默认情况下以1970.01.01为开始时间计算
            var timeSpan = datetime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64(timeSpan.TotalMilliseconds);
        }

        /// <summary>将string类型的时间转换成int32
        /// </summary>
        /// <param name="datetime">string类型时间</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int ToInt32(string datetime, int defaultValue = 0)
        {
            if (!IsDataTime(datetime))
            {
                return defaultValue;
            }
            var end = Convert.ToDateTime(datetime);
            return ToInt32(end);
        }

        /// <summary>将int32类型的整数时间戳转换成时间
        /// </summary>
        /// <param name="seconds">整数时间戳(从1970-01-01 00:00:00 开始计算的总秒数)</param>
        /// <returns></returns>
        public static DateTime ToDateTime(int seconds)
        {

            var begtime = Convert.ToInt64(seconds) * 10000000; //100毫微秒为单位
            var dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var tricks1970 = dt1970.Ticks; //1970年1月1日刻度
            var timeTricks = tricks1970 + begtime; //日志日期刻度
            var dt = new DateTime(timeTricks, DateTimeKind.Utc); //转化为DateTime
            return dt;
        }

        /// <summary>将long类型的整数时间(以毫秒为单位)转换成时间
        /// </summary>
        /// <param name="millSeconds">long类型时间戳(从1970-01-01 00:00:00 开始计算的总毫秒数)</param>
        /// <returns></returns>
        public static DateTime ToDateTime(long millSeconds)
        {
            var begtime = millSeconds * 10000; //100毫微秒为单位
            var dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var tricks1970 = dt1970.Ticks; //1970年1月1日刻度
            var timeTricks = tricks1970 + begtime; //日志日期刻度
            var dt = new DateTime(timeTricks, DateTimeKind.Utc); //转化为DateTime
            //DateTime enddt = dt.Date;//获取到日期整数
            return dt;
        }

        /// <summary>获取String类型的时间拼接,拼接到天
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public static string GetPadDay(DateTime time)
        {
            var month = time.Month.ToString().PadLeft(2, '0');
            var day = time.Day.ToString().PadLeft(2, '0');
            var pad = $"{time.Year}{month}{day}";
            return pad;
        }

        /// <summary>获取string类型拼接的时间 拼接到秒
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public static string GetPadSecond(DateTime time)
        {
            var month = time.Month.ToString().PadLeft(2, '0');
            var day = time.Day.ToString().PadLeft(2, '0');
            var hour = time.Hour.ToString().PadLeft(2, '0');
            var minute = time.Minute.ToString().PadLeft(2, '0');
            var second = time.Second.ToString().PadLeft(2, '0');
            var pad = $"{time.Year}{month}{day}{hour}{minute}{second}";
            return pad;
        }

        /// <summary>获取string类型拼接的时间,拼接到秒,但是不包括最早的2位
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public static string GetPadSecondWithoutPrefix(DateTime time)
        {
            var month = time.Month.ToString().PadLeft(2, '0');
            var day = time.Day.ToString().PadLeft(2, '0');
            var hour = time.Hour.ToString().PadLeft(2, '0');
            var minute = time.Minute.ToString().PadLeft(2, '0');
            var second = time.Second.ToString().PadLeft(2, '0');
            var pad = $"{time.Year.ToString().Substring(2)}{month}{day}{hour}{minute}{second}";
            return pad;
        }

        /// <summary>获取string类型拼接的时间,拼接到毫秒
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public static string GetPadMillSecond(DateTime time)
        {
            var month = time.Month.ToString().PadLeft(2, '0');
            var day = time.Day.ToString().PadLeft(2, '0');
            var hour = time.Hour.ToString().PadLeft(2, '0');
            var minute = time.Minute.ToString().PadLeft(2, '0');
            var second = time.Second.ToString().PadLeft(2, '0');
            var minSecond = time.Millisecond.ToString().PadLeft(3, '0');
            var pad = $"{time.Year}{month}{day}{hour}{minute}{second}{minSecond}";
            return pad;
        }

        /// <summary>获取string类型拼接的时间,拼接到秒,但是不包括最早的2位,精确到毫秒
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public static string GetPadMillSecondWithoutPrefix(DateTime time)
        {
            var month = time.Month.ToString().PadLeft(2, '0');
            var day = time.Day.ToString().PadLeft(2, '0');
            var hour = time.Hour.ToString().PadLeft(2, '0');
            var minute = time.Minute.ToString().PadLeft(2, '0');
            var second = time.Second.ToString().PadLeft(2, '0');
            var minSecond = time.Millisecond.ToString().PadLeft(3, '0');
            var pad = $"{time.Year.ToString().Substring(2)}{month}{day}{hour}{minute}{second}{minSecond}";
            return pad;
        }

        /// <summary>是否为有效的日期时间
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static bool IsDataTime(string source)
        {
            DateTime d;
            return !string.IsNullOrWhiteSpace(source) && DateTime.TryParse(source, CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
        }

    }
}
