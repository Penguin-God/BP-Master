using System.Linq;
using UnityEngine;

[System.Serializable]
public class SkillActionTextField
{
    public SkillType Type;
    public string Text;
}

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

    [SerializeField] SkillActionTextField[] skillActionTextFields;


    SkillAmountTextBuilder CreateAmountBuilder() => new SkillAmountTextBuilder(new AmountTextData(increaseText, decreaseText, fixText));
    ActionTextConverter CreateSkillConverter() => new ActionTextConverter(template, CreateAmountBuilder(), new SkillConvertKey("", "", valueKey, actionKey, statKey));

    public SkillTextBuilder CreateSkillTextBuilder() => new SkillTextBuilder(CreateSkillConverter());
}
