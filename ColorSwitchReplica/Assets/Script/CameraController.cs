using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _playerTransform;
    [SerializeField] [Range(0,1f)]private float _damp;

    private void Start()
    {
        _playerTransform = FindObjectOfType<Player>().transform;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position,_playerTransform.position + Vector3.back * 10,_damp);
    }
}
