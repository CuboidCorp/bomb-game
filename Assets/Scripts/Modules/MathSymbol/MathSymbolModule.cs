using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MathSymbolModule : Module
{
    private MathSymbolRule targetRule;

    [SerializeField] private Image[] buttons;
    [SerializeField] private TMP_Text numberHolder;

    private Sprite[] symbolSprites;

    private int currentCount = 0;

    public override void ModuleInteract(Ray rayInteract)
    {
        GetComponent<Collider>().enabled = false;
        if (Physics.Raycast(rayInteract, out RaycastHit hit, 10))
        {
            if (hit.collider.gameObject.name.StartsWith("Num") && hit.collider.gameObject.name != "NumberHolder")
            {
                AudioManager.Instance.PlaySoundEffect(SoundEffects.BUTTON_PRESS);
                OnButtonClicked(hit.collider.gameObject.name);
            }
        }
        GetComponent<Collider>().enabled = true;
    }

    public override void OnModuleHoldEnd()
    {
        //Rien pr ce module
    }

    public override void OnModuleHoldStart(Ray rayInteract, InputAction pos)
    {
        //Rien pr ce module
    }

    public override void SetupModule(RuleHolder rules)
    {
        symbolSprites = rules.mathSymbolRuleGenerator.GetSymbolsSprites();
        targetRule = rules.mathSymbolRuleGenerator.GetRule();

        numberHolder.text = targetRule.targetNumber.ToString();

        Debug.Log($"Solution {gameObject.name} : {targetRule}");

        for (int i = 0; i < buttons.Length; i++)
        {
            int index = (int)targetRule.valeursBtn.Keys.ToArray()[i];
            buttons[i].sprite = symbolSprites[index];
        }

    }

    private void OnButtonClicked(string btnName)
    {
        //On recup le int du num du bouton
        int num = int.Parse(btnName[3..]);

        //On recup le numéro associé au bouton
        int value = targetRule.valeursBtn[targetRule.valeursBtn.Keys.ToArray()[num - 1]];

        currentCount += value;
        Debug.Log("Current count : " + currentCount + " / " + targetRule.targetNumber);

        if (currentCount == targetRule.targetNumber)
        {
            AudioManager.Instance.PlaySoundEffect(SoundEffects.MODULE_SUCCESS);
            Success();
        }
        else if (currentCount > targetRule.targetNumber)
        {
            AudioManager.Instance.PlaySoundEffect(SoundEffects.MODULE_FAIL);
            currentCount = 0;
            Fail();
        }


    }
}
