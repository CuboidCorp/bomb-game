using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class MathSymbolRulesElement : VisualElement
{
    private VisualElement TabSymbols => this.Q<VisualElement>("tabSymbols");

    public void Init(MathSymbolRuleGenerator generator)
    {
        Sprite[] symbolsSprites = generator.GetSymbolsSprites();
        Symbols[,] symbolsRep = generator.GetSymbolsRepartition();
        int[] buttonValues = generator.GetButtonValues();

        for (int i = 0; i < 8; i++)
        {
            Label numHolder = TabSymbols.Q<Label>($"num{i + 1}");
            VisualElement symbolHolder = TabSymbols.Q<VisualElement>($"symbols{i + 1}");

            numHolder.text = buttonValues[i].ToString();
            for (int y = 0; y < 5; y++)
            {
                VisualElement symbol = new();
                symbol.style.backgroundImage = symbolsSprites[(int)symbolsRep[i, y]].texture;
                symbol.style.width = 64;
                symbol.style.height = 64;
                symbolHolder.Add(symbol);

            }
        }
    }

    public MathSymbolRulesElement() { }

}
