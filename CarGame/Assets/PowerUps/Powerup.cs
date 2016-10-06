using UnityEngine;
using System;
using System.Collections;

public abstract class Powerup : MonoBehaviour {

    /// <summary>
    ///Notify's listeners when the inherited powerup is pickedUp/expended
    /// </summary>
    protected void NotifyOnPickup()
    {
        if (PickedUp != null)
        {
            PickedUp(this, new EventArgs());
        }
    }

    public event EventHandler PickedUp;

}
