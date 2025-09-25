using EParserLib;
using ENoteLib;

namespace EFileLib
{
    public static class FileHandler
    {
        private static readonly char _sl = Path.DirectorySeparatorChar;
        private static readonly string _configFilePath = $"..{_sl}..{_sl}..{_sl}config.txt"; // Файл, содержащий пути к остальным файлам системы.
        private static string _dataBasePath = $"..{_sl}..{_sl}..{_sl}notes.json"; // Файл с заметками.
        private static string _tagsFilePath = $"..{_sl}..{_sl}..{_sl}tags.txt"; // Файл с тегами.

        /// <summary>
        /// Путь до файла с заметками.
        /// </summary>
        public static string MemoryFilePath
        {
            get => _dataBasePath;
            set
            {
                _dataBasePath = value;
                File.WriteAllText(_configFilePath, value + "\n" + _tagsFilePath); // Запись изменений в файл конфигурации.
            }
        }

        /// <summary>
        /// Путь до файла с тегами.
        /// </summary>
        public static string TagsFilePath
        {
            get => _tagsFilePath;
            set
            {
                _tagsFilePath = value;
                File.WriteAllText(_configFilePath, _dataBasePath + "\n" + value); // Запись изменений в файл конфигурации.
            }
        }

        /// <summary>
        /// Читает Информацию из файлов, содержащих информацию о заметках и тегах.
        /// </summary>
        /// <returns>Список заметок.</returns>
        public static (List<Note>, List<string>) ReadFiles()
        {
            string json = File.ReadAllText(_dataBasePath);
            string[] tags = File.ReadAllLines(_tagsFilePath);
            return (Parser.ReadJson(json), [.. tags]);
        }

        /// <summary>
        /// Записывает информацию о переданном списке заметок в файловую систему.
        /// </summary>
        /// <param name="notes">Список с заметками.</param>
        /// <param name="tags">Список тегов.</param>
        public static void WriteFiles(List<Note> notes, List<string> tags)
        {
            string json = Parser.WriteJSON(notes);
            File.WriteAllText(_dataBasePath, json);
            File.WriteAllLines(_tagsFilePath, tags);
        }

        /// <summary>
        /// Активирует файловую систему. Вызывается в начале сеанса.
        /// </summary>
        public static void ActivateFileSystem()
        {
            if (File.Exists(_configFilePath))
            {
                AwakeFileSystem();
            }
            else
            {
                CreateFileSystem();
                AwakeFileSystem();
            }
        }

        /// <summary>
        /// Создает файловую систему.
        /// </summary>
        private static void CreateFileSystem()
        {
            File.WriteAllText($"..{_sl}..{_sl}..{_sl}config.txt", _dataBasePath + "\n" + _tagsFilePath);
        }

        /// <summary>
        /// Получает информацию о файлах системы из файла конфигуратора. Дополняет систему при необходимости.
        /// </summary>
        private static void AwakeFileSystem()
        {
            string[] paths = File.ReadAllLines(_configFilePath);
            (_dataBasePath, _tagsFilePath) = (paths[0], paths[1]);

            if (!File.Exists(_dataBasePath))
            {
                File.WriteAllText(_dataBasePath, "{ \"notes\" " + ": [] }");
            }
            if (!File.Exists(_tagsFilePath))
            {
                File.WriteAllText(_tagsFilePath, "");
            }
        }
    }
}
