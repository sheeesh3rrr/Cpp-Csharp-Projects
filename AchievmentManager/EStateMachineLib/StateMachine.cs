using EAchievmentsLib;

namespace EStateMachineLib
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
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FormatException"></exception>
        public List<Dictionary<string, string>> ProcessStateMachine()
        {
            if (_input is null or "")
            {
                throw new ArgumentException();
            }
            _input += "      ";
            char[] passers = [' ', '\n', '\t', '\r']; // Символы, которые стоит пропускать и не учитывать.

            List<Dictionary<string, string>> ans = [];
            string strCurr = "";
            string keyCurr = "";
            string valCurr;
            Dictionary<string, string> dCurr = [];

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
                        _state = State.Passing; break;

                    case State.Passing when passers.Contains(s):
                        _state = State.Passing; break;
                    case State.Passing when s == '{':
                        _state = State.WaitingForKey; break;

                    case State.WaitingForKey when passers.Contains(s):
                        _state = State.WaitingForKey; break;
                    case State.WaitingForKey when s == '"':
                        _state = State.GettingKey; break;

                    case State.GettingKey when s == '"':
                        _state = State.WaitingForPoints;
                        keyCurr = strCurr;
                        strCurr = "";
                        break;
                    case State.GettingKey:
                        strCurr += s;
                        break;

                    case State.WaitingForPoints when passers.Contains(s):
                        _state = State.WaitingForPoints; break;
                    case State.WaitingForPoints when s == ':':
                        _state = State.WaitingForValue; break;

                    case State.WaitingForValue when passers.Contains(s):
                        _state = State.WaitingForValue; break;
                    case State.WaitingForValue when s == '"':
                        _state = State.GettingValue; break;

                    case State.WaitingForValue when s == 't':
                        if (_input[i + 1] == 'r' && _input[i + 2] == 'u' && _input[i + 3] == 'e')
                        {
                            _state = State.WaitingForComma;
                            valCurr = "true";
                            strCurr = "";
                            dCurr.Add(keyCurr, valCurr);
                            keyCurr = "";
                            i += 3;
                            break;
                        }
                        else
                        {
                            throw new FormatException();
                        }
                    case State.WaitingForValue when s == 'f':
                        if (_input[i + 1] == 'a' && _input[i + 2] == 'l' && _input[i + 3] == 's' && _input[i + 4] == 'e')
                        {
                            _state = State.WaitingForComma;
                            valCurr = "false";
                            strCurr = "";
                            dCurr.Add(keyCurr, valCurr);
                            keyCurr = "";
                            i += 4;
                            break;
                        }
                        else
                        {
                            throw new FormatException();
                        }

                    case State.GettingValue when s == '"':
                        _state = State.WaitingForComma;
                        valCurr = strCurr;
                        strCurr = "";
                        dCurr.Add(keyCurr, valCurr);
                        keyCurr = "";
                        break;
                    case State.GettingValue:
                        strCurr += s;
                        break;

                    case State.WaitingForComma when passers.Contains(s):
                        _state = State.WaitingForComma; break;
                    case State.WaitingForComma when s == ',':
                        _state = State.WaitingForKey; break;
                    case State.WaitingForComma when s == '}':
                        ans.Add(dCurr);
                        dCurr = [];
                        _state = State.WaitingForInterSectionComma; break;

                    case State.WaitingForInterSectionComma when passers.Contains(s):
                        _state = State.WaitingForInterSectionComma; break;
                    case State.WaitingForInterSectionComma when s == ',':
                        _state = State.Passing; break;
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
