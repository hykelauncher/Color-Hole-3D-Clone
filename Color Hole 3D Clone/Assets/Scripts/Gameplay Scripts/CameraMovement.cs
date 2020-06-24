using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    public bool canMove;
    public static CameraMovement Instance;

    private void Start()
    {
        canMove = false;
        if (Instance == null) Instance = this;
    }

    private void LateUpdate()
    {
        if (transform.position.z < 11.5f && canMove)
        {
            Movement();
        }
        
        else if (transform.position.z >= 11.5f && canMove) canMove = false;
    }

    private void Movement()
    {
        var currentCameraPosition = transform.position;
        transform.position = Vector3.MoveTowards(currentCameraPosition,
            new Vector3(currentCameraPosition.x, currentCameraPosition.y, 11.5f), 15 * Time.deltaTime);
    }
}
