using System;
using Enums;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Actions
        public Action<GameState> OnGameStateChanged;
        public Action<Frog> OnFrogClicked;
        public Action OnValidMove;
        public Action OnMoveNumberDecrease;
        public Action OnFrogDestroyed;
        #endregion

        public GameState CurrentGameState { get; private set; }
        public static GameManager Instance { get; private set; }
        private int _moveNumber;

        private void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
            {
                Instance = this;
            }
            CurrentGameState = GameState.WaitingInput;
            _moveNumber = LevelManager.Instance.CurrentLevel.moveLimit == 0 ? int.MaxValue : LevelManager.Instance.CurrentLevel.moveLimit;
        }

        public int GetCurrentMoveCount() => _moveNumber;

        public void ChangeGameState(GameState state)
        {
            if (CurrentGameState != state)
            {
                CurrentGameState = state;
                OnGameStateChanged?.Invoke(state);
            }
        }

        private void UpdateCurrentMoveNumber()
        {
            --_moveNumber;
            OnMoveNumberDecrease?.Invoke();
            if (_moveNumber == 0)
            {
                ChangeGameState(GameState.Lose);
            }
        }

        private void CheckWinCondition()
        {
            if (GridManager.Instance.FrogList.Count <= 0)
            {
                if (PlayerPrefs.GetInt("LevelIndex") == LevelManager.Instance.TotalLevelNumber)
                {
                    ChangeGameState(GameState.Finish);
                }
                else if (PlayerPrefs.HasKey("LevelIndex"))
                {
                    PlayerPrefs.SetInt("LevelIndex", PlayerPrefs.GetInt("LevelIndex") + 1);
                    ChangeGameState(GameState.Win);
                }
                else
                {
                    PlayerPrefs.SetInt("LevelIndex", ++LevelManager.Instance.CurrentLevelIndex);
                    ChangeGameState(GameState.Win);
                }
            }
        }

        private void OnEnable()
        {
            OnValidMove += UpdateCurrentMoveNumber;
            OnFrogDestroyed += CheckWinCondition;
        }

        private void OnDisable()
        {
            OnValidMove -= UpdateCurrentMoveNumber;
            OnFrogDestroyed -= CheckWinCondition;
        }
    }
}
