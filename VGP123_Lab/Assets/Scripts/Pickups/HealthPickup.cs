using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup
{

    protected override void ActivatePickup()
    {
        //add health to megaman
        base.ActivatePickup();
    }
}
