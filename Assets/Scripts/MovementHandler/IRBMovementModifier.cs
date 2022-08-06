using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ari.Runner.Movement
{
    // interface class for the Movement modifiers
    public interface IRBMovementModifier
    {
        Vector3 ForceValue { get; }
        Vector3 AccelerationValue { get; }
        Vector3 ImpulseValue { get; }
        Vector3 VelocityChange { get;  }
    }
}