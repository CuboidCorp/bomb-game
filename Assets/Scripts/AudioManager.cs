using UnityEngine;

/// <summary>
/// Classe pour gerer toute la musique et les sounds effects
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundEffects;
    private AudioSource audioSource;

    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Joue un sound effect a un volume specifique
    /// </summary>
    /// <param name="soundEffect">Le sound effect a jouer</param>
    /// <param name="volume">Le volume du sound effect</param>
    public void PlaySoundEffect(SoundEffects soundEffect, int volume = 1)
    {
        audioSource.PlayOneShot(soundEffects[(int)soundEffect], 1);
    }

}
