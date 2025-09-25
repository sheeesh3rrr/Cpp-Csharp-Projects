namespace EMenuLib
{
    public static class CheckMenu
    {
        /// <summary>
        /// Обрабатывает все операции, связанные с выборным меню.
        /// </summary>
        /// <param name="mainTile">Текст, выводимый в верхней части меню.</param>
        /// <param name="postTile">Текст, выводимый в нижней части меню.</param>
        /// <param name="titles">Варинаты выбора меню.</param>
        /// <returns>Индексы выбранных пунктов меню.</returns>
        public static int[] ProcessCheckMenu(string mainTile, string postTile, string[] titles)
        {
            Console.Clear();
            PaintCheckMenuSkeleton(mainTile, postTile, titles);
            return ProcessArrowMovement(titles.Length);
        }

        /// <summary>
        /// Отрисовывает начальное положение меню.
        /// </summary>
        /// <param name="mainTile">Текст, выводимый в верхней части меню.</param>
        /// <param name="postTile">Текст, выводимый в нижней части меню.</param>
        /// <param name="titles">Варианты выбора меню.</param>
        private static void PaintCheckMenuSkeleton(string mainTile, string postTile, string[] titles)
        {
            Console.WriteLine(mainTile + '\n');

            for (int i = 0; i < titles.Length; i++)
            {
                Console.WriteLine("    " + titles[i]);
            }
            Console.WriteLine("   Подтвердить выбор");
            Console.SetCursorPosition(0, 2);
            Console.Write('>');

            Console.SetCursorPosition(0, titles.Length + 4);
            Console.WriteLine(postTile);
        }

        /// <summary>
        /// Обрабатывает операции по пользовательским взаимодействиям с меню.
        /// </summary>
        /// <param name="len">количество пунктов выбора.</param>
        /// <returns>Индексы выбранных пунктов.</returns>
        private static int[] ProcessArrowMovement(int len)
        {
            List<int> list = [];
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
                    position += position < len + 1 ? 1 : 0;
                }
                else if (key.Key == ConsoleKey.Enter && position != len + 1)
                {
                    Console.SetCursorPosition(2, position + 1);

                    if (list.Contains(position - 1))
                    {
                        Console.Write(' ');
                        _ = list.Remove(position - 1);
                    }
                    else
                    {
                        Console.Write('V');
                        list.Add(position - 1);
                    }
                }
                else if (key.Key == ConsoleKey.Enter && position == len + 1)
                {
                    Console.Clear();
                    return [.. list];
                }

                Console.SetCursorPosition(0, position + 1);
                Console.Write('>');
                Console.SetCursorPosition(0, len + 5);
            }
        }
    }
}
