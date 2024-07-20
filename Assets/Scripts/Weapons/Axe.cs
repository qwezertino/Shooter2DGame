using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Axe : MonoBehaviour
{
    public event EventHandler OnAxeSwing;
    public void Attack()
    {
        OnAxeSwing?.Invoke(this, EventArgs.Empty);
    }
}
