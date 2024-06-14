using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [Header("Scroll View")]
    [SerializeField] private Transform panelLocation;
    [SerializeField] private GameObject panelExample;
    [SerializeField] private GameObject mainScroll;
    private SkinPanelUI[] panels;

    private bool isReady;
    private GameObject[] PlayerSkins;
    private Vector2 boysRange;
    private Vector2 girlsRange;
    
    private DressTypes currentDressType;
    private int currentIndex;
    private float cooldown = 0;
    private float delay = 0.16f;

    private GameObject currentSkin;

    private SkinCost useButtonBehaviour;

    

    // Start is called before the first frame update
    void Start()
    {
        boysRange = Globals.MaleSkins;
        girlsRange = Globals.FemaleSkins;

        PlayerSkins = new GameObject[50];
        mainMenu.OnBackToMainMenu += ReturnBack;
        back.SetActive(false);
        panelExample.SetActive(false);

        //init scrol
        panels = new SkinPanelUI[50];
        for (int i = 0; i < panels.Length; i++)
        {
            DressTypes d = DressTypes.BoySkin;

            if (i>=boysRange.x && i<=boysRange.y)
            {
                d = DressTypes.BoySkin;
            }
            else if (i >= girlsRange.x && i <= girlsRange.y)
            {
                d = DressTypes.GirlSkin;
            }

            GameObject p = Instantiate(panelExample, panelLocation);
            p.SetActive(false);
            SkinPanelUI panel = p.GetComponent<SkinPanelUI>();

            SkinCost cost = getSkinCost(i);
            int quality = 0;
            if (cost.Cost == SkinCost.CostType.free)
            {
                quality = 0;
            }
            else
            {
                switch (cost.Gold)
                {
                    case 100:
                        quality = 1;
                        break;

                    case 200:
                        quality = 2;
                        break;

                    case 300:
                        quality = 3;
                        break;

                    case 400:
                        quality = 3;
                        break;
                }
            }

            panel.SetData(GetSkinIconByType(i), i, d, quality, changeCurrentIndexByClick);
            panels[i] = panel;
        }

        


        for (int i = (int)boysRange.x; i <= (int)boysRange.y; i++)
        {
            PlayerSkins[i] = SkinControl.GetSkinGameobject((Skins)i);
            PlayerSkins[i].transform.parent = location;
            PlayerSkins[i].transform.position = Globals.UIPlayerPosition + new Vector3(-1.15f, 0.1f, 0);
            PlayerSkins[i].transform.eulerAngles = new Vector3(0, 150, 0);
            PlayerSkins[i].transform.localScale = Vector3.one * 0.9f;
            PlayerSkins[i].SetActive(false);
        }

        for (int i = (int)girlsRange.x; i <= (int)girlsRange.y; i++)
        {
            PlayerSkins[i] = SkinControl.GetSkinGameobject((Skins)i);
            PlayerSkins[i].transform.parent = location;
            PlayerSkins[i].transform.position = Globals.UIPlayerPosition + new Vector3(-1.15f, 0.1f, 0);
            PlayerSkins[i].transform.eulerAngles = new Vector3(0, 150, 0);
            PlayerSkins[i].transform.localScale = Vector3.one * 0.9f;
            PlayerSkins[i].SetActive(false);
        }

        
        setBoys();

        /*
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
        */
        leftScroll.gameObject.SetActive(false);
        rightScroll.gameObject.SetActive(false);


        boysB.onClick.AddListener(() =>
        {
            if (currentDressType == DressTypes.BoySkin) return;
            SoundUI.Instance.PlayUISound(SoundsUI.click);
            cooldown = delay;
            setBoys();
            //updateByIndex((int)boysRange.x, 0);
        });

        girlsB.onClick.AddListener(() =>
        {
            if (currentDressType == DressTypes.GirlSkin) return;
            SoundUI.Instance.PlayUISound(SoundsUI.click);
            cooldown = delay;
            currentIndex = (int)girlsRange.x;
            setGirls();
            //updateByIndex((int)girlsRange.x, 0);
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

                panels.ToList().ForEach(item => {
                    if (item.IsChosen) item.SetCurrentlyChosen(false);
                });

                panels[currentIndex].SetCurrentlyChosen(true);
                updateUI();
                /*
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
                */
                mainMenu.ChangeMainSkin(true);
            }
        });
    }

    private void changeCurrentIndexByClick(int newIndex)
    {
        //if (newIndex == currentIndex) return;

        panels.ToList().ForEach(item => { 
            if (item.IsClicked) item.SetCurrentlyClicked(false); 
        });

        panels[newIndex].SetCurrentlyClicked(true);

        if (currentSkin!=null) currentSkin.SetActive(false);

        currentIndex = newIndex;

        currentSkin = PlayerSkins[currentIndex];

        currentSkin.SetActive(true);
        updateUI();
    }

    /*
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
    }*/

    private void skinUpdate()
    {
        int currSkin = currentIndex;

        if (currSkin >= boysRange.x && currSkin <= boysRange.y)
        {
            if (currentDressType != DressTypes.BoySkin) setBoys();
        }
        else if (currSkin >= girlsRange.x && currSkin <= girlsRange.y)
        {
            if (currentDressType != DressTypes.GirlSkin) setGirls();
        }
                
        panels[currSkin].SetCurrentlyChosen(true);
        changeCurrentIndexByClick(currentIndex);
        updateUI();
    }

    public void SetOn()
    {
        back.SetActive(true);
        //mainMenu.GetCameraTransform.DOMove(new Vector3(1.4f+5f, 0, -9), 0.5f).SetEase(Ease.Linear);
        //mainMenu.MainPlayerSkin.transform.eulerAngles = new Vector3(0, 150, 0);
        mainMenu.MainPlayerSkin.SetActive(false);

        currentIndex = Globals.MainPlayerData.CS;
        skinUpdate();


        int currSkin = currentIndex;

        if (currSkin >= boysRange.x && currSkin <= boysRange.y)
        {
            setBoys();
        }
        else if (currSkin >= girlsRange.x && currSkin <= girlsRange.y)
        {
            setGirls();
        }
        //currentIndex = (int)boysRange.x;
        //setBoys();
        //updateByIndex(currentIndex,0);


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
            
            if (Globals.IsMobile)
            {
                mainScroll.transform.localScale = Vector3.one * 0.9f;
            }
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
                //checkBorders();
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
        if (currentIndex == Globals.MainPlayerData.CS)
        {
            useButton.gameObject.SetActive(false);
            return;
        }

        useButton.gameObject.SetActive(true);
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

    public static Sprite GetSkinIconByType(int skinID)
    {
        Skins skin = (Skins)skinID;

        switch (skin)
        {            
            case Skins.civilian_male_1: 
                return Resources.Load<Sprite>("Skins/icons/char2_3d_male");
                

            case Skins.civilian_male_2: 
                return Resources.Load<Sprite>("Skins/icons/old_male_02");
                

            case Skins.civilian_male_3: 
                return Resources.Load<Sprite>("Skins/icons/char2male1");
                

            case Skins.civilian_male_4: 
                return Resources.Load<Sprite>("Skins/icons/char11_3d_male");
                

            case Skins.civilian_male_5: 
                return Resources.Load<Sprite>("Skins/icons/old_male_03");
                

            case Skins.civilian_male_6: 
                return Resources.Load<Sprite>("Skins/icons/char2male2");
                

            case Skins.civilian_male_gold_0:
                return Resources.Load<Sprite>("Skins/icons/old_male_04");
                

            case Skins.civilian_male_gold_1:
                return Resources.Load<Sprite>("Skins/icons/char71_3d_male");
                

            case Skins.civilian_male_gold_2:
                return Resources.Load<Sprite>("Skins/icons/old_male_01");
                

            case Skins.civilian_male_gold_3:
                return Resources.Load<Sprite>("Skins/icons/char56_3d_male");
                

            case Skins.civilian_male_gold_4:
                return Resources.Load<Sprite>("Skins/icons/char35_3d_male");
                

            case Skins.civilian_male_gold_5:
                return Resources.Load<Sprite>("Skins/icons/char83_3d_male");
                

            case Skins.civilian_male_gem_1:
                return Resources.Load<Sprite>("Skins/icons/char77_3d_male");
                

            case Skins.civilian_male_gem_2:
                return Resources.Load<Sprite>("Skins/icons/char70_3d_male");
                

            case Skins.civilian_male_gem_3:
                return Resources.Load<Sprite>("Skins/icons/char76_3d_male");
                

            case Skins.civilian_male_gem_4:
                return Resources.Load<Sprite>("Skins/icons/char86_3d_male");
                

            case Skins.civilian_male_gem_5:
                return Resources.Load<Sprite>("Skins/icons/char53_3d_all");
                

            case Skins.civilian_male_gem_6:
                return Resources.Load<Sprite>("Skins/icons/char78_3d_all");
                







            case Skins.civilian_female_1:
                return Resources.Load<Sprite>("Skins/icons/char7_3d_female");
                

            case Skins.civilian_female_2:
                return Resources.Load<Sprite>("Skins/icons/old_female_03");
                

            case Skins.civilian_female_3:
                return Resources.Load<Sprite>("Skins/icons/char2fem1");
                

            case Skins.civilian_female_4:
                return Resources.Load<Sprite>("Skins/icons/char32_3d_female");
                

            case Skins.civilian_female_5:
                return Resources.Load<Sprite>("Skins/icons/char109_3d_female");
                

            case Skins.civilian_female_gold_1:
                return Resources.Load<Sprite>("Skins/icons/char2fem4");
                

            case Skins.civilian_female_gold_2:
                return Resources.Load<Sprite>("Skins/icons/char103_3d_female");
                

            case Skins.civilian_female_gold_3:
                return Resources.Load<Sprite>("Skins/icons/char2fem2");
                

            case Skins.civilian_female_gold_4:
                return Resources.Load<Sprite>("Skins/icons/char2fem3");
                

            case Skins.civilian_female_gold_5:
                return Resources.Load<Sprite>("Skins/icons/old_female_02");
                

            case Skins.civilian_female_gem_1:
                return Resources.Load<Sprite>("Skins/icons/char12_3d_female");
                

            case Skins.civilian_female_gem_2:
                return Resources.Load<Sprite>("Skins/icons/old_female_01");
                

            case Skins.civilian_female_gem_3:
                return Resources.Load<Sprite>("Skins/icons/char43_3d_female");
                

            case Skins.civilian_female_gem_4:
                return Resources.Load<Sprite>("Skins/icons/char47_3d_female");
                

            case Skins.civilian_female_gem_5:
                return Resources.Load<Sprite>("Skins/icons/char57_3d_female");
                

            case Skins.civilian_female_gem_6:
                return Resources.Load<Sprite>("Skins/icons/char60_3d_female");
                

            case Skins.civilian_female_gem_7:
                return Resources.Load<Sprite>("Skins/icons/char62_3d_female");
                

            case Skins.civilian_female_gem_8:
                return Resources.Load<Sprite>("Skins/icons/char77_3d_female");
                

            case Skins.civilian_female_gem_9:
                return Resources.Load<Sprite>("Skins/icons/charXX_3d_all");
                




            default:
                return Resources.Load<Sprite>("Skins/icons/char2_3d_male");
        }


        return null;
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
                return result.SkinCostGold(100);

            case 8:
                return result.SkinCostGold(100);
            case 9:
                return result.SkinCostGold(100);
            case 10:
                return result.SkinCostGold(100);
            case 11:
                return result.SkinCostGold(200);
            case 12:
                return result.SkinCostGold(200);
            case 13:
                return result.SkinCostGold(200);
            case 14:
                return result.SkinCostGold(200);
            case 15:
                return result.SkinCostGold(300);
            case 16:
                return result.SkinCostGold(300);
            case 17:
                return result.SkinCostGold(300);
            case 18:
                return result.SkinCostGold(400);
            case 19:
                return result.SkinCostGold(400);




            case 25:
                return result.SkinCostFree();
            case 26:
                return result.SkinCostFree();
            case 27:
                return result.SkinCostFree();
            case 28:
                return result.SkinCostFree();


            case 29:
                return result.SkinCostGold(100);

            case 30:
                return result.SkinCostGold(100);
            case 31:
                return result.SkinCostGold(100);
            case 32:
                return result.SkinCostGold(100);
            case 33:
                return result.SkinCostGold(200);
            case 34:
                return result.SkinCostGold(200);
            case 35:
                return result.SkinCostGold(200);
            case 36:
                return result.SkinCostGold(200);
            case 37:
                return result.SkinCostGold(300);
            case 38:
                return result.SkinCostGold(300);
            case 39:
                return result.SkinCostGold(300);
            case 40:
                return result.SkinCostGold(300);
            case 41:
                return result.SkinCostGold(400);
            case 42:
                return result.SkinCostGold(400);
            case 43:
                return result.SkinCostGold(400);
        }

        return result;
    } 

    private void setBoys()
    {
        setAllGameobjectsToFalse();
        currentDressType = DressTypes.BoySkin;        
        boysB.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        boysB.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        girlsB.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 0.3f);
        girlsB.transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1, 0.7f);

        boysB.transform.GetChild(1).gameObject.SetActive(true);
        girlsB.transform.GetChild(1).gameObject.SetActive(false);

        panels.ToList().ForEach(x => { if (x.SkinID>=boysRange.x && x.SkinID <= boysRange.y) x.gameObject.SetActive(true); });

        if (currentSkin!= null) currentSkin.SetActive(true);

        //accessoriesB.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

    private void setGirls()
    {
        setAllGameobjectsToFalse();
        currentDressType = DressTypes.GirlSkin;
        boysB.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 0.3f);
        boysB.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.7f);
        girlsB.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        girlsB.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1f);

        boysB.transform.GetChild(1).gameObject.SetActive(false);
        girlsB.transform.GetChild(1).gameObject.SetActive(true);

        panels.ToList().ForEach(x => { if (x.SkinID >= girlsRange.x && x.SkinID <= girlsRange.y) x.gameObject.SetActive(true); });
        //accessoriesB.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        if (currentSkin != null) currentSkin.SetActive(true);
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

        panels.ToList().ForEach(x => { if (x.gameObject.activeSelf) x.gameObject.SetActive(false); });
    }

    private void ReturnBack()
    {
        setAllGameobjectsToFalse();
        mainMenu.BackToMainMenu();
    }

    

}

public enum DressTypes
{
    BoySkin,
    GirlSkin,
    Accessories
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
