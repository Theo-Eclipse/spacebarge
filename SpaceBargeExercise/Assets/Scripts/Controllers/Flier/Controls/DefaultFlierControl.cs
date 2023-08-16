using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flier.Controls
{
    public class DefaultFlierControl : BasicFlier
    {
        public UiJoystick screenJoystick;
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
