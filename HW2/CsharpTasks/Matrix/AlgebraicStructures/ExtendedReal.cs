using Matrix.Interfaces;

namespace Matrix.AlgebraicStructures
{
    public class ExtendedReal : ISerializable
    {
        
        public float Value { get; private set; }
        public void FromWord(string str)
        {
            if (str == "[ ? ]")
            {
                Value = float.NaN;
            }
            else if (str == "inf")
            {
                Value = float.PositiveInfinity;
            }
            else
            {
                Value = float.Parse(str);
            }
        }

        public string ToWord()
        {
            if (float.IsNaN(Value))
            {
                return "[ ? ]";
            }

            return float.IsInfinity(Value) ? "inf" : Value.ToString();
        }
        
        public ExtendedReal()
        {
            Value = 0.0f;
        }

        public ExtendedReal(float i)
        {
            Value = i;
        }
    }
    
    public class ExtendedRealSemigroup : ISemigroupPO<ExtendedReal>
    {
        public ExtendedReal Add(ExtendedReal t1, ExtendedReal t2)
        {
            if (float.IsNaN(t1.Value) || float.IsNaN(t2.Value))
            {
                return new ExtendedReal(float.NaN);
            }

            if (float.IsInfinity(t1.Value) || float.IsInfinity(t2.Value))
            {
                return new ExtendedReal(float.PositiveInfinity);
            }

            return new ExtendedReal(t1.Value + t2.Value);
        }

        public bool LessOrEqual(ExtendedReal t1, ExtendedReal t2)
        {
            if (float.IsNaN(t1.Value) || float.IsNaN(t2.Value))
            {
                return false;
            }
            
            if (float.IsInfinity(t1.Value) && float.IsInfinity(t2.Value))
            {
                return true;
            }

            if (float.IsInfinity(t1.Value))
            {
                return false;
            }

            if (float.IsInfinity(t2.Value))
            {
                return true;
            }
            
            return t1.Value <= t2.Value;
        }
    }

    public class ExtendedRealSemiring : ISemiring<ExtendedReal>
    {
        public ExtendedReal Add(ExtendedReal t1, ExtendedReal t2)
        {
            if (float.IsNaN(t1.Value) || float.IsNaN(t2.Value))
            {
                return new ExtendedReal(float.NaN);
            }

            if (float.IsInfinity(t1.Value) || float.IsInfinity(t2.Value))
            {
                return new ExtendedReal(float.PositiveInfinity);
            }

            return new ExtendedReal(t1.Value + t2.Value);
        }

        public ExtendedReal GetIdentityElement()
        {
            return new ExtendedReal(0.0f);
        }

        public ExtendedReal Multiply(ExtendedReal t1, ExtendedReal t2)
        {

            if (float.IsNaN(t1.Value) || float.IsNaN(t2.Value))
            {
                return new ExtendedReal(float.NaN);
            }
            
            if (float.IsInfinity(t1.Value) || float.IsInfinity(t2.Value))
            {
                if (float.IsInfinity(t1.Value) && float.IsInfinity(t2.Value))
                {
                    return new ExtendedReal(float.PositiveInfinity);
                }

                if (t1.Value != 0.0f || t2.Value != 0.0f)
                {
                    return new ExtendedReal(float.NaN);
                }

                return new ExtendedReal(float.PositiveInfinity);
            }
            return new ExtendedReal(t1.Value * t2.Value);
        }
    }
}