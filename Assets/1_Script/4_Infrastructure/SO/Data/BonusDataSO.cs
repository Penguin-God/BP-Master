using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct BonusData
{
    public int Threshold;
    public int Bonus;
}

[CreateAssetMenu(fileName = "BonusDataSO", menuName = "Data/Bonus")]
public class BonusDataSO : ScriptableObject
{
    [SerializeField] BonusData[] bonusDatas;
    public SortedDictionary<int, int> BonusDatas => new SortedDictionary<int, int>(bonusDatas.ToDictionary(x => x.Threshold, x => x.Bonus));
    public BonusCalculator Bonus => new BonusCalculator(BonusDatas);
}
