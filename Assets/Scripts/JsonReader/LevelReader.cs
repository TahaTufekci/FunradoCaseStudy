using UnityEngine;

namespace JsonReader
{
    public class LevelReader : MonoBehaviour
    {
        [SerializeField] private string levelsFolder = "Levels";

        public LevelData LoadLevel(int levelIndex)
        {
            string fileName = $"Level{levelIndex}"; // Assuming level files are named like level0, level1, etc.
            TextAsset levelFile = Resources.Load<TextAsset>($"{levelsFolder}/{fileName}");

            if (levelFile != null)
            {
                return JsonUtility.FromJson<LevelData>(levelFile.text);
            }
            else
            {
                Debug.LogError($"Level file not found: {fileName}");
                return null;
            }
        }
    }
}
