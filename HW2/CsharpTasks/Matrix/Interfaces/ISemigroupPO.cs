﻿using System;
namespace Matrix
{
    public interface ISemigroupPO<T>
    {
        public T Multiply(T t1, T t2);

        public bool LessOrEqual(T t1, T t2);
    }
}
