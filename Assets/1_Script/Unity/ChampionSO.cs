using UnityEngine;

enum TraitType
{
    None,
    AttackChanger,
}

[System.Serializable]
public class TraitData
{
    [SerializeField] TraitType traitType;
    [SerializeField] Side targetSide;
    [SerializeField] TargetRange range;
    [SerializeField] int amount;

    public Trait CreateTrait()
    {
        switch (traitType)
        {
            case TraitType.AttackChanger: return new Trait(targetSide, range, new AttackChanger(amount));
            default: return null;
        }
    }
}

[CreateAssetMenu(fileName = "ChampionSO", menuName = "BP Master/ChampionSO")]
public class ChampionSO : ScriptableObject
{
    [SerializeField] int id;
    public int Id => id;

    [SerializeField] string championName;
    public string ChampionName => championName;

    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int speed;
    public ChampionStatData StatData => new ChampionStatData(attack, defense, speed);

    [Header("특성")]
    [SerializeField] TraitData traitData;
}
