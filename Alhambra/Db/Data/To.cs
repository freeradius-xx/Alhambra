﻿using System;
using System.Data.SqlTypes;
using System.Globalization;
using NUnit.Framework;

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

        #region test
        [TestFixture]
        public class Test
        {
            #region DateTime
            [Test]
            public void DateTime()
            {
                Assert.That(To.DateTime(null), Is.EqualTo(System.DateTime.Parse("1/1/1753 12:00:00")));
                Assert.That(To.DateTime("100.1"), Is.EqualTo(new DateTime(100, 1, 1)));
                Assert.That(To.DateTime(new DateTime(2009, 4, 7)), Is.EqualTo(new DateTime(2009, 4, 7)));

                Assert.That(To.DateTime("2009/04/07 0:00:00"), Is.EqualTo(new DateTime(2009, 4, 7)));
                Assert.That(To.DateTime(new SqlDateTime(2009, 4, 7)), Is.EqualTo(new DateTime(2009, 4, 7)));
            }

            [Test]
            [ExpectedException(typeof(InvalidCastException))]
            public void 整数はDateTime変換しない()
            {
                To.DateTime(100);
            }

            [Test]
            [ExpectedException(typeof(InvalidCastException))]
            public void 少数はDateTime変換しない()
            {
                To.DateTime(100.1);
            }

            [Test]
            [ExpectedException(typeof(FormatException))]
            public void 整数文字列はDateTime変換しない()
            {
                To.DateTime("100");
            }

            [Test]
            [ExpectedException(typeof(FormatException))]
            public void 空文字列はDateTime変換しない()
            {
                To.DateTime("");
            }

            [Test]
            [ExpectedException(typeof(FormatException))]
            public void アルファベット文字列はDateTime変換しない()
            {
                To.DateTime("ABC");
            }
            #endregion

            #region Decimal
            [Test]
            public void Decimal()
            {
                Assert.That(To.Decimal(null), Is.EqualTo(0));
                Assert.That(To.Decimal(100), Is.EqualTo(100));
                Assert.That(To.Decimal(100.1), Is.EqualTo(100.1));
                Assert.That(To.Decimal("100"), Is.EqualTo(100));
                Assert.That(To.Decimal("100.1"), Is.EqualTo(100.1));
            }

            [Test]
            [ExpectedException(typeof(FormatException))]
            public void 空文字はDecimal変換しない()
            {
                To.Decimal("");
            }

            [Test]
            [ExpectedException(typeof(FormatException))]
            public void アルファベットはDecimalに変換しない()
            {
                To.Decimal("ABC");
            }

            [Test]
            [ExpectedException(typeof(InvalidCastException))]
            public void DateTimeはDecimalに変換しない()
            {
                To.Decimal(new DateTime());
            }
            #endregion Decimal

            #region Double
            [Test]
            public void Double()
            {
                Assert.That(To.Double(null), Is.EqualTo(0));
                Assert.That(To.Double(100), Is.EqualTo(100));
                Assert.That(To.Double(100.1), Is.EqualTo(100.1));
                Assert.That(To.Double("100"), Is.EqualTo(100));
                Assert.That(To.Double("100.1"), Is.EqualTo(100.1));
            }

            [Test]
            [ExpectedException(typeof(FormatException))]
            public void 空文字はDoubleに変換しない()
            {
                To.Double("");
            }

            [Test]
            [ExpectedException(typeof(FormatException))]
            public void アルファベットはDoubleに変換しない()
            {
                To.Double("ABC");
            }

            [Test]
            [ExpectedException(typeof(InvalidCastException))]
            public void DateTimeはDoubleに変換しない()
            {
                To.Double(new DateTime());
            }
            #endregion

            #region UInt
            [Test]
            public void UInt()
            {
                Assert.That(To.UInt(null), Is.EqualTo(0));
                Assert.That(To.UInt(100), Is.EqualTo(100));
                Assert.That(To.UInt(100.1), Is.EqualTo(100));
                Assert.That(To.UInt("100"), Is.EqualTo(100));
            }

            [Test]
            [ExpectedException(typeof(OverflowException))]
            public void 負数はUIntに変換しない()
            {
                To.UInt(-100);
            }

            [Test]
            [ExpectedException(typeof(FormatException))]
            public void アルファベットはUIntに変換しない()
            {
                To.UInt("ABC");
            }

            [Test]
            [ExpectedException(typeof(InvalidCastException))]
            public void DateTimeはUIntに変換しない()
            {
                To.UInt(new DateTime());
            }

            [Test]
            [ExpectedException(typeof(FormatException))]
            public void 空文字はUIntに変換しない()
            {
                To.UInt("");
            }

            [Test]
            [ExpectedException(typeof(FormatException))]
            public void 小数点付き文字列はUIntに変換しない()
            {
                To.UInt("100.1");
            }
            #endregion

            #region Int
            [Test]
            public void Int()
            {
                Assert.That(To.Int(null), Is.EqualTo(0));
                Assert.That(To.Int(100), Is.EqualTo(100));
                Assert.That(To.Int(100.1), Is.EqualTo(100));
                Assert.That(To.Int("100"), Is.EqualTo(100));
            }

            [Test]
            [ExpectedException(typeof(FormatException))]
            public void アルファベットはIntに変換しない()
            {
                To.Int("ABC");
            }

            [Test]
            [ExpectedException(typeof(InvalidCastException))]
            public void DateTimeはIntに変換しない()
            {
                To.Int(new DateTime());
            }

            [Test]
            [ExpectedException(typeof(FormatException))]
            public void 空文字はIntに変換しない()
            {
                To.Int("");
            }

            [Test]
            [ExpectedException(typeof(FormatException))]
            public void 小数点付き文字列はIntに変換しない()
            {
                To.Int("100.1");
            }
            #endregion

            [Test]
            public void String()
            {
                Assert.That(To.String(null), Is.EqualTo(""));
                Assert.That(To.String(100), Is.EqualTo("100"));
                Assert.That(To.String(100.1), Is.EqualTo("100.1"));
                Assert.That(To.String("100"), Is.EqualTo("100"));
                Assert.That(To.String("100.1"), Is.EqualTo("100.1"));
                Assert.That(To.String(""), Is.EqualTo(""));
                Assert.That(To.String("ABC"), Is.EqualTo("ABC"));
                Assert.That(To.String(new DateTime(2009, 4, 7)), Is.EqualTo("2009/04/07 0:00:00"));
            }

            [Test]
            public void Bool()
            {
                Assert.That(To.Bool(null), Is.False);
                Assert.That(To.Bool(-1), Is.True);
                Assert.That(To.Bool(0), Is.False);
                Assert.That(To.Bool(1), Is.True);
                Assert.That(To.Bool(100), Is.True);
                Assert.That(To.Bool("TRue"), Is.True);
                Assert.That(To.Bool("falSe"), Is.False);
                Assert.That(To.Bool(true), Is.True);
                Assert.That(To.Bool(false), Is.False);
            }

            [Test]
            [ExpectedException(typeof(FormatException))]
            public void 真理値変換できない文字列()
            {
                To.Bool("x");
            }

            [Test]
            public void IsNull()
            {
                Assert.That(To.IsNull(null), Is.True);
                Assert.That(To.IsNull(DBNull.Value), Is.True);
            }
        }
        #endregion
    }
}
