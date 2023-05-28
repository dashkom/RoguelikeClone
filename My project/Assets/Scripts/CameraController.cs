using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Room curRoom;
    public float moveSpeedWhenRoomChange;

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        if (curRoom == null)
        {
            return;
        }

        Vector3 targetPos = GetCameraTargetPos();
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeedWhenRoomChange);
    }

    Vector3 GetCameraTargetPos()
    {
        if (curRoom == null)
        {
            return Vector3.zero;
        }

        Vector3 targetPos = curRoom.GetRoomCentre();
        targetPos.z = transform.position.z;

        return targetPos;
    }

    public bool IsSwitchigScene()
    {
        return transform.position.Equals(GetCameraTargetPos()) == false;
    }
}
