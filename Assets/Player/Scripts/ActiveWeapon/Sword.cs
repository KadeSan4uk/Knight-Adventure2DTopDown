using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Sword : MonoBehaviour
{
    public event EventHandler OnSwordSwing; 
    public void Attack()
    {
        OnSwordSwing?.Invoke(this,EventArgs.Empty);
    }
}
