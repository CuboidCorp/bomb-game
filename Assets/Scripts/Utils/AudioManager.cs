using UnityEngine;

/// <summary>
/// Classe pour gerer toute la musique et les sounds effects
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundEffects;

    [SerializeField] private AudioSource audioSourceSFX;
    [SerializeField] private AudioSource audioSourceMusic;

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Joue un sound effect a un volume specifique
    /// </summary>
    /// <param name="soundEffect">Le sound effect a jouer</param>
    /// <param name="volume">Le volume du sound effect</param>
    public void PlaySoundEffect(SoundEffects soundEffect, int volume = 1)
    {
        audioSourceSFX.PlayOneShot(soundEffects[(int)soundEffect], volume);
    }

}
