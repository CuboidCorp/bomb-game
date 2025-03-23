using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class WireRuleGenerator : IRuleGenerator
{
    private const int NB_WIRES_MIN = 3;
    private const int NB_WIRES_MAX = 5;

    private const int NB_RULES = 4;
    private WireRule[] rules;
    /// <summary>
    /// Regles triées et randomisées par nombre de fils
    /// </summary>
    private Dictionary<int, WireRule[]> rulesByNbWires;
    private int currentNbRulesIndex = 0;

    private List<int> nbWiresList;
    private int currentNbWiresIndex = 0;

    public void SetupRules()
    {
        nbWiresList = new List<int>();
        int nbWireRules = NB_WIRES_MAX + 1 - NB_WIRES_MIN;
        rulesByNbWires = new Dictionary<int, WireRule[]>();
        rules = new WireRule[nbWireRules * NB_RULES];
        List<WireRule> rulesList = new();
        for (int i = 0; i < nbWireRules; i++)
        {
            nbWiresList.Add(NB_WIRES_MIN + i);
            WireRule? lastWire = null;
            for (int y = 0; y < NB_RULES; y++)
            {
                WireRule newRule = GenerateRule(NB_WIRES_MIN + i, lastWire);
                rules[i * NB_RULES + y] = newRule;
                rulesList.Add(newRule);
                lastWire = newRule;
            }
            Functions.Shuffle(rulesList);
            rulesByNbWires.Add(NB_WIRES_MIN + i, rulesList.ToArray());
        }

        Functions.Shuffle(nbWiresList);

    }

    public int GetNbWire()
    {
        currentNbWiresIndex++;
        if (currentNbWiresIndex > nbWiresList.Count)
        {
            currentNbWiresIndex = 1;
        }
        return nbWiresList[currentNbWiresIndex - 1];
    }

    public WireRule GetRandomRuleFromNbWire(int nbWire)
    {
        currentNbRulesIndex++;
        if (currentNbRulesIndex > NB_RULES)
        {
            currentNbRulesIndex = 1;
        }
        return rulesByNbWires[nbWire][currentNbRulesIndex - 1];
    }

    /// <summary>
    /// Génère une règle pour un nombre de fils donné, en prenant en compte la dernière règle générée pour éviter les doublons
    /// </summary>
    /// <param name="nbWires">Nombre de fils sur lesquels s'appuyer</param>
    /// <param name="lastRule">Denière règle généré, null si c'est la première</param>
    /// <returns>Une nouvelle règle différente des anciennes</returns>
    private WireRule GenerateRule(int nbWires, WireRule? lastRule)
    {
        bool invertCondition;
        WireConditionTarget condition;
        WireMaterials? targetMaterial = null;
        WireType? targetType = null;
        WireRuleTarget action;
        WireRule wireRule = default;

        bool isRuleOkay = false;
        while (isRuleOkay == false)
        {
            invertCondition = Random.Range(0, 2) == 0;
            condition = (WireConditionTarget)Random.Range(0, Enum.GetValues(typeof(WireConditionTarget)).Length);
            switch (condition)
            {
                case WireConditionTarget.Material:
                    targetMaterial = (WireMaterials)Random.Range(0, Enum.GetValues(typeof(WireMaterials)).Length);
                    targetType = null;
                    break;
                case WireConditionTarget.Type:
                    targetType = (WireType)Random.Range(0, Enum.GetValues(typeof(WireType)).Length);
                    targetMaterial = null;
                    break;
            }
            action = new WireRuleTarget(Random.Range(0, nbWires));

            wireRule = new(nbWires, invertCondition, targetType, targetMaterial, condition, action);
            if (lastRule != null)
            {
                if (wireRule.Equals(lastRule.Value) || wireRule.Equals(lastRule.Value.GetRuleInverse()))
                {
                    continue;
                }
                bool isRuleInverseInConstraints = false;
                foreach (WireRule constraint in lastRule.Value.constraints)
                {
                    if (wireRule.Equals(constraint) || wireRule.Equals(constraint.GetRuleInverse()))
                    {
                        isRuleInverseInConstraints = true;
                        break;
                    }
                }
                if (isRuleInverseInConstraints)
                {
                    continue;
                }

                isRuleOkay = true;
                wireRule.AddConstraint(lastRule.Value);
            }
            else
            {
                isRuleOkay = true;
            }

        }

        return wireRule;
    }

    /// <summary>
    /// Renvoie les règles pour un nombre de fils donné
    /// </summary>
    /// <param name="nbWire">Le nombre de fils</param>
    /// <returns>L'ensemble des regles pour ce nombre de fils</returns>
    public WireRule[] GetRulesFromNbWire(int nbWire)
    {
        List<WireRule> rulesList = new();

        int startIndex = (nbWire - NB_WIRES_MIN) * NB_RULES;

        for (int i = startIndex; i < startIndex + NB_RULES; i++)
        {
            rulesList.Add(rules[i]);
        }

        return rulesList.ToArray();
    }



    /// <summary>
    /// Renvoie le nombre de fils minimum
    /// </summary>
    /// <returns>Le nombre de fils minimum</returns>
    public int GetNbWiresMin()
    {
        return NB_WIRES_MIN;
    }

    /// <summary>
    /// Renvoie le nombre de fils maximum (Ajoute +1 pour random pour l'inclure)
    /// </summary>
    /// <returns>Le nombre de fils max</returns>
    public int GetNbWiresMax()
    {
        return NB_WIRES_MAX;
    }
}
