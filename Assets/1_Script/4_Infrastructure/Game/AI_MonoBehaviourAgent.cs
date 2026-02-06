using System.Collections;
using UnityEngine;

public class AI_MonoBehaviourAgent : MonoBehaviour, IPhaseEntry
{
    AI_SkillExecutionUseCase skillUseCase;

    public void EnterBan()
    {
        throw new System.NotImplementedException();
    }

    public void EnterPick()
    {
        throw new System.NotImplementedException();
    }

    public void Init(AI_SkillExecutionUseCase aI_TraitAgent)
    {
        skillUseCase = aI_TraitAgent;
    }

    public void UseSkill(SlotData slot) => StartCoroutine(Co_UseTrait(slot));

    IEnumerator Co_UseTrait(SlotData slot)
    {
        yield return new WaitForSeconds(1.5f);
        skillUseCase.UseSkill(slot);
    }
}
