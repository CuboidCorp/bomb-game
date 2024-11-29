using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

[UxmlElement]
public partial class LabyVisuElement : VisualElement
{
    private VisualElement LabyHolder => this.Q("labyHolder");

    private Label LabyName => this.Q<Label>("labyName");

    private WallState[,] laby;
    private Sprite[] labyImages;

    private const int CellSize = 64;
    private const int CellMargin = 2;

    public void Init(LabyRule rule, int index)
    {
        LabyName.text = "Laby " + (index + 1);

        laby = rule.GetLaby();
        labyImages = Resources.LoadAll<Sprite>("Textures/LabyImages");
        foreach (Sprite sprite in labyImages)
        {
            Debug.Log(sprite.name);
        }
        Vector2Int labySize = rule.GetLabySize();

        int totalWidth = labySize.x * (CellSize + CellMargin);
        int totalHeight = labySize.y * (CellSize + CellMargin);

        LabyHolder.style.width = totalWidth;
        LabyHolder.style.height = totalHeight;

        Vector2Int startPos = rule.GetStartPos();
        Vector2Int endPos = rule.GetEndPos();

        for (int y = labySize.y - 1; y >= 0; y--)
        {
            for (int x = 0; x < labySize.x; x++)
            {
                VisualElement cell = new();
                cell.AddToClassList("laby-grid-item");

                cell.style.width = CellSize;
                cell.style.height = CellSize;
                cell.style.marginRight = CellMargin;
                cell.style.marginBottom = CellMargin;

                Sprite sprite = GetSpriteFromPos(new Vector2Int(x, y));
                cell.style.backgroundImage = sprite != null ? sprite.texture : null;

                Vector2Int currentPos = new(x, y);
                if (currentPos == startPos)
                {
                    cell.style.backgroundColor = Color.green;
                }
                else if (currentPos == endPos)
                {
                    cell.style.backgroundColor = Color.blue;
                }

                LabyHolder.Add(cell);
            }
        }
    }

    private Sprite GetSpriteFromPos(Vector2Int pos)
    {
        int index = (int)(laby[pos.x, pos.y] & ~WallState.VISITED);
        return index > 0 ? labyImages[index - 1] : null;
    }

    public LabyVisuElement() { }
}
