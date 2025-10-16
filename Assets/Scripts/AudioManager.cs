using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Clips")]
    public AudioClip MainMenuMusic;
    public AudioClip DayPhaseMusic;
    public AudioClip FireSound;
    public AudioClip CollectWoodSoundFx;
    public AudioClip CollectGunPartSoundFx;
    public AudioClip[] NightPhaseGhostSoundFxs;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource m_musicSource; // For looping music
    [SerializeField] private AudioSource m_sfxSource;   // For sound effects

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMainMenuMusic()
    {
        m_musicSource.clip = MainMenuMusic;
        m_musicSource.Play();
    }
    
    public void PlayDayPhaseMusic()
    {
        m_musicSource.clip = DayPhaseMusic;
        m_musicSource.Play();
    }

    public void PlayFireSound()
    {
        m_musicSource.clip = FireSound;
        m_musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        m_sfxSource.PlayOneShot(clip);
    }

    public void PlayRandomNightPhaseGhostSoundFx()
    {
        if (NightPhaseGhostSoundFxs != null && NightPhaseGhostSoundFxs.Length > 0)
        {
            m_sfxSource.PlayOneShot(NightPhaseGhostSoundFxs[Random.Range(0, NightPhaseGhostSoundFxs.Length)]);
        }
    }
}