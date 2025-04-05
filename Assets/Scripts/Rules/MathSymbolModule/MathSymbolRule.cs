using System.Collections.Generic;
using System.Linq;

public struct MathSymbolRule
{
    public int targetNumber;
    public List<KeyValuePair<Symbols, int>> valeursBtn;

    public override string ToString()
    {
        return "Nombre cible : " + targetNumber + " Valeurs btn : | " + string.Join(" | ", valeursBtn.Select(kvp => kvp.Value));
    }

}
