using UnityEditor;
using TEDCore.Utils;

[CustomPropertyDrawer(typeof(TestStringEnum))]
public class TestStringEnumPropertyDrawer : StringEnumPropertyDrawer<TestEnum>
{

}