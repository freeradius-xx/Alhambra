using System;
using System.Collections.Generic;
using System.Data;
using Alhambra.ConfigUtil;
using Alhambra.Db.Data;
using Alhambra.Db.Helper;

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
        private readonly IDbConnection _connection;
        private IDbTransaction _trans = null;
        protected readonly IDbCommand _cmd;
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
        abstract protected IDbDataAdapter CreateAdapter(string sql, IDbConnection con);

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

        #region SQL���s
        /// <summary>
        /// ���ʂ�Ԃ��Ȃ�SQL�����s���܂�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int Execute(string sql)
        {
            if (String.IsNullOrEmpty(sql))
                throw new ArgumentException(SQL_SHOULD_NOT_NULL_OR_EMPTY);

            return PrepareCommand(sql).Execute();
        }

        /// <summary>
        /// �l����Ԃ�SQL�����s���܂�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public TypeConvertableWrapper SelectOne(string sql)
        {
            if (String.IsNullOrEmpty(sql))
                throw new ArgumentException(SQL_SHOULD_NOT_NULL_OR_EMPTY);

            return PrepareCommand(sql).SelectOne();
        }

        /// <summary>
        /// SELECT�����s���܂��B
        /// ���ʂ�DataRowAccessor�̃��X�g�ŕԂ��܂��B
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IEnumerable<DataRowAccessor> Select(string sql)
        {
            if (String.IsNullOrEmpty(sql))
                throw new ArgumentException(SQL_SHOULD_NOT_NULL_OR_EMPTY);

            return PrepareDataAdapter(sql).SelectFromDataAdapter();
        }

        /// <summary>
        /// SELECT�����s���܂��B
        /// ���ʂ�DataSet�ŕԂ��܂��B
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet SelectDataSet(string sql)
        {
            if (String.IsNullOrEmpty(sql))
                throw new ArgumentException(SQL_SHOULD_NOT_NULL_OR_EMPTY);

            return PrepareDataAdapter(sql).SelectDataSetFromDataAdapter();
        }
        #endregion

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

        #region �v���C�x�[�g���\�b�h
        private IDbCommand PrepareCommand(string sql)
        {
            _cmd.CommandText = sql;
            _cmd.CommandTimeout = _timeout;
            return _cmd;
        }

        private IDbDataAdapter PrepareDataAdapter(string sql)
        {
            var dataAdapter = CreateAdapter(sql, _connection);
            dataAdapter.SelectCommand.CommandTimeout = _timeout;
            dataAdapter.SelectCommand.Transaction = _trans;
            return dataAdapter;
        }
        #endregion


    }
}
