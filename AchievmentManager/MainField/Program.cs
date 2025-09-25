using EMenuLib;

namespace MainField
{
    /// <summary>
    /// Основной класс программы.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Точка входа.
        /// </summary>
        public static void Main()
        {
            while (true)
            {
                try
                {
                    int key = ArrowMenu.ProcessArrowMenu("Добро пожаловать!",
                        "Для смены позиции указателя используйте стрелочки (вверх/вниз). Для выбора пункта нажмите Enter",
                        "Старт", "ReadMe", "Выйти");
                    switch (key)
                    {
                        case 1:
                            ActionHandler.InitiateMainOperations();
                            break;
                        case 2:
                            ActionHandler.LoadReadMe();
                            break;
                        case 3:
                            return;

                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Необработанная ошибка");
                    Console.WriteLine("Нажмите любую клавишу, чтобы продолжить");
                    _ = Console.ReadKey();
                }
            }
        }
    }
}
