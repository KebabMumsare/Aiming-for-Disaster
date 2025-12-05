using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class shopHandler : MonoBehaviour
{
    public BoxCollider2D shopCollider;
    public GameObject ShopPurchaseConfirmationUI;
    public GameObject shopItemPrefab;

    private Button buyButton;
    private Currencies playerCurrencies;
    private string currentItem;
    private float currentPrice;
    private string currentCurrencyType; // "Bullets" or "Magazines"

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerCurrencies = player.GetComponent<Currencies>();
            if (playerCurrencies == null)
            {
                Debug.LogWarning("Currencies component not found on Player!");
            }
            else
            {
                Debug.Log("Player currencies found successfully!");
            }
        }
        else
        {
            Debug.LogWarning("Player GameObject not found!");
        }
    }

    private void SetupBuyButton()
    {
        if (buyButton != null && buyButton.onClick.GetPersistentEventCount() > 0)
        {
            // Button already setup
            return;
        }

        if (ShopPurchaseConfirmationUI != null)
        {
            // Try to find the buy button
            Transform buttonTransform = ShopPurchaseConfirmationUI.transform.Find("BuyButton");
            if (buttonTransform != null)
            {
                buyButton = buttonTransform.GetComponent<Button>();
                if (buyButton != null)
                {
                    // Remove any existing listeners to avoid duplicates
                    buyButton.onClick.RemoveAllListeners();
                    buyButton.onClick.AddListener(OnBuyButtonClicked);
                    Debug.Log("Buy button found and listener attached!");
                }
                else
                {
                    Debug.LogWarning("BuyButton found but no Button component attached!");
                }
            }
            else
            {
                Debug.LogWarning("BuyButton not found in ShopPurchaseConfirmationUI! Make sure the GameObject is named 'BuyButton'");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
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
        if (ShopPurchaseConfirmationUI == null)
        {
            Debug.LogWarning("ShopPurchaseConfirmationUI is not assigned!");
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

        ShopPurchaseConfirmationUI.SetActive(true);
    }

    private void OnBuyButtonClicked()
    {
        Debug.Log("Buy button clicked!");

        if (playerCurrencies == null)
        {
            Debug.LogWarning("Player currencies not found!");
            return;
        }

        Debug.Log($"Attempting to buy {currentItem} for {currentPrice} {currentCurrencyType}");
        Debug.Log($"Current Bullets: {playerCurrencies.currentBullets}, Current Magazines: {playerCurrencies.currentMagazines}");

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
            Debug.Log($"Successfully purchased {currentItem} for {currentPrice} {currentCurrencyType}!");
            DropItem(currentItem);
            ShopPurchaseConfirmationUI.SetActive(false);
        }
        else
        {
            Debug.Log($"Not enough {currentCurrencyType}! Need {currentPrice}, but you don't have enough.");
        }
    }

    private void DropItem(string itemName)
    {
        if (shopItemPrefab == null)
        {
            Debug.LogWarning($"Shop item prefab is not assigned on {gameObject.name}!");
            return;
        }

        // Spawn the item near the pedestal
        Vector3 spawnPosition = new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z);
        Instantiate(shopItemPrefab, spawnPosition, Quaternion.identity);
        Debug.Log($"Dropped {itemName} at {spawnPosition}");
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
        Transform child = ShopPurchaseConfirmationUI.transform.Find(path);
        if (child != null)
        {
            child.gameObject.SetActive(active);
        }
    }

    private void SetItemText(string path, string text)
    {
        Transform child = ShopPurchaseConfirmationUI.transform.Find(path);
        if (child != null)
        {
            TextMeshProUGUI tmp = child.GetComponent<TextMeshProUGUI>();
            if (tmp != null) tmp.text = text;
        }
    }

    private void SetItemBuffText(string path, string text, Color color)
    {
        Transform child = ShopPurchaseConfirmationUI.transform.Find(path);
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
        Transform child = ShopPurchaseConfirmationUI.transform.Find(path);
        if (child != null)
        {
            TextMeshProUGUI tmp = child.GetComponent<TextMeshProUGUI>();
            if (tmp != null) tmp.text = price;
        }
    }

    private void SetCurrencyImage(string path, bool active)
    {
        Transform child = ShopPurchaseConfirmationUI.transform.Find(path);
        if (child != null)
        {
            child.gameObject.SetActive(active);
        }
    }
}
