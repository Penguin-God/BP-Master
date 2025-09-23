using UnityEngine;

public class UtilKey : MonoBehaviour
{
    GameBanPickStorage storage;
    [SerializeField] DraftTurnSO ban;
    [SerializeField] DraftTurnSO pick;
    public void Init(GameBanPickStorage storage)
    {
        this.storage = storage;
    }

    int id = 0;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && id == 0)
        {
            foreach (var item in ban.Turns)
            {
                id++;
                storage.SaveSelect(new SelectInfo(item, SelectType.Ban, id));
            }

            foreach (var item in pick.Turns)
            {
                id++;
                storage.SaveSelect(new SelectInfo(item, SelectType.Pick, id));
            }
        }
    }
}
