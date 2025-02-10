using System.Collections;
using UnityEngine;

public class AgentTutoHandler : MonoBehaviour
{
    public TutorialStep[] TutorialSteps;

    private void Awake()
    {
        TutorialSteps = new TutorialStep[]
        {
            new(()=>Debug.Log("Texte1"),()=>AttendreSecondes(3f))
        };
    }

    private void Start()
    {
        MainGeneration.Instance.SetSeed(0);
        MainGeneration.Instance.GenerateModules();

        //Faire une boucle sur chaque étape du tutoriel
        //Yield return sur la couroutine de l'étape
        //A la fin on dit gg

    }

    IEnumerator AttendreSecondes(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
