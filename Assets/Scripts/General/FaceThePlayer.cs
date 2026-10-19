using System;
using UnityEngine;

public class FaceThePlayer : MonoBehaviour
{
    [SerializeField] Vector3 Offset;
    private Transform camTransform;

    private void Start()
    {
        camTransform = ServiceHub.Instance.player.GetComponent<PlayerController>().cam.transform;
    }
    void Update()
    {
        transform.LookAt(camTransform);
        transform.rotation *= Quaternion.Euler(Offset);
    }
}
