using UnityEngine;

public class SafeModule : Module
{
    public override void SetupModule(RuleHolder rules)
    {
        //TODO : Récupération des règles du module
    }

    public override void OnModuleHoldEnd()
    {
        //TODO : On arrete la rotation
    }

    public override void OnModuleHoldStart(Ray rayInteract)
    {
        //TODO : On commence la rotation en fonction du delta de la pos de la souris
    }






    public override void ModuleInteract(Ray rayInteract)
    {
        //Rien pour ce module
    }
}
