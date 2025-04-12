using System;
using UnityEngine;
using UnityEngine.Localization;


[CreateAssetMenu(fileName = "PopupData", menuName = "ScriptableObjects/PopupData", order = 1)]
public class PopupData : ScriptableObject
{
    public Popups popupType;
    public LocalizedString mainText;
    public LocalizedString subText;

    public LocalizedString[] buttonsText;
    public PopupAction[] buttonsActionsEnum;
    public int nbButtons;
    public bool isCloseBtnVisible;

    [NonSerialized] public Action[] buttonsActions;

    private void OnValidate()
    {
        if (buttonsText.Length != nbButtons)
        {
            Debug.LogError($"Le nombre d'éléments dans buttonsText ({buttonsText.Length}) ne correspond pas à nbButtons ({nbButtons}).");
        }

        if (buttonsActionsEnum.Length != nbButtons)
        {
            Debug.LogError($"Le nombre d'éléments dans buttonsActionsEnum ({buttonsActionsEnum.Length}) ne correspond pas à nbButtons ({nbButtons}).");
        }
    }


}
