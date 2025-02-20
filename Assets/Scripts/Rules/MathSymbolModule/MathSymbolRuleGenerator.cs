using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MathSymbolRuleGenerator : IRuleGenerator
{
    private const int NB_RULES = 4;

    private MathSymbolRule[] rules;
    private int currentRuleIndex = 0;

    private readonly int[] buttonValues = new int[] { 2, 5, 40, 200, 500, 12, 98, 150 };

    private List<MathSymbolRule> rulesList;

    private Sprite[] symbolsSprites;
    private Symbols[,] symbolsRepartition;

    public void SetupRules()
    {
        rulesList = new List<MathSymbolRule>();
        rules = new MathSymbolRule[NB_RULES];

        //On répartit les 40 symboles dans 8 groupes différents 
        List<Symbols> symbols = Enum.GetValues(typeof(Symbols)).Cast<Symbols>().ToList();
        Functions.Shuffle(symbols);

        symbolsRepartition = new Symbols[8, 5];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                symbolsRepartition[i, j] = symbols[0];
                symbols.RemoveAt(0);
            }
        }


        for (int i = 0; i < NB_RULES; i++)
        {
            rulesList.Add(GenerateMathSymbolRule());
        }

        symbolsSprites = Resources.LoadAll<Sprite>("Textures/Symbols/");

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

        List<int> randomButtonValues = new(buttonValues);
        Functions.Shuffle(randomButtonValues);

        int nbSymbolsChosen = 0;
        while (nbSymbolsChosen < 8)
        {
            Symbols randSymbol = (Symbols)Random.Range(0, Enum.GetValues(typeof(Symbols)).Length);
            if (valeursBtn.ContainsKey(randSymbol))
            {
                continue;
            }
            valeursBtn[randSymbol] = randomButtonValues[nbSymbolsChosen];
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

    public Sprite[] GetSymbolsSprites()
    {
        return symbolsSprites;
    }

    public Symbols[,] GetSymbolsRepartition()
    {
        return symbolsRepartition;
    }

    public int[] GetButtonValues()
    {
        return buttonValues;
    }

}
