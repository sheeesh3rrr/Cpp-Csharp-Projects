namespace EMenuLib
{
    public static class ArrowMenu
    {
        /// <summary>
        /// Обрабатывает все операции, связанные со стрелочным меню.
        /// </summary>
        /// <param name="mainTile">Текст, выводимый в верхней части меню.</param>
        /// <param name="postTile">Текст, выводимый в нижней части меню.</param>
        /// <param name="titles">Варинаты выбора меню.</param>
        /// <returns>Номер выбранного пункта меню.</returns>
        public static int ProcessArrowMenu(string mainTile, string postTile, params string[] titles)
        {
            Console.Clear();
            titles = titles.Length > 20 ? titles[..20] : titles;
            PaintArrowMenuSkeleton(mainTile, postTile, titles);
            return ProcessArrowMovement(titles.Length);
        }

        /// <summary>
        /// Отрисовывает начальное положение меню.
        /// </summary>
        /// <param name="mainTile">Текст, выводимый в верхней части меню.</param>
        /// <param name="postTile">Текст, выводимый в нижней части меню.</param>
        /// <param name="titles">Варинаты выбора меню.</param>
        private static void PaintArrowMenuSkeleton(string mainTile, string postTile, params string[] titles)
        {
            Console.WriteLine(mainTile + '\n');

            for (int i = 0; i < titles.Length; i++)
            {
                Console.WriteLine("  " + titles[i]);
            }
            Console.SetCursorPosition(0, 2);
            Console.Write('>');

            Console.SetCursorPosition(0, titles.Length + 3);
            Console.WriteLine(postTile);
        }

        /// <summary>
        /// Обрабатывает операции по пользовательским взаимодействиям с меню.
        /// </summary>
        /// <param name="len">количество пунктов выбора.</param>
        /// <returns>Номер выбранного пункта.</returns>
        private static int ProcessArrowMovement(int len)
        {
            int position = 1;
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.SetCursorPosition(0, position + 1);
                Console.Write(' ');

                if (key.Key == ConsoleKey.UpArrow)
                {
                    position += position > 1 ? -1 : 0;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    position += position < len ? 1 : 0;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    return position;
                }

                Console.SetCursorPosition(0, position + 1);
                Console.Write('>');
                Console.SetCursorPosition(0, len + 4);
            }
        }
    }
}
