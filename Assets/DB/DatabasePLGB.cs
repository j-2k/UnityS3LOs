using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.Networking;



[System.Serializable]

public struct UserData //We make serialized struct that we will be taking the users data from
{

    public string username;

    public int password;

    public UserData(string username, int password) //constructer for username & pass
    {

        this.username = username;

        this.password = password;

    }

}



public class DatabasePLGB : MonoBehaviour

{

    UserData data;

    // Start is called before the first frame update

    void Start()
    {

        StartCoroutine(GetDBInfo("juma")); 	//we have to use a coroutine to get this to work since the server will take time to run 						through fully & a coroutine waits for the server which is exactly what we need. 

    }

    IEnumerator GetDBInfo(string username)		//This is our Get couroutine function
    {
        UnityWebRequest www = UnityWebRequest.Get($"https://plgb-db.herokuapp.com/exercises/get"); //Same as before here is our get request the IP being local host & the port is the port we entered in our .env_dev file which is 4000 & then access DBInfo which is the javascript file & get the username specifically through the web 

        yield return www.SendWebRequest();	//Waiting part of the coroutine where we send the request & wait for the feedback

        if (www.isNetworkError || www.isHttpError)		//LOG ERRORS
        {
            Debug.Log(www.error);
        }
        else							//LOG THE INFORMATION WE GOT FROM THE DATABASE
        {
            Debug.Log("forum uploaded! (GET)");
            data = JsonUtility.FromJson<UserData>(www.downloadHandler.text);
            Debug.Log(data.username);
            Debug.Log(data.password);
        }
    }

    IEnumerator SetDBInfo(UserData userData)    //Here is our couroutine function to set database info.
    //THIS WAS DONE FOR TESTING WE MOSTLY USE THE GET FUNCTION WHICH IS BELOW SINCE WE WANT 						TO GET INFORMATION FROM THE DB
    {

        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:4000", JsonUtility.ToJson(userData));	//We send a post request to the IP & Port in our case is IP = localhost & port = 4000 which is what we entered in our .env_dev file in javascript.
        //UnityWebRequest www = UnityWebRequest.Get("http://127.0.0.1:4000", JsonUtility.ToJson(userData));


        yield return www.SendWebRequest();	//here is the "waiting" part of the coroutine where we send a request to the IP & port 						we entered above.



        if (www.isNetworkError || www.isHttpError)	//This is for logging errors we might encounter with our program.
        {

            Debug.Log(www.error);

        }
        else						//If the data goes through we say forum uploaded set because this is part of 							the set function this is just to make sure we know what ran.
        {

            Debug.Log("forum uploaded! (SET)");

        }

    }

}