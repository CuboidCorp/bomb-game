using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MathSymbolRuleGenerator : IRuleGenerator
{
    private const int NB_RULES = 5;

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
            MathSymbolRule rule = GenerateMathSymbolRule(i);
            rulesList.Add(rule);
        }

        symbolsSprites = Resources.LoadAll<Sprite>("Textures/Symbols/");

        Functions.Shuffle(rulesList);

        //On shuffle les symboles de chaque ligne
        for (int i = 0; i < 8; i++)
        {
            List<Symbols> ligne = new();
            for (int j = 0; j < 5; j++)
            {
                ligne.Add(symbolsRepartition[i, j]);
            }
            Functions.Shuffle(ligne);
            for (int j = 0; j < 5; j++)
            {
                symbolsRepartition[i, j] = ligne[j];
            }
        }

        rules = rulesList.ToArray();

    }

    private MathSymbolRule GenerateMathSymbolRule(int index)
    {
        MathSymbolRule rule = new()
        {
            targetNumber = Random.Range(400, 999)
        };

        List<KeyValuePair<Symbols, int>> valeursBtn = new();

        for (int i = 0; i < 8; i++)
        {
            Symbols symbol = symbolsRepartition[i, index];
            valeursBtn.Add(new(symbol, buttonValues[i]));
        }

        Functions.Shuffle(valeursBtn);

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
