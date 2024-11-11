using UnityEngine.UIElements;

[UxmlElement]
public partial class IntroElement : VisualElement
{
    private Label title => this.Q<Label>("titre");
    private Label desc1 => this.Q<Label>("desc1");

    private Label desc2 => this.Q<Label>("desc2");

    public void Init()
    {
        title.text = TextFR.INTRO_TITLE;
        desc1.text = TextFR.INTRO_DESC_1;
        desc2.text = TextFR.INTRO_DESC_2;
    }

    public IntroElement() { }

}
