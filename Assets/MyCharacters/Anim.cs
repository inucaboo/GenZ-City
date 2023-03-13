using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim : MonoBehaviour
{
    [Header("Animator")]
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (JoystickLeft.positionX < 0)
        {
            print("player is moving left at the speed: " + JoystickLeft.positionX);
            anim.Play("Running");
        }

        if (JoystickLeft.positionX > 0)
        {
            print("player is moving right at the speed: " + JoystickLeft.positionX);
            anim.Play("Runing");

        }

        if (JoystickLeft.positionY < 0)
        {
            print("player is moving backward at the speed: " + JoystickLeft.positionY);
            anim.Play("Running");

        }

        if (JoystickLeft.positionY > 0)
        {
            print("player is moving forward at the speed: " + JoystickLeft.positionY);
            anim.Play("Running");

        }

        if (JoystickLeft.positionX == 0 || JoystickLeft.positionY == 0)
        {
            anim.Play("Idle");
        }
    }

    public void Jump()
    {
        if (JoystickRight.jump)
        {
            //JoystickRight.jump = false;
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 300, 0));
            anim.Play("Jumping");
        }
    }
}
