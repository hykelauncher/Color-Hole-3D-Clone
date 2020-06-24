using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    private Collider _theCollider, _holeCollider;
    private Rigidbody _rb;
    
    [Header("Hole Attributes")]
    public float holeDetectionDistance;
    public GameObject hole;

    public float fallForce;
    private void Awake()
    {
        _theCollider = GetComponent<Collider>();
        hole = GameObject.FindGameObjectWithTag(Tags.HoleTag);
        _rb = GetComponent<Rigidbody>();
        _holeCollider = hole.GetComponent<Collider>();
    }

    private void Update()
    {
        if (transform.position.y < -3)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (!_theCollider.enabled) return;
        var holePosition = hole.transform.position;
        var holeCenterPoint = new Vector3(holePosition.x, _holeCollider.bounds.max.y, holePosition.z);
        var direction = holeCenterPoint - transform.position;
        if (Vector3.Distance(holeCenterPoint, transform.position) < holeDetectionDistance)
        {
            _rb.AddForce(fallForce * direction, ForceMode.Force);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(Tags.HoleTag))
        {
            if (gameObject.CompareTag(Tags.DangerousCubeTag) && !LevelController.Instance.canTranslate)
            {
                LevelController.Instance.isGameOver = true;
            }
            _theCollider.enabled = false;
            if (LevelController.Instance.firstStageCubes.Contains(gameObject))
            {
                LevelController.Instance.firstStageCubes.Remove(gameObject);
                LevelController.Instance.firstStageBar.value += 1;
            }
            
            else if (LevelController.Instance.secondStageCubes.Contains(gameObject))
            {
                LevelController.Instance.secondStageCubes.Remove(gameObject);
                LevelController.Instance.secondStageBar.value += 1;
            }
        }
    }
}