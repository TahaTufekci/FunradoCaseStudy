using JsonReader;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelReader levelReader;
        public LevelData CurrentLevel { get; private set; }
        public static LevelManager Instance { get; private set; }
        public int CurrentLevelIndex { get; set; } = 1;
        public int TotalLevelNumber { get; private set; }

        private void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
            {
                Instance = this;
            }
            TotalLevelNumber = GetTotalLevelsCount();
            LoadLevel();
        }

        private void LoadLevel()
        {
            if (PlayerPrefs.HasKey("LevelIndex"))
            {
                SetCurrentLevel(levelReader.LoadLevel(PlayerPrefs.GetInt("LevelIndex")));
            }
            else
            {
                SetCurrentLevel(levelReader.LoadLevel(CurrentLevelIndex));
            }
        }

         private void SetCurrentLevel(LevelData level)
        {
            CurrentLevel = level;
        }

        public void SetCurrentLevelIndex(int levelIndex)
        {
            if (levelIndex < 0)
            {
                Debug.LogError("Level index cannot be negative.");
                return;
            }

            CurrentLevelIndex = levelIndex;
            LoadLevel();
        }

        public int GetTotalLevelsCount()
        {
            return LevelCountUtility.GetLevelCount();
        }
    }
}
