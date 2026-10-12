using UnityEngine;

public class StageSaveHelper : MonoBehaviour
{
    [SerializeField] int clearStage;

    [ContextMenu("스테이지 저장")]
    public void Save() => new PlayerPrefsStageStorage().SaveUnlockedStage(clearStage);
}
