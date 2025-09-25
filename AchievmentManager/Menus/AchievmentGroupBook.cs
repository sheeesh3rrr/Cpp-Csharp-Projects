namespace EMenuLib
{
    /// <summary>
    /// Работает с карточками групп достижений.
    /// </summary>
    internal class AchievmentGroupBook
    {
        /// <summary>
        /// Все страницы книги.
        /// </summary>
        public KeyValuePair<AchievmentGroupCard, AchievmentGroupCard?>[] Book { get; }

        /// <summary>
        /// Количество страниц в книге.
        /// </summary>
        public int Length => Book.Length;

        /// <summary>
        /// Создает книгу.
        /// </summary>
        /// <param name="arr">Карточки групп достижений.</param>
        public AchievmentGroupBook(AchievmentGroupCard[] arr)
        {
            Book = Build(arr);
        }

        /// <summary>
        /// Отрисовывает в консоли 1 страницу книги.
        /// </summary>
        /// <param name="page">Номер страницы.</param>
        /// <param name="mainTitle">Заголовок страинцы.</param>
        /// <param name="extraTitle">Дополнительный заголовок.</param>
        public void Draw(int page, string mainTitle, string extraTitle)
        {
            Console.Clear();
            Console.WriteLine(mainTitle);
            Console.WriteLine(extraTitle);
            Console.WriteLine($"Страница: {page + 1}");
            Console.WriteLine();

            Book[page].Key.Draw();
            Console.WriteLine();

            if (Book[page].Value != null)
            {
                Book[page].Value!.Draw();
            }
        }

        /// <summary>
        /// Заполняет книгу страницами.
        /// </summary>
        /// <param name="arr">Карточки групп достижений.</param>
        /// <returns>Массив страниц.</returns>
        private KeyValuePair<AchievmentGroupCard, AchievmentGroupCard?>[] Build(AchievmentGroupCard[] arr)
        {
            KeyValuePair<AchievmentGroupCard, AchievmentGroupCard?>[] ans =
                new KeyValuePair<AchievmentGroupCard, AchievmentGroupCard?>[(arr.Length + 1) / 2];

            for (int i = 0; i < arr.Length; i += 2)
            {
                ans[i / 2] = i + 1 >= arr.Length ? new(arr[i], null) : new(arr[i], arr[i + 1]);
            }

            return ans;
        }
    }
}
