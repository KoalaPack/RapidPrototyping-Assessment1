using System.Collections.Generic;
using UnityEngine;

public class PlayGround : MonoBehaviour
{
    public GameObject cube;
    public List<string> names;
    public BV.Range spawnRate;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            //cube.gameObject.GetComponent<Renderer>().material.color = ColorX.GetRandomColor;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            ListX.ShuffleList(names);
            Debug.Log(names[0]);
        }
    }
}
