using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCell", menuName = "ScriptableObjects/CellSO")]
public class CellSO : ScriptableObject
{
    public Texture2D texture; // Texture for the cell
    public int colorValue; // Color value of the cell
}
