using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveState
{
    public DateTime TimeStamp;
    public int SaveCount = 0;
    public string Description = "";

    public int Gold = 0;
    public bool DLCPurchased = false;
}
