using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script pour gérer toutes les interactions du joueur dans la scene de desamorcage
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

    }

    protected void UnSetupActions()
    {
        actions.Player.Tap.performed -= OnTap;

    }

    private void OnEnable()
    {
        actions.Enable();
        SetupActions();
        actions.Player.Escape.performed += OnEscapePerformed;
    }

    private void OnDisable()
    {
        actions.Disable();
        UnSetupActions();
        actions.Player.Escape.performed -= OnEscapePerformed;
    }

    /// <summary>
    /// Ouvre ou ferme le menu pause
    /// </summary>
    private void OnEscapePerformed(InputAction.CallbackContext _)
    {
        if (PauseMenuManager.Instance.OpenOrClose())
        {
            UnSetupActions();
        }
        else
        {
            SetupActions();
        }
    }

    private void Update()
    {
        if (isZooming)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, zoomPosition, zoomSpeed * Time.deltaTime);
            if (Vector3.Distance(mainCamera.transform.position, zoomPosition) <= .05f)
            {
                PcLevelSelectManager.Instance.StartComputer();
                mainCamera.transform.position = zoomPosition;
                isZooming = false;
            }
        }
    }

    /// <summary>
    /// Fait que la camera zoome jusqu'à la position donnée
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
}
