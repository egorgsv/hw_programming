using System;

namespace Matrix
{
    public class Natural : ISerializable
    {
        public uint value { get; private set; }

        public void FromWord(string str)
        {
            value = UInt32.Parse(str);
        }

        public string ToWord()
        {
            return value.ToString();
        }

        public Natural()
        {
            value = 0;
        }

        public Natural(uint i)
        {
            value = i;
        }
    }
    
    public class NaturalSemigroup : ISemigroupPO<Natural>
    {
        public Natural Multiply(Natural t1, Natural t2)
        {
            return new Natural(t1.value + t2.value);
        }

        public bool LessOrEqual(Natural t1, Natural t2)
        {
            return t1.value <= t2.value;
        }
    }
    
    public class NaturalSemiring : ISemiring<Natural>
    {
        public Natural Add(Natural t1, Natural t2)
        {
            return new Natural(t1.value + t2.value);
        }

        public Natural GetIdentityElement()
        {
            return new Natural(0);
        }

        public Natural Multiply(Natural t1, Natural t2)
        {
            return new Natural(t1.value*t2.value);
        }
    }
}
