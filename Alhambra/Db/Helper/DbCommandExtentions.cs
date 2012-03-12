using System;
using System.Collections.Generic;
using System.Data;
using Ledsun.Alhambra.Db.Data;

namespace Ledsun.Alhambra.Db.Helper
{
    static class DBCommandExtentions
    {
        /// <summary>
        /// SQL�����s���܂��B
        /// �e���̂������l��Ԃ��܂��B
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        internal static int Execute(this IDbCommand cmd)
        {
            try
            {
                return cmd.ExecuteNonQuery();
            }
            catch (SystemException e)
            {

                throw new DBHelperException(e, cmd);
            }
        }

        /// <summary>
        /// SQL�����s���Ĉ�̒l��Ԃ��܂��B
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        internal static TypeConvertableWrapper SelectOne(this IDbCommand cmd)
        {
            try
            {
                return new TypeConvertableWrapper(cmd.ExecuteScalar());
            }
            catch (SystemException e)
            {
                throw new DBHelperException(e, cmd);
            }
        }


    }
}
