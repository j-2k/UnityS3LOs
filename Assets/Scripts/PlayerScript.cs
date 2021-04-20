using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviourPunCallbacks
{
    public float Speed = 10f;
    public float hp = 100;
    public Image hpBar;
    public Image hpBar_Parent;
    public Transform shotPos;
    public GameObject bulletPrefab;
    public Text playerName;
    CharacterController cc;

    public Image MY_hpBar;
    public Image MY_hpBar_Parent;
    public Text MY_playerName;

    public Joystick joystick;

    public GameObject phoneCanvas;
    public GameObject startButton;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        PhotonNetwork.AutomaticallySyncScene = true;
        //DontDestroyOnLoad(this.gameObject);
        if (photonView.IsMine)
        {
            playerName.text = PhotonNetwork.NickName;
            MY_playerName.text = PhotonNetwork.NickName;
            
            hpBar.enabled = false;
            hpBar_Parent.enabled = false;
            playerName.enabled = false;
            
            transform.Find("PlayerCamera").gameObject.SetActive(true);
            GetComponent<MeshRenderer>().material.color = Color.blue;
            #if UNITY_ANDROID
            //PHONE CODE HERE
            phoneCanvas.SetActive(true);
            #else
            //PC CODE HERE
            phoneCanvas.SetActive(false);
            #endif
        }
        else
        {
            startButton.SetActive(false);

            phoneCanvas.SetActive(false);
            MY_playerName.enabled = false;
            MY_hpBar_Parent.enabled = false;
            MY_hpBar.enabled = false;
            
            playerName.enabled = true;
            hpBar.enabled = true;
            hpBar_Parent.enabled = true;

            transform.Find("PlayerCamera").gameObject.SetActive(false);
            playerName.text = photonView.Owner.NickName;
            GetComponent<MeshRenderer>().material.color = Color.red;
        }

        if (PhotonNetwork.IsMasterClient == true)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
    }

    private void Update()
    {
        hpBar.fillAmount = hp / 100;
        MY_hpBar.fillAmount = hp / 100;

        if (photonView.IsMine)
        {
            #if UNITY_ANDROID
            //PHONE CODE HERE
            InputMovementMobile();

#else
            //PC CODE HERE
            InputMovementPC();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Fire();
            }
#endif

            if (hp <= 0)
            {
                PhotonNetwork.Destroy(gameObject);
                Debug.Log(photonView.Owner.NickName + " Has died!");
            }
        }
    }

    [PunRPC]
    void RemoveLobbyPlayers()
    {
        GameObject[] goPlayers = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < goPlayers.Length; i++)
        {
            Destroy(goPlayers[i].gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyBullet")
        {
            ChangeHealth(-10);
        }
    }

    void ChangeHealth(float value)
    {
        hp += value;
    }

    // used as Observed component in a PhotonView, this only reads/writes the position
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            Vector3 pos = transform.localPosition;
            stream.Serialize(ref pos);
            stream.SendNext(hp);
            stream.SendNext(playerName.text);
        }
        else
        {
            Vector3 pos = Vector3.zero;
            stream.Serialize(ref pos);  // pos gets filled-in. must be used somewhere
            hp = (float)stream.ReceiveNext();
            playerName.text = (string)stream.ReceiveNext();
        }
    }

    public void Fire()
    {
        PhotonNetwork.Instantiate(bulletPrefab.name, shotPos.transform.position, shotPos.rotation);
    }

    void InputMovementPC()
    {
        float h = Input.GetAxis("Horizontal") * Time.deltaTime * 100;
        float v = Input.GetAxis("Vertical") * Time.deltaTime * Speed;

        cc.Move(transform.forward * v);
        cc.transform.Rotate(0, h, 0);
    }

    void InputMovementMobile()
    {
        Debug.Log("mobile Test moveing true");
        float h = joystick.Horizontal * Time.deltaTime * 100;
        float v = joystick.Vertical * Time.deltaTime * Speed;

        cc.Move(transform.forward * v);
        cc.transform.Rotate(0, h, 0);
    }

    public void StartGameSecondLevel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("RemoveLobbyPlayers", RpcTarget.All);
            PhotonNetwork.LoadLevel("Second");
        }
    }
}
