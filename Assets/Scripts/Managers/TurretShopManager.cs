using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretShopManager : MonoBehaviour
{
    [SerializeField] private GameObject _turretCardPrefab;
    [SerializeField] private Transform _turretCardContainer;
    [Header("Turret Settings")]
    [SerializeField] private List<TurretSetting> _turrets;

    private Node _selectedNode;

    void Start()
    {
        foreach (var setting in _turrets)
        {
            GameObject card = Instantiate(_turretCardPrefab, _turretCardContainer, false);
            card.GetComponent<TurretCard>().SetupTurretButton(setting);
            card.GetComponent<Button>().onClick.AddListener(() =>
            {
                card.GetComponent<TurretCard>().PlaceTurret();
            });
        }

        Node.OnNodeSelected += HandleNodeSelected;
        TurretCard.OnPlaceTurret += HandlePlaceTurret;
    }

    private void OnDestroy()
    {
        Node.OnNodeSelected -= HandleNodeSelected;
        TurretCard.OnPlaceTurret -= HandlePlaceTurret;
    }

    private void HandleNodeSelected(Node node)
    {
        _selectedNode = node;
        UIManager.Instance.OpenTurretShopPanel();
    }

    private void HandlePlaceTurret(TurretSetting setting)
    {
        if (_selectedNode == null || !_selectedNode.IsEmpty())
            return;

        GameObject turret = Instantiate(setting.TurretPrefab, _selectedNode.transform.position, _selectedNode.transform.rotation);
        _selectedNode.SetTurret(turret.GetComponent<Turret>());
    }
}