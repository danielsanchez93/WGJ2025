using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Audio Manager (singleton) adapted from Daggerhart Lab example.
/// Manages up to five AudioSources plus an optional noise source.
/// </summary>
public class AudioManager : MonoBehaviour
{
    // Singleton instance.
    public static AudioManager Instance = null;

    // Audio player components.
    // Five versatile audio sources for custom audio playback.
    public AudioSource AudioSource1;
    public AudioSource AudioSource2;
    public AudioSource AudioSource3;
    public AudioSource AudioSource4;
    public AudioSource AudioSource5;

    // Dedicated noise AudioSource whose volume can be lowered on correct answers.
    public AudioSource NoiseSource;

    // Tracks the number of correct answers.
    public int correctAnswers = 0;

    // A list of AudioClip arrays, where each array contains 5 clips for one level.
    public List<AudioClip[]> levelAudioClips = new List<AudioClip[]>();

    // Initialize the singleton instance.
    private void Awake()
    {
        // If there is not already an instance of AudioManager, set it to this.
        if (Instance == null)
        {
            Instance = this;
        }
        // If an instance already exists, destroy this to enforce the singleton.
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Keep this object across scene loads.
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Play a clip on a specified AudioSource.
    /// </summary>
    /// <param name="clip">AudioClip to play.</param>
    /// <param name="sourceNumber">AudioSource slot number (1–5).</param>
    public void Play(AudioClip clip, int sourceNumber)
    {
        switch (sourceNumber)
        {
            case 1:
                AudioSource1.clip = clip;
                AudioSource1.Play();
                break;
            case 2:
                AudioSource2.clip = clip;
                AudioSource2.Play();
                break;
            case 3:
                AudioSource3.clip = clip;
                AudioSource3.Play();
                break;
            case 4:
                AudioSource4.clip = clip;
                AudioSource4.Play();
                break;
            case 5:
                AudioSource5.clip = clip;
                AudioSource5.Play();
                break;
            default:
                Debug.LogWarning("AudioManager: Invalid sourceNumber passed to Play()");
                break;
        }
    }

    /// <summary>
    /// Handle responses: if wrong, do nothing; if correct, play clip, lower noise volume, and increment correctAnswers.
    /// </summary>
    /// <param name="clip">AudioClip to play on correct answer.</param>
    /// <param name="sourceNumber">AudioSource slot (1–5) to use for playback.</param>
    /// <param name="loweredVolume">Volume level to set for the NoiseSource.</param>
    /// <param name="correct">Whether the player's answer is correct.</param>
    public void AnswerFeedback(AudioClip clip, int sourceNumber, float loweredVolume, bool correct)
    {
        if (!correct)
        {
            // Wrong answer—no action taken, no increment in correctAnswers.
            return;
        }

        // Right answer—play the clip.
        Play(clip, sourceNumber);

        // Lower the noise volume.
        if (NoiseSource != null)
        {
            NoiseSource.volume = loweredVolume;
        }
        else
        {
            Debug.LogWarning("AudioManager: NoiseSource is not assigned.");
        }

        // Increment the correct answer counter.
        correctAnswers++;
    }

    /// <summary>
    /// Overwrites the AudioClips assigned to the AudioSources based on a level index.
    /// </summary>
    /// <param name="levelIndex">Index of the level whose audio should be loaded.</param>
    public void SetLevelAudioClips(int levelIndex)
    {
        // Validate index.
        if (levelIndex < 0 || levelIndex >= levelAudioClips.Count)
        {
            Debug.LogWarning("AudioManager: Invalid levelIndex passed to SetLevelAudioClips().");
            return;
        }

        AudioClip[] clipsForLevel = levelAudioClips[levelIndex];

        // Validate that we have exactly 5 clips for this level.
        if (clipsForLevel.Length != 5)
        {
            Debug.LogWarning("AudioManager: Each level must have exactly 5 audio clips assigned.");
            return;
        }

        // Assign clips to AudioSources.
        AudioSource1.clip = clipsForLevel[0];
        AudioSource2.clip = clipsForLevel[1];
        AudioSource3.clip = clipsForLevel[2];
        AudioSource4.clip = clipsForLevel[3];
        AudioSource5.clip = clipsForLevel[4];
    }
}

