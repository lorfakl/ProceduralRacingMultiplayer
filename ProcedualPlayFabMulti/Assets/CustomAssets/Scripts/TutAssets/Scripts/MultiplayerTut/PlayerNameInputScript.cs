using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(InputField))]

public class PlayerNameInputScript : MonoBehaviour
{
    private const string playerNamePrefKey = "PlayerName";

    private void Awake()
    {
        this.GetComponent<InputField>().onValueChanged.AddListener(SetPlayerName);
    }

    void Start()
    {
        string defaultName = string.Empty;
        InputField playerNameField = this.GetComponent<InputField>();

        if(playerNameField != null)
        {
            if(PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                playerNameField.text = defaultName;
            }
        }

        PhotonNetwork.NickName = defaultName;
    }

    public void SetPlayerName(string val)
    {
        if(string.IsNullOrEmpty(val))
        {
            Debug.LogError("Player name is null");
            return;
        }

        PhotonNetwork.NickName = val;

        PlayerPrefs.SetString(playerNamePrefKey, val);
    }
}
