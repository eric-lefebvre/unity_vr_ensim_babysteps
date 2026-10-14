using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro; // Ajout nécessaire pour TextMeshPro

public class BlackjackManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject startButton; // This will now appear in your Inspector
    public TableTrigger tableTrigger; // Reference to sync the trigger state
    public TMP_Text resultText; // Référence directe au composant TextMeshPro

    [Header("Card Setup")]
    public List<GameObject> deckPrefabs;
    private List<GameObject> cardsOnTable = new List<GameObject>();
    private List<GameObject> shuffledDeck = new List<GameObject>();

    private List<int> dealerHandValues = new List<int>();
    private List<int> playerHandValues = new List<int>();

    [Header("Table Spawn Points")]
    public Transform playerSlot1; public Transform playerSlot2;
    public Transform playerSlot3; public Transform playerSlot4;
    public Transform dealerSlot1; public Transform dealerSlot2;
    public Transform dealerSlot3; public Transform dealerSlot4;

    [Header("Game Settings")]
    public float dealSpeed = 0.4f;
    public float waitBeforeFlip = 1.0f;

    public void StartNewGame()
    {
        Debug.Log("StartNewGame called");

        // Safety Check: Make sure we have cards and slots assigned
        if (deckPrefabs.Count == 0 || playerSlot1 == null)
        {
            Debug.LogError("Missing Prefabs or Slots in the GameManager Inspector!");
            return;
        }

        // Tell the trigger the game has started
        if (tableTrigger != null)
        {
            tableTrigger.LockTrigger();
        }

        // 1. Hide the Start Button
        if (startButton != null)
        {
            startButton.SetActive(false);
        }

        // Cacher le texte de résultat au début d'une nouvelle partie
        if (resultText != null)
        {
            resultText.gameObject.SetActive(false);
        }

        // 2. Reset the table
        StopAllCoroutines();
        foreach (GameObject card in cardsOnTable) Destroy(card);
        cardsOnTable.Clear();
        dealerHandValues.Clear();
        playerHandValues.Clear();

        // 3. Shuffle and Deal
        ShuffleDeck();
        StartCoroutine(DealSequence());
    }

    IEnumerator DealSequence()
    {
        // Deal 2 to player, 2 to dealer
        yield return Spawn(playerSlot1, false); yield return new WaitForSeconds(dealSpeed);
        yield return Spawn(dealerSlot1, true); yield return new WaitForSeconds(dealSpeed);
        yield return Spawn(playerSlot2, false); yield return new WaitForSeconds(dealSpeed);
        yield return Spawn(dealerSlot2, true); yield return new WaitForSeconds(dealSpeed);

        yield return new WaitForSeconds(waitBeforeFlip);

        // Reveal initial cards
        foreach (GameObject c in cardsOnTable)
        {
            if (c.TryGetComponent(out BlackjackCard cardScript))
                cardScript.Reveal();
        }

        // Tour du joueur (IA simplifiée : pioche si le total est inférieur à celui du croupier)
        // Attention : C'est une IA très basique. Dans un vrai Blackjack, le joueur décide de "Hit" ou "Stand".
        // De plus, la deuxième carte du croupier est généralement face cachée jusqu'à la fin du tour du joueur.
        // Ici, nous comparons avec la main initiale *révélée* du croupier.
        int playerCurrentTotal = GetHandTotal(playerHandValues);
        int dealerInitialTotal = GetHandTotal(dealerHandValues); // Total du croupier après la distribution initiale

        while (playerCurrentTotal < dealerInitialTotal && playerCurrentTotal <= 21)
        {
            Transform nextPlayerSlot = GetNextPlayerSlot(playerHandValues.Count);
            if (nextPlayerSlot == null)
            {
                Debug.LogWarning("Le joueur n'a plus de slots disponibles pour piocher des cartes !");
                break; // Plus de slots prédéfinis pour le joueur
            }

            yield return Spawn(nextPlayerSlot, false);
            if (cardsOnTable[cardsOnTable.Count - 1].TryGetComponent(out BlackjackCard nextPlayerCard))
                nextPlayerCard.Reveal();
            yield return new WaitForSeconds(1.0f); // Délai pour voir la carte piochée

            playerCurrentTotal = GetHandTotal(playerHandValues); // Mettre à jour le total du joueur
        }

        // Vérifier si le joueur a "bust" (dépassé 21) pendant son tour
        if (playerCurrentTotal > 21) {
            DetermineWinner(); // Le joueur a bust, la partie est terminée
            yield break; // Quitter la coroutine, le croupier n'a pas besoin de jouer
        }

        // Dealer Turn: Hit until 17
        // Refactored to use a more flexible slot selection if possible
        while (GetHandTotal(dealerHandValues) < 17)
        {
            Transform nextSlot = GetNextDealerSlot(dealerHandValues.Count);
            if (nextSlot == null) break; // No more slots available

            yield return Spawn(nextSlot, true);
            if (cardsOnTable[cardsOnTable.Count - 1].TryGetComponent(out BlackjackCard nextCard))
                nextCard.Reveal();
            yield return new WaitForSeconds(1.0f);
        }

        DetermineWinner();
    }

    void DetermineWinner()
    {
        int playerTotal = GetHandTotal(playerHandValues);
        int dealerTotal = GetHandTotal(dealerHandValues);
        string resultMessage = "";

        if (playerTotal > 21) resultMessage = "Bust! Perdu.";
        else if (dealerTotal > 21) resultMessage = "Croupier Bust! Gagné !";
        else if (playerTotal > dealerTotal) resultMessage = "Gagné !";
        else if (dealerTotal > playerTotal) resultMessage = "Perdu.";
        else resultMessage = "Égalité !";

        resultMessage += $"\nJoueur: {playerTotal} | Croupier: {dealerTotal}";

        Debug.Log(resultMessage);

        // Mise à jour de l'affichage UI
        if (resultText != null)
        {
            resultText.text = resultMessage;
            resultText.gameObject.SetActive(true);
        }

        // Réinitialisation des triggers pour pouvoir rejouer
        if (tableTrigger != null)
        {
            tableTrigger.UnlockTrigger();
        }

        if (startButton != null)
        {
            startButton.SetActive(true);
        }
    }

    IEnumerator Spawn(Transform slot, bool isDealer)
    {
        if (shuffledDeck.Count > 0)
        {
            GameObject prefab = shuffledDeck[0];
            shuffledDeck.RemoveAt(0);

            GameObject newCard = Instantiate(prefab, slot.position, slot.rotation);
            cardsOnTable.Add(newCard);

            if (newCard.TryGetComponent(out BlackjackCard cardScript))
            {
                if (isDealer) dealerHandValues.Add(cardScript.cardValue);
                else playerHandValues.Add(cardScript.cardValue);
            }
        }
        yield return null;
    }

    Transform GetNextDealerSlot(int currentCount)
    {
        return currentCount switch
        {
            2 => dealerSlot3,
            3 => dealerSlot4,
            _ => null
        };
    }

    Transform GetNextPlayerSlot(int currentCount)
    {
        return currentCount switch
        {
            2 => playerSlot3, // Après les 2 cartes initiales, le prochain slot est le 3
            3 => playerSlot4, // Après 3 cartes, le prochain slot est le 4
            _ => null // Plus de slots prédéfinis disponibles
        };
    }

    void ShuffleDeck()
    {
        shuffledDeck = new List<GameObject>(deckPrefabs);
        for (int i = 0; i < shuffledDeck.Count; i++)
        {
            GameObject temp = shuffledDeck[i];
            int r = Random.Range(i, shuffledDeck.Count);
            shuffledDeck[i] = shuffledDeck[r];
            shuffledDeck[r] = temp;
        }
    }

    int GetHandTotal(List<int> values)
    {
        int total = 0;
        int aces = 0;
        foreach (int v in values)
        {
            if (v == 11) aces++;
            total += v;
        }
        while (total > 21 && aces > 0)
        {
            total -= 10;
            aces--;
        }
        return total;
    }
}