using Match;
using UnityEngine;

public class AAAA : IChampionProvider // 데이터 매니저 만들기?
{
    public ChampionProfile GetProfile(int id)
    {
        var so = ChampionDataLoder.GetChampionData(id);
        return new ChampionProfile(id, so.name, so.StatData, so.Skill);
    }
}

public class LocalPlayerDataLoader : IPlayerDataLoader
{
    readonly string playerName;
    readonly JsonMasterySaver saver;

    public LocalPlayerDataLoader(string playerName, JsonMasterySaver saver)
    {
        this.playerName = playerName;
        this.saver = saver;
    }

    public PlayerData LoadPlayer(int id)
    {
        var inventory = saver.Load();
        if(inventory == null)
            inventory = new MasteryProfile(0);
        return new PlayerData(id, playerName, inventory.BoardCollection);
    }
}

public class LobbyScene : MonoBehaviour
{
    [SerializeField] UI_StageSelection uI_StageSelection;
    [SerializeField] MatchConfigSO matchConfigSO;
    [SerializeField] UI_MasteryPoint uI_MasteryPoint;
    [SerializeField] SkillTextSO skillTextSO;
    [SerializeField] TutorialTrigger tutorialTrigger;
    [SerializeField] PlayerDataProviderFactorySO playerDataProviderFactory;
    
    void Awake()
    {   
        //MatchContext.OnSeriesFinished -= SaveGameProgress;
        //MatchContext.OnSeriesFinished += SaveGameProgress;

        var dataIO = new JsonMasterySaver();
        var inventory = dataIO.Load();
        if (inventory == null)
            inventory = new MasteryProfile(startPoints: 15);

        uI_StageSelection.Init(new StageProgressPresenter(new PlayerPrefsStageStorage()), OnStageSelect);
        uI_MasteryPoint.Init(new MasteryPointPresenter(inventory, new ChampionTextBuilder(new AAAA(), skillTextSO.CreateSkillTextBuilder(), new ChampionStatusTextBuilder()), uI_MasteryPoint, dataIO), inventory);

        tutorialTrigger.PlayTutorial();

    }

    void OnStageSelect(int stage)
    {
        print(stage);
    }

    // void SaveGameProgress(MatchData matchData, MatchWinCounter winCounter) => new PlayerPrefsStageStorage().SaveUnlockedStage();
}