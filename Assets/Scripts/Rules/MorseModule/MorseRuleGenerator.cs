using System.Collections.Generic;

public class MorseRuleGenerator
{
    //Les booleens c'est true pr long et false pr court
    private static Dictionary<char, bool[]> morseAlphabet = new()
    {
        { 'A',new bool[] { false,true } },
        { 'B',new bool[] { true,false,false,false } },
        { 'C',new bool[] { true,false,true,false } },
        { 'D',new bool[] { true,false,false } },
        { 'E',new bool[] { false } },
        { 'F',new bool[] { false,false,true,false } },
        { 'G',new bool[] { true,true,false } },
        { 'H',new bool[] { false,false,false,false } },
        { 'I',new bool[] { false,false } },
        { 'J',new bool[] { false,true,true,true } },
        { 'K',new bool[] { true,false,true } },
        { 'L',new bool[] { false,true,false,false } },
        { 'M',new bool[] { true,true } },
        { 'N',new bool[] { true,false } },
        { 'O',new bool[] { true,true,true } },
        { 'P',new bool[] { false,true,true,false } },
        { 'Q',new bool[] { true,true,false,true } },
        { 'R',new bool[] { false,true,false } },
        { 'S',new bool[] { false,false,false } },
        { 'T',new bool[] { true } },
        { 'U',new bool[] { false,false, true } },
        { 'V',new bool[] { false,false,false,true } },
        { 'W',new bool[] { false,true,true } },
        { 'X',new bool[] { true,false,false,true } },
        { 'Y',new bool[] { true,false,true,true } },
        { 'Z',new bool[] { true,true,false,false } },
        { '0',new bool[] { true,true,true,true,true } },
        { '1',new bool[] { false,true,true,true,true } },
        { '2',new bool[] { false,false,true,true,true } },
        { '3',new bool[] { false,false,false,true,true } },
        { '4',new bool[] { false,false,false,false,true } },
        { '5',new bool[] { false,false,false,false,false } },
        { '6',new bool[] { true,false,false,false,false } },
        { '7',new bool[] { true,true,false,false,false } },
        { '8',new bool[] { true,true,true,false,false } },
        { '9',new bool[] { true,true,true,true,false } },
    };

    //TODO : Pret rajouter kes symboles spéciaux si besoin / customiser  les symboles
    private List<char[]> groups;

    private List<MorseRule> rules;
    private const int NB_RULES = 8;
    private const int IMG_X = 3;
    private const int IMG_Y = 5;

    private const int PROBA_ACTIVATED = 50;

    private int currentRuleIndex = 0;

    public void SetupRules() 
    {
        rules = new();

        GenerateGroups();

        for (int i = 0; i < NB_RULES; i++)
        {
            rules[i] = GenerateRule(i);
        }

        Functions.Shuffle(rules);
    }

    private MorseRule GenerateRule(int index)
    {
        char[] targetGroup = groups[index];
        char targetChar = targetGroup[Random.Range(0, targetGroup.Length)];
        bool[] targetMorse = morseAlphabet[targetChar];

        bool[,] correctImage = new bool[IMG_X, IMG_Y];
        for (int i = 0; i < IMG_X; i++)
        {
            for (int j = 0; j < IMG_Y; j++)
            {
                correctImage[i, j] = Random.Range(0, 100) < PROBA_ACTIVATED;
            }
        }

        return new MorseRule
        {
            targetCharacter = targetChar,
            targetMorseCode = targetMorse,
            correctImage = correctImage
        };

    }

    private void GenerateGroups()
    {
        //On divise les caracteres en NB_RULES groupes
        groups = new();

        List<char> caract = new(morseAlphabet.Keys);
        Functions.Shuffle(caract);

        int nbCaract = caract.Count;
        int nbCaractPerGroup = nbCaract / NB_RULES;
        int reste = nbCaract % nbCaractPerGroup;

        List<int> taillesGroupes = new();
        for (int i = 0; i < NB_RULES; i++)
        {
            taillesGroupes[i] = nbCaractPerGroup;
            if (reste > 0)
            {
                taillesGroupes[i]++;
                reste--;
            }
        }

        Functions.Shuffle(taillesGroupes);
        int index = 0;

        for(int i = 0; i < NB_RULES; i++)
        {
            char[] groupe = caract.Skip(index).Take(taillesGroupes[i]).ToArray();
            groups.Add(groupe);
            index += taillesGroupes[i];
        }

        //AFFICHAGE DEBUG TEMPORAIRE
        for (int i = 0; i < groupes.Count; i++)
        {
            Debug.Log($"Groupe {i + 1}: {new string(groupes[i])}");
        }
    }

    public MorseRule GetRule()
    {
        currentRuleIndex++;
        if (currentRuleIndex > NB_RULES)
        {
            currentRuleIndex = 1;
        }
        return rules[currentRuleIndex];
    }

    public Vector2Int GetImageSize()
    {
        return new Vector2Int(IMG_X, IMG_Y);
    }
}
