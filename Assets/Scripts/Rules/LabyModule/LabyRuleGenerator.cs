using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LabyRuleGenerator
{
    private struct Neighbour
    {
        public Vector2Int Pos;
        public WallState SharedWall;
    }

    private const int LABY_HEIGHT = 6;
    private const int LABY_WIDTH = 6;

    private const int NB_RULES = 4;


    private LabyRule[] labyRules;
    private LabyRule currentRule;

    public void SetupRules()
    {
        for (int i = 0; i < NB_RULES; i++)
        {
            Vector2Int startPos = i switch
            {
                0 => new Vector2Int(0, 0),
                1 => new Vector2Int(LABY_WIDTH - 1, 0),
                2 => new Vector2Int(0, LABY_HEIGHT - 1),
                3 => new Vector2Int(LABY_WIDTH - 1, LABY_HEIGHT - 1),
                _ => new Vector2Int(0, 0),
            };

            Vector2Int endPos = i switch
            {
                0 => new Vector2Int(LABY_WIDTH - 1, LABY_HEIGHT - 1),
                1 => new Vector2Int(0, LABY_HEIGHT - 1),
                2 => new Vector2Int(LABY_WIDTH - 1, 0),
                3 => new Vector2Int(0, 0),
                _ => new Vector2Int(0, 0),
            };

            currentRule.labyStart = startPos;
            currentRule.labyEnd = endPos;
            currentRule.labySize = new Vector2Int(LABY_WIDTH, LABY_HEIGHT);
            GenerateMaze(currentRule, LABY_WIDTH, LABY_HEIGHT);

        }
    }

    private struct Voisin
    {
        public Vector2Int pos;
        public WallState murPartage;
    }

    private static void GenerateMaze(LabyRule rule, int width, int height)
    {
        // Initialisation du labyrinthe avec tous les murs
        rule.laby = new WallState[width, height];
        WallState initialState = WallState.LEFT_WALL | WallState.RIGHT_WALL | WallState.TOP_WALL | WallState.BOTTOM_WALL;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                rule.laby[i, j] = initialState;
            }
        }

        bool aEuDesVoisins = true;
        Stack<Vector2Int> stack = new();
        Vector2Int currentCell = new(Random.Range(0, width), Random.Range(0, height));
        rule.laby[currentCell.x, currentCell.y] |= WallState.VISITED;
        stack.Push(currentCell);

        while (stack.Count > 0)
        {
            Vector2Int posActuelle = stack.Pop();
            Neighbour[] voisins = GetUnvisitedNeighbor(rule, posActuelle);
            if (voisins.Length > 0)
            {
                aEuDesVoisins = true;
                stack.Push(posActuelle);

                int randIndex = Random.Range(0, voisins.Length);
                Neighbour randomNeighbour = voisins[randIndex];
                Vector2Int posNeighbour = randomNeighbour.Pos;

                rule.laby[posActuelle.x, posActuelle.y] &= ~randomNeighbour.SharedWall;
                rule.laby[posNeighbour.x, posNeighbour.y] &= ~GetOppositeWall(randomNeighbour.SharedWall);
                rule.laby[posNeighbour.x, posNeighbour.y] |= WallState.VISITED;

                stack.Push(posNeighbour);
            }
            else if (aEuDesVoisins)
            {
                aEuDesVoisins = false;
            }
        }
    }



    private static WallState GetOppositeWall(WallState wall)
    {
        return wall switch
        {
            WallState.LEFT_WALL => WallState.RIGHT_WALL,
            WallState.RIGHT_WALL => WallState.LEFT_WALL,
            WallState.TOP_WALL => WallState.BOTTOM_WALL,
            WallState.BOTTOM_WALL => WallState.TOP_WALL,
            _ => WallState.LEFT_WALL,
        };
    }

    private static Neighbour[] GetUnvisitedNeighbor(LabyRule rule, Vector2Int pos)
    {
        List<Neighbour> unvisited = new();

        if (pos.x > 0 && !rule.laby[pos.x - 1, pos.y].HasFlag(WallState.VISITED))
        {
            unvisited.Add(new Neighbour { Pos = new Vector2Int(pos.x - 1, pos.y), SharedWall = WallState.LEFT_WALL });
        }
        if (pos.x < LABY_WIDTH - 1 && !rule.laby[pos.x + 1, pos.y].HasFlag(WallState.VISITED))
        {
            unvisited.Add(new Neighbour { Pos = new Vector2Int(pos.x + 1, pos.y), SharedWall = WallState.RIGHT_WALL });
        }
        if (pos.y > 0 && !rule.laby[pos.x, pos.y - 1].HasFlag(WallState.VISITED))
        {
            unvisited.Add(new Neighbour { Pos = new Vector2Int(pos.x, pos.y - 1), SharedWall = WallState.BOTTOM_WALL });
        }
        if (pos.y < LABY_HEIGHT - 1 && !rule.laby[pos.x, pos.y + 1].HasFlag(WallState.VISITED))
        {
            unvisited.Add(new Neighbour { Pos = new Vector2Int(pos.x, pos.y + 1), SharedWall = WallState.TOP_WALL });
        }
        return unvisited.ToArray();
    }


}
