using UnityEngine;

public class SafeRuleGenerator : IRuleGenerator
{
    private List<SafeRule> rules;
    public const int NB_RULES = 1;

    private SerialNumberGenerator serialNumberGen;

    /// <summary>
    /// Nombre d'�tapes � faire pour la regle
    /// </summary>
    private const int NB_DIRECTIONS = 3;

    private int currentRuleIndex = 0;

    public void SetupRules() //TODO : Recup le serial number pour les traitement des regles
    {
        rules = new();

        for(int i = 0; i < NB_RULES;i++)
        {
            rules.Add(GenerateRule());
        }

        Functions.Shuffle(rules);
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Genere une regle pour le d�verouillage du module
    /// </summary>
    /// <returns>La regle cr��e</returns>
    private SafeRule GenerateRule()
    {
        bool[] directions = new bool[NB_DIRECTIONS];
        int[] valeurs = new int[NB_DIRECTIONS];

        for(int i = 0; i < NB_DIRECTIONS;i++) //Temporaire tant qu'on a pas recup les infos du num de s�rie
        {
            directions[i] = Random.Range(0,2) == 0;
            valeurs[i] = Random.Range(0, 100);
        }

        return new SafeRule
        {
            directions = directions,
            valeurs = valeurs
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
    /// <param name="sng">Le gen�rateur des donn�es du num�ro de s�rie</param>
    public void SetSerialNumberGen(SerialNumberGenerator sng)
    {
        serialNumberGen = sng;
    }
}
