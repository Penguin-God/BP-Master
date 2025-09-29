using System.Collections.Generic;
using System.Linq;

public static class TestHelper
{
    public static SlotData CreateBlueSlot(int index) => new SlotData(Team.Blue, index);
    public static SlotData CreateRedSlot(int index) => new SlotData(Team.Red, index);

    public static Champion CreateTraitChamp(Side side, TargetRange range, int amount) 
        => new Champion(0, "", default,  
            new TraitData(TraitType.AttackChanger, amount, TraitConditionType.None, 0, new TraitTargetRule(side, range)));

    public static Champion CreateChamp(int id = 0, string name = "", ChampionStatData stat = default, TraitData trait = null) => new Champion(id, name, stat, trait);
    public static Champion CreateStatChamp(int att = 0, int def = 0, int speed = 0) => new Champion(0, "", new ChampionStatData(att, def, speed), null);

    public static IEnumerable<SlotData> CreateBlueSlots(params int[] indexs) => indexs.Select(x => CreateBlueSlot(x));
    public static IEnumerable<SlotData> CreateRedSlots(params int[] indexs) => indexs.Select(x => CreateRedSlot(x));

    public static ChampionStatus CreateStatus(int att = 0, int def = 0, int speed = 0) => new ChampionStatus(new ChampionStatData(att, def, speed));
    public static TraitData CreateTraitData(TraitType traitType, int amount, TraitConditionType traitConditionType = TraitConditionType.None, int threshold = 0) => new TraitData(traitType, amount, traitConditionType, threshold, default);

    public static TraitData CreateAttTraitData(int amount, TraitConditionType traitConditionType = 0, int threshold = 0, Side side = 0, TargetRange range = 0)
        => new TraitData(TraitType.AttackChanger, amount, traitConditionType, threshold, new TraitTargetRule(side, range));
}

public class TestAttackChangeAction : ITraitAction
{
    readonly int Amount;
    public TestAttackChangeAction(int amount) => Amount = amount;

    public void Do(ChampionStatus target) => target.ChangeStat(target.Stat.ChangeAttack(target.Stat.Attack + Amount));
}