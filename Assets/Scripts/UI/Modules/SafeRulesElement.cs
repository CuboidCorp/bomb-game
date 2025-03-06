using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

[UxmlElement]
public partial class SafeRulesElement : VisualElement
{
    private Label[] Nums = new Label[3];
    private Label[] Dirs = new Label[3];

    private Label Num1 => this.Q<Label>("num1");
    private Label Num2 => this.Q<Label>("num2");
    private Label Num3 => this.Q<Label>("num3");

    private Label Dir1 => this.Q<Label>("dir1");
    private Label Dir2 => this.Q<Label>("dir2");
    private Label Dir3 => this.Q<Label>("dir3");

    public void Init(SafeRuleGenerator safeRuleGen)
    {
        SafeRule rule = safeRuleGen.GetRule();

        Nums[0] = Num1;
        Nums[1] = Num2;
        Nums[2] = Num3;

        Dirs[0] = Dir1;
        Dirs[1] = Dir2;
        Dirs[2] = Dir3;

        int cpt = 0;
        string value;
        string text;
        int index = 0;
        for (int i = 0; i < rule.questions.Count; i++)
        {
            value = rule.annexesQuestions[cpt];
            text = GetTextFromCond(rule.questions[i], value);
            text += " " + TextLocalizationHandler.LoadString("GenericText", "THEN");
            text += "\n";
            if (i % 2 == 0)
            {
                LocalizedString numText = TextLocalizationHandler.GetSmartString("TexteManuel", "SAFE_TARGET_NUMBER");
                numText.Arguments = new object[] { rule.valeurs[cpt] };
                text += numText.GetLocalizedString();
            }
            else
            {
                text += rule.directions[cpt] ? TextLocalizationHandler.LoadString("TexteManuel", "TURN_CLOCKWISE") : TextLocalizationHandler.LoadString("TexteManuel", "TURN_COUNTERCLOCKWISE");
            }

            text += "\n";
            text += TextLocalizationHandler.LoadString("GenericText", "ELSE");
            text += "\n";

            if (i % 2 == 0)
            {
                LocalizedString numText = TextLocalizationHandler.GetSmartString("TexteManuel", "SAFE_TARGET_NUMBER");
                numText.Arguments = new object[] { rule.valeurs[cpt + 1] };
                text += numText.GetLocalizedString();
                Nums[index].text = text;
            }
            else
            {
                text += rule.directions[cpt + 1] ? TextLocalizationHandler.LoadString("TexteManuel", "TURN_CLOCKWISE") : TextLocalizationHandler.LoadString("TexteManuel", "TURN_COUNTERCLOCKWISE");
                Dirs[index].text = text;
                index++;
                cpt += 2;
            }
        }
    }

    private string GetTextFromCond(SerialNumberConditions cond, string value)
    {
        LocalizedString smartText;
        string texte;
        switch (cond)
        {
            case SerialNumberConditions.HAS_CHAR:
                smartText = TextLocalizationHandler.GetSmartString("TexteManuel", "SERIAL_NUMBER_CONTAINS");
                smartText.Arguments = new object[] { value };
                break;
            case SerialNumberConditions.HAS_REPEATING_CHARACTERS:
                smartText = TextLocalizationHandler.GetSmartString("TexteManuel", "SERIAL_NUMBER_HAS_DUPE");
                smartText.Arguments = new object[] { value };
                break;
            case SerialNumberConditions.HAS_NUMBER_GREATER_THAN:
                smartText = TextLocalizationHandler.GetSmartString("TexteManuel", "SERIAL_NUMBER_HAS_NUMBER");
                texte = TextLocalizationHandler.LoadString("GenericText", "GREATER_THAN");
                smartText.Arguments = new object[] { texte, value };
                break;
            case SerialNumberConditions.HAS_NUMBER_LESSER_THAN:
                smartText = TextLocalizationHandler.GetSmartString("TexteManuel", "SERIAL_NUMBER_HAS_NUMBER");
                texte = TextLocalizationHandler.LoadString("GenericText", "LESSER_THAN");
                smartText.Arguments = new object[] { texte, value };
                break;
            case SerialNumberConditions.HAS_NUMBER_EQUAL_TO:
                smartText = TextLocalizationHandler.GetSmartString("TexteManuel", "SERIAL_NUMBER_HAS_NUMBER");
                texte = TextLocalizationHandler.LoadString("GenericText", "EQUAL_TO");
                smartText.Arguments = new object[] { texte, value };
                break;
            case SerialNumberConditions.HAS_SUM_GREATER_THAN:
                smartText = TextLocalizationHandler.GetSmartString("TexteManuel", "SERIAL_NUMBER_SUM");
                texte = TextLocalizationHandler.LoadString("GenericText", "GREATER_THAN");
                smartText.Arguments = new object[] { texte, value };
                break;
            case SerialNumberConditions.HAS_SUM_LESSER_THAN:
                smartText = TextLocalizationHandler.GetSmartString("TexteManuel", "SERIAL_NUMBER_SUM");
                texte = TextLocalizationHandler.LoadString("GenericText", "LESSER_THAN");
                smartText.Arguments = new object[] { texte, value };
                break;
            case SerialNumberConditions.HAS_SUM_EQUAL_TO:
                smartText = TextLocalizationHandler.GetSmartString("TexteManuel", "SERIAL_NUMBER_SUM");
                texte = TextLocalizationHandler.LoadString("GenericText", "EQUAL_TO");
                smartText.Arguments = new object[] { texte, value };
                break;
            case SerialNumberConditions.HAS_NB_VOWEL_GREATER_THAN:
                smartText = TextLocalizationHandler.GetSmartString("TexteManuel", "SERIAL_NUMBER_NB_VOWELS");
                texte = TextLocalizationHandler.LoadString("GenericText", "GREATER_THAN");
                smartText.Arguments = new object[] { texte, value };
                break;
            case SerialNumberConditions.HAS_NB_VOWEL_LESSER_THAN:
                smartText = TextLocalizationHandler.GetSmartString("TexteManuel", "SERIAL_NUMBER_NB_VOWELS");
                texte = TextLocalizationHandler.LoadString("GenericText", "LESSER_THAN");
                smartText.Arguments = new object[] { texte, value };
                break;
            case SerialNumberConditions.HAS_NB_VOWEL_EQUAL_TO:
                smartText = TextLocalizationHandler.GetSmartString("TexteManuel", "SERIAL_NUMBER_NB_VOWELS");
                texte = TextLocalizationHandler.LoadString("GenericText", "EQUAL_TO");
                smartText.Arguments = new object[] { texte, value };
                break;
            case SerialNumberConditions.HAS_NB_CONSONANT_GREATER_THAN:
                smartText = TextLocalizationHandler.GetSmartString("TexteManuel", "SERIAL_NUMBER_NB_CONS");
                texte = TextLocalizationHandler.LoadString("GenericText", "GREATER_THAN");
                smartText.Arguments = new object[] { texte, value };
                break;
            case SerialNumberConditions.HAS_NB_CONSONANT_LESSER_THAN:
                smartText = TextLocalizationHandler.GetSmartString("TexteManuel", "SERIAL_NUMBER_NB_CONS");
                texte = TextLocalizationHandler.LoadString("GenericText", "LESSER_THAN");
                smartText.Arguments = new object[] { texte, value };
                break;
            case SerialNumberConditions.HAS_NB_CONSONANT_EQUAL_TO:
                smartText = TextLocalizationHandler.GetSmartString("TexteManuel", "SERIAL_NUMBER_NB_CONS");
                texte = TextLocalizationHandler.LoadString("GenericText", "EQUAL_TO");
                smartText.Arguments = new object[] { texte, value };
                break;
            default:
                throw new System.Exception("Condition non gérée");
        }
        return smartText.GetLocalizedString();
    }

    public SafeRulesElement() { }
}
