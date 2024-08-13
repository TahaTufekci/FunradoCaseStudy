using UnityEngine;

[CreateAssetMenu(fileName = "NewFrog", menuName = "ScriptableObjects/FrogSO", order = 2)]
public class FrogSO : ScriptableObject
{
    public Texture2D texture; // Texture for the frog
    public int colorValue; // Color value of the frog
}
