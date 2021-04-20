using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Comparing : MonoBehaviour
{
    public Vector3 rot;
    public Vector3 pos;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        rot = new Vector3(RandFunctionToThread(), RandFunctionToThread(), RandFunctionToThread());
        pos = new Vector3(FunctionToThread(), FunctionToThread(), FunctionToThread());
        transform.position += pos * Time.deltaTime;
        transform.Rotate(rot * Time.deltaTime);
    }


    static float FunctionToThread()
    {
        System.Random rNum = new System.Random();
        float moveFloat = (float)Math.Sqrt(Math.Sqrt(Math.Sqrt(Math.Pow(Math.Sqrt(Math.Sqrt(rNum.Next(5000, 7500))), 5))));
        return moveFloat;

        //return 3;
    }

    static float RandFunctionToThread()
    {
        System.Random rNum = new System.Random();
        float a = rNum.Next(1, 3);
        float b = rNum.Next(2, 5);
        float final = (float)Math.Sqrt(Math.Sqrt(Math.Pow(a, b)));

        return (float)Math.Pow(final, 20f);
        //return 2.75f;
    }
}
