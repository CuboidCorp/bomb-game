using UnityEngine;

public class ButtonModule : Module
{

    /// <summary>
    /// Le bouton dans le module
    /// </summary>
    [SerializeField] private GameObject button;

    /// <summary>
    /// Le texte pour afficher le mot
    /// </summary>
    [SerializeField] private TMP_Text text; 
    private ButtonRule targetRule;

    private float pressStartTime;

    public override void SetupModule(RuleHolder rules)
    {
        targetRule = rules.buttonRuleGenerator.GetRule();

        button.GetComponent<MeshRenderer>().material = targetRule.buttonMaterial;

    }

    public override void ModuleInteract(Ray rayInteract)
    {
        if (Physics.Raycast(rayInteract, out RaycastHit hit, 10))
        {
            //On check si on touche le bouton si oui 
            if(hit.collider.gameObject == button)
            {
                pressStartTime = Time.time;
            }
        }
        GetComponent<Collider>().enabled = true;
    }

    //TODO : Ajouter une fonction qui est ajoutée au controle pour check la release du click d'interaction
    private void OnRelease()
    {
        float pressTime = Time.time - pressStartTime;
        bool success = false;
        switch (targetRule.condition)
        {
            case ButtonCondition.IMMEDIATE:
                if (pressTime < 0.2f)
                {
                    Debug.Log("Bouton relaché immédiatement");
                    success = true;
                    Success();
                }
                break;
            case ButtonCondition.PRESS_FOR:
                if (pressTime > targetRule.targetPressTime && pressTime < targetRule.targetPressTime + .5f)
                {
                    Debug.Log("Bouton relaché après un certain temps");
                    success = true;
                    Success();
                }
                break;
            case ButtonCondition.PRESS_UNTIL_TIMER_CONTAINS:
                string timerText = TimeSpan.FromSeconds(Timer.instance.nbSeconds).ToString("mm\\:ss");
                if (timerText.Contains(targetRule.targetTimerNumber.ToString()))
                {
                    Debug.Log("Bouton relaché quand le timer contient un certain chiffre");
                    success = true;
                    Success();
                }
                break;
            case ButtonCondition.PRESS_UNTIL_TIMER_BETWEEN::
                int nbSecondes = Timer.instance.nbSeconds % 60;
                if (nbSecondes >= targetRule.targetTimerBetweenBounds.x && nbSecondes < targetRule.targetTimerBetweenBounds.y)
                {
                    Debug.Log("Bouton relaché quand le timer est entre deux valeurs");
                    success = true;
                    Success();
                }
                break;  
        }

        if(success == false)
        {
            Fail();
        }
    }
}