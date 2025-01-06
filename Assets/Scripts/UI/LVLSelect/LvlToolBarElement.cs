using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class LvlToolBarElement : VisualElement
{
#nullable enable

    private PcLevelSelectManager pcUiManager;

    private Button HomeButton => this.Q<Button>("mainMenuBtn");

    public void Init()
    {
        pcUiManager = PcLevelSelectManager.Instance;
        HomeButton.clicked += GoToDesktop;
    }

    private void GoToDesktop()
    {
        Debug.Log("Go to desktop");
        pcUiManager.OpenDesktop();
    }

    public LvlToolBarElement()
    {
        pcUiManager = PcLevelSelectManager.Instance;
    }

}
