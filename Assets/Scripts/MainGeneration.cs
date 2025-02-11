using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
using System.Linq;

public class MainGeneration : MonoBehaviour
{
    [SerializeField] private int seed = 0;
    [SerializeField] private BombTypes bombType;

    public bool isDebug = false;

    public static MainGeneration Instance;

    private RuleHolder ruleHolder;

    /// <summary>
    /// Array qui contient tous les modules de la bombe
    /// </summary>
    private ModuleType[] bombModules;

    #region ModuleGeneration
    private ModuleType[] allModules;
    private Dictionary<ModuleType, float> moduleWeights;

    [SerializeField] private float weightDecreaseFactor = 0.5f;
    [SerializeField] private float weightIncreaseFactor = .1f;
    [SerializeField] private float minimumWeight = .1f;

    private float totalWeight;
    #endregion

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
        ruleHolder = new();
        if (isDebug)
        {
            Debug.LogWarning("Lancement en mode debug, le jeu sera diff�rent de la release, meme avec le meme seed");
            Debug.Log("Set seed : " + seed);
            Random.InitState(seed);
        }
    }

    /// <summary>
    /// Set le seed de la game 
    /// </summary>
    /// <param name="seed">Le seed a mettre</param>
    public void SetSeed(int seed)
    {
        this.seed = seed;
        Debug.Log("Set seed : " + seed);
        Random.InitState(seed);
    }

    /// <summary>
    /// Set le type de bombe
    /// </summary>
    /// <param name="bombtype">The bomb type</param>
    public void SetBombType(BombTypes bombtype)
    {
        Debug.Log("Set bomb type : " + bombtype);
        bombType = bombtype;
    }

    /// <summary>
    /// Genere les modules de la bombe
    /// </summary>
    public void GenerateModules()
    {
        int nbModules = bombType switch
        {
            BombTypes.SIX_SLOTS => 5,
            BombTypes.TWELVE_SLOTS => 11,
            _ => throw new NotImplementedException(),
        };

        allModules = (ModuleType[])Enum.GetValues(typeof(ModuleType));
        moduleWeights = new();
        foreach (ModuleType module in allModules)
        {
            moduleWeights.Add(module, 1);
        }
        totalWeight = allModules.Length;

        bombModules = new ModuleType[nbModules];
        for (int i = 0; i < nbModules; i++)
        {
            bombModules[i] = SelectModule();
            Debug.Log("Module " + i + " : " + bombModules[i]);
        }

        ruleHolder.Generate(bombModules);

    }

    /// <summary>
    /// Génération des modules de la bombe en fonction des modules passés en paramètre
    /// </summary>
    /// <param name="modules">Les modules en question</param>
    public void SetModules(ModuleType[] modules)
    {
        bombModules = modules;
        ruleHolder.Generate(bombModules);
    }

    /// <summary>
    /// Selection un module en fonction des poids des différents modules,
    /// Permet de réduire les chances d'avoir le même module plusieurs fois sans l'interdire
    /// </summary>
    /// <returns>Le module selectionné</returns>
    private ModuleType SelectModule()
    {
        float randomValue = Random.Range(0, totalWeight);
        float currentWeight = 0;
        foreach (ModuleType module in allModules)
        {
            currentWeight += moduleWeights[module];
            if (currentWeight >= randomValue)
            {
                AdjustWeights(module);
                return module;
            }
        }
        Debug.LogWarning("Aucun module n'a ete selectionne");
        return allModules[0];
    }

    /// <summary>
    /// Reajuste les poids des modules en fonction du module selectionné
    /// </summary>
    /// <param name="selectedModule">Le module qui a été </param>
    private void AdjustWeights(ModuleType selectedModule)
    {
        totalWeight = 0;
        moduleWeights[selectedModule] = Mathf.Max(moduleWeights[selectedModule] * weightDecreaseFactor, minimumWeight);
        totalWeight += moduleWeights[selectedModule];

        foreach (ModuleType module in allModules)
        {
            if (module != selectedModule)
            {
                moduleWeights[module] += weightIncreaseFactor;
                totalWeight += moduleWeights[module];
            }

        }
    }

    /// <summary>
    /// Genere la bombe pour l'agent 
    /// </summary>
    /// <param name="position">Position de la bombe</param>
    /// <param name="rotation">Rotation de la bombe</param>
    /// <returns>Le gameobject instancié et configuré de la bombe</returns>
    public GameObject GenerateBomb(Vector3 position, Vector3 rotation)
    {
        string resourcesPath = bombType switch
        {
            BombTypes.SIX_SLOTS => "Bomb/6Bomb",
            BombTypes.TWELVE_SLOTS => "Bomb/12Bomb",
            _ => throw new NotImplementedException(),
        };
        GameObject bomb = Instantiate(Resources.Load<GameObject>(resourcesPath), position, Quaternion.Euler(rotation));
        bomb.GetComponent<Bomb>().SetupBomb();
        bomb.GetComponent<Bomb>().SetupModules(bombModules, ruleHolder);
        bomb.GetComponent<Bomb>().StartBomb();
        return bomb;
    }

    /// <summary>
    /// Genere le manuel pour l'opérateur, pour avoir la documentation de tous les modules sur la bombe (sans doublons)
    /// </summary>
    /// <returns>Un array de visual element qui correspond aux manuels des modules</returns>
    public VisualElement[] GenerateManuel()
    {
        //TODO : Regarder si c'est bien en generant les regles des modules qui y a pas pour compliquer la tache de l'operateur
        List<VisualElement> modulesRules = new();
        ModuleType[] uniqueModules = bombModules.Distinct().ToArray();

        VisualTreeAsset[] images = Resources.LoadAll<VisualTreeAsset>("ManuelImages");
        VisualTreeAsset[] modules = Resources.LoadAll<VisualTreeAsset>("ManuelModules");

        foreach (ModuleType module in uniqueModules)
        {
            VisualElement visualElement = modules[(int)module].CloneTree();

            switch (module)
            {
                case ModuleType.WIRES:
                    visualElement.Q<WireRulesElement>().Init(ruleHolder.wireRuleGenerator);
                    break;
                case ModuleType.LABY:
                    visualElement.Q<LabyRulesElement>().Init(ruleHolder.labyRuleGenerator, images[(int)ManuelImages.LABY_TEMPLATE]);
                    break;
                case ModuleType.BUTTON:
                    visualElement.Q<ButtonRulesElement>().Init(ruleHolder.buttonRuleGenerator, images[(int)ManuelImages.BUTTON_TEMPLATE]);
                    break;
                case ModuleType.MORSE:
                    visualElement.Q<MorseRulesElement>().Init(ruleHolder.morseRuleGenerator, images[(int)ManuelImages.MORSE_TEMPLATE]);
                    break;
                case ModuleType.MATH_SYMBOL:
                    visualElement.Q<MathSymbolRulesElement>().Init(ruleHolder.mathSymbolRuleGenerator);
                    break;
                default:
                    throw new NotImplementedException();
            }

            modulesRules.Add(visualElement);
        }
        return modulesRules.ToArray();
    }

    /// <summary>
    /// Retourne le RuleHolder de la bombe
    /// </summary>
    /// <returns>L'element qui contient les regles de la bombe</returns>
    public RuleHolder GetRuleHolder()
    {
        return ruleHolder;
    }

    /// <summary>
    /// Gènere la bombe pour le tutoriel (sans lancer la bombe)
    /// </summary>
    /// <param name="position">Position de la bombe</param>
    /// <param name="rotation">Rotation de la bombe</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public GameObject GenerateBombTutorial(Vector3 position, Vector3 rotation)
    {
        string resourcesPath = bombType switch
        {
            BombTypes.SIX_SLOTS => "Bomb/6Bomb",
            BombTypes.TWELVE_SLOTS => "Bomb/12Bomb",
            _ => throw new NotImplementedException(),
        };
        GameObject bomb = Instantiate(Resources.Load<GameObject>(resourcesPath), position, Quaternion.Euler(rotation));
        bomb.GetComponent<Bomb>().SetupBomb();
        bomb.GetComponent<Bomb>().SetupModules(bombModules, ruleHolder);
        return bomb;
    }
}
