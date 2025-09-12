
using System.Collections.Generic;
using System.Linq;

public static class TestHelper
{
    public static ChampionSlot CreateBlueSlot(int index) => new ChampionSlot(Team.Blue, index);
    public static ChampionSlot CreateRedSlot(int index) => new ChampionSlot(Team.Red, index);

    public static Trait CreateTestTrait(Side side, TargetRange range, int amount) => new Trait(side, range, new TestAttackChanger(amount));
    public static Champion CreateTraitChamp(Side side, TargetRange range, int amount) => new Champion(0, "", default, CreateTestTrait(side, range, amount));

    public static IEnumerable<ChampionSlot> CreateBlueSlots(params int[] indexs) => indexs.Select(x => CreateBlueSlot(x));
    public static IEnumerable<ChampionSlot> CreateRedSlots(params int[] indexs) => indexs.Select(x => CreateRedSlot(x));
}

public class TestAttackChanger : ITraitAction
{
    readonly int Amount;
    public TestAttackChanger(int amount) => Amount = amount;

    public ChampionStatData Do(ChampionStatData stat) => stat.ChangeAttack(stat.Attack + Amount);
}