using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerawork : MonoBehaviour
{
    [Tooltip("distance in local x-z plane to the target")]
    [SerializeField]
    private float distance = 7.0f;

    [Tooltip("height the camera is above the target")]
    [SerializeField]
    private float height = 3.0f;

    [Tooltip("the smooth time lag for the height of the camera")]
    [SerializeField]
    private float heightSmoothLag = 0.3f;

    [Tooltip("allow the camera to be offset vertically from target")]
    [SerializeField]
    private Vector3 centerOff = Vector3.zero;

    [Tooltip("set to false if a component is instaniated by Photon Network")]
    [SerializeField]
    private bool followOnStart = false;

    public static Camera cam;

    Transform cameraTransform;
    bool isFollowing;
    private float heightVelocity;
    private float targetHeight = 100000.0f;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        if(followOnStart)
        {
            OnStartFollowing();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(cameraTransform == null && isFollowing)
        {
            OnStartFollowing();

        }

        if(isFollowing)
        {
            Apply();
        }
    }

    public void OnStartFollowing()
    {
        cameraTransform = Camera.main.transform;
        isFollowing = true;
        cam = cameraTransform.GetComponent<Camera>();
        JumpCut(); //nothing was smoothed just a jump cut
    }

    void Apply()
    {
        Vector3 targetcenter = transform.position + centerOff; //calculate current and target rot angles

        float originalTargetAngele = transform.eulerAngles.y;
        float currentAngle = cameraTransform.eulerAngles.y;//adjust real target angle when camera is locked

        float targetAngle = originalTargetAngele;
        currentAngle = targetAngle;
        targetHeight = targetcenter.y + height;

        float currentheight = cameraTransform.position.y;
        currentheight = Mathf.SmoothDamp(currentheight, targetHeight, ref heightVelocity, heightSmoothLag);

        Quaternion currentRotation = Quaternion.Euler(0, currentAngle, 0);

        cameraTransform.position = targetcenter;
        cameraTransform.position += currentRotation * Vector3.back * distance;

        cameraTransform.position = new Vector3(cameraTransform.position.x, currentheight, cameraTransform.position.z);
        SetUpRotation(targetcenter);

    }

    void JumpCut()
    {
        float oldheightSmooth = heightSmoothLag;
        heightSmoothLag = 0.001f;
        Apply();
        heightSmoothLag = oldheightSmooth;
    }

    void SetUpRotation(Vector3 centerPos)
    {
        Vector3 cameraPos = cameraTransform.position;
        Vector3 offsetToCenter = centerPos - cameraPos;

        Quaternion yRotation = Quaternion.LookRotation(new Vector3(offsetToCenter.x, 0, offsetToCenter.z));
        Vector3 relativeoffset = Vector3.forward * distance + Vector3.down * height;
        cameraTransform.rotation = yRotation * Quaternion.LookRotation(relativeoffset);
    }

}
