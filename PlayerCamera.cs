using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Player playerObject;

    private const int CAMERA_OFFSET_X = 0, CAMERA_OFFSET_Y = 0, CAMERA_OFFSET_Z = -1;
    private  Vector3 cameraOffSet = new Vector3(CAMERA_OFFSET_X, CAMERA_OFFSET_Y, CAMERA_OFFSET_Z);

    private void LateUpdate()
    {
        if(playerObject != null) TrackingPlayerObject();
    }

    private void TrackingPlayerObject()
    {
        transform.position = playerObject.GetPlayerObjectPosition().position + cameraOffSet;
    }
}
