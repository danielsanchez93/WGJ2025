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


    // Dedicated noise AudioSource whose volume can be lowered on correct answers.
    public AudioSource NoiseSource;

    // Tracks the number of correct answers.
    public int correctAnswers = 0;

    // A list of AudioClip arrays, where each array contains 5 clips for one level.
    public List<CharacterInfo> levelCharacters;

    public int correctCount = 0;

    public bool debugMode;
    public int levelIndex;

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

    private void Update()
    {
        if (debugMode)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                SetLevelAudioClips(levelIndex);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                AnswerFeedback(true);
            }
        }
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
    public void AnswerFeedback(bool correct)
    {
        if (!correct)
        {
            // Wrong answer—no changes
            return;
        }

        correctAnswers += 1; 

        AudioSource targetSource = null;

        switch (correctAnswers)
        {
            case 1:
                targetSource = AudioSource1;
                break;
            case 2:
                targetSource = AudioSource2;
                break;
            case 3:
                targetSource = AudioSource3;
                break;
            case 4:
                targetSource = AudioSource4;
                break;
            default:
                Debug.LogWarning("AudioManager: Invalid sourceNumber passed to AnswerFeedback()");
                break;
        }

        if (targetSource != null)
        {
            if (!targetSource.isPlaying)
            {
                targetSource.loop = true; // make sure it loops
                targetSource.Play();
            }
        }

        // Lower the noise volume by -2 dB each correct answer
        if (NoiseSource != null)
        {
            float reductionFactor = Mathf.Pow(10f, -6f / 20f); // ~0.7943
            NoiseSource.volume *= reductionFactor;
            NoiseSource.volume = Mathf.Max(0f, NoiseSource.volume); // no negative values
        }
        else
        {
            Debug.LogWarning("AudioManager: NoiseSource is not assigned.");
        }

        correctAnswers++;
    }

    /// <summary>
    /// Overwrites the AudioClips assigned to the AudioSources based on a level index.
    /// </summary>
    /// <param name="levelIndex">Index of the level whose audio should be loaded.</param>
    public void SetLevelAudioClips(int inLevelIndex)
    {
        levelIndex = inLevelIndex;
        // Validate index.
        if (levelIndex < 0 || levelIndex >= levelCharacters.Count)
        {
            Debug.LogWarning("AudioManager: Invalid levelIndex passed to SetLevelAudioClips().");
            return;
        }

        var clipsForLevel = levelCharacters[levelIndex].AudioClips;

        // Validate that we have exactly 5 clips for this level.
        if (clipsForLevel.Count != 4)
        {
            Debug.LogWarning("AudioManager: Each level must have exactly 5 audio clips assigned.");
            return;
        }

        // Assign clips to AudioSources.
        AudioSource1.clip = clipsForLevel[0];
        AudioSource2.clip = clipsForLevel[1];
        AudioSource3.clip = clipsForLevel[2];
        AudioSource4.clip = clipsForLevel[3];
       
    }
}

