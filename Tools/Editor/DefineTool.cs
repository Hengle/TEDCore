using UnityEditor;
using System;

public class DefineTool
{
    private const string DEVELOP_DEFINE = "DEVELOP";
    private const string RELEASE_DEFINE = "RELEASE";

    [MenuItem("TEDTools/Define/Develop")]
    public static void SetDefineSymbolDevelop()
    {
        EditorPrefs.SetBool("develop", true);

        string[] separate = new string[]{";"};
        string[] defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(separate, StringSplitOptions.RemoveEmptyEntries);
        string result = DEVELOP_DEFINE;

        for (int cnt = 0; cnt < defines.Length; cnt++)
        {
            if (!IsVersionDefine(defines[cnt]))
            {
                result += ";";
                result += defines[cnt];
            }
        }

        PlayerSettings.SetScriptingDefineSymbolsForGroup (EditorUserBuildSettings.selectedBuildTargetGroup, result);
    }
    [MenuItem("TEDTools/Define/Develop", true)]
    private static bool SetDefineSymbolDevelopValidate()
    {
        Menu.SetChecked("TEDTools/Define/Develop", EditorPrefs.GetBool("develop", true));
        Menu.SetChecked("TEDTools/Define/Release", !EditorPrefs.GetBool("develop", true));
        return !EditorPrefs.GetBool("develop", true);
    }


    [MenuItem("TEDTools/Define/Release")]
    public static void SetDefineSymbolRelease()
    {
        EditorPrefs.SetBool("develop", false);

        string[] separate = new string[]{";"};
        string[] defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(separate, StringSplitOptions.RemoveEmptyEntries);
        string result = RELEASE_DEFINE;

        for (int cnt = 0; cnt < defines.Length; cnt++)
        {
            if (!IsVersionDefine(defines[cnt]))
            {
                result += ";";
                result += defines[cnt];
            }
        }

        PlayerSettings.SetScriptingDefineSymbolsForGroup (EditorUserBuildSettings.selectedBuildTargetGroup, result);
    }
    [MenuItem("TEDTools/Define/Release", true)]
    private static bool SetDefineSymbolReleaseValidate()
    {
        Menu.SetChecked("TEDTools/Define/Develop", EditorPrefs.GetBool("develop", true));
        Menu.SetChecked("TEDTools/Define/Release", !EditorPrefs.GetBool("develop", true));
        return EditorPrefs.GetBool("develop", true);
    }


    private static bool IsVersionDefine(string value)
    {
        if (value == DEVELOP_DEFINE ||
            value == RELEASE_DEFINE)
        {
            return true;
        }

        return false;
    }
}
