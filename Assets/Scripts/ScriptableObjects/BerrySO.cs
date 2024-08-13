using UnityEngine;

[CreateAssetMenu(fileName = "NewBerry", menuName = "ScriptableObjects/BerrySO", order = 1)]
public class BerrySO : ScriptableObject
{
    public Texture2D texture; // Texture for the berry
    public int colorValue; // Color value of the berry
}
