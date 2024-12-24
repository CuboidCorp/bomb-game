using UnityEngine;

/// <summary>
/// Le fil a couper apres 
/// </summary>
public struct WireRuleTarget
{
    /// <summary>
    /// L'index du fil a couper
    /// </summary>
    public int targetIndex;

    /// <summary>
    /// Le materiel a couper NYI
    /// </summary>
    public Material targetMaterial;

    public WireRuleTarget(int targetIndex, Material targetMaterial = null)
    {
        this.targetIndex = targetIndex;
        this.targetMaterial = targetMaterial;
    }


    public bool IsIndexCorrect(int index)
    {
        return targetIndex == index;
    }
}
