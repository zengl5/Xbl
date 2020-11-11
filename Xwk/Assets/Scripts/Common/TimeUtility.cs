using UnityEngine;
using System.Collections;
using System;

public static class TimeUtility
{
	/// <summary>
	/// 返回天数.
	/// </summary>
	public static int GetDays (int second)
	{
		TimeSpan tSpan = new TimeSpan (0, 0, second);
		return tSpan.Days;
	}

	/// <summary>
	/// 返回将秒数转换天、月、年.
	/// </summary>
	/// <returns>The time.</returns>
	/// <param name="second">Second.</param>
	public static string Second2FormatDMY (int second)
	{
		TimeSpan tSpan = new TimeSpan (0, 0, second);
		if (tSpan.Days >= 1) {
			int years = tSpan.Days / 365;
			int mouths = tSpan.Days / 30;

			if (years >= 1)
				return years.ToString () + "年";
			else if (mouths >= 1)
				return mouths.ToString () + "月";
			else
				return tSpan.Days.ToString () + "天";
		}
		return "";
	}

	/// <summary>
	/// 将10位Unix时间戳转换为距当前时间间隔
	/// </summary>
	/// <returns>The difftime from timestamp.</returns>
	/// <param name="time">Time.</param>
	public static string getDifftimeFromTimestamp (uint time)
	{
		DateTime t1 = DateTime.Parse ("1970-01-01 00:00:00") + new TimeSpan (0, 0, (int)time);
		DateTime t2 = TimeZone.CurrentTimeZone.ToLocalTime (t1);
		TimeSpan subTime = DateTime.Now - t2;
		string str;
		if (subTime.Days / 365 > 0)
			str = subTime.Days / 365 + "年前";
		else if (subTime.Days / 30 > 0)
			str = subTime.Days / 30 + "月前";
		else if (subTime.Days / 7 > 0)
			str = subTime.Days / 7 + "周前";
		else if (subTime.Days > 0)
			str = subTime.Days + "天前";
		else if (subTime.Hours > 0)
			str = subTime.Hours + "小时前";
		else
			str = "刚刚";
		return str;
	}

	/// <summary>
	/// 获取TimeSpan对象.
	/// </summary>
	public static TimeSpan GetTimeSpan (int second)
	{
		TimeSpan tSpan = new TimeSpan (0, 0, second);
		return tSpan;
	}

	public static string getStringFromTime (uint time)
	{
		DateTime t1 = DateTime.Parse ("1970-01-01 00:00:00") + new TimeSpan (0, 0, (int)time);
		DateTime t2 = TimeZone.CurrentTimeZone.ToLocalTime (t1);
		return t2.ToString ();
	}

	public static string GetDateYMDHMS ()
	{
		DateTime current = DateTime.Now;
		string ret = string.Format ("{0}{1}{2}{3}{4}{5}", current.Year, current.Month, current.Day, current.Hour, current.Minute, current.Second);
		return ret;
	}

	public static DateTime getDateTimeFromTime (uint time)
	{
		DateTime t1 = DateTime.Parse ("1970-01-01 00:00:00") + new TimeSpan (0, 0, (int)time);
		return t1;
	}

	public static uint getTimeFromString (string time)
	{
		DateTime t = DateTime.Parse (time);
		DateTime t1 = DateTime.Parse ("1970-01-01 00:00:00") - new TimeSpan (0, 0, 8 * 3600);
		TimeSpan timeSpan = t - t1;
		return (uint)timeSpan.TotalSeconds;
	}

	public static DateTime GetDateTime (string timeStamp)
	{
		DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime (new DateTime (1970, 1, 1));
		long lTime = long.Parse (timeStamp + "0000000");
		TimeSpan toNow = new TimeSpan (lTime);
		return dtStart.Add (toNow);
	}

    static DateTime TIMEBASE = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

    /// <summary>
    /// 获取unix时间戳，精确到秒
    /// </summary>
    /// <returns></returns>
    public static double GetCurrentTimestamp ()
	{
		TimeSpan span = DateTime.Now - TIMEBASE;
		return span.TotalSeconds;
	}

	/// <summary>
	/// 获取unix时间戳，精确到毫秒
	/// </summary>
	/// <returns></returns>
	public static double GetCurrentTimestampMS ()
	{
		TimeSpan span = DateTime.Now - TIMEBASE;
		return span.TotalMilliseconds;
	}

	public static int DayOfYear (int year, int month, int day)
	{
		int[] a = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
		int[] b = { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
		int i, k, days = 0;

		if (year % 4 == 0 && year % 100 != 0 || year % 4 == 0 && year % 400 == 0) {
			k = 1;
		} else {
			k = 0;
		}
		switch (k) {
		case 0:
			for (i = 0; i < month; i++) {
				days = days + a [i];
			}
			break;
		case 1:
			for (i = 0; i < month; i++) {
				days = days + b [i];
			}
			break;
		default:
			break;
		}
		return days + day;
	}

	/// <summary>
	/// 根据秒数转化为时分秒
	/// </summary>
	/// <param name="seconds">Seconds.</param>
	public static string STD (int seconds)
	{
		TimeSpan span = new TimeSpan (0, 0, 0, seconds, 1);
		if ((int)span.Hours != 0) {
			return span.Hours.ToString ().PadLeft (2, '0') + "小时"
			+ span.Minutes.ToString ().PadLeft (2, '0') + "分钟"
			+ span.Seconds.ToString ().PadLeft (2, '0') + "秒";
		} else {
			return span.Minutes.ToString ().PadLeft (2, '0') + "分钟"
			+ span.Seconds.ToString ().PadLeft (2, '0') + "秒";
		}
	}

	public static string STD_Number (int seconds)
	{
		TimeSpan span = new TimeSpan (0, 0, 0, seconds, 1);
		if ((int)span.Hours != 0) {
			return span.Hours.ToString ().PadLeft (2, '0') + ":"
			+ span.Minutes.ToString ().PadLeft (2, '0') + ":"
			+ span.Seconds.ToString ().PadLeft (2, '0');
		} else {
			return span.Minutes.ToString ().PadLeft (2, '0') + ":"
			+ span.Seconds.ToString ().PadLeft (2, '0');
		}
	}

    public static string GetHoursMinsTime()
    {
        return DateTime.Now.ToString("HH:mm") ;
    }
}
