using System;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class FaceThePlayer : MonoBehaviour
{
    [SerializeField] Vector3 Offset; 
    void Update()
    {
        transform.LookAt(ServiceHub.Instance.player.transform);
        transform.rotation *= Quaternion.Euler(Offset);
    }
}
