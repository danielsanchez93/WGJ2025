using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonConstructor : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI buttonText;
    [SerializeField] private bool isAnswer;
    
    private Button button;
    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void Setup(string text, bool answer)
    {
        buttonText.text = text;
        isAnswer = answer;
    }
}
