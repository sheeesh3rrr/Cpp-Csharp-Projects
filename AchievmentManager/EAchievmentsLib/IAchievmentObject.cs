namespace EAchievmentsLib
{
    /// <summary>
    /// Описывает объект, который связан с достижениями.
    /// </summary>
    public interface IAchievmentObject
    {
        /// <summary>
        /// Тип объекта.
        /// </summary>
        AchievmentObjectType Type { get; }

        /// <summary>
        /// ID объекта.
        /// </summary>
        string? ID { get; }
    }
}
