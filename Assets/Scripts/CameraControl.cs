using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Transform mainPlayer;
    private Transform mainCamera;
    private readonly Vector3 basePosition = new Vector3(0,6,-8);
    private readonly Vector3 baseRotation = new Vector3(30, 0, 0);

    private bool isUpdate = true;

    public void SetData(Transform player, Transform _camera)
    {
        mainPlayer = player;
        mainCamera = _camera;
        mainCamera.transform.eulerAngles = baseRotation;
    }

    public void SwapControlBody(Transform newTransform)
    {
        mainPlayer = newTransform;
        isUpdate = false;
        StartCoroutine(playSwap());
    }
    private IEnumerator playSwap()
    {
        mainCamera.DOMove(mainPlayer.position + basePosition, 0.1f);
        yield return new WaitForSeconds(0.1f);
        isUpdate = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isUpdate) return;
        mainCamera.position = mainPlayer.position + basePosition;
    }
}
