using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

public class MorseModule : Module
{
    private MorseRule targetRule;

    #region Boutons
    [SerializeField] private Transform[] buttons;
    [SerializeField] private Material buttonOnMaterial;
    [SerializeField] private Material buttonOffMaterial;

    private const string BUTTON_NAME_PATTERN = @"^\d:\d$";
    private Vector2Int imageSize;
    private bool[,] currentImage;
    #endregion

    #region Morse Blink

    [SerializeField] private GameObject lamp;
    [SerializeField] private Material lampOffMaterial;
    [SerializeField] private Material lampOnMaterial;
    [SerializeField] private float morseTimeUnit = 0.5f;
    /** Note sur le code morse
     *  Un point c'est 1 unité de temps
     *  Un trait c'est 3 unités de temps
     *  Entre chaque point ou trait il y a 1 unité de temps
     *  Entre chaque lettre il y a 3 unités de temps (Quand on repete la lettre) Donc 2 unités + l'écart
     */

    private Coroutine blinkCoroutine;
    private bool[] blinkSequence;

    #endregion

    public override void SetupModule(RuleHolder rules)
    {
        targetRule = rules.morseRuleGenerator.GetRule();

        blinkSequence = targetRule.targetMorseCode;
        imageSize = rules.morseRuleGenerator.GetImageSize();

        if (MainGeneration.Instance.isDebug)
        {
            Debug.Log($"Module Morse on {gameObject.name}");
            Debug.Log($"Target Char : {targetRule.targetCharacter}");
            string img = "";
            for (int i = 0; i < imageSize.y; i++)
            {
                for (int j = 0; j < imageSize.x; j++)
                {
                    img += targetRule.correctImage[j, i] ? "1" : "0";
                }
                img += "\n";
            }
            Debug.Log(img);
        }


        currentImage = new bool[imageSize.x, imageSize.y];

        blinkCoroutine = StartCoroutine(BlinkCoroutine());
    }

    private void OnDisable()
    {
        StopCoroutine(blinkCoroutine);
    }

    public override void ModuleInteract(Ray rayInteract)
    {
        GetComponent<Collider>().enabled = false;
        if (Physics.Raycast(rayInteract, out RaycastHit hit, 10))
        {
            string goName = hit.collider.gameObject.name;
            Debug.Log(goName);
            if (goName == "BoutonValider")
            {
                CheckSucces();
            }
            else if (Regex.IsMatch(goName, BUTTON_NAME_PATTERN))
            {
                Debug.Log("Bouton appuyé");
                string[] split = goName.Split(':');
                int x = int.Parse(split[0]);
                int y = int.Parse(split[1]);
                currentImage[x, y] = !currentImage[x, y];
                hit.collider.GetComponent<MeshRenderer>().material = currentImage[x, y] ? buttonOnMaterial : buttonOffMaterial;
            }
        }
        GetComponent<Collider>().enabled = true;
    }

    /// <summary>
    /// Verifie si les boutons appuyés sont corrects
    /// </summary>
    private void CheckSucces()
    {
        for (int i = 0; i < imageSize.x; i++)
        {
            for (int j = 0; j < imageSize.y; j++)
            {
                if (currentImage[i, j] != targetRule.correctImage[i, j])
                {
                    Fail();
                    return;
                }
            }
        }
        Success();
    }

    public override void OnModuleHoldStart(Ray rayInteract)
    {
        //Rien sur ce module
    }

    public override void OnModuleHoldEnd()
    {
        //Rien sur ce module
    }

    protected override void Success()
    {
        StopCoroutine(blinkCoroutine);
        foreach (Transform button in buttons)
        {
            button.GetComponent<MeshRenderer>().material = buttonOnMaterial;
        }
        base.Success();
    }

    protected override void Fail()
    {
        foreach (Transform button in buttons)
        {
            button.GetComponent<MeshRenderer>().material = buttonOffMaterial;
        }
        base.Fail();
    }

    private IEnumerator BlinkCoroutine()
    {
        while (true)
        {
            foreach (bool isLong in blinkSequence)
            {
                lamp.GetComponent<MeshRenderer>().material = lampOnMaterial;
                yield return new WaitForSeconds(isLong ? morseTimeUnit * 3 : morseTimeUnit);
                lamp.GetComponent<MeshRenderer>().material = lampOffMaterial;
                yield return new WaitForSeconds(morseTimeUnit);
            }
            yield return new WaitForSeconds(morseTimeUnit * 2);
        }
    }
}
