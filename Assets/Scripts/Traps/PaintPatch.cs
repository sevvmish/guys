using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintPatch : MonoBehaviour, Explosives
{
    [SerializeField] private GameObject patch1;
    [SerializeField] private GameObject patch2;
    [SerializeField] private GameObject patch3;
    [SerializeField] private GameObject destroyedVFX;
    [SerializeField] private float duration = -1;

    private Color color;
    private GameObject currentPatch;
    private float koeff = 0.5f;
    private bool isActive;
    private float timer = 3f;
    private HashSet<ConditionControl> players = new HashSet<ConditionControl>();
 
    private void OnEnable()
    {
        int rnd = UnityEngine.Random.Range(0, 3);
      
        switch (rnd)
        {
            case 0:
                patch1.SetActive(true);
                patch2.SetActive(false);
                patch3.SetActive(false);
                currentPatch = patch1;
                color = Color.green;
                break;

            case 1:
                patch1.SetActive(false);
                patch2.SetActive(true);
                patch3.SetActive(false);
                currentPatch = patch2;
                color = Color.blue;
                break;

            case 2:
                patch1.SetActive(false);
                patch2.SetActive(false);
                patch3.SetActive(true);
                currentPatch = patch3;
                color = Color.magenta;
                break;
        }

        currentPatch.transform.position += Vector3.up * UnityEngine.Random.Range(0.001f, 0.009f);

        if (duration > 0)
        {
            StartCoroutine(playDuration(duration));
        }

        isActive = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (isActive && other.TryGetComponent(out ConditionControl player) && !player.HasCondition(Conditions.painted)/*&& !players.Contains(player)*/)
        {
            //players.Add(player);
            //StartCoroutine(restartPlayer(player));
            player.MakePainted(koeff, timer, color);
        }
    }

    private IEnumerator playDuration(float patchDuration)
    {        
        yield return new WaitForSeconds(patchDuration);
        currentPatch.transform.DOScale(Vector3.zero * 0.01f, 0.2f);
        
        yield return new WaitForSeconds(0.2f);

        isActive = false;
        destroyedVFX.SetActive(true);

        yield return new WaitForSeconds(1f);
        currentPatch.transform.localScale = Vector3.one * 0.4f;
        gameObject.SetActive(false);
    }

    private IEnumerator restartPlayer(ConditionControl player)
    {
        yield return new WaitForSeconds(0.15f);
        players.Remove(player);
    }

    public void SetTTL(float seconds)
    {
        duration = seconds;
    }
}
