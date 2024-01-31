using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoAdsPurchase : MonoBehaviour
{
    public string PurchaseID = "no_adv";
    public string Price = "100";

    [SerializeField] private MakePurchase purchase;

    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI mainDescription;
    [SerializeField] private TextMeshProUGUI addDescription1;
    [SerializeField] private TextMeshProUGUI addDescription2;
    [SerializeField] private TextMeshProUGUI priceText;

    [SerializeField] private Button buyButton;
    [SerializeField] private Button close;
    [SerializeField] private GameObject back;

    private bool isReady;

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

    private void Update()
    {
        if (Globals.IsInitiated && !isReady)
        {
            isReady = true;
            title.text = Globals.Language.NoAdsTitle;
            mainDescription.text = Globals.Language.NoAdsMainDescription;
            addDescription1.text = Globals.Language.NoAdsAddDescription1;
            addDescription2.text = Globals.Language.NoAdsAddDescription2;
            priceText.text = Globals.Language.useBuy + ": " + Price + " " + Globals.Language.Yan;
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
