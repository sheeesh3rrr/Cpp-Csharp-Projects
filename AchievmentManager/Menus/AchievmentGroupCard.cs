using System.Xml.Linq;

namespace EMenuLib
{
    /// <summary>
    /// Работает с одной группой достижений.
    /// </summary>
    public class AchievmentGroupCard
    {
        /// <summary>
        /// Название группы.
        /// </summary>
        public string CategoryName { get; }

        /// <summary>
        /// Все достижения, относящиеся к этой группе.
        /// </summary>
        public string[] Achievments { get; }

        /// <summary>
        /// true, если карточка развернута, false - иначе.
        /// </summary>
        public bool IsOpened { get; set; }

        /// <summary>
        /// true, если на карточке сфокусированы, false - иначе.
        /// </summary>
        public bool IsHovered { get; set; }

        /// <summary>
        /// Создает карточку.
        /// </summary>
        public AchievmentGroupCard()
        {
            CategoryName = string.Empty;
            Achievments = [];
            IsOpened = false;
            IsOpened = false;
        }

        /// <summary>
        /// Создает карточку.
        /// </summary>
        /// <param name="categoryName">Название группы.</param>
        /// <param name="achievments">Все дсотижения, относящиеся к группе.</param>
        public AchievmentGroupCard(string categoryName, string[] achievments)
        {
            CategoryName = categoryName;
            Achievments = achievments.Length > 7 ? achievments[..7] : achievments;
            IsOpened = false;
            IsHovered = false;
        }

        /// <summary>
        /// Отрисовывает карточку в консоли на основе ее состояния.
        /// </summary>
        public void Draw()
        {
            if (IsHovered)
            {
                if (IsOpened)
                {
                    DrawOpenedHovered();
                }
                else
                {
                    DrawClosedHovered();
                }
            }
            else
            {
                if (IsOpened)
                {
                    DrawOpenedDefault();
                }
                else
                {
                    DrawClosedDefault();
                }
            }
        }

        /// <summary>
        /// Открывает / закрывает карточку.
        /// </summary>
        public void Click()
        {
            IsOpened = !IsOpened;
        }

        /// <summary>
        /// Отрисовывает карточку, когда она закрыта и не под фокусом.
        /// </summary>
        public void DrawClosedDefault()
        {
            Console.WriteLine("┌────────────────────────────────────────────────────────────────────┐");
            DrawVariableLine(CategoryName);
            Console.WriteLine("└────────────────────────────────────────────────────────────────────┘");
        }

        /// <summary>
        /// Отрисовывает карточку, когда она открыта и не под фокусом.
        /// </summary>
        public void DrawOpenedDefault()
        {
            Console.WriteLine("┌────────────────────────────────────────────────────────────────────┐");
            DrawVariableLine(CategoryName);
            Console.WriteLine("│────────────────────────────────────────────────────────────────────│");

            for (int i = 0; i < Achievments.Length; i++)
            {
                DrawVariableLine(Achievments[i], true);
            }

            Console.WriteLine("└────────────────────────────────────────────────────────────────────┘");
        }

        /// <summary>
        /// Отрисовывает карточку, когда она закрыта и под фокусом.
        /// </summary>
        public void DrawClosedHovered()
        {
            Console.WriteLine("┌────────────────────────────────────────────────────────────────────┐");
            DrawVariableLine(CategoryName, false, true);
            Console.WriteLine("└────────────────────────────────────────────────────────────────────┘▒");
            Console.WriteLine("  ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒");
        }

        /// <summary>
        /// Отрисовывает карточку, когда она открыта и под фокусом.
        /// </summary>
        public void DrawOpenedHovered()
        {
            Console.WriteLine("┌────────────────────────────────────────────────────────────────────┐");
            DrawVariableLine(CategoryName, false, true);
            Console.WriteLine("│────────────────────────────────────────────────────────────────────│▒");

            for (int i = 0; i < Achievments.Length; i++)
            {
                DrawVariableLine(Achievments[i], true, true);
            }

            Console.WriteLine("└────────────────────────────────────────────────────────────────────┘▒");
            Console.WriteLine("  ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒");
        }

        /// <summary>
        /// Отрисовывает строчку карточки, в которой есть название группы или достижения.
        /// </summary>
        /// <param name="var">название группы или достижения.</param>
        /// <param name="IsAchievment">Установить true, если идет работа с достижением.</param>
        /// <param name="IsShadowed">Установить true, если карточка под фокусом.</param>
        private void DrawVariableLine(string var, bool IsAchievment = false, bool IsShadowed = false)
        {
            string val = var.Length >= 65 ? var[..62] + "..." : var;
            if (IsAchievment)
            {
                Console.Write($"│ - {val}");
                for (int i = 0; i < 65 - val.Length; i++)
                {
                    Console.Write(' ');
                }
                Console.Write("│");
                if (IsShadowed)
                {
                    Console.WriteLine("▒");
                }
                else
                {
                    Console.WriteLine();
                }
            }
            else
            {
                Console.Write($"│ {val}");
                for (int i = 0; i < 67 - val.Length; i++)
                {
                    Console.Write(' ');
                }
                Console.Write("│");
                if (IsShadowed)
                {
                    Console.WriteLine("▒");
                }
                else
                {
                    Console.WriteLine();
                }
            }
        }
    }
}
