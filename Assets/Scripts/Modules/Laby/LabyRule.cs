using UnityEngine;

public struct LabyRule
{
    public WallState[,] laby;
    public Vector2Int labySize; //Info redondante mais plus simple pour les modules

    public Vector2Int labyStart;
    public Vector2Int labyEnd;

    public WallState[,] GetLaby()
    {
        return laby;
    }

    public Vector2Int GetLabySize()
    {
        return labySize;
    }

    public Vector2Int GetStartPos()
    {
        return labyStart;
    }

    public Vector2Int GetEndPos()
    {
        return labyEnd;
    }
}
