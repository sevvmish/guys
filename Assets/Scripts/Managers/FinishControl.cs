using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishControl : MonoBehaviour
{
    [SerializeField] private Transform[] places;
    [SerializeField] private GameObject[] placesVisualization;
    [SerializeField] private Transform[] otherPlaces;

    private bool isVisualsActive = true;

    private void Start()
    {
        setVisuals(false);
    }

    private void setVisuals(bool isActive)
    {
        if (isVisualsActive == isActive)
        {
            return;
        }
        isVisualsActive = isActive;

        if (placesVisualization.Length > 0)
        {
            for (int i = 0; i < placesVisualization.Length; i++)
            {
                if (placesVisualization[i].activeSelf != isActive) placesVisualization[i].SetActive(isActive);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerControl pc) && !pc.IsFinished)
        {
            GameManager.Instance.AddPlayerFinished(pc);
            //pc.FinishReached();
            StartCoroutine(checkPlace(pc));
        }
        else if (other.TryGetComponent(out RagdollPartCollisionChecker rag) && !rag.LinkToPlayerControl.gameObject.GetComponent<PlayerControl>().IsFinished)
        {
            PlayerControl plC = rag.LinkToPlayerControl.gameObject.GetComponent<PlayerControl>();
            //plC.FinishReached();
            GameManager.Instance.AddPlayerFinished(plC);
            StartCoroutine(checkPlace(plC));
        }
    }

    private IEnumerator checkPlace(PlayerControl player)
    {
        setVisuals(true);

        yield return new WaitForSeconds(0.1f);

        int place = GameManager.Instance.GetFinishPlace(player);
        player.transform.eulerAngles = new Vector3 (0, 180, 0);

        if (player.IsItMainPlayer)
        {
            GameManager.Instance.GetCameraControl().ResetCameraOnRespawn();
        }

        if (place > 0 && place <= 5)
        {
            player.transform.position = places[place - 1].position;
        }
        else
        {
            player.transform.position = Vector3.Lerp(otherPlaces[0].position, otherPlaces[1].position, UnityEngine.Random.Range(0f, 1f));
        }

        //yield return new WaitForSeconds(0.3f);
        player.FinishReached();
    }
}