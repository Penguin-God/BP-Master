

public enum BanPcikPhase { Ban, Pick, Swap, Done }
public class PhaseManager
{
    public BanPcikPhase CurrentPhase { get; private set; } = BanPcikPhase.Ban;
    public void ChangePhase(BanPcikPhase phase) => CurrentPhase = phase;
}
