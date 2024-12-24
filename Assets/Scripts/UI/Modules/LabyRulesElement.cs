using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

[UxmlElement]
public partial class LabyRulesElement : VisualElement
{
    private Label labyRulesDesc => this.Q<Label>("desc");
    private VisualElement LabyImageHolder => this.Q("labyHolder");

    private int mapSizeSide;

    public void Init(LabyRuleGenerator generator, VisualTreeAsset labyTemplate)
    {
        Vector2Int mapSize = generator.GetLabySize();
        mapSizeSide = mapSize.x;

        LocalizedString smartString = TextLocalizationHandler.GetSmartString("TexteManuel", "LABY_RULE_DESC");
        smartString.Arguments = new object[] { mapSize.x, mapSize.y };
        labyRulesDesc.text = smartString.GetLocalizedString();

        for (int i = 0; i < LabyRuleGenerator.NB_RULES; i++)
        {
            VisualElement labyRuleVisu = labyTemplate.CloneTree();
            labyRuleVisu.Q<LabyVisuElement>().Init(generator.GetRule(), i);
            LabyImageHolder.Add(labyRuleVisu);
        }
    }

    public LabyRulesElement() { }

}
