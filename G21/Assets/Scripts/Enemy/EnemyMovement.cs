using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 1;
    
    private GameObject Spawner;

    private SpriteRenderer _spriteRenderer;
    private Vector3 CurrentPointPosition, _lastPointPosition;
    private Waypoint Waypoints;
    private int _currentWaypointIndex;

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

    private void EndPointReached()
    {
        // OnEndReached?.Invoke(this);
        ObjectPooler.ReturnToPool(gameObject);
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

    private void Awake() 
    {
        Spawner =  GameObject.Find("Spawner");
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Waypoints = Spawner.GetComponent<Waypoint>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
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
}
