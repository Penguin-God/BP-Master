using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct BonusData
{
    public int Threshold;
    public int Bonus;
}

[CreateAssetMenu(fileName = "BonusDataSO", menuName = "BP Master/BonusDataSO")]
public class BonusDataSO : ScriptableObject
{
    [SerializeField] BonusData[] bonusDatas;
    public BonusCalculator Bonus => new BonusCalculator(new SortedDictionary<int, int>(bonusDatas.ToDictionary(x => x.Threshold, x => x.Bonus)));
}
