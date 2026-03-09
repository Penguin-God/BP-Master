using System.Linq;
using UnityEngine;

[System.Serializable]
public class SkillActionTextField
{
    public SkillType Type;
    public string Text;
}

[CreateAssetMenu(fileName = "SkillTextSO", menuName = "BP Master/SkillTextSO")]
public class SkillTextSO : ScriptableObject
{
    [SerializeField] string increaseText;
    [SerializeField] string decreaseText;
    [SerializeField] string fixText;

    [SerializeField] string valueKey;
    [SerializeField] string actionKey;
    [SerializeField] string statKey;

    [SerializeField] SkillActionTextField[] skillActionTextFields;


    SkillAmountTextBuilder CreateAmountBuilder() => new SkillAmountTextBuilder(new AmountTextData(increaseText, decreaseText, fixText));
    SkillTextConverter CreateSkillConverter() => new SkillTextConverter(skillActionTextFields.ToDictionary(x => x.Type, x => x.Text), CreateAmountBuilder(), new SkillConvertKeyRecord(valueKey, actionKey, statKey));

    public SkillTextBuilder CreateSkillTextBuilder() => new SkillTextBuilder(CreateSkillConverter());
}
