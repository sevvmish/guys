using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkByMesh : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] meshes;
    [SerializeField] private float[] firstDelays;
    [SerializeField] private float howLongStay = 0.7f;
    [SerializeField] private float howLongDelay = 0.7f;    

    private float[] _timers;
    private float[] _show;

    private void Start()
    {
        _timers = new float[meshes.Length];
        _show = new float[meshes.Length];

        for (int i = 0; i < meshes.Length; i++)
        {
            meshes[i].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < meshes.Length; i++)
        {
            if (firstDelays[i] > 0)
            {
                firstDelays[i] -= Time.deltaTime;
                continue;
            }

            if (_timers[i] > howLongDelay)
            {
                if (!meshes[i].enabled) meshes[i].enabled = true;

                if (_show[i] > howLongStay)
                {
                    _show[i] = 0;
                    _timers[i] = 0;
                    if (meshes[i].enabled) meshes[i].enabled = false;

                    //
                }
                else
                {                    
                    _show[i] += Time.deltaTime;
                }
            }
            else
            {                
                _timers[i] += Time.deltaTime;
            }
        }


    }
}
