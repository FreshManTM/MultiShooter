using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] GameManager gm;
    [Space]
    [SerializeField] Text ammoText;
    [SerializeField] Text killsText;
    [SerializeField] Slider healthSlider;
    [Space]
    [SerializeField] GameObject endCanvas;
    [SerializeField] Text stateText;
    [SerializeField] Text YresultText;
    [SerializeField] Text MresultText;
    

    GunManager gunManager;

    public void Update()
    {
        if(gm.gameState == GameState.Play)
        {
            SetText();
        }
        else if (gm.gameState == GameState.Win)
        {
            stateText.text = "Victory!!!";
            endCanvas.SetActive(true);
        }
        else if (gm.gameState == GameState.Lose)
        {
            print("This is lose.");
            stateText.text = "Lose...";
            endCanvas.SetActive(true);
            YresultText.text = $"Kills: {gm.kills}\nDamage:{gm.playerDamage}";
            MresultText.text = $"Kills: {gm.kills}\nDamage:{gm.playerDamage}";
        }
    }

    private void SetText()
    {
        if (gunManager != null)
        {
            healthSlider.value = gm.health / 100;
            gm.ammo = gunManager.GetAmmo();
            ammoText.text = gm.ammo[0] + "/" + gm.ammo[1];
        }
        else
        {
            gunManager = gm.gunManager;
        }
        killsText.text = gm.kills.ToString();
        //killsText.text = gm.damage.ToString();
    }
}
