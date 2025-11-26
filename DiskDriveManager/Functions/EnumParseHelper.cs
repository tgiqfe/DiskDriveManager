using System;
using System.Collections.Generic;
using System.Text;

namespace DiskDriveManager.Functions
{
    /// <summary>
    /// String <-> Enum(s) <-> Int
    /// Parser Pattern:
    /// - String -> Enum(s)
    /// - String -> Int
    /// - Enum(s) -> String
    /// - Enum(s) -> Int
    /// - Int -> String
    /// - Int -> Enum
    /// Other:
    /// - GetCorrect (String -> String)
    /// - MergeFlags (String + Enum(s) -> Enum(s))
    /// </summary>
    internal class EnumParseHelper
    {
        #region Parser pattern methods

        public static T StringToFlags<T>(string text, Dictionary<string[], T> map) where T : Enum
        {
            var flags = default(T);
            foreach (var part in text.Split(',').Select(x => x.Trim()))
            {
                bool found = false;
                foreach (var kvp in map)
                {
                    if (kvp.Key.Any(x => string.Equals(x, part, StringComparison.OrdinalIgnoreCase)))
                    {
                        flags = (T)(object)(((int)(object)flags) | ((int)(object)kvp.Value));
                        found = true;
                        break;
                    }
                }
                if (!found) throw new ArgumentException($"The text '{text}' does not correspond to any value of the enum '{typeof(T).Name}'.");
            }
            return flags;
        }

        public static int StringToNumber<T>(string text, Dictionary<string[], T> map) where T : Enum
        {
            int number = 0;
            foreach (var part in text.Split(',').Select(x => x.Trim()))
            {
                bool found = false;
                foreach (var kvp in map)
                {
                    if (kvp.Key.Any(x => string.Equals(x, part, StringComparison.OrdinalIgnoreCase)))
                    {
                        number += (int)(object)kvp.Value;
                        found = true;
                        break;
                    }
                }
                if (!found) throw new ArgumentException($"The text '{text}' does not correspond to any known value.");
            }
            return number;
        }

        public static string FlagsToString<T>(T flags, Dictionary<string[], T> map) where T : Enum
        {
            var parts = new List<string>();
            foreach (var kvp in map)
            {
                if (flags.HasFlag(kvp.Value))
                {
                    parts.Add(kvp.Key[0]);
                }
            }
            return parts.Count > 0 ? string.Join(", ", parts) : "Unknown";
        }

        public static int FlagsToNumber<T>(T flags, Dictionary<string[], int> map) where T : Enum
        {
            var number = 0;
            foreach (var kvp in map)
            {
                if (flags.HasFlag((T)(object)kvp.Value))
                {
                    number += kvp.Value;
                }
            }
            return number;
        }

        public static string NumberToString<T>(int number, Dictionary<string[], int> map) where T : Enum
        {
            foreach(var kvp in map)
            {
                if(kvp.Value == number)
                {
                    return kvp.Key[0];
                }
            }
            return number.ToString();
        }

        public static T NumberToFlags<T>(int number, Dictionary<string[], T> map) where T : Enum
        {
            foreach(var kvp in map)
            {
                if((int)(object)kvp.Value == number)
                {
                    return kvp.Value;
                }
            }
            throw new ArgumentException($"The number '{number}' does not correspond to any value of the enum '{typeof(T).Name}'.");
        }

        #endregion
        #region Other methods

        public static string GetCorrect<T>(string text, Dictionary<string[], T> map) where T : Enum
        {
            var parts = new List<string>();
            foreach (var part in text.Split(',').Select(x => x.Trim()))
            {
                bool found = false;
                foreach (var key in map.Keys)
                {
                    if (key.Any(x => string.Equals(x, part, StringComparison.OrdinalIgnoreCase)))
                    {
                        parts.Add(key[0]);
                        found = true;
                        break;
                    }
                }
                if (!found) throw new ArgumentException($"The text '{text}' does not correspond to any value of the enum '{typeof(T).Name}'.");
            }
            return parts.Count > 0 ? string.Join(", ", parts) : "Unknown";
        }

        public static T MergeFlags<T>(string text, T existingFlags, Dictionary<string[], T> map) where T : Enum
        {
            var result = existingFlags;
            foreach (var part in text.Split(',').Select(x => x.Trim()))
            {
                bool found = false;
                string tempPart = "";
                if (part.StartsWith("-"))
                {
                    tempPart = part.TrimStart('-');
                    foreach (var kvp in map)
                    {
                        if (kvp.Key.Any(x => string.Equals(x, tempPart, StringComparison.OrdinalIgnoreCase)))
                        {
                            result = (T)(object)(((int)(object)result) & ~((int)(object)kvp.Value));
                            found = true;
                            break;
                        }
                    }
                    continue;
                }
                tempPart = part.StartsWith("+") ? part.TrimStart('+') : part;
                foreach (var kvp in map)
                {
                    if (kvp.Key.Any(x => string.Equals(x, tempPart, StringComparison.OrdinalIgnoreCase)))
                    {
                        result = (T)(object)(((int)(object)result) | ((int)(object)kvp.Value));
                        found = true;
                        break;
                    }
                }
                if (!found) throw new ArgumentException($"The text '{text}' does not correspond to any value of the enum '{typeof(T).Name}'.");
            }
            return result;
        }

        #endregion
    }
}
