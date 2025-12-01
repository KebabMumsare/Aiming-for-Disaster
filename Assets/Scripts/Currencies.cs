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
    
    }

    public void AddBullets(float bullets) // Add bullets - Used in EnemyBehaviorController
    {
        currentBullets += bullets;
        currentBulletsText.text = "Bullets:" + currentBullets;
    }

    public void AddMagazines(float magazines) // Add magazines - Used in EnemyBehaviorController
    {
        currentMagazines += magazines;
        currentMagazinesText.text = "Mags:" + currentMagazines;
    }
}
