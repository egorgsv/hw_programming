using System;
using CsharpProj;

namespace TransitiveClosure
{
    public class Boolean : ISerializable
    {
        public bool value { get; private set; }

        public void FromWord(String str)
        {
            if (str.Equals("t"))
                value = true;
            else if (str.Equals("f"))
                value = false;
            else
                throw new ArgumentException("Boolean can be restored only from 't' or 'f'.");
        }

        public String ToWord() => value ? "t" : "f";

        public Boolean()
        {
            value = false;
        }

        public Boolean(bool i)
        {
            value = i;
        }
    }
    
    public class BooleanSemigroup : ISemigroupPO<Boolean>
    {
        public bool LessOrEqual(Boolean t1, Boolean t2)
        {
            return t1.value || !t2.value;
        }

        public Boolean Multiply(Boolean t1, Boolean t2)
        {
            return new Boolean(t1.value && t2.value);
        }
    }
}
