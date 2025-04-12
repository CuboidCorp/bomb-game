using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    private PopupData[] popupDatas;
    [SerializeField] private VisualTreeAsset popupAsset;

    private const int MAX_POPUPS = 20;
    [SerializeField] private float POPUP_SPAWN_TIME = 5f;

    private List<VisualElement> popups;

    private Vector2 screenSize;
    private int currentNbPopups = 0;

    private int popupIdCounter = 0;

    public Coroutine spawnPopupsCoroutine;


    private void Awake()
    {
        Instance = this;
        popupDatas = Resources.LoadAll<PopupData>("PopupData");
    }

    private void OnDisable()
    {
        if (spawnPopupsCoroutine != null)
        {
            StopCoroutine(spawnPopupsCoroutine);
            spawnPopupsCoroutine = null;
        }
    }

    public void InitPopups()
    {
        popups = new List<VisualElement>();
        screenSize = PcOperatorManager.Instance.GetScreenSize();
        spawnPopupsCoroutine = StartCoroutine(SpawnPopups());
    }

    private IEnumerator SpawnPopups()
    {
        while (true)
        {
            yield return new WaitForSeconds(POPUP_SPAWN_TIME);
            Popups randomPopup = (Popups)Random.Range(0, popupDatas.Length);
            if (currentNbPopups < MAX_POPUPS)
            {
                OpenPopup(randomPopup);
            }
            else
            {
                Debug.LogWarning("Maximum number of popups reached.");
                //TODO : Blue screen
            }
        }
    }

    public void OpenPopup(Popups popupType)
    {
        if (currentNbPopups >= MAX_POPUPS)
        {
            Debug.LogWarning("Maximum number of popups reached.");
            //TODO : Blue screen
            return;
        }
        PopupData data = LoadPopupDataFromType(popupType);
        VisualElement popup = popupAsset.CloneTree();

        Vector2 randomPos = new(Random.Range(0, screenSize.x - 750), Random.Range(0, screenSize.y - 300));
        popup.style.left = randomPos.x;
        popup.style.top = randomPos.y;
        popup.style.position = Position.Absolute;
        popup.name = GenerateUniqueId();

        data.buttonsActions = new System.Action[data.nbButtons];
        for (int i = 0; i < data.nbButtons; i++)
        {
            data.buttonsActions[i] = data.buttonsActionsEnum[i] switch
            {
                PopupAction.CLOSE => () => ClosePopup(popup.name),
                PopupAction.DUPLICATE => () => OpenPopup(data.popupType),
                _ => null,
            };
        }

        popup.Q<Button>("closeBtn").clicked += () => ClosePopup(popup.name);

        popup.Q<ClassicPopup>().Init(data);

        popups.Add(popup);
        PcOperatorManager.Instance.AddPopup(popup);
        currentNbPopups++;
    }

    private PopupData LoadPopupDataFromType(Popups popups)
    {
        return popupDatas[(int)popups];
    }

    public void ClosePopup(string popupId)
    {
        VisualElement popupToRemove = PcOperatorManager.Instance.GetElementByName(popupId);
        if (popupToRemove != null)
        {
            popups.Remove(popupToRemove);
            currentNbPopups--;
            PcOperatorManager.Instance.RemovePopup(popupToRemove);
        }
        else
        {
            Debug.LogWarning($"Popup with ID {popupId} not found.");
        }
    }

    private string GenerateUniqueId()
    {
        return "popup_" + popupIdCounter++;
    }

}
