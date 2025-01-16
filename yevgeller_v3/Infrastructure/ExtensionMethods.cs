namespace yevgeller_v3.Infrastructure
{
    public static class ListExtensions
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Shuffles a list in place using the Fisher-Yates algorithm.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to shuffle.</param>
        public static void Shuffle<T>(this List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = _random.Next(0, i + 1); // Random index between 0 and i (inclusive)
                (list[i], list[j]) = (list[j], list[i]); // Swap elements
            }
        }
    }
}