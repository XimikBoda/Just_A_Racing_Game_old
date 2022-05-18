using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    [SerializeField] private Vector3 Offset;
    [SerializeField] private Transform TargetObject;
    [SerializeField] private float TranslationSpeed;
    [SerializeField] private float RotationSpeed;
    
    void FixedUpdate()
    {
        HandleTranslation();
        HandleRotation();
    }

    private void HandleTranslation()
    {
        var targetPosition = TargetObject.TransformPoint(Offset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, TranslationSpeed * Time.deltaTime);
    }

    private void HandleRotation()
    {
        var direction = TargetObject.position - transform.position;
        var rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, RotationSpeed * Time.deltaTime);
    }
}
