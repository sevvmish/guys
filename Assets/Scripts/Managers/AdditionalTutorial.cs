using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdditionalTutorial : MonoBehaviour
{
    [Header("mobile movement")]
    [SerializeField] private GameObject MovementForMobile;
    [SerializeField] private TextMeshPro MovementLeftJTexter;

    [Header("mobile jump")]
    [SerializeField] private GameObject JumpForMobile;
    [SerializeField] private TextMeshPro JumpRightJTexter;

    [Header("PC jump")]
    [SerializeField] private GameObject JumpForPC;
    [SerializeField] private TextMeshPro JumpPCTexter;

    [Header("double jump")]
    [SerializeField] private GameObject DoubleJump;
    [SerializeField] private TextMeshPro DoubleJumpText;

    [Header("Camera helper PC")]
    [SerializeField] private GameObject CameraInfoPC;
    [SerializeField] private TextMeshPro CameraInfoPCTexter;

    [Header("Camera helper mobile")]
    [SerializeField] private GameObject CameraInfoMobile;
    [SerializeField] private TextMeshPro CameraInfoMobileTexter;

    [Header("Dont forget double jump")]
    [SerializeField] private GameObject DontForgetDJ;
    [SerializeField] private TextMeshPro DontForgetDJTexter;

    [Header("PC movement")]
    [SerializeField] private GameObject MovementForPC;
    [SerializeField] private TextMeshPro MovementKeyboard;
    [SerializeField] private TextMeshPro MovementLeftW;
    [SerializeField] private TextMeshPro MovementLeftA;
    [SerializeField] private TextMeshPro MovementLeftS;
    [SerializeField] private TextMeshPro MovementLeftD;

    private bool isTrigger1, isTrigger2, isFinish;

    [SerializeField] private Transform from;
    [SerializeField] private Transform to;


    private GameManager gm;
    private GameObject[] bots = new GameObject[10];
    private float _timer;
    private float[] timers = new float[] {2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
    private int botIndex = 0;

    private void Start()
    {
        MovementForMobile.SetActive(false);
        MovementForPC.SetActive(false);
        JumpForMobile.SetActive(false);
        JumpForPC.SetActive(false);
        DoubleJump.SetActive(false);
        CameraInfoPC.SetActive(false);
        CameraInfoMobile.SetActive(false);
        DontForgetDJ.SetActive(false);


        gm = GameManager.Instance;
        bool isLeft = false;

        for (int i = 0; i < bots.Length; i++)
        {
            Vector3 pos = Vector3.zero;
            if (isLeft)
            {
                isLeft = false;
                pos = new Vector3 (-3, 0, 0);
            }
            else
            {
                isLeft = true;
                pos = new Vector3(3, 0, 0);
            }

            int sex = UnityEngine.Random.Range(0, 2);
            Skins skins = Skins.civilian_male_1;

            switch (sex)
            {
                case 0:
                    skins = (Skins)UnityEngine.Random.Range(2, 18);
                    break;

                case 1:
                    skins = (Skins)UnityEngine.Random.Range(25, 39);
                    break;
            }


            bots[i] = gm.AddPlayer(false, pos, Vector3.zero, skins);
            bots[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (!gm.IsGameStarted) return;

        if (isTrigger1 && !bots[0].activeSelf)
        {
            bots[0].SetActive(true);
        }

        if (isTrigger2 && !bots[1].activeSelf)
        {
            bots[1].SetActive(true);
            _timer = 0;
        }

        if (bots[1].activeSelf && !bots[2].activeSelf)
        {
            if (_timer > 5)
            {
                _timer = 0;
                bots[2].SetActive(true);
                botIndex = 3;
            }
            else
            {
                _timer += Time.deltaTime;
            }
        }

        if (bots[1].activeSelf && bots[2].activeSelf)
        {
            if (_timer > 5)
            {
                _timer = 0;
                bots[botIndex].SetActive(true);

                if ((botIndex + 1) < bots.Length)
                {
                    botIndex++;
                }
            }
            else
            {
                _timer += Time.deltaTime;
            }
                
        }
        


        if (Globals.Language != null)
        {
            if (Globals.IsMobile)
            {
                MovementForMobile.SetActive(true);
                MovementLeftJTexter.text = Globals.Language.MovementHintLeftJ;

                JumpForMobile.SetActive(true);
                JumpRightJTexter.text = Globals.Language.JumpHintRightJ;

                CameraInfoMobile.SetActive(true);
                CameraInfoMobileTexter.text = Globals.Language.CameraHintMobile;
            }
            else
            {
                MovementForPC.SetActive(true);
                MovementLeftW.text = Globals.Language.UpArrowLetter;
                MovementLeftS.text = Globals.Language.DownArrowLetter;
                MovementLeftA.text = Globals.Language.LeftArrowLetter;
                MovementLeftD.text = Globals.Language.RightArrowLetter;
                MovementKeyboard.text = Globals.Language.MovementHintLetters;

                JumpForPC.SetActive(true);
                JumpPCTexter.text = Globals.Language.JumpHintKeyboard;

                CameraInfoPC.SetActive(true);
                CameraInfoPCTexter.text = Globals.Language.CameraHintPC;
            }

            DoubleJump.SetActive(true);
            DoubleJumpText.text = Globals.Language.DoubleJumpHint;

            DontForgetDJ.SetActive(true);
            DontForgetDJTexter.text = Globals.Language.DontForgetDoubleJump;
        }
    }

    public void Trigger1() => isTrigger1 = true;
    public void Trigger2() => isTrigger2 = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerControl pc))
        {
            if (pc.IsItMainPlayer)
            {
                gm.AddPlayerFinished(pc);
                gm.GetCameraControl().ResetCameraOnRespawn();
                StartCoroutine(playFinish(pc));
            }
            else
            {
                pc.FinishReached();
                pc.transform.position = Vector3.Lerp(from.position, to.position, UnityEngine.Random.Range(0.1f, 0.95f));
            }
        }

    }
    private IEnumerator playFinish(PlayerControl player)
    {
        player.FinishReached();

        yield return new WaitForSeconds(0.1f);
        player.transform.eulerAngles = new Vector3(0, 180, 0);
    }
}
