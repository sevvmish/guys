using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetRewardSystem : MonoBehaviour
{
    public static GetRewardSystem Instance { get; private set; }

    [SerializeField] private MainMenu menu;
    [SerializeField] private RectTransform goldRect;
    [SerializeField] private RectTransform gemRect;
    [SerializeField] private RectTransform xpRect;

    private List<ShowRewardEffect> effects = new List<ShowRewardEffect>();
    private bool isActive;
    private bool isReady;

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
    }

    public void ShowEffect(RewardTypes _type, int amount)
    {
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
        }

        if (effects.Count > 0 && !isActive)
        {
            StartCoroutine(playEffects());
        }
    }

    private IEnumerator playEffects()
    {
        isActive = true;

        for (int i = 0; i < effects.Count; i++)
        {
            GameObject g = default;
            Vector3 pos = Vector3.zero;

            switch(effects[i].RewardType)
            {
                case RewardTypes.gold:
                    g = Instantiate(goldRect.gameObject, transform);
                    pos = new Vector3(500, 500,0);
                    break;

                case RewardTypes.gem:
                    g = Instantiate(gemRect.gameObject, transform);
                    pos = new Vector3(700, 500,0);
                    break;

                case RewardTypes.xp:
                    g = Instantiate(xpRect.gameObject, transform);
                    pos = new Vector3(-700, -500,0);
                    break;
            }

            g.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = effects[i].Amount.ToString();
            StartCoroutine(playEndEffect(g.GetComponent<RectTransform>(), pos, effects[i].RewardType));

            yield return new WaitForSeconds(1.7f);
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

        SoundUI.Instance.PlayUISound(SoundsUI.cash);
        if (menu != null)
        {
            switch (_type)
            {
                case RewardTypes.gold:
                    menu.UpdateCurrencies();
                    break;

                case RewardTypes.gem:
                    menu.UpdateCurrencies();
                    break;

                case RewardTypes.xp:
                    menu.UpdateXP();
                    break;
            }
        }
        

        yield return new WaitForSeconds(0.5f);
        Destroy(r.gameObject);
    }
}

public enum RewardTypes
{
    gold,
    gem,
    xp
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
