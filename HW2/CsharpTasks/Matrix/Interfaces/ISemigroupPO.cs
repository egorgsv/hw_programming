namespace Matrix.Interfaces
{
    public interface ISemigroupPO<T>
    {
        public T Add(T t1, T t2);

        public bool LessOrEqual(T t1, T t2);
    }
}
