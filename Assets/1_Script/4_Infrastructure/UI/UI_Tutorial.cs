using UnityEngine;
using TMPro;

public class UI_Tutorial : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogueText;

    TutorialPresenter _presenter;

    public void StartTutorial(string[] dialogues)
    {
        if (dialogues == null || dialogues.Length == 0) return;

        _presenter = new TutorialPresenter(dialogues);
        gameObject.SetActive(true);
        ShowNextDialogue();
    }

    void Update()
    {
        // 튜토리얼이 진행 중일 때만 입력을 받습니다.
        if (_presenter == null) return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            ShowNextDialogue();
    }

    void ShowNextDialogue()
    {
        if (_presenter.Advance(out string text))
            dialogueText.text = text;
        else
            EndTutorial();
    }

    void EndTutorial()
    {
        _presenter = null;
        Destroy(gameObject);
    }
}