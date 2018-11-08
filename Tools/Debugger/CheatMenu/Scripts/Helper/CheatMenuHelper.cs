﻿using System;
using System.Collections.Generic;

namespace TEDCore.Debugger.CheatMenu
{
    public static class CheatMenuHelper
    {
        public static string ToScriptName(this string methodName)
        {
            string result = string.Empty;
            List<int> capitalIndexes = new List<int>();

            for (int i = 0; i < methodName.Length; i++)
            {
                if (Char.IsUpper(methodName[i]))
                {
                    capitalIndexes.Add(i);
                }
            }

            char cacheChar = ' ';
            for (int i = 0; i < methodName.Length; i++)
            {
                cacheChar = methodName[i];

                if (capitalIndexes.Contains(i))
                {
                    if (!capitalIndexes.Contains(i - 1))
                    {
                        result += " ";
                    }
                    else if(i + 1 < methodName.Length && !capitalIndexes.Contains(i + 1))
                    {
                        result += " ";
                    }
                }

                result += cacheChar;
            }

            return result;
        }
    }
}
