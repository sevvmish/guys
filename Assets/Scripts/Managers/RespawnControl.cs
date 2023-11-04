using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnControl : MonoBehaviour
{
    private GameManager gm;
    private PlayerControl playerControl;
    private Vector3 position;
    private Vector3 rotation;
    private float _timer;
    private float lastTimeTamp;
    private List<RespawnData> data = new List<RespawnData>();

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        playerControl = GetComponent<PlayerControl>();
        SetNewRespawn(new RespawnData(playerControl.transform.position, playerControl.transform.eulerAngles));
    }

    /*

    private void Update()
    {
        if (_timer > 0.2f)
        {
            _timer = 0;
            if (playerControl.IsGrounded && playerControl.CurrentActivePlatform == null && !playerControl.IsDead && playerControl.DangerZone == null)
            {
                data.Add(new RespawnData(playerControl.transform.position, playerControl.transform.eulerAngles));                
            }

            if (data.Count == 3)
            {
                SetNewRespawn(data[0]);
                data.Clear();
            }
        }
        else
        {
            _timer += Time.deltaTime;
        }

        
    }*/


    private void SetNewRespawn(RespawnData _data)
    {
        position = _data.Position;
        rotation = _data.Rotation;
        lastTimeTamp = gm.GameSecondsPlayed;
    }

    public void Die()
    {
        if (!playerControl.IsDead) playerControl.Respawn(position, rotation);
    }

    private struct RespawnData
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
