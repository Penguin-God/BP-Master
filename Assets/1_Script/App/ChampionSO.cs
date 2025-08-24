using UnityEngine;

[CreateAssetMenu(fileName = "ChampionSO", menuName = "BP Master/ChampionSO")]
public class ChampionSO : ScriptableObject
{
    [SerializeField] int id;
    public int Id => id;

    [SerializeField] string championName;
    public string ChampionName => championName;

    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int range;
    [SerializeField] int speed;
    public ChampionStatData StatData => new ChampionStatData(attack, defense, range, speed);
}
