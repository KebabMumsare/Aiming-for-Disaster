using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class shopHandler : MonoBehaviour
{
    public BoxCollider2D shopCollider;
    public GameObject shopItemPrefab;

    private GameObject shopPurchaseConfirmationUI;
    private Button buyButton;
    private Currencies playerCurrencies;
    private string currentItem;
    private float currentPrice;
    private string currentCurrencyType; // "Bullets" or "Magazines"

    private void SetupBuyButton()
    {
        if (buyButton != null && buyButton.onClick.GetPersistentEventCount() > 0)
        {
            // Button already setup
            return;
        }

        if (shopPurchaseConfirmationUI != null)
        {
            // Try to find the buy button
            Transform buttonTransform = shopPurchaseConfirmationUI.transform.Find("BuyButton");
            if (buttonTransform != null)
            {
                buyButton = buttonTransform.GetComponent<Button>();
                if (buyButton != null)
                {
                    // Remove any existing listeners to avoid duplicates
                    buyButton.onClick.RemoveAllListeners();
                    buyButton.onClick.AddListener(OnBuyButtonClicked);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Find the player's Currencies component
            GameObject player = other.gameObject;
            playerCurrencies = player.GetComponent<Currencies>();
            if (playerCurrencies == null)
            {
                return;
            }

            // Find the ShopPurchaseConfirmationUI as a child of the player
            Transform uiTransform = player.transform.Find("ShopPurchaseConfirmation");
            if (uiTransform == null)
            {
                // Try searching in UI or Canvas
                Canvas canvas = player.GetComponentInChildren<Canvas>();
                if (canvas != null)
                {
                    uiTransform = canvas.transform.Find("ShopPurchaseConfirmation");
                }
            }

            if (uiTransform != null)
            {
                shopPurchaseConfirmationUI = uiTransform.gameObject;
            }
            else
            {
                return;
            }

            switch (gameObject.name)
            {
                case "BootsPedestal":
                    OpenShopPurchaseConfirmationUI("Boots");
                    break;
                case "GlovesPedestal":
                    OpenShopPurchaseConfirmationUI("Gloves");
                    break;
                case "MjodPedestal":
                    OpenShopPurchaseConfirmationUI("Mjod");
                    break;
            }
        }
    }

    private void OpenShopPurchaseConfirmationUI(string itemName)
    {
        if (shopPurchaseConfirmationUI == null)
        {
            return;
        }

        // Setup the buy button (in case UI was inactive during Start)
        SetupBuyButton();

        HideAllItemImages();
        HideAllCurrencyImages();

        currentItem = itemName;

        if (itemName == "Boots")
        {
            currentPrice = 20;
            currentCurrencyType = "Bullets";
            SetItemImage("ItemImages/Boots", true);
            SetItemText("ItemName", "Boots");
            SetItemBuffText("ItemBuff", "+20% Movement Speed", new Color(0.53f, 0.81f, 0.92f)); // Sky blue
            SetPriceText("BuyButton/Price", currentPrice.ToString());
            SetCurrencyImage("BuyButton/CurrencyImages/Bullet", true);
        }
        else if (itemName == "Gloves")
        {
            currentPrice = 2;
            currentCurrencyType = "Magazines";
            SetItemImage("ItemImages/Gloves", true);
            SetItemText("ItemName", "Gloves");
            SetItemBuffText("ItemBuff", "+30% Damage", new Color(0.94f, 0.5f, 0.5f)); // Light coral
            SetPriceText("BuyButton/Price", currentPrice.ToString());
            SetCurrencyImage("BuyButton/CurrencyImages/Magazine", true);
        }
        else if (itemName == "Mjod")
        {
            currentPrice = 20;
            currentCurrencyType = "Bullets";
            SetItemImage("ItemImages/Mjod", true);
            SetItemText("ItemName", "Mjod");
            SetItemBuffText("ItemBuff", "+30% XP", new Color(0.68f, 0.85f, 0.9f)); // Light blue
            SetPriceText("BuyButton/Price", currentPrice.ToString());
            SetCurrencyImage("BuyButton/CurrencyImages/Bullet", true);
        }

        shopPurchaseConfirmationUI.SetActive(true);
    }

    private void OnBuyButtonClicked()
    {
        if (playerCurrencies == null)
        {
            return;
        }

        bool purchaseSuccessful = false;

        if (currentCurrencyType == "Bullets")
        {
            purchaseSuccessful = playerCurrencies.SpendBullets(currentPrice);
        }
        else if (currentCurrencyType == "Magazines")
        {
            purchaseSuccessful = playerCurrencies.SpendMagazines(currentPrice);
        }

        if (purchaseSuccessful)
        {
            DropItem(currentItem);
            shopPurchaseConfirmationUI.SetActive(false);
        }
    }

    private void DropItem(string itemName)
    {
        if (shopItemPrefab == null)
        {
            return;
        }

        // Spawn the item near the pedestal
        Vector3 spawnPosition = new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z);
        Instantiate(shopItemPrefab, spawnPosition, Quaternion.identity);
    }

    private void HideAllItemImages()
    {
        SetItemImage("ItemImages/Boots", false);
        SetItemImage("ItemImages/Gloves", false);
        SetItemImage("ItemImages/Mjod", false);
    }

    private void HideAllCurrencyImages()
    {
        SetCurrencyImage("BuyButton/CurrencyImages/Bullet", false);
        SetCurrencyImage("BuyButton/CurrencyImages/Magazine", false);
    }

    private void SetItemImage(string path, bool active)
    {
        Transform child = shopPurchaseConfirmationUI.transform.Find(path);
        if (child != null)
        {
            child.gameObject.SetActive(active);
        }
    }

    private void SetItemText(string path, string text)
    {
        Transform child = shopPurchaseConfirmationUI.transform.Find(path);
        if (child != null)
        {
            TextMeshProUGUI tmp = child.GetComponent<TextMeshProUGUI>();
            if (tmp != null) tmp.text = text;
        }
    }

    private void SetItemBuffText(string path, string text, Color color)
    {
        Transform child = shopPurchaseConfirmationUI.transform.Find(path);
        if (child != null)
        {
            TextMeshProUGUI tmp = child.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.text = text;
                tmp.color = color;
            }
        }
    }

    private void SetPriceText(string path, string price)
    {
        Transform child = shopPurchaseConfirmationUI.transform.Find(path);
        if (child != null)
        {
            TextMeshProUGUI tmp = child.GetComponent<TextMeshProUGUI>();
            if (tmp != null) tmp.text = price;
        }
    }

    private void SetCurrencyImage(string path, bool active)
    {
        Transform child = shopPurchaseConfirmationUI.transform.Find(path);
        if (child != null)
        {
            child.gameObject.SetActive(active);
        }
    }
}
