using System.Text;

namespace Zion
{
    public static class Text
    {
        #region Data
        private static readonly Dictionary<char, char> Brackets = new(3)
        {
            { '(', ')' },
            { '[', ']' },
            { '{', '}' }
        };

        private static readonly HashSet<char> ClosingBrackets = [')', ']', '}'];
        #endregion

        #region Edit
        public static string RemoveChars(this string String, Predicate<char> Condition)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(String);

            var Result = new List<char>(String.Length);

            foreach (var Char in String)
            {
                if (Condition(Char))
                {
                    continue;
                }
                Result.Add(Char);
            }

            return Result.Create();
        }

        public static string RemoveChars(this string String, params IEnumerable<char> RemovingChars)
        {
            return String.RemoveChars(RemovingChars.Contains);
        }

        public static string UnSpace(this string String)
        {
            return String.RemoveChars(Char.IsWhiteSpace);
        }

        public static string Minimize(this string String)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(String);

            var Result = new List<char>(String.Length);

            foreach (var Char in String)
            {
                if (!Char.IsWhiteSpace(Char))
                {
                    Result.Add(Char.ToLower(Char));
                }
            }

            return Result.Create();
        }

        public static string Duplicate(this string String, int Count)
        {
            ArgumentNullException.ThrowIfNull(String);

            if (Count < 0)
            {
                throw new ArgumentException($"Count(={Count}) < 0");
            }
            if (Count == 0)
            {
                return String.Empty;
            }

            var Result = new char[String.Length * Count];
            var Index = 0;

            while (Index < Result.Length)
            {
                for (var i = 0; i < String.Length; i++)
                {
                    Result[Index++] = String[i];
                }
            }
            return new string(Result);
        }

        public static string Capitalize(this string String)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(String);

            var Result = String.ToCharArray();

            Result[0] = Char.ToUpper(Result[0]);
            for (var i = 1; i < Result.Length; i++)
            {
                Result[i] = Char.ToLower(Result[i]);
            }

            return new string(Result);
        }

        public static string[] SplitIntoLines(this string String)
        {
            ArgumentNullException.ThrowIfNull(String);

            return String.Length == 0 ? Array.Empty<string>() : String.Split(Environment.NewLine).TrimEmptyLines();
        }

        public static string[] TrimEmptyLines(this string[] StringArray)
        {
            ArgumentNullException.ThrowIfNull(StringArray);

            if (StringArray.Length == 0)
            {
                return StringArray;
            }

            var Start = 0;
            var End = StringArray.Length - 1;

            while (Start <= End && String.IsNullOrWhiteSpace(StringArray[Start]))
            {
                Start++;
            }

            while (End >= Start && String.IsNullOrWhiteSpace(StringArray[End]))
            {
                End--;
            }

            return Start > End ? Array.Empty<string>() : StringArray[Start..(End + 1)];
        }


        public static string Centering(this string String, int TotalLength, char PaddingChar = ' ')
        {
            ArgumentNullException.ThrowIfNull(String);

            if (TotalLength <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(TotalLength), $"TotalLength(={TotalLength}) <= 0");
            }

            if (String.Length >= TotalLength)
            {
                return String;
            }

            var TotalPadding = TotalLength - String.Length;
            var LeftPadding = TotalPadding / 2;
            var RightPadding = TotalPadding - LeftPadding;

            return new StringBuilder(TotalLength)
                   .Append(PaddingChar, LeftPadding)
                   .Append(String)
                   .Append(PaddingChar, RightPadding)
                   .ToString();
        }

        public static string Truncate(this string String, int TotalLength)
        {
            ArgumentNullException.ThrowIfNull(String);

            return TotalLength <= 3
                ? throw new ArgumentOutOfRangeException(nameof(TotalLength), $"TotalLength(={TotalLength}) <= 0")
                : String.Length <= TotalLength
                    ? String
                    : String[..(TotalLength - 3)] + "...";
        }


        public static string[] Chunk(this string String, int ChunkLength)
        {
            if (String.IsNullOrEmpty(String) || ChunkLength <= 0)
            {
                return Array.Empty<string>();
            }

            var Chunks = new string[(String.Length + ChunkLength - 1) / ChunkLength];

            var End = Chunks.Length - 1;
            var ChunkIndex = 0;
            var Index = 0;

            while (ChunkIndex < End)
            {
                var EndIndex = Index + ChunkLength;
                Chunks[ChunkIndex] = String.Substring(Index, ChunkLength);
                Index = EndIndex;
                ChunkIndex++;
            }

            Chunks[End] = String.Substring(Index);

            return Chunks;
        }

        public static string[] ChunkFromRight(this string String, int ChunkLength)
        {
            if (String.IsNullOrEmpty(String) || ChunkLength <= 0)
            {
                return Array.Empty<string>();
            }

            var Chunks = new string[(String.Length + ChunkLength - 1) / ChunkLength];

            var End = Chunks.Length - 1;
            var ChunkIndex = End;
            var Index = String.Length;

            while (ChunkIndex > 0)
            {
                var StartIndex = Index - ChunkLength;
                Chunks[ChunkIndex] = String.Substring(StartIndex, ChunkLength);
                Index = StartIndex;
                ChunkIndex--;
            }

            Chunks[0] = String.Substring(0, Index);

            return Chunks;
        }


        public static string ConvertAll(this string String, Func<char, char> Converter)
        {
            ArgumentNullException.ThrowIfNull(String);
            ArgumentNullException.ThrowIfNull(Converter);

            return String.Length == 0
                ? String.Empty
                : String.Create
                (
                    String.Length,
                    String,
                    (Span, Original) =>
                    {
                        for (var i = 0; i < Original.Length; i++)
                        {
                            Span[i] = Converter(Original[i]);
                        }
                    }
                );
        }

        public static string RemoveEmptyLines(this string String)
        {
            ArgumentNullException.ThrowIfNull(String);

            var Builder = new StringBuilder(String.Length);

            using (var Reader = new StringReader(String))
            {
                string? Line;

                while ((Line = Reader.ReadLine()) is not null)
                {
                    if (!String.IsNullOrWhiteSpace(Line))
                    {
                        Builder.AppendLine(Line);
                    }
                }
            }

            return Builder.ToString();
        }

        public static string Create(this ICollection<char> Collection)
        {
            ArgumentNullException.ThrowIfNull(Collection);

            return String.Create(Collection.Count, Collection, (Span, List) =>
            {
                var Index = 0;
                foreach (var Char in List)
                {
                    Span[Index++] = Char;
                }
            });
        }
        #endregion

        #region Prefix & Suffix
        public static string RemovePrefix(this string String, string Prefix)
        {
            ArgumentNullException.ThrowIfNull(String);
            ArgumentNullException.ThrowIfNull(Prefix);

            return String.Length == 0 || String.Length < Prefix.Length ? String :
                String.StartsWith(Prefix) ? String[Prefix.Length..] : String;
        }

        public static string RemoveSuffix(this string String, string Suffix)
        {
            ArgumentNullException.ThrowIfNull(String);
            ArgumentNullException.ThrowIfNull(Suffix);

            return String.EndsWith(Suffix) ? String[Suffix.Length..] : String;
        }

        public static string EnsurePrefix(this string String, string Prefix)
        {
            ArgumentNullException.ThrowIfNull(String);
            ArgumentNullException.ThrowIfNull(Prefix);

            return Prefix.Length == 0 ? String : String.StartsWith(Prefix) ? String : Prefix + String;
        }

        public static string EnsureSuffix(this string String, string Suffix)
        {
            ArgumentNullException.ThrowIfNull(String);
            ArgumentNullException.ThrowIfNull(Suffix);

            return Suffix.Length == 0 ? String : String.EndsWith(Suffix) ? String : String + Suffix;
        }

        public static string GetCommonStart(this IEnumerable<string> Strings)
        {
            ArgumentNullException.ThrowIfNull(Strings);

            if (Strings.Any(String => String is null))
            {
                throw new ArgumentNullException("StringArray[`] is null");
            }
            if (Strings.IsNullOrEmpty())
            {
                return String.Empty;
            }

            var End = Strings.Select(String => String.Length).Min();

            if (End == 0)
            {
                return String.Empty;
            }

            var First = Strings.First();

            for (var i = 0; i < End; i++)
            {
                var Char = First[i];

                if (Strings.Any(String => String[i] != Char))
                {
                    return First[..i];
                }
            }

            return First[..End];
        }
        #endregion

        #region Predicate
        public static bool Begins(this string String, int Start, string Target, bool IgnoreCase = false)
        {
            ArgumentNullException.ThrowIfNull(String);
            ArgumentNullException.ThrowIfNull(Target);

            if (Start >= String.Length || String.Length - Start < Target.Length)
            {
                return false;
            }

            if (IgnoreCase)
            {
                for (var i = 0; i < Target.Length; i++)
                {
                    if (Char.ToLower(String[Start + i]) != Char.ToLower(Target[i]))
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (var i = 0; i < Target.Length; i++)
                {
                    if (String[Start + i] != Target[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool Exists(this string String, string Target, out int StartIndex)
        {
            ArgumentNullException.ThrowIfNull(String);
            ArgumentNullException.ThrowIfNull(Target);

            StartIndex = -1;

            if (String.Length == 0 && Target.Length == 0)
            {
                return true;
            }
            if (Target.Length > String.Length)
            {
                return false;
            }

            StartIndex = 0;
            var End = String.Length - Target.Length;

            while (StartIndex <= End)
            {
                var Founded = true;

                for (var j = 0; j < Target.Length; j++)
                {
                    if (String[StartIndex + j] != Target[j])
                    {
                        Founded = false;
                        break;
                    }
                }

                if (Founded)
                {
                    return true;
                }

                StartIndex++;
            }

            StartIndex = -1;
            return false;
        }

        public static bool CheckBrackets(this string String)
        {
            return String.CheckBrackets(Brackets, ClosingBrackets);
        }

        public static bool CheckBrackets(this string String, Dictionary<char, char> Brackets)
        {
            return String.CheckBrackets(Brackets, Brackets.Values.ToHashSet());
        }

        public static bool CheckBrackets(this string String, Dictionary<char, char> Brackets,
            HashSet<char>                            ClosedBrackets)
        {
            if (String.IsNullOrEmpty(String) || Brackets.Count == 0)
            {
                return true;
            }

            var Stack = new Stack<char>(String.Length / 2 + 1);
            var Current = '\0';

            for (var i = 0; i < String.Length; i++)
            {
                Current = String[i];

                if (Brackets.TryGetValue(Current, out var ClosedBracket))
                {
                    Stack.Push(ClosedBracket);
                }
                else if (ClosedBrackets.Contains(Current) && (Stack.Count == 0 || Current != Stack.Pop()))
                {
                    return false;
                }
            }
            return Stack.Count == 0;
        }

        public static bool TrueFor(this string String, int Start, int End, Predicate<char> Condition)
        {
            ArgumentNullException.ThrowIfNull(String);
            ArgumentOutOfRangeException.ThrowIfNegative(Start);

            if (End > String.Length)
            {
                throw new ArgumentOutOfRangeException($"End(={End}) >= String.Length(={String.Length})");
            }

            for (var i = Start; i < End; i++)
            {
                if (!Condition(String[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsOpenBracket(this char Char)
        {
            return Brackets.ContainsKey(Char);
        }

        public static bool IsOpenBracket(this char Char, out char ClosingBracket)
        {
            return Brackets.TryGetValue(Char, out ClosingBracket);
        }

        public static bool Contains(this string String, Predicate<char> Condition)
        {
            foreach (var Char in String.NotNull())
            {
                if (Condition(Char))
                {
                    return true;
                }
            }
            return false;
        }


        public static bool Contains(this string String, HashSet<char> Chars)
        {
            ArgumentNullException.ThrowIfNull(Chars);

            foreach (var Char in String.NotNull())
            {
                if (Chars.Contains(Char))
                {
                    return true;
                }
            }
            return false;
        }

        public static int IndexOf(this string String, Predicate<char> Condition)
        {
            ArgumentNullException.ThrowIfNull(String);
            ArgumentNullException.ThrowIfNull(Condition);

            for (var i = 0; i < String.Length; i++)
            {
                if (Condition(String[i]))
                {
                    return i;
                }
            }
            return -1;
        }
        #endregion

        #region Programming
        public static bool IsIdentifier(this string String)
        {
            ArgumentNullException.ThrowIfNull(String);

            if (String.Length == 0 ||
                Char.IsNumber(String[0])
               )
            {
                return false;
            }

            for (var i = 1; i < String.Length; i++)
            {
                var Char = String[i];

                if (!(Char.IsEnglish() || Char == '_' || Char.IsDigit(Char)))
                {
                    return false;
                }
            }

            return true;
        }

        public static string[] SplitIdentifier(this string String)
        {
            if (String.IsNullOrEmpty(String))
            {
                return Array.Empty<string>();
            }

            var Result = new List<string>();
            var CurrentWord = new StringBuilder();
            bool? LastWasUpper = null;
            var LastWasDigit = false;

            void AddWord()
            {
                if (CurrentWord.Length > 0)
                {
                    Result.Add(CurrentWord.ToString());
                    CurrentWord.Clear();
                }
            }

            for (var i = 0; i < String.Length; i++)
            {
                var Char = String[i];

                if (Char == '_' || Char == ' ' || Char == '-')
                {
                    AddWord();
                    LastWasUpper = null;
                    LastWasDigit = false;
                    continue;
                }

                if (Char.IsDigit(Char))
                {
                    if (!LastWasDigit && CurrentWord.Length > 0)
                    {
                        AddWord();
                    }
                    CurrentWord.Append(Char);
                    LastWasDigit = true;
                    LastWasUpper = false;
                    continue;
                }

                if (Char.IsUpper(Char))
                {
                    if (CurrentWord.Length > 0 && LastWasUpper == false)
                    {
                        AddWord();
                    }
                    else if (CurrentWord.Length > 0 && LastWasUpper == true &&
                             i < String.Length - 1 && Char.IsLower(String[i + 1]))
                    {
                        AddWord();
                    }

                    CurrentWord.Append(Char);
                    LastWasUpper = true;
                    LastWasDigit = false;
                }
                else
                {
                    if (LastWasDigit)
                    {
                        AddWord();
                    }
                    CurrentWord.Append(Char);

                    LastWasUpper = false;
                    LastWasDigit = false;
                }
            }

            AddWord();

            return Result.ToArray();
        }


        public static string ToCamelCase(this IList<string> Strings, bool Underlining = false)
        {
            ArgumentNullException.ThrowIfNull(Strings);

            if (Strings.Count == 0)
            {
                return String.Empty;
            }

            var Builder = new StringBuilder(Strings.Sum(static String => String.Length) + (Underlining ? 1 : 0));

            Builder.Append(Strings[0].ToLower());
            for (var i = 1; i < Strings.Count; i++)
            {
                Builder.Append(Strings[i].Capitalize());
            }

            var Result = Builder.ToString();
            return Underlining ? '_' + Result : Result;
        }

        public static string ToPascalCase(this ICollection<string> Strings, bool Underlining = false)
        {
            ArgumentNullException.ThrowIfNull(Strings);

            if (Strings.Count == 0)
            {
                return String.Empty;
            }

            var Builder = new StringBuilder(Strings.Sum(static String => String.Length) + (Underlining ? 1 : 0));

            foreach (var Word in Strings)
            {
                Builder.Append(Word.Capitalize());
            }

            var Result = Builder.ToString();
            return Underlining ? '_' + Result : Result;
        }

        public static string ToSnakeCase(this ICollection<string> Strings, bool Underlining = false)
        {
            ArgumentNullException.ThrowIfNull(Strings);

            if (Strings.Count == 0)
            {
                return String.Empty;
            }

            var Result = String.Join('_', Strings.Select(String => String.ToLower()));
            return Underlining ? '_' + Result : Result;
        }

        public static string ToKebabCase(this ICollection<string> Strings, bool Underlining = false)
        {
            ArgumentNullException.ThrowIfNull(Strings);

            if (Strings.Count == 0)
            {
                return String.Empty;
            }

            var Result = String.Join('-', Strings.Select(String => String.ToLower()));
            return Underlining ? '_' + Result : Result;
        }


        public static string ToCamelCase(this string String, bool Underlining = false)
        {
            return String.NotNull().SplitIdentifier().ToCamelCase(Underlining);
        }

        public static string ToPascalCase(this string String, bool Underlining = false)
        {
            return String.NotNull().SplitIdentifier().ToPascalCase(Underlining);
        }

        public static string ToSnakeCase(this string String, bool Underlining = false)
        {
            return String.NotNull().SplitIdentifier().ToSnakeCase(Underlining);
        }

        public static string ToKebabCase(this string String, bool Underlining = false)
        {
            return String.NotNull().SplitIdentifier().ToKebabCase(Underlining);
        }
        #endregion

        #region Indexes
        public static int GetLevel(this string String)
        {
            var Level = 0;

            foreach (var Char in String)
            {
                if (Char == '\t')
                {
                    Level++;
                }
                else
                {
                    break;
                }
            }

            return Level;
        }

        public static int CountOf(this string String, char Target)
        {
            var Count = 0;

            foreach (var Char in String)
            {
                if (Char == Target)
                {
                    Count++;
                }
            }
            return Count;
        }

        public static string JoinTrimEnd(this IList<string> Strings, string Separator)
        {
            ArgumentNullException.ThrowIfNull(Strings);
            ArgumentNullException.ThrowIfNull(Separator);

            var End = Strings.LastIndexOf(static String => !String.IsNullOrWhiteSpace(String));
            if (End == -1)
            {
                return String.Empty;
            }

            var Builder = new StringBuilder(Strings.Sum(static String => String.Length) + Separator.Length * End);

            Builder.Append(Strings[0]);
            for (var i = 1; i <= End; i++)
            {
                Builder.Append(Separator);
                Builder.Append(Strings[i]);
            }

            return Builder.ToString();
        }
        #endregion

        #region Parsing
        public static void SkipSpaces(this string String, ref int Index)
        {
            String.Skip(ref Index, Char.IsWhiteSpace);
        }

        public static int SkipSpaces(this string String, int Start)
        {
            return String.Skip(Start, Char.IsWhiteSpace);
        }


        public static void Skip(this string String, ref int Index, Predicate<char> Condition)
        {
            ArgumentNullException.ThrowIfNull(String);

            while (Index < String.Length && Condition(String[Index]))
            {
                Index++;
            }
        }

        public static int Skip(this string String, int Start, Predicate<char> Condition)
        {
            ArgumentNullException.ThrowIfNull(String);

            var Index = Start;
            while (Index < String.Length && Condition(String[Index]))
            {
                Index++;
            }
            return Index;
        }

        public static int Skip(this string String, int Start, params IEnumerable<char> SkipChars)
        {
            ArgumentNullException.ThrowIfNull(String);

            var Index = Start;
            while (Index < String.Length && SkipChars.Contains(String[Index]))
            {
                Index++;
            }
            return Index;
        }
        #endregion

        public static string GetExpression(this string String, int Start, out int End)
        {
            var OpenBrackets = 0;
            var CloseBrackets = 0;

            for (var i = Start; i < String.Length; i++)
            {
                switch (String[i])
                {
                    case '(':
                        OpenBrackets++;
                        break;
                    case ')':
                        CloseBrackets++;
                        if (OpenBrackets == CloseBrackets)
                        {
                            End = i + 1;
                            return String[(Start + 1)..i];
                        }
                        break;
                }
            }
            throw new Exception("The string does not contain the expression");
        }

        public static IEnumerable<char> JoinLines(this IList<IEnumerable<char>> Lines)
        {
            if (Lines is null || Lines.Count == 0)
            {
                yield break;
            }

            if (Lines.Count == 1)
            {
                foreach (var Char in Lines.First())
                {
                    yield return Char;
                }
                yield break;
            }

            foreach (var Char in Lines[0])
            {
                yield return Char;
            }

            var NewLine = Environment.NewLine;

            for (var i = 1; i < Lines.Count; i++)
            {
                foreach (var Char in NewLine)
                {
                    yield return Char;
                }
                foreach (var Char in Lines[i])
                {
                    yield return Char;
                }
            }
        }


        internal static bool IsClosingBracket(this char Char, out char Bracket)
        {
            foreach (var Pair in Brackets)
            {
                if (Char == Pair.Value)
                {
                    Bracket = Pair.Value;
                    return true;
                }
            }

            Bracket = default;
            return false;
        }
    }
}