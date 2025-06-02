using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public static Action<Enemy> OnEndReached;

    [SerializeField] private int moveSpeed = 5;
    [SerializeField] private int deathCoinReward = 10;

    private int _currentWaypointIndex;
    private Vector3 _lastPointPosition;
    private SpriteRenderer _spriteRenderer;
    private EnemyHealth _enemyHealth;

    public Waypoint Waypoints;
    public int DeathCoinReward => deathCoinReward;
    public Vector3 CurrentPointPosition => Waypoints.GetWaypointPosition(_currentWaypointIndex);
    public EnemyHealth EnemyHealth { get; set; }
    public Transform bodyAim;
    public int MoveSpeed
    {
        get => moveSpeed;
        private set => moveSpeed = value;
    }

    private void Start()
    {
        _enemyHealth = GetComponent<EnemyHealth>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        EnemyHealth = GetComponent<EnemyHealth>();

        _currentWaypointIndex = 0;
        MoveSpeed = moveSpeed;
        transform.position = CurrentPointPosition;
        _lastPointPosition = transform.position;
        bodyAim = transform.Find("Body");
    }

    private void Update()
    {
        Move();
        Rotate();

        if (CurrentPointPositionReached())
        {
            UpdateCurrentPointIndex();
        }
    }
    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, CurrentPointPosition, moveSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        Vector3 newScale = transform.localScale;

        // So sánh vị trí waypoint hiện tại với vị trí cũ để biết hướng di chuyển
        if (CurrentPointPosition.x > _lastPointPosition.x)
        {
            // Di chuyển sang phải -> đảm bảo newScale.x là dương
            if (newScale.x > 0f) newScale.x = -newScale.x;
        }
        else
        {
            // Di chuyển sang trái -> đảm bảo newScale.x là âm
            if (newScale.x < 0f) newScale.x = -newScale.x;
        }

        transform.localScale = newScale;
    }

    private bool CurrentPointPositionReached()
    {
        float distanceToNextPointPosition = (transform.position - CurrentPointPosition).magnitude;
        if (distanceToNextPointPosition < 0.1f)
        {
            _lastPointPosition = transform.position;
            return true;
        }
        return false;
    }

    private void UpdateCurrentPointIndex()
    {
        int lastWaypointIndex = Waypoints.Points.Length - 1;
        if (_currentWaypointIndex < lastWaypointIndex)
        {
            _currentWaypointIndex++;
        }
        else
        {
            EndPointReached();
        }
    }

    private void EndPointReached()
    {
        OnEndReached?.Invoke(this);
        _enemyHealth.ResetHealth();
        ObjectPooler.ReturnToPool(gameObject);
    }

    public void StopMovement() => moveSpeed = 0;

    public void ResumeMovement(int speed) => moveSpeed = speed;

    public void SetFirstPoint()
    {
        transform.position = Waypoints.Points[0];
        _currentWaypointIndex = 0;
        _lastPointPosition = transform.position;
    }
}