using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorEvents : MonoBehaviour
{
    private bool basketGameFinished;
    private bool slutMachineFinished;

    public event Action<bool> OnSlutMachineFinished;
    public event Action<bool> OnBasketGameFinished;
    public GameEvent BothGamesSalle1Finished;

    public bool HasGameStarted;

    public GameEvent PlayerAfkForTooLong;
    public Transform Player;
    private Vector3 playerSpawnPos;
    public float MaxTimeAfkBeforeEvent;
    private bool afkEventTriggered;
    private float timer;

    public GameEvent PlayerNoInteractForTooLong;
    private bool isPlayerOutBabyPark;
    public float MaxTimeInteractionBeforeEvent;
    private bool interactionEventTriggered;
    private void Start()
    {
        playerSpawnPos = Player.position;
    }

    private void Update()
    {
        if (!HasGameStarted)
            return;

        if (!afkEventTriggered)
        {
            if ((Player.position - playerSpawnPos).magnitude > 0.1f)
            {
                Debug.Log("Player Has Moved !");
                afkEventTriggered = true;
            }

            timer += Time.deltaTime;
            if (timer > MaxTimeAfkBeforeEvent)
            {
                Debug.Log("Player afk for too long");
                PlayerAfkForTooLong.TriggerEvent();
                afkEventTriggered = true;
            }
        }

        else if (isPlayerOutBabyPark && !interactionEventTriggered)
        {
            timer += Time.deltaTime;
            if (timer > MaxTimeInteractionBeforeEvent)
            {
                Debug.Log("Player no interaction for too long");
                PlayerNoInteractForTooLong.TriggerEvent();
                interactionEventTriggered = true;
            }
        }
    }

    public void SetInteractionEventTriggeredTrue()
    {
        Debug.Log("Player has interacted !");
        interactionEventTriggered = true;
    }

    public void SetIsPlayerOutBabyParkTrue()
    {
        timer = 0f;
        isPlayerOutBabyPark = true;
    }

    public void SetHasGameStartedTrue()
    {
        timer = 0f;
        HasGameStarted = true;
    }

    public bool BasketGameFinished
    {
        get => basketGameFinished;
        set
        {
            if (basketGameFinished == value) return;

            basketGameFinished = value;
            OnBasketGameFinished?.Invoke(basketGameFinished);
        }
    }

    public bool SlutMachineFinished
    {
        get => slutMachineFinished;
        set
        {
            if (slutMachineFinished == value) return;

            slutMachineFinished = value;
            OnSlutMachineFinished?.Invoke(slutMachineFinished);
        }
    }

    private void Awake()
    {
        OnBasketGameFinished += OpenDoorIfBothGamesFinished;
        OnSlutMachineFinished += OpenDoorIfBothGamesFinished;
    }

    public void OpenDoorIfBothGamesFinished(bool newValue)
    {
        if (basketGameFinished && slutMachineFinished)
        {
            BothGamesSalle1Finished.TriggerEvent();
        }
    }
}
