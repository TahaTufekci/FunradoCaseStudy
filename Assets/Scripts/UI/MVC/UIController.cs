using Models;
using System;

namespace Controllers
{
    public class UIController
    {
        private UIModel _model;
        public event Action OnMoveCountChanged;

        public UIController(UIModel model)
        {
            _model = model;
        }

        public void DecreaseMoveCount()
        {
            _model.SetCurrentMoveCount(_model.CurrentMoveCount - 1);
            OnMoveCountChanged?.Invoke();
        }

        public int GetCurrentMoveCount()
        {
            return _model.CurrentMoveCount;
        }

        public void SetInitialMoveCount(int count)
        {
            _model.SetCurrentMoveCount(count);
        }
    }
}
