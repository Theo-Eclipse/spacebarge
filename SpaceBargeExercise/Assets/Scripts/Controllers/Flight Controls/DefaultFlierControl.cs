using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultFlierControl : BasicFlier
{
    public UiJoystick screenJoystick;
    private Vector2 inputAxis => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    // Update is called once per frame
    protected override void Update()
    {
        if(inputAxis.magnitude > 0.1f)
            HandleInput(inputAxis);
        else if(screenJoystick && screenJoystick.GetInput.magnitude > 0.1f)
            HandleInput(screenJoystick.GetInput);
        base.Update();
    }
}
