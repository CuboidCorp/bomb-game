using UnityEngine.UIElements;

[UxmlElement]
public partial class WireRulesElement : VisualElement
{
    private VisualElement rulesHolder => this.Q("wireController");

    public void Init(WireRuleGenerator generator)
    {
        int nbWiresMin = generator.GetNbWiresMin();
        int nbWiresMax = generator.GetNbWiresMax();

        int nbRules = nbWiresMax - nbWiresMin;

        for (int i = 0; i <= nbRules; i++)
        {
            Foldout rule = new() //TODO : Remplacer pas de texte sans localisation
            {
                text = $"Dans le cas de {nbWiresMin + i} fils"
            };
            rule.AddToClassList("wireFoldout");
            WireRule[] rules = generator.GetRulesFromNbWire(i + nbWiresMin);
            for (int y = 0; y < rules.Length; y++)
            {
                Label label = new();
                if (y > 0)
                {
                    label.text = "Sinon " + rules[y].GetRuleString();
                }
                else
                {
                    label.text = rules[y].GetRuleString();
                }
                rule.Add(label);
            }

            rulesHolder.Add(rule);
        }

    }

    public WireRulesElement() { }

}
