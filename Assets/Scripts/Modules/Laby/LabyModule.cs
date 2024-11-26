using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabyModule : Module
{
    private Vector2Int labySize;
    private Vector2Int labyStart;
    private Vector2Int labyEnd;

    private Vector2Int currentPos;
    private WallState[,] laby;

    private WallState allValues = WallState.LEFT_WALL | WallState.RIGHT_WALL | WallState.TOP_WALL | WallState.BOTTOM_WALL | WallState.VISITED;

    [SerializeField] private float blinkInterval = 1f;
    private Coroutine blinkCoroutine;

    [SerializeField] private Transform canvaHolder;
    [SerializeField] List<Texture> imagesLaby;

    public override void SetupModule(RuleHolder rules)
    {
        laby = rules.labyRuleGenerator.GetLaby();
        labySize = rules.labyRuleGenerator.GetLabySize();
        labyStart = rules.labyRuleGenerator.GetStartPos();
        labyEnd = rules.labyRuleGenerator.GetEndPos();
        currentPos = labyStart;
        RenderLaby();
        blinkCoroutine = StartCoroutine(BlinkCoroutine());
    }

    private void OnDisable()
    {
        StopCoroutine(blinkCoroutine);
    }

    public override void ModuleInteract(Ray rayInteract)
    {
        if (Physics.Raycast(rayInteract, out RaycastHit hit, 10))
        {
            if (hit.collider.gameObject.name.StartsWith("Triangle"))
            {
                Debug.Log(hit.collider.gameObject.name);
                Deplacement(hit.collider.gameObject.name);
            }
        }
        GetComponent<Collider>().enabled = true;
    }

    private void RenderLaby()
    {
        for (int y = 0; y < labySize.y; y++)
        {
            for (int x = 0; x < labySize.x; x++)
            {
                int childIndex = y * labySize.x + x;
                Transform imageHolder = canvaHolder.GetChild(childIndex);
                RawImage img = imageHolder.GetComponent<RawImage>();
                int index = GetIndexCouloir(new Vector2Int(x, y));
                Debug.Log((laby[x, y] & ~WallState.VISITED) + " x :" + x + " y:" + y + " nb : " + index);
                if (index == 0)
                {
                    img.texture = null;
                }
                else
                {
                    img.texture = imagesLaby[index];
                }
            }
        }
        //On change la couleur des cases du début et de la fin
        SetBaseColor(GetCellPos(labyStart), labyStart);
        SetBaseColor(GetCellPos(labyEnd), labyEnd);
    }

    /// <summary>
    /// Renvoie l'index d'un couloir en fonction de son voisin dans la grille
    /// </summary>
    /// <returns>L'index du couloir dans la liste des couloirs</returns>
    private int GetIndexCouloir(Vector2Int position)
    {
        int allFlagsMask = (int)(WallState.BOTTOM_WALL | WallState.TOP_WALL | WallState.LEFT_WALL | WallState.RIGHT_WALL | WallState.VISITED);
        return ~(int)laby[position.x, position.y] & allFlagsMask;
    }

    private RawImage GetCellPos(Vector2Int pos)
    {
        int childIndex = pos.y * labySize.x + pos.x;
        Transform imageHolder = canvaHolder.GetChild(childIndex);
        return imageHolder.GetComponent<RawImage>();
    }

    private void SetBaseColor(RawImage img, Vector2Int pos)
    {
        if (pos == labyStart)
        {
            img.color = Color.green;
        }
        else if (pos == labyEnd)
        {
            img.color = Color.blue;
        }
        else
        {
            img.color = Color.white;
        }
    }

    private void Deplacement(string nomFleche)
    {
        WallState wallState;
        Vector2Int direction;
        switch (nomFleche)
        {
            case "TriangleU":
                wallState = WallState.TOP_WALL;
                direction = Vector2Int.up;
                break;
            case "TriangleD":
                wallState = WallState.BOTTOM_WALL;
                direction = Vector2Int.down;
                break;
            case "TriangleL":
                wallState = WallState.LEFT_WALL;
                direction = Vector2Int.left;
                break;
            case "TriangleR":
                wallState = WallState.RIGHT_WALL;
                direction = Vector2Int.right;
                break;
            default:
                throw new System.Exception("Nom de flèche invalide");
        }

        //On vérifie si on est en dehors du labyrinthe
        if (currentPos.x + direction.x < 0 || currentPos.x + direction.x >= labySize.x || currentPos.y + direction.y < 0 || currentPos.y + direction.y >= labySize.y)
        {
            ModuleFail.Invoke();
            return;
        }

        //On vérifie si ça nous fait rentrer dans un mur
        //Si le mur est présent Fail
        if ((laby[currentPos.x, currentPos.y] & wallState) != 0)
        {
            ModuleFail.Invoke();
            return;
        }

        currentPos += direction;
        if (currentPos == labyEnd)
        {
            ModuleSuccess.Invoke();
        }

    }

    private IEnumerator BlinkCoroutine()
    {
        while (true)
        {
            RawImage img = GetCellPos(currentPos);
            img.color = Color.yellow;
            yield return new WaitForSeconds(blinkInterval);
            SetBaseColor(img, currentPos);
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
