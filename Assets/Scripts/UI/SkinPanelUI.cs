using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkinPanelUI : MonoBehaviour
{
    public int SkinID { get; private set; }
    public bool IsChosen { get; private set; }
    public bool IsClicked { get; private set; }

    [SerializeField] private Image back;
    [SerializeField] private Image mainIcon;

    [SerializeField] private GameObject inUseToggle;
    [SerializeField] private GameObject inUseOutline;

    [SerializeField] private GameObject[] glowsimple;
    [SerializeField] private GameObject[] glowMedium;
    [SerializeField] private GameObject[] glowHigh;

    [SerializeField] private Button pressAction;

    private DressTypes dressType;    
    private int quality;    
    private Action<int> panelPressed;

    public void SetData(Sprite sprite, int skin, DressTypes dress, int q, Action<int> panelPressed)
    {        
        mainIcon.sprite = sprite;

        dressType = dress;
        SkinID = skin;
        quality = q;

        glowsimple.ToList().ForEach(x => x.SetActive(false));
        glowMedium.ToList().ForEach(x => x.SetActive(false));
        glowHigh.ToList().ForEach(x => x.SetActive(false));
        SetQuality();

        IsChosen = false;
        IsClicked = false;
        inUseToggle.SetActive(false);
        inUseOutline.SetActive(false);

        this.panelPressed = panelPressed;
        pressAction.onClick.AddListener(() => { this.panelPressed.Invoke(SkinID); });
        
        
    }

    public void SetQuality()
    {
        switch(quality)
        {
            default:
                back.color = new Color(195f/256f, 158f/256f, 122f/256f, 1);
                //if (!Globals.IsLowFPS) glowsimple.ToList().ForEach(x => x.SetActive(true));
                break;

            case 1:
                back.color = new Color(124f/256f, 195f/256f, 122f/256f, 1);
                //if (!Globals.IsLowFPS) glowsimple.ToList().ForEach(x => x.SetActive(true));
                break;

            case 2:
                back.color = new Color(38f/256f, 105f/256f, 197f/256f, 1);
                //if (!Globals.IsLowFPS) glowMedium.ToList().ForEach(x => x.SetActive(true));
                break;

            case 3:
                back.color = new Color(195f/256f, 68f/256f, 186f/256f, 1);
                //if (!Globals.IsLowFPS) glowHigh.ToList().ForEach(x => x.SetActive(true));
                break;
        }
    }

    public void SetCurrentlyChosen(bool isChosen)
    {
        this.IsChosen = isChosen;
        inUseToggle.SetActive(isChosen);
    }

    public void SetCurrentlyClicked(bool isClicked)
    {        
        this.IsClicked = isClicked;
        inUseOutline.SetActive(isClicked);
    }


}
