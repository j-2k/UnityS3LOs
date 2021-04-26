using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[System.Serializable]
public class UserData
{
    public DeserializeUserInformation arrayResult;
}

[System.Serializable]
public class SerializeUserInformation
{
    public string username;
    public int userPassword;
}

[System.Serializable]
public class DeserializeUserInformation
{
    public string username;
    public string emailID;
    public int userPassword;
}

[System.Serializable]
public class IngameData
{
    public SerializeGameItems arrayResult;
}

[System.Serializable]
public class SerializeGameItems
{
    public string username;
    public bool[] gunArray = new bool[5];
    public bool[] cardArray = new bool[5];
    public int[] enemyArray = new int[5];
}

/*
[System.Serializable]
public class SerializeGameItems
{
    public string username;
    public GameItems gameItems;
}

[System.Serializable]
public class GameItems
{
    public bool[] gunArray = new bool[5];
    public bool[] cardArray = new bool[5];
    public int[] enemyArray = new int[5];
}
*/


public class DB : MonoBehaviour
{
    //usersInfo
    string addReq = "http://localhost:3000/users/add";
    string setReq = "http://localhost:3000/users/set";
    string getReq = "http://localhost:3000/users/get";
    string herokuSetReq = "https://plgb-db.herokuapp.com/users/set";
    string herokuGetReq = "https://plgb-db.herokuapp.com/users/get";

    //ingameInfo
    string setReqIG = "http://localhost:3000/exercises/set";
    string getReqIG = "http://localhost:3000/exercises/get";
    string herokuSetReqIG = "https://plgb-db.herokuapp.com/exercises/set";
    string herokuGetReqIG = "https://plgb-db.herokuapp.com/exercises/get";

    // Start is called before the first frame update
    void Start()
    {
        //GetUserInformationFromUsername(herokuGetReq,"juma");
        //SetUserInformationFromUsername(herokuSetReq, "juma",8057);

        GetUserGameInfoFromUsername(getReqIG, "ziad");
        //SetUserGameInfoFromUsername(herokuSetReqIG, "juma",0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GetUserInformationFromUsername(string url,string usernameString)
    {
        SerializeUserInformation userInfo = new SerializeUserInformation();
        userInfo.username = usernameString;
        StartCoroutine(GetPostRequestUser(url, JsonUtility.ToJson(userInfo)));
    }

    void SetUserInformationFromUsername(string url,string usernameString, int password)
    {
        SerializeUserInformation userInfo = new SerializeUserInformation();
        userInfo.username = usernameString;
        userInfo.userPassword = password;
        StartCoroutine(SetPostRequestUser(url, JsonUtility.ToJson(userInfo)));
    }

    void GetUserGameInfoFromUsername(string url, string usernameString)
    {
        SerializeGameItems userInfo = new SerializeGameItems();
        userInfo.username = usernameString;
        StartCoroutine(GetPostRequestIG(url, JsonUtility.ToJson(userInfo)));
    }

    void SetUserGameInfoFromUsername(string url, string usernameString, int arrayIndex) //3rd param is the array to set
    {
        SerializeGameItems gameItemsInfo = new SerializeGameItems();
        gameItemsInfo.username = usernameString;
        gameItemsInfo.gunArray[arrayIndex] = true;
        gameItemsInfo.cardArray[arrayIndex] = false;
        gameItemsInfo.enemyArray[arrayIndex] = 999;
        StartCoroutine(SetPostRequestIG(url, JsonUtility.ToJson(gameItemsInfo)));
    }

    /*void SetUserGameInfoFromUsername(string url, string usernameString, int arrayIndex) //3rd param is the array to set
    {
        GameItems playerItems = new GameItems();
        playerItems.gunArray[arrayIndex] = true;
        playerItems.cardArray[arrayIndex] = true;
        playerItems.enemyArray[arrayIndex] = 98986435;
        SerializeGameItems gameItemsInfo = new SerializeGameItems();
        gameItemsInfo.username = usernameString;
        gameItemsInfo.gameItems = playerItems;
        StartCoroutine(SetPostRequestIG(url, JsonUtility.ToJson(gameItemsInfo)));
    }*/

    IEnumerator GetPostRequestUser(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            UserData t = JsonUtility.FromJson<UserData>(uwr.downloadHandler.text);
            Debug.Log(t.arrayResult.emailID + " " + t.arrayResult.username + " " + t.arrayResult.userPassword);
        }
    }

    IEnumerator SetPostRequestUser(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }

    IEnumerator GetPostRequestIG(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            //IngameData t = JsonUtility.FromJson<IngameData>(uwr.downloadHandler.text);
            //Debug.Log(t.arrayResult.gameItems.cardArray[0] + " " + t.arrayResult.gameItems.gunArray[0] + " " + t.arrayResult.gameItems.enemyArray[0]);
            SerializeGameItems t = JsonUtility.FromJson<SerializeGameItems>(uwr.downloadHandler.text);
            Debug.Log(t.cardArray[0] + " " + t.gunArray[0] + " " + t.enemyArray[0]);
        }
    }

    IEnumerator SetPostRequestIG(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }
}
