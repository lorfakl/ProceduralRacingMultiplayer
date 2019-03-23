using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [Tooltip("Beams GameObject to control")]
    [SerializeField]
    private GameObject beams;

    [Tooltip("The current Health of our player")]
    public float health = 1f;

    [Tooltip("local prefab for the player")]
    public static GameObject localPlayerPrefab;

    [Tooltip("The Player's UI Gameobject")]
    public GameObject playerUIPrefab;


    bool isFiring;

    private void Awake()
    {
        if(beams == null)
        {
            Debug.LogError("Missing Beams reference", this);

        }
        else
        {
            beams.SetActive(false);
        }

        if(photonView.IsMine)
        {
            PlayerManager.localPlayerPrefab = this.gameObject;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

        
        Camerawork cameraWork = this.gameObject.GetComponent<Camerawork>();

        if (cameraWork != null)
        {
            if(photonView.IsMine)
            {
                cameraWork.OnStartFollowing();
            }
        }
        else
        {
            Debug.LogError("Missing camerawork component", this);
        }

        if(playerUIPrefab != null)
        {
            /*Debug.Log("Ui instaniated in start");
            GameObject ui = PhotonNetwork.Instantiate(playerUIPrefab.name, Vector3.zero, Quaternion.identity);
            ui.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);*/
        }
        else
        {
            Debug.LogWarning("Missing playerUI prefab reference", this);
        }

        #if UNITY_5_4_OR_NEWER
            SceneManager.sceneLoaded += (scene, loadingMode) => { this.CalledOnLevelWasLoaded(scene.buildIndex); };
        #endif


    }


    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        {
            ProcessInputs();
        }

        if (beams != null && isFiring != beams.activeSelf)
        {
            beams.SetActive(isFiring);
        }

        if(health <= 0f)
        {
            GameManager.instance.LeaveRoom();
        }
    }

    #if !UNITY_5_4_OR_NEWER
    void OnLevelWasLoaded(int level)
    {
        this.CalledOnLevelWasLoaded(level);
    }
    #endif

    void CalledOnLevelWasLoaded(int level)
    {
        if(!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 5f, 0f);
        }
        Debug.Log("UI instaniated on scene change");
        GameObject ui = PhotonNetwork.Instantiate(playerUIPrefab.name, Vector3.zero, Quaternion.identity);
        ui.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!photonView.IsMine)
        {
            return;
        }

        if(!other.name.Contains("leftBeam"))
        {
            return;
        }
        Debug.Log("this being triggered?");
        health -= 0.1f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (!other.name.Contains("beam"))
        {
            return;
        }

        health -= 0.1f * Time.deltaTime;
    }

    void ProcessInputs()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if(!isFiring)
            {
                isFiring = true;
            }
        }

        if(Input.GetButtonUp("Fire1"))
        {
            if(isFiring)
            {
                isFiring = false;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(isFiring);
            stream.SendNext(health);
        }
        else
        {
            this.isFiring = (bool)stream.ReceiveNext();
            this.health = (float)stream.ReceiveNext();
        }
    }
}
