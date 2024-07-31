using UnityEngine;
using System.Linq;
using GamePush;

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
        //Debug.Log(data);
        PlayerPrefs.SetString(ID, data);
        GP_Player.Set("player_data", data);
        GP_Player.Sync(true);
    }


    public static void Load()
    {
        GP_Player.Load();

        string fromSave = "";
        fromSave = GP_Player.GetString("player_data");

        if (string.IsNullOrEmpty(fromSave))
        {
            Globals.MainPlayerData = new PlayerData();
        }
        else
        {
            try
            {
                Globals.MainPlayerData = JsonUtility.FromJson<PlayerData>(fromSave);
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning(ex);

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
    }

}
