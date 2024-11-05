public class RuleHolder
{
    public WireRuleGenerator wireRuleGenerator = null;

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
            }
        }
    }
}
