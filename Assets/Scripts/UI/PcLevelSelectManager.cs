using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PcLevelSelectManager : PcUiManager
{
    public static PcLevelSelectManager Instance;

    [SerializeField] private VisualTreeAsset runWindow;
    [SerializeField] private VisualTreeAsset scoreboardWindow;
    [SerializeField] private VisualTreeAsset creditsWindow;

    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        ClearRenderTexture();
        Instance = this;
        InstanceAbs = this;
    }


    /// <summary>
    /// Ouvre le bureau
    /// </summary>
    public override void OpenDesktop()
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
    /// Ouvre les crédits
    /// </summary>
    public void OpenCreditsWindow()
    {
        screenHolder.Clear();
        screenHolder.Add(creditsWindow.CloneTree());
        doc.rootVisualElement.Q<LvlCreditsElement>().Init();
    }

    /// <summary>
    /// Ouvre le tutoriel
    /// </summary>
    public void GoToTutorial()
    {
        UnSetupDoc();
        SceneManager.LoadScene("TutoAgent");
    }

    /// <summary>
    /// Setup la barre de taches
    /// </summary>
    protected override void SetupToolBar()
    {
        LvlToolBarElement toolbar = doc.rootVisualElement.Q<LvlToolBarElement>();
        toolbar.Init();
    }


    /// <summary>
    /// Génère la main generation qui permet aux données de passer à travers les scènes
    /// </summary>
    /// <param name="seed">Le seed de la game</param>
    /// <param name="bombType">Le type de la bombe</param>
    public void GenerateDataHolder(int seed, BombTypes bombType)
    {
        GameObject mainGen = Instantiate(Resources.Load<GameObject>("MainGen"));

        mainGen.GetComponent<MainGeneration>().SetSeed(seed);
        mainGen.GetComponent<MainGeneration>().SetBombType(bombType);
    }
}

