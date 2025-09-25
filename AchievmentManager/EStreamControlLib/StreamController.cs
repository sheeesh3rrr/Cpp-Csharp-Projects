using System.Reflection.PortableExecutable;
using System.IO;
using System.Text;

namespace EStreamControlLib
{
    /// <summary>
    /// Контролирует работу потоков.
    /// </summary>
    public static class StreamController
    {
        /// <summary>
        /// Загружает данные из стандартного потока.
        /// </summary>
        /// <returns></returns>
        public static string LoadThroughConsole()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            string ans = "";

            using (StreamReader sr = new(Console.OpenStandardInput(), Encoding.UTF8))
            {
                Console.WriteLine("Вставьте JSON, после чего на новой строке введите stop. Далее - нажмите Enter");
                string str = "";
                string local = "";

                while (true)
                {
                    if (str == "stop")
                    {
                        break;
                    }
                    local += str + '\n';
                    str = Console.ReadLine()!;
                }
                ans = local;
                sr.Dispose();
            }
            return ans;
        }

        /// <summary>
        /// Загружает данные из файлового потока.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <returns></returns>
        public static string LoadThroughFile(string path)
        {
            using StreamReader sr = new(path, Encoding.UTF8);
            return sr.ReadToEnd();
        }

        /// <summary>
        /// Выгружает данные в стандартный поток.
        /// </summary>
        /// <param name="json">Строка, которую необходимо вывести в консоль.</param>
        public static void WriteThroughConsole(string json)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            using StreamWriter sw = new(Console.OpenStandardOutput(), Encoding.UTF8);
            Console.WriteLine(json);
            sw.Dispose();
        }

        /// <summary>
        /// Выгружает данные в файловый поток.
        /// </summary>
        /// <param name="json">Строка, которую необходимо записать в файл.</param>
        /// <param name="path">Путь к файлу.</param>
        public static void WriteThroughFile(string json, string path)
        {
            using StreamWriter sw = new(path);
            sw.WriteLine(json);
        }
    }
}
