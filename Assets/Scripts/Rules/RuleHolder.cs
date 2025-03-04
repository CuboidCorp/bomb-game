public class RuleHolder
{
    public WireRuleGenerator wireRuleGenerator = null;
    public LabyRuleGenerator labyRuleGenerator = null;
    public ButtonRuleGenerator buttonRuleGenerator = null;
    public MorseRuleGenerator morseRuleGenerator = null;
    public MathSymbolRuleGenerator mathSymbolRuleGenerator = null;
    public SafeRuleGenerator safeRuleGenerator = null;

    public SerialNumberGenerator serialNumberGenerator = null;

    /// <summary>
    /// Genere les annexes de la bombe
    /// </summary>
    public void GenerateAppendixes()
    {
        serialNumberGenerator = new();
        serialNumberGenerator.GenerateSerialNumber();
    }

    /// <summary>
    /// G�n�re les r�gles des modules
    /// </summary>
    /// <param name="modules">Les modules dont on veux que les r�gles soient g�n�r�es</param>
    /// <exception cref="System.Exception">Si on donne un module non implement�</exception>
    public void Generate(ModuleType[] modules)
    {
        GenerateAppendixes();

        foreach (ModuleType module in modules)
        {
            switch (module)
            {
                case ModuleType.WIRES:
                    if (wireRuleGenerator == null)
                    {
                        wireRuleGenerator = new();
                        wireRuleGenerator.SetupRules();
                    }
                    break;
                case ModuleType.LABY:
                    if (labyRuleGenerator == null)
                    {
                        labyRuleGenerator = new();
                        labyRuleGenerator.SetupRules();
                    }
                    break;
                case ModuleType.BUTTON:
                    if (buttonRuleGenerator == null)
                    {
                        buttonRuleGenerator = new();
                        buttonRuleGenerator.SetupRules();
                    }
                    break;
                case ModuleType.MORSE:
                    if (morseRuleGenerator == null)
                    {
                        morseRuleGenerator = new();
                        morseRuleGenerator.SetupRules();
                    }
                    break;
                case ModuleType.MATH_SYMBOL:
                    if (mathSymbolRuleGenerator == null)
                    {
                        mathSymbolRuleGenerator = new();
                        mathSymbolRuleGenerator.SetupRules();
                    }
                    break;
                case ModuleType.SAFE:
                    if(safeRuleGenerator == null)
                    {
                        safeRuleGenerator = new();
                        safeRuleGenerator.SetSerialNumberGen(serialNumberGenerator);
                        safeRuleGenerator.SetupRules();
                    }
                    break;
                case ModuleType.EMPTY:
                    break;
                default:
                    throw new System.Exception("Module non reconnu");
            }
        }
    }

    /// <summary>
    /// Renvoie le g�n�rateur de r�gles du module en morse
    /// </summary>
    /// <returns>Le g�n�rateur si il existe, sinon un nouveau</returns>
    public MorseRuleGenerator GetMorseRuleGenerator()
    {
        if (morseRuleGenerator == null)
        {
            return new MorseRuleGenerator();
        }
        return morseRuleGenerator;
    }
}
