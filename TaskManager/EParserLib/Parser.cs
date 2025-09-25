using ENoteLib;
using System.Text.Json;

namespace EParserLib
{
    public class Parser
    {
        /// <summary>
        /// Преобразовывает JSON-файл в список объектов.
        /// </summary>
        /// <param name="input">JSON-файл.</param>
        /// <returns>Список объектов.</returns>
        public static List<Note> ReadJson(string input)
        {
            StateMachine sm = new(input);
            List<string> readed = sm.ProcessStateMachine();
            return ConvertIntoObjects(readed);
        }

        /// <summary>
        /// Формирует JSON-файл из списка объектов.
        /// </summary>
        /// <param name="groups">Группы достижений.</param>
        /// <param name="achievments">Достижения.</param>
        /// <returns>Строку, представляющую из себя JSON-файл.</returns>
        public static string WriteJSON(List<Note> notes)
        {
            string ans = "{ \"notes\" : [\n\n";
            for (int i = 0; i < notes.Count; i++)
            {
                ans += JsonSerializer.Serialize(notes[i]) + ",\n";
            }
            ans = ans[..^2];
            ans += "\n]}";
            return ans;
        }

        /// <summary>
        /// Преобразует данные из JSON в объекты.
        /// </summary>
        /// <param name="objs">Список JSON-словарей.</param>
        /// <returns>Список объектов.</returns>
        private static List<Note> ConvertIntoObjects(List<string> objs)
        {
            List<Note> ans = [];
            foreach (string obj in objs)
            {
                Note? note = JsonSerializer.Deserialize<Note>(obj);
                if (note != null)
                {
                    ans.Add(note);
                }
            }
            return ans;
        }
    }
}
