using System;

public interface ISlotView
{
    void StopCurrentAnimation();
    void SetState(SlotUIState state);
    void UpdateStatView(ChampionStatData stat);
}

public class NullSlotView : ISlotView
{
    public void StopCurrentAnimation() { }
    public void SetState(SlotUIState state) { }
    public void UpdateStatView(ChampionStatData stat) { }
}

public class CardDetailView
{
    readonly Action<int, ChampionStatData> _onUpdate;

    public int LastUpdatedChampionId { get; private set; }
    public ChampionStatData LastUpdatedStat { get; private set; }
    public int UpdateCallCount { get; private set; }

    public CardDetailView() : this((_, _) => { }) { }

    public CardDetailView(Action<int, ChampionStatData> onUpdate)
    {
        _onUpdate = onUpdate;
    }

    public void Update(int championId, ChampionStatData stat)
    {
        LastUpdatedChampionId = championId;
        LastUpdatedStat = stat;
        UpdateCallCount++;
        _onUpdate(championId, stat);
    }
}

public class SlotPresenter
{
    readonly SlotData _slot;
    readonly ChampionStatus _status;
    readonly int _championId;
    readonly CardDetailView _cardDetailView;
    readonly ISlotView _view;
    readonly Action<SlotData> _onTargetSelected;

    public SlotUIState CurrentState { get; private set; } = SlotUIState.Default;
    public SlotData Slot => _slot;
    public ChampionStatus Status => _status;
    public int ChampionId => _championId;

    public SlotPresenter(SlotData slot, ChampionStatus status, int championId, CardDetailView cardDetailView) : this(slot, status, championId, cardDetailView, new NullSlotView(), _ => { }) { }

    public SlotPresenter(SlotData slot, ChampionStatus status, int championId, CardDetailView cardDetailView, ISlotView view) : this(slot, status, championId, cardDetailView, view, _ => { }) { }

    public SlotPresenter(SlotData slot, ChampionStatus status, int championId, CardDetailView cardDetailView, ISlotView view, Action<SlotData> onTargetSelected)
    {
        _slot = slot;
        _status = status;
        _championId = championId;
        _cardDetailView = cardDetailView;
        _view = view;
        _onTargetSelected = onTargetSelected;
        _status.OnStatChanged += OnStatChanged;
        SetState(SlotStateResolver.ResolveObservationState(_status.IsDead));
    }

    public void SetState(SlotUIState newState)
    {
        _view.StopCurrentAnimation();
        CurrentState = newState;

        if (newState == SlotUIState.Dead)
        {
            _status.Die();
            _view.UpdateStatView(_status.Stat);
        }

        _view.SetState(newState);
    }

    public void OnImpactCompleted()
    {
        SetState(SlotStateResolver.ResolvePostImpactState(_status.IsDead));
    }

    public void OnClick()
    {
        if (CurrentState == SlotUIState.Default || CurrentState == SlotUIState.Dead)
        {
            _cardDetailView.Update(_championId, _status.Stat);
            return;
        }

        if (CurrentState == SlotUIState.Targetable)
        {
            _onTargetSelected(_slot);
        }
    }

    void OnStatChanged(ChampionStatData before, ChampionStatData after)
    {
        if (after.Defense <= 0)
            _view.UpdateStatView(after);
    }
}
