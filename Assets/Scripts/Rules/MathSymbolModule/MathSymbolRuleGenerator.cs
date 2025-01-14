using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class MathSymbolRuleGenerator : IRuleGenerator
{
    private const int NB_RULES = 4;

    private MathSymbolRule[] rules;
    private int currentRuleIndex = 0;

    private readonly int[] buttonValues = new int[] { 2, 5, 40, 200, 500, 12, 98, 150 };

    private List<MathSymbolRule> rulesList;

    public void SetupRules()
    {
        rulesList = new List<MathSymbolRule>();
        rules = new MathSymbolRule[NB_RULES];

        for (int i = 0; i < NB_RULES; i++)
        {
            rulesList.Add(GenerateMathSymbolRule());
        }

        Functions.Shuffle(rulesList);
        rules = rulesList.ToArray();

    }

    private MathSymbolRule GenerateMathSymbolRule()
    {
        MathSymbolRule rule = new()
        {
            targetNumber = Random.Range(400, 999)
        };
        Dictionary<Symbols, int> valeursBtn = new();

        int nbSymbolsChosen = 0;
        while (nbSymbolsChosen < 8)
        {
            Symbols randSymbol = (Symbols)Random.Range(0, Enum.GetValues(typeof(Symbols)).Length);
            if (valeursBtn.ContainsKey(randSymbol))
            {
                continue;
            }
            valeursBtn[randSymbol] = buttonValues[nbSymbolsChosen];
            nbSymbolsChosen++;
        }
        rule.valeursBtn = valeursBtn;

        return rule;
    }


    public MathSymbolRule GetRule()
    {
        currentRuleIndex++;
        if (currentRuleIndex > NB_RULES)
        {
            currentRuleIndex = 1;
        }
        return rules[currentRuleIndex - 1];
    }
}
