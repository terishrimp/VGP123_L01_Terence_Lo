using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPickup : Pickup
{

    protected override void ActivatePickup()
    {
        //add energy to megaman
        base.ActivatePickup();
    }
}
