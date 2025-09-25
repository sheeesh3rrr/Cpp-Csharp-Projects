namespace EAchievmentsLib
{
    public class AchievmentGroup : IJSONObject, IAchievmentObject
    {
        private string? _id;
        private string? _label;
        private string? _iconUnlocked;
        private List<Achievment> _achievements;

        /// <summary>
        /// Уникальный ID.
        /// </summary>
        public string? ID
        {
            get => _id;
            set => _id = value ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Ярлык.
        /// </summary>
        public string? Label
        {
            get => _label;
            set => _label = value ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Иконка.
        /// </summary>
        public string? IconUnlocked
        {
            get => _iconUnlocked;
            set => _iconUnlocked = value ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Список достижений, относящихся к категории с названием ID.
        /// </summary>
        public List<Achievment> Achievments
        {
            get => _achievements;
            set => _achievements = value ?? [];
        }

        /// <summary>
        /// Тип объекта.
        /// </summary>
        public AchievmentObjectType Type => AchievmentObjectType.Group;

        /// <summary>
        /// Создает AchievmentGroup.
        /// </summary>
        public AchievmentGroup()
        {
            _id = null;
            _label = null;
            _iconUnlocked = null;
            _achievements = [];
        }

        /// <summary>
        /// Создает AchievmentGroup.
        /// </summary>
        /// <param name="id">Уникальный ID.</param>
        /// <param name="label">Ярлык.</param>
        /// <param name="iconUnlocked">Иконка.</param>
        public AchievmentGroup(string id, string label, string iconUnlocked)
        {
            ID = id;
            Label = label;
            IconUnlocked = iconUnlocked;
            _achievements = [];
        }

        /// <summary>
        /// Возвращает IEnumerable, содержащий названия всех полей.
        /// </summary>
        /// <returns>IEnumerable, содержащий названия всех полей класса.</returns>
        public IEnumerable<string> GetAllFields()
        {
            return ["id", "label", "iconUnlocked"];
        }

        /// <summary>
        /// Возвращает значение переданного поля.
        /// </summary>
        /// <param name="fieldName">Поле объекта</param>
        /// <returns>Значение запрашиваемого поля.</returns>
        public string? GetField(string fieldName)
        {
            return fieldName switch
            {
                "id" => ID,
                "label" => Label,
                "iconUnlocked" => IconUnlocked,
                _ => null,
            };
        }

        /// <summary>
        /// Устанавливает значение полю.
        /// </summary>
        /// <param name="fieldName">Поле, которому нужно установить значение.</param>
        /// <param name="value">Значение, которое нужно установить полю.</param>
        /// <exception cref="KeyNotFoundException"></exception>
        public void SetField(string fieldName, string value)
        {
            switch(fieldName)
            {
                case "id":
                    ID = value;
                    break;
                case "label":
                    Label = value;
                    break;
                case "iconUnlocked":
                    IconUnlocked = value;
                    break;
                default: throw new KeyNotFoundException($"{fieldName} Не является полем класса AchievmentGroup");
            }
        }

        /// <summary>
        /// Выводит всю информацию об объекте в читабельном виде.
        /// </summary>
        /// <returns>Строка с информацией об объекте.</returns>
        public override string ToString()
        {
            return $"id: {ID}\n" +
                $"Значение иконки: {IconUnlocked}\n" +
                $"Ярлык: {Label}";
        }
    }
}
