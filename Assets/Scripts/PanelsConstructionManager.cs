using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class PanelsConstructionManager : MonoBehaviour
{
    public CharacterHandler source;
    public Transform container;
    public ButtonConstructor buttonPrefab;
    public Image portraitTarget;

    private List<CharacterInfo> characters = new List<CharacterInfo>();

    private void Start()
    {
        characters = GameManager.Instance.characters;
    }

    public void Build(CharacterInfo character)
    {
        Clear();

        if (portraitTarget != null)
        {
            portraitTarget.sprite = character != null ? character.Portrait : null;
        }

        if (character == null || buttonPrefab == null || container == null) return;

        List<string> correct = new List<string>();
        for (int i = 0; i < character.PoemLines.Count; i++)
        {
            correct.Add(character.PoemLines[i]);
        }

        List<string> correctPick = new List<string>();
        if (correct.Count >= 4)
        {
            List<int> idxs = new List<int>();
            while (idxs.Count < 4)
            {
                int r = Random.Range(0, correct.Count);
                if (!idxs.Contains(r)) idxs.Add(r);
            }
            for (int i = 0; i < idxs.Count; i++)
            {
                correctPick.Add(correct[idxs[i]]);
            }
        }
        else
        {
            for (int i = 0; i < correct.Count; i++) correctPick.Add(correct[i]);
            while (correctPick.Count < 4 && correct.Count > 0)
            {
                int r = Random.Range(0, correct.Count);
                correctPick.Add(correct[r]);
            }
        }

        List<string> othersPool = new List<string>();
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i] == null || characters[i] == character) continue;
            for (int j = 0; j < characters[i].PoemLines.Count; j++)
            {
                othersPool.Add(characters[i].PoemLines[j]);
            }
        }

        List<string> wrongPick = new List<string>();
        if (othersPool.Count >= 2)
        {
            int a = Random.Range(0, othersPool.Count);
            int b;
            do { b = Random.Range(0, othersPool.Count); } while (b == a);
            wrongPick.Add(othersPool[a]);
            wrongPick.Add(othersPool[b]);
        }
        else if (othersPool.Count == 1)
        {
            wrongPick.Add(othersPool[0]);
            wrongPick.Add(othersPool[0]);
        }

        List<(string, bool)> entries = new List<(string, bool)>();
        for (int i = 0; i < correctPick.Count; i++) entries.Add((correctPick[i], true));
        for (int i = 0; i < wrongPick.Count; i++) entries.Add((wrongPick[i], false));

        Shuffle(entries);

        for (int i = 0; i < entries.Count; i++)
        {
            ButtonConstructor btn = Instantiate(buttonPrefab, container);
            btn.Setup(entries[i].Item1, entries[i].Item2);
        }
    }

    public void Clear()
    {
        for (int i = container.childCount - 1; i >= 0; i--)
        {
            GameObject go = container.GetChild(i).gameObject;
            Destroy(go);
        }
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(i, list.Count);
            T tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }
    }
}
