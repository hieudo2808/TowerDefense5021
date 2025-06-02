using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    [Header("Panels")]
    [SerializeField] private GameObject turretShopPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject overlayBlocker;
    [SerializeField] private GameObject nodeUIPanel;

    [Header("Text")]
    //[SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private TextMeshProUGUI sellText;
    [SerializeField] private TextMeshProUGUI turretLevelText;
    [SerializeField] private TextMeshProUGUI totalCoinsText;
    [SerializeField] private TextMeshProUGUI livesText;
    //[SerializeField] private TextMeshProUGUI gameOverTotalCoinsText;

    private Node _currentNodeSelected;

    private void Awake()
    {
        turretShopPanel.SetActive(false);
        
        if (overlayBlocker != null)
        {
            overlayBlocker.SetActive(false);
        }
    }

    private void Update()
    {
        totalCoinsText.text = CurrencySystem.Instance.TotalCoins.ToString();
        livesText.text = LevelManager.Instance.TotalLives.ToString();
    }    
    
    public void CloseTurretShopPanel()
    {
        if (turretShopPanel != null)
        {
            turretShopPanel.SetActive(false);
            DisableOverlay();
            NormalTime();
        }
    }

    public void OpenTurretShopPanel()
    {
        if (turretShopPanel != null && _currentNodeSelected.IsEmpty())
        {
            turretShopPanel.SetActive(true);
            EnableOverlay();
            PauseTime();
        }
    }

    public void PauseTime()
    {
        Time.timeScale = 0f;
    }

    public void NormalTime()
    {
        Time.timeScale = 1f;
    }    
    
    public void ShowWinPanel()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            EnableOverlay();
            PauseTime();
        }
    }

    public void ShowGameOverPanel()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            EnableOverlay();
            PauseTime();
        }
    }

    private void EnableOverlay()
    {
        if (overlayBlocker != null)
        {
            overlayBlocker.SetActive(true);
        }
    }

    private void DisableOverlay()
    {
        if (overlayBlocker != null)
        {
            overlayBlocker.SetActive(false);
        }
    }    
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    private void UpdateSellValue()
    {
        int sellAmount = _currentNodeSelected.Turret.TurretCost;
        sellText.text = sellAmount.ToString();
    }

    public void SellTurret()
    {
        CurrencySystem.Instance.AddCoins(Instance._currentNodeSelected.Turret.TurretCost);
        _currentNodeSelected.SellTurret();
        _currentNodeSelected = null;
        nodeUIPanel.SetActive(false);
    }

    private void ShowNodeUI()
    {
        nodeUIPanel.SetActive(true);
        UpdateSellValue();
    }

    private void NodeSelected(Node nodeSelected)
    {
        _currentNodeSelected = nodeSelected;
        if (_currentNodeSelected.IsEmpty())
        {
            turretShopPanel.SetActive(true);
        }
        else
        {
            turretShopPanel.SetActive(false);
            ShowNodeUI();
        }
    }

    public void CloseUINodePanel()
    {
        if (nodeUIPanel != null)
        {
            nodeUIPanel.SetActive(false);
        }
    }

    private void OnEnable()
    {
        Node.OnNodeSelected += NodeSelected;
    }

    private void OnDisable()
    {
        Node.OnNodeSelected -= NodeSelected;
    }
}