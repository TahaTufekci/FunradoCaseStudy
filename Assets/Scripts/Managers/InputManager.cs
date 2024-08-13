using Enums;
using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        private Vector3 inputPosition;
        private Camera _mainCamera;
        private RaycastHit[] _hit;
        private int _hitCount;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _hit = new RaycastHit[1];
        }

        private void Update()
        {
            if (GameManager.Instance.CurrentGameState != GameState.WaitingInput) return;
            GenerateInput();
        }

        private void GenerateInput()
        {
            // Check for mouse input (right-click)
            if (Input.GetMouseButtonDown(1))
            {
                inputPosition = Input.mousePosition;
                // Check for hit object
                var hitObject = GetHitObject(inputPosition);
                if (hitObject == null)
                {
                    Debug.Log("No object hit.");
                    return;
                }

                // Check if the hit object is a frog
                if (hitObject.TryGetComponent(out Frog frog))
                {
                    GameManager.Instance.OnValidMove?.Invoke();
                    GameManager.Instance.OnFrogClicked?.Invoke(frog);
                    GameManager.Instance.ChangeGameState(GameState.TongueMoving);
                }
                else
                {
                    Debug.Log($"Hit object is not a frog. Hit: {hitObject.name}");
                }
            }
        }

        private GameObject GetHitObject(Vector3 position)
        {
            Ray ray = _mainCamera.ScreenPointToRay(position);

            // Only raycast against the FrogLayer
            int layerMask = LayerMask.GetMask("FrogLayer");
            _hitCount = Physics.RaycastNonAlloc(ray, _hit, Mathf.Infinity, layerMask);

            if (_hitCount > 0 && _hit[0].collider != null)
            {
                return _hit[0].collider.gameObject;
            }
            return null;
        }
    }
}
