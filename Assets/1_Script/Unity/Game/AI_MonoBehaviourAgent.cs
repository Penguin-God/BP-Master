using System.Collections;
using UnityEngine;

public class AI_MonoBehaviourAgent : MonoBehaviour
{
    AI_TraitAgent traitAgent;
    public void Init(AI_TraitAgent aI_TraitAgent)
    {
        traitAgent = aI_TraitAgent;
    }

    public void UseTrait(Team team) => StartCoroutine(Co_UseTrait(team));

    IEnumerator Co_UseTrait(Team team)
    {
        yield return new WaitForSeconds(1.5f);
        traitAgent.UseTrait(team);
    }
}
