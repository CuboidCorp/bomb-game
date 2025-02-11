using System.Collections;
using UnityEngine;

public class AgentTutoHandler : MonoBehaviour
{
    public TutorialStep[] TutorialSteps;

    private const string LOCALIZATION_TUTO_TABLE = "";

    private void Awake()
    {
        TutorialSteps = new TutorialStep[]
        {
            new(()=>Debug.Log("Texte1"),()=>WaitForSeconds(3f)),
            new(()=>FirstTutoStep(),()=>WaitForSeconds(3f))
        };

        //Que sont les vrais étapes du tuto
        //1- Bienvenue au tutoriel en tant qu'agent, l'objectif est de desamorcer la bombe, condition espace ou 5s
        //2- Il fait très sombre ici, allumer la flashlight , condition appuyer sur la touche pour la flashlight
        //3- Bon maintenant il faut apprendre à désamorcer une bombe , condition espace ou 5s
        //4- Spawn la bombe et dit voici la bombe que vous devez desamorcer, elle est composée de modules condition espace ou 5s
        //5- Description du timer -condition espace ou 5s
        //6- Description d'un module et on dit assez parler il faut commencer à le desamorcer condition espace ou 5s
        //7- On decrit comment desamorcer le module condition désamorçage du module
        //8- Description d'un autre module condition espace ou 5s
        //9- Activation du timer si le timer va a 0 9bis et on revient à 9
        //9bis - Boom, nan je trolle tu vas pas mourir sur le tuto quand même
        //10- Bref avant que la bombe explose faut desamorcer ce module 
        //11- On dit un truc faux genre et il se prend un strike
        //12- Oups dsl c'était ça qu'il fallait faire et on desamorce le module
        //12 bis - Si il a juste, ahah je m'étais trompé mais faut ecouter ses potes normalement, on force le strike
        //13 - Maintenant on a un strike le temps va plus vite 
        //14 - Bref le jeu est simple communique avec ton camarade et desamorce la bombe 
        //15 - D'ailleurs j'ai trop parlé explosion, maintenant va et desamorce des bombres
    }

    private void Start()
    {
        MainGeneration.Instance.SetSeed(0);
        MainGeneration.Instance.GenerateModules();

        StartCoroutine(RunTutorial());

    }

    /// <summary>
    /// Coroutine qui permet de lancer le tutoriel
    /// </summary>
    /// <returns>Quand le tutoriel est terminé</returns>
    private IEnumerator RunTutorial()
    {
        foreach(TutorialStep step in TutorialSteps) //TODO : Pas un foreach mais à la fin de chaque etape on va vers une autre etape
        {
            step.StepAction?.Invoke();

            yield return StartCoroutine(step.WaitCondition());
        }
    }

    #region

    private void FirstTutoStep()
    {
        Debug.Log("Texte2");
    }

    #endregion

    IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    IEnumerator WaitForAnyCondition(params Func<bool>[] conditions)
    {
        while (!conditions.Any(condition => condition()))
        {
            yield return null; // attendre la frame suivante
        }
    }
}
