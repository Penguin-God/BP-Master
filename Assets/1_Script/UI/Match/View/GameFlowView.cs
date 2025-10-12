using TMPro;
using UnityEngine;

public class GameFlowView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI gameFlowText;

    public void ViewGameFlow(GameFlowData flow) => gameFlowText.text = new GameFlowTextBuilder().BuildFlowText(flow);

    public void ViewTraitUseLog()
    {

    }
}
