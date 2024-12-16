using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class MainGeneration : MonoBehaviour
{
    [SerializeField] private int seed = 0;
    [SerializeField] private BombTypes bombType;

    public bool isDebug = false;

    public static MainGeneration Instance;

    private RuleHolder ruleHolder;


    private ModuleType[] modules;

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

    public void GenerateModules()
    {
        int nbModules = bombType switch
        {
            BombTypes.SIX_SLOTS => 5,
            BombTypes.TWELVE_SLOTS => 11,
            _ => throw new NotImplementedException(),
        };

        modules = new ModuleType[nbModules];
        for (int i = 0; i < nbModules; i++)
        {
            modules[i] = (ModuleType)Random.Range(0, Enum.GetValues(typeof(ModuleType)).Length);
            Debug.Log("Module " + i + " : " + modules[i]);
        }

        ruleHolder.Generate(modules); //TODO : On genere plusieurs fois des modules identiques, a changer 
    }


    public GameObject GenerateBomb(Vector3 position, Vector3 rotation, string resourcesPath)
    {
        GameObject bomb = Instantiate(Resources.Load<GameObject>(resourcesPath), position, Quaternion.Euler(rotation));
        bomb.GetComponent<Bomb>().SetupBomb();
        bomb.GetComponent<Bomb>().SetupModules(modules, ruleHolder);
        bomb.GetComponent<Bomb>().StartBomb();
        return bomb;
    }

    public VisualElement[] GenerateManuel()
    {
        List<VisualElement> modulesRules = new();
        List<ModuleType> differentModulesList = new();

        VisualTreeAsset[] images = Resources.LoadAll<VisualTreeAsset>("ManuelImages");
        VisualTreeAsset[] visualTreeAssets = Resources.LoadAll<VisualTreeAsset>("ManuelModules");

        foreach (ModuleType module in modules)
        {
            if (!differentModulesList.Contains(module))
            {
                differentModulesList.Add(module);
            }
        }

        foreach (ModuleType module in differentModulesList)
        {
            VisualElement visualElement = visualTreeAssets[(int)module].CloneTree();

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
                default:
                    throw new NotImplementedException();
            }

            modulesRules.Add(visualElement);
        }

        return modulesRules.ToArray();
    }

}
