using System.Collections.Generic;
using UnityEngine.UIElements;

[UxmlElement]
public partial class MorseRulesElement : VisualElement
{
    private VisualElement GroupeInfoHolder => this.Q("groupeInfoHolder");

    public void Init(MorseRuleGenerator ruleGen, VisualTreeAsset morseGroupeImage)
    {
        //On doit recup les groupes du generator
        //On recup les regles et on les associe aux groupes
        int nbRules = MorseRuleGenerator.NB_RULES;

        for (int i = 0; i < nbRules; i++)
        {
            MorseRule rule = ruleGen.GetRule();
            VisualElement groupeVisu = morseGroupeImage.CloneTree();
            groupeVisu.Q<MorseVisuElement>().Init(rule.targetGroup, rule.correctImage);
            GroupeInfoHolder.Add(groupeVisu);
        }
    }
}
