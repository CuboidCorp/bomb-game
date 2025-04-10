using UnityEngine.UIElements;

[UxmlElement]
public partial class LvlCreditsElement : VisualElement
{
    private Button CloseBtn => this.Q<Button>("closeButton");
    public void Init()
    {
        CloseBtn.clicked += PcLevelSelectManager.Instance.OpenDesktop;
    }

    public LvlCreditsElement() { }
}
