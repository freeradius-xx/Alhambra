using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ledsun.Alhambra.Db
{
    //SQL�X�e�[�g�����g���쐬���邽�߂̃N���X�ł��B
    //@�ň͂񂾕�������A�w��̒l�ɒu�������Ă����Replace���\�b�h��񋟂��܂��B
    //�ȉ��̂悤�ɂ���SQL��������쐬���邱�Ƃ��ł��܂��B
    // new SqlStatement("SELECT * FROM TABLE WHERE ID = @ID@").Replace("ID", 100).ToString();
    public class SqlStatement
    {
        private const string SQL_DATETIME_FORMAT = "\\'yyyy/MM/dd HH:mm:ss\\'";
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

            if (string.IsNullOrEmpty(newValue))
            {
                throw new ArgumentException("newValue");
            }

            return ReplaceByAtmark(oldValue, StringToString(newValue));
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
                    if (val == null)
                    {
                        return "NULL";
                    }
                    else if (typeof(T) == typeof(string))
                    {
                        return StringToString((string)(object)val);
                    }
                    else if (typeof(T) == typeof(bool))
                    {
                        return BoolToString((bool)(object)val);
                    }
                    else if (typeof(T) == typeof(DateTime))
                    {
                        return DateTimeToString((DateTime)(object)val);
                    }
                    else
                    {
                        return val.ToString();
                    }
                });

                return ReplaceByAtmark(oldValue, "(" + string.Join(",", strs) + ")");
            }

            throw new ArgumentException("newStrings");
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

            if (string.IsNullOrEmpty(newValue))
            {
                throw new ArgumentException("newValue");
            }

            return ReplaceByAtmark(oldValue, "'%" + Sanitize(newValue) + "%'");
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
                throw new ArgumentException("oldString");
            }

            if (string.IsNullOrEmpty(newValue))
            {
                throw new ArgumentException("newString");
            }

            return ReplaceByAtmark(oldValue, Sanitize(newValue));
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
                return BoolToString((bool)(object)value);
            }

            if (typeof(T) == typeof(DateTime))
            {
                return DateTimeToString((DateTime)(object)value);
            }

            return value.ToString();
        }

        private static string BoolToString(bool val)
        {
            return val ? "1" : "0";
        }

        private static string DateTimeToString(DateTime val)
        {
            return val.ToString(SQL_DATETIME_FORMAT);
        }

        private string StringToString(string val)
        {
            return "N'" + Sanitize(val) + "'";
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

        /// <summary>
        /// ������u������SQL�C���W�F�N�V�����΍�Ɋ댯�ȕ�����i�V���O���N�H�[�g�ƃp�[�Z���g�j���G�X�P�[�v���܂��B
        /// varchar�^����Null���w�肵�����ꍇ�́Anull�ł͂Ȃ�������"NULL"���w�肵�Ă��������B
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string Sanitize(string value)
        {
            //IEnumerable�̗v�f��null�������Ă����ꍇ�̓`�F�b�N�ł��Ȃ��̂ŁA������null���󕶎��ɒu�������܂�
            if (String.IsNullOrEmpty(value))
            {
                return "";
            }

            var builder = new StringBuilder(value.Length);
            foreach (char c in value)
            {
                if (c == '\'')
                {
                    builder.Append('\'');
                }
                if (c == '%')
                {
                    builder.Append('\\');
                }
                builder.Append(c);
            }
            return builder.ToString();
        }
        #endregion
    }
}