using System.Collections.Generic;

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

    /// <summary>
    /// Tableaux des index des bonnes directions (dans directions)
    /// </summary>
    public int[] goodDirIndex;

    /// <summary>
    /// Tableau des index des bonnes valeurs (dans valeurs)
    /// </summary>
    public int[] goodValIndex;

    /// <summary>
    /// Liste des questions 
    /// </summary>
    public List<SerialNumberConditions> questions;

    /// <summary>
    /// Liste des annexes des questions
    /// </summary>
    public string[] annexesQuestions;
}
