using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Classe pour gerer le timer de la bombe
/// </summary>
public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text strikesText;

    private int nbSeconds;
    private float timeBetweenSeconds = 1f;

    private Coroutine timerCoroutine;

    public UnityEvent TimerFinished;

    private void OnDisable()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
    }

    /// <summary>
    /// Start le timer avec le temps donné
    /// </summary>
    /// <param name="time">Le temps de départ de la bombe</param>
    public void StartTimer(TimeSpan time)
    {
        Debug.Log("Starting timer with time : " + time.ToString());
        timerText.text = time.ToString("mm\\:ss");
        nbSeconds = (int)time.TotalSeconds;
        strikesText.text = "";
        timeBetweenSeconds = 1f;

        timerCoroutine = StartCoroutine(TimeTickingDown());
    }

    /// <summary>
    /// Ajoute un strike au timer et divise le temps entre chaque seconde par 2
    /// </summary>
    public void AddStrike()
    {
        strikesText.text += "X";
        timeBetweenSeconds /= 1.5f;
    }

    private IEnumerator TimeTickingDown()
    {
        while (nbSeconds > 0)
        {
            yield return new WaitForSeconds(timeBetweenSeconds);
            AudioManager.Instance.PlaySoundEffect(SoundEffects.BOMB_BEEP);
            nbSeconds--;
            timerText.text = TimeSpan.FromSeconds(nbSeconds).ToString("mm\\:ss");
        }
        TimerFinished.Invoke();
    }
}
