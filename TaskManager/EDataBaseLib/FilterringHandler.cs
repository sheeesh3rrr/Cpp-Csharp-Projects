using ENoteLib;

namespace EDataBaseLib
{
    /// <summary>
    /// Создает Фильтратор.
    /// </summary>
    /// <param name="dataBase">База данных.</param>
    /// <param name="dateRange">Диапазон дат.</param>
    /// <param name="keyWord">Ключевое слово.</param>
    /// <param name="tags">Набор тегов.</param>
    /// <param name="numsOfOperations">Номера применяемых фильтраций</param>
    public class FilterringHandler(ref DataBase dataBase, KeyValuePair<DateOnly, DateOnly> dateRange, string keyWord, string[] tags, int[] numsOfOperations)
    {
        private readonly DataBase _dataBase = dataBase;
        private readonly KeyValuePair<DateOnly, DateOnly> _dateRange = dateRange;
        private readonly string _keyWord = keyWord;
        private readonly string[] _tags = tags;
        private readonly int[] _numsOfOperations = numsOfOperations;

        /// <summary>
        /// Фильтрует базу данных.
        /// </summary>
        public void ProcessFilterring()
        {
            List<Note> list = _dataBase.Notes;
            FilterringOperation[] operations = [FilterByDates, FilterByKeyWord, FilterByTags];
            foreach (int i in _numsOfOperations)
            {
                operations[i](ref list);
            }
            _dataBase.Notes = list;
        }

        /// <summary>
        /// Фильтрует базу данных по диапазону дат.
        /// </summary>
        /// <param name="given">Передаваемый список заметок.</param>
        private void FilterByDates(ref List<Note> given)
        {
            Note[] notes = [.. _dataBase.Notes];
            given = [.. Array.FindAll(notes, el => el.Date > _dateRange.Key && el.Date < _dateRange.Value)];
        }

        /// <summary>
        /// Фильтрует базу данных по ключевому слову.
        /// </summary>
        /// <param name="given">Передаваемый список заметок.</param>
        private void FilterByKeyWord(ref List<Note> given)
        {
            Note[] notes = [.. _dataBase.Notes];
            given = [.. Array.FindAll(notes, el =>
                el.Heading!.ToLower().Contains(_keyWord.ToLower()) ||
                el.Text!.ToLower().Contains(_keyWord.ToLower()))];
        }

        /// <summary>
        /// Фильтрует базу данных по набору тегов.
        /// </summary>
        /// <param name="given">Передаваемый список заметок.</param>
        private void FilterByTags(ref List<Note> given)
        {
            Note[] notes = [.. _dataBase.Notes];
            given = [.. Array.FindAll(notes, el => (el.Tags.Intersect(_tags).Count() != 0) || (el.Tags.Count == 0 && _tags.Length == 0))];
        }
    }
}
