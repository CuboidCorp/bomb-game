using UnityEngine.UIElements;

[UxmlElement]
public partial class ButtonRulesElement : VisualElement
{
    private VisualElement GridHolder => this.Q("labyHolder");

    private VisualElement gridPlaced;

    public void Init(ButtonRuleGenerator generator, VisualTreeAsset gridElem)
    {
        //Premiere ligne on fait les mots localisés
        for (int i = 0; i <= ButtonRuleGenerator.NB_RULES; i++)
        {
            if (i == 0)
            {
                gridPlaced = gridElem.CloneTree();
                gridPlaced.Q<Label>("word").text = " ";

            }
            else
            {
                gridPlaced = gridElem.CloneTree();
                //TODO : Trouver comment recup vraiment les mots localisés

            }
        }
    }

    public ButtonRulesElement() { }

}
