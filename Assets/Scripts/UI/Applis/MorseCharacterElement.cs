using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class MorseCharacterElement : VisualElement
{
    private Label labelChara => this.Q<Label>("labelChara");
    private VisualElement signalHolder => this.Q("signalHolder");

    private const string LONG_SPRITE_PATH = "Textures/MorseModule/LongSprite";
    private const string SHORT_SPRITE_PATH = "Textures/MorseModule/ShortSprite";
    private const string IMAGE_CLASS = "morse-signal";

    private Sprite longSprite;
    private Sprite shortSprite;

    public void Init(char caractere, bool[] signaux)
    {
        longSprite = Resources.Load<Sprite>(LONG_SPRITE_PATH);
        shortSprite = Resources.Load<Sprite>(SHORT_SPRITE_PATH);
        labelChara.text = caractere.ToString();
        for (int i = 0; i < signaux.Length; i++)
        {
            VisualElement signal = new VisualElement();
            signal.AddToClassList(IMAGE_CLASS);
            signal.style.backgroundImage = signaux[i] ? longSprite.texture : shortSprite.texture;
            signalHolder.Add(signal);
        }
    }
}