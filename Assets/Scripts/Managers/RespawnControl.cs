using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnControl : MonoBehaviour
{
    public int CurrentRespawnIndex { get; private set; } = -1;

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
        SetNewRespawn(new RespawnData(playerControl.transform.position, playerControl.transform.eulerAngles), -1);
    }

    public void SetNewRespawn(RespawnData _data, int index)
    {
        position = _data.Position;
        rotation = _data.Rotation;
        lastTimeTamp = gm.GameSecondsPlayed;
        if (CurrentRespawnIndex < index) CurrentRespawnIndex = index;
    }

    public void Die()
    {
        if (!playerControl.IsDead)
        {
            playerControl.Respawn(position, rotation);

            if (playerControl.IsItMainPlayer)
            {
                gm.AddDeath();
            }
        }
            
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
