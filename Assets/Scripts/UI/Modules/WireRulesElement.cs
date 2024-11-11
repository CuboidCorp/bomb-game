using UnityEngine.UIElements;

[UxmlElement]
public partial class WireRulesElement : VisualElement
{
    private VisualElement rulesHolder => this.Q("wireModule");
    private Label description => this.Q<Label>("description");

    public void Init(WireRuleGenerator generator)
    {
        description.text.Replace("{nb_wires_min}", generator.GetNbWiresMin().ToString());
        description.text.Replace("{nb_wires_max}", generator.GetNbWiresMax().ToString());

        int nbRules = generator.GetNbWiresMax() - generator.GetNbWiresMin();

        for (int i = 0; i < nbRules; i++)
        {
            Foldout rule = new()
            {
                text = $"Dans le cas de {i + 1} fils"
            };
            rule.AddToClassList("wireFoldout");
            WireRule[] rules = generator.GetRulesFromNbWire(i + generator.GetNbWiresMin());
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
