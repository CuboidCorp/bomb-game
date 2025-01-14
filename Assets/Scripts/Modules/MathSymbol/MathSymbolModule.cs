using UnityEngine;

public class MathSymbolModule : Module
{
    public override void ModuleInteract(Ray rayInteract)
    {
        throw new System.NotImplementedException();
    }

    public override void OnModuleHoldEnd()
    {
        //Rien pr ce module
    }

    public override void OnModuleHoldStart(Ray rayInteract)
    {
        //Rien pr ce module
    }

    public override void SetupModule(RuleHolder rules)
    {
        throw new System.NotImplementedException();
    }
}
