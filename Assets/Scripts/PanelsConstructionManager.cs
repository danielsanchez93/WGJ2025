using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class PanelsConstructionManager : MonoBehaviour
{
    public CharacterHandler source;
    public Transform container;
    public ButtonConstructor buttonPrefab;
    public Image targetImage;
    public Animator targetAnimator;
    public bool shuffle = true;
    public int randomSeed = 0;

    private System.Random rng;
    private PlayableGraph graph;

    void Awake()
    {
        rng = (randomSeed != 0) ? new System.Random(randomSeed) : new System.Random();
    }

    void OnDisable()
    {
        if (graph.IsValid()) graph.Destroy();
    }

    public void Build(CharacterInfo character, Sprite sprite, AnimationClip clip)
    {
        if (source == null || character == null || buttonPrefab == null || container == null) return;
        Clear();
        if (targetImage != null) targetImage.sprite = sprite;

        var bundle = source.BuildBundleFor(character);
        var entries = new List<(string, bool)>();
        foreach (var s in bundle.ownLines) entries.Add((s, true));
        foreach (var s in bundle.randomOtherLines) entries.Add((s, false));
        if (shuffle) Shuffle(entries);
        foreach (var e in entries)
        {
            var btn = Instantiate(buttonPrefab, container);
            //btn.Setup(e.Item1, e.Item2);
        }

        PlayClip(clip);
    }

    public void Clear()
    {
        for (int i = container.childCount - 1; i >= 0; i--)
            Destroy(container.GetChild(i).gameObject);
        if (graph.IsValid()) { graph.Destroy(); }
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = rng.Next(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    private void PlayClip(AnimationClip clip)
    {
        if (targetAnimator == null || clip == null) return;
        if (graph.IsValid()) graph.Destroy();
        graph = PlayableGraph.Create("PanelsConstructionClip");
        var output = AnimationPlayableOutput.Create(graph, "Output", targetAnimator);
        var playable = AnimationClipPlayable.Create(graph, clip);
        output.SetSourcePlayable(playable);
        graph.Play();
    }
}
