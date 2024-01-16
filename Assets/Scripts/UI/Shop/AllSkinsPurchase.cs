using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AllSkinsPurchase : MonoBehaviour
{
    public string PurchaseID = "all_skins";
    public string OldPrice = "100";
    public string Price = "75";

    [SerializeField] private MakePurchase purchase;

    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI addDescription1;
    [SerializeField] private TextMeshProUGUI addDescription2;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI priceOldText;
    [SerializeField] private TextMeshProUGUI priceNewText;

    [SerializeField] private Button buyButton;
    [SerializeField] private Button close;

    private bool isReady;

    // Start is called before the first frame update
    void Start()
    {
        close.gameObject.SetActive(false);

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

    public void TurnOnCloseButton() => close.gameObject.SetActive(true);

    private void Update()
    {
        if (Globals.IsInitiated && !isReady)
        {
            isReady = true;
            title.text = Globals.Language.AllSkinsTitle;
            addDescription1.text = Globals.Language.AllSkinsMainDescription1;
            addDescription2.text = Globals.Language.AllSkinsMainDescription2;
            priceText.text = Globals.Language.useBuy + ":";
            priceOldText.text = OldPrice + " " + Globals.Language.Yan;
            priceNewText.text = Price + " " + Globals.Language.Yan;
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
