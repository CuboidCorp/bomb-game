using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class ClassicPopup : VisualElement
{
    private Button closeBtn => this.Q<Button>("closeBtn");
}
