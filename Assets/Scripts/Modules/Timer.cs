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

    public static Timer instance;

    public int nbSeconds;
    private float timeBetweenSeconds = 1f;

    private Coroutine timerCoroutine;

    public UnityEvent TimerFinished;

    private void Awake()
    {
        instance = this;
    }


    private void OnDisable()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
    }

    /// <summary>
    /// Start le timer avec le temps donn�
    /// </summary>
    /// <param name="time">Le temps de d�part de la bombe</param>
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
    /// Setup le timer avec le temps donné sans le lancer
    /// </summary>
    /// <param name="time">Le temps de départ de la bombe</param>
    public void SetupTimer(TimeSpan time)
    {
        timerText.text = time.ToString("mm\\:ss");
        nbSeconds = (int)time.TotalSeconds;
        strikesText.text = "";
        timeBetweenSeconds = 1f;
    }

    /// <summary>
    /// Lance le timer
    /// </summary>
    public void LaunchTimer()
    {
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

    /// <summary>
    /// Coroutine pour faire descendre le timer
    /// </summary>
    private IEnumerator TimeTickingDown()
    {
        while (nbSeconds > 0)
        {
            yield return new WaitForSecondsRealtime(timeBetweenSeconds);
            AudioManager.Instance.PlaySoundEffect(SoundEffects.BOMB_BEEP, 0.5f);
            nbSeconds--;
            timerText.text = TimeSpan.FromSeconds(nbSeconds).ToString("mm\\:ss");
        }
        TimerFinished.Invoke();
    }

    /// <summary>
    /// Stop le timer
    /// </summary>
    public void StopTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
    }

    /// <summary>
    /// Retourne le temps restant sur le timer
    /// </summary>
    /// <returns>Le temps restant sur le timer en secondes</returns>
    public int GetTimeLeft()
    {
        return nbSeconds;
    }

}
