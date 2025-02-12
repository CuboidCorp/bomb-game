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
            new(GoToTimerModuleStep,()=>WaitUntilInputPerformed(skipTuto))
        };

        bombModules = new ModuleType[] {
            ModuleType.WIRES,
            ModuleType.BUTTON,
            ModuleType.WIRES,
            ModuleType.BUTTON,
            ModuleType.WIRES,
        };

        doc = GetComponent<UIDocument>();
        textLabel = doc.rootVisualElement.Q<Label>("tutorialText");

        //Que sont les vrais étapes du tuto
        //1- Bienvenue au tutoriel en tant qu'agent, l'objectif est de desamorcer la bombe, condition espace ou 5s
        //2- Il fait très sombre ici, allumer la flashlight , condition appuyer sur la touche pour la flashlight
        //3- Bon maintenant il faut apprendre à désamorcer une bombe , condition espace ou 5s
        //4- Spawn la bombe et dit voici la bombe que vous devez desamorcer, elle est composée de modules condition espace ou 5s
        //5- On dit qu'il faut click gauche pour zoommer sur la bombe, quand c'est fait on passe à l'étape suivante
        //6- Description du timer -condition espace ou 5s
        //7- Description d'un module et on dit assez parler il faut commencer à le desamorcer condition espace ou 5s
        //8- On decrit comment desamorcer le module condition désamorçage du module
        //8- Description d'un autre module condition espace ou 5s
        //9- Activation du timer si le timer va a 0 9bis et on revient à 9
        //9bis - Boom, nan je trolle tu vas pas mourir sur le tuto quand même
        //10- Bref avant que la bombe explose faut desamorcer ce module 
        //11- On dit un truc faux genre et il se prend un strike
        //12- Oups dsl c'était ça qu'il fallait faire et on desamorce le module
        //12 bis - Si il a juste, ahah je m'étais trompé mais faut ecouter ses potes normalement, on force le strike
        //13 - Maintenant on a un strike le temps va plus vite 
        //14 - Bref le jeu est simple communique avec ton camarade et desamorce la bombe 
        //15 - D'ailleurs j'ai trop parlé explosion, maintenant va et desamorce des bombres
    }

    private void Start()
    {
        StartCoroutine(RunTutorial());
    }

    /// <summary>
    /// Coroutine qui permet de lancer le tutoriel
    /// </summary>
    /// <returns>Quand le tutoriel est terminé</returns>
    private IEnumerator RunTutorial()
    {
        foreach (TutorialStep step in TutorialSteps) //TODO : Pas un foreach mais à la fin de chaque etape on va vers une autre etape
        {
            step.StepAction?.Invoke();

            yield return StartCoroutine(step.WaitCondition());
        }
    }

    #region Tutorials

    /// <summary>
    /// Affiche le texte du tutoriel
    /// </summary>
    /// <param name="key"></param>
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
    /// Contient les fonction pour l'étape 4 du tutoriel
    /// </summary>
    private void GenerateBombStep()
    {
        RenderText("TUTO_AGENT_4");
        MainGeneration.Instance.SetBombType(BombTypes.SIX_SLOTS);
        MainGeneration.Instance.SetModules(bombModules);
        bomb = MainGeneration.Instance.GenerateBombTutorial(bombPos, bombRot);
        bomb.GetComponent<Bomb>().SetupTutorialBomb();
    }

    /// <summary>
    /// Contient les fonctions pour l'étape 5 du tutoriel
    /// </summary>
    private void GoToTimerModuleStep()
    {
        //Faudrait zoomer sur le module du timer
        RenderText("TUTO_AGENT_6");

    }

    #endregion

    /// <summary>
    /// Affiche le texte progressivement
    /// </summary>
    /// <param name="text">Le texte à afficher</param>
    /// <returns>Quand le texte est affiché entierement</returns>
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
    /// <param name="seconds">Le nombre de secondes à attendre</param>
    /// <returns>Quand le timer est fini</returns>
    IEnumerator WaitForSeconds(float seconds)
    {
        //On attend que le texte soit render
        yield return new WaitUntil(() => isTextDisplayed);
        yield return new WaitForSeconds(seconds);
    }

    IEnumerator WaitForAnyCondition(params Func<bool>[] conditions)
    {
        while (!conditions.Any(condition => condition()))
        {
            yield return null; // attendre la frame suivante bn
        }
    }

    IEnumerator WaitForBombToBeClose()
    {
        yield return new WaitUntil(() => isTextDisplayed);

        //On vérifie si la bombe est grabbed dans le bombinteract
        yield return new WaitUntil(() => bombInteract.IsBombGrabbed());
    }

    /// <summary>
    /// Attend qu'un input soit performé (appuyé)
    /// </summary>
    /// <param name="action">L'input qu'on attend</param>
    /// <returns>Quand l'input a été appelé</returns>
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

    #endregion
}
