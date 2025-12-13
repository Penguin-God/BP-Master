using System.Collections;
using UnityEngine;

public class AI_MonoBehaviourAgent : MonoBehaviour, IPhaseEntry
{
    AI_SkillUseAgent traitAgent;

    public void EnterBan()
    {
        throw new System.NotImplementedException();
    }

    public void EnterPick()
    {
        throw new System.NotImplementedException();
    }

    public void Init(AI_SkillUseAgent aI_TraitAgent)
    {
        traitAgent = aI_TraitAgent;
    }

    public void UseSkill(SlotData slot) => StartCoroutine(Co_UseTrait(slot));

    IEnumerator Co_UseTrait(SlotData slot)
    {
        yield return new WaitForSeconds(1.5f);
        traitAgent.UseSkill(slot);
    }
}
