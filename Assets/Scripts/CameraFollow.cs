using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform target;
    [SerializeField] private float translateSpeed;
    [SerializeField] private float rotationSpeed;

    float speed;
    private Vector3 _position;

    private void OnEnable() {
        _position = target.transform.position;
        //Time.timeScale = 1.5;
    }

    private void UpdateSpeed()
    {
        var dt = Time.deltaTime;
        var current = target.transform.position;
        var delta = Vector3.Distance(current, _position);
        speed = delta / dt;
        _position = current;
        //Debug.Log(speed);
    }

    private void FixedUpdate()
    {
        HandleTranslation();
        HandleRotation();
        UpdateSpeed();
        //GetComponent<Camera>().fieldOfView = speed;

    }

    private void HandleTranslation()
    {
        var targetPosition = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, translateSpeed * Time.deltaTime);
    }
    private void HandleRotation()
    {
        var direction = target.position - transform.position;
        var rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}
