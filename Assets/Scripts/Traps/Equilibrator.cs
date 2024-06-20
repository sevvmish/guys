using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equilibrator : MonoBehaviour
{
    private HashSet<PlayerControl> players = new HashSet<PlayerControl>();
    private Transform _transform;
    private float _timer;
    private bool isDotween;

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
    }

    private void FixedUpdate()
    {
        if (_timer > 0.2f)
        {
            _timer = 0;
            float delta = Mathf.Abs(_transform.eulerAngles.z);
            if (players.Count == 0 && delta > 3 && !isDotween)
            {
                float timer = delta * 0.005f;

                isDotween = true;
                _transform.DORotate(Vector3.zero, timer).SetUpdate(UpdateType.Fixed).SetEase(Ease.Linear);
            }
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerControl p) && !players.Contains(p))
        {
            players.Add(p);
            isDotween = false;
            _transform.DOKill();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerControl p) && players.Contains(p))
        {
            players.Remove(p);
        }
    }
}
