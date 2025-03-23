/// <summary>
/// Le fil a couper apres 
/// </summary>
public struct WireRuleTarget
{
    /// <summary>
    /// L'index du fil a couper
    /// </summary>
    public int targetIndex;

    public WireRuleTarget(int targetIndex)
    {
        this.targetIndex = targetIndex;
    }


    public bool IsIndexCorrect(int index)
    {
        return targetIndex == index;
    }
}
