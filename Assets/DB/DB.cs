using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[System.Serializable]
public class UserData
{
    public DeserializeUserInformation[] arrayResult;
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
    public SerializeGameItems[] arrayResult;
}

[System.Serializable]
public class SerializeGameItems
{
    public string username;
    public GameItems gameItems;
}

[System.Serializable]
public class GameItems
{
    public bool[] gunArray = new bool[3];
    public bool[] cardArray = new bool[3];
    public int[] enemyArray = new int[3];
}


public class DB : MonoBehaviour
{
    //usersInfo
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

        //GetUserGameInfoFromUsername(getReqIG,"juma");
        //SetUserGameInfoFromUsername(setReqIG, "juma",0);
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
        SerializeUserInformation userInfo = new SerializeUserInformation();
        userInfo.username = usernameString;
        StartCoroutine(GetPostRequestIG(url, JsonUtility.ToJson(userInfo)));
    }

    void SetUserGameInfoFromUsername(string url, string usernameString, int arrayIndex) //3rd param is the array to set
    {
        GameItems playerItems = new GameItems();
        playerItems.gunArray[arrayIndex] = true;
        playerItems.cardArray[arrayIndex] = true;
        playerItems.enemyArray[arrayIndex] = 423453245;
        SerializeGameItems gameItemsInfo = new SerializeGameItems();
        gameItemsInfo.username = usernameString;
        gameItemsInfo.gameItems = playerItems;
        StartCoroutine(SetPostRequestIG(url, JsonUtility.ToJson(gameItemsInfo)));
        /*
        GameItems newGItems = new GameItems();
        gameItemsInfo.gameItems = newGItems;
        newGItems.gunArray[arrayIndex] = false;
        newGItems.cardArray[arrayIndex] = false;
        newGItems.enemyArray[arrayIndex] = 999;
        */



        /*
        gameItemsInfo.gameItems.gunArray[arrayIndex] = false;
        gameItemsInfo.gameItems.cardArray[arrayIndex] = false;
        gameItemsInfo.gameItems.enemyArray[arrayIndex] = 999;
        /*
        GameItems gameItemsNew = new GameItems();
        gameItemsNew.gunArray[arrayIndex] = false;
        gameItemsNew.cardArray[arrayIndex] = false;
        gameItemsNew.enemyArray[arrayIndex] = 999;
        /*

        /*
        GameItems gItems = new GameItems();
        gItems.gunArray[arrayIndex] = false;
        gItems.cardArray[arrayIndex] = false;
        gItems.enemyArray[arrayIndex] = 999;
        */
        //StartCoroutine(SetPostRequestIG(url, JsonUtility.ToJson(gameItemsNew)));
    }

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
            Debug.Log(t.arrayResult[0].emailID + " " + t.arrayResult[0].username + " " + t.arrayResult[0].userPassword);
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
            IngameData t = JsonUtility.FromJson<IngameData>(uwr.downloadHandler.text);
            Debug.Log(t.arrayResult[0].gameItems.cardArray[0] + " " + t.arrayResult[0].gameItems.gunArray[0] + " " + t.arrayResult[0].gameItems.enemyArray[0]);
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
