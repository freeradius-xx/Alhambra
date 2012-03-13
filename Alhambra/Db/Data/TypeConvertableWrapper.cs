using System;

namespace Ledsun.Alhambra.Db.Data
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

        //sqlserver compact �̐����i1/1/1753 12:00:00 AM ���� 12/31/9999 11:59:59 PM �܂ł̊ԂłȂ���΂Ȃ�܂���B�j
        public static DateTime DateTimeDefault = System.DateTime.Parse("1/1/1753 12:00:00");

        public TypeConvertableWrapper(object rawData)
        {
            _value = rawData;
        }

        //�����I�Ȍ^���w�肵���v���p�e�B
        public uint UInt { get { return To.UInt(_value); } }
        public int Int { get { return To.Int(_value); } }
        public Int16 Int16 { get { return To.Int16(_value); } }
        public Int64 Int64 { get { return To.Int64(_value); } }
        public Byte Byte { get { return To.Byte(_value); } }
        public string String { get { return To.String(_value); } }
        public decimal Decimal { get { return To.Decimal(_value); } }
        public DateTime DateTime { get { return To.DateTime(_value); } }
        public DateTime? DateTimeNull { get { return To.DateTimeNull(_value); } }
        public double Double { get { return To.Double(_value); } }

        /// <summary>
        /// 0��False����ȊO�̐�����True
        /// ������̏ꍇ��True�AFalse�͕ϊ��\�i�啶������������ʂ��Ȃ��j�B����ȊO�͗�O���o���B
        /// </summary>
        public bool Bool { get { return To.Bool(_value); } }

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
