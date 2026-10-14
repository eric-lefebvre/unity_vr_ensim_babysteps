using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WhackAMoleManager : MonoBehaviour
{
    private Taupe[] taupes;
    private Taupe currentTaupe;

    public const int ScoreLimit = 10;
    public static int Score = 0;
    public static bool IsFinished { get; private set; }
    private bool win = false;
    private int previousRandomIndex = -1;

    public GameEvent TaupeFrappe;
    [SerializeField] private GameEvent WhackTermine;

    private void Awake()
    {
        Animator[] taupesAnimators = GetComponentsInChildren<Animator>();
        foreach (Animator a in taupesAnimators)
        {
            a.enabled = true;
        }
    }
    void Start()
    {
        IsFinished = false;
        taupes = GetComponentsInChildren<Taupe>();
        StartCoroutine(MoleLoop());
    }

    private void EndGame()
    {
        win = true;
        IsFinished = true;
        StopAllCoroutines();

        WhackTermine.TriggerEvent();

        for (int i = 0; i < taupes.Length; i++)
        {
            taupes[i].ShowUpFinal();
            taupes[i].DisableTaupe();
        }
    }


    private int GetRandomIndex()
    {
        int randomIndex;
        do {
            randomIndex = Random.Range(0, taupes.Length);
        } while (randomIndex == previousRandomIndex);
        previousRandomIndex = randomIndex;
        return randomIndex;
    }

    IEnumerator MoleLoop()
    {
        while (!win)
        {
            // Choisir une taupe
            currentTaupe = taupes[GetRandomIndex()];

            float timeUp = Random.Range(1f, 2f);
            yield return new WaitForSeconds(timeUp);

            currentTaupe.GoUp();

            timeUp = Random.Range(1f, 3f);
            yield return new WaitForSeconds(timeUp);

            currentTaupe.GoDown();

            yield return new WaitForSeconds(2f);
        }

    }

    public void OnTaupeHit(Taupe taupe)
    {
        if (taupe != currentTaupe || Score >= ScoreLimit || win) return;

        Score++;
        Debug.Log("Score : " + Score);

        StopAllCoroutines();

        TaupeFrappe.TriggerEvent();
        if (Score >= ScoreLimit)
        {
            EndGame();
            return;
        }
        currentTaupe.GoDown();
        StartCoroutine(MoleLoop());
    }
}