using ENoteLib;

namespace EParserLib
{
    /// <summary>
    /// Создает машину состояний.
    /// </summary>
    /// <param name="input">Строка, с которой работает машина.</param>
    public class StateMachine(string input)
    {
        private State _state = State.Awakening;
        private string _input = input;

        /// <summary>
        /// Запускает машину состояний.
        /// </summary>
        /// <returns>Список словарей, представляющих собой данные для создания объектов.</returns>
        /// <exception cref="FormatException"></exception>
        public List<string> ProcessStateMachine()
        {
            if (_input is null or "")
            {
                throw new FormatException();
            }
            _input += "      ";
            char[] passers = [' ', '\n', '\t', '\r']; // Символы, которые стоит пропускать и не учитывать.

            List<string> ans = [];
            string strCurr = "";

            for (int i = 0; i < _input.Length; i++)
            {
                char s = _input[i];
                switch (_state)
                {
                    case State.Awakening when passers.Contains(s):
                        _state = State.Awakening; break;
                    case State.Awakening when s == '{':
                        _state = State.WaitingForMainKey; break;

                    case State.WaitingForMainKey when passers.Contains(s):
                        _state = State.WaitingForMainKey; break;
                    case State.WaitingForMainKey when s == '"':
                        _state = State.GettingMainKey; break;
                    case State.GettingMainKey when s == '"':
                        _state = State.WaitingForMainPoints; break;
                    case State.GettingMainKey:
                        _state = State.GettingMainKey; break;

                    case State.WaitingForMainPoints when passers.Contains(s):
                        _state = State.WaitingForMainPoints; break;
                    case State.WaitingForMainPoints when s == ':':
                        _state = State.Starting; break;

                    case State.Starting when passers.Contains(s):
                        _state = State.Starting; break;
                    case State.Starting when s == '[':
                        _state = State.WaitingForFirstObject; break;

                    case State.WaitingForFirstObject when passers.Contains(s):
                        _state = State.WaitingForFirstObject; break;
                    case State.WaitingForFirstObject when s == '{':
                        strCurr += s;
                        _state = State.Reading; break;
                    case State.Passing when passers.Contains(s):
                        _state = State.Passing; break;
                    case State.Passing when s == '{':
                        strCurr += s;
                        _state = State.Reading; break;
                    case State.Reading when s != '}':
                        strCurr += s; break;
                    case State.Reading when s == '}':
                        strCurr += s;
                        ans.Add(strCurr);
                        strCurr = "";
                        _state = State.WaitingForInterSectionComma; break;

                    case State.WaitingForInterSectionComma when passers.Contains(s):
                        _state = State.WaitingForInterSectionComma; break;
                    case State.WaitingForInterSectionComma when s == ',':
                        _state = State.Passing; break;
                    case State.WaitingForFirstObject when s == ']':
                        _state = State.Stopping; break;
                    case State.WaitingForInterSectionComma when s == ']':
                        _state = State.Stopping; break;

                    case State.Stopping when passers.Contains(s):
                        _state = State.Stopping; break;
                    case State.Stopping when s == '}':
                        return ans;

                    default:
                        throw new FormatException();
                }
            }

            throw new FormatException();
        }
    }
}
