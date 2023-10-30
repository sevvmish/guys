using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Transform mainPlayer;
    private PlayerControl playerControl;
    private Transform mainCamera;
    private Transform outerCamera;
    private readonly Vector3 basePosition = new Vector3(0,6,-8);
    private readonly Vector3 baseRotation = new Vector3(30, 0, 0);

    private bool isUpdate = true;

    public void SetData(Transform player, Transform _camera)
    {
        mainPlayer = player;
        playerControl = mainPlayer.GetComponent<PlayerControl>();
        mainCamera = _camera;
        outerCamera = mainCamera.parent;
        mainCamera.transform.localPosition = basePosition;
        mainCamera.transform.localEulerAngles = baseRotation;
    }

    public void SwapControlBody(Transform newTransform)
    {
        mainPlayer = newTransform;
        isUpdate = false;
        StartCoroutine(playSwap());
    }
    private IEnumerator playSwap()
    {
        outerCamera.DOMove(mainPlayer.position/* + basePosition*/, 0.1f);
        yield return new WaitForSeconds(0.1f);
        isUpdate = true;
    }

    public void ChangeCameraAngleY(float angleY)
    {       
        //outerCamera.eulerAngles = new Vector3(outerCamera.eulerAngles.x, angleY, outerCamera.eulerAngles.z);
    }

    public void ChangeCameraAngleX(float angleX)
    {
        if (!Globals.IsMobile && Mathf.Abs(angleX) > 5) return;
        

        if (angleX > 0 && outerCamera.localEulerAngles.x > 20 && outerCamera.localEulerAngles.x < 30) return;
        if (angleX < 0 && outerCamera.localEulerAngles.x < 340 && outerCamera.localEulerAngles.x > 330) return;

        //if ((angleX > 0 && (outerCamera.localEulerAngles.x < 20 || outerCamera.localEulerAngles.x > 340)) || (angleX < 0 && ((outerCamera.localEulerAngles.x > 340 && outerCamera.localEulerAngles.x < 359.9f) || (outerCamera.localEulerAngles.x >= 0 && outerCamera.localEulerAngles.x < 359.9f))))
        //{
        outerCamera.localEulerAngles = new Vector3(outerCamera.localEulerAngles.x + angleX * 2, outerCamera.localEulerAngles.y, outerCamera.localEulerAngles.z);
        //}
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isUpdate) return;
        outerCamera.position = mainPlayer.position/* + basePosition*/;
        
        if (Globals.IsMobile)
        {
            outerCamera.eulerAngles = new Vector3(outerCamera.eulerAngles.x, playerControl.angleYForMobile, outerCamera.eulerAngles.z);
        }
        else
        {
            outerCamera.eulerAngles = new Vector3(outerCamera.eulerAngles.x, mainPlayer.eulerAngles.y, outerCamera.eulerAngles.z);
        }
    }
}
