namespace EAchievmentsLib
{
    /// <summary>
    /// Описывает объект, которой формируется после считывания JSON файла
    /// </summary>
    public interface IJSONObject
    {
        /// <summary>
        /// Возвращает IEnumerable, содержащий названия всех полей.
        /// </summary>
        /// <returns>IEnumerable, содержащий названия всех полей объекта.</returns>
        IEnumerable<string> GetAllFields();

        /// <summary>
        /// Возвращает значение переданного поля.
        /// </summary>
        /// <param name="fieldName">Поле объекта</param>
        /// <returns>Значение запрашиваемого поля.</returns>
        string? GetField(string fieldName);

        /// <summary>
        /// Устанавливает значение полю.
        /// </summary>
        /// <param name="fieldName">Поле, которому нужно установить значение.</param>
        /// <param name="value">Значение, которое нужно установить полю.</param>
        void SetField(string fieldName, string value);
    }
}
