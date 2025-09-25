namespace EMenuLib
{
    public static class ShadeMenu
    {
        /// <summary>
        /// Обрабатывает все операции, связанные с теневым меню.
        /// </summary>
        /// <param name="mainTile">Текст, выводимый в верхней части меню.</param>
        /// <param name="extraTile">Дополнительный текст меню.</param>
        /// <param name="cards">Карточки достижений.</param>
        public static void ProcessShadeMenu(string mainTitle, string extraTitle, AchievmentGroupCard[] cards)
        {
            if (cards.Length == 0)
            {
                throw new ArgumentNullException("Не было получено информации о группах. Демонстрация TUI невозможна");
            }
            AchievmentGroupBook book = new(cards);

            PrintShadeMenuSkeleton(mainTitle, extraTitle, book);
            ProcessShadeMovement(book, mainTitle, extraTitle);
        }

        /// <summary>
        /// Отрисовывает начальное положение меню.
        /// </summary>
        /// <param name="mainTile">Текст, выводимый в верхней части меню.</param>
        /// <param name="extraTile">Дополнительный текст меню.</param>
        /// <param name="cards">Карточки достижений.</param>
        private static void PrintShadeMenuSkeleton(string mainTitle, string extraTitle, AchievmentGroupBook book)
        {
            book.Book[0].Key.IsHovered = true;
            book.Draw(0, mainTitle, extraTitle);
        }

        /// <summary>
        /// Обрабатывает операции по пользовательским взаимодействиям с меню.
        /// </summary>
        /// <param name="mainTile">Текст, выводимый в верхней части меню.</param>
        /// <param name="extraTile">Дополнительный текст меню.</param>
        /// <param name="book">Книга с карточками достижений.</param>
        private static void ProcessShadeMovement(AchievmentGroupBook book, string mainTitle, string extraTitle)
        {
            int page = 0;
            int card = 1;

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();

                if (key.Key == ConsoleKey.UpArrow)
                {
                    book.Book[page].Key.IsHovered = true;
                    if (book.Book[page].Value != null)
                    {
                        book.Book[page].Value!.IsHovered = false;
                    }

                    card = 1;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    if (book.Book[page].Value != null)
                    {
                        book.Book[page].Value!.IsHovered = true;
                        book.Book[page].Key.IsHovered = false;
                        card = 2;
                    }
                    else
                    {
                        card = 1;
                        book.Book[page].Key.IsHovered = true;
                    }
                }

                else if (key.Key == ConsoleKey.LeftArrow)
                {
                    book.Book[page].Key.IsHovered = false;
                    if (book.Book[page].Value != null)
                    {
                        book.Book[page].Value!.IsHovered = false;
                    }

                    page += page > 0 ? -1 : 0;
                    book.Book[page].Key.IsHovered = true;
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    book.Book[page].Key.IsHovered = false;
                    if (book.Book[page].Value != null)
                    {
                        book.Book[page].Value!.IsHovered = false;
                    }

                    page += page < book.Length - 1 ? 1 : 0;
                    book.Book[page].Key.IsHovered = true;
                }

                else if (key.Key == ConsoleKey.Enter)
                {
                    if (card == 1)
                    {
                        book.Book[page].Key.Click();
                    }
                    else
                    {
                        if (book.Book[page].Value != null)
                        {
                            book.Book[page].Value!.Click();
                        }
                    }
                }

                else if (key.Key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    return;
                }

                book.Draw(page, mainTitle, extraTitle);
            }
        }
    }
}
