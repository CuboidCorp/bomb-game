using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class LabyVisuElement : VisualElement
{
    private VisualElement labyVisu => this.Q("labyVisuController");

    private WallState[,] laby;
    private Sprite[] labyImages;

    private const int CellSize = 64;
    private const int CellMargin = 2;

    public void Init(LabyRule rule)
    {
        // Charger les données du labyrinthe
        laby = rule.GetLaby();
        labyImages = Resources.LoadAll<Sprite>("Textures/LabyImages");
        Vector2Int labySize = rule.GetLabySize();

        // Calculer la taille totale du conteneur
        int totalWidth = labySize.x * (CellSize + CellMargin);
        int totalHeight = labySize.y * (CellSize + CellMargin);

        labyVisu.style.width = totalWidth;
        labyVisu.style.height = totalHeight;
        labyVisu.style.flexDirection = FlexDirection.Row;
        labyVisu.style.flexWrap = Wrap.Wrap;

        // Récupérer les positions de départ et de fin
        Vector2Int startPos = rule.GetStartPos();
        Vector2Int endPos = rule.GetEndPos();

        // Créer une cellule pour chaque case du labyrinthe
        for (int y = 0; y < labySize.y; y++)
        {
            for (int x = 0; x < labySize.x; x++)
            {
                var cell = new VisualElement();
                cell.AddToClassList("laby-grid-item");

                // Définir la taille des cellules
                cell.style.width = CellSize;
                cell.style.height = CellSize;
                cell.style.marginRight = CellMargin;
                cell.style.marginBottom = CellMargin;

                // Obtenir l'image correspondante pour la cellule
                Sprite sprite = GetSpriteFromPos(new Vector2Int(x, y));
                cell.style.backgroundImage = sprite != null ? sprite.texture : null;

                // Vérifier si cette cellule est le départ ou la fin
                Vector2Int currentPos = new Vector2Int(x, y);
                if (currentPos == startPos)
                {
                    var startLabel = new Label("D");
                    startLabel.AddToClassList("laby-label");
                    cell.Add(startLabel);
                }
                else if (currentPos == endPos)
                {
                    var endLabel = new Label("F");
                    endLabel.AddToClassList("laby-label");
                    cell.Add(endLabel);
                }

                labyVisu.Add(cell);
            }
        }
    }

    private Sprite GetSpriteFromPos(Vector2Int pos)
    {
        // Détermine l'index de l'image à partir de l'état de la cellule
        int index = (int)(laby[pos.x, pos.y] & ~WallState.VISITED);
        return index > 0 ? labyImages[index - 1] : null;
    }

    public LabyVisuElement() { }
}
