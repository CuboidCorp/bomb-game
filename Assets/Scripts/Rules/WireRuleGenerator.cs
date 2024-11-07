using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WireRuleGenerator
{
    private const int NB_WIRES_MIN = 3;
    private const int NB_WIRES_MAX = 5;

    private const int NB_RULES = 4;

    private WireRule[] rules;

    public void SetupRules()
    {
        int nbWireRules = NB_WIRES_MAX - NB_WIRES_MIN;
        rules = new WireRule[nbWireRules * NB_RULES];
        for (int i = 0; i < nbWireRules; i++)
        {
            WireRule? lastWire = null;
            for (int y = 0; y < NB_RULES; y++)
            {
                rules[i * NB_RULES + y] = GenerateRule(lastWire);
                lastWire = rules[i * NB_RULES + y];
            }
        }

        foreach (WireRule rule in rules)
        {
            Debug.Log(rule.GetRuleString());
        }

    }

    private WireRule GenerateRule(WireRule? lastRule)
    {
        //Randomiser certaines parties
        bool invertCondition;
        WireConditionTarget condition = (WireConditionTarget)Random.Range(0, Enum.GetValues(typeof(WireConditionTarget)).Length);
        Enum targetType = condition switch
        {
            WireConditionTarget.Material => (WireMaterials)Random.Range(0, Enum.GetValues(typeof(WireMaterials)).Length),
            WireConditionTarget.Type => (WireType)Random.Range(0, Enum.GetValues(typeof(WireType)).Length),
            _ => throw new NotImplementedException(),
        };

        int quantity = Random.Range(0, 3);
        if (quantity == 0)
        {
            invertCondition = Random.Range(0, 2) == 0;
        }
        else
        {
            invertCondition = false;
        }
        //En fonction de la quantité, on va choisir un type de quantité
        QuantityType quantityType = (QuantityType)Random.Range(0, Enum.GetValues(typeof(QuantityType)).Length - 1);
        bool isRuleOkay = false;
        WireRule wireRule = new(invertCondition, targetType, condition, quantity, quantityType);
        while (isRuleOkay == false)
        {
            wireRule = new(invertCondition, targetType, condition, quantity, quantityType);
            if (lastRule != null)
            {
                if (wireRule.Equals(lastRule.Value) || wireRule.Equals(lastRule.Value.GetRuleInverse()))
                {
                    continue;
                }
                foreach (WireRule constraint in lastRule.Value.constraints)
                {
                    if (wireRule.Equals(constraint) || wireRule.Equals(constraint.GetRuleInverse()))
                    {
                        continue;
                    }
                }

                isRuleOkay = true;
            }
            else
            {
                isRuleOkay = true;
            }

        }

        if (lastRule != null)
        {
            wireRule.AddConstraint((WireRule)lastRule);
        }

        return wireRule;
    }

    public WireRule[] GetRulesFromNbWire(int nbWire)
    {
        List<WireRule> rulesList = new();

        int startIndex = (NB_WIRES_MIN - nbWire) * NB_RULES;

        for (int i = startIndex; i < startIndex + NB_RULES; i++)
        {
            rulesList.Add(rules[i]);
        }

        return rulesList.ToArray();
    }

    public int GetNbWiresMin()
    {
        return NB_WIRES_MIN;
    }

    public int GetNbWiresMax()
    {
        return NB_WIRES_MAX;
    }
}
