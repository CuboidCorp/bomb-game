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

    private int currentRuleIndex = 0;

    public void SetupRules()
    {
        rule = GenerateRule();

        throw new System.NotImplementedException();
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

        for(int i = 0; i < NB_CONDITIONS ; i+=2)
        {
            directions[i] = Random.Range(0,2) == 0; 
            valeurs[i] = Random.Range(0, 100);

            directions[i + 1] = !directions[i];
            valeurs[i+1] = Random.Range(0, 100);
        }

        //Dans l'affichage des question réponses si l'index est pair c'est true, sinon c'est false
        //Choix des questions
        questions.Add((SerialNumberConditions)0);
        questions.Add((SerialNumberConditions)1);
        questions.Add((SerialNumberConditions)Random.Range(2,5));
        questions.Add((SerialNumberConditions)Random.Range(5, 8));
        questions.Add((SerialNumberConditions)Random.Range(8, 11));
        questions.Add((SerialNumberConditions)Random.Range(11, 14));

        Functions.Shuffle(questions);
        
        //Calcul des bons 
        
        for(int i = 0 ; i< NB_CONDITIONS ;i++)
        {
            SerialNumberConditions cond = questions[i];
            switch (cond)
            {
                case SerialNumberConditions.HAS_CHAR:
                    char randomChar = 'a';

                    break;
            }
        }

        return new SafeRule
        {
            directions = directions,
            valeurs = valeurs,
            goodDirIndex = goodDirIndex,
            goodValIndex = goodValIndex,
            questions = questions
        };
    }

    /// <summary>
    /// Renvoie la prochaine regle dans la liste
    /// </summary>
    /// <returns>Une regle pour le module</returns>
    public SafeRule GetRule()
    {
        currentRuleIndex++;
        if (currentRuleIndex > NB_RULES)
        {
            currentRuleIndex = 1;
        }
        return rules[currentRuleIndex - 1];
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
