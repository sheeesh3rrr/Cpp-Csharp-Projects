using ENoteLib;
using EFileLib;

namespace EDataBaseLib
{
    /// <summary>
    /// База данных, содержащая заметки во время сеанса программы.
    /// </summary>
    public class DataBase
    {
        /// <summary>
        /// Список незаархивированных заметок.
        /// </summary>
        public List<Note> Notes { get; set; }

        /// <summary>
        /// Список заархивированных заметок.
        /// </summary>
        public List<Note> ArchivedNotes { get; set; }

        /// <summary>
        /// Список всех существующих тегов.
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// Создает базу данных.
        /// </summary>
        public DataBase()
        {
            Notes = [];
            ArchivedNotes = [];
            Tags = [];

            FileHandler.ActivateFileSystem();
            if (File.Exists(FileHandler.MemoryFilePath))
            {
                LoadData();
            }
        }

        /// <summary>
        /// Загружает информацию о заметках из файловой системы.
        /// </summary>
        private void LoadData()
        {
            List<Note> notes;
            List<string> tags;
            (notes, tags) = FileHandler.ReadFiles();

            for (int i = 0; i < notes.Count; i++)
            {
                if (notes[i].Archived)
                {
                    ArchivedNotes.Add(notes[i]);
                }
                else
                {
                    Notes.Add(notes[i]);
                }
            }
            Tags = tags;
        }

        /// <summary>
        /// Передает в файловую систему информацию о заметках для записи.
        /// </summary>
        public void SaveData()
        {
            List<Note> notes = Notes.Union(ArchivedNotes).ToList();
            FileHandler.WriteFiles(notes, Tags);
        }

        /// <summary>
        /// Добавляет новый Note в базу.
        /// </summary>
        /// <param name="heading">Заголовок заметки.</param>
        /// <param name="text">текст заметки.</param>
        /// <param name="tags">Набор тегов заметки.</param>
        public void AddNote(string heading, string text, List<string> tags)
        {
            DateTime now = DateTime.Now;
            DateOnly date = new(now.Year, now.Month, now.Day);
            Notes.Add(new((uint)(Notes.Count + ArchivedNotes.Count + 1), heading, text, date, tags));
        }

        /// <summary>
        /// Удаляет из списка неаархивированных заметок те заметки, которые стоят на определенных индексах.
        /// </summary>
        /// <param name="idxs">Индексы, которые указывают на то, какие заметки необходимо удалить.</param>
        public void DeleteNotes(int[] idxs)
        {
            List<Note> newList = [];
            for (int i = 0; i < Notes.Count; i++)
            {
                if (!idxs.Contains(i))
                {
                    newList.Add(Notes[i]);
                }
            }
            Notes = newList;
        }

        /// <summary>
        /// Перемещает заметки между списками заархивированных и незаархивированных замметок.
        /// </summary>
        /// <param name="idxs">Индексы заметок, которые должны быть перемещены.</param>
        /// <param name="toArchive">Направление перемещения.</param>
        public void TransferBetweenLists(int[] idxs, bool toArchive)
        {
            List<Note> newList = [];
            if (toArchive)
            {
                for (int i = 0; i < Notes.Count; i++)
                {
                    if (!idxs.Contains(i))
                    {
                        newList.Add(Notes[i]);
                    }
                    else
                    {
                        ArchivedNotes.Add(Notes[i]);
                    }
                }
                Notes = newList;
            }
            else
            {
                for (int i = 0; i < ArchivedNotes.Count; i++)
                {
                    if (!idxs.Contains(i))
                    {
                        newList.Add(Notes[i]);
                    }
                    else
                    {
                        Notes.Add(ArchivedNotes[i]);
                    }
                }
                ArchivedNotes = newList;
            }
        }

        /// <summary>
        /// Формирует массив целых значений - количества заметок, имеющих определенный тег.
        /// </summary>
        /// <returns>Массив количества заметок для каждого тега.</returns>
        public int[] GetTagsStats()
        {
            int[] ans = new int[Tags.Count];
            for (int i = 0; i < ans.Length; i++)
            {
                // Собираем теги как с обычных, так и с заархивированных заметок.
                ans[i] = Notes.Count(el => el.Tags.Contains(Tags[i])) + ArchivedNotes.Count(el => el.Tags.Contains(Tags[i]));
            }

            return ans;
        }

        /// <summary>
        /// Меняет расположение файлов, хранящих информацию о заметках и тегах.
        /// </summary>
        /// <param name="path">Путь к нововому файлу</param>
        /// <param name="notesFile">Установить true, если нужно поменять файл с заметками, false - иначе.</param>
        public void ChangeFiles(string path, bool notesFile = true)
        {
            if (notesFile)
            {
                FileHandler.MemoryFilePath = path + ".json";
            }
            else
            {
                FileHandler.TagsFilePath = path + ".txt";
            }
        }
    }
}
