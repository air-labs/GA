namespace AIRLab.Bulldozer
{
    public class MutablePair<T1, T2>
    {
        public T1 Key { get; set; }
        public T2 Value { get; set; }

        public MutablePair(T1 key, T2 value)
        {
            Key = key;
            Value = value;
        }

        public static MutablePair<T1, T2> Create(T1 key, T2 value)
        {
            return new MutablePair<T1, T2>(key, value);
        }
    }
}