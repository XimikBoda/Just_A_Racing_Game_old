using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPhysics : MonoBehaviour
{
    [SerializeField] private Transform CarBody;
    [SerializeField] private Rigidbody CarRigidbody;
    [SerializeField] private WheelCollider FLCollider, FRCollider, RLCollider, RRCollider;
    [SerializeField] private bool isEnabled = false;
    
    // Trying to add ragdoll physics to car body
    void FixPosition()
    {
        if (isEnabled)
        {
            float tmp = CarBody.rotation.eulerAngles.z % 360;
            //----------------------
            if (tmp > 60 && tmp < 180
                         && !FRCollider.isGrounded && !RRCollider.isGrounded)
            {
                CarRigidbody.angularVelocity = new Vector3(0, 0, 3);
            }
            else if (tmp > 60 && tmp < 180 
                              && !FLCollider.isGrounded && !RLCollider.isGrounded)
            {
                CarRigidbody.angularVelocity = new Vector3(0,0,-3);
            }
            //Debug.Log("Euler angle "+CarBody.rotation.eulerAngles.z % 360);
            //----------------------
            //Don't change rotation in this direction
            /*if (CarBody.rotation.eulerAngles.y > 60 && !FRCollider.isGrounded && !RRCollider.isGrounded)
            {
                CarRigidbody.velocity = new Vector3( currentVelocity.x, 2, currentVelocity.z);
            }
            else if (CarBody.rotation.eulerAngles.y < 300 && !FLCollider.isGrounded && !RLCollider.isGrounded)
            {
                CarRigidbody.velocity = new Vector3(currentVelocity.x, -2, currentVelocity.z);
            }*/
            //----------------------
            tmp = CarBody.rotation.eulerAngles.x % 360;
            if (tmp > 60 && tmp < 180 
                && !RRCollider.isGrounded && !RLCollider.isGrounded)
            {
                CarRigidbody.angularVelocity = new Vector3(3,0,0);
            }
            else if (tmp > 60 && tmp < 180 
                              && !FLCollider.isGrounded && !FRCollider.isGrounded)
            {
                CarRigidbody.angularVelocity = new Vector3(-3, 0, 0);
            }
        }
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        FixPosition();
    }

}
