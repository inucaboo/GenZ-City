using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderGun : MonoBehaviour
{
 
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0, 90, JoystickRight.rotY + 90);
    }
}
