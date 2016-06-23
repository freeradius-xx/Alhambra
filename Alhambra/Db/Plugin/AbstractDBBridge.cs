using System;
using System.Data;
using Alhambra.ConfigUtil;

namespace Alhambra.Db.Plugin
{
    /// <summary>
    /// DB�ɑ΂��鑀����`�����N���X�ł��B
    /// DB���ƁiSQLServer�Ƃ�Oracle�Ƃ��j�Ɏ����N���X���쐬���܂��B
    /// </summary>
    public abstract class AbstractDBBridge : IDisposable
    {
        private const string SQL_SHOULD_NOT_NULL_OR_EMPTY = "SQL�Ƀk���܂��͋󕶎��͎w��ł��܂���B";

        private readonly int _timeout;
        public int Timeout { get { return _timeout; } }

        private readonly IDbConnection _connection;
        public IDbConnection Connection { get { return _connection; } }

        private IDbTransaction _trans = null;
        public IDbTransaction Trans { get { return _trans; } }

        protected readonly IDbCommand _cmd;
        public IDbCommand Cmd {  get { return _cmd; } }

        private bool _isNotTrasactionComplete = true;

        /// <summary>
        /// DB�ւ̃R�l�N�V�������쐬���܂��B
        /// </summary>
        /// <returns></returns>
        abstract protected IDbConnection CreateConnection();

        /// <summary>
        /// DB�Ƃ�DataAdapter���쐬���܂��B
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="con"></param>
        /// <returns></returns>
        abstract public IDbDataAdapter CreateAdapter(string sql, IDbConnection con);

        /// <summary>
        /// �v���O�C������Ԃ��܂��B
        /// </summary>
        abstract public string PluginName { get; }

        /// <summary>
        /// DB�ւ̐ڑ��������Ԃ��܂��B
        /// </summary>
        abstract protected string ConnectionString { get; }

        /// <summary>
        /// �R���X�g���N�^�B
        /// �����N���X����Ăяo���B
        /// </summary>
        protected AbstractDBBridge()
        {
            _timeout = Config.Value.SqlCommandTimeout;
            _connection = CreateConnection();
            _connection.ConnectionString = ConnectionString;
            _connection.Open();
            _cmd = _connection.CreateCommand();
        }

        /// <summary>
        /// �g�����U�N�V�����ƃR�l�N�V������Еt���܂��B
        /// �g�����U�N�V�������R�~�b�g����Ă��Ȃ���΃��[���o�b�N���܂��B
        /// </summary>
        public void Dispose()
        {
            _cmd.Dispose();
            if (_trans != null)
            {
                if (_isNotTrasactionComplete) Rollback();
                _trans.Dispose();
            }
            _connection.Dispose();
        }

        #region �g�����U�N�V��������
        /// <summary>
        /// �g�����U�N�V�����J�n
        /// </summary>
        public void BeginTransaction()
        {
            _trans = _connection.BeginTransaction();
            _cmd.Transaction = _trans;
        }

        /// <summary>
        /// �R�~�b�g
        /// </summary>
        public void Commit()
        {
            _trans.Commit();
            _isNotTrasactionComplete = false;
        }

        /// <summary>
        /// ���[���o�b�N
        /// </summary>
        public void Rollback()
        {
            _trans.Rollback();
            _isNotTrasactionComplete = false;
        }
        #endregion
    }
}
