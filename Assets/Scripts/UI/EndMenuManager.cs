using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EndMenuManager : MonoBehaviour
{
    public static EndMenuManager Instance;

    private UIDocument doc;

    private Label title;

    private Label timerTime;
    private Label nbStrikesLabel;
    private Label scoreLabel;

    private Button shareBtn;
    private Button quitBtn;

    private void Awake()
    {
        Instance = this;
        doc = GetComponent<UIDocument>();
    }

    /// <summary>
    /// Ouvre le menu de fin et affiche les stats de la partie
    /// </summary>
    /// <param name="timeLeftS">Le temps restant sur le timer en secondes</param>
    /// <param name="nbStrikes">Le nombre d'erreurs</param>
    /// <param name="isVictory">Si la partie est réussie ou non</param>
    public void OpenEndMenu(int timeLeftS, int nbStrikes, bool isVictory)
    {
        doc.enabled = true;
        title = doc.rootVisualElement.Q<Label>("title");
        timerTime = doc.rootVisualElement.Q<Label>("timerTime");
        nbStrikesLabel = doc.rootVisualElement.Q<Label>("nbStrikes");
        scoreLabel = doc.rootVisualElement.Q<Label>("score");

        shareBtn = doc.rootVisualElement.Q<Button>("shareBtn");
        quitBtn = doc.rootVisualElement.Q<Button>("quitBtn");

        shareBtn.clicked += ShareScore;
        quitBtn.clicked += Quit;

        int score = CalculateScore(timeLeftS, nbStrikes, isVictory);

        timerTime.text = timeLeftS.ToString() + " s";
        nbStrikesLabel.text = nbStrikes.ToString();
        scoreLabel.text = score.ToString();

        if (isVictory)
        {
            title.text = TextLocalizationHandler.LoadString("PauseMenu", "END_WIN");
        }
        else
        {
            title.text = TextLocalizationHandler.LoadString("PauseMenu", "END_GAMEOVER");
        }
    }



    /// <summary>
    /// Calcule le score en fonction du temps restant et du nombre de strikes
    /// </summary>
    /// <param name="timeLeftS">Temps restant en secondes</param>
    /// <param name="nbStrikes">Nombre de strikes</param>
    /// <returns>Le score</returns>
    private int CalculateScore(int timeLeftS, int nbStrikes, bool isVictory)
    {
        if (isVictory)//TODO  : Changer le calcul du score
        {
            return timeLeftS * 10 - nbStrikes * 50;
        }

        return -(timeLeftS * 10 + nbStrikes * 50);
    }

    /// <summary>
    /// Partager le score sur les scoreboard
    /// </summary>
    private void ShareScore()
    {
        //TODO : Stockage et voir les scoreboard
        Debug.Log("Share score");
    }

    /// <summary>
    /// Quitter vers le menu principal
    /// </summary>
    private void Quit()
    {
        Destroy(MainGeneration.Instance.gameObject);
        //TODO : Faire un truc propre : 
        Camera.main.transform.GetComponent<BombInteract>().enabled = false;

        SceneManager.LoadScene("LevelSelect");
    }
}
