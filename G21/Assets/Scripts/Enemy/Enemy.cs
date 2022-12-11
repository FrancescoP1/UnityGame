using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 1f;
    
    private GameObject Spawner;
    private SpriteRenderer _spriteRenderer;
    private Vector3 CurrentPointPosition, _lastPointPosition;
    private Waypoint Waypoints;
    private int _currentWaypointIndex;

    public static Action<Enemy> OnEndReached;
   // private EnemyHealth _enemyHealth;

    public float moveSpeed { get; set; }

    // Start is called before the first frame update
    void Start()
    {

       // _enemyHealth = GetComponent<EnemyHealth>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
       // EnemyHealth = GetComponent<EnemyHealth>();

        moveSpeed = MoveSpeed;

        _currentWaypointIndex = 0;
        CurrentPointPosition =  Waypoints.Points[_currentWaypointIndex];
        transform.position = CurrentPointPosition;
    }

    // Update is called once per frame
    void Update()
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
        transform.position = Vector3.MoveTowards(transform.position, CurrentPointPosition, MoveSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        if (CurrentPointPosition.x > _lastPointPosition.x) 
        {
            _spriteRenderer.flipX = false;
        }
        else
        {
            _spriteRenderer.flipX = true;
        }
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
            CurrentPointPosition = Waypoints.Points[_currentWaypointIndex];
        }
        else
        {
            EndPointReached();
        }
    }

    private void EndPointReached()
    {
        OnEndReached?.Invoke(this);
        ObjectPooler.ReturnToPool(gameObject);
    }

    private void Awake() 
    {
        Spawner =  GameObject.Find("Spawner");
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Waypoints = Spawner.GetComponent<Waypoint>();
        
    }

    public void StopMovement()
    {
        moveSpeed = 0f;
    }

    public void ResumeMovement()
    {
        moveSpeed = MoveSpeed;
    }
}
