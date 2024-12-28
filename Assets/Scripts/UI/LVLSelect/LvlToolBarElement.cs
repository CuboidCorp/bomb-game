using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class LvlToolBarElement : VisualElement
{
#nullable enable
    private ComputerLocation computerLocation;

    private PcLevelSelectManager pcUiManager;

    private const string selectedBtnClass = "selected-task-bar-item";

    private Button HomeButton => this.Q<Button>("mainMenuBtn");

    public void Init()
    {
        pcUiManager = PcLevelSelectManager.Instance;
        computerLocation = ComputerLocation.HOME;
        HomeButton.clicked += GoToDesktop;
    }

    private void GoToDesktop()
    {
        Debug.Log("Go to desktop");
        computerLocation = ComputerLocation.HOME;
        pcUiManager.OpenDesktop();
    }

    public LvlToolBarElement()
    {
        pcUiManager = PcLevelSelectManager.Instance;
    }

}
