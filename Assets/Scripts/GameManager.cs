using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PanelsConstructionManager panels;
    public CharacterHandler characterHandler;
    public List<CharacterInfo> characters = new List<CharacterInfo>();
    public List<Sprite> characterSprites = new List<Sprite>();
    public int index;

    void Start()
    {
        if (panels != null && characterHandler != null) BuildCurrent();
    }

    public void BuildCurrent()
    {
        if (characters.Count == 0) return;
        index = Mathf.Clamp(index, 0, characters.Count - 1);
        var ch = characters[index];
        Sprite sp = null;
        if (index >= 0 && index < characterSprites.Count) sp = characterSprites[index];
        panels.Build(ch, sp, ch.Clip);
    }

    public void NextCharacter()
    {
        if (characters.Count == 0) return;
        index = (index + 1) % characters.Count;
        BuildCurrent();
    }

    public void SetCharacterIndex(int newIndex)
    {
        if (characters.Count == 0) return;
        index = Mathf.Clamp(newIndex, 0, characters.Count - 1);
        BuildCurrent();
    }
}