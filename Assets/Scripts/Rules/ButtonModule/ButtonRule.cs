using UnityEngine;
public struct ButtonRule
{
    public Material buttonMaterial;
    public string wordKey;
    public ButtonCondition condition;

    public float targetPressTime;
    public int targetTimerNumber;
    public Vector2Int targetTimerBetweenBounds;

    public override string ToString()
    {
        return condition.ToString() + " " + targetPressTime + " " + targetTimerNumber + " " + targetTimerBetweenBounds.ToString();
    }
}