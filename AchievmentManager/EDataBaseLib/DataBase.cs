using EAchievmentsLib;
using System.Collections;

namespace EDataBaseLib
{
    /// <summary>
    /// Содержит информацию о полученных достижениях и группах достижений.
    /// </summary>
    public class DataBase
    {
        private List<AchievmentGroup> _groups;
        private List<Achievment> _achievments;

        /// <summary>
        /// Группы достижений.
        /// </summary>
        public List<AchievmentGroup> Groups
        {
            get => _groups;
            set => _groups = value ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Достижения.
        /// </summary>
        public List<Achievment> Achievments
        {
            get => _achievments;
            set => _achievments = value ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Возвращает true, если в базе данных есть элементы, false - иначе.
        /// </summary>
        public bool IsEmpty => _groups.Count == 0 && _achievments.Count == 0;

        /// <summary>
        /// Создает DataBase.
        /// </summary>
        public DataBase()
        {
            _groups = [];
            _achievments = [];
        }

        /// <summary>
        /// Очищает базу данных.
        /// </summary>
        public void Clear()
        {
            _groups = [];
            _achievments = [];
        }

        /// <summary>
        /// Формирует базу данных на основе полученных объектов.
        /// </summary>
        /// <param name="parsed">Список объектов, которые прошли парсинг.</param>
        /// <param name="redefine">Доопределяет группы, если true</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Build(List<IAchievmentObject> parsed, bool redefine = true)
        {
            if (parsed == null || parsed.Count == 0)
            {
                throw new ArgumentNullException("Переданы пустые данные");
            }

            FillInTheGaps(parsed);
            if (_groups.Count > 0) 
            {
                FillInTheGroups();
            }
            else if (redefine)
            {
                BuildGroups();
            }
        }

        /// <summary>
        /// Возвращает ID всех групп.
        /// </summary>
        /// <returns>Массив, содержащий ID всех групп.</returns>
        public string[] GetGroupsIDs()
        {
            string[] ans = new string[_groups.Count];
            for (int i =  0; i < _groups.Count; i++)
            {
                ans[i] = _groups[i].ID!;
            }
            return ans;
        }


        /// <summary>
        /// Возвращает ID всех достижений.
        /// </summary>
        /// <returns>Массив, содержащий ID всех достижений.</returns>
        public string[] GetAcievmentsIDs()
        {
            string[] ans = new string[_achievments.Count];
            for (int i = 0; i < _achievments.Count; i++)
            {
                ans[i] = _achievments[i].ID!;
            }
            return ans;
        }

        /// <summary>
        /// Фильтрует список групп или достижений на основе поля и целевого значения поля.
        /// </summary>
        /// <param name="field">Поле, по которому производится фильтрация.</param>
        /// <param name="target">Целевое значение поля.</param>
        /// <param name="type">Тип фильтруемых объектов.</param>
        /// <returns></returns>
        public List<IAchievmentObject> Filter(int field, string target, AchievmentObjectType type)
        {
            List<IAchievmentObject> ans = [];

            if (type == AchievmentObjectType.Group)
            {
                foreach (AchievmentGroup ag in _groups)
                {
                    string param = ag.GetAllFields().ElementAt(field);
                    if (ag.GetField(param) == target)
                    {
                        ans.Add(ag);
                    }
                }
                return ans;
            }
            else
            {
                foreach (Achievment ag in _achievments)
                {
                    string param = ag.GetAllFields().ElementAt(field);
                    if (ag.GetField(param) == target)
                    {
                        ans.Add(ag);
                    }
                }
                return ans;
            }
        }

        /// <summary>
        /// Сортирует список групп или достижений на основе поля и целевого значения поля.
        /// </summary>
        /// <param name="field">Поле, по которому производится сортировка.</param>
        /// <param name="target">Целевое значение поля.</param>
        /// <param name="type">Тип сортируемых объектов.</param>
        /// <returns></returns>
        public List<IAchievmentObject> Sort(int field, AchievmentObjectType type)
        {
            if (type == AchievmentObjectType.Group)
            {
                List<AchievmentGroup> ans = _groups;
                switch (field)
                {
                    case 1:
                        ans.Sort((x, y) => x.ID!.CompareTo(y.ID!));
                        return ConvertIntoIAchevment(ans);
                    case 2:
                        ans.Sort((x, y) => x.Label!.CompareTo(y.Label!));
                        return ConvertIntoIAchevment(ans);
                    case 3:
                        ans.Sort((x, y) => x.IconUnlocked!.CompareTo(y.IconUnlocked!));
                        return ConvertIntoIAchevment(ans);
                    default:
                        throw new ArgumentException();
                }
            }
            else
            {
                List<Achievment> ans = _achievments;
                switch (field)
                {
                    case 1:
                        ans.Sort((x, y) => x.ID!.CompareTo(y.ID!));
                        return ConvertIntoIAchevment(ans);
                    case 2:
                        ans.Sort((x, y) => x.Category!.CompareTo(y.Category!));
                        return ConvertIntoIAchevment(ans);
                    case 3:
                        ans.Sort((x, y) => x.IconUnlocked!.CompareTo(y.IconUnlocked!));
                        return ConvertIntoIAchevment(ans);
                    case 4:
                        ans.Sort((x, y) => x.Label!.CompareTo(y.Label!));
                        return ConvertIntoIAchevment(ans);
                    case 5:
                        ans.Sort((x, y) => x.DescriptionUnlocked!.CompareTo(y.DescriptionUnlocked!));
                        return ConvertIntoIAchevment(ans);
                    default:
                        throw new ArgumentException("Невозможно произвести сортировку");
                }
            }
        }

        /// <summary>
        /// Преобразует IAchievmentObject в AchievmentGroup.
        /// </summary>
        /// <param name="objs">Объект, который требуется преобразовать.</param>
        /// <returns>AchievmentGroup</returns>
        private List<IAchievmentObject> ConvertIntoIAchevment(List<AchievmentGroup> objs)
        {
            List<IAchievmentObject> ans = [];
            for (int i = 0; i < objs.Count; i++)
            {
                ans.Add(objs[i]);
            }
            return ans;
        }

        /// <summary>
        /// Преобразует IAchievmentObject в Achievment.
        /// </summary>
        /// <param name="objs">Объект, который требуется преобразовать.</param>
        /// <returns>Achievment</returns>
        private List<IAchievmentObject> ConvertIntoIAchevment(List<Achievment> objs)
        {
            List<IAchievmentObject> ans = [];
            for (int i = 0; i < objs.Count; i++)
            {
                ans.Add(objs[i]);
            }
            return ans;
        }

        /// <summary>
        /// Доопределяет пустые поля.
        /// </summary>
        /// <param name="parsed">Список объектов, которые прошли парсинг.</param>
        private void FillInTheGaps(List<IAchievmentObject> parsed)
        {
            for (int i = 0; i < parsed.Count; i++)
            {
                if (parsed[i].Type == AchievmentObjectType.Single)
                {
                    Achievment ach = (Achievment)parsed[i];
                    IEnumerable<string> fields = ach.GetAllFields();
                    for (int j = 0; j < fields.Count(); j++)
                    {
                        if (ach.GetField(fields.ElementAt(j)) == null)
                        {
                            ach.SetField(fields.ElementAt(j), "not listed");
                        }
                    }
                    _achievments.Add(ach);
                }
                else
                {
                    AchievmentGroup ag = (AchievmentGroup)parsed[i];
                    IEnumerable<string> fields = ag.GetAllFields();
                    for (int j = 0; j < fields.Count(); j++)
                    {
                        if (ag.GetField(fields.ElementAt(j)) == null)
                        {
                            ag.SetField(fields.ElementAt(j), "not listed");
                        }
                    }
                    _groups.Add(ag);
                }
            }
        }

        /// <summary>
        /// Заполняет группы достижениями.
        /// </summary>
        private void FillInTheGroups()
        {
            for (int i = 0; i < _groups.Count; i++)
            {
                for (int j = 0; j < _achievments.Count; j++)
                {
                    if (_groups[i].ID == _achievments[j].Category)
                    {
                        _groups[i].Achievments.Add(_achievments[j]);
                    }
                }
            }
        }

        /// <summary>
        /// Доопределяет группы на основе списка достижений.
        /// </summary>
        private void BuildGroups()
        {
            for (int i = 0; i < _achievments.Count; i++)
            {
                if (_groups.Exists(el => el.ID == _achievments[i].Category))
                {
                    _groups[_groups.FindIndex(el => el.ID == _achievments[i].Category)]
                        .Achievments.Add(_achievments[i]);
                }
                else
                {
                    AchievmentGroup group = new(_achievments[i].Category!, "self-created", "self-created");
                    group.Achievments.Add(_achievments[i]);
                    _groups.Add(group);
                }
            }
        }
    }
}
