using UnityEngine;

[CreateAssetMenu(fileName = "New Arrow", menuName = "ScriptableObjects/ArrowSO")]
public class ArrowSO : ScriptableObject
{
    public Material material;
    public int colorValue;
}
