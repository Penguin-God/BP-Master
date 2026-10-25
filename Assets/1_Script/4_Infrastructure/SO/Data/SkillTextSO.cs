using UnityEngine;


[CreateAssetMenu(fileName = "SkillTextSO", menuName = "Data/SkillTextSO")]
public class SkillTextSO : ScriptableObject
{
    [SerializeField] string increaseText;
    [SerializeField] string decreaseText;
    [SerializeField] string fixText;

    [SerializeField] string conditionKey;
    [SerializeField] string ruleKey;
    [SerializeField] string valueKey;
    [SerializeField] string actionKey;
    [SerializeField] string statKey;
    [SerializeField] string template;


    StatChangeTextData StatChangeText => new(increaseText, decreaseText, fixText);
    ActionTextConverter CreateSkillConverter() => new ActionTextConverter(template, StatChangeText, new SkillConvertKey("", "", valueKey, actionKey, statKey));
    SkillConvertKey SkillConvertKey => new("", "", statKey, valueKey, actionKey);

    public SkillTextBuilder CreateSkillTextBuilder() => new SkillTextBuilder(template, StatChangeText, SkillConvertKey);
}
