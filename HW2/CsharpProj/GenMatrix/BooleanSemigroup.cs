using System;
using CsharpProj;

namespace TransitiveClosure
{
    public class BooleanSemigroup : IOrder<Boolean>
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
