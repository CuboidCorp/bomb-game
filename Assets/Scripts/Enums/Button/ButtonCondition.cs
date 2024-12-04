/// <summary>
/// La condition pour que le bouton soit relaché correctement.
/// </summary>
public enum ButtonCondition
{
    /// <summary>
    /// Le bouton doit être relaché immédiatement.
    /// </summary>
    IMMEDIATE,
    /// <summary>
    /// Le bouton doit être relaché après un certain temps.
    /// </summary>
    PRESS_FOR,
    /// <summary>
    /// Le bouton doit être relaché quand le timer contient un certain chiffre
    /// </summary>
    PRESS_UNTIL_TIMER_CONTAINS,

    /// <summary>
    /// Le bouton doit être relaché quand les secondes du timer sont entre deux valeurs
    /// </summary>
    PRESS_UNTIL_TIMER_BETWEEN,

}