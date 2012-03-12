using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Ledsun.Alhambra.Db.Plugin;

namespace Ledsun.Alhambra.Db.Helper
{
    /// <summary>
    /// �N���C�A���g�N���X�Ŏg�p����g�����U�N�V�����N���X�ł��B
    /// �g�����U�N�V�������J����AbstractDBBridge��ێ����Ă���̂ŃN���C�A���g�͖{�N���X�̃C���X�^���X�������܂��܂��B
    /// </summary>
    /// <example>
    /// using(var tr = new DBTran){
    ///     DBHelper.Select("INSERT INTO T_PARENT...", tr);
    ///     DBHelper.Select("INSERT INTO T_CHILD...", tr);
    ///     DB.Commit();
    /// }
    /// </example>
    public class DBTran : IDisposable
    {
        internal AbstractDBBridge DB{get; private set;}

        public DBTran()
        {
            DB = DBFactory.NewDB;
            DB.BeginTransaction();
        }

        public void Commit()
        {
            DB.Commit();
        }

        public void Rollback()
        {
            DB.Rollback();
        }

        public void Dispose()
        {
            DB.Dispose();
        }
    }
}
