using UnityEngine;
using System;
using System.Collections;

public abstract class Powerup : MonoBehaviour
{
    /// <summary>
    /// This pickup was collected by an object
    /// </summary>
    public event PickUpEvent PickedUp;

    /// <summary>
    ///Notify's listeners when the inherited powerup is pickedUp/expended
    /// </summary>
    protected void NotifyOnPickup(GameObject collector)
    {
        if (PickedUp != null)
            PickedUp(this, new PickupEventArgs(collector));
    }
}

/// <summary>
/// Event delegate for an interaction with a pickup
/// </summary>
public delegate void PickUpEvent(object sender, PickupEventArgs e);

/// <summary>
/// Pickup event args used during a pickup event to store data on the object collecting the pickup
/// </summary>
public class PickupEventArgs
{
    /// <summary>
    /// The object collecting the pickup
    /// </summary>
    public GameObject Collector { get; private set; }

    public PickupEventArgs(GameObject collector)
    {
        Collector = collector;
    }
}
