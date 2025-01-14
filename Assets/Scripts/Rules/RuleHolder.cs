public class RuleHolder
{
    public WireRuleGenerator wireRuleGenerator = null;
    public LabyRuleGenerator labyRuleGenerator = null;
    public ButtonRuleGenerator buttonRuleGenerator = null;
    public MorseRuleGenerator morseRuleGenerator = null;
    public MathSymbolRuleGenerator mathSymbolRuleGenerator = null;

    public void Generate(ModuleType[] modules)
    {
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
                default:
                    throw new System.Exception("Module non reconnu");
            }
        }
    }

    public MorseRuleGenerator GetMorseRuleGenerator()
    {
        if (morseRuleGenerator == null)
        {
            return new MorseRuleGenerator();
        }
        return morseRuleGenerator;
    }
}
