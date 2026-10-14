using UnityEngine;
using System.Collections;

public class ElevatorAction : MonoBehaviour
{
    [Header("Elevator Settings")]
    public float height = 10f;
    public float speed = 3f;
    public float startDelay = 2f;
    public AudioSource liftSound;

    [Header("Gate Settings")]
    public Transform gate; 
    public float gateMoveDistance = 2.5f;
    public float gateSpeed = 2f;

    private Vector3 _bottomPos;
    private Vector3 _topPos;
    private Vector3 _gateOpenLocalPos;
    private Vector3 _gateClosedLocalPos;
    
    private Vector3 _currentTarget;
    private bool _isMoving = false;
    private bool _isGateClosing = false;

    void Start()
    {
        _bottomPos = transform.position;
        _topPos = _bottomPos + Vector3.up * height;
        
        if (gate != null)
        {
            _gateOpenLocalPos = gate.localPosition;
            _gateClosedLocalPos = _gateOpenLocalPos + Vector3.up * gateMoveDistance;
        }
    }

    // --- TRIGGER FUNCTIONS ---

    public void StartGoingUp()
    {
        if (_isMoving) return; // Don't interrupt if already moving
        _currentTarget = _topPos;
        StartCoroutine(ElevatorSequence());
    }

    public void StartGoingDown()
    {
        if (_isMoving) return;
        _currentTarget = _bottomPos;
        StartCoroutine(ElevatorSequence());
    }

    // --- LOGIC ---

    IEnumerator ElevatorSequence()
    {
        // 1. Close Gate
        _isGateClosing = true;
        
        // 2. Wait for the delay
        yield return new WaitForSeconds(startDelay);

        // 3. Move Elevator
        _isMoving = true;
        if (liftSound != null) liftSound.Play();
    }

    void Update()
    {
        // Handle Gate Closing (Up relative to platform)
        if (_isGateClosing && gate != null)
        {
            gate.localPosition = Vector3.MoveTowards(gate.localPosition, _gateClosedLocalPos, gateSpeed * Time.deltaTime);
        }

        // Handle Elevator Movement
        if (_isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, _currentTarget, speed * Time.deltaTime);

            // Arrival Check
            if (Vector3.Distance(transform.position, _currentTarget) < 0.01f)
            {
                _isMoving = false;
                _isGateClosing = false; 
                StartCoroutine(OpenGateAtDestination());
            }
        }
    }

    IEnumerator OpenGateAtDestination()
    {
        // Moves the gate back down relative to the platform
        while (gate != null && Vector3.Distance(gate.localPosition, _gateOpenLocalPos) > 0.01f)
        {
            gate.localPosition = Vector3.MoveTowards(gate.localPosition, _gateOpenLocalPos, gateSpeed * Time.deltaTime);
            yield return null;
        }
    }
}