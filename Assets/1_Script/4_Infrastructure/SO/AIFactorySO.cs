using System;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public struct AIConfig
{
    [HorizontalGroup("AI", LabelWidth = 30)]
    public int Id;

    [HorizontalGroup("AI")]
    public AI_SelectorFactory SelectorFactory;
}

[CreateAssetMenu(fileName = "AIFactorySO", menuName = "BP Master/AIFactorySO")]
public class AIFactorySO : ScriptableObject
{
    [TableList(AlwaysExpanded = true)]
    [SerializeField] AIConfig[] aiConfigs;

    public AI_SelectorFactory CreateAI(int id, Team team, BanPickStorage storage, ChampionCatalog championCatalog, MasteryRegistry masteryRegistry, BanPickHandler banPickHandler, PhaseAdvancer phaseAdvancer)
    {
        var config = aiConfigs.FirstOrDefault(x => x.Id == id);
        if (config.SelectorFactory == null) 
            throw new Exception($"해당 ID({id})에 매핑된 AI 팩토리를 찾을 수 없습니다.");

        var factory = Instantiate(config.SelectorFactory);

        factory.Init(team, championCatalog, masteryRegistry.GetTeamMasteryManager(team), banPickHandler.PickSlotFacade.StatusSlots);

        PredictValueSelectorFactory perdictFactory = factory as PredictValueSelectorFactory;
        if(perdictFactory != null)
            perdictFactory.Inject(storage, phaseAdvancer);
        return factory;
    }
}