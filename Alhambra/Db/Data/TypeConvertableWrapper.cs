using System;
using ObjectExtentions.TypeConvert;

namespace Alhambra.Db.Data
{
    /// <summary>
    /// �^�ϊ��@�\�����w���p�[�N���X
    /// �����Ŏ擾�����l���v���p�e�B�Ō^���w�肵�ĔC�ӂ̌^�ɕϊ����Ď擾�ł��܂��B
    /// ���DB����擾�����l��DataTransferObject�̌^�ɍ��킹�ĕϊ����邽�߂Ɏg���܂��B
    /// </summary>
    /// <example>
    /// �^���̃v���p�e�B�Œl�𓖊Y�^�ɕϊ����Ď擾�ł��܂��B
    ///     new TypeConvertableWrapper("1").Int
    /// String�^�ւ͈Öٌ^�ϊ����\�ł��B
    ///     string hoge = new TypeConvertableWrapper("1");
    /// </example>
    public class TypeConvertableWrapper
    {
        private readonly Object _value;

        public static DateTime DateTimeDefault = ObjectTypeConvertExtentions.DateTimeDefault;

        public TypeConvertableWrapper(object rawData)
        {
            _value = rawData;
        }

        //�����I�Ȍ^���w�肵���v���p�e�B
        public uint UInt { get { return _value.UInt(); } }
        public int Int { get { return _value.Int(); } }
        public Int16 Int16 { get { return _value.Int16(); } }
        public Int64 Int64 { get { return _value.Int64(); } }
        public Byte Byte { get { return _value.Byte(); } }
        public string String { get { return _value.String(); } }
        public decimal Decimal { get { return _value.Decimal(); } }
        public DateTime DateTime { get { return _value.DateTime(); } }
        public DateTime? DateTimeNull { get { return _value.DateTimeNull(); } }
        public double Double { get { return _value.Double(); } }

        /// <summary>
        /// 0��False����ȊO�̐�����True
        /// ������̏ꍇ��True�AFalse�͕ϊ��\�i�啶������������ʂ��Ȃ��j�B����ȊO�͗�O���o���B
        /// </summary>
        public bool Bool { get { return _value.Bool(); } }

        //String�Ɋւ��Ă͈Öق̌^�ϊ����\
        public static implicit operator string(TypeConvertableWrapper value)
        {
            return value.String;
        }

        public override string ToString()
        {
            return String;
        }       
    }
}
