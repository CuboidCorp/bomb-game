using System;
using System.Collections.Generic;
using UnityEngine;

public struct WireRule
{
    /// <summary>
    /// Nombre de fils visés par la règle
    /// </summary>
    public int nbWires;
    /// <summary>
    /// Si la condition est inversée ou non
    /// </summary>
    public bool invertCondition;
    /// <summary>
    /// L'objet sur lequel la condition s'applique
    /// Ici (Wire Materials and Wire Type)
    /// </summary>
    public Enum targetType;
    /// <summary>
    /// Permet d'identifier le target de la condition
    /// </summary>
    public WireConditionTarget condition;
    /// <summary>
    /// Quantité de fils qui doivent respecter la condition NYI
    /// </summary>
    public int quantity;
    /// <summary>
    /// Relation a check en fonction de la quantité NYI
    /// </summary>
    public QuantityType quantityType;

    /// <summary>
    /// L'index du fil sur lequel la règle s'applique  NYI
    /// (De 0 à nbWires - 1)
    /// </summary>
    public int wirePositionIndex;

    /// <summary>
    /// Contraintes à respecter pour cette règle ( Liste des regles précédentes)
    /// </summary>
    public List<WireRule> constraints;

    /// <summary>
    /// Le fil a couper apres
    /// </summary>
    public WireRuleTarget action;

    public WireRule(int nbWires, bool invertCondition, Enum targetType, WireConditionTarget condition, int quantity, QuantityType quantityType, int wirePosition, WireRuleTarget action, List<WireRule> constraints = null)
    {
        this.nbWires = nbWires;
        this.invertCondition = invertCondition;
        this.targetType = targetType;
        this.condition = condition;
        this.quantity = quantity;
        this.quantityType = quantityType;
        this.constraints = constraints;
        this.wirePositionIndex = wirePosition;
        this.action = action;
        this.constraints ??= new List<WireRule>();
    }

    public readonly WireRule GetRuleInverse() => new(nbWires, !invertCondition, targetType, condition, quantity, GetInverseQuantityType(quantityType), wirePositionIndex, action, constraints);

    private static QuantityType GetInverseQuantityType(QuantityType qt)
    {
        return qt switch
        {
            QuantityType.LESSER_THAN => QuantityType.GREATER_THAN_OR_EQUALS,
            QuantityType.GREATER_THAN => QuantityType.LESSER_THAN_OR_EQUALS,
            QuantityType.EQUALS => QuantityType.NOT_EQUALS,
            QuantityType.LESSER_THAN_OR_EQUALS => QuantityType.GREATER_THAN,
            QuantityType.GREATER_THAN_OR_EQUALS => QuantityType.LESSER_THAN,
            QuantityType.NOT_EQUALS => QuantityType.EQUALS,
            _ => qt, // Par sécurité, retourne le type actuel si non défini
        };
    }

    /// <summary>
    /// Ajoute l'inverse d'une regle aux contraintes de cette regle
    /// </summary>
    /// <param name="ruleToAdd"></param>
    public readonly void AddConstraint(WireRule ruleToAdd) => constraints.Add(ruleToAdd.GetRuleInverse());

    public string GetRuleString()
    {
        string ruleString = "Si il ";
        if (invertCondition)
        {
            ruleString += "n'";
        }
        ruleString += "y a ";
        if (invertCondition)
        {
            ruleString += "pas ";
        }
        switch (condition)
        {
            case WireConditionTarget.Material:
                ruleString += $"le matériel ";
                break;
            case WireConditionTarget.Type:
                ruleString += $"le type ";
                break;
        }

        ruleString += targetType.ToString() + " alors ";

        ruleString += action.GetWireTargetString();

        return ruleString;
    }

    public string GetRuleStringWithoutAction()
    {
        string ruleString = "Si il ";
        if (invertCondition)
        {
            ruleString += "n'";
        }
        ruleString += "y a ";
        if (invertCondition)
        {
            ruleString += "pas ";
        }
        switch (condition)
        {
            case WireConditionTarget.Material:
                ruleString += $"le matériel ";
                break;
            case WireConditionTarget.Type:
                ruleString += $"le type ";
                break;
        }

        ruleString += targetType.ToString() + " ";
        return ruleString;
    }


    public string GetFullRuleString()
    {
        string ruleString = GetRuleString();
        foreach (WireRule constraint in constraints)
        {
            ruleString += "et ";
            ruleString += constraint.GetRuleStringWithoutAction();
        }
        return ruleString;
    }

    public override bool Equals(object obj)
    {
        WireRule? other = obj as WireRule?;

        if (other == null)
        {
            return false;
        }
        if (nbWires != other.Value.nbWires)
        {
            return false;
        }
        if (invertCondition != other.Value.invertCondition)
        {
            return false;
        }
        if (targetType != other.Value.targetType)
        {
            return false;
        }
        if (condition != other.Value.condition)
        {
            return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(invertCondition, targetType, condition, quantity, quantityType, constraints);
    }

    public bool IsWireCorrect(int wireIndex, int wireType, Material mat)
    {
        //Pour le momement on ne check que l'index du fil
        return action.IsIndexCorrect(wireIndex);
    }
}
