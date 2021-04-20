using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using System.Runtime.ExceptionServices;

public class SecondLevel : MonoBehaviourPunCallbacks
{
    public Transform[] spawnPos;
    public PlayerScript playerAccess;
    PhotonView pv;
    float t;
    bool firstStart;

    // Start is called before the first frame update
    void Start()
    {
        firstStart = false;
        PhotonNetwork.AutomaticallySyncScene = true;
        pv = GetComponent<PhotonView>();

        
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            pv.RPC("StartGame", players[i], spawnPos[i].position, spawnPos[i].rotation);
        }
        //StartCoroutine(RemoveStartButton());
    }


    // Update is called once per frame
    void Update()
    {
        if(firstStart == false)
        {
            t += Time.deltaTime;
            if (t >= 3)
            {
                GameObject[] barriers = GameObject.FindGameObjectsWithTag("Barrier");
                for (int i = 0; i < barriers.Length; i++)
                {
                    barriers[i].SetActive(false);
                }
                firstStart = true;
            }
        }
    }

    IEnumerator RemoveStartButton()
    {
        yield return new WaitForEndOfFrame();
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Start");
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }
    }

    [PunRPC]
    void StartGame(Vector3 spawn,Quaternion rot)
    {
        PhotonNetwork.Instantiate("PlayerNetwork", spawn,rot);
    }
    [PunRPC]
    void RemoveLobbyPlayers()
    {
        GameObject[] goPlayers = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < goPlayers.Length; i++)
        {
            if (PhotonNetwork.IsMasterClient == true)
            {

            }
                Destroy(goPlayers[i].gameObject);
        }
    }

}
