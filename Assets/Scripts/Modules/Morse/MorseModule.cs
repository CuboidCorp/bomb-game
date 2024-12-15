using UnityEngine;

public class MorseModule : Module
{

    public override void SetupModule(RuleHolder rules)
    {
    }

    public override void ModuleInteract(Ray rayInteract)
    {
        GetComponent<Collider>().enabled = false;
        if (Physics.Raycast(rayInteract, out RaycastHit hit, 10))
        {
            //Faire l'interaction
        }
        GetComponent<Collider>().enabled = true;
    }

    public override void OnModuleHoldStart(Ray rayInteract)
    {
        //Rien sur ce module
    }

    public override void OnModuleHoldEnd()
    {
        //Rien sur ce module
    }
}
