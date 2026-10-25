using UnityEngine;


[CreateAssetMenu(fileName = "SkillTextSO", menuName = "Data/SkillTextSO")]
public class SkillTextSO : ScriptableObject
{
    [SerializeField] string increaseText;
    [SerializeField] string decreaseText;
    [SerializeField] string fixText;

    [SerializeField] string valueKey;
    [SerializeField] string actionKey;
    [SerializeField] string statKey;
    [SerializeField] string template;


    StatChangeTextData CreateAmountBuilder() => new(increaseText, decreaseText, fixText);
    ActionTextConverter CreateSkillConverter() => new ActionTextConverter(template, CreateAmountBuilder(), new SkillConvertKey("", "", valueKey, actionKey, statKey));

    public SkillTextBuilder CreateSkillTextBuilder() => new SkillTextBuilder(CreateSkillConverter());
}
