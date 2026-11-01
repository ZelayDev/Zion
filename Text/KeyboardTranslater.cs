namespace Zion
{
    public static class KeyboardTranslater
    {
        private static readonly HashSet<char>          UniversalChar;
        private static readonly Dictionary<char, char> RusEngLayout;
        private static readonly Dictionary<char, char> EngRusLayout;

        static KeyboardTranslater()
        {
            UniversalChar = new HashSet<char>
            {
                ' ', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '!', '%', '*', '(', ')', '_', '+', '-', '=', '\\'
            };
            EngRusLayout = new Dictionary<char, char>
            {
                { 'q', 'й' }, { 'w', 'ц' }, { 'e', 'у' }, { 'r', 'к' }, { 't', 'е' }, { 'y', 'н' }, { 'u', 'г' },
                { 'i', 'ш' }, { 'o', 'щ' }, { 'p', 'з' },
                { 'a', 'ф' }, { 's', 'ы' }, { 'd', 'в' }, { 'f', 'а' }, { 'g', 'п' }, { 'h', 'р' }, { 'j', 'о' },
                { 'k', 'л' }, { 'l', 'д' },
                { 'z', 'я' }, { 'x', 'ч' }, { 'c', 'с' }, { 'v', 'м' }, { 'b', 'и' }, { 'n', 'т' }, { 'm', 'ь' },

                { '@', '\"' }, { '#', '№' }, { '$', ';' }, { '^', ':' }, { '&', '?' },
                { '`', 'ё' }, { '[', 'х' }, { ']', 'ъ' }, { ';', 'ж' }, { '\'', 'э' }, { ',', 'б' }, { '.', 'ю' },
                { '/', '.' },
                { '~', 'Ё' }, { '{', 'Х' }, { '}', 'Ъ' }, { ':', 'Ж' }, { '\"', 'Э' }, { '<', 'Б' }, { '>', 'Ю' },
                { '?', ',' }
            };
            RusEngLayout = EngRusLayout.Reverse();
        }


        public static string ToEnglish(string Input)
        {
            var InputToChar = Input.ToCharArray();

            for (var i = 0; i < Input.Length; i++)
            {
                InputToChar[i] = ToEnglish(InputToChar[i]);
            }
            return new string(InputToChar);
        }

        public static string ToRussian(string Input)
        {
            var InputToChar = Input.ToCharArray();

            for (var i = 0; i < Input.Length; i++)
            {
                InputToChar[i] = ToRussian(InputToChar[i]);
            }
            return new string(InputToChar);
        }


        public static string Translate(string Input)
        {
            var Words = Input.Split(' ');

            for (var i = 0; i < Words.Length; i++)
            {
                Words[i] = TranslateWord(Words[i]);
            }

            return String.Join(" ", Words);
        }

        public static string[] Translate(string[] Lines)
        {
            for (var i = 0; i < Lines.Length; i++)
            {
                Lines[i] = Translate(Lines[i]);
            }
            return Lines;
        }

        public static string TranslateWord(string Word)
        {
            if (String.IsNullOrWhiteSpace(Word))
            {
                return Word;
            }

            for (var i = 0; i < Word.Length; i++)
            {
                if (Word[i].IsEnglish())
                {
                    return ToRussian(Word);
                }
                if (Word[i].IsRussian())
                {
                    return ToEnglish(Word);
                }
            }
            return Word;
        }


        public static char ToEnglish(char String)
        {
            return String.IsEnglish() ? String : Translate(String, RusEngLayout);
        }

        public static char ToRussian(char String)
        {
            return String.IsRussian() ? String : Translate(String, EngRusLayout);
        }

        public static char Translate(char String)
        {
            return String.IsEnglish() ? ToRussian(String) : ToEnglish(String);
        }


        public static bool IsEnglish(this char Char)
        {
            int CharIndex = Char;
            return (65 <= CharIndex && CharIndex <= 90) || (97 <= CharIndex && CharIndex <= 122);
        }

        public static bool IsRussian(this char Char)
        {
            int CharIndex = Char;
            return 1040 <= CharIndex && CharIndex <= 1103;
        }


        private static char Translate(char Char, Dictionary<char, char> Dictionary)
        {
            var IsUpper = Char.IsUpper(Char);
            var LowerChar = Char.ToLower(Char);

            return !UniversalChar.Contains(Char) && IsLetter(Char) && Dictionary.TryGetValue(Char, out var Value)
                ? IsUpper ? Char.ToUpper(Value) : Value
                : Char;
        }

        private static bool IsLetter(char Input)
        {
            return Input.IsRussian() || Input.IsEnglish();
        }
    }
}