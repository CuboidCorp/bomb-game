using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class AgentTutoHandler : MonoBehaviour
{
    #region BombGeneration
    private const int BOMB_SEED = 8;
    private ModuleType[] bombModules;
    [SerializeField] private Vector3 bombPos;
    [SerializeField] private Vector3 bombRot;
    public GameObject bomb;

    private static readonly Vector3 basePos = new Vector3(-50, 5.6f, 7);
    private static readonly Vector3 timerPos = new Vector3(-50, 6.1f, 6.5f);
    private static readonly Vector3 buttonPos = new Vector3(-49, 6.1f, 6.5f);
    #endregion

    private PlayerControls playerControls;
    private InputAction skipTuto;
    private InputAction flashLight;

    private TutorialStep[] TutorialSteps;

    private UIDocument doc;
    private Label textLabel;
    private bool isTextDisplayed = false;

    public bool isDebug = false;

    [SerializeField] private BombInteract bombInteract;

    private const float TIME_BETWEEN_LETTERS = 0.1f;

    private const string LOCALIZATION_TUTO_TABLE = "TutorialText";

    private void OnEnable()
    {
        playerControls = new PlayerControls();
        playerControls.Enable();
        skipTuto = playerControls.Player.AdvanceTuto;
        flashLight = playerControls.Player.Flashlight;
        skipTuto.Enable();
        flashLight.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
        flashLight.Disable();
        skipTuto.Disable();
    }

    private void Awake()
    {
        TutorialSteps = new TutorialStep[]
        {
            new(()=>RenderText("TUTO_AGENT_1"),()=>WaitUntilInputPerformed(skipTuto)),
            new(()=>RenderText("TUTO_AGENT_2"),()=>WaitUntilInputPerformed(flashLight)),
            new(()=>RenderText("TUTO_AGENT_3"),()=>WaitUntilInputPerformed(skipTuto)),
            new(GenerateBombStep,()=>WaitUntilInputPerformed(skipTuto)),
            new(()=>RenderText("TUTO_AGENT_5"),()=>WaitForBombToBeClose()),
            new(GoToTimerModuleStep,()=>WaitUntilInputPerformed(skipTuto)),
            new(GoToButtonModuleStep,()=>WaitUntilInputPerformed(skipTuto)),
            new(FakeDefuseStep,()=>WaitForStrike()),
            new(()=>RenderText("TUTO_AGENT_9"),()=>WaitUntilInputPerformed(skipTuto)),
            new(()=>RenderText("TUTO_AGENT_10"),()=>WaitUntilInputPerformed(skipTuto)),
            new(StartBombTimerStep,()=>WaitForBombExplosionOrDefusal())
        };

        bombModules = new ModuleType[] {
            ModuleType.BUTTON,
            ModuleType.EMPTY,
            ModuleType.EMPTY,
            ModuleType.EMPTY,
            ModuleType.EMPTY,
        };

        doc = GetComponent<UIDocument>();
        textLabel = doc.rootVisualElement.Q<Label>("tutorialText");
    }

    private void Start()
    {
        StartCoroutine(RunTutorial());
    }

    /// <summary>
    /// Coroutine qui permet de lancer le tutoriel
    /// </summary>
    /// <returns>Quand le tutoriel est termin�</returns>
    private IEnumerator RunTutorial()
    {
        foreach (TutorialStep step in TutorialSteps) //TODO : Pas un foreach mais � la fin de chaque etape on va vers une autre etape
        {
            step.StepAction?.Invoke();

            yield return StartCoroutine(step.WaitCondition());
        }
    }

    #region Tutorials

    /// <summary>
    /// Affiche le texte du tutoriel
    /// </summary>
    /// <param name="key">La clé pour charger le texte dans la table de traduction</param>
    private void RenderText(string key)
    {
        string text = TextLocalizationHandler.LoadString(LOCALIZATION_TUTO_TABLE, key);
        if (isDebug)
        {
            isTextDisplayed = true;
            textLabel.text = text;
        }
        else
        {
            StartCoroutine(DisplayProgressiveText(text));
        }

    }

    /// <summary>
    /// Contient les fonction pour l'�tape 4 du tutoriel
    /// </summary>
    private void GenerateBombStep()
    {
        MainGeneration.Instance.SetSeed(BOMB_SEED);
        RenderText("TUTO_AGENT_4");
        MainGeneration.Instance.SetBombType(BombTypes.SIX_SLOTS);
        MainGeneration.Instance.SetModules(bombModules);
        bomb = MainGeneration.Instance.GenerateBombTutorial(bombPos, bombRot);
        bomb.GetComponent<Bomb>().SetupTutorialBomb();
    }

    /// <summary>
    /// Contient les fonctions pour l'�tape 6 du tutoriel
    /// </summary>
    private void GoToTimerModuleStep()
    {
        StartCoroutine(MoveCameraTo(basePos, timerPos));
        RenderText("TUTO_AGENT_6");
    }


    private void GoToButtonModuleStep()
    {
        StartCoroutine(MoveCameraTo(timerPos, buttonPos));
        RenderText("TUTO_AGENT_7");
    }

    private void FakeDefuseStep()
    {
        bomb.GetComponent<Bomb>().EnableCollider();
        RenderText("TUTO_AGENT_8");
    }

    private void StartBombTimerStep()
    {
        bomb.GetComponent<Bomb>().EnableCollider();
        bomb.GetComponent<Bomb>().StartTutorialBomb();
        RenderText("TUTO_AGENT_11");
    }

    #endregion

    /// <summary>
    /// Affiche le texte progressivement
    /// </summary>
    /// <param name="text">Le texte � afficher</param>
    /// <returns>Quand le texte est affich� entierement</returns>
    IEnumerator DisplayProgressiveText(string text)
    {
        textLabel.text = "";
        isTextDisplayed = false;
        for (int i = 0; i < text.Length; i++)
        {
            textLabel.text += text[i];
            if (text[i] != ' ' && text[i] != '\n' && text[i] != '.' && text[i] != ',')
                yield return new WaitForSeconds(TIME_BETWEEN_LETTERS);
        }
        isTextDisplayed = true;
    }

    /// <summary>
    /// Déplace la caméra de from à to
    /// </summary>
    /// <param name="from">Point de départ</param>
    /// <param name="to">Point d'arrivée</param>
    /// <returns>Quand le déplacement est terminé </returns>
    IEnumerator MoveCameraTo(Vector3 from, Vector3 to)
    {
        float duration = 1.0f; // Durée du déplacement en secondes
        float elapsedTime = 0f;

        Camera.main.transform.position = from;

        while (elapsedTime < duration)
        {
            Camera.main.transform.position = Vector3.Lerp(from, to, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Camera.main.transform.position = to;
    }

    #region Coroutines Wait

    /// <summary>
    /// Attends que la bombe soit proche
    /// </summary>
    /// <returns>Quand la bombe est proche</returns>
    IEnumerator WaitForBombToBeClose()
    {
        yield return new WaitUntil(() => isTextDisplayed);

        //On v�rifie si la bombe est grabbed dans le bombinteract
        yield return new WaitUntil(() => bombInteract.IsBombGrabbed());
    }

    /// <summary>
    /// Attend qu'un input soit perform� (appuy�)
    /// </summary>
    /// <param name="action">L'input qu'on attend</param>
    /// <returns>Quand l'input a �t� appel�</returns>
    public IEnumerator WaitUntilInputPerformed(InputAction action)
    {
        bool inputReceived = false;

        void onPerformed(InputAction.CallbackContext ctx) => inputReceived = true;

        //On attend que le texte soit render
        yield return new WaitUntil(() => isTextDisplayed);

        action.performed += onPerformed;

        yield return new WaitUntil(() => inputReceived);

        action.performed -= onPerformed;
    }

    IEnumerator WaitForBombExplosionOrDefusal()
    {
        yield return new WaitUntil(() => isTextDisplayed);
        //Si bomb explosion recursivité, sinon fin du tuto
        yield return new WaitUntil(() => bomb.GetComponent<Bomb>().isBombDefused || bomb.GetComponent<Bomb>().isBombExploded);

        if (bomb.GetComponent<Bomb>().isBombDefused)
        {
            //Fin du tuto
            RenderText("TUTO_AGENT_12");

            yield return new WaitUntil(() => isTextDisplayed);

            yield return StartCoroutine(WaitUntilInputPerformed(skipTuto));

            SceneManager.LoadScene("LevelSelect");
        }
        else
        {
            //La bombe a explosé
            RenderText("TUTO_AGENT_11_ALT");
            yield return new WaitUntil(() => isTextDisplayed);

            StartBombTimerStep();

            yield return StartCoroutine(WaitForBombExplosionOrDefusal()); //Recursivité pas ouf ptet infini mais bon faut pas troll aussi
        }

    }

    /// <summary>
    /// Attend que le nombre de strikes soit supérieur � 0
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForStrike()
    {
        yield return new WaitUntil(() => isTextDisplayed);
        yield return new WaitUntil(() => bomb.GetComponent<Bomb>().GetNbStrikes() >= 1);
        bomb.GetComponent<Bomb>().DisableCollider();
    }
    #endregion
}
