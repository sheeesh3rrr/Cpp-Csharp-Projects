namespace EAchievmentsLib
{
    /// <summary>
    /// Содержит информацию об одном достижении и предоставляет методы для работы с этими данными.
    /// </summary>
    public struct Achievment : IJSONObject, IAchievmentObject
    {
        private string? _id;
        private string? _category;
        private string? _iconUnlocked;
        private string? _label;
        private string? _descriptionUnlocked;

        /// <summary>
        /// Уникальный ID.
        /// </summary>
        public string? ID
        {
            readonly get => _id;
            set => _id = value ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Категория.
        /// </summary>
        public string? Category
        {
            readonly get => _category;
            set => _category = value ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Иконка.
        /// </summary>
        public string? IconUnlocked
        {
            readonly get => _iconUnlocked;
            set => _iconUnlocked = value ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Ярлык.
        /// </summary>
        public string? Label
        {
            readonly get => _label;
            set => _label = value ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Описание достижения.
        /// </summary>
        public string? DescriptionUnlocked
        {
            readonly get => _descriptionUnlocked;
            set => _descriptionUnlocked = value ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Индивидуальное описание.
        /// </summary>
        public bool SingleDescription { readonly get; set; }

        /// <summary>
        /// Разрешение.
        /// </summary>
        public bool ValidateOnStoreFront { readonly get; set; }

        /// <summary>
        /// Тип объекта.
        /// </summary>
        public readonly AchievmentObjectType Type => AchievmentObjectType.Single;

        /// <summary>
        /// Создает Achievment.
        /// </summary>
        public Achievment()
        {
            _id = null;
            _category = null;
            _iconUnlocked = null;
            _label = null;
            _descriptionUnlocked = null;
            SingleDescription = false;
            ValidateOnStoreFront = false;
        }

        /// <summary>
        /// Создает Achievment
        /// </summary>
        /// <param name="id">Уникальный ID.</param>
        /// <param name="category">Категория.</param>
        /// <param name="iconUnlocked">Иконка.</param>
        /// <param name="label">Ярлык.</param>
        /// <param name="descriptionUnlocked">Описание.</param>
        /// <param name="singleDescription">Индивидуальное описание.</param>
        /// <param name="validateOnStoreFront">Разрешение.</param>
        public Achievment(
            string id, string category, string iconUnlocked, string label, 
            string descriptionUnlocked, bool singleDescription, bool validateOnStoreFront
            )
        {
            ID = id;
            Category = category;
            IconUnlocked = iconUnlocked;
            Label = label;
            DescriptionUnlocked = descriptionUnlocked;
            SingleDescription = singleDescription;
            ValidateOnStoreFront = validateOnStoreFront;
        }

        /// <summary>
        /// Возвращает IEnumerable, содержащий названия всех полей.
        /// </summary>
        /// <returns>IEnumerable, содержащий названия всех полей структуры.</returns>
        public readonly IEnumerable<string> GetAllFields()
        {
            return ["id", "category", "iconUnlocked", "singleDescription", "validateOnStorefront", "label", "descriptionunlocked"];
        }

        /// <summary>
        /// Возвращает значение переданного поля.
        /// </summary>
        /// <param name="fieldName">Поле объекта</param>
        /// <returns>Значение запрашиваемого поля.</returns>
        public readonly string? GetField(string fieldName)
        {
            return fieldName switch
            {
                "id" => ID,
                "category" => Category,
                "iconUnlocked" => IconUnlocked,
                "label" => Label,
                "descriptionunlocked" => DescriptionUnlocked,
                "singleDescription" => SingleDescription ? "true" : "false",
                "validateOnStorefront" => ValidateOnStoreFront ? "true" : "false",
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
            switch (fieldName)
            {
                case "id": ID = value; break;
                case "category": Category = value; break;
                case "iconUnlocked": IconUnlocked = value; break;
                case "label": Label = value; break;
                case "descriptionunlocked": DescriptionUnlocked = value; break;
                case "singleDescription": SingleDescription = value == "true"; break;
                case "validateOnStorefront": ValidateOnStoreFront = value == "true"; break;
                default: throw new KeyNotFoundException($"{fieldName} Не является полем структуры Achievment");
            }
        }

        /// <summary>
        /// Выводит всю информацию об объекте в читабельном виде.
        /// </summary>
        /// <returns>Строка с информацией об объекте.</returns>
        public override readonly string ToString()
        {
            return 
                $"id: {ID}\n" +
                $"Категория: {Category}\n" +
                $"Значение иконки: {IconUnlocked}\n" +
                $"Ярлык: {Label}\n" +
                $"Описание: {DescriptionUnlocked}\n" +
                $"Одиночное описание : {SingleDescription}\n" +
                $"Разрешение : {ValidateOnStoreFront}";
        }
    }
}
