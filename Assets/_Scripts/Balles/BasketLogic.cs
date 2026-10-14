using UnityEngine;

public class BasketLogic : MonoBehaviour
{
    public GameEvent FirstBallInBasketEvent;
    public GameEvent BasketFullEvent;
    public GameEvent BallInBasket;
    public GameEvent OtherInBasket;
    public int BallsNeeded = 3;
    private int currentBalls = 0;

    private void OnTriggerEnter(Collider other)
    {
        // 1. Check if it's a ball
        Balle ball = other.GetComponent<Balle>();

        if (ball == null)
        {
            OtherInBasket.TriggerEvent();
        }

        // 2. Only proceed if it IS a ball AND it hasn't been counted yet
        else
        {
            if (!ball.isAlreadyInBasket)
            {
                currentBalls++;
                Debug.Log("Unique ball added! Count: " + currentBalls);
                BallInBasket.TriggerEvent();
                // Mark it so it can't be counted again
                ball.isAlreadyInBasket = true;

                if (currentBalls == 1)
                {
                    FirstBallInBasketEvent.TriggerEvent();
                }
                if (currentBalls == BallsNeeded)
                {
                    BasketFullEvent.TriggerEvent();
                }
            }
        }
    }
}