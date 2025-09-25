using EStateMachineLib;
using EAchievmentsLib;

namespace EDataBaseLib
{
    /// <summary>
    /// Работает с JSON-файлами.
    /// </summary>
    public static class JsonParser
    {
        /// <summary>
        /// Преобразовывает JSON-файл в список объектов.
        /// </summary>
        /// <param name="input">JSON-файл.</param>
        /// <returns>Список объектов.</returns>
        public static List<IAchievmentObject> ReadJson(string input)
        {
            StateMachine sm = new(input);
            List<Dictionary<string, string>> readed = sm.ProcessStateMachine();
            return ConvertIntoObjects(readed);
        }

        /// <summary>
        /// Формирует JSON-файл из списков объектов.
        /// </summary>
        /// <param name="groups">Группы достижений.</param>
        /// <param name="achievments">Достижения.</param>
        /// <returns>Строку, представляющую из себя JSON-файл.</returns>
        public static string WriteJSON(List<AchievmentGroup> groups, List<Achievment> achievments)
        {
            string ans = "{ \"achievments\" : [\n\n";
            for (int i = 0; i < groups.Count; i++)
            {
                ans += ConvertIntoString(groups[i]);
            }
            ans += "\n";
            for (int i = 0; i < achievments.Count; i++)
            {
                ans += ConvertIntoString(achievments[i]);
            }
            ans = ans[..^3];
            ans += "\n]}";
            return ans;
        }

        /// <summary>
        /// Преобразует данные из JSON в объекты.
        /// </summary>
        /// <param name="objs">Список JSON-словарей.</param>
        /// <returns>Список объектов.</returns>
        private static List<IAchievmentObject> ConvertIntoObjects(List<Dictionary<string, string>> objs)
        {
            List<IAchievmentObject> ans = [];
            foreach (Dictionary<string, string> obj in objs)
            {
                if(obj.ContainsKey("isCategory") && obj["isCategory"] == "true")
                {
                    AchievmentGroup group = new();
                    foreach (string field in group.GetAllFields())
                    {
                        if (obj.ContainsKey(field))
                        {
                            group.SetField(field, obj[field]);
                        }
                    }
                    ans.Add(group);
                }
                else
                {
                    Achievment ach = new();
                    foreach (string field in ach.GetAllFields())
                    {
                        if (obj.ContainsKey(field))
                        {
                            ach.SetField(field, obj[field]);
                        }
                    }
                    ans.Add(ach);
                }
            }
            return ans;
        }

        /// <summary>
        /// Формирует строку, представляющую из себя JSON-словарь на основе переданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <returns>Строка, представляющая из себя JSON-словарь.</returns>
        private static string ConvertIntoString(IAchievmentObject obj)
        {
            string ans = "{ ";
            if (obj.Type == AchievmentObjectType.Group)
            {
                ans += $"\"isCategory\": true, ";
                AchievmentGroup curr = (obj as AchievmentGroup)!;
                foreach (string field in curr.GetAllFields())
                {
                    ans += $"\"{field}\" : \"{curr.GetField(field)}\", ";
                }
                ans = ans[..^2] + " }, \n";
            }
            else
            {
                Achievment curr = (Achievment)obj;
                foreach (string field in curr.GetAllFields())
                {
                    if (field is "singleDescription" or "validateOnStorefront")
                    {
                        ans += $"\"{field}\" : {curr.GetField(field)}, ";
                    }
                    else
                    {
                        ans += $"\"{field}\" : \"{curr.GetField(field)}\", ";
                    }
                }
                ans = ans[..^2] + " }, \n";
            }

            return ans;
        }
    }
}
