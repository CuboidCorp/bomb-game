using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

[UxmlElement]
public partial class LvlRunElement : VisualElement
{
    private Button CloseBtn => this.Q<Button>("closeButton");
    private IntegerField SeedFied => this.Q<IntegerField>("seedField");

    private Button CopySeedBtn => this.Q<Button>("copySeedBtn");

    private Button RandomizeSeedButton => this.Q<Button>("randomizeSeedBtn");

    private EnumField NbModules => this.Q<EnumField>("bombType");

    private Button RandomizeModulesButton => this.Q<Button>("randomizeModulesBtn");

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


        if (!runInfo.isRandomizable)
        {
            RandomizeSeedButton.SetEnabled(false);
            RandomizeModulesButton.SetEnabled(false);
        }
        else
        {
            RandomizeSeedButton.clicked += RandomizeSeed;
            RandomizeModulesButton.clicked += RandomizeModules;
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
        int seed = SeedFied.value;
        Debug.Log("Seed copied : " + seed);

        GUIUtility.systemCopyBuffer = seed.ToString();
    }

    /// <summary>
    /// Lance le debut de la game en tant qu'operateur
    /// </summary>
    private void StartGameOperateur()
    {
        PcLevelSelectManager.Instance.GenerateDataHolder(SeedFied.value, (BombTypes)NbModules.value);
        PcLevelSelectManager.Instance.UnSetupDoc();
        SceneManager.LoadScene("Operator");
    }

    /// <summary>
    /// Lance le debut de la game en tant qu'agent
    /// </summary>
    private void StartGameAgent()
    {
        PcLevelSelectManager.Instance.GenerateDataHolder(SeedFied.value, (BombTypes)NbModules.value);
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

    /// <summary>
    /// Randomize le nombre de modules
    /// </summary>
    private void RandomizeModules()
    {
        NbModules.value = (BombTypes)Random.Range(0, Enum.GetValues(typeof(BombTypes)).Length);
    }


    public LvlRunElement() { }
}
