public enum TutorialType
{
    GameStart,
    MatchStart,
    SecondSetEnter,
    MasteryUIEnter
}

public class TutorialTriggerUseCase
{
    readonly ITutorialStorage _storage;
    readonly ITutorialViewer _viewer; // 출력 책임을 인터페이스로 위임

    public TutorialTriggerUseCase(ITutorialStorage storage, ITutorialViewer viewer)
    {
        _storage = storage;
        _viewer = viewer;
    }

    public void TriggerIfFirstTime(TutorialType type)
    {
        if (_storage.HasSeen(type)) return;

        _storage.MarkAsSeen(type);
        _viewer.Show(type);
    }
}