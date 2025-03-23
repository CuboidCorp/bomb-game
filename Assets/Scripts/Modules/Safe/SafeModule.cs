using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SafeModule : Module
{
    private SafeRule rule;

    private int[] valeursCibles;
    private bool[] directions;

    private bool isHolding = false;
    private int currentStep = 0;
    private InputAction mousePos;

    private float currentRotation = 0f;
    private int currentValue = 0;
    private float accumulatedDelta = 0f;
    private const float rotationStep = 3.6f;

    private int nbErrors = 0;
    private const int MAX_ERRORS = 3;

    [SerializeField] private Transform rotatingPart;
    [SerializeField] private float sensitivity = 0.1f;
    private float lastStepDirection = 0f;

    [SerializeField] private TMP_Text valueText;

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
        Debug.Log($"Solution {gameObject.name}");
        Debug.Log($"Valeurs cibles : {valeursCibles[0]} {valeursCibles[1]} {valeursCibles[2]}");
        Debug.Log($"Directions : {directions[0]} {directions[1]} {directions[2]}");

    }

    public override void OnModuleHoldEnd()
    {
        isHolding = false;

        int targetValue = valeursCibles[currentStep];
        bool expectedDirection = directions[currentStep]; // true = clockwise, false = anticlockwise

        bool correctDirection = false;
        if (lastStepDirection != 0f)
        {
            correctDirection = (lastStepDirection > 0 && expectedDirection) || (lastStepDirection < 0 && !expectedDirection);
        }

        Debug.Log($"Step {currentStep}: currentValue = {currentValue}, targetValue = {targetValue}, lastStepDirection = {lastStepDirection}, expectedDirection = {expectedDirection}");

        if (currentValue == targetValue && correctDirection)
        {
            AudioManager.Instance.PlaySoundEffect(SoundEffects.LIGHT_CLICK);
            currentStep++;

            if (currentStep == 3)
            {
                Success();
            }
        }
        else
        {
            nbErrors++;
            if (nbErrors >= MAX_ERRORS)
            {
                Fail();
                nbErrors = 0;
            }
        }
    }

    public override void OnModuleHoldStart(Ray rayInteract, InputAction pos)
    {
        isHolding = true;
        mousePos = pos;
    }

    public override void ModuleInteract(Ray rayInteract)
    {
        //Rien pour ce module
    }

    private void Update()
    {
        if (isHolding)
        {
            Vector2 mouseScreenPos = mousePos.ReadValue<Vector2>();
            Vector3 screenPos = new(
                mouseScreenPos.x,
                mouseScreenPos.y,
                Camera.main.WorldToScreenPoint(rotatingPart.position).z);
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(screenPos);

            Vector2 direction = mouseWorldPos - rotatingPart.position;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            targetAngle = (targetAngle + 270f) % 360f;

            float angleDiff = Mathf.DeltaAngle(currentRotation, targetAngle);
            if (Mathf.Abs(angleDiff) < rotationStep)
            {
                angleDiff = 0f;
            }

            float rotationDelta = angleDiff * sensitivity;
            accumulatedDelta += rotationDelta;

            while (Mathf.Abs(accumulatedDelta) >= rotationStep)
            {
                float stepSign = Mathf.Sign(accumulatedDelta);

                currentRotation += rotationStep * stepSign;
                currentRotation %= 360f;
                if (currentRotation < 0)
                    currentRotation += 360f;

                currentValue = Mathf.RoundToInt(currentRotation / rotationStep) % 100;
                valueText.text = currentValue.ToString();

                lastStepDirection = stepSign;
                accumulatedDelta -= rotationStep * stepSign;
            }

            rotatingPart.localEulerAngles = new Vector3(currentRotation - 90, -90, -90);

        }
    }


}
