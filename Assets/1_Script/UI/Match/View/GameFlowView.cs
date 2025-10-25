using System.Collections;
using TMPro;
using UnityEngine;

public class GameFlowView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI gameFlowText;
    [SerializeField] TextMeshProUGUI traitUseText;

    public void ViewGameFlow(GameFlowData flow) => gameFlowText.text = new GameFlowTextBuilder().BuildFlowText(flow);

    SlotStorage<ChampionSO> champions;
    public void Init(SlotStorage<ChampionSO> champions)
    {
        this.champions = champions;
    }

    public void ViewTraitUseLog(SlotData useSlot) => StartCoroutine(Co_ViewTriatUseLog(champions.GetSlot(useSlot).ChampionName));

    IEnumerator Co_ViewTriatUseLog(string name)
    {
        traitUseText.text = $"{name} 특성 사용!";
        yield return new WaitForSeconds(1f);
        traitUseText.text = string.Empty;
    }
}
