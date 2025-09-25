using EDataBaseLib;
using EMenuLib;
using System.Text.Json;

namespace MainField
{
    /// <summary>
    /// Отвечает за взаимодействия с пользователем.
    /// </summary>
    internal static class UserActionHandler
    {
        /// <summary>
        /// Проводит все операции, связанные с работой главного меню.
        /// </summary>
        public static void ProcessMainMenu()
        {
            try
            {
                while (true)
                {
                    int key = ArrowMenu.ProcessArrowMenu(
                        "Добро пожаловать!",
                        "Для изменения позиции указателя используйте стрелочки (вверх / вниз), для выбора пункта меню - Enter",
                        "Старт", "ReadMe", "Выйти");

                    switch (key)
                    {
                        case 1:
                            InitiateMainOperations();
                            break;
                        case 2:
                            LoadReadMe();
                            break;
                        case 3:
                            return;
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Неизвестная ошибка.");
                AwaitExit();
            }
        }

        /// <summary>
        /// Проводит все операции, связанные с работой меню выбора функций.
        /// </summary>
        private static void InitiateMainOperations()
        {
            try
            {
                DataBase dataBase = new();
                while (true)
                {
                    int key = ArrowMenu.ProcessArrowMenu(
                        "Выберите функцию", "",
                        "Добавить заметку", "Удалить заметки", "Редактировать заметку", "Просмотреть информацию о заметках",
                        "Отсортировать заметки", "Отфильровать заметки", "Заархивировать/Разархивировать заметки",
                        "Просмотреть статистику по тегам", "Создать тег", "Поменять пути расположения файлов хранения информации о заметках",
                        "Сохранить и вернуться в главное меню");

                    switch (key)
                    {
                        case 1:
                            AddNewNote(ref dataBase);
                            break;
                        case 2:
                            EraseNotes(ref dataBase);
                            break;
                        case 3:
                            EditNote(ref dataBase);
                            break;
                        case 4:
                            ProcessNotesShowCase(dataBase);
                            break;
                        case 5:
                            ProcessSorting(dataBase);
                            break;
                        case 6:
                            ProcessFilterring(dataBase);
                            break;
                        case 7:
                            ProcessArchiving(ref dataBase);
                            break;
                        case 8:
                            PrintTagsStats(dataBase);
                            break;
                        case 9:
                            CreateNewTag(ref dataBase);
                            break;
                        case 10:
                            UpdatePath(dataBase);
                            break;
                        case 11:
                            dataBase.SaveData();
                            return;
                    }
                }
            }
            catch (ArgumentException)
            {
                Console.Clear();
                Console.WriteLine("Один из файлов программы имеет недопустимый путь.");
                Console.WriteLine("Попробуйте завершить программу,");
                Console.WriteLine("затем вручную удалить файл config.txt, находящийся в папке решения.");
                Console.WriteLine("После этого перезапустите программу.");
                AwaitExit();
            }
            catch (FormatException)
            {
                Console.Clear();
                Console.WriteLine("Файл, содержащий информацию о заметках поврежден или некорректно структурирован.");
                Console.WriteLine("Попробуйте завершить программу,");
                Console.WriteLine("затем вручную удалить файл config.txt, находящийся в папке решения.");
                Console.WriteLine("После этого перезапустите программу.");
                Console.WriteLine("ЛИБО");
                Console.WriteLine("Попробуйте вручную исправить файл, содержащий информацию о заметках.");
                AwaitExit();
            }
            catch (JsonException)
            {
                Console.Clear();
                Console.WriteLine("Возникла проблема при работе с Json-файлами программы.");
                Console.WriteLine("Попробуйте завершить программу,");
                Console.WriteLine("затем вручную удалить файл config.txt, находящийся в папке решения.");
                Console.WriteLine("После этого перезапустите программу.");
                AwaitExit();
            }
            catch (PathTooLongException)
            {
                Console.Clear();
                Console.WriteLine("Один из файлов программы имеет слишком длинный путь.");
                Console.WriteLine("Попробуйте завершить программу,");
                Console.WriteLine("затем вручную удалить файл config.txt, находящийся в папке решения.");
                Console.WriteLine("После этого перезапустите программу.");
                AwaitExit();
            }
            catch (DirectoryNotFoundException)
            {
                Console.Clear();
                Console.WriteLine("Один из файлов программы находится в неизвестном директории.");
                Console.WriteLine("Попробуйте завершить программу,");
                Console.WriteLine("затем вручную удалить файл config.txt, находящийся в папке решения.");
                Console.WriteLine("После этого перезапустите программу.");
                AwaitExit();
            }
            catch (FileNotFoundException)
            {
                Console.Clear();
                Console.WriteLine("Один из необходимых файлов программы не найден.");
                Console.WriteLine("Попробуйте завершить программу,");
                Console.WriteLine("затем вручную удалить файл config.txt, находящийся в папке решения.");
                Console.WriteLine("После этого перезапустите программу.");
                AwaitExit();
            }
            catch (IOException)
            {
                Console.Clear();
                Console.WriteLine("Ошибка ввода-вывода.");
                Console.WriteLine("Попробуйте вернуться в главное меню и затем вновь воспользоваться функцией обновления пути к файлу.");
                Console.WriteLine("НЕ ОСТАНАВЛИВАЙТЕ ПРИЛОЖЕНИЕ ПРИНУДИТЕЛЬНО!");
                Console.WriteLine("В ПРОТИВНОМ СЛУЧАЕ РЕЗУЛЬТАТЫ ПОСЛЕДНЕГО СЕАНСА НЕ БУДУТ СОХРАНЕНЫ!");
                AwaitExit();
            }
            catch (UnauthorizedAccessException)
            {
                Console.Clear();
                Console.WriteLine("Нет разрешения на использование одного из файлов программы.");
                Console.WriteLine("Попробуйте завершить программу,");
                Console.WriteLine("затем вручную удалить файл config.txt, находящийся в папке решения.");
                Console.WriteLine("После этого перезапустите программу.");
                AwaitExit();
            }
            catch (NotSupportedException)
            {
                Console.Clear();
                Console.WriteLine("Один из файлов программы имеет неподдерживаемый формат.");
                Console.WriteLine("Попробуйте завершить программу,");
                Console.WriteLine("затем вручную удалить файл config.txt, находящийся в папке решения.");
                Console.WriteLine("После этого перезапустите программу.");
                AwaitExit();
            }
            catch (System.Security.SecurityException)
            {
                Console.Clear();
                Console.WriteLine("Нет разрешения на использование одного из файлов программы.");
                Console.WriteLine("Попробуйте завершить программу,");
                Console.WriteLine("затем вручную удалить файл config.txt, находящийся в папке решения.");
                Console.WriteLine("После этого перезапустите программу.");
                AwaitExit();
            }
        }

        /// <summary>
        /// Запрашивает данные для новой заметки и добавляет ее в базу данных.
        /// </summary>
        /// <param name="dataBase">База данных.</param>
        private static void AddNewNote(ref DataBase dataBase)
        {
            try
            {
                Console.Clear();

                RequestHeading(out string heading);

                Console.WriteLine();

                RequestMultiStringInput(out string text);
                RequestTags(dataBase, out List<string> tags);

                dataBase.AddNote(heading, text, tags);
            }
            catch (IOException)
            {
                Console.Clear();
                Console.WriteLine("Ошибка ввода-вывода.");
                Console.WriteLine("Попробуйте вернуться в главное меню и затем вновь воспользоваться функцией добавления заметки.");
                Console.WriteLine("НЕ ОСТАНАВЛИВАЙТЕ ПРИЛОЖЕНИЕ ПРИНУДИТЕЛЬНО!");
                Console.WriteLine("В ПРОТИВНОМ СЛУЧАЕ РЕЗУЛЬТАТЫ ПОСЛЕДНЕГО СЕАНСА НЕ БУДУТ СОХРАНЕНЫ!");
                AwaitExit();
            }
            catch (OutOfMemoryException)
            {
                Console.Clear();
                Console.WriteLine("Недостаточно памяти.");
                Console.WriteLine("Попробуйте освободить память устройства.");
                AwaitExit();
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Введена слишком длинная строка.");
                AwaitExit();
            }
        }

        /// <summary>
        /// Запрашивает заметки для удаления и удаляет их из базы данных.
        /// </summary>
        /// <param name="dataBase">База данныз.</param>
        private static void EraseNotes(ref DataBase dataBase)
        {
            try
            {
                if (CheckForEmptiness(dataBase))
                {
                    return;
                }

                string[] notesHeadings = PrepareHeadingsArray(dataBase);

                int[] idxs = CheckMenu.ProcessCheckMenu("Укажите, какие заметки должны быть удалены: ",
                    "Только незаархивированные заметки могут быть удалены", notesHeadings);

                dataBase.DeleteNotes(idxs);
            }
            catch (IOException)
            {
                Console.Clear();
                Console.WriteLine("Ошибка ввода-вывода.");
                Console.WriteLine("Попробуйте вернуться в главное меню и затем вновь воспользоваться функцией удаления заметки.");
                Console.WriteLine("НЕ ОСТАНАВЛИВАЙТЕ ПРИЛОЖЕНИЕ ПРИНУДИТЕЛЬНО!");
                Console.WriteLine("В ПРОТИВНОМ СЛУЧАЕ РЕЗУЛЬТАТЫ ПОСЛЕДНЕГО СЕАНСА НЕ БУДУТ СОХРАНЕНЫ!");
                AwaitExit();
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Во время ожидания ввода определенной клавиши была активирована сторонняя функция,");
                Console.WriteLine("не поддерживаемая данным приложением.");
                AwaitExit();
            }
        }

        /// <summary>
        /// Запрашивает данные для редактирования заметки и редактирует ее.
        /// </summary>
        /// <param name="dataBase">База данных.</param>
        private static void EditNote(ref DataBase dataBase)
        {
            try
            {
                if (CheckForEmptiness(dataBase))
                {
                    return;
                }

                string[] noteHeadings = PrepareHeadingsArray(dataBase);
                int number = ArrowMenu.ProcessArrowMenu(
                    "Укажите, какая заметка должна быть отредактирована:",
                    "Только незаархивированные заметки могут быть отредактированны", noteHeadings);

                int category = ArrowMenu.ProcessArrowMenu(
                    "Укажите, что должно быть изменено в заметке:", 
                    "", "Заголовок", "Текст", "Теги", "Отменить редактирование заметки");

                Console.Clear();
                switch (category)
                {
                    case 1:
                        RequestHeading(out string heading);
                        if (ConfirmAction("Вы уверены, что хотите заменить заголовок?"))
                        {
                            dataBase.Notes[number - 1].Heading = heading;
                        }
                        break;
                    case 2:
                        RequestMultiStringInput(out string text);
                        if (ConfirmAction("Вы уверены, что хотите заменить текст заметки?"))
                        {
                            dataBase.Notes[number - 1].Text = text;
                        }
                        break;
                    case 3:
                        RequestTags(dataBase, out List<string> tags);
                        if (ConfirmAction("Вы уверены, что хотите применить выбранный набор тегов?"))
                        {
                            dataBase.Notes[number - 1].Tags = tags;
                        }
                        break;
                    case 4:
                        return;
                }
            }
            catch (IOException)
            {
                Console.Clear();
                Console.WriteLine("Ошибка ввода-вывода.");
                Console.WriteLine("Попробуйте вернуться в главное меню и затем вновь воспользоваться функцией редактирования заметки.");
                Console.WriteLine("НЕ ОСТАНАВЛИВАЙТЕ ПРИЛОЖЕНИЕ ПРИНУДИТЕЛЬНО!");
                Console.WriteLine("В ПРОТИВНОМ СЛУЧАЕ РЕЗУЛЬТАТЫ ПОСЛЕДНЕГО СЕАНСА НЕ БУДУТ СОХРАНЕНЫ!");
                AwaitExit();
            }
            catch (OutOfMemoryException)
            {
                Console.Clear();
                Console.WriteLine("Недостаточно памяти.");
                Console.WriteLine("Попробуйте освободить память устройства.");
                AwaitExit();
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Введена слишком длинная строка.");
                AwaitExit();
            }
        }

        /// <summary>
        /// Выводит информацию о заметках в консоль.
        /// </summary>
        /// <param name="dataBase">База данных.</param>
        private static void ProcessNotesShowCase(DataBase dataBase)
        {
            try
            {
                int way = ArrowMenu.ProcessArrowMenu(
                    "Выберите действие:", "", "Просмотреть незаархивированные заметки",
                    "Просмотреть заархивированные заметки",
                    "Вернуться в меню выбора функций");

                if (way == 3)
                {
                    return;
                }

                if (CheckForEmptiness(dataBase, way == 2))
                {
                    return;
                }

                // Формируем массив заголовков на основе того, какую группу заметок просматриваем.
                string[] titles = way == 1 ? (new string[dataBase.Notes.Count + 1]) : (new string[dataBase.ArchivedNotes.Count + 1]);
                string[] headings = PrepareHeadingsArray(dataBase, way == 2);

                for (int i = 0; i < headings.Length; i++)
                {
                    titles[i] = headings[i];
                }
                titles[^1] = "Вернуться в меню";

                while (true)
                {
                    int key = ArrowMenu.ProcessArrowMenu("Выберите заметку для просмотра более детальной информации о ней:",
                        "", titles);

                    if (key == titles.Length)
                    {
                        return;
                    }

                    if (way == 1)
                    {
                        Console.WriteLine(dataBase.Notes[key - 1].FullInfo());
                    }
                    else
                    {
                        Console.WriteLine(dataBase.ArchivedNotes[key - 1].FullInfo());
                    }
                    AwaitExit("Нажмите Enter для возвращения в меню просмотра заметок");
                }
            }
            catch (IOException)
            {
                Console.Clear();
                Console.WriteLine("Ошибка ввода-вывода.");
                Console.WriteLine("Попробуйте вернуться в главное меню и затем вновь воспользоваться функцией просмотра заметок.");
                Console.WriteLine("НЕ ОСТАНАВЛИВАЙТЕ ПРИЛОЖЕНИЕ ПРИНУДИТЕЛЬНО!");
                Console.WriteLine("В ПРОТИВНОМ СЛУЧАЕ РЕЗУЛЬТАТЫ ПОСЛЕДНЕГО СЕАНСА НЕ БУДУТ СОХРАНЕНЫ!");
                AwaitExit();
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Во время ожидания ввода определенной клавиши была активирована сторонняя функция,");
                Console.WriteLine("не поддерживаемая данным приложением.");
                AwaitExit();
            }
        }

        /// <summary>
        /// Запрашивает данные для сортировки и инициализирует ее.
        /// </summary>
        /// <param name="dataBase">База данных</param>
        private static void ProcessSorting(DataBase dataBase)
        {
            try
            {
                if (CheckForEmptiness(dataBase))
                {
                    return;
                }

                Field[] fields = [Field.ID, Field.Heading, Field.Text, Field.Date, Field.Tags];

                int param = ArrowMenu.ProcessArrowMenu(
                    "Выберите параметр сортировки:", "К сортировке доступны только незаархивированные заметки",
                    "ID", "Заголовок (сортировка в алфавитном порядке)", "Текст (сортировка в алфавитном порядке)",
                    "Дата", "Количество тегов (в порядке возрастания)");

                SortingHandler sh = new(dataBase, fields[param - 1]);

                DataBase sortedBase = new()
                {
                    Notes = sh.SortDataBase()
                };

                ProcessNotesShowCase(sortedBase);

                if (ConfirmAction("Сохранить данные в отсортированном виде?"))
                {
                    dataBase.Notes = sortedBase.Notes;
                }
            }
            catch (IOException)
            {
                Console.Clear();
                Console.WriteLine("Ошибка ввода-вывода.");
                Console.WriteLine("Попробуйте вернуться в главное меню и затем вновь воспользоваться функцией сортировки заметок.");
                Console.WriteLine("НЕ ОСТАНАВЛИВАЙТЕ ПРИЛОЖЕНИЕ ПРИНУДИТЕЛЬНО!");
                Console.WriteLine("В ПРОТИВНОМ СЛУЧАЕ РЕЗУЛЬТАТЫ ПОСЛЕДНЕГО СЕАНСА НЕ БУДУТ СОХРАНЕНЫ!");
                AwaitExit();
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Во время ожидания ввода определенной клавиши была активирована сторонняя функция,");
                Console.WriteLine("не поддерживаемая данным приложением.");
                AwaitExit();
            }
        }

        /// <summary>
        /// Запрашивает данные для процесса фильтрации и запускает его.
        /// </summary>
        /// <param name="dataBase">База данных.</param>
        private static void ProcessFilterring(DataBase dataBase)
        {
            try
            {
                if (CheckForEmptiness(dataBase))
                {
                    return;
                }

                int[] filters = CheckMenu.ProcessCheckMenu("Укажите параметры фильтрации:",
                    "К фильтрации доступны только незаархивированные заметки", ["Диапазон дат",
                    "Ключевые слова в заголовке/тексте", "Теги"]);

                KeyValuePair<DateOnly, DateOnly> datesRange = new(new(), new());
                string keyWord = "";
                List<string> tags = [];

                if (filters.Contains(0))
                {
                    datesRange = RequestDatesRange();
                }
                if (filters.Contains(1))
                {
                    Console.Write("Введите ключевое слово: ");
                    keyWord = Console.ReadLine()!;
                }
                if (filters.Contains(2))
                {
                    RequestTags(dataBase, out tags);
                }

                DataBase filterredBase = new()
                {
                    Notes = dataBase.Notes,
                    Tags = dataBase.Tags,
                };
                FilterringHandler fh = new(ref filterredBase, datesRange, keyWord, [.. tags], filters);
                fh.ProcessFilterring();

                ProcessNotesShowCase(filterredBase);

                if (ConfirmAction("Сохранить данные в отфильтрованном виде?"))
                {
                    dataBase.Notes = filterredBase.Notes;
                }
            }
            catch (IOException)
            {
                Console.Clear();
                Console.WriteLine("Ошибка ввода-вывода.");
                Console.WriteLine("Попробуйте вернуться в главное меню и затем вновь воспользоваться функцией фильтрации заметок.");
                Console.WriteLine("НЕ ОСТАНАВЛИВАЙТЕ ПРИЛОЖЕНИЕ ПРИНУДИТЕЛЬНО!");
                Console.WriteLine("В ПРОТИВНОМ СЛУЧАЕ РЕЗУЛЬТАТЫ ПОСЛЕДНЕГО СЕАНСА НЕ БУДУТ СОХРАНЕНЫ!");
                AwaitExit();
            }
        }

        /// <summary>
        /// Запрашивает данные для процесса архивации и запускает его.
        /// </summary>
        /// <param name="dataBase">База данных.</param>
        private static void ProcessArchiving(ref DataBase dataBase)
        {
            try
            {
                int way = ArrowMenu.ProcessArrowMenu(
                    "Выберите действие:", "", "Заархивировать заметки", "Разархивировать заметки",
                    "Вернуться в меню выбора функций");

                if (way == 3)
                {
                    return;
                }

                if (CheckForEmptiness(dataBase, way == 2))
                {
                    return;
                }

                string[] headings = PrepareHeadingsArray(dataBase, way == 2);
                int[] chosen = CheckMenu.ProcessCheckMenu(
                    $"Выберите заметки, которые нужно {(way == 2 ? "Разархивировать" : "Заархивировать")}:",
                    "", headings);

                dataBase.TransferBetweenLists(chosen, way == 1);
            }
            catch (IOException)
            {
                Console.Clear();
                Console.WriteLine("Ошибка ввода-вывода.");
                Console.WriteLine("Попробуйте вернуться в главное меню и затем вновь воспользоваться функцией архивации заметок.");
                Console.WriteLine("НЕ ОСТАНАВЛИВАЙТЕ ПРИЛОЖЕНИЕ ПРИНУДИТЕЛЬНО!");
                Console.WriteLine("В ПРОТИВНОМ СЛУЧАЕ РЕЗУЛЬТАТЫ ПОСЛЕДНЕГО СЕАНСА НЕ БУДУТ СОХРАНЕНЫ!");
                AwaitExit();
            }
        }

        /// <summary>
        /// Выводит информацию о тегах в консоль.
        /// </summary>
        /// <param name="dataBase">База данных.</param>
        private static void PrintTagsStats(DataBase dataBase)
        {
            try
            {
                int[] nums = dataBase.GetTagsStats();

                Console.WriteLine("Статистика по всем тегам:\n");
                for (int i = 0; i < nums.Length; i++)
                {
                    Console.WriteLine($"{dataBase.Tags[i]} : {nums[i]} заметок");
                }

                AwaitExit();
            }
            catch (IOException)
            {
                Console.Clear();
                Console.WriteLine("Ошибка ввода-вывода.");
                Console.WriteLine("Попробуйте вернуться в главное меню и затем вновь воспользоваться функцией просмотра статистики.");
                Console.WriteLine("НЕ ОСТАНАВЛИВАЙТЕ ПРИЛОЖЕНИЕ ПРИНУДИТЕЛЬНО!");
                Console.WriteLine("В ПРОТИВНОМ СЛУЧАЕ РЕЗУЛЬТАТЫ ПОСЛЕДНЕГО СЕАНСА НЕ БУДУТ СОХРАНЕНЫ!");
                AwaitExit();
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Во время ожидания ввода определенной клавиши была активирована сторонняя функция,");
                Console.WriteLine("не поддерживаемая данным приложением.");
                AwaitExit();
            }
        }

        /// <summary>
        /// Запрашивает данные для создания нового тега.
        /// </summary>
        /// <param name="dataBase">База данных.</param>
        private static void CreateNewTag(ref DataBase dataBase)
        {
            try
            {
                Console.Clear();
                Console.Write("Введите название нового тега: ");
                dataBase.Tags.Add(Console.ReadLine()!);
            }
            catch (IOException)
            {
                Console.Clear();
                Console.WriteLine("Ошибка ввода-вывода.");
                Console.WriteLine("Попробуйте вернуться в главное меню и затем вновь воспользоваться функцией создания тега.");
                Console.WriteLine("НЕ ОСТАНАВЛИВАЙТЕ ПРИЛОЖЕНИЕ ПРИНУДИТЕЛЬНО!");
                Console.WriteLine("В ПРОТИВНОМ СЛУЧАЕ РЕЗУЛЬТАТЫ ПОСЛЕДНЕГО СЕАНСА НЕ БУДУТ СОХРАНЕНЫ!");
                AwaitExit();
            }
            catch (OutOfMemoryException)
            {
                Console.Clear();
                Console.WriteLine("Недостаточно памяти.");
                Console.WriteLine("Попробуйте освободить память устройства.");
                AwaitExit();
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Введена слишком длинная строка.");
                AwaitExit();
            }
        }

        /// <summary>
        /// Запрашивает данные для обновления пути к одному из фалов системы.
        /// </summary>
        /// <param name="dataBase">База данных.</param>
        private static void UpdatePath(DataBase dataBase)
        {
            try
            {
                Console.Write("Введите путь к новому файлу БЕЗ КАВЫЧЕК, БЕЗ РАСШИРЕНИЯ И БЕЗ ТОЧЕК:");
                string path = Console.ReadLine()!;

                int key = ArrowMenu.ProcessArrowMenu("Укажите, путь к какому файлу нужно изменить:", "",
                    "Путь к файлу с заметками", "Путь к файлу с тегами");

                if (CheckPath(path))
                {
                    dataBase.ChangeFiles(path, key == 1);
                }
            }
            catch (IOException)
            {
                Console.Clear();
                Console.WriteLine("Ошибка ввода-вывода.");
                Console.WriteLine("Попробуйте вернуться в главное меню и затем вновь воспользоваться функцией обновления пути к файлу.");
                Console.WriteLine("НЕ ОСТАНАВЛИВАЙТЕ ПРИЛОЖЕНИЕ ПРИНУДИТЕЛЬНО!");
                Console.WriteLine("В ПРОТИВНОМ СЛУЧАЕ РЕЗУЛЬТАТЫ ПОСЛЕДНЕГО СЕАНСА НЕ БУДУТ СОХРАНЕНЫ!");
                AwaitExit();
            }
            catch (OutOfMemoryException)
            {
                Console.Clear();
                Console.WriteLine("Недостаточно памяти.");
                Console.WriteLine("Попробуйте освободить память устройства.");
                AwaitExit();
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Введена слишком длинная строка.");
                AwaitExit();
            }
        }

        /// <summary>
        /// Запрашивает заголовок для заметки.
        /// </summary>
        /// <param name="heading">Заголовок.</param>
        private static void RequestHeading(out string heading)
        {
            Console.Write("Введите заголовок заметки: ");
            heading = Console.ReadLine()!;
        }

        /// <summary>
        /// Запрашивает многострочный ввод.
        /// </summary>
        /// <param name="text">Текст.</param>
        private static void RequestMultiStringInput(out string text)
        {
            Console.WriteLine("Введите текст заметки (Поддерживается многострочный ввод)");
            Console.WriteLine("(Для окончания ввода введите на новой строке: >>>stop<<<)");

            string str;
            text = "";
            while (true)
            {
                str = Console.ReadLine()!;
                if (str == ">>>stop<<<")
                {
                    return;
                }
                text += str + "\n";
            }
        }

        /// <summary>
        /// Запрашивает набор тегов.
        /// </summary>
        /// <param name="dataBase">База данных.</param>
        /// <param name="tags">Набор тегов.</param>
        private static void RequestTags(DataBase dataBase, out List<string> tags)
        {
            tags = [];
            if (dataBase.Tags.Count == 0)
            {
                return;
            }

            int[] idxs = CheckMenu.ProcessCheckMenu(
                "Выберите теги, которые следует присвоить заметке",
                "", [.. dataBase.Tags]);

            foreach (int idx in idxs)
            {
                tags.Add(dataBase.Tags[idx]);
            }
        }

        /// <summary>
        /// Запрашивает диапазон дат.
        /// </summary>
        /// <returns>Пара, ключ - левая граница дат, значение - правая граница.</returns>
        private static KeyValuePair<DateOnly, DateOnly> RequestDatesRange()
        {
            Console.Clear();
            Console.WriteLine("Введите левую границу дат:");
            DateOnly left = RequestDate();
            Console.WriteLine("Введите правую границу дат:");
            DateOnly right = RequestDate();
            return new(left, right);
        }

        /// <summary>
        /// Запрашивает дату.
        /// </summary>
        /// <returns>Дата.</returns>
        private static DateOnly RequestDate()
        {
            Console.Write("Введите год: ");
            int year = int.Parse(Console.ReadLine()!);
            Console.Write("Введите номер месяца: ");
            int month = int.Parse(Console.ReadLine()!);
            Console.Write("Введите число месяца: ");
            int day = int.Parse(Console.ReadLine()!);
            return new(year, month, day);
        }

        /// <summary>
        /// Проверяет указанный путь на корректность.
        /// </summary>
        /// <param name="path">Путь.</param>
        /// <returns>true, если файл по этому пути может быть создан, false - иначе.</returns>
        private static bool CheckPath(string path)
        {
            try
            {
                Console.Clear();
                if (path.Contains('.') || path.Contains('\'') || path.Contains('\"'))
                {
                    throw new ArgumentException();
                }

                File.WriteAllText(path + ".txt", "test, delete this file if it exists.");
                File.Delete(path + ".txt");
                return true;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Введенный путь содержит недопустимые символы.");
                Console.WriteLine("Введите другой путь.");
                AwaitExit();
                return false;
            }
            catch (PathTooLongException)
            {
                Console.WriteLine("Введен слишком длинный путь к файлу.");
                AwaitExit();
                return false;
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Не найден директорий по указанному пути к файлу.");
                AwaitExit();
                return false;
            }
            catch (IOException)
            {
                Console.WriteLine("Ошибка ввода-вывода.");
                Console.WriteLine("Попробуйте вернуться в главное меню и затем вновь воспользоваться функцией обновления пути к файлу.");
                Console.WriteLine("НЕ ОСТАНАВЛИВАЙТЕ ПРИЛОЖЕНИЕ ПРИНУДИТЕЛЬНО!");
                Console.WriteLine("В ПРОТИВНОМ СЛУЧАЕ РЕЗУЛЬТАТЫ ПОСЛЕДНЕГО СЕАНСА НЕ БУДУТ СОХРАНЕНЫ!");
                AwaitExit();
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Нет разрешения на использование данного файла или директория.");
                Console.WriteLine("Попробуйте разместить новый файл в другом директории.");
                AwaitExit();
                return false;
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("Путь имеет недопустимый формат.");
                Console.WriteLine("Попробуйте исмпользовать путь к уже существующему файлу.");
                AwaitExit();
                return false;
            }
            catch (System.Security.SecurityException)
            {
                Console.WriteLine("Нет разрешения на использование данного файла или директория.");
                Console.WriteLine("Попробуйте разместить новый файл в другом директории.");
                AwaitExit();
                return false;
            }
        }

        /// <summary>
        /// Формирует массив заголовков.
        /// </summary>
        /// <param name="dataBase">База данных.</param>
        /// <param name="archived">Установить true, если нужно сформировать массив заголовков заархивированных заметок.</param>
        /// <returns></returns>
        private static string[] PrepareHeadingsArray(DataBase dataBase, bool archived = false)
        {
            string[] ans;
            if (!archived)
            {
                ans = new string[dataBase.Notes.Count];

                for (int i = 0; i < ans.Length; i++)
                {
                    ans[i] = dataBase.Notes[i].ToString();
                }
            }
            else
            {
                ans = new string[dataBase.ArchivedNotes.Count];

                for (int i = 0; i < ans.Length; i++)
                {
                    ans[i] = dataBase.ArchivedNotes[i].ToString();
                }
            }

            return ans;
        }

        /// <summary>
        /// Выводит на экран указанное сообщение и останавливает выполнение программы до момента нажатия запрашиваемой клавиши.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <exception cref="InvalidOperationException"></exception>
        private static void AwaitExit(string text = "Нажмите Enter для продолжения", ConsoleKey key = ConsoleKey.Enter)
        {
            Console.WriteLine(text);
            while (Console.ReadKey().Key != key)
            {
                continue;
            }
            Console.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataBase"></param>
        /// <param name="archived"></param>
        /// <exception cref="IOException"></exception>
        /// <returns></returns>
        private static bool CheckForEmptiness(DataBase dataBase, bool archived = false)
        {
            Console.Clear();
            if (archived && dataBase.ArchivedNotes.Count == 0)
            {
                Console.WriteLine("Не существует ни одной заархивированной заметки");
                AwaitExit();
                return true;
            }
            if (!archived && dataBase.Notes.Count == 0)
            {
                Console.WriteLine("Не существует ни одной незаархивированной заметки");
                AwaitExit();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Запрашивает подтверждение действия.
        /// </summary>
        /// <param name="message">Сообщение, выводимое в консоль.</param>
        /// <returns>true, если действие подтверждено, false - иначе.</returns>
        private static bool ConfirmAction(string message)
        {
            return ArrowMenu.ProcessArrowMenu(message, "", "Да", "Нет") == 1;
        }

        /// <summary>
        /// Выводит в консоль ReadMe.
        /// </summary>
        private static void LoadReadMe()
        {
            Console.WriteLine("Данная программа представляет из себя менеджер заметок.");
            Console.WriteLine("Все взаимодействия с программой производятся через консоль.");
            Console.WriteLine("Для сохранения данных последнего сеанса, необходимо завершить работу программы");
            Console.WriteLine("через экранное меню, а не через закрытие окна консоли.");
            Console.WriteLine();
            Console.WriteLine("Внимание. Известные разработчику инструменты .NET для очистки консоли не");
            Console.WriteLine("позволяют очистить консоль полностью, в случае если в консоль был введен/выведен текст");
            Console.WriteLine("больше ~30 строк консольного окна. В связи с этим крайне не рекомендуется,");
            Console.WriteLine("по возможности, превышать данный лимит. В протианом случае консоль будет засорена до");
            Console.WriteLine("конца сеанса.");
            Console.WriteLine();
            Console.WriteLine("  ^       ^ ");
            Console.WriteLine(" / \\     / \\");
            Console.WriteLine("/   -----   \\");
            Console.WriteLine("|   O   O   |");
            Console.WriteLine("| ==  *  == |");
            Console.WriteLine("\\    \\_/    /       Курсор желает вам приятной работы");
            Console.WriteLine(" -----------        и благодарит за все прошедшые проекты. До встречи!");
            Console.WriteLine();

            Console.WriteLine("Зарегестрированная торговая марка CursorTechnologies.Inc");
            Console.WriteLine("Все авторские права защищены");
            Console.WriteLine("Исполнитель и разработчик: Кудрявцев Георгий, БПИ 245-1, Вариант 7");
            Console.WriteLine();

            AwaitExit();
        }
    }
}
