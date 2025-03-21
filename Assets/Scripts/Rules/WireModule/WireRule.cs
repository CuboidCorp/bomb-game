using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public struct WireRule
{
    /// <summary>
    /// Nombre de fils vis�s par la r�gle
    /// </summary>
    public int nbWires;
    /// <summary>
    /// Si la condition est invers�e ou non
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
    /// Quantit� de fils qui doivent respecter la condition NYI
    /// </summary>
    public int quantity;
    /// <summary>
    /// Relation a check en fonction de la quantit� NYI
    /// </summary>
    public QuantityType quantityType;

    /// <summary>
    /// L'index du fil sur lequel la r�gle s'applique  NYI
    /// (De 0 � nbWires - 1)
    /// </summary>
    public int wirePositionIndex;

    /// <summary>
    /// Contraintes � respecter pour cette r�gle ( Liste des regles pr�c�dentes)
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
            _ => qt, // Par s�curit�, retourne le type actuel si non d�fini
        };
    }

    /// <summary>
    /// Ajoute l'inverse d'une regle aux contraintes de cette regle
    /// </summary>
    /// <param name="ruleToAdd"></param>
    public readonly void AddConstraint(WireRule ruleToAdd) => constraints.Add(ruleToAdd.GetRuleInverse());

    /// <summary>
    /// Renvoie la r�gle sous forme lisible traduite en fonction de la locale
    /// </summary>
    /// <returns>La regle en string</returns>
    public readonly string GetRuleString()
    {
        LocalizedString ruleString;
        if (invertCondition)
        {
            ruleString = TextLocalizationHandler.GetSmartString("TexteManuel", "WIRE_RULE_INVERTED");
        }
        else
        {
            ruleString = TextLocalizationHandler.GetSmartString("TexteManuel", "WIRE_RULE_NORMAL");
        }
        object[] args = new object[2];
        switch (condition)
        {
            case WireConditionTarget.Material:
                args[0] = TextLocalizationHandler.LoadString("GenericText", $"WIRE_COLOR{((int)(WireMaterials)targetType) + 1}");
                break;
            case WireConditionTarget.Type:
                args[0] = TextLocalizationHandler.LoadString("TexteManuel", $"WIRE_TYPE{((int)(WireType)targetType) + 1}_DESC");
                break;
        }

        args[1] = TextLocalizationHandler.LoadString("GenericText", $"NB_TO_TEXT{action.targetIndex + 1}");

        ruleString.Arguments = args;

        return ruleString.GetLocalizedString();
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
