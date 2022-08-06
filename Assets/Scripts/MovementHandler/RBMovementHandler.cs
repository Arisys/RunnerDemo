using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ari.Runner.Movement
{
     public class RBMovementHandler : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private Rigidbody rb = null;

        private readonly List<IRBMovementModifier> rbForceModifiers = new List<IRBMovementModifier>();

        private void Update() => Move();

        // access add/remove Methods to add/remove from the List
        public void AddModifier(IRBMovementModifier movementModifier) => rbForceModifiers.Add(movementModifier);
        public void RemoveModifier(IRBMovementModifier movementModifier) => rbForceModifiers.Remove(movementModifier);

        private void Move()
        {
            // initialize Movement modifier vectors
            Vector3 rbMovementForce = Vector3.zero;
            Vector3 rbMovementAcceleration = Vector3.zero;
            Vector3 rbMovementImpulse = Vector3.zero;
            Vector3 rbMovementVelocityChange = Vector3.zero;

            // Loop over all force-Modifiers
            foreach (IRBMovementModifier rbForceModifier in rbForceModifiers)
            {
                rbMovementForce += rbForceModifier.ForceValue;
                rbMovementAcceleration += rbForceModifier.AccelerationValue;
                rbMovementImpulse += rbForceModifier.ImpulseValue;
                rbMovementVelocityChange += rbForceModifier.VelocityChange;
            }

            // apply calculated forces
            rb.AddForce(rbMovementForce, ForceMode.Force);
            rb.AddForce(rbMovementAcceleration, ForceMode.Acceleration);
            rb.AddForce(rbMovementImpulse, ForceMode.Impulse);
            rb.AddForce(rbMovementImpulse, ForceMode.VelocityChange);
        }
    }
}