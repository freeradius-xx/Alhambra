using System;
using System.Collections.Generic;
using System.Data;

using NUnit.Framework;

namespace Ledsun.Alhambra.Db
{
    /// <summary>
    /// DB�A�N�Z�X�p�̃��[�e�B���e�B�N���X�ł��B
    /// ���s�ɂ�config�t�@�C����connectionString��DBHelper�Ƃ������O�Őڑ��������ݒ肷��K�v������܂��B
    /// SQL�̐����ɂ�SqlStatement�̎g�p��z�肵�Ă��܂����A�����̕�����ł����s�\�ł��B
    /// </summary>
    /// <example>
    /// int value =DBHelper.Select(new SqlStatement(@"
    ///              SELECT
    ///                  VALE
    ///              FROM EXAMPLE_TABLE
    ///              WHERE ID = @ID@
    ///              ")
    ///             .Replace("ID", 100)
    ///             )[0]["VALUE"].Int;
    /// </example>
    public class DBHelper
    {
        /// <summary>
        /// UPDATE,DELETE�Ȃǂ̌��ʂ�Ԃ��Ȃ�SQL�����s���܂��B
        /// </summary>
        /// <param name="sql">���s����SQL������</param>
        public static int Execute(string sql, DBHelperWithTransaction tran = null)
        {
            return tran == null ? Execute(sql) : tran.Execute(sql);
        }

        public static List<DataRowAccessor> Select(string sql, DBHelperWithTransaction tran = null)
        {
            return tran == null ? Select(sql) : tran.Select(sql);
        }

        public static TypeConvertableWrapper SelectOne(string sql, DBHelperWithTransaction tran = null)
        {
            return tran == null ? SelectOne(sql) : tran.SelectOne(sql);
        }

        public static DataSet SelectDataSet(string sql, DBHelperWithTransaction tran = null)
        {
            return tran == null ? SelectDataSet(sql) : tran.SelectDataSet(sql);
        }

        private static int Execute(string sql)
        {
            using (var db = new DBBridgeForSqlServer())
                return db.Execute(sql);
        }

        private static List<DataRowAccessor> Select(string sql)
        {
            using (var db = new DBBridgeForSqlServer())
                return db.Select(sql);
        }

        private static TypeConvertableWrapper SelectOne(string sql)
        {
            using (var db = new DBBridgeForSqlServer())
                return db.SelectOne(sql);
        }

        private static DataSet SelectDataSet(string sql)
        {
            using (var db = new DBBridgeForSqlServer())
                return db.SelectDataSet(sql);
        }

        #region TEST
        [TestFixture]
        public class Test
        {
            [Test]
            public void SelectDataSet()
            {
                var ds = DBHelper.SelectDataSet("SELECT 3");
                Assert.That((int)ds.Tables[0].Rows[0][0], Is.EqualTo(3));
            }

            [Test]
            public void SelectOne��DBNull�̎���0���Ԃ�()
            {
                var t = DBHelper.SelectOne("SELECT ID FROM ( SELECT 1 ID ) A WHERE ID = 0");
                Assert.That(t.Int, Is.EqualTo(0));
            }

            [Test]
            [ExpectedException(typeof(ApplicationException))]
            public void SelectOne�ŗ�O���������Ƃ���ApplicationException���Ԃ�()
            {
                DBHelper.SelectOne("x");
            }

            #region �󕶎��܂���null�̓��͂͋֎~
            [Test]
            [ExpectedException(typeof(ArgumentException))]
            public void EmptySelect()
            {
                DBHelper.Select("");
            }

            [Test]
            [ExpectedException(typeof(ArgumentException))]
            public void NullSelect()
            {
                DBHelper.Select(null);
            }

            [Test]
            [ExpectedException(typeof(ArgumentException))]
            public void EmptySelectOne()
            {
                DBHelper.SelectOne("");
            }

            [Test]
            [ExpectedException(typeof(ArgumentException))]
            public void NullSelectOne()
            {
                DBHelper.SelectOne(null);
            }

            [Test]
            [ExpectedException(typeof(ArgumentException))]
            public void EmptySelectDataSet()
            {
                DBHelper.SelectDataSet("");
            }

            [Test]
            [ExpectedException(typeof(ArgumentException))]
            public void NullSelectDataSet()
            {
                DBHelper.SelectDataSet(null);
            }

            [Test]
            [ExpectedException(typeof(ArgumentException))]
            public void EmptyExecute()
            {
                DBHelper.Execute("");
            }

            [Test]
            [ExpectedException(typeof(ArgumentException))]
            public void NullExecute()
            {
                DBHelper.Execute(null);
            }
            #endregion
        }
        #endregion
    }
}