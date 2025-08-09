using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

public class UIConstructionManager : MonoBehaviour
{
    [Header("CSV Settings")]
    [Tooltip("Assign a .csv")]
    public TextAsset csvFile;

    [Tooltip("Ignore empty cells beyond the first column")]
    public bool ignoreEmptyCells = true;

    [Tooltip("Optional fixed seed for deterministic random picks")]
    public int randomSeed = 0;

    private readonly Dictionary<string, List<string>> textsByCharacter = new Dictionary<string, List<string>>();

    [Serializable]
    public class CharacterData
    {
        public string characterName;
        public List<string> ownTexts = new List<string>();
        public List<string> randomOtherAnswers = new List<string>();
    }

    [Header("Parse Preview (read-only)")]
    [SerializeField] private List<CharacterData> preview = new List<CharacterData>();

    private System.Random rng;

    void Awake()
    {
        rng = (randomSeed != 0) ? new System.Random(randomSeed) : new System.Random();

        if (csvFile == null)
        {
            Debug.LogError("UIConstructionManager: No CSV file assigned.");
            return;
        }

        ParseTSV(csvFile.text);
        BuildAllCharacterDataForPreview();
    }

    /// <summary>
    /// Parses TSV text into textsByCharacter.
    /// First column is the character name. Other columns are that character's texts.
    /// </summary>
    private void ParseTSV(string raw)
    {
        textsByCharacter.Clear();

        using (StringReader reader = new StringReader(raw))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] cells = line.Split('\t');
                if (cells.Length == 0) continue;

                string characterName = cells[0].Trim();
                if (string.IsNullOrEmpty(characterName)) continue;

                if (!textsByCharacter.TryGetValue(characterName, out var list))
                {
                    list = new List<string>();
                    textsByCharacter[characterName] = list;
                }

                for (int i = 1; i < cells.Length; i++)
                {
                    string text = cells[i]?.Trim() ?? string.Empty;
                    if (ignoreEmptyCells && string.IsNullOrEmpty(text)) continue;
                    list.Add(text);
                }
            }
        }
    }

    /// <summary>
    /// Returns own texts and the same number of random answers from other characters.
    /// </summary>
    public CharacterData BuildListsFor(string characterName)
    {
        var data = new CharacterData { characterName = characterName };

        if (!textsByCharacter.TryGetValue(characterName, out var own))
            return data;

        data.ownTexts = new List<string>(own);

        List<string> othersPool = new List<string>();
        foreach (var kv in textsByCharacter)
        {
            if (kv.Key == characterName) continue;
            othersPool.AddRange(kv.Value);
        }

        int needed = data.ownTexts.Count;

        if (othersPool.Count == 0 || needed == 0)
            return data;

        if (othersPool.Count >= needed)
        {
            for (int i = 0; i < needed; i++)
            {
                int j = rng.Next(i, othersPool.Count);
                (othersPool[i], othersPool[j]) = (othersPool[j], othersPool[i]);
                data.randomOtherAnswers.Add(othersPool[i]);
            }
        }
        else
        {
            data.randomOtherAnswers.AddRange(othersPool);
            int remaining = needed - othersPool.Count;
            for (int i = 0; i < remaining; i++)
            {
                int idx = rng.Next(0, othersPool.Count);
                data.randomOtherAnswers.Add(othersPool[idx]);
            }
        }

        return data;
    }

    /// <summary>
    /// Builds data for all characters and exposes it in the Inspector for quick sanity checks.
    /// </summary>
    private void BuildAllCharacterDataForPreview()
    {
        preview.Clear();
        foreach (var kv in textsByCharacter)
        {
            preview.Add(BuildListsFor(kv.Key));
        }
    }

    // Example hook you can call from elsewhere
    public IReadOnlyDictionary<string, List<string>> GetAllCharacterTexts() => textsByCharacter;
}
