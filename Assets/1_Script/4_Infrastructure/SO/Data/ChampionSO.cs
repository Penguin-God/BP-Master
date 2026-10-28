using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ChampionSO", menuName = "Data/Champion")]
public class ChampionSO : SerializedScriptableObject
{
    [SerializeField] int id;
    public int Id => id;

    [SerializeField] string championName;
    public string ChampionName => championName;

    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int speed;
    public ChampionStatData StatData => new ChampionStatData(attack, defense, speed);

    [Header("스킬")]
    [SerializeField] TraitDataConfig[] skillDatas;

    public Skill Skill => new Skill(skillDatas.Select(x => x.CreateSkillData()));
    public IEnumerable<SkillViewModel> SkillViewModels => skillDatas.Select(x => x.CreateViewModel());
    public ChampionStatus CreateStatus() => new ChampionStatus(StatData);
    public Champion CreateChampion() => new Champion(Id, Skill, CreateStatus());
    public ChampionProfile CreateProfile() => new ChampionProfile(Id, championName, StatData, SkillViewModels);

    public Skill CreateMergedSkill() => new Skill(skillDatas.SelectMany(x => x.CreateSkillDatas()));
    public IEnumerable<SkillViewModel> CreateMergedSkillViewModels() => skillDatas.SelectMany(x => x.CreateViewModels());

    [SerializeField][PreviewField(70)] Sprite icon;
    public Sprite Icon => icon;
}