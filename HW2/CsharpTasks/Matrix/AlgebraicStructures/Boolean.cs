using System;
using Matrix.Interfaces;

namespace Matrix.AlgebraicStructures
{
    public class Boolean : ISerializable
    {
        public bool Value { get; private set; }

        public void FromWord(string str)
        {
            Value = str switch
            {
                "t" => true,
                "f" => false,
                _ => throw new ArgumentException("Boolean can be restored only from 't' or 'f'.")
            };
        }

        public string ToWord() => Value ? "t" : "f";

        public Boolean()
        {
            Value = false;
        }

        public Boolean(bool i)
        {
            Value = i;
        }
    }

    public class BooleanSemigroup : ISemigroupPO<Boolean>
    {
        public bool LessOrEqual(Boolean t1, Boolean t2)
        {
            return t1.Value || !t2.Value;
        }

        public Boolean Add(Boolean t1, Boolean t2)
        {
            return new Boolean(t1.Value && t2.Value);
        }
    }
}