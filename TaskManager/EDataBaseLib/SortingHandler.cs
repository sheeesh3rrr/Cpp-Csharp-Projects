using ENoteLib;

namespace EDataBaseLib
{
    /// <summary>
    /// Создает Сортировщик базы данных.
    /// </summary>
    /// <param name="dataBase">База данных, которую нужно отсортировать.</param>
    /// <param name="field">Параметр сортировки.</param>
    public class SortingHandler(DataBase dataBase, Field field)
    {
        private readonly DataBase _dataBase = dataBase;
        private readonly Field _field = field;

        /// <summary>
        /// Сортирует базу данных.
        /// </summary>
        /// <returns>Список Note в отсортированном по заданному параметру порядке</returns>
        public List<Note> SortDataBase()
        {
            switch (_field)
            {
                case Field.ID:
                    _dataBase.Notes.Sort(CompareById);
                    return _dataBase.Notes;
                case Field.Heading:
                    _dataBase.Notes.Sort(CompareByHeading);
                    return _dataBase.Notes;
                case Field.Text:
                    _dataBase.Notes.Sort(CompareByText);
                    return _dataBase.Notes;
                case Field.Date:
                    _dataBase.Notes.Sort(CompareByDate);
                    return _dataBase.Notes;
                case Field.Tags:
                    _dataBase.Notes.Sort(CompareByTags);
                    return _dataBase.Notes;
                default:
                    return _dataBase.Notes;
            }
        }

        /// <summary>
        /// Сравнивает заметки по ID.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>-1, если сравниваемое значение X меньше чем у Y. 0, если они равны, 1, если больше.</returns>
        private int CompareById(Note x, Note y)
        {
            return x.ID.CompareTo(y.ID);
        }

        /// <summary>
        /// Сравнивает заметки по загаловкам.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>-1, если сравниваемое значение X меньше чем у Y. 0, если они равны, 1, если больше.</returns>
        private int CompareByHeading(Note x, Note y)
        {
            return x.Heading!.CompareTo(y.Heading);
        }

        /// <summary>
        /// Сравнивает заметки по тексту заметки.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>-1, если сравниваемое значение X меньше чем у Y. 0, если они равны, 1, если больше.</returns>
        private int CompareByText(Note x, Note y)
        {
            return x.Text!.CompareTo(y.Text);
        }

        /// <summary>
        /// Сравнивает заметки по количеству тегов.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>-1, если сравниваемое значение X меньше чем у Y. 0, если они равны, 1, если больше.</returns>
        private int CompareByTags(Note x, Note y)
        {
            return x.Tags.Count.CompareTo(y.Tags.Count);
        }

        /// <summary>
        /// Сравнивает заметки по дате.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>-1, если сравниваемое значение X меньше чем у Y. 0, если они равны, 1, если больше.</returns>
        private int CompareByDate(Note x, Note y)
        {
            return x.Date!.CompareTo(y.Date);
        }
    }
}
