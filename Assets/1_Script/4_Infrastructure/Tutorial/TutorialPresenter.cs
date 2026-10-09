public class TutorialPresenter
{
    string[] _currentDialogues;
    int _dialogueIndex;

    public TutorialPresenter(string[] dialogues)
    {
        _currentDialogues = dialogues;
        _dialogueIndex = -1;
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