using Matrix.Interfaces;

namespace Matrix.AlgebraicStructures
{
    public class Natural : ISerializable
    {
        public uint Value { get; private set; }

        public void FromWord(string str)
        {
            Value = uint.Parse(str);
        }

        public string ToWord()
        {
            return Value.ToString();
        }

        public Natural()
        {
            Value = 0;
        }

        public Natural(uint i)
        {
            Value = i;
        }
    }

    public class NaturalSemigroup : ISemigroupPO<Natural>
    {
        public Natural Multiply(Natural t1, Natural t2)
        {
            return new Natural(t1.Value + t2.Value);
        }

        public bool LessOrEqual(Natural t1, Natural t2)
        {
            return t1.Value <= t2.Value;
        }
    }

    public class NaturalSemiring : ISemiring<Natural>
    {
        public Natural Add(Natural t1, Natural t2)
        {
            return new Natural(t1.Value + t2.Value);
        }

        public Natural GetIdentityElement()
        {
            return new Natural(0);
        }

        public Natural Multiply(Natural t1, Natural t2)
        {
            return new Natural(t1.Value * t2.Value);
        }
    }
}