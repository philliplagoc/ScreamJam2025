using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds;
    public Sound[] sfxSounds;

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

        // Create AudioSource components for all music clips
        foreach (Sound s in musicSounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.Clip;
            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
            s.Source.loop = s.loop;
        }

        // Create AudioSource components for all sound effect clips
        foreach (Sound s in sfxSounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.Clip;
            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
            s.Source.loop = s.loop;
        }
    }

    public void PlayMusic(string name)
    {
        // Find the sound in the musicSounds array with the matching name
        Sound s = Array.Find(musicSounds, sound => sound.Name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        
        // Stop all other music before playing the new one
        foreach (Sound music in musicSounds)
        {
            music.Source.Stop();
        }

        s.Source.Play();
    }
    
    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, sound => sound.Name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.Source.PlayOneShot(s.Clip);
    }
}