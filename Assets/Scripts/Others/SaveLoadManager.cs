using UnityEngine;
using System.Linq;

public class SaveLoadManager
{
    
    private const string ID = "Player29";

    public static void Save()
    {        
        Globals.MainPlayerData.L = Globals.CurrentLanguage;
        Globals.MainPlayerData.M = Globals.IsMobile ? 1 : 0;
        Globals.MainPlayerData.Mus = Globals.IsMusicOn ? 1 : 0;
        Globals.MainPlayerData.S = Globals.IsSoundOn ? 1 : 0;

        string data = JsonUtility.ToJson(Globals.MainPlayerData);
        Debug.Log(data);
        PlayerPrefs.SetString(ID, data);
        
    }


    public static void Load()
    {
        string fromSave = "";
        fromSave = PlayerPrefs.GetString(ID);

        if (string.IsNullOrEmpty(fromSave))
        {
            Globals.MainPlayerData = new PlayerData();
        }
        else
        {
            Globals.MainPlayerData = JsonUtility.FromJson<PlayerData>(fromSave);
        }


    }

}
