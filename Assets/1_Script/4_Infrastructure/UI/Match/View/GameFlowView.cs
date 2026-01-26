using System.Collections;
using TMPro;
using UnityEngine;

public class GameFlowView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI gameFlowText;
    [SerializeField] ChampionRepository championRepository;
    public void ViewGameFlow(GameFlowData flow)
    {
        if(flow.Phase == GamePhase.Done)
        {
            gameFlowText.text = string.Empty;
            return;
        }
        gameFlowText.text = new GameFlowTextBuilder().BuildFlowText(flow);
    }

    SlotStorage<int> ids;
    public void Init(SlotStorage<int> ids)
    {
        this.ids = ids;
    }

    public void UpdateUseSkill(SlotData useSlot)
    {
        string skillChamp = championRepository.GetChampionName(ids.GetSlot(useSlot));
        StartCoroutine(Co_ViewTriatUseLog(skillChamp));
    }

    IEnumerator Co_ViewTriatUseLog(string name)
    {
        string temp = gameFlowText.text;
        gameFlowText.text = $"{name} 스킬 사용!";
        yield return new WaitForSeconds(1f);
        gameFlowText.text = temp;
    }
}
