using System;
using System.Linq;
using Controllers;
using DG.Tweening;
using Enums;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup losePanel;
        [SerializeField] private CanvasGroup winPanel;
        [SerializeField] private CanvasGroup pausePanel;
        [SerializeField] private CanvasGroup finishPanel;
        [SerializeField] private Image mainMask;
        [SerializeField] private TextMeshProUGUI moveNoText;
        [SerializeField] private TextMeshProUGUI levelNoText;
        private UIController _controller;

        private void Awake()
        {
            var model = new UIModel();
            _controller = new UIController(model);

            _controller.OnMoveCountChanged += UpdateMoveNumber;
        }
        private void Start()
        {
            var initialMoveCount = LevelManager.Instance.CurrentLevel.moveLimit;
            _controller.SetInitialMoveCount(initialMoveCount);

            moveNoText.text = initialMoveCount == 0 ? "? Moves" : $"{_controller.GetCurrentMoveCount()} Moves";

            var currentLevelNo = GetCurrentLevelNumber();
            levelNoText.text = $"Level: {currentLevelNo}";
        }
       
        public int GetCurrentLevelNumber()
        {
            if (PlayerPrefs.HasKey("LevelIndex"))
            {
                return PlayerPrefs.GetInt("LevelIndex");
            }
            else
            {
                return LevelManager.Instance.CurrentLevelIndex;
            }
        }

        private void ControlPanels(GameState gameState)
        {
            // Only fade in the main mask if the game state is Win, Lose, or Pause
            if (gameState.HasFlag(GameState.Win) || gameState.HasFlag(GameState.Lose) || gameState.HasFlag(GameState.Pause) || gameState.HasFlag(GameState.Finish))
            {
                mainMask.DOFade(0.5f, 0.5f).SetDelay(0.8f).From(0f);
                SetMaskState(mainMask, true);
            }
            else
            {
                SetMaskState(mainMask, false);
            }

            var sequence = DOTween.Sequence();
            var delay = 0.8f;

            if (gameState.HasFlag(GameState.Lose))
            {
                sequence.PrependInterval(delay).OnComplete(() => OpenLosePanel());
            }
            else if (gameState.HasFlag(GameState.Win))
            {
                sequence.PrependInterval(delay).OnComplete(() => OpenWinPanel());
            }
            else if (gameState.HasFlag(GameState.Pause))
            {
                sequence.PrependInterval(delay).OnComplete(() => OpenPausePanel());
            }
            else if (gameState.HasFlag(GameState.Finish))
            {
                sequence.PrependInterval(delay).OnComplete(() => OpenFinishPanel());
            }
        }
        private void OpenFinishPanel()
        {
            finishPanel.gameObject.SetActive(true);
        }
        public void OpenPausePanel()
        {
            pausePanel.gameObject.SetActive(true);
        }
        private void OpenWinPanel()
        {
            winPanel.gameObject.SetActive(true);
        }
        public void OpenLosePanel()
        {
            losePanel.gameObject.SetActive(true);
        }
        private void SetMaskState(Image mask, bool isActive, Action onClickAction = null)
        {
            if (isActive)
            {
                SetMaskClickAction(mask, onClickAction);
                mask.gameObject.SetActive(true);
            }
            else
            {
                mask.gameObject.SetActive(false);
            }
        }

        private void SetMaskClickAction(Image mask, Action action)
        {
            var trigger = mask.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            trigger.triggers.Clear();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => { action?.Invoke(); });
            trigger.triggers.Add(entry);
        }
        private void UpdateMoveNumber()
        {
            if (LevelManager.Instance.CurrentLevel.moveLimit != 0)
            {
                moveNoText.text = $"{GameManager.Instance.GetCurrentMoveCount().ToString()} Moves";
            }
        }

        private void OnEnable()
        {
            GameManager.Instance.OnMoveNumberDecrease += UpdateMoveNumber;
            GameManager.Instance.OnGameStateChanged += ControlPanels;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnMoveNumberDecrease -= UpdateMoveNumber;
            GameManager.Instance.OnGameStateChanged -= ControlPanels;
        }
    }
}
