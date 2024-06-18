using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeactivateAllExceptView : MonoBehaviour
{
    [SerializeField] private GameObject[] all;
    [SerializeField] private int howManyVisible = 1;



    private void OnEnable()
    {
        if (all.Length == 0)
        {
            return;
        }
        else if (all.Length == 1)
        {
            all[0].SetActive(true);
        }
        else
        {
            all.ToList().ForEach(x => x.SetActive(false));
            int visible = 0;

            for (int i = 0; i < 1000; i++)
            {
                if (visible >= howManyVisible) break;
                int rnd = UnityEngine.Random.Range(0, all.Length);

                if (!all[rnd].activeSelf)
                {
                    visible++;
                    all[rnd].SetActive(true);
                }

            }

        }
    }
}
