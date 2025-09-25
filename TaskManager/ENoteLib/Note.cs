namespace ENoteLib
{
    /// <summary>
    /// Заметка, содержащая id, заголовок, текст, дату создания и набор тегов.
    /// </summary>
    public class Note
    {
        private string? _heading;
        private string? _text;

        /// <summary>
        /// ID заметки.
        /// </summary>
        public uint ID { get; set; }

        /// <summary>
        /// Заголовок заметки.
        /// </summary>
        public string? Heading
        {
            get => _heading;
            set => _heading = value is null or "" ? "<<Не введенный заголовок>>" : value;
        }

        /// <summary>
        /// Текст заметки.
        /// </summary>
        public string? Text
        {
            get => _text;
            set => _text = value is null or "" ? "<<Не введенный текст>>" : value;
        }
        
        /// <summary>
        /// Дата создания заметки.
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Набор тегов заметки.
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// true, если заметка заархивирована, false - иначе.
        /// </summary>
        public bool Archived { get; set; }

        /// <summary>
        /// Создает Note.
        /// </summary>
        public Note()
        {
            ID = 0;
            Heading = "";
            Text = "";
            Date = new();
            Tags = [];
            Archived = false;
        }

        /// <summary>
        /// Создает Note.
        /// </summary>
        public Note(uint id, string heading, string text, DateOnly date, List<string> tags)
        {
            ID = id;
            Heading = heading;
            Text = text;
            Date = date;
            Tags = tags;
            Archived = false;
        }

        /// <summary>
        /// Формирует строку, содержащую полную информацию о заметке.
        /// </summary>
        /// <returns>Строка с информацией о заметке.</returns>
        public string FullInfo()
        {
            return $"{ID} {Heading} {Date}\n" +
                $"{Text}\n" +
                $"Теги:\n" +
                $"{GetTagsString()}";
        }

        /// <summary>
        /// Формирует строку, содержащую краткую информацию о заметке.
        /// </summary>
        /// <returns>Строка с информацией о заметке.</returns>
        public override string ToString()
        {
            return $"{ID} {Heading} {Date}";
        }

        /// <summary>
        /// Формирует строку, состоящую из названий тегов, присвоенных данной заметке.
        /// </summary>
        /// <returns>Строка с названиями тегов.</returns>
        private string GetTagsString()
        {
            string ans = "";

            foreach (string tag in Tags)
            {
                ans += tag + "\n";
            }

            return ans;
        }
    }
}
