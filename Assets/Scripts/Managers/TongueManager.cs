using Managers;
using UnityEngine;

public class TongueManager : MonoBehaviour
{
    [SerializeField] private GameObject tonguePrefab;

    private void InitializeTongue(Frog frog)
    {
        frog.Interact();
        var tongueObject = Instantiate(tonguePrefab, frog.transform.position, Quaternion.identity);
        var tongue = tongueObject.GetComponent<Tongue>();
        var currentCell = GridManager.Instance.GetCell(frog.PosX, frog.PosY, frog.PosZ);
        tongue.Initialize(currentCell, frog.Direction, frog);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnFrogClicked += InitializeTongue;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnFrogClicked -= InitializeTongue;
    }
}
