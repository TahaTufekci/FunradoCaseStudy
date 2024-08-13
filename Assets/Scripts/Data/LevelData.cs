using System.Collections.Generic;

[System.Serializable]
public class LevelData
{
    public int moveLimit;
    public int rows;
    public int columns;
    public List<Content> contents;
}
public enum ContentType
{
    Frog,
    Berry,
    Arrow
}

[System.Serializable]
public class Position
{
    public int x;
    public int y;
    public int z;
}

[System.Serializable]
public class Content
{
    public ContentType type;
    public Position position;
    public int colorValue;
    public Direction direction;
}

[System.Serializable]
public enum Direction
{
    Up,
    Right,
    Down,
    Left
}