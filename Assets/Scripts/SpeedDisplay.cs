using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedDisplay : MonoBehaviour
{
    [SerializeField] public Text SpeedText;
    [SerializeField] public bool isEnabled = true;
    [SerializeField] public Rigidbody Car;
    
    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
        {
            SpeedText.text = ((int)Car.velocity.magnitude).ToString();
        }
    }
}
