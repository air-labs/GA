namespace AIRLab.Bulldozer
{
    public static class Identity
    {
        private static long _identity = 0;
        private static readonly object Obj = new object();

        public static long NewIdentity()
        {
            lock (Obj)
            {
                return ++_identity;
            }
        }

        public static void Reset()
        {
            lock (Obj)
            {
                _identity = 0;
            }
        }
    }
}