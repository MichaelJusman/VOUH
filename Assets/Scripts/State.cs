using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : GameBehaviour
{
    public bool isComplete { get; protected set; }

    public virtual void OnEnter() { }
    public virtual void OnUpdate() { }
    public virtual void OnFixedUpdate() { }
    public virtual void OnExit() { }
}

