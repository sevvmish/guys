using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG.Utils.Pay;
using YG;

public class AllMapsPurchase : MonoBehaviour
{
    public string PurchaseID = "all_maps";
    public string Price = "120";

    [SerializeField] private MakePurchase purchase;

    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI mainDescription;
    [SerializeField] private TextMeshProUGUI priceText;

    [SerializeField] private Button buyButton;
    [SerializeField] private Button close;
    [SerializeField] private GameObject back;

    private bool isReady;
    private Purchase currentPurchaseData;

    // Start is called before the first frame update
    void Start()
    {
        //close.gameObject.SetActive(false);
        //back.SetActive(false);

        buyButton.onClick.AddListener(() =>
        {
            SoundUI.Instance.PlayUISound(SoundsUI.click);
            StartCoroutine(playPurchase());
            purchase.Buy(PurchaseID);
            gameObject.SetActive(false);
        });

        close.onClick.AddListener(() =>
        {
            SoundUI.Instance.PlayUISound(SoundsUI.click);
            this.gameObject.SetActive(false);
        });
    }

    private void OnEnable()
    {
        currentPurchaseData = YandexGame.PurchaseByID(PurchaseID);
        priceText.text = Globals.Language.useBuy + ": " + currentPurchaseData.priceValue + " " + currentPurchaseData.currencyCode;
    }

    public void TurnOnCloseButton()
    {
        close.gameObject.SetActive(true);
        back.SetActive(true);
        //StartCoroutine (playON());
    }
    private IEnumerator playON()
    {
        yield return new WaitForSeconds(1);
        close.gameObject.SetActive(true);
        back.SetActive(true);
    }

    private void Update()
    {
        if (Globals.IsInitiated && !isReady)
        {
            isReady = true;
            title.text = Globals.Language.AllMapsTitle;
            mainDescription.text = Globals.Language.AllMapsMainDescription;
            //priceText.text = Globals.Language.useBuy + ": " + Price + " " + Globals.Language.Yan;
        }
    }

    private IEnumerator playPurchase()
    {
        buyButton.interactable = false;
        transform.DOPunchPosition(new Vector3(10, 10, 1), 0.5f, 30).SetEase(Ease.InOutQuad);

        yield return new WaitForSeconds(1);
        buyButton.interactable = true;
    }

}
