using UnityEngine;

/// <summary>
/// Script pour gérer toutes les interactions du joueur dans la scene de desamorcage
/// </summary>
public class ManualInteract : BaseInteract
{
    [SerializeField] private Vector3 computerCamPosition;

    private bool isZoomedOnComputer = false;

    protected override void Setup()
    {
        base.Setup();
    }

    protected override void SetupActions()
    {
        base.SetupActions();
    }

    protected override void UnSetupActions()
    {
        base.UnSetupActions();
    }


    private void Update()
    {
        if (isZooming)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, zoomPosition, zoomSpeed * Time.deltaTime);
            if (Vector3.Distance(mainCamera.transform.position, zoomPosition) <= .1f)
            {
                isZooming = false;
            }
        }
    }

    protected override void OnTap(Vector2 pos)
    {
        Ray ray = mainCamera.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.CompareTag("Computer"))
            {
                if (isZoomedOnComputer)
                {
                    if (hit.collider.TryGetComponent(out PcUiManager computer))
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

    protected override void UnZoom()
    {
        PcUiManager.Instance.UnSetupDoc();
        ZoomTo(mainCameraBasePosition);
        isZoomedOnComputer = false;
    }
}
