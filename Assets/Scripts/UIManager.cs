using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject turretShopPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        turretShopPanel.SetActive(false);
    }
    public void CloseTurretShopPanel()
    {
        if (turretShopPanel != null)
        {
            turretShopPanel.SetActive(false);
        }
    }

    public void OpenTurretShopPanel()
    {
        if (turretShopPanel != null)
        {
            turretShopPanel.SetActive(true);
        }
    }
}