using UnityEngine;
using UnityEngine.UI;

public class SwapController : MonoBehaviour
{
    [SerializeField] Button swapDoneBtn;
    PhaseManager phaseManager;
    public void Init(Team team)
    {
        swapDoneBtn.gameObject.SetActive(true);
        swapDoneBtn.onClick.AddListener(() => SwapDone(team));
    }

    public void Inject(PhaseManager phaseManager)
    {
        this.phaseManager = phaseManager;
    }

    void SwapDone(Team team)
    {
        if (phaseManager.CurrentFlow.Phase == GamePhase.Swap)
        {
            phaseManager.SubmitAction(team);
            swapDoneBtn.gameObject.SetActive(false);
        }
    }
}
