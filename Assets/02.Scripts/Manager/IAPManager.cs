using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
    [Header("Product ID")]
    public readonly string productId_dia_100 = "dia_100";
    public readonly string productId_dia_500 = "dia_500_";
    public readonly string productId_dia_1000 = "dia_1000";
    public readonly string productId_dia_3000 = "dia_3000";

    [Header("Chache")]
    private IStoreController storeController;
    private IExtensionProvider extensionProvider;
    [SerializeField] CoinEffectManager coinEffectManager;

    // Start is called before the first frame update
    void Start()
    {
        InitUnityIAP();
    }

    private void InitUnityIAP()
    {
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(productId_dia_100, ProductType.Consumable, new IDs() { { productId_dia_100, GooglePlay.Name } });
        builder.AddProduct(productId_dia_500, ProductType.Consumable, new IDs() { { productId_dia_500, GooglePlay.Name } });
        builder.AddProduct(productId_dia_1000, ProductType.Consumable, new IDs() { { productId_dia_1000, GooglePlay.Name } });
        builder.AddProduct(productId_dia_3000, ProductType.Consumable, new IDs() { { productId_dia_3000, GooglePlay.Name } });

        UnityPurchasing.Initialize(this, builder);
    }

    public void Purchase(string productId)
    {
        Product product = storeController.products.WithID(productId);

        if (product != null && product.availableToPurchase)
        {
            storeController.InitiatePurchase(product);
        }
        else
        {
            Debug.Log("상품이 존재하지 않거나 구매가 불가능");
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("초기화 실패");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("초기화 실패: " + message);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        int cur_Dia = GameManager.Instance.Dia;

        if (purchaseEvent.purchasedProduct.definition.id == productId_dia_100)
        {
            coinEffectManager.RewardPileOfCoin(cur_Dia, cur_Dia + 100, 1);
        }

        else if (purchaseEvent.purchasedProduct.definition.id == productId_dia_500)
        {
            coinEffectManager.RewardPileOfCoin(cur_Dia, cur_Dia + 500, 1);
        }

        else if (purchaseEvent.purchasedProduct.definition.id == productId_dia_1000)
        {
            coinEffectManager.RewardPileOfCoin(cur_Dia, cur_Dia + 1000, 1);
        }

        else if (purchaseEvent.purchasedProduct.definition.id == productId_dia_3000)
        {
            coinEffectManager.RewardPileOfCoin(cur_Dia, cur_Dia + 3000, 1);
        }

        Debug.Log("구매 성공! 상품 ID: " + purchaseEvent.purchasedProduct.definition.id);
        
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log("구매에 실패하였습니다. 상품ID: " + product.definition.id + "\n" + failureReason);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("유니티 IAP 초기화 성공");
        storeController = controller;
        extensionProvider = extensions;
    }
}
