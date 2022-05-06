using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float rotationSpeed = 80f;
    private PlayerController playerController;
    private int rotateCount=300;
    private bool rotateByFragments = true;//rotates by a big angle in one press of a button

    private Camera mainCamera;

    private Vector3 lastMousePos;
    private Vector3 currentMousePos;

    private float sensitivytyX = 1;
    private float sensitivytyY = 1;
    private float rotationSensitivyty = 1.5f;

    private Vector3 defaultPosition;
    private Quaternion defaultRotation;
    private float defaultCameraSize;

    private bool rotateWithRightMouseButton = false;
    private bool followPlayer = false;

    private GameObject player;
    
    void Start()
    {
        mainCamera=Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
        defaultCameraSize = mainCamera.orthographicSize;
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;
        playerController = GameObject.FindObjectOfType(typeof(PlayerController)) as PlayerController;
    }
    

    void Update()
    {
        if (rotateByFragments)//rotate camera
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(rotateCamera(rotateCount, 1));
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(rotateCamera(rotateCount, -1));
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Q))
            {
                transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime,Space.World);
                playerController.ModifyDefaultAngles(transform.rotation.eulerAngles.y);
            }
            if (Input.GetKey(KeyCode.E))
            {
                transform.Rotate(Vector3.down * rotationSpeed * Time.deltaTime,Space.World);
                playerController.ModifyDefaultAngles(transform.rotation.eulerAngles.y);
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0) {//scale camera
            if (mainCamera.orthographicSize > 1.5f)
            {
                mainCamera.orthographicSize -= 0.5f;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            if (mainCamera.orthographicSize < 25f)
            {
                mainCamera.orthographicSize += 0.5f;
            }
        }
        
        if (Input.GetMouseButton(1))//rotate camera
        {
            if (rotateWithRightMouseButton)//if enabled also moves the camera, when rotates
            {
                transform.position -= transform.right * Input.GetAxis("Mouse X") * mainCamera.orthographicSize/30f * sensitivytyX;
                transform.position -= transform.up * Input.GetAxis("Mouse Y") * mainCamera.orthographicSize/30f * sensitivytyY;
            }
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * rotationSensitivyty,Space.World);
            playerController.ModifyDefaultAngles(transform.rotation.eulerAngles.y);
        }

        if (!followPlayer)
        {

            //if (MouseMoved())
            //{
            //currentMousePos = Input.mousePosition;
            //var deltaMousePos = currentMousePos - lastMousePos;
            //lastMousePos = currentMousePos;
            if (Input.GetMouseButton(0)) //move camera
            {
                transform.position -= transform.right * Input.GetAxis("Mouse X") * mainCamera.orthographicSize / 30f *
                                      sensitivytyX; //30f is like magic number, which i dont know how to get correctly but it almost fixes camera movement relative to its size
                transform.position -= transform.up * Input.GetAxis("Mouse Y") * mainCamera.orthographicSize / 30f *
                                      sensitivytyY;
                //transform.Translate(deltaMousePos/cameraOffsetZ, Space.Self);
            }
            //}
        }
        else
        {
            transform.position = player.transform.position;
        }

        if (Input.GetKeyDown(KeyCode.C))//return to default position
        {
            if (!followPlayer)
            {
                transform.position = defaultPosition;
            }
            transform.rotation = defaultRotation;
            mainCamera.orthographicSize = defaultCameraSize;
            playerController.ModifyDefaultAngles(transform.rotation.eulerAngles.y);
        }
        
        if (Input.GetKeyDown(KeyCode.V))//makes camera follow player
        {
            followPlayer = !followPlayer;
        }
    }

    bool MouseMoved()
    {
        return (Input.GetAxisRaw("Mouse X")!= 0 || Input.GetAxisRaw("Mouse Y")!= 0);
    }
    
    IEnumerator rotateCamera(int rotCount, int direction)
    {
        for (int i = 0; i < rotCount; i++)
        {
            float timeInc = Mathf.Abs(Mathf.Cos(((float)i / rotCount) * Mathf.PI));//(currenly bad.) makes it fast at start and finish, and slow at middle //Mathf.Abs(Mathf.Cos(((180 * ((float)i / rotCount)) * Mathf.PI) / 180))
            transform.Rotate(new Vector3(0, direction, 0) * timeInc * rotationSpeed * Time.deltaTime,Space.World);
            playerController.ModifyDefaultAngles(transform.rotation.eulerAngles.y);
            yield return new WaitForSeconds(0.001f);
        }
    }
}
