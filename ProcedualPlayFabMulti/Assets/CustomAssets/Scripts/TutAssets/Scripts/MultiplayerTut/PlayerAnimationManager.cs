using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerAnimationManager : MonoBehaviourPun
{
    private Animator animator;

    [SerializeField]
    private float directionDampTime;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        if(!animator)
        {
            Debug.LogError("Attached gameobject is missing animator component", this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            print("Returning Not your prefab");
            return;
        }
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if(stateInfo.IsName("Base Layer.Run"))
        {
            if(Input.GetButtonDown("Fire2"))
            {
                animator.SetTrigger("Jump");
            }
        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (!animator)
        {
            return;
        }
        else
        {
            

            if(v<0)
            {
                v = 0;
            }
            animator.SetFloat("Speed", h * h + v * v);
        }

        animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
    }
}
