using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class LabyRulesElement : VisualElement
{
    private VisualElement RulesHolder => this.Q("labyController");
    private Label Title => this.Q<Label>("titre");
    private Label Description => this.Q<Label>("description");

    public void Init(LabyRuleGenerator generator, VisualTreeAsset labyTemplate)
    {
        Vector2Int mapSize = generator.GetLabySize();

        Title.text = TextFR.LABY_RULE_TITLE;
        Description.text = TextFR.LABY_RULE_DESC.Replace("{nb_laby}", mapSize.x.ToString());

        for (int i = 0; i < LabyRuleGenerator.NB_RULES; i++)
        {
            VisualElement labyRuleVisu = labyTemplate.CloneTree();
            labyRuleVisu.Q<LabyVisuElement>().Init(generator.GetRule());
            RulesHolder.Add(labyRuleVisu);
        }
    }

    public LabyRulesElement() { }

}
