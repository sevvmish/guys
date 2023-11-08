using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class testAI : MonoBehaviour
{
    [SerializeField] private NavMeshSurface surface;
    NavMeshData navMeshData;

    // Start is called before the first frame update
    void Start()
    {
        surface.BuildNavMesh();
        navMeshData = surface.navMeshData;
    }

    // Update is called once per frame
    void Update()
    {
        //surface.UpdateNavMesh(navMeshData);
    }
}
