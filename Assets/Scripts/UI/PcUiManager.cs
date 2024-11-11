using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PcUiManager : MonoBehaviour
{
    private bool isStarted = false;
    private bool hasLoggedIn = false;
    public static PcUiManager Instance;

    [Header("Windows")]
    [SerializeField] private VisualTreeAsset startWindow;
    [SerializeField] private VisualTreeAsset mainScreenWindow;

    private VisualElement screenHolder;

    [Header("Apps")]
    [SerializeField] private VisualTreeAsset desktopWindow;
    [SerializeField] private VisualTreeAsset manualWindow;
    [SerializeField] private VisualTreeAsset calcWindow;

    [Header("Manual")]
    [SerializeField] private VisualTreeAsset introManual;

    private UIDocument doc;

    private const int startupTime = 5;

    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        Instance = this;
    }

    public void StartComputer()
    {
        if (isStarted)
        {
            return;
        }
        isStarted = true;
        if (hasLoggedIn)
        {
            SetupDoc();
        }
        else
        {
            Debug.Log("Starting computer");
            hasLoggedIn = true;
            doc.visualTreeAsset = startWindow;
            //TODO : Bruit de démarrage
            StartCoroutine(WaitForStartup());
        }

    }

    private void SetupDoc()
    {
        doc.panelSettings.SetScreenToPanelSpaceFunction(ScreenToPanelSpaceFunction);
    }

    public void UnSetupDoc()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        UnityEngine.Cursor.visible = true;
#endif
        isStarted = false;
        doc.panelSettings.SetScreenToPanelSpaceFunction(null);
    }

    private void OnComputerLoggedIn()
    {
        doc.visualTreeAsset = mainScreenWindow;
        screenHolder = doc.rootVisualElement.Q("screenHolder");
        SetupToolBar();
        OpenDesktop();
    }

    public void OpenDesktop()
    {
        screenHolder.Clear();
        screenHolder.Add(desktopWindow.CloneTree());
    }

    public void OpenManual()
    {
        screenHolder.Clear();
        screenHolder.Add(manualWindow.CloneTree());
        VisualElement[] modules = ManuelSquad.Instance.GetModuleRules();
        VisualElement main = doc.rootVisualElement.Q<VisualElement>("scrollManualZone");
        //On rajoute l'intro en premier
        VisualElement intro = introManual.CloneTree();
        intro.Q<IntroElement>().Init();
        main.Add(intro);
        foreach (VisualElement module in modules)
        {
            main.Add(module);
        }
    }

    public void OpenCalculator()
    {
        screenHolder.Clear();
        screenHolder.Add(desktopWindow.CloneTree());
    }

    private void SetupToolBar()
    {
        ToolBarElement toolbar = doc.rootVisualElement.Q<ToolBarElement>();
        toolbar.Init();
    }

    private IEnumerator WaitForStartup()
    {
        yield return new WaitForSeconds(startupTime);
        SetupDoc();
        OnComputerLoggedIn();
    }

    private Vector2 ScreenToPanelSpaceFunction(Vector2 screenPosition)
    {
        Vector2 invalidPosition = new(float.NaN, float.NaN);

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Debug.DrawRay(ray.origin, ray.direction * 5, Color.red, 3f);

        if (!Physics.Raycast(ray, out RaycastHit hit, 5, LayerMask.GetMask("UI")))
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            UnityEngine.Cursor.visible = true;
#endif
            //Debug.Log("Invalid position");
            return invalidPosition;
        }

#if UNITY_STANDALONE || UNITY_EDITOR
        UnityEngine.Cursor.visible = false;
#endif

        Vector2 pixelUV = hit.textureCoord;

        pixelUV.x *= doc.panelSettings.targetTexture.width;
        pixelUV.y *= doc.panelSettings.targetTexture.height;

        VisualElement cursor = doc.rootVisualElement.Q("cursor");

        if (cursor != null)
        {
            cursor.style.left = pixelUV.x - 16;
            cursor.style.top = pixelUV.y - 16;
        }


        return pixelUV;
    }

}
