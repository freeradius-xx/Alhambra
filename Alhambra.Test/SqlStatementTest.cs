﻿using Ledsun.Alhambra.Db;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AlhambraTest
{
    /// <summary>
    ///SqlStatementのテストクラス
    ///</summary>
    [TestClass()]
    public class SqlStatementTest
    {
        [TestMethod]
        public void 単純置換()
        {
            Assert.AreEqual<string>("N'FUGA'", new SqlStatement("@HOGE@").Replace("HOGE", "FUGA"));
            Assert.AreEqual<string>("1", new SqlStatement("@HOGE@").Replace("HOGE", true));
            Assert.AreEqual<string>("1", new SqlStatement("@HOGE@").Replace("HOGE", 1));
            Assert.AreEqual<string>("1.0", new SqlStatement("@HOGE@").Replace("HOGE", 1.0m));
            Assert.AreEqual<string>("'2012/03/12 00:00:00'", new SqlStatement("@HOGE@").Replace("HOGE", new DateTime(2012, 3, 12)));
        }

        [TestMethod]
        public void ヌル許容型の置換()
        {
            bool? boo = null;
            Assert.AreEqual<string>("NULL", new SqlStatement("@HOGE@").Replace("HOGE", boo));
            boo = true;
            Assert.AreEqual<string>("1", new SqlStatement("@HOGE@").Replace("HOGE", boo));
            boo = false;
            Assert.AreEqual<string>("0", new SqlStatement("@HOGE@").Replace("HOGE", boo));

            int? intn = null;
            Assert.AreEqual<string>("NULL", new SqlStatement("@HOGE@").Replace("HOGE", intn));
            decimal? deci = null;
            Assert.AreEqual<string>("NULL", new SqlStatement("@HOGE@").Replace("HOGE", deci));
            DateTime? date = null;
            Assert.AreEqual<string>("NULL", new SqlStatement("@HOGE@").Replace("HOGE", date));
        }

        [TestMethod]
        public void 配列の置換()
        {
            //文字列
            var moto = new string[] { "aaa", "bbb", "ccc" };
            Assert.AreEqual<string>("WHERE HAGE IN (N'aaa',N'bbb',N'ccc')", new SqlStatement("WHERE HAGE IN @HAGES@").ReplaceIn("HAGES", moto));

            //数値
            var moto1 = new int[] { 1, 2, 3 };
            Assert.AreEqual<string>("WHERE HAGE IN (1,2,3)", new SqlStatement("WHERE HAGE IN @HAGES@").ReplaceIn("HAGES", moto1));

            //日付
            var moto3 = new DateTime[] { new DateTime(2001, 1, 1), new DateTime(2001, 2, 1), new DateTime(2001, 12, 31) };
            Assert.AreEqual<string>("WHERE HAGE IN ('2001/01/01 00:00:00','2001/02/01 00:00:00','2001/12/31 00:00:00')", new SqlStatement("WHERE HAGE IN @HAGES@").ReplaceIn("HAGES", moto3));

        }

        [TestMethod]
        public void 部分一致置換()
        {
            Assert.AreEqual<string>("'%FUGA%'", new SqlStatement("@HOGE@").ReplaceForPartialMatchRetrieval("HOGE", "FUGA"));
        }

        [TestMethod]
        public void シングルクォートでくくらない文字列置換()
        {
            Assert.AreEqual<string>("FUGA", new SqlStatement("@HOGE@").ReplaceStripString("HOGE", "FUGA"));
        }

        [TestMethod]
        public void ToStringのオーバーライド()
        {
            Assert.AreEqual<string>("SELECT 1", new SqlStatement("SELECT 1").ToString());
            //暗黙的な文字列変換と同じ結果を返します。
            Assert.AreEqual<string>(new SqlStatement("SELECT 1"), new SqlStatement("SELECT 1").ToString());
        }
    }
}
