using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectJob : MonoBehaviour
{
    System.Numerics.Vector3 systemVector3;
    Vector3 unityVector3;
    Vector3 changePositionVec;
    bool firstStart = false;

    private void Update()
    {
        ObjectMath();
    }

    private void ObjectMath()
    {
        if (JobManager.i.isMultithreading == true)
        {
            if(firstStart == false)
            {
                JobManager.i.AddObjectToJob(this);
                firstStart = true;
            }
            //transform.position += new Vector3(SFunctionToThread(), SFunctionToThread(), SFunctionToThread()) * Time.deltaTime;
            transform.position += changePositionVec * Time.deltaTime;
            transform.Rotate(unityVector3 * Time.deltaTime);
        }
        else
        {
            ComplexGarbage();
            //transform.position += new Vector3(SFunctionToThread(), SFunctionToThread(), SFunctionToThread()) * Time.deltaTime;
            transform.position += changePositionVec * Time.deltaTime;
            transform.Rotate(unityVector3 * Time.deltaTime);
        }
    }

    public void ComplexGarbage()
    {
        System.Random rNum = new System.Random();
        for (int i = 0; i < 100; i++)
        {
            systemVector3.X = rNum.Next(0, 10);
            systemVector3.Y = rNum.Next(0, 10);
            systemVector3.Z = rNum.Next(0, 10);
            systemVector3.Z = systemVector3.Z / rNum.Next(1, 10);
            unityVector3.x = systemVector3.X;
            unityVector3.y = systemVector3.Y;
            unityVector3.z = systemVector3.Z;
        }
        changePositionVec = new Vector3(SFunctionToThread(), SFunctionToThread(), SFunctionToThread());//* Time.deltaTime
    }

    public float SFunctionToThread()
    {
        System.Random rNum = new System.Random();
        float moveFloat = (float)Math.Sqrt(Math.Sqrt(Math.Sqrt(Math.Pow(Math.Sqrt(Math.Sqrt(rNum.Next(5000, 7500))), 5))));
        return moveFloat;

        //return 3;
    }

    public float SRandFunctionToThread()
    {
        System.Random rNum = new System.Random();
        float a = rNum.Next(1, 3);
        float b = rNum.Next(2, 5);
        float final = (float)Math.Sqrt(Math.Sqrt(Math.Pow(a, b)));

        return (float)Math.Pow(final, 20f);
        //return 2.75f;
    }

    public static float FunctionToThread()
    {
        System.Random rNum = new System.Random();
        float moveFloat = (float)Math.Sqrt(Math.Sqrt(Math.Sqrt(Math.Pow(Math.Sqrt(Math.Sqrt(rNum.Next(5000, 7500))), 5))));
        return moveFloat;

        //return 3;
    }

    public static float RandFunctionToThread()
    {
        System.Random rNum = new System.Random();
        float a = rNum.Next(1, 3);
        float b = rNum.Next(2, 5);
        float final = (float)Math.Sqrt(Math.Sqrt(Math.Pow(a, b)));

        return (float)Math.Pow(final, 20f);
        //return 2.75f;
    }

}
