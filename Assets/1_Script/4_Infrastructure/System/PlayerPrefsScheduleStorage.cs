using UnityEngine;

public class PlayerPrefsScheduleStorage : IScheduleStorage
{
    const string ScheduleIndexKey = "League_CurrentIndex";

    public void SaveIndex(int index)
    {
        PlayerPrefs.SetInt(ScheduleIndexKey, index);
        PlayerPrefs.Save();
    }

    public int LoadIndex() => PlayerPrefs.GetInt(ScheduleIndexKey, defaultValue: 0);
}