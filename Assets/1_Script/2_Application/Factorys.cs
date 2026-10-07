using UnityEngine;

public static class Factorys
{
    public static TargetCountCalculator CreateTeamCounter<T>(SlotStorage<T> slotStorage) => new TargetCountCalculator(slotStorage.GetTeamCount(Team.Blue), slotStorage.GetTeamCount(Team.Red));
}
