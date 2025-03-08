using System.Collections.Generic;
using UnityEngine;

public class SafeRuleGenerator : IRuleGenerator
{
    private SafeRule rule;
    public const int NB_RULES = 1;

    private SerialNumberGenerator serialNumberGen;

    /// <summary>
    /// Nombre d'étapes à faire pour la regle
    /// </summary>
    private const int NB_DIRECTIONS = 3;

    /// <summary>
    /// Nombre de si a mettre normalement NB_DIRECTIONS * 2
    /// </summary>
    private const int NB_CONDITIONS = 6;

    public void SetupRules()
    {
        rule = GenerateRule();


    }

    /// <summary>
    /// Genere une regle pour le déverouillage du module
    /// </summary>
    /// <returns>La regle créée</returns>
    private SafeRule GenerateRule()
    {
        bool[] directions = new bool[NB_CONDITIONS];
        int[] valeurs = new int[NB_CONDITIONS];

        int[] goodDirIndex = new int[NB_DIRECTIONS];
        int[] goodValIndex = new int[NB_DIRECTIONS];

        List<SerialNumberConditions> questions = new();
        string[] annexesQuestions = new string[NB_CONDITIONS];

        for (int i = 0; i < NB_CONDITIONS; i += 2)
        {
            directions[i] = Random.Range(0, 2) == 0;
            valeurs[i] = Random.Range(0, 100);

            directions[i + 1] = !directions[i];
            valeurs[i + 1] = Random.Range(0, 100);
        }

        //Dans l'affichage des question réponses si l'index est pair c'est true, sinon c'est false
        //Choix des questions
        questions.Add((SerialNumberConditions)0);
        questions.Add((SerialNumberConditions)1);
        questions.Add((SerialNumberConditions)Random.Range(2, 5));
        questions.Add((SerialNumberConditions)Random.Range(5, 8));
        questions.Add((SerialNumberConditions)Random.Range(8, 11));
        questions.Add((SerialNumberConditions)Random.Range(11, 14));

        Functions.Shuffle(questions);

        //Calcul des bons 

        for (int i = 0; i < NB_CONDITIONS; i++)
        {
            SerialNumberConditions cond = questions[i];
            switch (cond)
            {
                case SerialNumberConditions.HAS_CHAR:
                    string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    char targetChar = chars[Random.Range(0, chars.Length)];
                    annexesQuestions[i] = targetChar.ToString();
                    if (serialNumberGen.ContainsChar(targetChar))
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i - 1;
                        }

                    }
                    else
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i + 1;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i;
                        }
                    }
                    break;
                case SerialNumberConditions.HAS_REPEATING_CHARACTERS:
                    if (serialNumberGen.HasDuplicateChar())
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i - 1;
                        }
                    }
                    else
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i + 1;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i;
                        }
                    }
                    break;
                case SerialNumberConditions.HAS_NUMBER_GREATER_THAN:
                    int targetNumber = Random.Range(0, 8);
                    annexesQuestions[i] = targetNumber.ToString();
                    bool aUnNombre = false;
                    while (targetNumber < 9)
                    {
                        //On check si il y a un nombre conforme
                        if (serialNumberGen.ContainsChar(targetNumber.ToString()[0]))
                        {
                            aUnNombre = true;
                            break;
                        }
                        targetNumber++;
                    }
                    if (aUnNombre)
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i - 1;
                        }
                    }
                    else
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i + 1;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i;
                        }
                    }
                    break;
                case SerialNumberConditions.HAS_NUMBER_LESSER_THAN:
                    int targetNumber2 = Random.Range(1, 10);
                    annexesQuestions[i] = targetNumber2.ToString();
                    bool aUnNombre2 = false;
                    while (targetNumber2 > 0)
                    {
                        //On check si il y a un nombre conforme
                        if (serialNumberGen.ContainsChar(targetNumber2.ToString()[0]))
                        {
                            aUnNombre2 = true;
                            break;
                        }
                        targetNumber2--;
                    }

                    if (aUnNombre2)
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i - 1;
                        }
                    }
                    else
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i + 1;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i;
                        }
                    }

                    break;
                case SerialNumberConditions.HAS_NUMBER_EQUAL_TO:
                    int targetNumber3 = Random.Range(0, 10);
                    annexesQuestions[i] = targetNumber3.ToString();
                    if (serialNumberGen.ContainsChar(targetNumber3.ToString()[0]))
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i - 1;
                        }
                    }
                    else
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i + 1;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i;
                        }
                    }
                    break;
                case SerialNumberConditions.HAS_SUM_GREATER_THAN:
                    int targetSum = Random.Range(10, 15);
                    annexesQuestions[i] = targetSum.ToString();
                    if (serialNumberGen.GetSumOfSerialNumber() > targetSum)
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i - 1;
                        }
                    }
                    else
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i + 1;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i;
                        }
                    }
                    break;
                case SerialNumberConditions.HAS_SUM_LESSER_THAN:
                    int targetSum2 = Random.Range(10, 15);
                    annexesQuestions[i] = targetSum2.ToString();
                    if (serialNumberGen.GetSumOfSerialNumber() < targetSum2)
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i - 1;
                        }
                    }
                    else
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i + 1;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i;
                        }
                    }
                    break;
                case SerialNumberConditions.HAS_SUM_EQUAL_TO:
                    int targetSum3 = Random.Range(10, 15);
                    annexesQuestions[i] = targetSum3.ToString();
                    if (serialNumberGen.GetSumOfSerialNumber() == targetSum3)
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i - 1;
                        }
                    }
                    else
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i + 1;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i;
                        }
                    }
                    break;
                case SerialNumberConditions.HAS_NB_VOWEL_GREATER_THAN:
                    int targetNbVowel = Random.Range(0, 4);
                    annexesQuestions[i] = targetNbVowel.ToString();
                    if (serialNumberGen.GetNbVowels() > targetNbVowel)
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i - 1;
                        }
                    }
                    else
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i + 1;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i;
                        }
                    }
                    break;
                case SerialNumberConditions.HAS_NB_VOWEL_LESSER_THAN:
                    int targetNbVowel2 = Random.Range(0, 4);
                    annexesQuestions[i] = targetNbVowel2.ToString();
                    if (serialNumberGen.GetNbVowels() < targetNbVowel2)
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i - 1;
                        }
                    }
                    else
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i + 1;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i;
                        }
                    }
                    break;
                case SerialNumberConditions.HAS_NB_VOWEL_EQUAL_TO:
                    int targetNbVowel3 = Random.Range(0, 4);
                    annexesQuestions[i] = targetNbVowel3.ToString();
                    if (serialNumberGen.GetNbVowels() == targetNbVowel3)
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i - 1;
                        }
                    }
                    else
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i + 1;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i;
                        }
                    }
                    break;
                case SerialNumberConditions.HAS_NB_CONSONANT_GREATER_THAN:
                    int targetNbConsonant = Random.Range(3, 9);
                    annexesQuestions[i] = targetNbConsonant.ToString();
                    if (serialNumberGen.GetNbConsonants() > targetNbConsonant)
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i - 1;
                        }
                    }
                    else
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i + 1;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i;
                        }
                    }
                    break;
                case SerialNumberConditions.HAS_NB_CONSONANT_LESSER_THAN:
                    int targetNbConsonant2 = Random.Range(3, 9);
                    annexesQuestions[i] = targetNbConsonant2.ToString();
                    if (serialNumberGen.GetNbConsonants() < targetNbConsonant2)
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i - 1;
                        }
                    }
                    else
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i + 1;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i;
                        }
                    }
                    break;
                case SerialNumberConditions.HAS_NB_CONSONANT_EQUAL_TO:
                    int targetNbConsonant3 = Random.Range(3, 9);
                    annexesQuestions[i] = targetNbConsonant3.ToString();
                    if (serialNumberGen.GetNbConsonants() == targetNbConsonant3)
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i - 1;
                        }
                    }
                    else
                    {
                        if (i % 2 == 0)
                        {
                            goodDirIndex[i / 2] = i + 1;
                        }
                        else
                        {
                            goodValIndex[i / 2] = i;
                        }
                    }
                    break;
            }
        }

        return new SafeRule
        {
            directions = directions,
            valeurs = valeurs,
            goodDirIndex = goodDirIndex,
            goodValIndex = goodValIndex,
            questions = questions,
            annexesQuestions = annexesQuestions
        };
    }

    /// <summary>
    /// Renvoie la prochaine regle dans la liste
    /// </summary>
    /// <returns>Une regle pour le module</returns>
    public SafeRule GetRule()
    {
        return rule;
    }

    /// <summary>
    /// Set le serialNumberGenerator pour l'utiliser dans le setup
    /// </summary>
    /// <param name="sng">Le genérateur des données du numéro de série</param>
    public void SetSerialNumberGen(SerialNumberGenerator sng)
    {
        serialNumberGen = sng;
    }
}
