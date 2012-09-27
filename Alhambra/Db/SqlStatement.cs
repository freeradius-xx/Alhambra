using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Alhambra.Db.SqlExtentions;

namespace Alhambra.Db
{
    /// <summary>
    /// SQL�X�e�[�g�����g���쐬���邽�߂̃N���X�ł��B
    /// @�ň͂񂾕�������A�w��̒l�ɒu�������Ă����Replace���\�b�h��񋟂��܂��B
    /// �ȉ��̂悤�ɂ���SQL��������쐬���邱�Ƃ��ł��܂��B
    /// new SqlStatement("SELECT * FROM TABLE WHERE ID = @ID@").Replace("ID", 100).ToString();
    ///[�݌v����]
    /// �l�^�p�̃��\�b�h��l�^�i�k�����e�j�Ɠ��l�̃W�F�l���b�N�ł͂Ȃ��I�[�o�[���[�h�Ŏ����������R�B
    ///  �l�^�p�̃W�F�l���b�N�ȃ��\�b�h������ƁA������ւ̈Öٕϊ������������I�u�W�F�N�g�������ɂ����Ƃ���
    ///  ������������Ɏ��I�[�o�[���[�h�ł͂Ȃ��W�F�l���b�N�̃��\�b�h���D�悵�ČĂ΂�܂��B
    ///  ���̂��ߌĂяo�����ɖ����I�ɕ�����ɕϊ�����K�v���o�Ă��܂��B
    /// </summary>
    public class SqlStatement
    {
        private readonly string _baseSql;

        /// <summary>
        /// �R���X�g���N�^
        /// ���ɂȂ镶������w�肵�܂��B
        /// </summary>
        /// <example>new SqlStatement("SELECT * FROM TABLE WHERE ID = @ID@")</example>
        /// <param name="baseSql"></param>
        public SqlStatement(string baseSql)
        {
            if (string.IsNullOrEmpty(baseSql))
            {
                throw new ArgumentException("baseSql");
            }

            _baseSql = (string)baseSql.Clone();
        }

        /// <summary>
        /// ������^�̒u��
        /// �V���O���N�H�[�g�ň݂͂܂��B
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public SqlStatement Replace(string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(oldValue))
            {
                throw new ArgumentException("oldValue");
            }

            return ReplaceByAtmark(oldValue, newValue.ToMultiByteString());
        }

        #region �l�^�̒u��
        /// <summary>
        /// �^���l�^��u�������܂�
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public SqlStatement Replace(string oldValue, bool newValue)
        {
            if (string.IsNullOrEmpty(oldValue))
            {
                throw new ArgumentException("oldValue");
            }

            return ReplaceByAtmark(oldValue, ToValue(newValue));
        }

        /// <summary>
        /// �����^��u�������܂�
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public SqlStatement Replace(string oldValue, int newValue)
        {
            if (string.IsNullOrEmpty(oldValue))
            {
                throw new ArgumentException("oldValue");
            }

            return ReplaceByAtmark(oldValue, ToValue(newValue));
        }

        /// <summary>
        /// �����_�^��u�������܂�
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public SqlStatement Replace(string oldValue, decimal newValue)
        {
            if (string.IsNullOrEmpty(oldValue))
            {
                throw new ArgumentException("oldValue");
            }

            return ReplaceByAtmark(oldValue, ToValue(newValue));
        }

        /// <summary>
        /// ���t�^��u�������܂�
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public SqlStatement Replace(string oldValue, DateTime newValue)
        {
            if (string.IsNullOrEmpty(oldValue))
            {
                throw new ArgumentException("oldValue");
            }

            return ReplaceByAtmark(oldValue, ToValue(newValue));
        }
        #endregion

        /// <summary>
        /// �l�^�i�k�����e�j��u�������܂�
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public SqlStatement Replace<T>(string oldValue, T? newValue) where T : struct
        {
            if (string.IsNullOrEmpty(oldValue))
            {
                throw new ArgumentException("oldValue");
            }

            return ReplaceByAtmark(oldValue, NullToValue(newValue));
        }

        /// <summary>
        /// IN��p�����l�u��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldValue"></param>
        /// <param name="newValues"></param>
        /// <returns></returns>
        public SqlStatement Replace<T>(string oldValue, IEnumerable<T> newValues)
        {
            if (string.IsNullOrEmpty(oldValue))
            {
                throw new ArgumentException("oldValue");
            }

            if (newValues == null)
            {
                throw new ArgumentNullException("newValues");
            }

            if (newValues.Any())
            {
                var strs = newValues.Select(val =>
                {
                    if (typeof(T) == typeof(string))
                    {
                        return ((string)(object)val).ToMultiByteString();
                    }

                    if (val == null)
                    {
                        return "NULL";
                    }

                    if (typeof(T) == typeof(bool))
                    {
                        return ((bool)(object)val).ToSqlString();
                    }

                    if (typeof(T) == typeof(DateTime))
                    {
                        return ((DateTime)(object)val).ToSqlString();
                    }

                    return val.ToString();
                });

                return ReplaceByAtmark(oldValue, "(" + string.Join(",", strs) + ")");
            }

            throw new ArgumentException("newValues ��0���ł��B");
        }

        /// <summary>
        /// ������v�u��
        /// %%�ň݂͂܂�
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public SqlStatement ReplaceForPartialMatch(string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(oldValue))
            {
                throw new ArgumentException("oldValue");
            }

            return ReplaceByAtmark(oldValue, newValue.ToPartialMatchString());
        }

        /// <summary>
        /// AND�����pReplace
        /// (�J������ LIKE '%���1%' AND �J������ LIKE '%���2%')�`���ɒu�������܂��B
        /// </summary>
        /// <param name="oldValue">�u���������s��������</param>
        /// <param name="columnName">�����Ώۂ̃J������</param>
        /// <param name="newValues">�󔒋�؂�̕��������B������0���������ꍇ��(0=0)��Ԃ��܂��B</param>
        /// <returns>()�Ŋ�����������ɒu��������̂ŁA���̂܂�OR��ƌq�����Ƃ��ł��܂��B</returns>
        public SqlStatement ReplaceMultiLike(string oldValue, string columnName, string newValues)
        {
            var parameters = Regex.Split(newValues, "\\s")
                .Where(s => s != "")
                .Select(s => columnName + " LIKE " + s.ToPartialMatchString())
                .AndJoin();

            return ReplaceByAtmark(oldValue, "(" + (parameters.Length > 0 ? parameters : "0=0") + ")");
        }

        /// <summary>
        /// �����񂾂��V���O���N�H�[�g�ň͂܂Ȃ�
        /// DB���A�e�[�u�����̒u���ɕK�v
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public SqlStatement ReplaceStripString(string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(oldValue))
            {
                throw new ArgumentException("oldValue");
            }

            if (string.IsNullOrEmpty(newValue))
            {
                throw new ArgumentException("newValue");
            }

            return ReplaceByAtmark(oldValue, newValue.Sanitize());
        }

        #region ������ϊ�
        /// <summary>
        /// String�^�ւ̈Öٌ^�ϊ����Z�q
        /// ���̃��\�b�h���������邱�ƂŁAString�^�ւ̃L���X�g��SQL��������擾�ł��܂��B
        /// </summary>
        /// <param name="value"></param>
        /// <returns>�쐬����SQL������</returns>
        public static implicit operator string(SqlStatement value)
        {
            return value._baseSql;
        }

        /// <summary>
        /// �ÖٓI�Ȍ^�ϊ����T�|�[�g���Ă���̂ŃL���X�g�����SQL�����񂪎擾�ł��邽�߁A�����ɂ͖{���\�b�h�͕K�v����܂���B
        /// �������AToString���\�b�h��String�^�ւ̌^�ϊ��̌��ʂ��قȂ�ꍇ�A�������������ߓ������ʂ�Ԃ��܂��B
        /// </summary>
        /// <returns>�쐬����SQL������</returns>
        public override string ToString()
        {
            return this;
        }
        #endregion

        #region private_method
        /// <summary>
        /// NULL���e�^��l�ɕϊ����܂��B
        /// null��������NULL�ɂ��܂��B
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nullValue"></param>
        /// <returns></returns>
        private static string NullToValue<T>(T? nullValue) where T : struct
        {
            if (!nullValue.HasValue)
            {
                return "NULL";
            }

            return ToValue<T>(nullValue.Value);
        }

        private static string ToValue<T>(T value) where T : struct
        {
            if (typeof(T) == typeof(bool))
            {
                return ((bool)(object)value).ToSqlString();
            }

            if (typeof(T) == typeof(DateTime))
            {
                return ((DateTime)(object)value).ToSqlString();
            }

            return value.ToString();
        }

        /// <summary>
        /// �P���ɕ������u�������܂��B
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        private SqlStatement ReplaceByAtmark(string oldValue, string newValue)
        {
            return new SqlStatement(_baseSql.Replace("@" + oldValue + "@", newValue));
        }
        #endregion
    }
}