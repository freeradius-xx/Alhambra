using System.Collections.Generic;
using System.Data;
using Ledsun.Alhambra.Db.Data;
using Ledsun.Alhambra.Db.Plugin;

namespace Ledsun.Alhambra.Db.Helper
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
            return tran == null
                ? DBFactory.NewDB.Execute(sql)
                : tran.DB.Execute(sql);
        }

        public static IEnumerable<DataRowAccessor> Select(string sql, DBTran tran = null)
        {
            return tran == null
                ? DBFactory.NewDB.Select(sql)
                : tran.DB.Select(sql);
        }

        public static TypeConvertableWrapper SelectOne(string sql, DBTran tran = null)
        {
            return tran == null
                ? DBFactory.NewDB.SelectOne(sql)
                : tran.DB.SelectOne(sql);
        }

        public static DataSet SelectDataSet(string sql, DBTran tran = null)
        {
            return tran == null
                ? DBFactory.NewDB.SelectDataSet(sql)
                : tran.DB.SelectDataSet(sql);
        }
    }
}