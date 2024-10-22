using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : GameBehaviour
{
    protected virtual void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
