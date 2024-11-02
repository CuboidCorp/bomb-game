using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script pour gérer toutes les interactions du joueur
/// </summary>
public class InteractScript : MonoBehaviour
{
    private PlayerControls actions;
    private InputAction _pos;

    private bool isZoomedOnBomb = false;
    private bool isZoomedOnModule = false;
    private bool isRotating = false;

    private Vector2 lastPos;

    [Header("Settings")]

    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float zoomSpeed = 0.5f;

    private const float rayDistance = 10;

    private Transform bombTransform = null;
    private Transform moduleTransform = null;

    private int bombLayerMask;
    private int moduleLayerMask;

    private Camera mainCamera;
    [SerializeField] private Vector3 mainCameraBasePosition;


    private void Awake()
    {
        actions = new PlayerControls();
        actions.Enable();

        mainCamera = Camera.main;
        mainCamera.transform.position = mainCameraBasePosition;

        bombLayerMask = LayerMask.GetMask("Bomb");
        moduleLayerMask = LayerMask.GetMask("Module");

        _pos = actions.Player.Position;

        actions.Player.Tap.performed += _ => OnTap(_pos.ReadValue<Vector2>());
        actions.Player.Hold.performed += _ => StartRotate();
        actions.Player.Hold.canceled += _ => StopRotate();
    }

    private void OnEnable()
    {
        actions.Enable();
    }

    private void OnDisable()
    {
        actions.Disable();
    }

    private void StartRotate()
    {
        isRotating = true;
        lastPos = _pos.ReadValue<Vector2>();
    }

    private void StopRotate()
    {
        isRotating = false;
    }

    private void Update()
    {
        if (isRotating)
        {
            if (bombTransform != null && isZoomedOnBomb && !isZoomedOnModule)
            {
                RotateObject();
            }
        }
    }

    private void OnTap(Vector2 pos)
    {
        Debug.Log("Tap " + pos);
        Ray ray = mainCamera.ScreenPointToRay(pos);
        if (isZoomedOnBomb)
        {
            //On quitte ou on zoome sur le module
            Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.yellow, 5);
            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, moduleLayerMask))
            {
                isZoomedOnModule = true;
                moduleTransform = hit.transform;
                ZoomOnTransform(moduleTransform);
                Debug.Log("Module");
            }
            else
            {
                if (!Physics.Raycast(ray, out _, rayDistance, bombLayerMask))
                {
                    isZoomedOnBomb = false;
                    bombTransform = null;
                    ZoomOnTransform(null);
                    Debug.Log("Leaving bomb");
                }

            }
        }
        else
        {
            //On zoome sur la bombe
            Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red, 5);
            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, bombLayerMask))
            {
                Debug.Log("Bomb");
                isZoomedOnBomb = true;
                bombTransform = hit.transform;
                ZoomOnTransform(bombTransform);
            }
            else
            {
                Debug.Log("No bomb");
            }

        }
    }

    private void ZoomOnTransform(Transform transform)
    {
        //Zoomer sur un transform ou sur la position initiale 
        Vector3 zoomPosition = mainCameraBasePosition; // Ajustez cette valeur pour un meilleur effet de zoom
        if (transform != null)
        {
            zoomPosition = transform.position + transform.forward * -2f; // Ajustez cette valeur pour un meilleur effet de zoom}
        }
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, zoomPosition, zoomSpeed);
    }

    private void RotateObject()
    {
        Vector2 currentPos = _pos.ReadValue<Vector2>();
        Vector2 deltaPos = currentPos - lastPos; // Calcul de la différence de position
        float rotationY;
        if (Vector3.Dot(bombTransform.up, Vector3.up) >= 0)
        {
            rotationY = -Vector3.Dot(deltaPos, mainCamera.transform.right) * rotationSpeed * Time.deltaTime;
        }
        else
        {
            rotationY = Vector3.Dot(deltaPos, mainCamera.transform.right) * rotationSpeed * Time.deltaTime;
        }

        // Rotation autour de l'axe Y (horizontal) et X (vertical)
        float rotationX = Vector3.Dot(deltaPos, mainCamera.transform.up) * rotationSpeed * Time.deltaTime;

        // Appliquer la rotation
        bombTransform.Rotate(bombTransform.up, rotationY, Space.World);
        bombTransform.Rotate(mainCamera.transform.right, rotationX, Space.World);

        lastPos = currentPos; // Mettre à jour la dernière position
    }
}
