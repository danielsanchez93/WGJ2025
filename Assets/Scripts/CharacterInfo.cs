using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Character/New Character")]
public class CharacterInfo : ScriptableObject
{
    [SerializeField] private List<string> poemLine = new List<string>();
    [SerializeField] private string characterName;
    [SerializeField] private AnimationClip animationClip;
    [SerializeField] private Sprite characterSprite;
    [SerializeField] private  List<AudioClip> audioclip = new List<AudioClip>();


    public string CharacterName => characterName;
    public IReadOnlyList<string> PoemLines => poemLine;
    public AnimationClip Clip => animationClip;
    public Sprite Portrait => characterSprite;
    public IReadOnlyList<AudioClip> AudioClips => audioclip;
}
