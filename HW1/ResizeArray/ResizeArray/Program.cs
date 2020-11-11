using System;

namespace ResizeArray
{
    public class ResizeArray<T>{
        private T[] array;
        private int count;
        static int default_count = 10;

        public ResizeArray()
        {
            array = new T[ResizeArray<T>.default_count];
            count = 0;
        }

        public void AddTail(T tail)
        {
            count++;
            if (count > array.Length)
            {
                T[] newArray = new T[array.Length * 2];
                array.CopyTo(newArray, 0);
                array= newArray;
            }
            array[count - 1] = tail;
        }

        private void checkIndex(int i)
        {
            if (i < 0 || i >= count)
                throw new IndexOutOfRangeException("Индекс должен быть от 0 до " + count + ". Передан индекс " + i);
        }

        public T this[int i]
        {

            get
            {
                checkIndex(i);
                return array[i];
            }

            set
            {
                checkIndex(i);
                array[i] = value;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
