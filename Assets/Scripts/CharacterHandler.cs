using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHandler : MonoBehaviour
{
    public List<CharacterInfo> characters = new List<CharacterInfo>();
    public bool ignoreEmpty = true;
    public int randomSeed = 0;

    [Serializable]
    public class CharacterPoemBundle
    {
        public string characterName;
        public List<string> ownLines = new List<string>();
        public List<string> randomOtherLines = new List<string>();
    }

    [SerializeField] private List<CharacterPoemBundle> preview = new List<CharacterPoemBundle>();

    private System.Random rng;

    void Awake()
    {
        rng = (randomSeed != 0) ? new System.Random(randomSeed) : new System.Random();
        BuildAllBundlesForPreview();
    }

    private void BuildAllBundlesForPreview()
    {
        preview.Clear();
        foreach (var ch in characters)
        {
            if (ch == null) continue;
            preview.Add(BuildBundleFor(ch));
        }
    }

    public CharacterPoemBundle BuildBundleFor(CharacterInfo character)
    {
        var bundle = new CharacterPoemBundle { characterName = character.CharacterName };

        foreach (var line in character.PoemLines)
        {
            var trimmed = line?.Trim() ?? string.Empty;
            if (ignoreEmpty && string.IsNullOrEmpty(trimmed)) continue;
            bundle.ownLines.Add(trimmed);
        }

        int needed = bundle.ownLines.Count;
        if (needed == 0) return bundle;

        var othersPool = new List<string>();
        foreach (var other in characters)
        {
            if (other == null || other == character) continue;
            foreach (var line in other.PoemLines)
            {
                var t = line?.Trim() ?? string.Empty;
                if (ignoreEmpty && string.IsNullOrEmpty(t)) continue;
                othersPool.Add(t);
            }
        }

        if (othersPool.Count == 0) return bundle;

        if (othersPool.Count >= needed)
        {
            for (int i = 0; i < othersPool.Count; i++)
            {
                int j = rng.Next(i, othersPool.Count);
                (othersPool[i], othersPool[j]) = (othersPool[j], othersPool[i]);
            }
            for (int k = 0; k < needed; k++)
                bundle.randomOtherLines.Add(othersPool[k]);
        }
        else
        {
            bundle.randomOtherLines.AddRange(othersPool);
            int remaining = needed - othersPool.Count;
            for (int i = 0; i < remaining; i++)
            {
                int idx = rng.Next(0, othersPool.Count);
                bundle.randomOtherLines.Add(othersPool[idx]);
            }
        }

        return bundle;
    }

    public List<CharacterPoemBundle> BuildAllBundles()
    {
        var result = new List<CharacterPoemBundle>();
        foreach (var ch in characters)
        {
            if (ch == null) continue;
            result.Add(BuildBundleFor(ch));
        }
        return result;
    }
}
