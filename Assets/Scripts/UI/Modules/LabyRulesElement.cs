using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class LabyRulesElement : VisualElement
{
    private VisualElement rulesHolder => this.Q("labyController");
    private Label title => this.Q<Label>("titre");
    private Label description => this.Q<Label>("description");

    public void Init(LabyRuleGenerator generator)
    {
        Vector2Int mapSize = generator.GetLabySize();

        title.text = TextFR.LABY_RULE_TITLE;
        description.text = TextFR.LABY_RULE_DESC.Replace("{nb_laby}", mapSize.x.ToString());

    }

    public LabyRulesElement() { }

}
