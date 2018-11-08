using System;

namespace TEDCore.Debugger.CheatMenu
{
    public class CheatMenuData
    {
        public string MethodName;
        public Attribute Attribute;

        public CheatMenuData(string methodName, Attribute attribute)
        {
            MethodName = methodName;
            Attribute = attribute;
        }
    }
}
