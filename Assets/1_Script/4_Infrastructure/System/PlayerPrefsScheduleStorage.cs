using UnityEngine;

public class PlayerPrefsScheduleStorage : IScheduleStorage
{
    readonly string _key;
    public PlayerPrefsScheduleStorage(string key) => _key = key;

    public void SaveIndex(int index)
    {
        PlayerPrefs.SetInt(_key, index);
        PlayerPrefs.Save();
    }

    public int LoadIndex() => PlayerPrefs.GetInt(_key, defaultValue: 0);
}