using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CarInfo : NetworkBehaviour
{
    [field: SyncVar]
    public Player Player { get; set; }
}
