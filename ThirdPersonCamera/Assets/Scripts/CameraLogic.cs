using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{

    Vector3 cameraTarget;
    GameObject player;

    float cameraOffSet = 1.0f;
    float distanceZ = 5.0f;
    float rotationX, rotationY;

    const float MIN_X = -20.0f;
    const float MAX_X = 20.0f;
    const float MIN_Z = -5.0f;
    const float MAX_Z = 8.0f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        cameraTarget = player.transform.position;
        cameraTarget.y += cameraOffSet;
        if (Input.GetButton("Fire2"))
        {
            rotationY += Input.GetAxis("Mouse X");
            rotationX -= Input.GetAxis("Mouse Y");
            rotationX = Mathf.Clamp(rotationX, MIN_X, MAX_X);
        }
        distanceZ -= Input.GetAxis("Mouse ScrollWheel");
        distanceZ = Mathf.Clamp(distanceZ, MIN_Z, MAX_Z);
    }
    private void LateUpdate()
    {
        Quaternion cameraRotation = Quaternion.Euler(rotationX, rotationY, 0);
        Vector3 cameraOffSet = new Vector3(0, 0, -distanceZ);
        transform.position = cameraTarget + cameraRotation * cameraOffSet;
        transform.LookAt(cameraTarget);
    }
    public Vector3 GetForwadVector()
    {
        Quaternion rotation = Quaternion.Euler(0, rotationY, 0);
        return rotation * Vector3.forward;
    }
}
