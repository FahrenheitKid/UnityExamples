using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : Enemy
{

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        speed = 5;
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

}
