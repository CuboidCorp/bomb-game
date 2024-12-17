using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class MorseVisuElement : VisualElement
{
    private Label MorseGroupLabel => this.Q<Label>("morseGroupLabel");

    private VisualElement GridHolder => this.Q("gridHolder");

    private Sprite offSprite;
    private Sprite onSprite;

    public void Init(char[] groupe, bool[,] image)
    {
        offSprite = Resources.Load<Sprite>("Textures/MorseModule/OffSprite");
        onSprite = Resources.Load<Sprite>("Textures/MorseModule/OnSprite");

        MorseGroupLabel.text = "[";

        for (int i = 0; i < groupe.Length - 1; i++)
        {
            MorseGroupLabel.text += groupe[i] + ",";
        }
        MorseGroupLabel.text += $"{groupe[groupe.Length - 1]}]";

        for (int y = 0; y < 6; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                VisualElement cell = new();
                //cell.AddToClassList("morse-grid-item");

                cell.style.width = 64;
                cell.style.height = 64;
                cell.style.marginRight = 2;
                cell.style.marginBottom = 2;

                cell.style.backgroundImage = image[x, y] ? onSprite.texture : offSprite.texture;

                GridHolder.Add(cell);
            }
        }
    }
}
