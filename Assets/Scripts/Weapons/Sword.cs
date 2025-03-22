using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Sword : MonoBehaviour
{
    public event EventHandler OnSwing;
    public void Attack()
    {
        OnSwing?.Invoke(this, EventArgs.Empty);
    }
}
