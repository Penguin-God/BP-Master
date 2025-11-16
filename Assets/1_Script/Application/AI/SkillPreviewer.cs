
public class SkillPreviewer
{
    public SlotStorage<ChampionStatus> PreviewSkill(SlotStorage<ChampionStatus> originSlots, Skill skill)
    {
        var copiedSlots = CloneSlots(originSlots);

        // 복사된 슬롯으로만 동작하는 컨트롤러 생성
        var previewController = new SkillUseController(copiedSlots);

        previewController.UseSkill(default, new SlotData[] { new SlotData(Team.Blue, 0) }, skill.SkillDatas);
        return copiedSlots;
    }

    SlotStorage<ChampionStatus> CloneSlots(SlotStorage<ChampionStatus> origin)
    {
        var result = new SlotStorage<ChampionStatus>();
        foreach (var slot in origin.GetAllSlotDatas())
            result.AddSlot(slot.Team, origin.GetSlot(slot).DeepCopy());
        return result;
    }
}
