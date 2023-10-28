using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotNavPoint : MonoBehaviour
{
    private NavPointSystem nps;

    // Start is called before the first frame update
    void Start()
    {
        nps = NavPointSystem.Instance;
        nps.AddPoint(this);
    }

}
