public interface IActionHandler
{
    void OnRequestAction(DraftActionController draftAction, GamePhase phase);
}
