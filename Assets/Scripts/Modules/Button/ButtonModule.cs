using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

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

    private bool isPressed = false;
    private float pressStartTime;

    public override void SetupModule(RuleHolder rules)
    {
        targetRule = rules.buttonRuleGenerator.GetRule();

        text.text = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("TexteInGame", targetRule.wordKey).Result;

        button.GetComponent<MeshRenderer>().material = targetRule.buttonMaterial;

    }

    public override void ModuleInteract(Ray rayInteract)
    {
        //On fait rien, sur ce module seul le hold est important
    }

    public override void OnModuleHoldStart(Ray rayInteract)
    {
        Debug.DrawRay(rayInteract.origin, rayInteract.direction * 10, Color.black, 5);
        GetComponent<Collider>().enabled = false;
        if (Physics.Raycast(rayInteract, out RaycastHit hit, 10))
        {
            Debug.Log("Hold a touché");
            if (hit.collider.gameObject == button)
            {
                Debug.Log("Hold sur bouton");
                isPressed = true;
                pressStartTime = Time.time;
            }
        }
        GetComponent<Collider>().enabled = true;
    }

    public override void OnModuleHoldEnd()
    {
        if (!isPressed)
        {
            return;
        }
        isPressed = false;
        float pressTime = Time.time - pressStartTime;
        bool success = false;
        Debug.Log("Press time : " + pressTime);
        switch (targetRule.condition)
        {
            case ButtonCondition.IMMEDIATE:
                Debug.Log("Immediate");
                if (pressTime < 0.2f)
                {
                    Debug.Log("Bouton relaché immédiatement");
                    success = true;
                    Success();
                }
                break;
            case ButtonCondition.PRESS_FOR:
                Debug.Log("PRESS_FOR");
                Debug.Log("Target press" + targetRule.targetPressTime);
                if (pressTime > targetRule.targetPressTime && pressTime < targetRule.targetPressTime + .5f)
                {
                    Debug.Log("Bouton relaché après un certain temps");
                    success = true;
                    Success();
                }
                break;
            case ButtonCondition.PRESS_UNTIL_TIMER_CONTAINS:
                Debug.Log("PRESS_UNTIL_TIMER_CONTAINS");
                Debug.Log("Target timer" + targetRule.targetTimerNumber);
                string timerText = TimeSpan.FromSeconds(Timer.instance.nbSeconds).ToString("mm\\:ss");
                if (timerText.Contains(targetRule.targetTimerNumber.ToString()))
                {
                    Debug.Log("Bouton relaché quand le timer contient un certain chiffre");
                    success = true;
                    Success();
                }
                break;
            case ButtonCondition.PRESS_UNTIL_TIMER_BETWEEN:
                int nbSecondes = Timer.instance.nbSeconds % 60;
                Debug.Log("PRESS_UNTIL_TIMER_BETWEEN");
                Debug.Log("Target timer between bounds" + targetRule.targetTimerBetweenBounds);
                if (nbSecondes >= targetRule.targetTimerBetweenBounds.x && nbSecondes < targetRule.targetTimerBetweenBounds.y)
                {
                    Debug.Log("Bouton relaché quand le timer est entre deux valeurs");
                    success = true;
                    Success();
                }
                break;
        }

        if (success == false)
        {

            Fail();
        }
    }
}