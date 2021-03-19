using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup
{
    [SerializeField] int healthGain = 1;

    protected override void ActivatePickup()
    {
        //add health to megaman
        GameManager.instance.Health += healthGain;
        base.ActivatePickup();
    }
}
