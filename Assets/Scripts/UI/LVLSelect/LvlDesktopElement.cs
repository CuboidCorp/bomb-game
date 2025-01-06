using System;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

[UxmlElement]
public partial class LvlDesktopElement : VisualElement
{
    private Button Trashcan => this.Q<Button>("trashcan");
    private Button RandomRun => this.Q<Button>("randomRun");
    private Button CustomRun => this.Q<Button>("customRun");
    private Button DailyRun => this.Q<Button>("dailyRun");
    private Button Scoreboard => this.Q<Button>("scoreboard");

    public void Init()
    {
        Trashcan.clicked += GoToTrashcan;
        RandomRun.clicked += GoToRandomRun;
        CustomRun.clicked += GoToCustomRun;
        DailyRun.clicked += GoToDailyRun;
        Scoreboard.clicked += GoToScoreboard;
    }

    /// <summary>
    /// Ouvre la corbeille (NYI)
    /// </summary>
    private void GoToTrashcan()
    {
        Debug.Log("Go to trashcan");
    }

    /// <summary>
    /// Ouvre la fenetre pour lancer une run aléatoire
    /// </summary>
    private void GoToRandomRun()
    {
        RunInfoHolder runInfo = new()
        {
            seed = Random.Range(0, 1000000),
            isSeedLocked = true,
            bombTypeIndex = Random.Range(0, Enum.GetValues(typeof(BombTypes)).Length),
            isBombTypeLocked = false,
            isRandomizable = true
        };
        PcLevelSelectManager.Instance.OpenRunWindow(runInfo);
    }

    /// <summary>
    /// Ouvre la fenetre pour lancer une run custom
    /// </summary>
    private void GoToCustomRun()
    {
        RunInfoHolder runInfo = new()
        {
            seed = 0,
            isSeedLocked = false,
            bombTypeIndex = (int)BombTypes.SIX_SLOTS,
            isBombTypeLocked = false,
            isRandomizable = true
        };
        PcLevelSelectManager.Instance.OpenRunWindow(runInfo);
    }

    /// <summary>
    /// Ouvre la fenetre pour lancer la daily run
    /// </summary>
    private void GoToDailyRun()
    {
        int daySeed = int.Parse($"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}");
        Random.InitState(daySeed);

        RunInfoHolder runInfo = new()
        {
            seed = daySeed,
            isSeedLocked = true,
            bombTypeIndex = Random.Range(0, Enum.GetValues(typeof(BombTypes)).Length),
            isBombTypeLocked = true,
            isRandomizable = false
        };

        PcLevelSelectManager.Instance.OpenRunWindow(runInfo);
    }

    /// <summary>
    /// Ouvre le scoreboard
    /// </summary>
    private void GoToScoreboard()
    {
        PcLevelSelectManager.Instance.OpenScoreboardWindow();
    }

    public LvlDesktopElement() { }
}
