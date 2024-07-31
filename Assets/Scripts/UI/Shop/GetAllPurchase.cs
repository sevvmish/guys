using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GetAllPurchase : MonoBehaviour
{
    public string PurchaseID = "get_all";
    public string Price = "200";
    public string OldPrice = "300";

    [SerializeField] private MakePurchase purchase;

    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI mainDescription;
    [SerializeField] private TextMeshProUGUI addDescription1;
    [SerializeField] private TextMeshProUGUI addDescription2;
    [SerializeField] private TextMeshProUGUI addDescription3;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI newPriceText;
    [SerializeField] private TextMeshProUGUI oldPriceText;

    [SerializeField] private Button buyButton;
    [SerializeField] private Button close;
    [SerializeField] private GameObject back;

    private bool isReady;
    //private Purchase currentPurchaseData;

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

    private void OnEnable()
    {
        //currentPurchaseData = YandexGame.PurchaseByID(PurchaseID);
        //newPriceText.text = currentPurchaseData.priceValue + " " + currentPurchaseData.currencyCode;
    }

    private void Update()
    {
        if (Globals.IsInitiated && !isReady)
        {
            isReady = true;
            title.text = Globals.Language.GetAllTitle;
            mainDescription.text = Globals.Language.GetAllMainDescription;
            addDescription1.text = Globals.Language.GetAllAddDescription1;
            addDescription2.text = Globals.Language.GetAllAddDescription2;
            addDescription3.text = Globals.Language.GetAllAddDescription3;
            priceText.text = Globals.Language.useBuy + ":";
            //newPriceText.text = Price + " " + Globals.Language.Yan;
            //oldPriceText.text = OldPrice + " " + currentPurchaseData.currencyCode;
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
