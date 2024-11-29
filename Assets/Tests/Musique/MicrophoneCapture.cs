using System.Collections;
using UnityEngine;

public class MicrophoneCapture : MonoBehaviour
{
    private AudioSource audioSource;
    private string microphoneName;
    private bool isRecording = false;
    string[] noteNames = { "Do", "Do#", "Ré", "Ré#", "Mi", "Fa", "Fa#", "Sol", "Sol#", "La", "La#", "Si" };
    [SerializeField] private float amplitudeThreshold = 0.005f; // Seuil d'amplitude minimale
    private Coroutine recordingCoroutine;
    [SerializeField] private AudioClip[] audioClips;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomClip()
    {
        PlayAndAnalyzeAudioClip(audioClips[Random.Range(0, audioClips.Length)]);
    }

    private void PlayAndAnalyzeAudioClip(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("Aucun fichier audio n'a été fourni !");
            return;
        }

        if (isRecording)
        {
            Debug.LogWarning("Une analyse est déjà en cours !");
            return;
        }

        audioSource.clip = clip;
        audioSource.Play(); // Joue le fichier audio
        isRecording = true;

        Debug.Log($"Lecture et analyse de {clip.name} commencé !");
    }


    public void StartRecording()
    {
        if (Microphone.devices.Length > 0)
        {
            if (!isRecording)
            {
                microphoneName = Microphone.devices[0]; // Utilise le premier micro trouvé
                Debug.Log($"Microphone détecté : {microphoneName}");
                recordingCoroutine = StartCoroutine(RecordCoroutine());
            }
            else
            {
                Debug.LogWarning("Un enregistrement est déjà en cours !");
            }
        }
        else
        {
            Debug.LogWarning("Aucun microphone détecté !");
        }
    }

    private IEnumerator RecordCoroutine()
    {
        audioSource.clip = Microphone.Start(microphoneName, true, 10, 44100); // Enregistrement de 10 secondes à 44.1 kHz
        audioSource.loop = true;

        // Attendre que le micro démarre
        while (!(Microphone.GetPosition(microphoneName) > 0))
        {
            yield return null; // Attend un frame avant de réessayer
        }

        audioSource.Play(); // Joue le son capturé
        isRecording = true;
        Debug.Log("Enregistrement commencé !");
    }

    public void StopRecording()
    {
        if (isRecording)
        {
            isRecording = false;
            StopCoroutine(recordingCoroutine);
            Microphone.End(microphoneName);
            audioSource.Stop();
            Debug.Log("Enregistrement arrêté !");
        }
    }

    void Update()
    {
        if (isRecording)
        {
            DebugAudioInfo();
        }
    }

    private void DebugAudioInfo()
    {
        // Récupérer les données audio
        float[] samples = new float[1024];
        audioSource.GetOutputData(samples, 0);

        // Calculer la fréquence dominante
        float[] spectrum = new float[1024];
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        float maxVal = 0f;
        int maxIndex = 0;

        for (int i = 0; i < spectrum.Length; i++)
        {
            if (spectrum[i] > maxVal)
            {
                maxVal = spectrum[i];
                maxIndex = i;
            }
        }

        // Si l'amplitude est trop faible, ne pas afficher les informations
        if (maxVal < amplitudeThreshold)
        {
            Debug.Log("Amplitude trop faible, aucune donnée utile détectée.");
            return;
        }

        // Convertir l'index de la fréquence en Hertz
        float freq = maxIndex * AudioSettings.outputSampleRate / 2 / spectrum.Length;

        int midi = FrequencyToMidi(freq);

        // Afficher des informations utiles dans la console
        //Debug.Log($"Amplitude max : {maxVal}");
        Debug.Log($"Fréquence dominante : {freq} Hz");
        Debug.Log($"Note MIDI : {midi} ({GetNoteName(midi)})");
    }

    int FrequencyToMidi(float frequency)
    {
        return Mathf.RoundToInt(12 * Mathf.Log(frequency / 440f, 2) + 69);
    }

    string GetNoteName(int midiNumber)
    {
        int noteIndex = (midiNumber - 12) % 12; // MIDI 12 correspond au premier Do
        return noteNames[noteIndex];
    }
}
