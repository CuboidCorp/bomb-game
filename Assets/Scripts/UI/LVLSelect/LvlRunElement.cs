using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[UxmlElement]
public partial class LvlRunElement : VisualElement
{
    private Button CloseBtn => this.Q<Button>("closeButton");
    private IntegerField SeedFied => this.Q<IntegerField>("seedField");

    private Button CopySeedBtn => this.Q<Button>("copyBtn");

    private Button RandomizeButton => this.Q<Button>("randomizeBtn");

    private EnumField NbModules => this.Q<EnumField>("bombType");

    private Button StartOperateur => this.Q<Button>("playAsOp");
    private Button StartAgent => this.Q<Button>("playAsAgent");

    public void Init(RunInfoHolder runInfo)
    {
        CloseBtn.clicked += PcLevelSelectManager.Instance.OpenDesktop;
        SeedFied.value = runInfo.seed;
        if (runInfo.isSeedLocked)
        {
            SeedFied.SetEnabled(false);
        }
        CopySeedBtn.clicked += CopySeed;
        RandomizeButton.clicked += RandomizeSeed;

        if (!runInfo.isRandomizable)
        {
            RandomizeButton.SetEnabled(false);
        }

        NbModules.Init((BombTypes)runInfo.bombTypeIndex);
        if (runInfo.isBombTypeLocked)
        {
            NbModules.SetEnabled(false);
        }

        StartOperateur.clicked += StartGameOperateur;
        StartAgent.clicked += StartGameAgent;

    }

    /// <summary>
    /// Copie le seed dans le presse papier
    /// </summary>
    private void CopySeed()
    {
        //TODO : Copier le seed dans le presse papier
    }

    /// <summary>
    /// Lance le debut de la game en tant qu'operateur
    /// </summary>
    private void StartGameOperateur()
    {
        PcLevelSelectManager.Instance.GenerateSeedHolder(SeedFied.value);
        PcLevelSelectManager.Instance.UnSetupDoc();
        SceneManager.LoadScene("Operator");
    }

    /// <summary>
    /// Lance le debut de la game en tant qu'agent
    /// </summary>
    private void StartGameAgent()
    {
        PcLevelSelectManager.Instance.GenerateSeedHolder(SeedFied.value);
        PcLevelSelectManager.Instance.UnSetupDoc();
        SceneManager.LoadScene("Agent");
    }

    /// <summary>
    /// Randomize le seed
    /// </summary>
    private void RandomizeSeed()
    {
        SeedFied.value = Random.Range(0, 1000000);
    }


    public LvlRunElement() { }
}
