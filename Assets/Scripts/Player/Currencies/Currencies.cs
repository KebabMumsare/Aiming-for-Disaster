using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Currencies : MonoBehaviour
{
    public float currentBullets = 0;
    public float currentMagazines = 0;
    
    public TextMeshProUGUI currentBulletsText; // in this game bullets act as main currency and magazines as more exclusive currency
    public TextMeshProUGUI currentMagazinesText;

    
    void Start()
    {
        if (currentBulletsText == null || currentMagazinesText == null)
        {
            Debug.LogError("Bullets or magazines text are not assigned. Assign them on the 'Player'");
            return;
        }
    
    }

    public void AddBullets(float bullets) // Add bullets - Used in EnemyBehaviorController
    {
        currentBullets += bullets;
        UpdateBulletsUI();
    }

    public void AddMagazines(float magazines) // Add magazines - Used in EnemyBehaviorController
    {
        currentMagazines += magazines;
        UpdateMagazinesUI();
    }

    public bool SpendBullets(float bullets) // Deduct bullets if player has enough
    {
        if (currentBullets >= bullets)
        {
            currentBullets -= bullets;
            UpdateBulletsUI();
            return true;
        }
        return false;
    }

    public bool SpendMagazines(float magazines) // Deduct magazines if player has enough
    {
        if (currentMagazines >= magazines)
        {
            currentMagazines -= magazines;
            UpdateMagazinesUI();
            return true;
        }
        return false;
    }

    private void UpdateBulletsUI()
    {
        if (currentBulletsText != null)
        {
            currentBulletsText.text = currentBullets.ToString();
        }
    }

    private void UpdateMagazinesUI()
    {
        if (currentMagazinesText != null)
        {
            currentMagazinesText.text = currentMagazines.ToString();
        }
    }

    public void Reset()
    {
        currentBullets = 0;
        currentMagazines = 0;
        UpdateBulletsUI();
        UpdateMagazinesUI();
    }
}
