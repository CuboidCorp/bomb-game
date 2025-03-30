using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DigitalTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    void Start()
    {
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (true)
        {
            DateTime now = DateTime.Now;
            timerText.text = now.ToString("HH:mm");
            yield return new WaitForSeconds(0.5f);

            timerText.text = now.ToString("HH mm");
            yield return new WaitForSeconds(0.5f);
        }
    }
}
