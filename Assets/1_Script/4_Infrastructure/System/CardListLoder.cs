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
            var cardList = Resources.Load<CardListSO>($"{BasePath}/Stage{i}");
            if (cardList != null)
                list.Add(cardList);
        }

        return list.ToArray();
    }

    public static CardListSO LoadDefault() => Resources.Load<CardListSO>($"{BasePath}/Default");

    public static IEnumerable<ChampionSO> LoadStageCards(int stage) => LoadStage(stage).SelectMany(x => x.CardList);
}
