using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Flier.Controls
{
    public class DefaultFlierControl : BasicFlier
    {
        public UiJoystick screenJoystick;
        public ToggleFire fireToggle;
        [Tooltip("Destroy the flier on collision with other flier.")]public bool destroyOnCollision = false;
        private Vector2 InputAxis => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        private BasicFlier otherFlier;
        // Update is called once per frame
        protected override void Update()
        {
            if (screenJoystick.GetThrustPower() > thrustPower)
                thrustPower = screenJoystick.GetThrustPower();
            if (InputAxis.magnitude > 0.1f)
            {
                thrustPower = Mathf.Clamp01(InputAxis.magnitude);
                HandleInput(InputAxis);
            }
            else
            {
                HandleInput(screenJoystick.GetInput());
            }
            if (Input.GetKeyDown(KeyCode.Space))
                fireToggle.fireAllToggle.isOn = true;
            else if(Input.GetKeyUp(KeyCode.Space))
                fireToggle.fireAllToggle.isOn = false;
            base.Update();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!destroyOnCollision)
                return;
            otherFlier = collision.collider.GetComponentInParent<BasicFlier>();
            if (otherFlier && destroyOnCollision)
                Destroy();
        }
    }
}
