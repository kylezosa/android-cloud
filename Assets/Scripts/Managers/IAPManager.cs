using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;



public class IAPManager : Singleton<IAPManager>, IStoreListener
{
    private IStoreController m_StoreController = null;
    private IExtensionProvider m_StoreExtensionProvider = null;

    public const string GOLD_50 = "com.kyle.cloudtest.gold_50";
    public const string DLC = "com.kyle.cloudtest.dlc";

    private void Start()
    {
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }

    private void InitializePurchasing()
    {
        CuConsole.Log(string.Format("Initialize Purchasing called."), logArea: CuArea.IAPManager);

        if (IsInitialized())
        {
            return;
        }

        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(GOLD_50, ProductType.Consumable);
        builder.AddProduct(DLC, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void Buy50Gold()
    {
        BuyProductID(GOLD_50);
    }

    public void BuyDLC()
    {
        BuyProductID(DLC);
    }

    void BuyProductID(string productId)
    {
        CuConsole.Log(string.Format("Buy Product ID: {0} called.", productId), logArea: CuArea.IAPManager);

        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                CuConsole.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id), logArea: CuArea.IAPManager);
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                CuConsole.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase", logArea: CuArea.IAPManager);
            }
        }
        else
        {
            CuConsole.Log("BuyProductID FAIL. Not initialized.", logArea: CuArea.IAPManager);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        CuConsole.Log("IAP Initialized.", logArea: CuArea.IAPManager);

        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;

        CuConsole.Log(string.Format("Product {0} has receipt: {1}", GOLD_50, m_StoreController.products.WithID(GOLD_50)?.hasReceipt), logArea: CuArea.IAPManager);
        CuConsole.Log(string.Format("Product {0} has receipt: {1}", DLC, m_StoreController.products.WithID(DLC)?.hasReceipt), logArea: CuArea.IAPManager);
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        CuConsole.Log(string.Format("IAP Initialization Failed: {0}", error), logArea: CuArea.IAPManager);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        CuConsole.Log(string.Format("IAP Purchase of {0} failed: {1}", product.definition.id, failureReason), logArea:CuArea.IAPManager);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        CuConsole.Log(string.Format("Process Purchase called."), logArea: CuArea.IAPManager);
        string productId = purchaseEvent.purchasedProduct.definition.id;
        switch (productId)
        {
            case GOLD_50:
                CuConsole.Log(string.Format("IAP Purchase of {0} passed.", GOLD_50), logArea: CuArea.IAPManager);
                CloudSaveManager.Instance.State.Gold += 50;
                break;
            case DLC:
                CuConsole.Log(string.Format("IAP Purchase of {0} passed.", DLC), logArea: CuArea.IAPManager);
                CloudSaveManager.Instance.State.DLCPurchased = true;
                break;
            default:
                CuConsole.Log(string.Format("IAP Purchase of Unknown Product {0}.", productId), logArea: CuArea.IAPManager);
                break;
        }
        return PurchaseProcessingResult.Complete;
    }
}
