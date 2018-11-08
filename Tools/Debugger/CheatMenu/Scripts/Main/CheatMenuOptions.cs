using System;
using System.Collections.Generic;
using System.Reflection;

namespace TEDCore.UnitTesting
{
    public partial class CheatMenuOptions
    {
        private const string EMPTY_CATEGORY_NAME = "Others";

        public string[] GetAllCategories()
        {
            List<string> categories = new List<string>();

            Type type = GetType();
            foreach (MethodInfo method in type.GetMethods())
            {
                foreach (Attribute attribute in method.GetCustomAttributes(true))
                {
                    Category category = attribute as Category;

                    if (null == category)
                    {
                        if(!categories.Contains(EMPTY_CATEGORY_NAME))
                        {
                            categories.Add(EMPTY_CATEGORY_NAME);
                        }
                        continue;
                    }

                    if (!categories.Contains(category.Name))
                    {
                        categories.Add(category.Name);
                    }
                }
            }

            return categories.ToArray();
        }

        private MethodInfo[] GetMethodInfoWithCategory(string categoryName)
        {
            List<MethodInfo> methodInfos = new List<MethodInfo>();

            foreach (MethodInfo method in GetType().GetMethods())
            {
                if(categoryName == EMPTY_CATEGORY_NAME)
                {
                    if (method.GetCustomAttributes(typeof(Category), true).Length == 0)
                    {
                        methodInfos.Add(method);
                    }
                }
                else
                {
                    foreach (Attribute attribute in method.GetCustomAttributes(typeof(Category), true))
                    {
                        Category category = attribute as Category;

                        if (null == category)
                        {
                            continue;
                        }

                        if (category.Name != categoryName)
                        {
                            continue;
                        }

                        methodInfos.Add(method);
                        break;
                    }
                }
            }

            return methodInfos.ToArray();
        }

        public UnitTestingData[] GetUnitTestingData<T>(string categoryName) where T : Attribute
        {
            List<UnitTestingData> unitTestingData = new List<UnitTestingData>();

            foreach (MethodInfo method in GetMethodInfoWithCategory(categoryName))
            {
                foreach (Attribute attribute in method.GetCustomAttributes(true))
                {
                    T test = attribute as T;

                    if (null == test)
                    {
                        continue;
                    }

                    unitTestingData.Add(new UnitTestingData(method.Name, test));
                }

            }

            return unitTestingData.ToArray();
        }

        public void RunTestMethod(string testMethodName, object[] data = null)
        {
            Type type = this.GetType();
            MethodInfo method = type.GetMethod(testMethodName);

            if (null == method)
            {
                TEDDebug.LogError("[CheatMenuOptions] - No such test method " + testMethodName);
                return;
            }

            method.Invoke(this, data);
        }
    }
}
