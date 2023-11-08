using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotNavPoint : MonoBehaviour
{
    [SerializeField] private int index = 1;
    public int Index { get; private set; }
    public bool IsActive => gameObject.activeSelf;
    private NavPointSystem nps;

    // Start is called before the first frame update
    void Start()
    {
        Index = index;
        nps = NavPointSystem.Instance;
        nps.AddPoint(this);
    }

    public void SetNewIndex(int newIndex)
    {
        index = newIndex;
        Index = newIndex;
    }

}
