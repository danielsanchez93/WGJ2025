using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
public class CurrentChapterManager : MonoBehaviour
{
    
    public List<bool> chaptersApproved = new List<bool>();
    public List<bool> currentChaptersApproved = new List<bool>();
    public int currentPoems;
    
    public static CurrentChapterManager instance;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void CurrentChapterGrade(bool grade)
    {
        currentChaptersApproved.Add(grade);
        currentPoems++;
        if (currentPoems >= 4)
        {
            //Aumentar Capitulo
            List<bool> boolList = new List<bool> { false, true, false };
            bool anyFalse = boolList.Any(b => b == false);
            if (anyFalse)
            {
                chaptersApproved.Add(false);
            }
            ClearCurrentChapterGrade();
            GameManager.Instance.NextCharacter();
            //AQUI PUEDE IR UNA TRANSICION Y COSAS ASI????? PLZ
        }
    }

    public void ClearAll()
    {
        chaptersApproved.Clear();
        currentChaptersApproved.Clear();
    }
    public void ClearCurrentChapterGrade()
    {
        currentChaptersApproved.Clear();
        currentPoems = 0;
    }
    
}
