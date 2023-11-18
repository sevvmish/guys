using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotNavPoint : MonoBehaviour
{
    [SerializeField] private int index = 1;
    public int Index { get; private set; }
    public bool IsActive;
    private NavPointSystem nps;

    // Start is called before the first frame update
    void Start()
    {
        Index = index;
        nps = NavPointSystem.Instance;
        nps.AddPoint(this);
        IsActive = gameObject.activeSelf;
    }

    public void SetNewIndex(int newIndex)
    {
        index = newIndex;
        Index = newIndex;
    }

}
