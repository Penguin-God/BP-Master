using UnityEngine;
using TMPro;

public class UI_Tutorial : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogueText;

    TutorialPresenter _presenter;

    public void StartTutorial(TutorialPresenter presenter, int scheduleIndex)
    {
        _presenter = presenter;
        if (_presenter.TryStart(scheduleIndex) == false)
        {
            EndTutorial();
            return;
        }

        ShowNextDialogue();
    }

    void Update()
    {
        if (_presenter == null) return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            ShowNextDialogue();
    }

    void ShowNextDialogue()
    {
        if (_presenter.Advance(out string text)) dialogueText.text = text;
        else EndTutorial();
    }

    void EndTutorial()
    {
        _presenter = null;
        Destroy(gameObject);
    }
}