using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    [Tooltip("prefab for the player")]
    public GameObject playerPrefab;

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    void LoadArena()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("Tryin to load level but not da master. what do?");
        }

        Debug.LogFormat("Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel("RoomFor" + PhotonNetwork.CurrentRoom.PlayerCount);  //only call if the caller is the master client

    }

    #region Photon Callbacks

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogFormat("Player entered room {0}", newPlayer.NickName);

        if(PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("Player entered room is the masterclient {0}", newPlayer.IsMasterClient);

            LoadArena();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogFormat("Player left room {0}", otherPlayer.NickName);

        if(PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("Player left is the master client {0}", PhotonNetwork.IsMasterClient);

            LoadArena();
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        if(playerPrefab == null)
        {
            Debug.LogError("missing playerprefeab reference ", this);
        }
        else
        {
            if(PlayerManager.localPlayerPrefab == null)
            {
                Debug.LogFormat("We are instantiating Localplayer from {0}", SceneManagerHelper.ActiveSceneName);

                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
