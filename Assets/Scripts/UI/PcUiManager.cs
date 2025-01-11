using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class PcUiManager : MonoBehaviour
{
    protected bool isStarted = false;
    protected bool hasLoggedIn = false;
    public static PcUiManager InstanceAbs;

    protected UIDocument doc;
    protected VisualElement screenHolder;

    [SerializeField] private int startupTime = 5;

    [SerializeField] protected RenderTexture screenTexture;

    [Header("Windows")]
    [SerializeField] protected VisualTreeAsset startWindow;
    [SerializeField] protected VisualTreeAsset mainScreenWindow;

    [Header("Apps")]
    [SerializeField] protected VisualTreeAsset desktopWindow;
    //Les autres pr les implementations

    //A Copier pr les implementations
    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        InstanceAbs = this;
    }

    /// <summary>
    /// Lance le pc si il n'est pas lancé
    /// </summary>
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
            //TODO : Bruit de d�marrage
            StartCoroutine(WaitForStartup());
        }

    }

    /// <summary>
    /// Met en place la fonction qui permet d'intéragir avec le pc
    /// </summary>
    private void SetupDoc()
    {
        doc.panelSettings.SetScreenToPanelSpaceFunction(ScreenToPanelSpaceFunction);
    }

    /// <summary>
    /// Setup le pc si il a déjà été lancé
    /// </summary>
    public void SetupIfStarted()
    {
        if (isStarted)
        {
            SetupDoc();
        }
    }

    /// <summary>
    /// Desactive la fonction d'intéraction avec le pc et eteint l'ordinateur
    /// </summary>
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

    /// <summary>
    /// Clear la texture de rendu
    /// </summary>
    protected void ClearRenderTexture()
    {
        screenTexture.Release();
    }

    /// <summary>
    /// Ouvre le bureau
    /// </summary>
    public virtual void OpenDesktop()
    {
        screenHolder.Clear();
        screenHolder.Add(desktopWindow.CloneTree());
    }

    /// <summary>
    /// Setup la barre de taches
    /// </summary>
    protected abstract void SetupToolBar();

    /// <summary>
    /// Allume l'ordinateur après un temps de chargement
    /// </summary>
    /// <returns>Quand le chargement est terminé</returns>
    private IEnumerator WaitForStartup()
    {
        yield return new WaitForSeconds(startupTime);
        SetupDoc();
        OnComputerLoggedIn();
    }

    /// <summary>
    /// Convertit une position sur l'écran (le vrai) sur une position sur le panel qui represente l'écran de l'ordinateur
    /// </summary>
    /// <param name="screenPosition">La position sur l'écran</param>
    /// <returns>La position sur le panel</returns>
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
            cursor.style.left = pixelUV.x;
            cursor.style.top = pixelUV.y;
        }


        return pixelUV;
    }

}
