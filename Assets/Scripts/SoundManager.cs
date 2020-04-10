using UnityEngine.Audio;
using System;
using UnityEngine;

/// <summary>
/// A class that holds all the methods related to playing, pausing or stopping a audio clip in the game.
/// </summary>
public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// An array that holds the list of sound clips used throughout the game.
    /// </summary>
    public Sound[] sounds;

    /// <summary>
    /// This method is called everytime when the Game Launches, the Awake() method focuses on waking up all the sound clips and prepare to play them throughout the game. The Audio clips' volume, pitch and loop are initialized.
    /// </summary>
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    /// <summary>
    /// A method to responsible to play the audio clip for a given audio clip name.
    /// </summary>
    /// <param name="name">The name of the audio clip.</param>
    public void Play(string name)
    {
        AudioSource audioSource = audioSource = GetComponent<AudioSource>();
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        if (!s.source.isPlaying)
            s.source.Play();
        if (name == "CoinCollection")
        {
            audioSource.PlayOneShot(s.clip, 0.7f);
        }
    }

    /// <summary>
    /// The method is responsible to pause the audio clip whenever neccessary. The next Play() on this audio track will resume from the point it is paused.
    /// </summary>
    /// <param name="name">The name of the audio clip.</param>
    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Pause();
    }

    /// <summary>
    /// The method responsible to stop the audio clip from playing whenever neccessary. The next Play() on this audio track will start from the beginning of the audio.
    /// </summary>
    /// <param name="name">The name of the audio clip.</param>
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }
}
