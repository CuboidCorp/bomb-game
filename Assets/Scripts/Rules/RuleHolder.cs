public class RuleHolder
{
    public WireRuleGenerator wireRuleGenerator;

    public void Generate(ModuleType[] modules)
    {
        foreach (ModuleType module in modules)
        {
            switch (module)
            {
                case ModuleType.WIRES:
                    wireRuleGenerator.SetupRules();
                    break;
            }
        }
    }
}
