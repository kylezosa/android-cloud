using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStoreScreen : UIScreen
{
    public override UIScreenType ScreenType { get => UIScreenType.GameStore; }

    public void Buy50Gold()
    {
        CuConsole.Log("Buy 50 Gold called.");

        IAPManager.Instance.Buy50Gold();
    }

    public void BuyDLC()
    {
        CuConsole.Log("Buy DLC called.");

        IAPManager.Instance.BuyDLC();
    }
}
