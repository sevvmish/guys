using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField] private int currentIndex;
    [SerializeField] private GameObject vfx;
    [SerializeField] private GameObject sound;

    private HashSet<GameObject> players = new HashSet<GameObject>();

    private void Awake()
    {
        if (vfx != null)
        {
            vfx.SetActive(true);
            sound.SetActive(false);
        }
        
        if (RespawnManager.Instance != null) RespawnManager.Instance.AddPoint(currentIndex, this);
    }

    private void Start()
    {
        if (RespawnManager.Instance != null && (RespawnManager.Instance.GetCurrentIndex) > currentIndex)
        {            
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out BotAI bot) && bot.CurrentIndex > currentIndex)
        {
            bot.ResetIndexToValue(currentIndex);
        }

        

        if (other.CompareTag("Player") && !players.Contains(other.gameObject) && other.TryGetComponent(out RespawnControl resp) /*&& resp.CurrentRespawnIndex < currentIndex*/)
        {
            if (resp.TryGetComponent(out PlayerControl pc) && pc.IsItMainPlayer)
            {
                switch (GameManager.Instance.GetLevelManager().GetCurrentLevelType())
                {                    
                    case LevelTypes.circus1:
                        Globals.MainPlayerData.M1 = currentIndex;
                        break;
                }

                SaveLoadManager.Save();
            }

            AddPlayerToPoint(resp);

            /*
            players.Add(other.gameObject);
            resp.SetNewRespawn(new RespawnControl.RespawnData(new Vector3(
                other.transform.position.x, 
                transform.position.y, 
                other.transform.position.z) , transform.eulerAngles), currentIndex);*/
        }
    }

    public void AddPlayerToPoint(RespawnControl resp)
    {
        if (vfx != null && resp.TryGetComponent(out PlayerControl pc) && pc.IsItMainPlayer)
        {
            StartCoroutine(playSound());
        }

        GameObject other = resp.gameObject;

        players.Add(other);
        resp.SetNewRespawn(new RespawnControl.RespawnData(new Vector3(
            other.transform.position.x,
            transform.position.y,
            other.transform.position.z), transform.eulerAngles), currentIndex);
    }

    private IEnumerator playSound()
    {
        sound.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        vfx.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(1.5f);
        vfx.SetActive(false);
        sound.SetActive(false);
    }
}
