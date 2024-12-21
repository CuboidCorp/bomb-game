using System.Collections.Generic;
using UnityEngine.UIElements;

[UxmlElement]
public partial class MorseAppliElement : VisualElement
{
    private VisualElement characterHolder => this.Q("characterHolder");

    public void Init(Dictionary<char, bool[]> morseAlphabet, VisualTreeAsset morseCharaTemplate)
    {
        foreach (var morseChar in morseAlphabet)
        {
            VisualElement characterVisu = morseCharaTemplate.CloneTree();
            characterVisu.Q<MorseCharacterElement>().Init(morseChar.Key, morseChar.Value);
            characterHolder.Add(characterVisu);
        }
    }

    public MorseAppliElement() { }
}
