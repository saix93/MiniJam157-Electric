using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Die3D : MonoBehaviour
{
    [Header("Parameters")]
    public float LaunchForce = 10f;
    public float MinAngle = 10f;
    public float MaxAngle = 45f;
    public float MaxTorque = 10f;
    
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void AddForces()
    {
        // Calculate launch direction with deviation
        var launchDirection = Quaternion.Euler(GetRandomAngle(), 0f, GetRandomAngle()) * transform.forward;

        // Launch the rigidbody
        rb.velocity = launchDirection * LaunchForce;
        
        float torqueX = Random.Range(-MaxTorque, MaxTorque);
        float torqueY = Random.Range(-MaxTorque, MaxTorque);
        float torqueZ = Random.Range(-MaxTorque, MaxTorque);

        Vector3 randomTorque = new Vector3(torqueX, torqueY, torqueZ);

        rb.AddTorque(randomTorque, ForceMode.Impulse);
    }

    private float GetRandomAngle()
    {
        var angle = Random.Range(MinAngle, MaxAngle);
        angle *= Random.Range(0, 2) == 1 ? 1 : -1;

        return angle;
    }

    public void ResetRigidbody()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
