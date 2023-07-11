using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fusion;
public class UIManager : NetworkBehaviour
{
    [SerializeField] GameManager gm;
    [Space]
    [SerializeField] Text ammoText;
    [SerializeField] Text killsText;
    [SerializeField] Text damageText;
    [SerializeField] Slider healthSlider;
    [Space]
    [SerializeField] GameObject endCanvas;
    [SerializeField] Text stateText;
    [SerializeField] Text resultText;


    GunManager gunManager;

    public override void FixedUpdateNetwork()
    {
        SetStateUI();
    }

    private void SetStateUI()
    {
        if (gm.gameState == GameState.Play)
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
            stateText.text = "Lose...";
            endCanvas.SetActive(true);
            resultText.text = $"Kills: {gm.kills}\nDamage: {gm.damage}";
        }
    }

    private void SetText()
    {

        healthSlider.value = gm.health / 100;
        ammoText.text = gm.ammo[0] + "/" + gm.ammo[1];

        killsText.text = gm.kills.ToString("000");
        damageText.text = gm.damage.ToString("000");
    }
    public void MenuButton()
    {
        SceneManager.LoadScene(0);
    }
}
