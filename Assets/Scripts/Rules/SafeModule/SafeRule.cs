using UnityEngine;

public struct SafeRule
{
    /// <summary>
    /// Les directions a prendre pour d�verouiller le coffre, true pour croissant, false pour d�croissant
    /// </summary>
    public bool[] directions;
    /// <summary>
    /// Les valeurs sur lesquelles s'arreter
    /// </summary>
    public int[] valeurs;
}
