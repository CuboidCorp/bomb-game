using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class LabyRulesElement : VisualElement
{
    private VisualElement LabyImageHolder => this.Q("labyHolder");

    private int mapSizeSide;

    public void Init(LabyRuleGenerator generator, VisualTreeAsset labyTemplate)
    {
        Vector2Int mapSize = generator.GetLabySize();
        mapSizeSide = mapSize.x;

        for (int i = 0; i < LabyRuleGenerator.NB_RULES; i++)
        {
            VisualElement labyRuleVisu = labyTemplate.CloneTree();
            labyRuleVisu.Q<LabyVisuElement>().Init(generator.GetRule(), i);
            LabyImageHolder.Add(labyRuleVisu);
        }
    }

    public LabyRulesElement() { }

}
