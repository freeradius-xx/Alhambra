using System.Collections.Generic;
using System.Data;
using Alhambra.Db.Data;
using Alhambra.Db.Plugin;

namespace Alhambra.Db.Helper
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
        public static int Execute(string sql, DBTran tran = null)
        {
            if (tran != null)
            {
                return tran.DB.Execute(sql);
            }

            using (var d = DBFactory.NewDB)
            {
                return d.Execute(sql);
            }
        }

        public static IEnumerable<DataRowAccessor> Select(string sql, DBTran tran = null)
        {
            if (tran != null)
            {
                return tran.DB.Select(sql);
            }

            using (var d = DBFactory.NewDB)
            {
                return d.Select(sql);
            }
        }

        public static TypeConvertableWrapper SelectOne(string sql, DBTran tran = null)
        {
            if (tran != null)
            {
                return tran.DB.SelectOne(sql);
            }

            using (var d = DBFactory.NewDB)
            {
                return d.SelectOne(sql);
            }
        }

        public static DataSet SelectDataSet(string sql, DBTran tran = null)
        {
            if (tran != null)
            {
                return tran.DB.SelectDataSet(sql);
            }

            using (var d = DBFactory.NewDB)
            {
                return d.SelectDataSet(sql);
            }
        }

        public static DataTable SelectTableSchema(string tableName)
        {
            using (var d = DBFactory.NewDB)
            {
                return d.SelectTableSchema(tableName);
            }
        }
    }
}