using UnityEngine;

public class UtilKey : MonoBehaviour
{
    GameBanPickStorage storage;
    PhaseManager phaseManager;
    [SerializeField] DraftTurnSO ban;
    [SerializeField] DraftTurnSO pick;
    public void Init(GameBanPickStorage storage, PhaseManager phaseManager)
    {
        this.storage = storage;
        this.phaseManager = phaseManager;
    }

    int id = 0;
    void Update()
    {
        print("AAAAAAAAAAAAAAAAAAAAAAAAAAAA");

        if (Input.GetKeyDown(KeyCode.F) && id == 0)
        {
            foreach (var item in ban.Turns)
            {
                id++;
                storage.SaveSelect(new SelectInfo(item, SelectType.Ban, id));
                phaseManager.SubmitAction(item);
            }

            foreach (var item in pick.Turns)
            {
                id++;
                storage.SaveSelect(new SelectInfo(item, SelectType.Pick, id));
                phaseManager.SubmitAction(item);
            }
        }
    }
}
