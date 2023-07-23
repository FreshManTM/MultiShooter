using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fusion;
public class UIManager : NetworkBehaviour
{
    [SerializeField] GameManager _gm;
    [Space]
    [SerializeField] Text _ammoText;
    [SerializeField] Text _killsText;
    [SerializeField] Text _damageText;
    [SerializeField] Slider _healthSlider;
    [Space]
    [SerializeField] GameObject _endCanvas;
    [SerializeField] Text _stateText;
    [SerializeField] Text _resultText;

    public override void FixedUpdateNetwork()
    {
        SetStateUI();
    }

    private void SetStateUI()
    {
        if (_gm.GameState == GameState.Play)
        {
            SetText();
        }
        else if (_gm.GameState == GameState.Win)
        {
            _stateText.text = "Victory!!!";
            _endCanvas.SetActive(true);
        }
        else if (_gm.GameState == GameState.Lose)
        {
            _stateText.text = "Lose...";
            _endCanvas.SetActive(true);
            _resultText.text = $"Kills: {_gm.Kills}\nDamage: {_gm.Damage}";
        }
    }

    private void SetText()
    {

        _healthSlider.value = _gm.Health / 100;
        _ammoText.text = _gm.Ammo[0] + "/" + _gm.Ammo[1];

        _killsText.text = _gm.Kills.ToString("000");
        _damageText.text = _gm.Damage.ToString("000");
    }
    public void MenuButton()
    {
        SceneManager.LoadScene(0);
    }
}
