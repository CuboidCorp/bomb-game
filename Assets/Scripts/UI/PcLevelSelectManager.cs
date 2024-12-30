using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PcLevelSelectManager : MonoBehaviour
{
    private bool isStarted = false;
    private bool hasLoggedIn = false;
    public static PcLevelSelectManager Instance;

    [Header("Windows")]
    [SerializeField] private VisualTreeAsset startWindow;
    [SerializeField] private VisualTreeAsset mainScreenWindow;

    private VisualElement screenHolder;

    [Header("Apps")]
    [SerializeField] private VisualTreeAsset desktopWindow;
    [SerializeField] private VisualTreeAsset runWindow;
    [SerializeField] private VisualTreeAsset scoreboardWindow;

    private UIDocument doc;

    private const int startupTime = 1;

    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        Instance = this;
    }


    /// <summary>
    /// Lance le pc si il n'est pas lancé
    /// </summary>
    public void StartComputer()
    {
        if (isStarted)
        {
            return;
        }
        isStarted = true;
        if (hasLoggedIn)
        {
            SetupDoc();
        }
        else
        {
            Debug.Log("Starting computer");
            hasLoggedIn = true;
            doc.visualTreeAsset = startWindow;
            //TODO : Bruit de d�marrage
            StartCoroutine(WaitForStartup());
        }

    }

    /// <summary>
    /// Met en place la fonction qui permet d'intéragir avec le pc
    /// </summary>
    private void SetupDoc()
    {
        doc.panelSettings.SetScreenToPanelSpaceFunction(ScreenToPanelSpaceFunction);
    }

    /// <summary>
    /// Desactive la fonction d'intéraction avec le pc et eteint l'ordinateur
    /// </summary>
    public void UnSetupDoc()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        UnityEngine.Cursor.visible = true;
#endif
        isStarted = false;
        doc.panelSettings.SetScreenToPanelSpaceFunction(null);
    }

    private void OnComputerLoggedIn()
    {
        doc.visualTreeAsset = mainScreenWindow;
        screenHolder = doc.rootVisualElement.Q("screenHolder");
        SetupToolBar();
        OpenDesktop();
    }

    /// <summary>
    /// Ouvre le bureau
    /// </summary>
    public void OpenDesktop()
    {
        screenHolder.Clear();
        screenHolder.Add(desktopWindow.CloneTree());
        doc.rootVisualElement.Q<LvlDesktopElement>().Init();
    }

    /// <summary>
    /// Ouvre le menu de lancement
    /// </summary>
    public void OpenRunWindow(RunInfoHolder runInfo)
    {
        screenHolder.Clear();
        screenHolder.Add(runWindow.CloneTree());
        doc.rootVisualElement.Q<LvlRunElement>().Init(runInfo);
    }

    /// <summary>
    /// Ouvre le tableau des scores
    /// </summary>
    public void OpenScoreboardWindow()
    {
        screenHolder.Clear();
        screenHolder.Add(scoreboardWindow.CloneTree());
    }

    /// <summary>
    /// Setup la barre de taches
    /// </summary>
    private void SetupToolBar()
    {
        LvlToolBarElement toolbar = doc.rootVisualElement.Q<LvlToolBarElement>();
        toolbar.Init();
    }

    /// <summary>
    /// Allume l'ordinateur après un temps de chargement
    /// </summary>
    /// <returns>Quand le chargement est terminé</returns>
    private IEnumerator WaitForStartup()
    {
        yield return new WaitForSeconds(startupTime);
        SetupDoc();
        OnComputerLoggedIn();
    }

    public void GenerateSeedHolder(int seed)
    {
        GameObject mainGen = Instantiate(Resources.Load<GameObject>("MainGen"));

        mainGen.GetComponent<MainGeneration>().SetSeed(seed);
    }

    /// <summary>
    /// Convertit une position sur l'écran (le vrai) sur une position sur le panel qui represente l'écran de l'ordinateur
    /// </summary>
    /// <param name="screenPosition">La position sur l'écran</param>
    /// <returns>La position sur le panel</returns>
    private Vector2 ScreenToPanelSpaceFunction(Vector2 screenPosition)
    {
        Vector2 invalidPosition = new(float.NaN, float.NaN);

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Debug.DrawRay(ray.origin, ray.direction * 5, Color.red, 3f);

        if (!Physics.Raycast(ray, out RaycastHit hit, 5, LayerMask.GetMask("UI")))
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            UnityEngine.Cursor.visible = true;
#endif
            //Debug.Log("Invalid position");
            return invalidPosition;
        }

#if UNITY_STANDALONE || UNITY_EDITOR
        UnityEngine.Cursor.visible = false;
#endif

        Vector2 pixelUV = hit.textureCoord;

        pixelUV.x *= doc.panelSettings.targetTexture.width;
        pixelUV.y *= doc.panelSettings.targetTexture.height;

        VisualElement cursor = doc.rootVisualElement.Q("cursor");

        if (cursor != null)
        {
            cursor.style.left = pixelUV.x - 16;
            cursor.style.top = pixelUV.y - 16;
        }


        return pixelUV;
    }
}

