using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBetweenPoints : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private Transform main;
    [SerializeField] private float speedPerM = 1f;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        if (points.Length == 0 || main == null) gameObject.SetActive(false);
        yield return new WaitForSeconds(0);

        main.transform.position = points[points.Length - 1].position;

        while (true)
        {
            for (int i = 0; i < points.Length; i++)
            {
                main.transform.DOLookAt(points[i].position, 2).SetEase(Ease.Linear);
                float _time = (main.transform.position - points[i].position).magnitude / speedPerM;
                main.transform.DOMove(points[i].position, _time).SetEase(Ease.Linear);
                yield return new WaitForSeconds(_time);
            }
        }
        
    }

    
}
