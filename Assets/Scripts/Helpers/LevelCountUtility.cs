#if UNITY_EDITOR
using UnityEditor;

public class LevelCountUtility
{
    public static int GetLevelCount()
    {
        string[] guids = AssetDatabase.FindAssets("t:TextAsset", new[] { "Assets/Resources/Levels" });
        return guids.Length;
    }
}
#endif
