using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class ToolBarElement : VisualElement
{
    private ComputerLocation computerLocation;

    private Button homeButton => this.Q<Button>("mainMenuBtn");
    private Button manualButton => this.Q<Button>("manualBtn");
    private Button calculatorButton => this.Q<Button>("calcBtn");

    public void Init()
    {
        Debug.Log("Inut toolbar");
        computerLocation = ComputerLocation.HOME;
        homeButton.clicked += GoToDesktop;
        manualButton.clicked += GoToManual;
        calculatorButton.clicked += GoToCalculator;
    }

    private void GoToDesktop()
    {
        Debug.Log("Go to desktop");
        computerLocation = ComputerLocation.HOME;
        PcUiManager.Instance.OpenDesktop();
    }

    private void GoToManual()
    {
        Debug.Log("Go to manual");
        computerLocation = ComputerLocation.MANUAL;
        PcUiManager.Instance.OpenManual();
    }

    private void GoToCalculator()
    {
        Debug.Log("Go to calculator");
        computerLocation = ComputerLocation.CALCULATOR;
        PcUiManager.Instance.OpenCalculator();
    }

    public ToolBarElement() { }

}
