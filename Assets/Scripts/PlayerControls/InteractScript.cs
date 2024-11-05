using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script pour gérer toutes les interactions du joueur
/// </summary>
public class InteractScript : MonoBehaviour
{
    private PlayerControls actions;
    private InputAction _pos;


    [Header("Booleans")]
    [SerializeField] private bool isZoomedOnBomb = false;
    [SerializeField] private bool isZoomedOnModule = false;
    [SerializeField] private bool isRotating = false;
    [SerializeField] private bool isZooming = false;

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
    private Vector3 zoomPosition;

    private Coroutine pinchDetectCoroutine;


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

        actions.Player.Return.performed += _ => UnZoom();
        actions.Player.SecondFingerContact.started += _ => PinchDetectStart();
        actions.Player.SecondFingerContact.canceled += _ => PinchDetectEnd();
    }

    private void OnEnable()
    {
        actions.Enable();
    }

    private void OnDisable()
    {
        actions.Disable();
        PinchDetectEnd();
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
        if (isZooming)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, zoomPosition, zoomSpeed * Time.deltaTime);
            if (Vector3.Distance(mainCamera.transform.position, zoomPosition) <= .25f)
            {
                isZooming = false;
            }
        }
    }

    private void OnTap(Vector2 pos)
    {
        Debug.Log("Tap " + pos);
        Ray ray = mainCamera.ScreenPointToRay(pos);
        if (isZoomedOnBomb)
        {
            if (isZoomedOnModule)
            {
                Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green, 5);
                if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, moduleLayerMask))
                {
                    moduleTransform.GetComponent<Collider>().enabled = false;
                    hit.transform.GetComponent<Module>().ModuleInteract(ray);
                }
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.yellow, 5);
                if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, moduleLayerMask))
                {
                    isZoomedOnModule = true;
                    moduleTransform = hit.transform;
                    ZoomOnTransform(moduleTransform);
                }
            }
        }
        else
        {
            //On zoome sur la bombe
            Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red, 5);
            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, bombLayerMask))
            {
                isZoomedOnBomb = true;
                bombTransform = hit.transform;
                bombTransform.GetComponent<Collider>().enabled = false;
                ZoomOnTransform(bombTransform);
            }

        }
    }

    private void PinchDetectStart()
    {
        pinchDetectCoroutine = StartCoroutine(PinchDetection());
    }

    private void PinchDetectEnd()
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

    private void UnZoom()
    {
        if (isZoomedOnModule)
        {
            isZoomedOnModule = false;
            moduleTransform.GetComponent<Collider>().enabled = true;
            ZoomOnTransform(bombTransform);
        }
        else if (isZoomedOnBomb)
        {
            isZoomedOnBomb = false;
            bombTransform.GetComponent<Collider>().enabled = true;
            ZoomOnTransform(null);
        }
        else
        {
            ZoomOnTransform(null);
        }
    }

    private void ZoomOnTransform(Transform targetTransform)
    {
        if (targetTransform != null)
        {
            zoomPosition = targetTransform.position + targetTransform.forward * 2f; // Ajustez cette valeur pour un meilleur effet de zoom}
        }
        else
        {
            zoomPosition = mainCameraBasePosition;
        }
        isZooming = true;
    }

    private void RotateObject()
    {
        Vector2 currentPos = _pos.ReadValue<Vector2>();
        Vector2 deltaPos = currentPos - lastPos; // Calcul de la différence de position
        float rotationY;
        if (Vector3.Dot(bombTransform.up, Vector3.up) >= 0)
        {
            rotationY = Vector3.Dot(deltaPos, mainCamera.transform.right) * rotationSpeed * Time.deltaTime;
        }
        else
        {
            rotationY = -Vector3.Dot(deltaPos, mainCamera.transform.right) * rotationSpeed * Time.deltaTime;
        }

        // Rotation autour de l'axe Y (horizontal) et X (vertical)
        float rotationX = Vector3.Dot(deltaPos, mainCamera.transform.up) * rotationSpeed * Time.deltaTime;

        // Appliquer la rotation
        bombTransform.Rotate(bombTransform.up, rotationY, Space.World);
        bombTransform.Rotate(mainCamera.transform.right, rotationX, Space.World);

        lastPos = currentPos; // Mettre à jour la dernière position
    }
}
