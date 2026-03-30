using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MasteryRegistryFactory", menuName = "SO/Match/MasteryRegistryFactory")]
public class MasteryRegistryFactorySO : ScriptableObject
{
    [SerializeField] int atkMultiplier;
    [SerializeField] int hpMultiplier;
    [SerializeField] int speedMultiplier;

    public MasteryRegistry CreateRegistry(Dictionary<Team, PlayerData> playerDatas)
    {
        var multiplier = new MasteryMultiplier(atkMultiplier, hpMultiplier, speedMultiplier);
        var factory = new MasteryStatCollectionFactory(multiplier);

        var blueMastery = factory.Create(playerDatas[Team.Blue].MasteryBoardCollection);
        var redMastery = factory.Create(playerDatas[Team.Red].MasteryBoardCollection);

        return new MasteryRegistry(blueMastery, redMastery);
    }

    public MasteryRegistry CreateRegistry(MasteryBoardCollection blueBoard, MasteryBoardCollection redBoard)
    {
        var multiplier = new MasteryMultiplier(atkMultiplier, hpMultiplier, speedMultiplier);
        var factory = new MasteryStatCollectionFactory(multiplier);

        var blueMastery = factory.Create(blueBoard);
        var redMastery = factory.Create(redBoard);

        return new MasteryRegistry(blueMastery, redMastery);
    }
}