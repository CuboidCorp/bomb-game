public class RuleHolder
{
    public WireRuleGenerator wireRuleGenerator = null;
    public LabyRuleGenerator labyRuleGenerator = null;
    public ButtonRuleGenerator buttonRuleGenerator = null;

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
                default:
                    throw new System.Exception("Module non reconnu");
            }
        }
    }
}
