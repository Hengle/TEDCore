using System;

namespace TEDCore.Debugger.CheatMenu
{
    public class CheatCategory : Attribute
    {
        public string CategoryName;
        public CheatCategory(string categoryName)
        {
            CategoryName = categoryName;
        }
    }
}