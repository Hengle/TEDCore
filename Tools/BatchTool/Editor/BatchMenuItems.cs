#if UNITY_EDITOR
using UnityEditor;

public class BatchMenuItems
{
    [MenuItem("GameObject/UI Batch/Batch Active Object", priority = 20)]
    private static void BatchActiveObject()
    {
        if(!Selection.activeTransform)
        {
            return;
        }

        Undo.RegisterFullObjectHierarchyUndo(Selection.activeTransform, "Batch");
        BatchTool.Batch(Selection.activeTransform);
    }
}
#endif