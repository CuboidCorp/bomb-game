using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonModule : Module
{

    /// <summary>
    /// Le bouton dans le module
    /// </summary>
    [SerializeField] private GameObject button;
    private Animator buttonAnimator;

    /// <summary>
    /// Le texte pour afficher le mot
    /// </summary>
    [SerializeField] private TMP_Text text;

    private ButtonRule targetRule;

    private bool isPressed = false;
    private float pressStartTime;

    public override void SetupModule(RuleHolder rules)
    {
        buttonAnimator = button.GetComponent<Animator>();

        targetRule = rules.buttonRuleGenerator.GetRule();

        text.text = TextLocalizationHandler.LoadString("TexteInGame", targetRule.wordKey);

        button.GetComponent<MeshRenderer>().material = targetRule.buttonMaterial;

        Debug.Log($"Solution {gameObject.name} : {targetRule}");
    }

    public override void ModuleInteract(Ray rayInteract)
    {
        //On fait rien, sur ce module seul le hold est important
    }

    public override void OnModuleHoldStart(Ray rayInteract, InputAction pos)
    {
        if (Physics.Raycast(rayInteract, out RaycastHit hit, 10))
        {
            if (hit.collider.gameObject == gameObject)
            {
                isPressed = true;
                buttonAnimator.SetTrigger("Press");
                pressStartTime = Time.time;
            }
        }
    }

    public override void OnModuleHoldEnd()
    {
        if (!isPressed)
        {
            return;
        }
        isPressed = false;
        buttonAnimator.SetTrigger("Release");
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
                if (pressTime > targetRule.targetPressTime - .25f && pressTime < targetRule.targetPressTime + .25f)
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