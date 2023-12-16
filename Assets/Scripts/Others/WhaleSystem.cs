using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleSystem : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private Transform tail1;
    [SerializeField] private Transform tail2;
    [SerializeField] private Transform tail3;
    [SerializeField] private Transform tail4;
    [SerializeField] private Transform tail5;
    [SerializeField] private Transform tail6;

    [SerializeField] private Transform RightWing1;
    [SerializeField] private Transform RightWing2;
    [SerializeField] private Transform RightWing3;

    [SerializeField] private Transform LeftWing1;
    [SerializeField] private Transform LeftWing2;
    [SerializeField] private Transform LeftWing3;

    [SerializeField] private Transform whale;
    [SerializeField] private Transform[] points;
    [SerializeField] private float speed = 0.4f;
    private float speedLookAt = 3f;

    private float tailTurnAngle = 3.5f;
    private float delta = 0.02f;

    [SerializeField] private AudioSource _audio;
    private float audioTimer, audioCooldown = 2f;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(playTail());
        StartCoroutine(playWing());
        StartCoroutine(playPoints());
    }

    private void Update()
    {
        whale.position += whale.forward * speed;

        if (audioTimer > audioCooldown)
        {
            audioTimer = 0;

            audioCooldown = UnityEngine.Random.Range(9, 12);

            _audio.Play();
        }
        else
        {
            audioTimer += Time.deltaTime;
        }
    }

    private IEnumerator playPoints()
    {
        yield return new WaitForSeconds(0);

        while (true)
        {
            for (int i = 0; i < points.Length; i++)
            {
                float angl = 0;

                while ((whale.position - points[i].position).magnitude > 10)
                {                    
                    angl += 0.002f;
                    //whale.rotation = Quaternion.LookRotation(points[i].position - whale.position);

                    whale.rotation = Quaternion.Lerp(whale.rotation, Quaternion.LookRotation(points[i].position - whale.position), angl);
                    
                    yield return new WaitForSeconds(Time.deltaTime);
                                        
                }
            }
        }        
    }

    private IEnumerator playWing()
    {
        yield return new WaitForSeconds(0);

        while (true)
        {
            for (float i = -tailTurnAngle; i < tailTurnAngle; i += 0.15f)
            {
                

                RightWing1.localEulerAngles = new Vector3(0, 0, -i * 2);
                RightWing2.localEulerAngles = new Vector3(0, 0, -i * 3f);
                RightWing3.localEulerAngles = new Vector3(0, 0, -i * 4f);

                LeftWing1.localEulerAngles = new Vector3(0, 0, i * 2);
                LeftWing2.localEulerAngles = new Vector3(0, 0, i * 3f);
                LeftWing3.localEulerAngles = new Vector3(0, 0, i * 4f);

                yield return new WaitForSeconds(delta);
            }

            for (float i = tailTurnAngle; i > -tailTurnAngle; i -= 0.15f)
            {
                

                RightWing1.localEulerAngles = new Vector3(0, 0, -i * 2);
                RightWing2.localEulerAngles = new Vector3(0, 0, -i * 3f);
                RightWing3.localEulerAngles = new Vector3(0, 0, -i * 4f);

                LeftWing1.localEulerAngles = new Vector3(0, 0, i * 2);
                LeftWing2.localEulerAngles = new Vector3(0, 0, i * 3f);
                LeftWing3.localEulerAngles = new Vector3(0, 0, i * 4f);

                yield return new WaitForSeconds(delta);
            }
        }


    }


    private IEnumerator playTail()
    {
        yield return new WaitForSeconds(0);

        while (true)
        {
            for (float i = -tailTurnAngle; i < tailTurnAngle; i += 0.05f)
            {
                tail1.localEulerAngles = new Vector3(i, 0, 0);
                tail2.localEulerAngles = new Vector3(i * 1.1f, 0, 0);
                tail3.localEulerAngles = new Vector3(i * 1.2f, 0, 0);
                tail4.localEulerAngles = new Vector3(i * 1.3f, 0, 0);
                tail5.localEulerAngles = new Vector3(i * 1.4f, 0, 0);
                tail6.localEulerAngles = new Vector3(i * 1.5f, 0, 0);


                yield return new WaitForSeconds(delta);
            }

            for (float i = tailTurnAngle; i > -tailTurnAngle; i -= 0.05f)
            {
                tail1.localEulerAngles = new Vector3(i, 0, 0);
                tail2.localEulerAngles = new Vector3(i * 1.1f, 0, 0);
                tail3.localEulerAngles = new Vector3(i * 1.2f, 0, 0);
                tail4.localEulerAngles = new Vector3(i * 1.3f, 0, 0);
                tail5.localEulerAngles = new Vector3(i * 1.4f, 0, 0);
                tail6.localEulerAngles = new Vector3(i * 1.5f, 0, 0);


                yield return new WaitForSeconds(delta);
            }
        }

      
    }
}
