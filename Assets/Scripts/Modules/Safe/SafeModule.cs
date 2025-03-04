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
    private Vector2 lastMousePos;

    private float currentRotation = 0f; // in degrees
    private int currentValue = 0;       // value from 0 to 99
    private float accumulatedDelta = 0f;
    private const float rotationStep = 3.6f; // each step represents one number
    // Adjust sensitivity to map pixel movement to degrees
    [SerializeField] private float sensitivity = 0.1f;
    // Record the direction of the last step (positive for clockwise, negative for anticlockwise)
    private float lastStepDirection = 0f;

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
        // Stop rotation.
        isHolding = false;

        // When the player stops, check if the dial's current number matches the target.
        int targetValue = valeursCibles[currentStep];
        bool expectedDirection = directions[currentStep]; // true = clockwise, false = anticlockwise

        // Determine if the final step's direction is correct.
        bool correctDirection = false;
        if (lastStepDirection != 0f)
        {
            // A positive step means clockwise.
            correctDirection = (lastStepDirection > 0 && expectedDirection) || (lastStepDirection < 0 && !expectedDirection);
        }

        Debug.Log($"Step {currentStep}: currentValue = {currentValue}, targetValue = {targetValue}, lastStepDirection = {lastStepDirection}, expectedDirection = {expectedDirection}");

        if (currentValue == targetValue && correctDirection)
        {
            Debug.Log("Success");
            //Success();
            currentStep++;
            // Optionally, reset values or prepare for the next step.
        }
        else
        {
            //Fail();
        }
    }

    public override void OnModuleHoldStart(Ray rayInteract, InputAction pos)
    {
        //TODO : On commence la rotation en fonction du delta de la pos de la souris
        isHolding = true;
        mousePos = pos;
        lastMousePos = pos.ReadValue<Vector2>();
    }

    public override void ModuleInteract(Ray rayInteract)
    {
        //Rien pour ce module
    }

    private void Update()
    {
        if (!isHolding)
            return;

        Vector2 currentMousePos = mousePos.ReadValue<Vector2>();
        float deltaX = currentMousePos.x - lastMousePos.x;
        lastMousePos = currentMousePos;

        // Convert pixel movement to degrees.
        float rotationDelta = deltaX * sensitivity;
        accumulatedDelta += rotationDelta;

        // Check if the accumulated delta exceeds one step.
        while (Mathf.Abs(accumulatedDelta) >= rotationStep)
        {
            // Determine the sign (direction) of the step.
            float stepSign = Mathf.Sign(accumulatedDelta);

            // Update the dial's rotation by one step.
            currentRotation += rotationStep * stepSign;
            currentRotation %= 360f;
            if (currentRotation < 0)
                currentRotation += 360f;

            // Each step corresponds to a new dial number.
            currentValue = Mathf.RoundToInt(currentRotation / rotationStep) % 100;

            // Record the direction of this step.
            lastStepDirection = stepSign;

            // Deduct the step from the accumulated delta.
            accumulatedDelta -= rotationStep * stepSign;

            // Update the dial's visual rotation.
            transform.localEulerAngles = new Vector3(0, 0, currentRotation);
        }
    }
}
