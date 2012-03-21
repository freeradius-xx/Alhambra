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
        const string DATABASE = "@DB@";
        const string DATETIME_FORMAT = "\\'yyyy/MM/dd HH:mm:ss\\'";
        private readonly string _baseSql;

        public SqlStatement(string baseSql)
        {
            _baseSql = (string)baseSql.Clone();
        }

        public SqlStatement(string baseSql, string dbName)
            : this(baseSql)
        {
            _baseSql = _baseSql.Replace(DATABASE, dbName);
        }

        public SqlStatement Replace(string oldString, bool newValue)
        {
            return ReplaceByAtmark(oldString, newValue ? "1" : "0");
        }

        public SqlStatement Replace(string oldString, int newValue)
        {
            return ReplaceByAtmark(oldString, newValue.ToString());
        }

        public SqlStatement Replace(string oldString, Decimal newValue)
        {
            return ReplaceByAtmark(oldString, newValue.ToString());
        }

        public SqlStatement Replace(string oldString, Decimal? newValue)
        {
            var newString = !newValue.HasValue ? "NULL" : newValue.Value.ToString();
            return ReplaceByAtmark(oldString, newString);
        }

        public SqlStatement Replace(string oldString, DateTime newDate)
        {
            //�������������ꂽ�l���w�肳�ꂽ�ꍇ��NULL�ɂ��܂��B
            //DB��datetime�^�͎w�薳���������l�����Ȃ�����NULL�������܂��B
            var newString = newDate == new DateTime(0) ? "NULL" : newDate.ToString(DATETIME_FORMAT);
            return ReplaceByAtmark(oldString, newString);
        }

        public SqlStatement Replace(string oldString, DateTime? newDate)
        {
            var newString = !newDate.HasValue ? "NULL" : newDate.Value.ToString(DATETIME_FORMAT);
            return ReplaceByAtmark(oldString, newString);
        }

        public SqlStatement Replace(string oldString, IEnumerable<string> newStrings)
        {
            if (string.IsNullOrEmpty(oldString))
            {
                throw new ArgumentException("oldString");
            }

            if (newStrings == null)
            {
                throw new ArgumentNullException("newStrings");
            }

            if (newStrings.Any())
            {
                var newString = "";
                foreach (string str in newStrings)
                {
                    newString += "'" + Sanitize(str) + "',";
                }

                return ReplaceByAtmark(oldString, CutLastChar(newString));
            }

            throw new ArgumentException("newStrings");
        }

        public SqlStatement Replace(string oldString, IEnumerable<long> newStrings)
        {
            if (string.IsNullOrEmpty(oldString))
            {
                throw new ArgumentException("oldString");
            }

            if (newStrings == null)
            {
                throw new ArgumentNullException("newStrings");
            }

            if (newStrings.Any())
            {
                var newString = "";
                foreach (long l in newStrings)
                {
                    newString += l.ToString() + ",";
                }

                return ReplaceByAtmark(oldString, CutLastChar(newString));
            }

            throw new ArgumentException("newStrings");
        }

        private static string CutLastChar(string val)
        {
            return val.Length > 0 ? "(" + val.Remove(val.Length - 1) + ")" : val;
        }

        //�V���O���N�H�[�g�ň͂ޔ�
        public SqlStatement Replace(string oldString, string newString)
        {
            return ReplaceByAtmark(oldString, "N'" + Sanitize(newString) + "'");
        }

        public SqlStatement ReplaceForPartialMatchRetrieval(string oldString, string newString)
        {
            return ReplaceByAtmark(oldString, "'%" + Sanitize(newString) + "%'");
        }

        public SqlStatement ReplaceStripString(string oldString, string newString)
        {
            return ReplaceByAtmark(oldString, Sanitize(newString));
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
        private SqlStatement ReplaceByAtmark(string oldString, string newString)
        {
            return new SqlStatement(_baseSql.Replace("@" + oldString + "@", newString));
        }

        /// <summary>
        /// ������u������SQL�C���W�F�N�V�����΍�Ɋ댯�ȕ�����i�V���O���N�H�[�g�ƃp�[�Z���g�j���G�X�P�[�v���܂��B
        /// varchar�^����Null���w�肵�����ꍇ�́Anull�ł͂Ȃ�������"NULL"���w�肵�Ă��������B
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string Sanitize(string value)
        {
            //������null���󕶎��ɒu��������̂͂��������ł��B
            //�Ăяo������public���\�b�h�ň�����null���`�F�b�N��ArgumentNullException��Ԃ��ׂ��ł��B
            //�������A�ǂ̃v���W�F�N�g�Ŏg���Ă��邩�킩��Ȃ��̂ł����C�����܂���B
            if (value == null) return "";
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