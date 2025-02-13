using System.Collections;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class AgentTutoHandler : MonoBehaviour
{
    #region BombGeneration
    private ModuleType[] bombModules;
    [SerializeField] private Vector3 bombPos;
    [SerializeField] private Vector3 bombRot;
    public GameObject bomb;
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
            new(StartBombTimerStep,()=>WaitForBombExplosionOrDefusal())
        };

        bombModules = new ModuleType[] { //TODO : Mettre un seul bouton et le reste c'est le module vide
            ModuleType.BUTTON,
            ModuleType.BUTTON,
            ModuleType.BUTTON,
            ModuleType.BUTTON,
            ModuleType.BUTTON,
        };

        doc = GetComponent<UIDocument>();
        textLabel = doc.rootVisualElement.Q<Label>("tutorialText");

        //Que sont les vrais �tapes du tuto
        //1- Bienvenue au tutoriel en tant qu'agent, l'objectif est de desamorcer la bombe, condition espace ou 5s
        //2- Il fait tr�s sombre ici, allumer la flashlight , condition appuyer sur la touche pour la flashlight
        //3- Bon maintenant il faut apprendre � d�samorcer une bombe , condition espace ou 5s
        //4- Spawn la bombe et dit voici la bombe que vous devez desamorcer, elle est compos�e de modules condition espace ou 5s
        //5- On dit qu'il faut click gauche pour zoommer sur la bombe, quand c'est fait on passe � l'�tape suivante
        //6- Description du timer -condition espace ou 5s
        //7- Description du module gros bouton
        //8- On fake decrit comment desamorcer le module gros bouton on va sur 9
        //TODO : trouver un seed pr pas que ce soit possible la condition genre faut que le timer ait un 5
        //9- On lance le timer et on donne la solution
        //9bis - Si il attend la fin du timer on le remet à 0 et on repart sur l'étape 10
        //10- Fin du tuto on dit gg, faut pas oublier l'objectif c'est de communiquer avec l'expert bisous.
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
        MainGeneration.Instance.SetSeed(0);
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
        //TODO : Faudrait zoomer sur le module du timer
        RenderText("TUTO_AGENT_6");

    }


    private void GoToButtonModuleStep()
    {
        //TODO : Zoom vers le module du gros bouton
        RenderText("TUTO_AGENT_7");
    }

    private void FakeDefuseStep()
    {
        //TODO : Activer le collider du gros bouton
        RenderText("TUTO_AGENT_8");
    }

    private void StartBombTimerStep()
    {
        //TODO : Activer le timer, le lancer et reactiver le collider du gros bouton
        RenderText("TUTO_AGENT_10");
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

    #region Coroutines Wait



    /// <summary>
    /// Attends un certain nombre de secondes
    /// </summary>
    /// <param name="seconds">Le nombre de secondes � attendre</param>
    /// <returns>Quand le timer est fini</returns>
    IEnumerator WaitForSeconds(float seconds)
    {
        //On attend que le texte soit render
        yield return new WaitUntil(() => isTextDisplayed);
        yield return new WaitForSeconds(seconds);
    }

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
        //Si bomb explosion recursivité, sinon fin du tuto
        yield return null;
    }

    IEnumerator WaitForBombExplosion()
    {
        yield return null;
    }

    IEnumerator WaitForBombToBeDefused()
    {
        yield return null;
    }

    IEnumerator WaitForStrike()
    {
        yield return null;
        //TODO : Desactivation du module du gros bouton
    }
    #endregion
}
