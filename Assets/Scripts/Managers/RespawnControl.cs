using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnControl : MonoBehaviour
{    
    private PlayerControl playerControl;
    private Vector3 position;
    private Vector3 rotation;


    // Start is called before the first frame update
    void Start()
    {        
        playerControl = GetComponent<PlayerControl>();
        SetNewRespawn(new RespawnData(playerControl.transform.position, playerControl.transform.eulerAngles), -1);
        
    }

    public void SetNewRespawn(RespawnData _data, int index)
    {
        position = _data.Position;
        rotation = _data.Rotation;
    }

    public void Die()
    {
        if (playerControl.IsDead) return;

        playerControl.Respawn(position, rotation);

    }

    public struct RespawnData
    {
        public Vector3 Position;
        public Vector3 Rotation;

        public RespawnData(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}
