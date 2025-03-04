using UnityEngine;

public class SafeModule : Module
{
    private SafeRule rule;

    private int[] valeursCibles;
    private bool[] directions;

    public override void SetupModule(RuleHolder rules)
    {
        rule = rules.safeRuleGenerator.GetRule();

        valeursCibles = new int[3];
        directions = new bool[3];

        for (int i = 0; i < 3; i++)
        {
            valeursCibles[i] = rule.valeurs[rule.goodValIndex[i]];
            directions[i] = rule.directions[rule.goodDirIndex[i]];
        }

        Debug.Log("Valeurs cibles : " + valeursCibles[0] + " " + valeursCibles[1] + " " + valeursCibles[2]);
        Debug.Log("Directions : " + directions[0] + " " + directions[1] + " " + directions[2]);

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
