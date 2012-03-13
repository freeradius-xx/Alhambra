﻿using System;
using System.Data.SqlTypes;
using System.Globalization;

namespace Ledsun.Alhambra.Db.Data
{
    static class To
    {
        /// <summary>
        /// 値を日付に変換する
        /// </summary>
        /// <param name="val">変換元の値</param>
        /// <returns>nullの場合は0相当の日付値、それ以外は変換された値</returns>
        internal static DateTime DateTime(object val)
        {
            return IsNull(val) ? TypeConvertableWrapper.DateTimeDefault
                : val is SqlDateTime ? ((SqlDateTime)val).Value
                : Convert.ToDateTime(val);
        }

        /// <summary>
        /// 値を日付に変換する
        /// </summary>
        /// <param name="val">変換元の値</param>
        /// <returns>nullの場合は0相当の日付値、それ以外は変換された値</returns>
        internal static DateTime? DateTimeNull(object val)
        {
            return IsNull(val) ? null
                : val is SqlDateTime ? new DateTime?(((SqlDateTime)val).Value)
                : new DateTime?(Convert.ToDateTime(val));
        }

        /// <summary>
        /// 値を数値に変換する
        /// </summary>
        /// <param name="val">変換元の値</param>
        /// <returns>nullの場合は0、それ以外は変換された数値</returns>
        internal static Decimal Decimal(object val)
        {
            return IsNull(val) ? 0 : Convert.ToDecimal(val, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// 値を浮動少数値に変換する
        /// </summary>
        /// <param name="val">変換元の値</param>
        /// <returns>nullの場合は0、それ以外は変換された値</returns>
        internal static double Double(object val)
        {
            return IsNull(val) ? 0 : Convert.ToDouble(val);
        }

        /// <summary>
        /// 値を正数に変換する
        /// </summary>
        /// <param name="val">変換元の値</param>
        /// <returns>nullの場合は0、それ以外は変換された数値</returns>
        internal static uint UInt(object val)
        {
            return IsNull(val) ? 0 : Convert.ToUInt32(val, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// 値を数値に変換する
        /// </summary>
        /// <param name="val">変換元の値</param>
        /// <returns>nullの場合は0、それ以外は変換された数値</returns>
        internal static int Int(object val)
        {
            return IsNull(val) ? 0 : Convert.ToInt32(val, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// 値を数値に変換する
        /// </summary>
        /// <param name="val">変換元の値</param>
        /// <returns>nullの場合は0、それ以外は変換された数値</returns>
        internal static System.Int16 Int16(object val)
        {
            return IsNull(val) ? (Int16)0 : Convert.ToInt16(val, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// 値を数値に変換する
        /// </summary>
        /// <param name="val">変換元の値</param>
        /// <returns>nullの場合は0、それ以外は変換された数値</returns>
        internal static System.Int64 Int64(object val)
        {
            return IsNull(val) ? (Int64)0 : Convert.ToInt64(val, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// 値を数値に変換する
        /// </summary>
        /// <param name="val">変換元の値</param>
        /// <returns>nullの場合は0、それ以外は変換された数値</returns>
        internal static System.Byte Byte(object val)
        {
            return IsNull(val) ? (Byte)0 : Convert.ToByte(val, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// 値を文字列に変換する
        /// </summary>
        /// <param name="val">変換元の値</param>
        /// <returns>nullの場合は空文字、それ以外は変換された文字</returns>
        internal static string String(object val)
        {
            return IsNull(val) ? "" : val.ToString();
        }

        internal static bool Bool(object val)
        {
            return IsNull(val) ? false : Convert.ToBoolean(val);
        }

        /// <summary>
        /// オブジェクトがNULLを表すものかどうかを返す
        /// </summary>
        /// <param name="val">判定する値</param>
        /// <returns>true:null false:null以外</returns>
        private static bool IsNull(object val)
        {
            return (null == val || val is DBNull);
        }
    }
}
