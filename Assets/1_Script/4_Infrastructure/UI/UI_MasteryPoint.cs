using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class UI_MasteryPoint : MonoBehaviour
{
    [AssetsOnly] [SerializeField] GameObject btnPerfab;
    [SerializeField] Transform parent;
    [SerializeField] TextMeshProUGUI _pointText;

    void Start()
    {
        foreach (Transform child in parent) Destroy(child.gameObject);

        new ChampionButtonCreator().DrawChampionButtons(parent, ChampionDataLoder.AllChampions, btnPerfab);
    }
}
