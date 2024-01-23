using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeUI : MonoBehaviour
{
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private MenuOptions menuOptions;
    [SerializeField] private Transform location;
    [SerializeField] private GameObject back;
    [SerializeField] private PointerBase pointer;

    [Header("Buttons")]
    [SerializeField] private Button boysB;
    [SerializeField] private Button girlsB;
    [SerializeField] private Button accessoriesB;
    [SerializeField] private Button leftScroll;
    [SerializeField] private Button rightScroll;

    [Header("Use/Buy")]
    [SerializeField] private Button useButton;
    [SerializeField] private Sprite greenBSprite;
    [SerializeField] private Sprite greyBSprite;
    [SerializeField] private GameObject useType;
    [SerializeField] private TextMeshProUGUI useTypeText;
    [SerializeField] private GameObject buyType;
    [SerializeField] private TextMeshProUGUI buyTypeText;
    [SerializeField] private GameObject buyTypeGold;
    [SerializeField] private GameObject buyTypeGem;
    [SerializeField] private TextMeshProUGUI goldAmountText;
    [SerializeField] private TextMeshProUGUI gemAmountText;

    private bool isReady;
    private GameObject[] PlayerSkins;
    private Vector2 boysRange = new Vector2(2,18);
    private Vector2 girlsRange = new Vector2(25, 39);
    
    private DressTypes currentDressType;
    private int currentIndex;
    private float cooldown = 0;
    private float delay = 0.16f;

    private GameObject currentSkin;

    private SkinCost useButtonBehaviour;

    private enum DressTypes
    {
        BoySkin,
        GirlSkin,
        Accessories
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerSkins = new GameObject[50];
        mainMenu.OnBackToMainMenu += ReturnBack;
        back.SetActive(false);

        for (int i = (int)boysRange.x; i <= (int)boysRange.y; i++)
        {
            PlayerSkins[i] = SkinControl.GetSkinGameobject((Skins)i);
            PlayerSkins[i].transform.parent = location;
            PlayerSkins[i].transform.position = Globals.UIPlayerPosition + new Vector3(2.9f + 3.4f, 0.1f, 0);
            PlayerSkins[i].transform.eulerAngles = new Vector3(0, 150, 0);
            PlayerSkins[i].transform.localScale = Vector3.one * 0.9f;
            PlayerSkins[i].SetActive(false);
        }

        for (int i = (int)girlsRange.x; i <= (int)girlsRange.y; i++)
        {
            PlayerSkins[i] = SkinControl.GetSkinGameobject((Skins)i);
            PlayerSkins[i].transform.parent = location;
            PlayerSkins[i].transform.position = Globals.UIPlayerPosition + new Vector3(2.9f + 3.4f, 0.1f, 0);
            PlayerSkins[i].transform.eulerAngles = new Vector3(0, 150, 0);
            PlayerSkins[i].transform.localScale = Vector3.one * 0.9f;
            PlayerSkins[i].SetActive(false);
        }

        leftScroll.onClick.AddListener(() =>
        {            
            SoundUI.Instance.PlayUISound(SoundsUI.click);
            cooldown = delay;
            int d = currentIndex - 1;
            updateByIndex(d, -1);
        });

        rightScroll.onClick.AddListener(() =>
        {
            SoundUI.Instance.PlayUISound(SoundsUI.click);
            cooldown = delay;
            int d = currentIndex + 1;
            updateByIndex(d, 1);
        });

        boysB.onClick.AddListener(() =>
        {
            if (currentDressType == DressTypes.BoySkin) return;
            SoundUI.Instance.PlayUISound(SoundsUI.click);
            cooldown = delay;
            setBoys();
            updateByIndex((int)boysRange.x, 0);
        });

        girlsB.onClick.AddListener(() =>
        {
            if (currentDressType == DressTypes.GirlSkin) return;
            SoundUI.Instance.PlayUISound(SoundsUI.click);
            cooldown = delay;
            currentIndex = (int)girlsRange.x;
            setGirls();
            updateByIndex((int)girlsRange.x, 0);
        });

        useButton.onClick.AddListener(() =>
        {
            int d = 0;
            bool isOK = false;

            switch (useButtonBehaviour.Cost)
            {
                case SkinCost.CostType.none:
                    SoundUI.Instance.PlayUISound(SoundsUI.error);
                    break;

                case SkinCost.CostType.free:
                    SoundUI.Instance.PlayUISound(SoundsUI.positive);
                    isOK = true;
                    
                    break;

                case SkinCost.CostType.gold:
                    SoundUI.Instance.PlayUISound(SoundsUI.positive);
                    isOK = true;
                    Globals.MainPlayerData.G -= useButtonBehaviour.Gold;

                    break;

                case SkinCost.CostType.gem:
                    SoundUI.Instance.PlayUISound(SoundsUI.positive);
                    isOK = true;
                    Globals.MainPlayerData.D -= useButtonBehaviour.Gem;

                    break;
            }

            if (isOK)
            {
                cooldown = delay;

                Globals.MainPlayerData.Skins[currentIndex] = 1;
                Globals.MainPlayerData.CS = currentIndex;
                SaveLoadManager.Save();
                menuOptions.UpdateCurrencyData();

                switch (currentDressType)
                {
                    case DressTypes.BoySkin:
                        if (currentIndex >= boysRange.y)
                        {
                            d = currentIndex - 1;
                            updateByIndex(d, -1);
                        }
                        else
                        {
                            d = currentIndex + 1;
                            updateByIndex(d, 1);
                        }
                        break;

                    case DressTypes.GirlSkin:
                        if (currentIndex >= girlsRange.y)
                        {
                            d = currentIndex - 1;
                            updateByIndex(d, -1);
                        }
                        else
                        {
                            d = currentIndex + 1;
                            updateByIndex(d, 1);
                        }
                        break;
                }

                mainMenu.ChangeMainSkin(true);
            }
        });
    }

    private void checkBorders()
    {
        switch(currentDressType)
        {
            case DressTypes.BoySkin:
                if (currentIndex == boysRange.x || (currentIndex == (boysRange.x + 1) && Globals.MainPlayerData.CS == boysRange.x))
                {
                    leftScroll.gameObject.SetActive(false);
                    rightScroll.gameObject.SetActive(true);
                }
                else if(currentIndex == boysRange.y || (currentIndex == (boysRange.y - 1) && Globals.MainPlayerData.CS == boysRange.y))
                {
                    leftScroll.gameObject.SetActive(true);
                    rightScroll.gameObject.SetActive(false);
                }
                else
                {
                    leftScroll.gameObject.SetActive(true);
                    rightScroll.gameObject.SetActive(true);
                }
                break;

            case DressTypes.GirlSkin:
                if (currentIndex == girlsRange.x || (currentIndex == (girlsRange.x + 1) && Globals.MainPlayerData.CS == girlsRange.x))
                {
                    leftScroll.gameObject.SetActive(false);
                    rightScroll.gameObject.SetActive(true);
                }
                else if (currentIndex == girlsRange.y || (currentIndex == (girlsRange.y - 1) && Globals.MainPlayerData.CS == girlsRange.y))
                {
                    leftScroll.gameObject.SetActive(true);
                    rightScroll.gameObject.SetActive(false);
                }
                else
                {
                    leftScroll.gameObject.SetActive(true);
                    rightScroll.gameObject.SetActive(true);
                }
                break;
        }
    }

    public void SetOn()
    {
        back.SetActive(true);
        mainMenu.GetCameraTransform.DOMove(new Vector3(1.4f+5f, 0, -9), 0.5f).SetEase(Ease.Linear);
        //mainMenu.MainPlayerSkin.transform.eulerAngles = new Vector3(0, 150, 0);
        mainMenu.MainPlayerSkin.SetActive(false);
        currentIndex = (int)boysRange.x;
        setBoys();
        updateByIndex(currentIndex,0);
    }

    private void rotateCharacters(Vector2 delta)
    {
        float speed = 5f;
        float civilY = 0;

        if (delta.x < 0)
        {
            civilY = currentSkin.transform.eulerAngles.y + speed;
        }
        else
        {
            civilY = currentSkin.transform.eulerAngles.y - speed;
        }

        currentSkin.transform.eulerAngles = new Vector3(currentSkin.transform.eulerAngles.x, civilY, currentSkin.transform.eulerAngles.z);
    }

    private void Update()
    {
        Vector2 delta = pointer.DeltaPosition;
        if (delta.x != 0) rotateCharacters(delta);

        if (cooldown > 0)
        {
            if (leftScroll.interactable) leftScroll.interactable = false;
            if (rightScroll.interactable) rightScroll.interactable = false;
            if (boysB.interactable) boysB.interactable = false;
            if (girlsB.interactable) girlsB.interactable = false;
            //if (accessoriesB.interactable) accessoriesB.interactable = false;
            if (useButton.interactable) useButton.interactable = false;
            cooldown -= Time.deltaTime;
        }
        else
        {
            if (!leftScroll.interactable) leftScroll.interactable = true;
            if (!rightScroll.interactable) rightScroll.interactable = true;
            if (!boysB.interactable) boysB.interactable = true;
            if (!girlsB.interactable) girlsB.interactable = true;
            //if (!accessoriesB.interactable) accessoriesB.interactable = true;
            if (!useButton.interactable) useButton.interactable = true;
        }

        if (Globals.IsInitiated && !isReady)
        {
            isReady = true;
            useTypeText.text = Globals.Language.usePutOn;
            buyTypeText.text = Globals.Language.useBuy;
        }
    }

    private void updateByIndex(int index, int direction)
    {
        //print("update from " + currentIndex + " to " + index);

        if (direction == 0)
        {
            direction = 1;
        }

        if (index == Globals.MainPlayerData.CS)
        {
            index+=direction;
        }

        int curr = currentIndex;
        int dir = direction;
        int indexToTake = index;

        for (int i = 0; i < 100; i++)
        {
            if (indexToTake > PlayerSkins.Length || PlayerSkins[indexToTake] == null || indexToTake == Globals.MainPlayerData.CS)
            {                
                switch(currentDressType)
                {
                    case DressTypes.BoySkin:
                        indexToTake += dir;
                        if (indexToTake < boysRange.x)
                        {
                            indexToTake = (int)boysRange.x;
                            return;
                        }
                        else if (indexToTake > boysRange.y)
                        {
                            indexToTake = (int)boysRange.y;
                            return;
                        }
                        break;

                    case DressTypes.GirlSkin:
                        indexToTake += dir;
                        if (indexToTake < girlsRange.x)
                        {
                            indexToTake = (int)girlsRange.x;
                            return;
                        }
                        else if (indexToTake > girlsRange.y)
                        {
                            indexToTake = (int)girlsRange.y;
                            return;
                        }
                        break;

                    case DressTypes.Accessories:
                        
                        break;
                }
                print(indexToTake + " !!!!no!!!!");
            }
            else
            {
                //setAllGameobjectsToFalse();
                //PlayerSkins[indexToTake].SetActive(true);
                StartCoroutine(playChange(currentIndex, indexToTake));
                currentIndex = indexToTake;
                currentSkin = PlayerSkins[currentIndex];
                //print(indexToTake  + " = " + currentIndex + " !!!!yes!!!!");
                updateUI();
                checkBorders();
                return;
            }
        }
    }
    private IEnumerator playChange(int from, int to)
    {
        if (from==to)
        {
            PlayerSkins[to].SetActive(true);
            PlayerSkins[to].transform.localScale = new Vector3(0, 0.9f, 0.9f);
            PlayerSkins[to].transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), delay / 2f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(delay / 2f);
            yield break;
        }


        PlayerSkins[from].transform.DOScale(new Vector3(0, 0.9f, 0.9f), delay/2f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(delay/2f);
        PlayerSkins[from].transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        setAllGameobjectsToFalse();
        
        PlayerSkins[to].SetActive(true);
        PlayerSkins[to].transform.localScale = new Vector3(0, 0.9f, 0.9f);
        PlayerSkins[to].transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), delay / 2f).SetEase(Ease.Linear);
    }

    private void updateUI()
    {
        SkinCost sc = getSkinCost(currentIndex);

        if (Globals.MainPlayerData.Skins[currentIndex] == 1)
        {
            useButton.GetComponent<Image>().sprite = greenBSprite;
            useButtonBehaviour = sc;
            useButtonBehaviour.Cost = SkinCost.CostType.free;
            useType.SetActive(true);
            buyType.SetActive(false);
            return;
        }

        

        switch(sc.Cost)
        {
            case SkinCost.CostType.free:
                useButton.GetComponent<Image>().sprite = greenBSprite;
                useButtonBehaviour = sc;
                useType.SetActive(true);
                buyType.SetActive(false);
                break;

            case SkinCost.CostType.gold:

                useType.SetActive(false);
                buyType.SetActive(true);
                buyTypeGold.SetActive(true);
                buyTypeGem.SetActive(false);

                if (Globals.MainPlayerData.G >= sc.Gold)
                {
                    useButton.GetComponent<Image>().sprite = greenBSprite;
                    goldAmountText.color = Color.yellow;
                    useButtonBehaviour = sc;
                }
                else
                {
                    useButton.GetComponent<Image>().sprite = greyBSprite;
                    goldAmountText.color = Color.red;
                    useButtonBehaviour = sc;
                    useButtonBehaviour.Cost = SkinCost.CostType.none;
                }

                goldAmountText.text = sc.Gold.ToString();

                
                break;

            case SkinCost.CostType.gem:

                useType.SetActive(false);
                buyType.SetActive(true);
                buyTypeGold.SetActive(false);
                buyTypeGem.SetActive(true);

                if (Globals.MainPlayerData.D >= sc.Gem)
                {
                    useButton.GetComponent<Image>().sprite = greenBSprite;
                    gemAmountText.color = Color.yellow;
                    useButtonBehaviour = sc;
                }
                else
                {
                    useButton.GetComponent<Image>().sprite = greyBSprite;
                    gemAmountText.color = Color.red;
                    useButtonBehaviour = sc;
                    useButtonBehaviour.Cost = SkinCost.CostType.none;
                }

                gemAmountText.text = sc.Gem.ToString();

                
                break;
        }

        
    }

    private SkinCost getSkinCost(int skin)
    {
        SkinCost result = new SkinCost();

        switch(skin)
        {
            case 2:
                return result.SkinCostFree();
            case 3:
                return result.SkinCostFree();
            case 4:
                return result.SkinCostFree();
            case 5:
                return result.SkinCostFree();
            case 6:
                return result.SkinCostFree();
            case 7:
                return result.SkinCostFree();

            case 8:
                return result.SkinCostGold(50);
            case 9:
                return result.SkinCostGold(50);
            case 10:
                return result.SkinCostGold(50);
            case 11:
                return result.SkinCostGold(80);
            case 12:
                return result.SkinCostGold(80);
            case 13:
                return result.SkinCostGold(80);
            case 14:
                return result.SkinCostGold(120);
            case 15:
                return result.SkinCostGold(120);
            case 16:
                return result.SkinCostGold(150);
            case 17:
                return result.SkinCostGold(150);
            case 18:
                return result.SkinCostGold(150);




            case 25:
                return result.SkinCostFree();
            case 26:
                return result.SkinCostFree();
            case 27:
                return result.SkinCostFree();
            case 28:
                return result.SkinCostFree();
            case 29:
                return result.SkinCostFree();

            case 30:
                return result.SkinCostGold(50);
            case 31:
                return result.SkinCostGold(50);
            case 32:
                return result.SkinCostGold(50);
            case 33:
                return result.SkinCostGold(80);
            case 34:
                return result.SkinCostGold(80);
            case 35:
                return result.SkinCostGold(120);
            case 36:
                return result.SkinCostGold(120);
            case 37:
                return result.SkinCostGold(120);
            case 38:
                return result.SkinCostGold(150);
            case 39:
                return result.SkinCostGold(150);
        }

        return result;
    } 

    private void setBoys()
    {
        setAllGameobjectsToFalse();
        currentDressType = DressTypes.BoySkin;        
        boysB.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 1);
        girlsB.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        //accessoriesB.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

    private void setGirls()
    {
        setAllGameobjectsToFalse();
        currentDressType = DressTypes.GirlSkin;
        boysB.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        girlsB.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 1);
        //accessoriesB.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

    private void setAccessories()
    {
        currentDressType = DressTypes.Accessories;
        boysB.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        girlsB.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        //accessoriesB.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 1);
    }

    private void setAllGameobjectsToFalse()
    {
        for (int i = 0; i < PlayerSkins.Length; i++)
        {
            if (PlayerSkins[i] != null && PlayerSkins[i].activeSelf)
            {
                PlayerSkins[i].SetActive(false);
            }
        }
    }

    private void ReturnBack()
    {
        setAllGameobjectsToFalse();
        mainMenu.BackToMainMenu();
    }

    

}

public struct SkinCost
{
    public enum CostType
    {
        none,
        free,
        gold,
        gem,
        real
    }

    public CostType Cost;
    public int Gold;
    public int Gem;
    public int Real;

    public SkinCost(CostType cost, int gold, int gem, int real)
    {
        Cost = cost;
        Gold = gold;
        Gem = gem;
        Real = real;
    }

    public SkinCost SkinCostFree()
    {
        Cost = CostType.free;
        Gold = 0;
        Gem = 0;
        Real = 0;
        return this;
    }

    public SkinCost SkinCostGold(int gold)
    {
        Cost = CostType.gold;
        Gold = gold;
        Gem = 0;
        Real = 0;
        return this;
    }

    public SkinCost SkinCostGem(int gem)
    {
        Cost = CostType.gem;
        Gold = 0;
        Gem = gem;
        Real = 0;
        return this;
    }

    public SkinCost SkinCostReal(int real)
    {
        Cost = CostType.real;
        Gold = 0;
        Gem = 0;
        Real = real;
        return this;
    }
}
