using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script pour g�rer toutes les interactions du joueur dans la scene de desamorcage
/// </summary>
public class LevelSelectInteract : MonoBehaviour
{
    public static LevelSelectInteract Instance;

    protected PlayerControls actions;
    protected InputAction _pos;

    protected bool isZooming = false;

    [Header("Settings")]

    [SerializeField] protected float zoomSpeed = 1.5f;
    protected const float rayDistance = 10;

    protected Camera mainCamera;
    [SerializeField] protected Vector3 mainCameraBasePosition;
    protected Vector3 zoomPosition;

    private Coroutine pinchDetectCoroutine;

    [SerializeField] private Vector3 computerCamPosition;

    private bool isZoomedOnComputer = false;

    private void Awake()
    {
        Instance = this;
        Setup();
    }

    private void Setup()
    {
        actions = new PlayerControls();

        mainCamera = Camera.main;
        mainCamera.transform.position = mainCameraBasePosition;
    }

    public void SetupActions()
    {
        _pos = actions.Player.Position;

        actions.Player.Tap.performed += OnTap;

        actions.Player.Return.performed += UnZoom;
        actions.Player.SecondFingerContact.started += PinchDetectStart;
        actions.Player.SecondFingerContact.canceled += PinchDetectEnd;
        actions.Player.Escape.performed += OnEscapePerformed;
    }

    protected void UnSetupActions()
    {
        actions.Player.Tap.performed -= OnTap;

        actions.Player.Return.performed -= UnZoom;
        actions.Player.SecondFingerContact.started -= PinchDetectStart;
        actions.Player.SecondFingerContact.canceled -= PinchDetectEnd;
        actions.Player.Escape.performed -= OnEscapePerformed;
    }

    private void OnEnable()
    {
        actions.Enable();
        SetupActions();
    }

    private void OnDisable()
    {
        actions.Disable();
        UnSetupActions();
        PinchDetectEnd();
    }

    /// <summary>
    /// Ouvre ou ferme le menu pause
    /// </summary>
    private void OnEscapePerformed(InputAction.CallbackContext _)
    {
        if (PauseMenuManager.Instance.OpenOrClose())
        {
            UnSetupActions();
            PinchDetectEnd();

        }
    }

    private void Update()
    {
        if (isZooming)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, zoomPosition, zoomSpeed * Time.deltaTime);
            if (Vector3.Distance(mainCamera.transform.position, zoomPosition) <= .05f)
            {
                mainCamera.transform.position = zoomPosition;
                isZooming = false;
            }
        }
    }

    private void PinchDetectStart(InputAction.CallbackContext _)
    {
        pinchDetectCoroutine = StartCoroutine(PinchDetection());
    }

    private void PinchDetectEnd(InputAction.CallbackContext ctx = new InputAction.CallbackContext())
    {
        if (pinchDetectCoroutine != null)
            StopCoroutine(pinchDetectCoroutine);
    }

    private IEnumerator PinchDetection()
    {
        float previousDistance = 0f, distance;
        while (true)
        {
            distance = Vector2.Distance(actions.Player.Position.ReadValue<Vector2>(), actions.Player.SecondFingerPosition.ReadValue<Vector2>());

            if (distance > previousDistance)
            {
                UnZoom();
            }
            previousDistance = distance;
        }
    }

    /// <summary>
    /// Fait que la camera zoome jusqu'� la position donn�e
    /// </summary>
    /// <param name="pos">La position </param>
    private void ZoomTo(Vector3 pos)
    {
        isZooming = true;
        zoomPosition = pos;
    }

    private void OnTap(InputAction.CallbackContext _)
    {
        Vector2 pos = _pos.ReadValue<Vector2>();
        Ray ray = mainCamera.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.CompareTag("Computer"))
            {
                if (isZoomedOnComputer)
                {
                    if (hit.collider.TryGetComponent(out PcLevelSelectManager computer))
                    {
                        computer.StartComputer();
                    }
                }
                else
                {
                    ZoomTo(computerCamPosition);
                    isZoomedOnComputer = true;
                }
            }
        }
    }

    private void UnZoom(InputAction.CallbackContext _ = new InputAction.CallbackContext())
    {
        PcLevelSelectManager.Instance.UnSetupDoc();
        ZoomTo(mainCameraBasePosition);
        isZoomedOnComputer = false;
    }
}
