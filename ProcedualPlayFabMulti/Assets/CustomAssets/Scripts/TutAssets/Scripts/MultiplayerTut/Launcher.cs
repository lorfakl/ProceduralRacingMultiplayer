using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayers = 4;

    [Tooltip("UI to let the user enter a display name and connect")]
    [SerializeField]
    private GameObject controlPanel;

    [Tooltip("UI Text to inform the user that a connection is in progress")]
    [SerializeField]
    private GameObject progressLabel;

    private bool isConnecting;
    #endregion

    #region Private Fields
    string gameVersion = "1";
    #endregion

    #region Monobehaviour CallBacks

    void Awake()
    {
        //allows the master client to use PhotonNetwork.Loadlevel which automatically syncs and loads the level for
        //other clients in the room
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Connect();
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        if(isConnecting)
        {
            print("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            PhotonNetwork.JoinRandomRoom();
        }
        

    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayers});
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Connected to a room");
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("Ding ding room for 1");

            PhotonNetwork.LoadLevel("RoomFor1");
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Didnt join a room buddy. Dont know why");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);

    }
    #endregion

    public void Connect()
    {
        isConnecting = true;
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}
