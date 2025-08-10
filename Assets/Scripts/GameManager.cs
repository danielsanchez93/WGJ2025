using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public PanelsConstructionManager panels;
    public GameObject nextPanel;
    public GameObject poemsPanel;
    public GameObject winPanel;
    public GameObject losePanel;
    public CharacterHandler characterHandler;
    public List<CharacterInfo> characters = new List<CharacterInfo>();
    public int index;

    public static GameManager Instance;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    void Start()
    {
        index = 0;
        BuildCurrent();
    }

    public void BuildCurrent()
    {
        CharacterInfo character = characters[index];
        panels.Build(character);
        BuildAudio();    
    }

    public void BuildAudio()
    {

        // NEW: Set audio clips for this level
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetLevelAudioClips(index);
        }
    }

    public void NextCharacter()
    {
        if (index >= characters.Count)
        {
            Debug.Log("Game Over");
            ///AQUI VA EL FINAL DEL JUEGO
            bool win = FindAnyObjectByType<CurrentChapterManager>().CheckGameEnd();
            if (win)
            {
                winPanel.SetActive(true);
            }
            else
            {
                losePanel.SetActive(true);
            }
        }
        else
        {
            index++;
            PoemArrangeManager.instance.ClearPoem();
            BuildCurrent();
        }

    }

    public void ReturnoToMenu()
    {
        SceneManager.LoadScene("Main");
    }

    public void PreviousCharacter()
    {
        if (index < 0)
        {
            Debug.Log("Nel Prro");
        }

        index--;
        BuildCurrent();
    }

    public void ShowOptionsPanel()
    {
        nextPanel.SetActive(true);
        poemsPanel.SetActive(false);
    }

    public void ShowPoemPanel()
    {
        poemsPanel.SetActive(true);
        nextPanel.SetActive(false);
    }

    public void SetCharacterIndex(int newIndex)
    {
        if (characters == null || characters.Count == 0) return;
        if (newIndex < 0) newIndex = 0;
        if (newIndex >= characters.Count) newIndex = characters.Count;
        index = newIndex;
        BuildCurrent();
        
    }
}