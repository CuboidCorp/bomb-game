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
        return condition switch
        {
            ButtonCondition.IMMEDIATE => "Relacher imm�diatement",
            ButtonCondition.PRESS_UNTIL_TIMER_CONTAINS => $"Relacher quand le timer contient {targetTimerNumber}",
            ButtonCondition.PRESS_FOR => $"Relacher apr�s {targetPressTime} secondes",
            ButtonCondition.PRESS_UNTIL_TIMER_BETWEEN => $"Relacher quand le timer est entre {targetTimerBetweenBounds.x} et {targetTimerBetweenBounds.y}",
            _ => "Condition inconnue",
        };
    }
}