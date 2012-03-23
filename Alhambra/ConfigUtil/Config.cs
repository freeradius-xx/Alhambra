using System;
using System.Configuration;
using System.Reflection;

namespace Ledsun.Alhambra.ConfigUtil
{
    /// <summary>
    /// XXX.Config�̐ݒ�l��ǂݎ��N���X�ł��B
    /// Config.Value.SqlCommandTimeout�Ƃ����`�Œl���擾�ł��܂��B
    /// �v���W�F�N�g�ŗL�̐ݒ��Config.ConfigValue�N���X�Ɋg�����\�b�h��ǉ�����Γ����悤�ɍs����Ǝv�����ǁA�����؂ł��B
    /// </summary>
    public class Config
    {
        //ConfigValue�p�����Ď������Ă�̂ŃC���X�^���X�����K�v�ł��B
        private static ConfigValue value = new ConfigValue();

        /// <summary>
        /// Config.Value.XXX��ÓI�ɎQ�Ƃł���悤�ɁA�ÓI�ȃv���p�e�B��񋟂��܂��B
        /// </summary>
        public static ConfigValue Value
        {
            get { return value; }
        }

        public class ConfigValue 
        {
            private static AppSettingsReader _reader = new AppSettingsReader();

            /// <summary>
            /// ���O���w�肵�ăf�[�^�x�[�X�ڑ���������擾����B
            /// �v���O�C�����ƂɃf�[�^�x�[�X�ڑ�������𕪂��邽�߂Ɏg�p�B
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public string GetConnectionString(string name)
            {
                var css = ConfigurationManager.ConnectionStrings[name];
                if (css != null && !String.IsNullOrEmpty(css.ConnectionString))
                {

                    return css.ConnectionString;
                }
                else
                {
                    throw new DBHelperException("config�t�@�C����ConnectionString��" + name + "�̐ڑ���������w�肵�ĉ������B");
                }
            }

            /// <summary>
            /// SQL���s�^�C���A�E�g��ݒ肵�܂��B
            /// �ݒ肳��ĂȂ����30�b���g���܂��B
            /// </summary>
            internal int SqlCommandTimeout
            {
                get
                {
                    try
                    {
                        return _reader.GetValue<int>(MethodBase.GetCurrentMethod().Name.Substring(4));
                    }
                    catch (InvalidOperationException)
                    {
                        return 30;
                    }
                }
            }
        }
    }
}