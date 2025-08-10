using System;
using TMPro;
using UnityEngine;

public class PoemArrangeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI poemText;

    public static PoemArrangeManager instance;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(instance);
    }

    public void AddLine(string line)
    {
        if (poemText == null) return;
        if (string.IsNullOrEmpty(poemText.text))
        {
            poemText.text = line;
        }
        else
        {
            poemText.text += "\n" + line;
        }
    }

    public void ClearPoem()
    {
        if (poemText == null) return;
        poemText.text = string.Empty;
    }
}