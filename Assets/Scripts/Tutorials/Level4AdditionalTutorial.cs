using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level4AdditionalTutorial : MonoBehaviour
{
    [Header("Camera helper PC")]
    [SerializeField] private GameObject CameraInfoPC;
    [SerializeField] private TextMeshPro CameraInfoPCTexter;

    [Header("Camera helper mobile")]
    [SerializeField] private GameObject CameraInfoMobile;
    [SerializeField] private TextMeshPro CameraInfoMobileTexter;

    private bool isReady;

    // Start is called before the first frame update
    void Start()
    {
        CameraInfoMobile.SetActive(false);
        CameraInfoPC.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Globals.Language != null && !isReady)
        {
            isReady = true;
            Transform obj = default;

            if (Globals.IsMobile)
            {                
                CameraInfoMobile.SetActive(true);
                obj = CameraInfoMobile.transform;
                CameraInfoMobileTexter.text = Globals.Language.CameraHintMobile;

            }
            else
            {                
                CameraInfoPC.SetActive(true);
                obj = CameraInfoPC.transform;
                CameraInfoPCTexter.text = Globals.Language.CameraHintPC;
            }

            Transform mainPlayer = GameManager.Instance.GetMainPlayerTransform();
            obj.position = mainPlayer.position + new Vector3(3,6.9f,0);
            obj.eulerAngles = new Vector3 (0,10,0);

        }
    }
}
