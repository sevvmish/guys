using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetRewardSystem : MonoBehaviour
{
    public static GetRewardSystem Instance { get; private set; }

    [SerializeField] private MainMenu menu;
    [SerializeField] private RectTransform goldRect;
    [SerializeField] private RectTransform gemRect;
    [SerializeField] private RectTransform xpRect;
    [SerializeField] private RectTransform newLvlRect;
    [SerializeField] private RectTransform newMapRect;
    [SerializeField] private RectTransform allMapRect;
    [SerializeField] private RectTransform allSkinsRect;
    [SerializeField] private RectTransform noADVRect;

    private List<ShowRewardEffect> effects = new List<ShowRewardEffect>();
    private bool isActive;
    private bool isReady;
    private bool isStop;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        Globals.IsLevelChangeStarted = false;
    }

    public void ShowEffect(RewardTypes _type, int amount)
    {
        if (isStop) return;
        effects.Add(new ShowRewardEffect(_type, amount));
    }

    private void Update()
    {
        if (Globals.IsInitiated && !isReady)
        {
            isReady = true;
            goldRect.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Globals.Language.Gold;
            gemRect.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Globals.Language.Gem;
            xpRect.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Globals.Language.XP;
            newLvlRect.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Globals.Language.NewLVL;
            newMapRect.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = Globals.Language.NewMap;
            allMapRect.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Globals.Language.AllMapsRewardSign;
            allSkinsRect.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Globals.Language.AllSkinsRewardSign;
            noADVRect.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Globals.Language.NoADVRewardSign;
        }

        if (effects.Count > 0 && !isActive)
        {
            StartCoroutine(playEffects());
        }

        if (Globals.IsLevelChangeStarted && !isStop)
        {
            isStop = true;
            effects.Clear();
            StopAllCoroutines();
        }
    }

    private IEnumerator playEffects()
    {
        isActive = true;
        float addWait = 0;

        for (int i = 0; i < effects.Count; i++)
        {
            GameObject g = default;
            Vector3 pos = Vector3.zero;

            switch(effects[i].RewardType)
            {
                case RewardTypes.gold:
                    SoundUI.Instance.PlayUISound(SoundsUI.cash);
                    g = Instantiate(goldRect.gameObject, transform);
                    pos = new Vector3(500, 500,0);
                    break;

                case RewardTypes.gem:
                    SoundUI.Instance.PlayUISound(SoundsUI.cash);
                    g = Instantiate(gemRect.gameObject, transform);
                    pos = new Vector3(700, 500,0);
                    break;

                case RewardTypes.xp:
                    SoundUI.Instance.PlayUISound(SoundsUI.success3);
                    g = Instantiate(xpRect.gameObject, transform);
                    pos = new Vector3(-700, -500,0);
                    break;

                case RewardTypes.newLvl:
                    g = Instantiate(newLvlRect.gameObject, transform);
                    SoundUI.Instance.PlayUISound(SoundsUI.success2);
                    pos = new Vector3(-700, -500, 0);                    
                    break;

                case RewardTypes.newMap:
                    g = Instantiate(newMapRect.gameObject, transform);
                    SoundUI.Instance.PlayUISound(SoundsUI.success2);
                    pos = new Vector3(-700, -500, 0);
                    break;

                case RewardTypes.all_maps:
                    g = Instantiate(allMapRect.gameObject, transform);
                    SoundUI.Instance.PlayUISound(SoundsUI.success2);
                    pos = new Vector3(-700, -500, 0);
                    break;

                case RewardTypes.all_skins:
                    g = Instantiate(allSkinsRect.gameObject, transform);
                    SoundUI.Instance.PlayUISound(SoundsUI.success2);
                    pos = new Vector3(-700, 200, 0);
                    break;

                case RewardTypes.no_adv:
                    g = Instantiate(noADVRect.gameObject, transform);
                    SoundUI.Instance.PlayUISound(SoundsUI.success2);
                    pos = new Vector3(300, 500, 0);
                    break;
            }

            if (effects[i].RewardType == RewardTypes.newMap)
            {
                g.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = LevelManager.GetLevelData((LevelTypes)effects[i].Amount).LevelName;
                g.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = LevelManager.GetLevelData((LevelTypes)effects[i].Amount).ImageSprite;
                addWait = 0.4f;

            }
            else if (effects[i].RewardType == RewardTypes.all_maps || effects[i].RewardType == RewardTypes.all_skins || effects[i].RewardType == RewardTypes.no_adv)
            {
                //
            }
            else
            {
                g.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = effects[i].Amount.ToString();
            }
            
            StartCoroutine(playEndEffect(g.GetComponent<RectTransform>(), pos, effects[i].RewardType));

            yield return new WaitForSeconds(1.7f + addWait);
        }

        effects.Clear();

        isActive = false;
    }
    private IEnumerator playEndEffect(RectTransform r, Vector3 pos, RewardTypes _type)
    {
        r.gameObject.SetActive(true);
        r.transform.localScale = Vector3.zero;
        r.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.25f);
        

        r.transform.DOShakeScale(0.2f, 1, 30).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(0.25f);

        yield return new WaitForSeconds(1f);

        r.DOAnchorPos3D(pos, 0.4f).SetEase(Ease.Linear);
        r.transform.DOScale(Vector3.zero, 0.4f).SetEase(Ease.Linear);


        switch (_type)
        {
            case RewardTypes.gold:
                
                if (menu != null) menu.UpdateCurrencies();
                break;

            case RewardTypes.gem:
                
                if (menu != null) menu.UpdateCurrencies();
                break;

            case RewardTypes.xp:
                if (menu != null) menu.UpdateXP();
                break;

            case RewardTypes.newLvl:
                
                if (menu != null) menu.UpdateXP();
                break;
        }


        yield return new WaitForSeconds(0.5f);
        Destroy(r.gameObject);
    }
}

public enum RewardTypes
{
    gold,
    gem,
    xp,
    newLvl,
    newMap,
    all_maps,
    all_skins,
    no_adv
}

public struct ShowRewardEffect
{
    public RewardTypes RewardType;
    public int Amount;

    public ShowRewardEffect(RewardTypes rewardType, int amount)
    {
        RewardType = rewardType;
        Amount = amount;
    }
}
