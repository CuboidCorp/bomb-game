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
    [SerializeField] private VisualTreeAsset desktopWindow;
    [SerializeField] private VisualTreeAsset manualWindow;
    [SerializeField] private VisualTreeAsset calcWindow;

    private UIDocument doc;

    private const int startupTime = 5;

    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        Instance = this;
    }

    public void StartComputer()
    {
        Debug.Log("Starting computer");
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
        doc.panelSettings.SetScreenToPanelSpaceFunction(null);
    }

    private void OpenDesktop()
    {
        doc.visualTreeAsset = desktopWindow;
        SetupDoc();
    }

    private IEnumerator WaitForStartup()
    {
        yield return new WaitForSeconds(startupTime);
        OpenDesktop();
    }

    private Vector2 ScreenToPanelSpaceFunction(Vector2 screenPosition)
    {
        Vector2 invalidPosition = new(float.NaN, float.NaN);

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, 10f, LayerMask.GetMask("UI")))
        {
            Debug.Log("Invalid position");
            return invalidPosition;
        }

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
