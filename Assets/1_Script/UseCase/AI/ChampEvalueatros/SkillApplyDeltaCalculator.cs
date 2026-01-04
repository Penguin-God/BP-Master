using System.Linq;

public record TeamStatChangeInfo(int Att, int Def, int Speed);
public record GameStatChangeInfo(TeamStatChangeInfo Blue, TeamStatChangeInfo Red);


public class SkillApplyDeltaCalculator
{
    readonly SkillPreviewer previewer;
    readonly SlotStorage<ChampionStatus> originalSlots;
    public SkillApplyDeltaCalculator(SkillPreviewer previewer, SlotStorage<ChampionStatus> originalSlots)
    {
        this.previewer = previewer;
        this.originalSlots = originalSlots;
    }

    public SkillApplyDeltaCalculator()
    {
    }

    public GameStatChangeInfo CalculateApplySkillStat(Champion champion)
    {
        SlotStorage<ChampionStatus> skillApplySlots = previewer.PreviewSkill(champion);

        TeamStatChangeInfo blueDelta = CalculateTeamDelta(skillApplySlots, Team.Blue);
        TeamStatChangeInfo redDelta = CalculateTeamDelta(skillApplySlots, Team.Red);

        return new GameStatChangeInfo(blueDelta, redDelta);
    }

    public GameStatChangeInfo CalculateStatDelta(SlotStorage<ChampionStatus> origin, SlotStorage<ChampionStatus> after)
    {
        TeamStatChangeInfo blueDelta = CalculateTeamDelta(origin, after, Team.Blue);
        TeamStatChangeInfo redDelta = CalculateTeamDelta(origin, after, Team.Red);

        return new GameStatChangeInfo(blueDelta, redDelta);
    }

    TeamStatChangeInfo CalculateTeamDelta(SlotStorage<ChampionStatus> after, Team team)
    {
        int totalAtt = 0;
        int totalDef = 0;
        int totalSpd = 0;

        var teamSlots = originalSlots.GetAllSlotDatas().Where(s => s.Team == team);

        foreach (var slotData in teamSlots)
        {
            ChampionStatus beforeStatus = originalSlots.GetSlot(slotData);
            ChampionStatus afterStatus = after.GetSlot(slotData);

            totalAtt += afterStatus.Stat.Attack - beforeStatus.Stat.Attack;
            totalDef += afterStatus.Stat.Defense - beforeStatus.Stat.Defense;
            totalSpd += afterStatus.Stat.Speed - beforeStatus.Stat.Speed;
        }

        return new TeamStatChangeInfo(totalAtt, totalDef, totalSpd);
    }

    TeamStatChangeInfo CalculateTeamDelta(SlotStorage<ChampionStatus> origin, SlotStorage<ChampionStatus> after, Team team)
    {
        int totalAtt = 0;
        int totalDef = 0;
        int totalSpd = 0;

        var teamSlots = origin.GetAllSlotDatas().Where(s => s.Team == team);

        foreach (var slotData in teamSlots)
        {
            ChampionStatus beforeStatus = origin.GetSlot(slotData);
            ChampionStatus afterStatus = after.GetSlot(slotData);

            totalAtt += afterStatus.Stat.Attack - beforeStatus.Stat.Attack;
            totalDef += afterStatus.Stat.Defense - beforeStatus.Stat.Defense;
            totalSpd += afterStatus.Stat.Speed - beforeStatus.Stat.Speed;
        }

        return new TeamStatChangeInfo(totalAtt, totalDef, totalSpd);
    }
}