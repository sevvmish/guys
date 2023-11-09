using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPoint : MonoBehaviour
{
    private PlayerControl pc;
    private RaycastHit hit;
    private LayerMask ignoreMask;
    private bool isChanged;

    public void SetData(PlayerControl pcontrol)
    {
        pc = pcontrol;
        ignoreMask = LayerMask.GetMask(new string[] { "trigger", "player", "ragdoll" });
    }

    private void Update()
    {        
        if (!pc.IsGrounded && pc.IsCanAct && !pc.IsRagdollActive && Physics.Raycast(pc.transform.position + Vector3.down*0.1f, Vector3.down, out hit, 5, ~ignoreMask)) 
        {            
            isChanged = true;
            transform.localPosition = new Vector3(0, hit.point.y - pc.transform.position.y + 0.05f, 0);
        }
        else if (pc.IsGrounded && isChanged)
        {
            isChanged = false;
            transform.localPosition = new Vector3(0, 0.05f, 0);
        }
        
    }
}
