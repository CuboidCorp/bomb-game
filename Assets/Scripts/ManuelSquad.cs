using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Classe pour gérer la scene de manuel
/// </summary>
public class ManuelSquad : MonoBehaviour
{
    public static ManuelSquad Instance;

    private VisualElement[] modulesRules;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        MainGeneration.Instance.GenerateModules();
        modulesRules = MainGeneration.Instance.GenerateManuel();
    }

}
