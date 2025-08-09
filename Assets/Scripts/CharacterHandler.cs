using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHandler : MonoBehaviour
{
    public List<CharacterInfo> characters = new List<CharacterInfo>();

    [Serializable]
    public struct OptionEntry
    {
        public string text;
        public bool isCorrect;
    }

    public List<OptionEntry> BuildOptions(CharacterInfo character)
    {
        List<OptionEntry> options = new List<OptionEntry>();

        for (int i = 0; i < character.PoemLines.Count; i++)
        {
            OptionEntry e;
            e.text = character.PoemLines[i];
            e.isCorrect = true;
            options.Add(e);
        }

        List<string> pool = new List<string>();
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i] == null || characters[i] == character) continue;
            for (int j = 0; j < characters[i].PoemLines.Count; j++)
            {
                pool.Add(characters[i].PoemLines[j]);
            }
        }

        if (pool.Count >= 2)
        {
            int a = UnityEngine.Random.Range(0, pool.Count);
            int b;
            do { b = UnityEngine.Random.Range(0, pool.Count); } while (b == a);

            OptionEntry e1;
            e1.text = pool[a];
            e1.isCorrect = false;
            options.Add(e1);

            OptionEntry e2;
            e2.text = pool[b];
            e2.isCorrect = false;
            options.Add(e2);
        }
        else
        {
            OptionEntry e1;
            e1.text = pool[0];
            e1.isCorrect = false;
            options.Add(e1);

            OptionEntry e2;
            e2.text = pool[0];
            e2.isCorrect = false;
            options.Add(e2);
        }

        return options;
    }
}