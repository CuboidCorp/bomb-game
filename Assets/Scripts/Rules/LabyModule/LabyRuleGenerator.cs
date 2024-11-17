using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LabyRuleGenerator
{

    private const int LABY_HEIGHT = 6;
    private const int LABY_WIDTH = 6;

    private LabyCell[,] laby;

    private Vector2Int startPos;
    private Vector2Int endPos;

    public void SetupRules()
    {
        GenerateMaze(LABY_WIDTH, LABY_HEIGHT);
        //Les positions de départ et d'arrivée sont les coins opposés du labyrinthe (Random d'un coté et l'opposé de l'autre)
        int startCorner = Random.Range(0, 4);
        startPos = startCorner switch
        {
            0 => new Vector2Int(0, 0),
            1 => new Vector2Int(LABY_WIDTH - 1, 0),
            2 => new Vector2Int(0, LABY_HEIGHT - 1),
            3 => new Vector2Int(LABY_WIDTH - 1, LABY_HEIGHT - 1),
            _ => new Vector2Int(0, 0),
        };

        endPos = startCorner switch
        {
            0 => new Vector2Int(LABY_WIDTH - 1, LABY_HEIGHT - 1),
            1 => new Vector2Int(0, LABY_HEIGHT - 1),
            2 => new Vector2Int(LABY_WIDTH - 1, 0),
            3 => new Vector2Int(0, 0),
            _ => new Vector2Int(0, 0),
        };
    }

    private struct Voisin
    {
        public Vector2Int pos;
        public LabyCell murPartage;
    }

    private void GenerateMaze(int width, int height)
    {
        // Initialisation du labyrinthe avec tous les murs
        laby = new LabyCell[width, height];
        LabyCell initialState = LabyCell.LEFT_WALL | LabyCell.RIGHT_WALL | LabyCell.TOP_WALL | LabyCell.BOTTOM_WALL;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                laby[i, j] = initialState;
            }
        }

        // Départ du backtracking
        Stack<Vector2Int> pile = new();
        Vector2Int positionInitiale = new(0, 0);
        pile.Push(positionInitiale);
        laby[0, 0] |= LabyCell.VISITED;

        while (pile.Count > 0)
        {
            Vector2Int current = pile.Peek();
            List<Voisin> voisins = TrouverVoisins(current, width, height);

            if (voisins.Count > 0)
            {
                Voisin voisinChoisi = voisins[Random.Range(0, voisins.Count)];

                // Supprimer le mur entre les deux cellules
                laby[current.x, current.y] &= ~voisinChoisi.murPartage;
                laby[voisinChoisi.pos.x, voisinChoisi.pos.y] &= ~MurOppose(voisinChoisi.murPartage);

                // Marquer la cellule voisine comme visitée
                laby[voisinChoisi.pos.x, voisinChoisi.pos.y] |= LabyCell.VISITED;

                // Ajouter la cellule voisine à la pile
                pile.Push(voisinChoisi.pos);
            }
            else
            {
                // Retour en arrière
                pile.Pop();
            }
        }
    }

    private List<Voisin> TrouverVoisins(Vector2Int cell, int width, int height)
    {
        List<Voisin> voisins = new List<Voisin>();

        if (cell.x > 0 && !CelluleVisitee(cell.x - 1, cell.y)) // Gauche
        {
            voisins.Add(new Voisin
            {
                pos = new Vector2Int(cell.x - 1, cell.y),
                murPartage = LabyCell.LEFT_WALL
            });
        }
        if (cell.x < width - 1 && !CelluleVisitee(cell.x + 1, cell.y)) // Droite
        {
            voisins.Add(new Voisin
            {
                pos = new Vector2Int(cell.x + 1, cell.y),
                murPartage = LabyCell.RIGHT_WALL
            });
        }
        if (cell.y > 0 && !CelluleVisitee(cell.x, cell.y - 1)) // Bas
        {
            voisins.Add(new Voisin
            {
                pos = new Vector2Int(cell.x, cell.y - 1),
                murPartage = LabyCell.BOTTOM_WALL
            });
        }
        if (cell.y < height - 1 && !CelluleVisitee(cell.x, cell.y + 1)) // Haut
        {
            voisins.Add(new Voisin
            {
                pos = new Vector2Int(cell.x, cell.y + 1),
                murPartage = LabyCell.TOP_WALL
            });
        }

        return voisins;
    }

    private bool CelluleVisitee(int x, int y)
    {
        return (laby[x, y] & LabyCell.VISITED) != 0;
    }

    private LabyCell MurOppose(LabyCell mur)
    {
        return mur switch
        {
            LabyCell.LEFT_WALL => LabyCell.RIGHT_WALL,
            LabyCell.RIGHT_WALL => LabyCell.LEFT_WALL,
            LabyCell.TOP_WALL => LabyCell.BOTTOM_WALL,
            LabyCell.BOTTOM_WALL => LabyCell.TOP_WALL,
            _ => 0,
        };
    }

    public LabyCell[,] GetLaby()
    {
        return laby;
    }

    public Vector2Int GetLabySize()
    {
        return new Vector2Int(LABY_WIDTH, LABY_HEIGHT);
    }

    public Vector2Int GetStartPos()
    {
        return startPos;
    }

    public Vector2Int GetEndPos()
    {
        return endPos;
    }
}
