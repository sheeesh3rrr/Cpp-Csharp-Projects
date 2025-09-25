using ENoteLib;

namespace EDataBaseLib
{
    /// <summary>
    /// Шаблон алгоритмов фильтрации.
    /// </summary>
    /// <param name="given">Передаваемый список заметок.</param>
    public delegate void FilterringOperation(ref List<Note> given);
}
