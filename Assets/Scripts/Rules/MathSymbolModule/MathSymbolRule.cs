using System.Collections.Generic;

public struct MathSymbolRule
{
    public int targetNumber;
    public Dictionary<Symbols, int> valeursBtn;

    public override string ToString()
    {
        return "Nombre cible : " + targetNumber + " Valeurs btn : | " + string.Join(" | ", valeursBtn.Values);
    }
}
