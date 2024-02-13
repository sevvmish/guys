using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeactivateOnButtonClick : MonoBehaviour
{
    [SerializeField] private GameObject[] toDeactivate;
    private Button button;


    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(() => 
        {
            button.interactable = false;

            if (toDeactivate.Length > 0)
            {
                for (int i = 0; i < toDeactivate.Length; i++)
                {
                    toDeactivate[i].SetActive(false);
                }
            }
        });
    }

    private void OnEnable()
    {
        StartCoroutine(play());
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.3f);
    }

    private IEnumerator play()
    {
        SoundUI.Instance.PlayUISound(SoundsUI.success2);
        yield return new WaitForSeconds(5);
        SoundUI.Instance.PlayUISound(SoundsUI.pop);
        if (toDeactivate.Length > 0)
        {
            for (int i = 0; i < toDeactivate.Length; i++)
            {
                toDeactivate[i].SetActive(false);
            }
        }
    }

}
