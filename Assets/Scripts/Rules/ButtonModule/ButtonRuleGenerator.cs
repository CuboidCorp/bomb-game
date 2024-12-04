using UnityEngine;

public class ButtonRuleGenerator
{
    private const int NB_RULES = 6;
    private Material[] materials;

    private LocalizedString[] words;

    private ButtonRule[] rules;
    private int currentRuleIndex = 0;
    
    private static Vector2Int[] timerBounds = new Vector2Int[]
    {
        new Vector2Int(0, 15),
        new Vector2Int(15, 30),
        new Vector2Int(30, 45),
        new Vector2Int(45, 60)
    };

    public void SetupRules()
    {
        rules = new ButtonRule[NB_RULES];

        //TODO : Trouver comment récupérer les matériaux et les mots


        for (int i = 0; i < NB_RULES; i++)
        {
            rules[i] = GenerateRule(i);
        }
    }

    private ButtonRule GenerateRule(int index)
    {
        ButtonRule rule = new();
        rule.buttonMaterial = materials[index];
        rule.word = words[index];
        rule.condition = (ButtonCondition)Random.Range(0, Enum.GetValues(typeof(ButtonCondition)).Length);
        switch (rule.condition)
        {
            case ButtonCondition.PRESS_FOR:
                rule.targetPressTime = Random.Range(1, 5);
                break;
            case ButtonCondition.PRESS_UNTIL_TIMER_CONTAINS:
                rule.targetTimerNumber = Random.Range(0, 10);
                break;
            case ButtonCondition.PRESS_UNTIL_TIMER_BETWEEN:
                rule.targetTimerBetweenBounds = timerBounds[Random.Range(0, timerBounds.Length)];
                break;
        }
        return rule;
    }

    public ButtonRule GetRule()
    {
        currentRuleIndex++;
        if (currentRuleIndex > NB_RULES)
        {
            currentRuleIndex = 1;
        }
        return rules[currentRuleIndex - 1];
    }
}