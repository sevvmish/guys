using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Level3Additional : MonoBehaviour
{
    [SerializeField] private GameObject cellExample;
    [SerializeField] private GameObject turnOffOnStart;

    [SerializeField] private Transform line1;
    [SerializeField] private Transform line2;
    [SerializeField] private Transform line3;
    [SerializeField] private Transform line4;
    [SerializeField] private Transform line5;

    private float xSize = 3.4f;
    private float xDeltaSplit = 1.7f;
    private float zSize = 1.88f;

    private Vector2 rowSize = new Vector2(8, 22);

    private bool isStart;

    // Start is called before the first frame update
    void Start()
    {
        cellExample.SetActive(false);
        AddRow(line1);
        AddRow(line2);
        AddRow(line3);
        AddRow(line4);
        //AddRow(line5);
    }

    private void AddRow(Transform line)
    {
        float deltaX = 0;
        float deltaZ = 0;
        float addXDelta = 0;
        GameObject g = default;
        bool isOne = false;

        for (int zAmount = 0; zAmount < rowSize.y; zAmount++)
        {
            if (!isOne)
            {
                isOne = true;
                addXDelta = 0;
            }
            else
            {
                isOne = false;
                addXDelta = xDeltaSplit;
            }

            for (int xAmount = 0; xAmount < rowSize.x; xAmount++)
            {
                g = Instantiate(cellExample, new Vector3(-rowSize.x / 2 * xSize + xSize / 3 + deltaX + addXDelta, line.position.y, -rowSize.y / 2 + deltaZ + zSize), Quaternion.identity, line);
                g.SetActive(true);
                deltaX += xSize;
            }
            deltaX = 0;
            deltaZ += zSize / 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStart && GameManager.Instance.IsGameStarted)
        {
            isStart = true;
            turnOffOnStart.SetActive(false);
        }
    }
}
