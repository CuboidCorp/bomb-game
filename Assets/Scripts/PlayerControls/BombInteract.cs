using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script pour gérer toutes les interactions du joueur dans la scene de desamorcage
/// </summary>
public class BombInteract : BaseInteract
{

    [SerializeField] private float rotationSpeed = 50f;

    [Header("Booleans")]
    [SerializeField] private bool isZoomedOnBomb = false;
    [SerializeField] private bool isZoomedOnModule = false;
    [SerializeField] private bool isRotating = false;

    private Vector2 lastPos;

    private Transform bombTransform = null;
    private Transform moduleTransform = null;

    private Module targetModule = null;
    private Ray? lastRay;

    private int bombLayerMask;
    private int moduleLayerMask;

    protected override void Setup()
    {
        base.Setup();
        bombLayerMask = LayerMask.GetMask("Bomb");
        moduleLayerMask = LayerMask.GetMask("Module");
    }

    protected override void SetupActions()
    {
        base.SetupActions();
    }

    protected override void UnSetupActions()
    {
        base.UnSetupActions();
        actions.Player.Hold.started -= HoldStartModule;
        actions.Player.Hold.canceled -= HoldEndModule;
        actions.Player.Hold.performed -= StartRotate;
        actions.Player.Hold.canceled -= StopRotate;
    }

    private void StartRotate(InputAction.CallbackContext ctx)
    {
        isRotating = true;
        lastPos = _pos.ReadValue<Vector2>();
    }

    private void StopRotate(InputAction.CallbackContext ctx)
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

    protected override void OnTap(Vector2 pos)
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
                    if (hit.transform.TryGetComponent(out Module module))
                    {
                        module.ModuleInteract(ray);
                    }

                }
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.yellow, 5);
                if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, moduleLayerMask))
                {
                    isZoomedOnModule = true;
                    targetModule = hit.transform.GetComponent<Module>();
                    lastRay = ray;
                    actions.Player.Hold.started += HoldStartModule;
                    actions.Player.Hold.canceled += HoldEndModule;
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
                Debug.Log("Activation Rotation");
                actions.Player.Hold.performed += StartRotate;
                actions.Player.Hold.canceled += StopRotate;
                isZoomedOnBomb = true;
                bombTransform = hit.transform;
                bombTransform.GetComponent<Collider>().enabled = false;
                ZoomOnTransform(bombTransform);
            }

        }
    }

    protected override void UnZoom()
    {
        if (isZoomedOnModule)//Si on quitte le module
        {
            targetModule = null;
            actions.Player.Hold.started -= HoldStartModule;
            actions.Player.Hold.canceled -= HoldEndModule;
            actions.Player.Hold.performed += StartRotate;
            actions.Player.Hold.canceled += StopRotate;
            isZoomedOnModule = false;
            moduleTransform.GetComponent<Collider>().enabled = true;
            ZoomOnTransform(bombTransform);
        }
        else if (isZoomedOnBomb)
        {
            actions.Player.Hold.performed -= StartRotate;
            actions.Player.Hold.canceled -= StopRotate;
            isZoomedOnBomb = false;
            bombTransform.GetComponent<Collider>().enabled = true;
            ZoomOnTransform(null);
        }
        else
        {
            ZoomOnTransform(null);
        }
    }

    private void HoldStartModule(InputAction.CallbackContext context)
    {
        if (lastRay != null && targetModule != null)
        {
            targetModule.OnModuleHoldStart(lastRay.Value);
            lastRay = null;
        }
    }

    private void HoldEndModule(InputAction.CallbackContext context)
    {
        targetModule.OnModuleHoldEnd();
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
