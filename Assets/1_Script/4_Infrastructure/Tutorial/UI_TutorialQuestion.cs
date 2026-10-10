using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_TutorialQuestion : MonoBehaviour
{
    [SerializeField] Button selectBtn;
    [SerializeField] ChampionSelector_UI championSelector;

    void Start()
    {
        selectBtn.onClick.AddListener(Click);
    }

    public void Do(int[] answers, Action )
    {
        _answers = answers;
        gameObject.SetActive(true);
        championSelector.ActiveSelectButton(false);
    }

    int[] _answers;

    public void Click()
    {
        if (_answers.Contains(championSelector.SelectId))
        {
            championSelector.NailDownChampion();

        }
    }
}


public class DialogueTuto
{
    public void Do(string[] dialgoue)
    {

    }
}