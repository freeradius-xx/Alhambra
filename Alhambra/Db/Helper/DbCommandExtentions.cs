using System;
using System.Collections.Generic;
using System.Data;
using Alhambra.Db.Data;

namespace Alhambra.Db.Helper
{
    /// <summary>
    /// IDbCommand�g��
    /// ��O�����������ꍇ�̓��b�Z�[�W�Ɏ��s����SQL��ǉ����܂��B
    /// </summary>
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
