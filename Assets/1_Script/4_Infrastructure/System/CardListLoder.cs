using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CardListLoder
{
    const string BasePath = "SO/UseableCardList";

    public static CardListSO[] LoadStage(int stage)
    {
        var list = new List<CardListSO>();
        list.Add(LoadDefault());

        for (int i = 1; i <= stage; i++)
        {
            var cardList = LoadCardListSO(i);
            if (cardList != null)
                list.Add(cardList);
        }

        return list.ToArray();
    }

    static CardListSO LoadDefault() => Resources.Load<CardListSO>($"{BasePath}/Default");

    public static IEnumerable<ChampionSO> LoadAllStageCards(int stage) => LoadStage(stage).SelectMany(x => x.CardList);
    public static IEnumerable<int> LoadAllStageCardIds(int stage) => LoadAllStageCards(stage).Select(x => x.Id);

    public static CardListSO LoadCardListSO(int stage) => Resources.Load<CardListSO>($"{BasePath}/Stage{stage}");
}
