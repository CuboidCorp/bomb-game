using UnityEngine.UIElements;

[UxmlElement]
public partial class WireRulesElement : VisualElement
{
    private VisualElement rulesHolder => this.Q("wireController");
    private Label title => this.Q<Label>("titre");
    private Label description => this.Q<Label>("description");

    public void Init(WireRuleGenerator generator)
    {
        int nbWiresMin = generator.GetNbWiresMin();
        int nbWiresMax = generator.GetNbWiresMax();

        title.text = TextFR.WIRE_RULE_TITLE;
        description.text = TextFR.WIRE_RULE_DESC.Replace("{nb_wires_min}", nbWiresMin.ToString()).Replace("{nb_wires_max}", nbWiresMax.ToString());

        int nbRules = nbWiresMax - nbWiresMin;

        for (int i = 0; i <= nbRules; i++)
        {
            Foldout rule = new()
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
