using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerUIScript : MonoBehaviour
{
    [Tooltip("UI Test for the player's Name")]
    [SerializeField]
    private Text playerNameText;

    [Tooltip("UI Slider to display the players health")]
    [SerializeField]
    private Slider playerHealthSlider;

    [Tooltip("Pixel offset from target")]
    [SerializeField]
    private Vector3 screenOffset = new Vector3(0f, 30f, 0f);

    private PlayerManager target;
    private float characterControllerHeight = 0f;
    private Transform targetTransform;
    private Vector3 targetPosition;

    private void Awake()
    {
        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHealthSlider != null)
        {
            playerHealthSlider.value = target.health;
        }

        if(target == null)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    private void LateUpdate()
    {
        if(targetTransform != null)
        {
            targetPosition = targetTransform.position;
            targetPosition.y += characterControllerHeight;
            this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
        }
    }

    public void SetTarget(PlayerManager target)
    {
        if(target == null)
        {
            Debug.LogError("Missing PlayerManager target for UI script", this);
            return;
        }

        this.target = target;
        if(playerNameText != null)
        {
            playerNameText.text = this.target.photonView.Owner.NickName;
        }

        CharacterController charCon = this.target.GetComponent<CharacterController>();
        if(charCon != null)
        {
            characterControllerHeight = charCon.height;
        }
    }
}
