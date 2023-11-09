using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPoint : MonoBehaviour
{
    private PlayerControl pc;
    private RaycastHit hit;
    private LayerMask ignoreMask;
    private bool isChanged;
    private GameObject shadow;

    public void SetData(PlayerControl pcontrol)
    {
        pc = pcontrol;
        ignoreMask = LayerMask.GetMask(new string[] { "trigger", "player", "ragdoll" });
        shadow = transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        if (!pc.IsGrounded && pc.IsCanAct && !pc.IsRagdollActive && Physics.Raycast(pc.transform.position + Vector3.down * 0.1f, Vector3.down, out hit, Globals.SHADOW_Y_DISTANCE, ~ignoreMask))
        {
            isChanged = true;
            shadow.transform.localPosition = new Vector3(0, hit.point.y - pc.transform.position.y, 0);
            if (!shadow.activeSelf) shadow.SetActive(true);
        }
        else if (!pc.IsGrounded && pc.IsCanAct && !pc.IsRagdollActive)
        {
            if (shadow.activeSelf) shadow.SetActive(false);
        }
        else if (pc.IsGrounded && isChanged)
        {
            if (!shadow.activeSelf) shadow.SetActive(true);
            isChanged = false;
            shadow.transform.localPosition = Vector3.zero;
        }        
    }
}
