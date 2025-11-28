using System.Collections;
using UnityEngine;

public class AI_MonoBehaviourAgent : MonoBehaviour
{
    AI_SkillAgent traitAgent;
    public void Init(AI_SkillAgent aI_TraitAgent)
    {
        traitAgent = aI_TraitAgent;
    }

    public void UseTrait(Team team) => StartCoroutine(Co_UseTrait(team));

    IEnumerator Co_UseTrait(Team team)
    {
        yield return new WaitForSeconds(1.5f);
        traitAgent.UseSkill(team);
    }
}
