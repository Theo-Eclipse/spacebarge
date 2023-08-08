using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultFlierControl : BasicFlier
{
    public UiJoystick screenJoystick;
    private Vector2 inputAxis => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    public float ThrustPowerDebug = 0.0f;
    // Update is called once per frame
    protected override void Update()
    {
        ThrustPowerDebug = screenJoystick.GetThrustPower();

        if (screenJoystick.GetThrustPower() > thrustPower)
            thrustPower = screenJoystick.GetThrustPower();
        if (inputAxis.magnitude > 0.1f)
        {
            thrustPower = Mathf.Clamp01(inputAxis.magnitude);
            HandleInput(inputAxis);
        }
        else
        {
            HandleInput(screenJoystick.GetInput());
        }
        base.Update();
    }
}