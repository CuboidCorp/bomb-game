using UnityEngine.Localization;
using UnityEngine.UIElements;

[UxmlElement]
public partial class WireRulesElement : VisualElement
{
    private Label description => this.Q<Label>("description");
    private VisualElement rulesHolder => this.Q("wireController");

    public void Init(WireRuleGenerator generator)
    {
        int nbWiresMin = generator.GetNbWiresMin();
        int nbWiresMax = generator.GetNbWiresMax();

        LocalizedString smartString = TextLocalizationHandler.GetSmartString("TexteManuel", "WIRE_RULE_DESC");
        smartString.Arguments = new object[] { nbWiresMin, nbWiresMax };
        description.text = smartString.GetLocalizedString();

        int nbRules = nbWiresMax - nbWiresMin;

        for (int i = 0; i <= nbRules; i++)
        {
            LocalizedString smartFoldout = TextLocalizationHandler.GetSmartString("TexteManuel", "WIRE_RULE_FOLDOUT");
            smartFoldout.Arguments = new object[] { nbWiresMin + i };

            Foldout rule = new() //TODO : Remplacer pas de texte sans localisation
            {
                text = smartFoldout.GetLocalizedString(),
            };
            rule.AddToClassList("wireFoldout");
            WireRule[] rules = generator.GetRulesFromNbWire(i + nbWiresMin);
            for (int y = 0; y < rules.Length; y++)
            {
                Label label = new();
                if (y > 0)
                {
                    label.text = TextLocalizationHandler.LoadString("TexteManuel", "ELSE") + rules[y].GetRuleString().ToLower();
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
