using System.Collections.Generic;
using UnityEngine.Localization;
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
                gridPlaced.Q<Label>("word").text = TextLocalizationHandler.LoadString("TexteManuel", ButtonRuleGenerator.wordKeys[i - 1]);
            }
            GridHolder.Add(gridPlaced);
        }

        Dictionary<string, ButtonRule> ruleDict = generator.GetRulesDict();

        //Maintenant on fait le tableau avec a gauche la liste des couleurs
        for (int i = 0; i < ButtonRuleGenerator.NB_RULES; i++)
        {
            gridPlaced = gridElem.CloneTree();
            gridPlaced.Q<Label>("word").text = TextLocalizationHandler.LoadString("TexteManuel", $"COLORS{i + 1}");
            GridHolder.Add(gridPlaced);

            for (int j = 0; j < ButtonRuleGenerator.NB_RULES; j++)
            {
                gridPlaced = gridElem.CloneTree();
                ButtonRule rule;
                if (ruleDict.ContainsKey($"{i}:{j}"))
                {
                    rule = ruleDict[$"{i}:{j}"];
                }
                else
                {
                    rule = generator.GetFakeRule(i, j);
                }
                LocalizedString localizedString = TextLocalizationHandler.GetSmartString("TexteManuel", $"BUTTON_RULE_INSTRUCTION{(int)rule.condition}");
                switch (rule.condition)
                {
                    case ButtonCondition.IMMEDIATE: //C'est pas un smart string donc pas de 
                        break;
                    case ButtonCondition.PRESS_FOR:
                        localizedString.Arguments = new object[] { rule.targetPressTime };
                        break;
                    case ButtonCondition.PRESS_UNTIL_TIMER_CONTAINS:
                        localizedString.Arguments = new object[] { rule.targetTimerNumber };
                        break;
                    case ButtonCondition.PRESS_UNTIL_TIMER_BETWEEN:
                        localizedString.Arguments = new object[] { rule.targetTimerBetweenBounds.x, rule.targetTimerBetweenBounds.y };
                        break;
                }

                gridPlaced.Q<Label>("word").text = localizedString.GetLocalizedString();
                GridHolder.Add(gridPlaced);
            }
        }


    }

    public ButtonRulesElement() { }

}
