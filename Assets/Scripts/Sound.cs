using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
/// <summary>
/// An Entity Object class created for sound clips to hold various attributes. 
/// </summary>
public class Sound
{
    /// <summary>
    /// A variable that holds the name of the sound track.
    /// </summary>
    public string name;

    /// <summary>
    /// A variable that holds the actual MP3 audio clip.
    /// </summary>
    public AudioClip clip;

    /// <summary>
    /// A variable that can be used to adjust the audio clip's volume between 0 to 1.
    /// </summary>
    [Range(0f, 1f)]
    public float volume;

    /// <summary>
    /// A variable that can be used to adjust the audio clip's pitch between 0.1 to 3.
    /// </summary>
    [Range(0.1f, 3f)]
    public float pitch;

    /// <summary>
    /// A variable that is used to play, pause or stop the audio clip.
    /// </summary>
    [HideInInspector]
    public AudioSource source;

    /// <summary>
    /// A variable that is used to store the whether the audio should replay when it finishes.
    /// </summary>
    public bool loop;
}
