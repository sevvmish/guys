using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintPatch : MonoBehaviour
{
    [SerializeField] private GameObject patch1;
    [SerializeField] private GameObject patch2;
    
    private Color color;
    private GameObject currentPatch;
    private float koeff = 0.5f;
    private float timer = 3f;
    private HashSet<ConditionControl> players = new HashSet<ConditionControl>();
    
    private void Start()
    {
        Setdata(Vector3.zero, 0.5f, 3f);
    }

    public void Setdata(Vector3 rot, float newKoeff, float newTimer)
    {        
        int rnd = UnityEngine.Random.Range(0, 2);
        koeff = newKoeff;
        timer = newTimer;

        switch (rnd)
        {
            case 0:
                patch1.SetActive(true);
                patch2.SetActive(false);
                currentPatch = patch1;
                color = Color.green;
                break;

            case 1:
                patch1.SetActive(false);
                patch2.SetActive(true);
                currentPatch = patch2;
                color = Color.blue;
                break;
        }

        transform.localEulerAngles = new Vector3 (0, UnityEngine.Random.Range(0, 270), 0);
        transform.eulerAngles = rot;

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out ConditionControl player) && !players.Contains(player))
        {
            players.Add(player);
            StartCoroutine(restartPlayer(player));
            player.MakePainted(koeff, timer, color);
        }
    }

    private IEnumerator restartPlayer(ConditionControl player)
    {
        yield return new WaitForSeconds(0.15f);
        players.Remove(player);
    }
}
