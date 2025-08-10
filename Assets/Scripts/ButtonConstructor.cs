using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonConstructor : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI buttonText;
    [SerializeField] private bool isAnswer;
    
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Setup(string text, bool answer)
    {
        Click();
        buttonText.text = text;
        isAnswer = answer;
    }

    public void Click()
    {
        button.onClick.AddListener((() =>
        {
            button.interactable = false;
            PoemArrangeManager.instance.AddLine(buttonText.text);
            CurrentChapterManager.instance.CurrentChapterGrade(isAnswer);
            if (isAnswer)
            {
                ///AQUI VA EL AUDIO POSITIVO OSEASE qUe EsSta CorrRectOhg
                Debug.Log("Good Answer");

            }
            else
            {
                ///AQUI VA EL AUDIO NEGATIVO
                Debug.Log("Bad Answer");
            }
        }));
    }
}
