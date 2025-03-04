using UnityEngine;

public struct SafeRule
{
    /// <summary>
    /// Les directions a prendre pour déverouiller le coffre, true pour croissant, false pour décroissant
    /// </summary>
    public bool[] directions;
    /// <summary>
    /// Les valeurs sur lesquelles s'arreter
    /// </summary>
    public int[] valeurs;
}
