using EMenuLib;
using EDataBaseLib;
using EStreamControlLib;
using EAchievmentsLib;
using System.Collections;


namespace MainField
{
    internal static class ActionHandler
    {
        /// <summary>
        /// Запускает основные функции программы.
        /// </summary>
        public static void InitiateMainOperations()
        {
            DataBase dataBase = new();
            while (true)
            {
                int key = ArrowMenu.ProcessArrowMenu("Выберите функцию", "",
                    $"{(dataBase.IsEmpty ? "Загрузить данные" : "Изменить данные")}",
                    "Вывести данные", "Отфильтровать данные", "Отсортировать данные", 
                    "Просмотреть достижения и группы достижений", "Вернуться в главное меню");

                try
                {
                    // Обработка выбора пункта меню.
                    switch (key)
                    {
                        case 1: RequestData(ref dataBase); break;
                        case 2: SubmitData(dataBase); break;
                        case 3: ProcessFiltering(dataBase); break;
                        case 4: ProcessSorting(dataBase); break;
                        case 5: ProcessGroupsShowCase(dataBase); break;
                        case 6: return;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Данные не могут быть обработаны.");
                    AwaitExit("Для продолжения нажмите Enter", ConsoleKey.Enter);
                }
            }
        }

        /// <summary>
        /// Показывает ReadMe.
        /// </summary>
        public static void LoadReadMe()
        {
            Console.WriteLine("Данная программа предоставляет инструменты для работы с JSON-файлами,");
            Console.WriteLine("содержащими информацию о достижениях из игры Cultist Simulator");
            Console.WriteLine();

            Console.WriteLine("Требования к JSON-файлам:");
            Console.WriteLine("Программа обрабатывает только JSON-файлы, соответствующие стандарту RFC 8259");
            Console.WriteLine("В случае несоответствия переданного файла стандарту, информация из него не будет считана");
            Console.WriteLine();

            Console.WriteLine("Программа поддерживает функцию вставки JSON-файла непосредственно в консоль");
            Console.WriteLine("Убедительная просьба, не использовать данную функцию при работе с большими файлами (более 20 строк),");
            Console.WriteLine("так как в таком случае засоряется буфер консоли, что может привести к некорректному выводу и");
            Console.WriteLine("работе некоторых функций");
            Console.WriteLine();

            Console.WriteLine("Внимание. При демонстрации TUI, карточки объектов содержат только первые 7 достижений,");
            Console.WriteLine("относящихся к данной категории, чтобы избежать засорения консоли.");

            Console.WriteLine();
            Console.WriteLine("  ^       ^ ");
            Console.WriteLine(" / \\     / \\");
            Console.WriteLine("/   -----   \\");
            Console.WriteLine("|   O   O   |");
            Console.WriteLine("| ==  *  == |");
            Console.WriteLine("\\    \\_/    /       Курсор желает вам");
            Console.WriteLine(" -----------        Приятной работы!");
            Console.WriteLine();

            Console.WriteLine("Зарегестрированная торговая марка CursorTechnologies.Inc");
            Console.WriteLine("Все авторские права защищены");
            Console.WriteLine("Исполнитель и разработчик: Кудрявцев Георгий, БПИ 245-1, Вариант 10");
            Console.WriteLine();

            AwaitExit();
        }

        /// <summary>
        /// Запрашивает способ получения данных у пользователя.
        /// </summary>
        /// <param name="dataBase">База данных.</param>
        private static void RequestData(ref DataBase dataBase)
        {
            int key = ArrowMenu.ProcessArrowMenu(
                "Выберите метод получения данных", "",
                "Ввод JSON в консоль (Крайне НЕ рекомендуется с большими данными, засоряется консоль)", 
                "Чтение JSON по переданному пути",
                "Вернуться в меню выбора функции"
                );

            switch (key)
            {
                case 1: GetThroughConsole(ref dataBase); break;
                case 2: GetThroughFile(ref dataBase); break;
                case 3: return;
            }
        }

        /// <summary>
        /// Запускает операции для получения данных из консоли.
        /// </summary>
        /// <param name="dataBase">База данных.</param>
        private static void GetThroughConsole(ref DataBase dataBase)
        {
            try
            {
                ClearUp();
                string input = StreamController.LoadThroughConsole();
                dataBase.Clear();
                dataBase.Build(JsonParser.ReadJson(input));
            }
            catch (OutOfMemoryException)
            {
                Console.WriteLine("Недостаточно памяти для загрузки данных");
                AwaitExit();
            }
            catch (IOException)
            {
                Console.WriteLine("Ошибка чтения файла");
                AwaitExit();
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Ошибка инициализации чтения");
                AwaitExit();
            }
            catch (FormatException)
            {
                Console.WriteLine("Некорректная структуризация JSON-файла");
                AwaitExit();
            }
            catch (System.Security.SecurityException)
            {
                Console.WriteLine("Ошибка безопасности");
                AwaitExit();
            }
            catch (Exception)
            {
                Console.WriteLine("Неизвестная ошибка");
                AwaitExit();
            }
        }

        /// <summary>
        /// Запускает операции для получения данных из файла.
        /// </summary>
        /// <param name="dataBase">База данных.</param>
        private static void GetThroughFile(ref DataBase dataBase)
        {
            try
            {
                ClearUp();
                Console.Write("Введите путь до файла БЕЗ кавычек: ");

                string? path = Console.ReadLine();
                if (path is null or "")
                {
                    Console.WriteLine("Введен пустой путь");
                    AwaitExit();
                    return;
                }

                string input = StreamController.LoadThroughFile(path);
                dataBase.Clear();
                dataBase.Build(JsonParser.ReadJson(input));
            }
            catch (OutOfMemoryException)
            {
                Console.WriteLine("Недостаточно памяти для загрузки данных");
                AwaitExit();
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Не найден директорий файла");
                AwaitExit();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл не найден");
                AwaitExit();
            }
            catch (IOException)
            {
                Console.WriteLine("Ошибка чтения файла");
                AwaitExit();
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Ошибка инициализации чтения");
                AwaitExit();
            }
            catch (FormatException)
            {
                Console.WriteLine("Некорректная структуризация JSON-файла");
                AwaitExit();
            }
            catch (Exception)
            {
                Console.WriteLine("Неизвестная ошибка");
                AwaitExit();
            }

        }

        /// <summary>
        /// Запрашивает метод отправки данных.
        /// </summary>
        /// <param name="dataBase">База данных.</param>
        private static void SubmitData(DataBase dataBase)
        {
            if (dataBase.IsEmpty)
            {
                Console.WriteLine("Пустые данные. Выполнение операции невозможно.");
                AwaitExit();
                return;
            }

            int key = ArrowMenu.ProcessArrowMenu(
                "Выберите метод отпраки данных", "", "Демонстрация данных в консоли в читабельном виде",
                "Отправка через консоль", "Отправка через файл по переданному пути",
                "Вернуться в меню выбора функции"
                );

            switch (key)
            {
                case 1: DemonstrateThroughConsole(dataBase); break;
                case 2: SubmitThroughConsole(dataBase); break;
                case 3: SubmitThroughFile(dataBase); break;
                case 4: return;
            }
        }

        /// <summary>
        /// Демонстрирует данные в консоли в читабельном виде.
        /// </summary>
        /// <param name="dataBase"></param>
        private static void DemonstrateThroughConsole(DataBase dataBase)
        {
            try
            {
                ClearUp();

                Console.WriteLine("Категории:\n");
                foreach (AchievmentGroup group in dataBase.Groups)
                {
                    Console.WriteLine(group);
                    Console.WriteLine();
                }
                Console.WriteLine("\nДостижения: ");
                foreach (Achievment achievment in dataBase.Achievments)
                {
                    Console.WriteLine(achievment);
                    Console.WriteLine();
                }
                AwaitExit();
            }
            catch (IOException)
            {
                Console.WriteLine("Ошибка ввода-вывода");
                AwaitExit();
            }
            catch (Exception)
            {
                Console.WriteLine("Неизвестная ошибка");
                AwaitExit();
            }
        }

        /// <summary>
        /// Запускает операции для вывода данных в консоль.
        /// </summary>
        /// <param name="dataBase">База данных.</param>
        private static void SubmitThroughConsole(DataBase dataBase)
        {
            try
            {
                ClearUp();

                string toSubmit = JsonParser.WriteJSON(dataBase.Groups, dataBase.Achievments);
                StreamController.WriteThroughConsole(toSubmit);
                AwaitExit();
            }
            catch (IOException)
            {
                Console.WriteLine("Ошибка ввода-вывода");
                AwaitExit();
            }
            catch (Exception)
            {
                Console.WriteLine("Неизвестная ошибка");
                AwaitExit();
            }
        }

        /// <summary>
        /// Запускает операции для вывода данных в файл.
        /// </summary>
        /// <param name="dataBase">База данных.</param>
        private static void SubmitThroughFile(DataBase dataBase)
        {
            try
            {
                ClearUp();

                Console.Write("Введите путь к файлу БЕЗ кавычек: ");
                string path = Console.ReadLine()!;

                string toSubmit = JsonParser.WriteJSON(dataBase.Groups, dataBase.Achievments);
                StreamController.WriteThroughFile(toSubmit, path);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Невозможно записать данные в файл");
                AwaitExit();
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Не найден директорий сохранения файла");
                AwaitExit();
            }
            catch (PathTooLongException)
            {
                Console.WriteLine("Слишком длинный путь до файла");
                AwaitExit();
            }
            catch (IOException)
            {
                Console.WriteLine("Ошибка ввода-вывода");
                AwaitExit();
            }
            catch (System.Security.SecurityException)
            {
                Console.WriteLine("Ошибка безопасности");
                AwaitExit();
            }
            catch (Exception)
            {
                Console.WriteLine("Неизвестная ошибка");
                AwaitExit();
            }
        }

        /// <summary>
        /// Запрашивает категорию данных для фильтрации.
        /// </summary>
        /// <param name="dataBase">База данных.</param>
        private static void ProcessFiltering(DataBase dataBase)
        {
            if (dataBase.IsEmpty)
            {
                Console.WriteLine("Пустые данные. Выполнение операции невозможно.");
                AwaitExit();
                return;
            }

            try
            {
                int key = ArrowMenu.ProcessArrowMenu(
                    "По каким группам объектов следует произвести фильтрацию?",
                    "", "Категории", "Достижения", "Вернуться в меню выбора функций");
                List<IAchievmentObject> lst;

                switch (key)
                {
                    case 1:
                        lst = ProcessGroupFiltering(dataBase);
                        InitiateAfterWorkOperations(lst);
                        break;
                    case 2:
                        lst = ProcessAchievmentsFiltering(dataBase);
                        InitiateAfterWorkOperations(lst);
                        break;
                    case 3: return;
                }
            }
            catch (IOException)
            {
                Console.WriteLine("Ошибка ввода-вывода");
                AwaitExit();
            }
            catch (OutOfMemoryException)
            {
                Console.WriteLine("Недостаточно памяти для произведения операции");
                AwaitExit();
            }
            catch (Exception)
            {
                Console.WriteLine("Неизвестная ошибка");
                AwaitExit();
            }
        }

        /// <summary>
        /// Запускает все процессы, связанные с фильтрацией групп.
        /// </summary>
        /// <param name="dataBase">База данных</param>
        /// <returns>Отфильтрованный список объектов</returns>
        private static List<IAchievmentObject> ProcessGroupFiltering(DataBase dataBase)
        {
            int key = ArrowMenu.ProcessArrowMenu(
                "По какому полю следует произвести фильтрацию?",
                "", "ID", "Ярлык", "Иконка"
                );
            Console.Write("Введите значение поля, по которому будет производиться фильтрация: ");
            string val = Console.ReadLine()!;

            List<IAchievmentObject> toStay = GetToStayObjects([.. dataBase.Groups], dataBase.GetGroupsIDs());
            List<IAchievmentObject> filtered = dataBase.Filter(key, val, AchievmentObjectType.Group);

            for (int i = 0; i < toStay.Count; i++)
            {
                if (!filtered.Contains(toStay[i]))
                {
                    filtered.Add(toStay[i]);
                }
            }

            return filtered;
        }

        /// <summary>
        /// Запускает все процессы, связанные с фильтрацией достижений.
        /// </summary>
        /// <param name="dataBase">База данных</param>
        /// <returns>Отфильтрованный список объектов</returns>
        private static List<IAchievmentObject> ProcessAchievmentsFiltering(DataBase dataBase)
        {
            int key = ArrowMenu.ProcessArrowMenu(
                "По какому полю следует произвести фильтрацию?",
                "", "ID", "Категория", "Иконка", "Ярлык", "Общее описание"
                ) - 1;
            Console.Write("Введите значение поля, по которому будет производиться фильтрация: ");
            string val = Console.ReadLine()!;
            key = key > 2 ? key + 2 : key;

            List<IAchievmentObject> toStay = GetToStayObjects([.. dataBase.Achievments], dataBase.GetAcievmentsIDs());
            List<IAchievmentObject> filtered = dataBase.Filter(key, val, AchievmentObjectType.Single);

            for (int i = 0; i < toStay.Count; i++)
            {
                if (!filtered.Contains(toStay[i]))
                {
                    filtered.Add(toStay[i]);
                }
            }

            return filtered;
        }

        /// <summary>
        /// Запускает операции, идущие после фильтрации и сортировки.
        /// </summary>
        /// <param name="prepared">Список объектов.</param>
        private static void InitiateAfterWorkOperations(List<IAchievmentObject> prepared)
        {
            int key = ArrowMenu.ProcessArrowMenu("Что необходимо сделать с отфильтрованными данными?",
                "", "Вывести в консоль в читабельном виде", "Сохранить в файл по указанному пути");

            DataBase local = new();
            local.Build(prepared, false);
            switch (key)
            {
                case 1:
                    DemonstrateThroughConsole(local);
                    break;
                case 2:
                    SubmitThroughFile(local); 
                    break;
            }
        }

        /// <summary>
        /// Получает объекты, которые должны остаться в выборке, несмотря на фильтрацию.
        /// </summary>
        /// <param name="objects">Фильтруемые объекты.</param>
        /// <param name="titles">Заголовки объектов.</param>
        /// <returns></returns>
        private static List<IAchievmentObject> GetToStayObjects(IAchievmentObject[] objects, string[] titles)
        {
            List<IAchievmentObject> ans = [];
            int[] indexes = CheckMenu.ProcessCheckMenu(
                "Выберите элементы, которые должны остаться в выборке, несмотря на фильтрацию: ", 
                "", titles);

            foreach (int index in indexes)
            {
                ans.Add(objects[index]);
            }

            return ans;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataBase"></param>
        private static void ProcessSorting(DataBase dataBase)
        {
            if (dataBase.IsEmpty)
            {
                Console.WriteLine("Пустые данные. Выполнение операции невозможно.");
                AwaitExit();
                return;
            }

            try
            {
                int key = ArrowMenu.ProcessArrowMenu(
                    "По каким группам объектов следует произвести сортировку?",
                    "", "Категории", "Достижения", "Вернуться в меню выбора функций");
                List<IAchievmentObject> lst;

                switch (key)
                {
                    case 1:
                        lst = ProcessGroupSorting(dataBase);
                        InitiateAfterWorkOperations(lst);
                        break;
                    case 2:
                        lst = ProcessAchievmentSorting(dataBase);
                        InitiateAfterWorkOperations(lst);
                        break;
                    case 3: return;
                }
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Невозможно произвести сортировку");
                AwaitExit();
            }
            catch (Exception)
            {
                Console.WriteLine("Неизвестная ошибка");
                AwaitExit();
            }
        }

        private static List<IAchievmentObject> ProcessGroupSorting(DataBase dataBase)
        {
            int key = ArrowMenu.ProcessArrowMenu(
                "По какому полю следует произвести сортировку?",
                "Сортировка будет производиться на основе сравнения значений по алфавитному порядку", 
                "ID", "Ярлык", "Иконка"
                );

            return dataBase.Sort(key, AchievmentObjectType.Group);
        }

        private static List<IAchievmentObject> ProcessAchievmentSorting(DataBase dataBase)
        {
            int key = ArrowMenu.ProcessArrowMenu(
                "По какому полю следует произвести сортировку?",
                "Сортировка будет производиться на основе сравнения значений по алфавитному порядку", 
                "ID", "Категория", "Иконка", "Ярлык", "Общее описание"
                );

            return dataBase.Sort(key, AchievmentObjectType.Single);
        }

        /// <summary>
        /// Запрашивает категорию данных для сортировки.
        /// </summary>
        /// <param name="dataBase">База данных.</param>
        private static void ProcessGroupsShowCase(DataBase dataBase)
        {
            if (dataBase.IsEmpty)
            {
                Console.WriteLine("Пустые данные. Выполнение операции невозможно.");
                AwaitExit();
                return;
            }

            try
            {
                AchievmentGroupCard[] cads = PrepareCards(dataBase.Groups);

                ShadeMenu.ProcessShadeMenu
                    ("Используйте стрелочки (вверх/вниз) для перемещения между карточками и (влево/вправо) для переключения страниц",
                    "Используйте Enter, чтобы свернуть/развернуть карточку и Esc для выхода из режима просмотра", cads);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
                AwaitExit();
            }
            catch (IOException)
            {
                Console.WriteLine("Ошибка ввода-вывода");
                AwaitExit();
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Невозможно обработать нажатую клавишу");
                AwaitExit();
            }
            catch (Exception)
            {
                Console.WriteLine("Неизвестная ошибка");
                AwaitExit();
            }
        }

        /// <summary>
        /// Готовит карточки групп достижений.
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        private static AchievmentGroupCard[] PrepareCards(List<AchievmentGroup> groups)
        {
            AchievmentGroupCard[] ans = new AchievmentGroupCard[groups.Count];

            for (int i = 0; i < groups.Count; i++)
            {
                List<Achievment> achs = groups[i].Achievments;
                string[] toWrite = new string[achs.Count];
                for (int j = 0; j < achs.Count; j++)
                {
                    toWrite[j] = achs[j].ID!;
                }

                ans[i] = new AchievmentGroupCard(groups[i].ID!, toWrite);
            }

            return ans;
        }

        /// <summary>
        /// Выводит на экран указанное сообщение и останавливает выполнение программы до момента нажатия запрашиваемой клавиши.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        private static void AwaitExit(
            string text = "Введите Enter для продолжения", ConsoleKey key = ConsoleKey.Enter)
        {
            Console.WriteLine(text);
            while (Console.ReadKey().Key != key)
            {
                continue;
            }
        }

        /// <summary>
        /// Очищает консоль. (Работает немного лучше Console.Clear()).
        /// </summary>
        private static void ClearUp()
        {
            Console.WriteLine();
            Console.Clear();
        }
    }
}
