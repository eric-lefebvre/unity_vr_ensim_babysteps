using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SimonManager : MonoBehaviour
{
    [Header("UI Setup")]
    public GameObject startButton;

    [Header("Buttons Setup")]
    public GameObject[] gameButtons;
    public Color flashColor = Color.white;

    [Header("Audio Setup")]
    public AudioSource gameSpeaker;   // Drag one AudioSource here (the "Speaker")
    public AudioClip[] buttonSounds; // NOW you can drag your 4 sound files here
    public AudioClip failSound;      // Optional: Drag your fail sound file here

    private List<int> gameSequence = new List<int>();
    private int playerStep = 0;
    private bool isInputEnabled = false;

    // ... (Keep StartGame, AddNextStep, PlaySequence the same) ...

    public void StartGame()
    {
        if (startButton != null) startButton.SetActive(false);
        gameSequence.Clear();
        playerStep = 0;
        AddNextStep();
    }

    void AddNextStep()
    {
        gameSequence.Add(Random.Range(0, gameButtons.Length));
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        isInputEnabled = false;
        yield return new WaitForSeconds(1f);

        foreach (int index in gameSequence)
        {
            FlashButton(index);
            yield return new WaitForSeconds(0.8f);
        }

        isInputEnabled = true;
        playerStep = 0;
    }

    public void PlayerPressedButton(int buttonID)
    {
        if (!isInputEnabled) return;
        isInputEnabled = false;

        FlashButton(buttonID);

        if (buttonID == gameSequence[playerStep])
        {
            playerStep++;
            if (playerStep == gameSequence.Count)
            {
                Invoke("AddNextStep", 1.2f);
            }
            else
            {
                StartCoroutine(ContinueInputDelay());
            }
        }
        else
        {
            if (gameSpeaker != null && failSound != null) gameSpeaker.PlayOneShot(failSound);
            GameOver();
        }
    }

    IEnumerator ContinueInputDelay()
    {
        yield return new WaitForSeconds(0.2f);
        isInputEnabled = true;
    }

    void GameOver()
    {
        gameSequence.Clear();
        isInputEnabled = false;
        if (startButton != null) startButton.SetActive(true);
    }

    void FlashButton(int index)
    {
        if (index >= 0 && index < gameButtons.Length)
        {
            StartCoroutine(FlashEffect(index));

            // This is how we play the clips now
            if (gameSpeaker != null && buttonSounds != null && index < buttonSounds.Length)
            {
                if (buttonSounds[index] != null)
                {
                    gameSpeaker.PlayOneShot(buttonSounds[index]);
                }
            }
        }
    }

    IEnumerator FlashEffect(int index)
    {
        Renderer rend = gameButtons[index].GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.EnableKeyword("_EMISSION");
            rend.material.SetColor("_EmissionColor", flashColor * 2f);
            yield return new WaitForSeconds(0.4f);
            rend.material.SetColor("_EmissionColor", Color.black);
        }
    }
}