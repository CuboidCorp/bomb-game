using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ButtonRuleGenerator : IRuleGenerator
{
    public const int NB_RULES = 6;
    public Material[] materials;

    public static readonly string[] wordKeys = new string[]
    {
        "BUTTON_MOD_TEXT1",
        "BUTTON_MOD_TEXT2",
        "BUTTON_MOD_TEXT3",
        "BUTTON_MOD_TEXT4",
        "BUTTON_MOD_TEXT5",
        "BUTTON_MOD_TEXT6"
    };

    private List<ButtonRule> rules;
    private int currentRuleIndex = 0;

    private Dictionary<string, ButtonRule> rulesDict;

    private static readonly Vector2Int[] timerBounds = new Vector2Int[]
    {
        new(0, 15),
        new(15, 30),
        new(30, 45),
        new(45, 60)
    };

    public void SetupRules()
    {
        rules = new();

        rulesDict = new();

        materials = Resources.LoadAll<Material>("Materials/Button");


        for (int i = 0; i < NB_RULES; i++)
        {
            rules.Add(GenerateRule(i, i)); //TODO : Pour changer randomiser le materiel et le texte en verifiant qu'ils ne sont pas deja pris
        }



        Functions.Shuffle(rules);
    }

    private ButtonRule GenerateRule(int matIndex, int wordIndex)
    {
        ButtonRule rule = new()
        {
            buttonMaterial = materials[matIndex],
            wordKey = wordKeys[wordIndex],
            condition = (ButtonCondition)Random.Range(0, Enum.GetValues(typeof(ButtonCondition)).Length)
        };

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

        rulesDict.Add($"{matIndex}:{wordIndex}", rule);
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

    public ButtonRule GetFakeRule(int materialIndex, int wordIndex)
    {
        ButtonRule rule = new()
        {
            buttonMaterial = materials[materialIndex],
            wordKey = wordKeys[wordIndex],
            condition = (ButtonCondition)Random.Range(0, Enum.GetValues(typeof(ButtonCondition)).Length)
        };

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

    public Dictionary<string, ButtonRule> GetRulesDict()
    {
        return rulesDict;
    }
}