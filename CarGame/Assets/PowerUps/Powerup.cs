using UnityEngine;
using System;
using System.Collections;

public abstract class Powerup : MonoBehaviour {

    protected void NotifyOnPickup()
    {
        if (PickedUp != null)
        {
            PickedUp(this, new EventArgs());
        }
    }

    public event EventHandler PickedUp;

}
