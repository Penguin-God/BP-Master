using UnityEngine;

[CreateAssetMenu(fileName = "MasteryRegistryFactory", menuName = "Data/MasteryRegistry")]
public class MasteryRegistryFactorySO : ScriptableObject
{
    [SerializeField] int atkMultiplier;
    [SerializeField] int hpMultiplier;
    [SerializeField] int speedMultiplier;

    public MasteryRegistry CreateRegistry(MasteryBoardCollection blueBoard, MasteryBoardCollection redBoard)
    {
        var multiplier = new MasteryMultiplier(atkMultiplier, hpMultiplier, speedMultiplier);
        var factory = new MasteryStatCollectionFactory(multiplier);

        var blueMastery = factory.Create(blueBoard);
        var redMastery = factory.Create(redBoard);

        return new MasteryRegistry(blueMastery, redMastery);
    }
}