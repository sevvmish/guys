using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerCameraLookTrigger : MonoBehaviour
{
    public float Xshift;
    private CameraControl cc;

    private void Start()
    {
        cc = GameManager.Instance.GetCameraControl();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerControl pc) && pc.IsItMainPlayer)
        {
            cc.RotateCameraOnVector(new Vector3(Xshift, 0, 0), 0.3f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerControl pc) && pc.IsItMainPlayer)
        {
            cc.RotateCameraOnVector(new Vector3(-Xshift, 0, 0), 0.3f);
        }
    }
}
