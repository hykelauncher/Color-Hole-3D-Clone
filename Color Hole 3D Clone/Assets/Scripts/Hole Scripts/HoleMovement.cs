using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleMovement : MonoBehaviour
{
    private Vector3 _mOffset;
    private float _zCoordinate, _yPositionAtStart;

    public Camera mainCamera;

    public float xClamp, zClamp;

    private void Awake()
    {
        if (Camera.main) mainCamera = Camera.main;
        _yPositionAtStart = transform.position.y;
    }

    private void Update()
    {
        if (!LevelController.Instance.canTranslate && LevelController.Instance.didLevelStarted && !LevelController.Instance.isGameOver)
            HoleDrag();
        else if (LevelController.Instance.didFirstStageEnded && LevelController.Instance.canTranslate &&
                 !LevelController.Instance.didLevelEnded)
        {
            HoleTransition();
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        var touchPoint = Input.mousePosition;
        touchPoint.z = _zCoordinate;
        return mainCamera.ScreenToWorldPoint(touchPoint);
    }

    private void HoleDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var currentPosition = gameObject.transform.position;
            _zCoordinate = mainCamera.WorldToScreenPoint(currentPosition).z;
            _mOffset = currentPosition - GetMouseWorldPos();
        }

        if (Input.GetMouseButton(0))
        {
            if (!LevelController.Instance.didFirstStageEnded)
            {
                transform.position = new Vector3(Mathf.Clamp(GetMouseWorldPos().x + _mOffset.x, -xClamp, xClamp),
                    _yPositionAtStart,
                    Mathf.Clamp(GetMouseWorldPos().z + _mOffset.z, -zClamp, zClamp));
            }
            else
            {
                transform.position = new Vector3(Mathf.Clamp(GetMouseWorldPos().x + _mOffset.x, -xClamp, xClamp),
                    _yPositionAtStart,
                    Mathf.Clamp(GetMouseWorldPos().z + _mOffset.z, 14f, 19f));
            }
        }
    }

    private void HoleTransition()
    {
        var currentPosition = transform.position;
        if (currentPosition.x > 0 || currentPosition.x < 0)
        {
            transform.position = Vector3.MoveTowards(currentPosition,
                new Vector3(0, currentPosition.y, currentPosition.z), Time.deltaTime);
        }
        else
        {
            CameraMovement.Instance.canMove = true;
            var newCurrentPosition = transform.position;
            transform.position = Vector3.MoveTowards(newCurrentPosition,
                new Vector3(newCurrentPosition.x, newCurrentPosition.y, 14.5f), 10 * Time.deltaTime);
            if (transform.position.z >= 14.5f) LevelController.Instance.canTranslate = false;
        }
    }
}