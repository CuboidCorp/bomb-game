using System;
using System.Collections.Generic;

public struct WireRule
{
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
    /// Contraintes à respecter pour cette règle ( Liste des regles précédentes)
    /// </summary>
    public List<WireRule> constraints;

    public WireRule(bool invertCondition, Enum targetType, WireConditionTarget condition, int quantity, QuantityType quantityType, List<WireRule> constraints = null)
    {
        this.invertCondition = invertCondition;
        this.targetType = targetType;
        this.condition = condition;
        this.quantity = quantity;
        this.quantityType = quantityType;
        this.constraints = constraints;
        this.constraints ??= new List<WireRule>();
    }

    public WireRule GetRuleInverse()
    {
        return new WireRule(!invertCondition, targetType, condition, quantity, GetInverseQuantityType(quantityType), constraints);
    }

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
    public void AddConstraint(WireRule ruleToAdd)
    {
        constraints.Add(ruleToAdd.GetRuleInverse());
    }

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

        ruleString += targetType.ToString() + " ";

        foreach (WireRule constraint in constraints)
        {
            ruleString += "et ";
            ruleString += constraint.GetRuleString();
        }

        return ruleString;
    }
}
