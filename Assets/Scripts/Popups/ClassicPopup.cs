using UnityEngine.UIElements;

[UxmlElement]
public partial class ClassicPopup : VisualElement
{
    private Button CloseBtn => this.Q<Button>("closeBtn");

    private Label MainText => this.Q<Label>("mainText");
    private Label SubText => this.Q<Label>("subText");

    private Button Btn1 => this.Q<Button>("btn1");
    private Button Btn2 => this.Q<Button>("btn2");

    public void Init(PopupData data)
    {
        MainText.text = data.mainText.GetLocalizedString();
        SubText.text = data.subText.GetLocalizedString();

        CloseBtn.style.display = data.isCloseBtnVisible ? DisplayStyle.Flex : DisplayStyle.None;

        if (data.nbButtons > 0)
        {
            Btn1.text = data.buttonsText[0].GetLocalizedString();
            Btn1.clicked += data.buttonsActions[0];

            if (data.nbButtons > 1)
            {
                Btn2.text = data.buttonsText[1].GetLocalizedString();
                Btn2.clicked += data.buttonsActions[1];
            }
            else
            {
                Btn2.style.display = DisplayStyle.None;
            }
        }
        else
        {
            Btn1.style.display = DisplayStyle.None;
            Btn2.style.display = DisplayStyle.None;
        }
    }

    public ClassicPopup() { }
}
