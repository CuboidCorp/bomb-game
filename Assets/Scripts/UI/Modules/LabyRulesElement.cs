using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

[UxmlElement]
public partial class LabyRulesElement : VisualElement
{
    private Label LabyRulesDesc => this.Q<Label>("desc");
    private VisualElement LabyImageHolder => this.Q("labyHolder");

    public void Init(LabyRuleGenerator generator, VisualTreeAsset labyTemplate)
    {
        Vector2Int mapSize = generator.GetLabySize();

        LocalizedString smartString = TextLocalizationHandler.GetSmartString("TexteManuel", "LABY_RULE_DESC");
        smartString.Arguments = new object[] { mapSize.x, mapSize.y };
        LabyRulesDesc.text = smartString.GetLocalizedString();

        for (int i = 0; i < LabyRuleGenerator.NB_RULES; i++)
        {
            VisualElement labyRuleVisu = labyTemplate.CloneTree();
            labyRuleVisu.Q<LabyVisuElement>().Init(generator.GetRule(), i);
            LabyImageHolder.Add(labyRuleVisu);
        }
    }

    public LabyRulesElement() { }

}
