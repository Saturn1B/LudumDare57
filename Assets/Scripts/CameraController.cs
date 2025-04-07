using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Control")]
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;
    [SerializeField] private float lookSensitivity;

    private float yaw;
    private float pitch;

    private Camera playerCamera;

    [SerializeField] private Transform submarineTransform;
    private Quaternion lastSubRot;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        lastSubRot = submarineTransform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale > 0 && GameManager.isGameStarted)
            HandleMouseLook();
    }

    private void HandleMouseLook()
    {
        yaw += Input.GetAxisRaw("Mouse X") * lookSensitivity;
        pitch -= Input.GetAxisRaw("Mouse Y") * lookSensitivity;

        pitch = ClampAngle(pitch, minPitch, maxPitch);

        Quaternion subRotation = submarineTransform.rotation;
        Quaternion yawRotation = Quaternion.Euler(.0f, yaw, .0f);
        transform.rotation = subRotation * yawRotation;

        playerCamera.transform.localRotation = Quaternion.Euler(pitch, .0f, .0f);

        //transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
        //playerCamera.transform.localEulerAngles = new Vector3(pitch, 0.0f, 0.0f);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }
}
