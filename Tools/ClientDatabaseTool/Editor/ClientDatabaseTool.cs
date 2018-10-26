using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace TEDCore.ClientDatabase
{
    public class ClientDatabaseTool : EditorWindow
    {
        private const string TEMPLATE_PATH = "Assets/Tools/ClientDatabaseTool/Templates/";
        private const string TEMPLATE_DATABASE_NAME = "Template_Database.txt";
        private const string TEMPLATE_DATABASEMANAGER_NAME = "Template_DatabaseManager.txt";
        private const string TEMPLATE_SCRIPTABLE_OBJECT_NAME = "Template_ScriptableObject.txt";

        private const string CLIENT_DATABASE_ROOT = "/ClientDatabase/";
        private const string CSV_PATH = "CsvResources/";
        private const string GENERATE_SCRIPT_PATH = "GenerateScripts/";
        private const string SCRIPTABLE_OBJECT_PATH = "Resources/";

        [MenuItem("TEDCore/Client Database/Initialize")]
        private static void Initialize()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + CLIENT_DATABASE_ROOT);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            directoryInfo = new DirectoryInfo(Application.dataPath + CLIENT_DATABASE_ROOT + CSV_PATH);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            AssetDatabase.Refresh();
        }


        [MenuItem("TEDCore/Client Database/Generate Scripts")]
        private static void GenerateScript()
        {
            ClientDatabaseTool window = EditorWindow.GetWindow<ClientDatabaseTool>();
            window.StartGenerateScript();
        }

        private int m_dataId;
        private string m_registerList;
        private string m_convertList;
        private bool m_compiling = false;
        private float m_timer = 1.0f;
        private List<string> m_classNames;
        private float m_currentProgress;
        private int m_totalProgress;

        private void Update()
        {
            if (!m_compiling)
            {
                return;
            }

            m_timer -= Time.deltaTime;
            if (m_timer <= 0)
            {
                m_timer = 0;

                if (!EditorApplication.isCompiling)
                {
                    m_compiling = false;

                    GenerateScriptableObject();
                    AssetDatabase.Refresh();
                    TEDDebug.Log("[ClientDatabaseTool] - Generating client database scripts has completed.");

                    EditorUtility.ClearProgressBar();

                    ClientDatabaseTool window = EditorWindow.GetWindow<ClientDatabaseTool>();
                    window.Close();
                }
            }
        }


        private void StartGenerateScript()
        {
            InitializeTool();
            CreateDatabaseScript();
            CreateDatabaseManagerScript();

            AssetDatabase.Refresh();

            m_compiling = true;
            m_timer = 1.0f;

            UpdateProgressBar("Please Wait", "Wait Editor Compiling....");
        }


        private void InitializeTool()
        {
            m_dataId = 0;
            m_registerList = string.Empty;
            m_convertList = string.Empty;
            m_classNames = new List<string>();

            if (Directory.Exists(Application.dataPath + CLIENT_DATABASE_ROOT + GENERATE_SCRIPT_PATH))
            {
                Directory.Delete(Application.dataPath + CLIENT_DATABASE_ROOT + GENERATE_SCRIPT_PATH, true);
            }

            Directory.CreateDirectory(Application.dataPath + CLIENT_DATABASE_ROOT + GENERATE_SCRIPT_PATH);
        }


        private void CreateDatabaseScript()
        {
            string[] csvPaths = Directory.GetFiles(Application.dataPath + CLIENT_DATABASE_ROOT + CSV_PATH, "*.csv", SearchOption.AllDirectories);
            string assetPath = "";
            TextAsset textAsset = null;
            m_totalProgress = csvPaths.Length * 2;
            m_currentProgress = 0;

            for (int cnt = 0; cnt < csvPaths.Length; cnt++)
            {
                assetPath = "Assets" + csvPaths[cnt].Replace(Application.dataPath, "").Replace('\\', '/');
                textAsset = (TextAsset)AssetDatabase.LoadAssetAtPath(assetPath, typeof(TextAsset));

                m_currentProgress++;
                UpdateProgressBar("Generate Data Script", textAsset.name + ".cs");

                m_registerList += string.Format("RegisterDataType (new {0}Database());\n", textAsset.name);
                if (cnt != csvPaths.Length - 1)
                {
                    m_registerList += "\t\t\t";
                }

                m_convertList += string.Format("CsvToJsonConverter.Convert<{0}Data>(\"{0}\");\n", textAsset.name);
                if (cnt != csvPaths.Length - 1)
                {
                    m_convertList += "\t\t\t";
                }

                CreateDatabaseScript(textAsset);
                CreateScriptableObjectScript(textAsset);
            }
        }


        private void CreateDatabaseScript(TextAsset textAsset)
        {
            m_dataId++;

            string template = GetTemplate(TEMPLATE_PATH + TEMPLATE_DATABASE_NAME);
            template = template.Replace("$DataClassName", textAsset.name + "Data");
            template = template.Replace("$DataAttributes", GetClassParameters(textAsset));
            template = template.Replace("$DataTypeName", textAsset.name + "Database");
            template = template.Replace("$DataID", m_dataId.ToString());
            template = template.Replace("$DataPath", "\"" + textAsset.name + "DataScriptableObject\"");
            template = template.Replace("$DataScriptableName", textAsset.name + "DataScriptableObject");

            GenerateScript(textAsset.name + "Database", template);
        }


        private void CreateScriptableObjectScript(TextAsset textAsset)
        {
            m_classNames.Add(textAsset.name + "DataScriptableObject");

            string template = GetTemplate(TEMPLATE_PATH + TEMPLATE_SCRIPTABLE_OBJECT_NAME);
            template = template.Replace("$DataClassName", textAsset.name + "Data");
            template = template.Replace("$DataAttributes", GetClassParameters(textAsset));
            template = template.Replace("$CsvSerialize", GetCsvSerialize(textAsset));
            template = template.Replace("$TextAssetPath", "\"Assets" + CLIENT_DATABASE_ROOT + CSV_PATH + textAsset.name + ".csv\"");

            GenerateScript(textAsset.name + "DataScriptableObject", template);
        }


        private void CreateDatabaseManagerScript()
        {
            string template = GetTemplate(TEMPLATE_PATH + TEMPLATE_DATABASEMANAGER_NAME);
            template = template.Replace("$RegisterList", m_registerList);
            GenerateScript("DatabaseManager", template);
        }


        private string GetClassParameters(TextAsset textAsset)
        {
            string[] csvParameter = CsvConverter.SerializeCSVParameter(textAsset);
            int keyCount = csvParameter.Length;

            string classParameters = string.Empty;

            for (int cnt = 0; cnt < keyCount; cnt++)
            {
                string[] attributes = csvParameter[cnt].Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);
                classParameters += string.Format("public {0} {1};", attributes[0], attributes[1]);

                if (cnt != keyCount - 1)
                {
                    classParameters += "\n";
                    classParameters += "\t\t";
                }
            }

            return classParameters;
        }


        private string GetCsvSerialize(TextAsset textAsset)
        {
            string[] csvParameter = CsvConverter.SerializeCSVParameter(textAsset);
            int keyCount = csvParameter.Length;

            string csvSerialize = string.Empty;

            for (int cnt = 0; cnt < keyCount; cnt++)
            {
                string[] attributes = csvParameter[cnt].Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);

                if (attributes[0] == "string")
                {
                    csvSerialize += string.Format("temp.{0} = datas[i][{1}];", attributes[1], cnt);
                }
                else if (attributes[0] == "bool")
                {
                    csvSerialize += GetCsvSerialize(attributes, cnt, "false");
                }
                else if (attributes[0] == "int")
                {
                    csvSerialize += GetCsvSerialize(attributes, cnt, "0");
                }
                else if (attributes[0] == "float")
                {
                    csvSerialize += GetCsvSerialize(attributes, cnt, "0.0f");
                }
                else if (attributes[0] == "string[]")
                {
                    csvSerialize += string.Format("temp.{0} = CsvConverter.ConvertToArray<string>(datas[i][{1}]);", attributes[1], cnt);
                }
                else if (attributes[0] == "bool[]")
                {
                    csvSerialize += string.Format("temp.{0} = CsvConverter.ConvertToArray<bool>(datas[i][{1}]);", attributes[1], cnt);
                }
                else if (attributes[0] == "int[]")
                {
                    csvSerialize += string.Format("temp.{0} = CsvConverter.ConvertToArray<int>(datas[i][{1}]);", attributes[1], cnt);
                }
                else if (attributes[0] == "float[]")
                {
                    csvSerialize += string.Format("temp.{0} = CsvConverter.ConvertToArray<float>(datas[i][{1}]);", attributes[1], cnt);
                }

                if (cnt != keyCount - 1)
                {
                    csvSerialize += "\n";
                    csvSerialize += "\t\t\t\t";
                }
            }

            return csvSerialize;
        }


        private static string GetCsvSerialize(string[] attributes, int arrayCount, string defaultValue)
        {
            string csvSerialize = "";

            csvSerialize += string.Format("\n\t\t\t\tif(!{0}.TryParse(datas[i][{1}], out temp.{2}))\n", attributes[0], arrayCount, attributes[1]);
            csvSerialize += "\t\t\t\t{\n";
            csvSerialize += string.Format("\t\t\t\t\ttemp.{0} = {1};\n", attributes[1], defaultValue);
            csvSerialize += "\t\t\t\t}";

            return csvSerialize;
        }


        private string GetTemplate(string path)
        {
            TextAsset txt = (TextAsset)AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset));

            return txt.text;
        }


        public void GenerateScript(string dataName, string data)
        {
            dataName = Application.dataPath + CLIENT_DATABASE_ROOT + GENERATE_SCRIPT_PATH + dataName + ".cs";

            if (File.Exists(dataName))
            {
                File.Delete(dataName);
            }

            StreamWriter sr = File.CreateText(dataName);
            sr.WriteLine(data);
            sr.Close();
        }


        private void GenerateScriptableObject()
        {
            if (Directory.Exists(Application.dataPath + CLIENT_DATABASE_ROOT + SCRIPTABLE_OBJECT_PATH))
            {
                Directory.Delete(Application.dataPath + CLIENT_DATABASE_ROOT + SCRIPTABLE_OBJECT_PATH, true);
            }

            Directory.CreateDirectory(Application.dataPath + CLIENT_DATABASE_ROOT + SCRIPTABLE_OBJECT_PATH);

            for (int i = 0; i < m_classNames.Count; i++)
            {
                GenerateScriptableObject(m_classNames[i]);
            }
        }


        private void GenerateScriptableObject(string className)
        {
            m_currentProgress++;
            UpdateProgressBar("Generate Scriptable Objects", className + ".asset");

            MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>("Assets" + CLIENT_DATABASE_ROOT + GENERATE_SCRIPT_PATH + className + ".cs");

            string targetPath = "Assets" + CLIENT_DATABASE_ROOT + SCRIPTABLE_OBJECT_PATH + className + ".asset";

            if (File.Exists(targetPath))
            {
                File.Delete(targetPath);
            }

            Object asset = ScriptableObject.CreateInstance(script.GetClass());
            AssetDatabase.CreateAsset(asset, targetPath);
            AssetDatabase.SaveAssets();

            ClientDatabaseScriptableObject scriptableObject = AssetDatabase.LoadAssetAtPath<ClientDatabaseScriptableObject>("Assets" + CLIENT_DATABASE_ROOT + SCRIPTABLE_OBJECT_PATH + className + ".asset");
            scriptableObject.LoadTextAsset();
        }


        private void UpdateProgressBar(string title, string info)
        {
            float process = m_currentProgress / m_totalProgress;
            EditorUtility.DisplayProgressBar(title, string.Format("[{0}%] {1}", (int)(process * 100), info), process);
        }
    }
}