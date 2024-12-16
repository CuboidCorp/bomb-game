public struct MorseRule
{
    public char targetCharacter;
    public bool[] targetMorseCode;

    /// <summary>
    /// Les boutons qui doivent être appuyés pour valider la règle
    /// </summary>
    public bool[,] correctImage;

}
