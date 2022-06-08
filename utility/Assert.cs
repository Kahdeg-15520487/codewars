namespace utility
{
    public static class Assert
    {
        public static void IsNotNull<T>(T type, string msg = "") where T : class
        {
            if (type != null)
            {
                Console.WriteLine("OK");
                return;
            }
            Console.WriteLine(msg);
        }

        public static void IsTrue(bool v, string msg = "")
        {
            if (v)
            {
                Console.WriteLine("OK");
                return;
            }
            Console.WriteLine(msg);
        }

        public static void AreEqual<T>(T v1, T v2, string msg = "") where T : IEquatable<T>
        {
            if (EqualityComparer<T>.Default.Equals(v1, v2))
            {
                Console.WriteLine("OK");
                return;
            }
            Console.WriteLine(msg);
        }
    }
}