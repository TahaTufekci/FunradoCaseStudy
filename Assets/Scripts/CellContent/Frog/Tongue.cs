using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Managers;
using Enums;
using System.Linq;

public class Tongue : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    private Direction _direction;
    private Dictionary<Vector3, CellContent> _tonguePositions = new Dictionary<Vector3, CellContent>();
    private Dictionary<GameObject, Cell> _berryPositions = new Dictionary<GameObject, Cell>();
    private List<Arrow> _arrows = new List<Arrow>();

    #region Boolean Variables
    private bool _encounteredInvalidBerry;
    private bool isBerryCollected;
    #endregion

    private int currentCellX, currentCellY, currentCellZ;
    private int _frogColorValue;
    private float berryHeightOffset = 0.28f;
    private float frogHeightOffset = 0.065f;
    private float moveDuration = 0.2f;


    public void Initialize(Cell currentCell, Direction direction, Frog frog)
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _direction = direction;
        _tonguePositions.Clear();
        _berryPositions.Clear();
        _frogColorValue = frog.ColorValue;
        _encounteredInvalidBerry = false;

        transform.position = frog.transform.position + Vector3.up * frogHeightOffset;

        AddTonguePosition(transform.position, frog);
        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, transform.position);

        currentCellX = currentCell.PosX;
        currentCellY = currentCell.PosY;
        currentCellZ = currentCell.PosZ;

        StartCoroutine(StartTongueMovement(frog));
    }

    private IEnumerator StartTongueMovement(Frog frog)
    {
        var currentBaseCell = GridManager.Instance.GetBaseCell(currentCellX, currentCellY);

        while (currentBaseCell != null && !_encounteredInvalidBerry)
        {
            var nextCell = GridManager.Instance.GetNextCell(currentBaseCell, _direction);
            if (nextCell != null)
            {
                ProcessCell(nextCell);
                currentBaseCell = GridManager.Instance.GetBaseCell(nextCell.PosX, nextCell.PosY);
            }
            else
            {
                break;
            }

            yield return new WaitForSeconds(moveDuration);
        }

        if (_encounteredInvalidBerry)
        {
            yield return ReturnWithoutBerries(frog);
        }
        else if (isBerryCollected)
        {
            yield return ReturnWithBerries(frog);
        }

        Destroy(gameObject);

        if (GameManager.Instance.GetCurrentMoveCount() != 0)
        {
            GameManager.Instance.ChangeGameState(GameState.WaitingInput);
        }
    }

    private void ProcessCell(Cell nextCell)
    {
        AddTonguePosition(nextCell.transform.position, nextCell.GetContent());

        if (nextCell.HasBerry())
        {
            HandleBerry(nextCell);
        }

        if (nextCell.HasArrow())
        {
            HandleArrow(nextCell);
        }

        if (nextCell.HasFrog())
        {
            _encounteredInvalidBerry = true;
        }
    }

    private void HandleBerry(Cell nextCell)
    {
        var berry = nextCell.GetContent().gameObject;
        var berryComponent = berry.GetComponent<Berry>();

        if (berryComponent != null && berryComponent.ColorValue == _frogColorValue)
        {
            _berryPositions[berry] = nextCell;
            berryComponent.Interact();
            isBerryCollected = true;
        }
        else
        {
            _encounteredInvalidBerry = true;
        }
    }

    private void HandleArrow(Cell nextCell)
    {
        var arrow = nextCell.GetContent().gameObject;
        var arrowComponent = arrow.GetComponent<Arrow>();
        if (arrowComponent != null && arrowComponent.ColorValue == _frogColorValue)
        {
            _direction = arrowComponent.GetDirection();
            _arrows.Add(arrowComponent);
            arrowComponent.Interact();
            isBerryCollected = true;
        }
        else
        {
            _encounteredInvalidBerry = true;
        }
    }

    private void AddTonguePosition(Vector3 position, CellContent content)
    {
        _tonguePositions.Add(position, content);
        _lineRenderer.positionCount = _tonguePositions.Count;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, position + Vector3.up * berryHeightOffset);
    }

    private IEnumerator ReturnWithoutBerries(Frog frog)
    {
        for (int i = _tonguePositions.Count - 1; i >= 0; i--)
        {
            MoveTongueToPosition(i);
            yield return new WaitForSeconds(moveDuration);
        }
    }

    private void MoveTongueToPosition(int index)
    {
        Vector3 currentPosition = _tonguePositions.ElementAt(index).Key + Vector3.up * berryHeightOffset;
        _lineRenderer.positionCount = index + 1;
        _lineRenderer.SetPosition(index, currentPosition);
    }

    private IEnumerator ReturnWithBerries(Frog frog)
    {
        var collectedBerries = new List<GameObject>(_berryPositions.Keys);
        var collectedTonguePositions = new List<Vector3>(_tonguePositions.Keys);
        bool isArrow = false;
        int berryIndex = 0;

        for (int i = _tonguePositions.Count - 1; i > 0; i--)
        {
            Vector3 currentPosition = collectedTonguePositions[i];
            Vector3 nextPosition = collectedTonguePositions[i - 1];

            MoveTongueToPosition(i);

            if (berryIndex < collectedBerries.Count)
            {
                var berry = collectedBerries[collectedBerries.Count - 1 - berryIndex];
                if (berry != null)
                {
                    MoveBerryToPosition(berry, currentPosition, moveDuration);
                    yield return new WaitForSeconds(moveDuration);
                }
                isArrow = _tonguePositions[nextPosition] is Arrow ? true : false;
                if (!isArrow) berryIndex++;
            }

            MoveBerriesToNextPosition(collectedBerries, collectedTonguePositions, nextPosition, berryIndex, moveDuration, isArrow, currentPosition);
        }

        yield return MoveBerriesToFrog(collectedBerries, frog.transform.position, moveDuration);
        DestroyCollectedObjects(collectedBerries);
        DestroyArrows();
        DestroyFrog(frog);
    }

    private void MoveBerryToPosition(GameObject berry, Vector3 position, float duration)
    {
        berry.transform.DOMove(position + Vector3.up * berryHeightOffset, duration)
            .SetEase(Ease.InSine);
    }

    private void MoveBerriesToNextPosition(List<GameObject> berries, List<Vector3> positions, Vector3 nextPosition, int berryIndex, float duration, bool isArrow, Vector3 currentPosition)
    {
        for (int j = berryIndex - 1; j >= 0; j--)
        {
            var berry = berries[berries.Count - 1 - j];
            if (berry != null)
            {
                bool isFirstBerryInLoop = (j == berryIndex - 1);

                berry.transform.DOMove(nextPosition + Vector3.up * berryHeightOffset, duration)
                    .SetEase(Ease.InSine)
                    .OnComplete(() =>
                    {
                        if (isArrow && isFirstBerryInLoop)
                        {
                            GridManager.Instance.DestroyArrowCell(_tonguePositions[nextPosition].gameObject);
                        }

                        if (isFirstBerryInLoop)
                        {
                            GridManager.Instance.DestroyBerryCell(_tonguePositions[currentPosition].gameObject);
                        }
                    });
            }
        }
    }

    private IEnumerator MoveBerriesToFrog(List<GameObject> berries, Vector3 frogPosition, float duration)
    {
        Sequence berrySequence = DOTween.Sequence();

        foreach (var berry in berries)
        {
            if (berry != null)
            {
                berrySequence.Join(berry.transform.DOMove(frogPosition + Vector3.up * berryHeightOffset, duration)
                    .SetEase(Ease.InSine));
            }
        }

        yield return berrySequence.WaitForCompletion();
    }

    private void DestroyCollectedObjects(List<GameObject> berries)
    {
        foreach (var berry in berries)
        {
            if (berry != null)
            {
                Destroy(berry);
            }
        }
    }

    private void DestroyArrows()
    {
        foreach (var arrow in _arrows)
        {
            Destroy(arrow.gameObject);
        }
    }

    private void DestroyFrog(Frog frog)
    {
        var frogCell = GridManager.Instance.GetCell(frog.PosX, frog.PosY, frog.PosZ);
        var frogBaseCell = GridManager.Instance.GetBaseCell(frogCell.PosX, frog.PosY);
        frogBaseCell.RemoveCell();
        GridManager.Instance.FrogList.Remove(frog);
        Destroy(frog.gameObject);
        GameManager.Instance.OnFrogDestroyed?.Invoke();
    }
}
