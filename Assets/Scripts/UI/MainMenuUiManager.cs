using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuUiManager : MonoBehaviour
{
    private UIDocument doc;
    private IntegerField seedField;
    private Button randSeedBtn;

    private Button startOpGameBtn;
    private Button startAgtGameBtn;

    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        seedField = doc.rootVisualElement.Q<IntegerField>("seed");
        randSeedBtn = doc.rootVisualElement.Q<Button>("randomizeSeed");

        startOpGameBtn = doc.rootVisualElement.Q<Button>("startOpBtn");
        startAgtGameBtn = doc.rootVisualElement.Q<Button>("startAgtBtn");
    }

    private void OnEnable()
    {
        randSeedBtn.clicked += RandomizeSeed;

        startOpGameBtn.clicked += StartOperatorGame;
        startAgtGameBtn.clicked += StartAgentGame;
    }

    private void OnDisable()
    {
        randSeedBtn.clicked -= RandomizeSeed;

        startOpGameBtn.clicked -= StartOperatorGame;
        startAgtGameBtn.clicked -= StartAgentGame;
    }

    /// <summary>
    /// Randomise le seed
    /// </summary>
    private void RandomizeSeed()
    {
        seedField.value = Random.Range(0, 100000);
    }

    private void GenerateSeedHolder()
    {
        int seed = seedField.value;

        GameObject mainGen = Instantiate(Resources.Load<GameObject>("MainGen"));

        mainGen.GetComponent<MainGeneration>().SetSeed(seed);
    }

    /// <summary>
    /// Start la scene de manuel
    /// </summary>
    private void StartOperatorGame()
    {
        Debug.Log("Starting Operator Game");
        if (MainGeneration.Instance == null)
        {
            GenerateSeedHolder();
        }
        SceneManager.LoadScene("Operator");
    }

    /// <summary>
    /// Start la scene de desamorcage
    /// </summary>
    private void StartAgentGame()
    {
        Debug.Log("Starting Agent Game");
        if (MainGeneration.Instance == null)
        {
            GenerateSeedHolder();
        }
        SceneManager.LoadScene("Agent");
    }

}
