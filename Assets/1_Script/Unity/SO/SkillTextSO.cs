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
    [SerializeField] SkillActionTextField[] skillActionTextFields;

    // public SkillTextConverter CreateSkillConverter() => new SkillTextConverter(skillActionTextFields.ToDictionary(x => x.Type, x => x.Text));
}
