using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonConstructor : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI buttonText;
    private bool isAnswer;
    
    private Button button;
    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void SetButtonText(string text, bool isAnswer=false)
    {
        buttonText.text = text;
        isAnswer = isAnswer;
    }
}
