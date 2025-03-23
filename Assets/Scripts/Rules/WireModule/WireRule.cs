using System;
using System.Collections.Generic;
using UnityEngine.Localization;

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
    /// Le matériau cible de la condition, null si on check le type
    /// </summary>
    public WireMaterials? targetMaterial;
    /// <summary>
    /// Le type cible de la condition, null si on check le matériau
    /// </summary>
    public WireType? targetType;
    /// <summary>
    /// Permet d'identifier le target de la condition
    /// </summary>
    public WireConditionTarget condition;

    /// <summary>
    /// Contraintes à respecter pour cette règle ( Liste des regles précédentes)
    /// </summary>
    public List<WireRule> constraints;

    /// <summary>
    /// Le fil a couper apres
    /// </summary>
    public WireRuleTarget action;

    public WireRule(int nbWires, bool invertCondition, WireType? targetType, WireMaterials? targetMaterial, WireConditionTarget condition, WireRuleTarget action, List<WireRule> constraints = null)
    {
        this.nbWires = nbWires;
        this.invertCondition = invertCondition;
        this.targetType = targetType;
        this.targetMaterial = targetMaterial;
        this.condition = condition;
        this.constraints = constraints;
        this.action = action;
        this.constraints ??= new List<WireRule>();
    }

    public readonly WireRule GetRuleInverse() => new(nbWires, !invertCondition, targetType, targetMaterial, condition, action, constraints);

    /// <summary>
    /// Ajoute l'inverse d'une regle aux contraintes de cette regle
    /// </summary>
    /// <param name="ruleToAdd"></param>
    public readonly void AddConstraint(WireRule ruleToAdd)
    {
        foreach (WireRule constraint in ruleToAdd.constraints)
        {
            constraints.Add(constraint);
        }
        constraints.Add(ruleToAdd.GetRuleInverse());
    }

    /// <summary>
    /// Renvoie la règle sous forme lisible traduite en fonction de la locale
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
                args[0] = TextLocalizationHandler.LoadString("GenericText", $"WIRE_COLOR{((int)targetMaterial.Value) + 1}");
                break;
            case WireConditionTarget.Type:
                args[0] = TextLocalizationHandler.LoadString("TexteManuel", $"WIRE_TYPE{((int)targetType.Value) + 1}_DESC");
                break;
        }

        args[1] = TextLocalizationHandler.LoadString("GenericText", $"NB_TO_TEXT{action.targetIndex + 1}");

        ruleString.Arguments = args;

        return ruleString.GetLocalizedString();
    }

    public override string ToString()
    {
        return GetRuleString();
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
        if (targetMaterial != other.Value.targetMaterial)
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
        return HashCode.Combine(invertCondition, targetType, condition, constraints);
    }

    /// <summary>
    /// Vérifie si le fil coupé est correct
    /// </summary>
    /// <param name="wireIndex">L'index du fil coupé</param>
    /// <returns>True si correct, false sinon</returns>
    public bool IsWireCorrect(int wireIndex)
    {
        //Pour le momement on ne check que l'index du fil
        return action.IsIndexCorrect(wireIndex);
    }
}
