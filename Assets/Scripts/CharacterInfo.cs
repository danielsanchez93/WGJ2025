using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Character/New Character")]
public class CharacterInfo : ScriptableObject
{
    [SerializeField] private List<string> poemLine = new List<string>();
    [SerializeField] private string characterName;
    [SerializeField] private AnimationClip animationClip;

    public string CharacterName => characterName;
    public IReadOnlyList<string> PoemLines => poemLine;
    public AnimationClip Clip => animationClip;
}
