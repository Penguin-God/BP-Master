using UnityEngine;

[CreateAssetMenu(fileName = "SkillTextSO", menuName = "Data/SkillTextSO")]
public class SkillTextSO : ScriptableObject
{
    [Header("수치 변화 텍스트")]
    [SerializeField] string increaseText;
    [SerializeField] string decreaseText;
    [SerializeField] string fixText;

    [Header("스탯 텍스트")]
    [SerializeField] string attackText;
    [SerializeField] string defenseText;
    [SerializeField] string speedText;

    [Header("진영(Side) 텍스트")]
    [SerializeField] string selfSideText;
    [SerializeField] string opponentSideText;
    [SerializeField] string allSideText;
    [SerializeField] string noneSideText;

    [Header("타깃 룰(TargetRule) 텍스트")]
    [SerializeField] string allCardsText;
    [SerializeField] string selectCardsPrefix;
    [SerializeField] string selectCardsSuffix;

    [Header("템플릿 및 치환 키")]
    [SerializeField] string conditionKey;
    [SerializeField] string ruleKey;
    [SerializeField] string statKey;
    [SerializeField] string valueKey;
    [SerializeField] string actionKey;
    [SerializeField] string template;

    StatChangeTextData StatChangeText => new(increaseText, decreaseText, fixText);
    StatTextData StatText => new(attackText, defenseText, speedText);
    SideTextData SideText => new(selfSideText, opponentSideText, allSideText, noneSideText, allCardsText, selectCardsPrefix, selectCardsSuffix);
    SkillConvertKey SkillConvertKey => new(conditionKey, ruleKey, statKey, valueKey, actionKey);

    public SkillTextBuilder CreateSkillTextBuilder()
        => new SkillTextBuilder(template, StatChangeText, StatText, SideText, SkillConvertKey);
}