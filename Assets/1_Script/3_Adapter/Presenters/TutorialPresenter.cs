using System.Collections.Generic;

public class TutorialPresenter
{
    readonly Dictionary<int, string[]> _tutorialData;

    string[] _currentDialogues;
    int _dialogueIndex;

    public TutorialPresenter(Dictionary<int, string[]> tutorialData)
    {
        _tutorialData = tutorialData;
    }

    public bool TryStart(int scheduleIndex)
    {
        if (_tutorialData.TryGetValue(scheduleIndex, out var dialogues))
        {
            _currentDialogues = dialogues;
            _dialogueIndex = -1;
            return true;
        }

        _currentDialogues = null;
        return false;
    }

    public bool Advance(out string text)
    {
        text = string.Empty;
        if (_currentDialogues == null) return false;

        _dialogueIndex++;

        if (_dialogueIndex < _currentDialogues.Length)
        {
            text = _currentDialogues[_dialogueIndex];
            return true;
        }

        _currentDialogues = null;
        return false;
    }
}