using System.Collections;
using UnityEngine;

public class WheelRotation : MonoBehaviour
{
    [Header("Roues (Assigner les pivots dans l'Inspector)")]
    public Transform wheel1;
    public Transform wheel2;
    public Transform wheel3;
    public Transform wheel4;

    [Header("Tableaux de Cubes (Remplis automatiquement)")]
    private Transform[] cubes1 = new Transform[8];
    private Transform[] cubes2 = new Transform[8];
    private Transform[] cubes3 = new Transform[8];
    private Transform[] cubes4 = new Transform[8];

    [Header("Rotation Settings")]
    public float rotationStep = 45f;
    public float rotationDuration = 1f;

    [Header("Levier Casino")]
    public Transform handleAnchor;
    public float thresholdAngle = 55f;

    [Header("Event")]
    public GameEvent motFini;

    public string word;
    private bool hasPulled = false;
    private int currentIndexWheel1 = 0;
    private int currentIndexWheel2 = 0;
    private int currentIndexWheel3 = 0;
    private int currentIndexWheel4 = 0;
    private int counter = 0;
    private bool isRotating = false;

    public void Start()
    {
        // Initialisation des tableaux au démarrage
        FillCubeArray(wheel1, cubes1, 1);
        FillCubeArray(wheel2, cubes2, 2);
        FillCubeArray(wheel3, cubes3, 3);
        FillCubeArray(wheel4, cubes4, 4);

        RotateStep();
    }

    private void FillCubeArray(Transform wheel, Transform[] array, int wheelIndex)
    {
        if (wheel == null) return;

        Transform[] allChildren = wheel.GetComponentsInChildren<Transform>();

        foreach (Transform child in allChildren)
        {
            string name = child.name; // Exemple attendu: "Wheel0_Cubes1R"

            // On vérifie que le nom commence par WheelN_Cube
            if (name.StartsWith("Wheel" + wheelIndex + "_Cube"))
            {
                // On extrait le chiffre juste après "_Cube" (longueur de "_Cube" est 5)
                int startIndex = name.IndexOf("_Cube") + 5;

                if (startIndex < name.Length && char.IsDigit(name[startIndex]))
                {
                    int cubePos = (int)char.GetNumericValue(name[startIndex]);

                    if (cubePos < array.Length)
                    {
                        array[cubePos] = child;
                        // Debug.Log($"Roue {wheelIndex} : Cube {cubePos} assigné ({child.name})");
                    }
                }
            }
        }

        // Vérification de sécurité pour voir si le tableau est complet
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == null)
                Debug.LogWarning($"Attention : Emplacement {i} vide pour la roue {wheelIndex}. Vérifie le nommage.");
        }
    }

    public void RotateStep()
    {
        if (!isRotating)
        {
            StartCoroutine(RotateToWord());
        }
    }

    private IEnumerator RotateToWord()
    {
        isRotating = true;
        string[] listeWords = { "MOOO", "MEOW", "JHIN", "BARK", "WOOF", "WICK", "CROA", "OINK", "ROAR", "PEEP" };
        word = listeWords[Random.Range(0, listeWords.Length)];
        Debug.Log("<color=green>Cible : " + word + "</color>");

        counter = 0;

        Coroutine[] routines = new Coroutine[4];
        for (int i = 0; i < 4; i++)
        {
            routines[i] = StartCoroutine(RotateSingleWheelToLetter(i, word[i]));
        }

        foreach (var r in routines) yield return r;
        RecalculateCounter();
        isRotating = false;
    }

    public void RecalculateCounter()
    {
        counter = 0;
        SnapCube[] snapCubes = FindObjectsOfType<SnapCube>();

        foreach (var snap in snapCubes)
        {
            if (snap.snappedObject != null)
            {
                string name = snap.snappedObject.name;
                char letterOnCube = name[name.Length - 1];

                int slotIndex = (int)char.GetNumericValue(snap.gameObject.name[snap.gameObject.name.Length - 1]);

                if (slotIndex >= 0 && slotIndex < word.Length)
                {
                    if (char.ToUpper(word[slotIndex]) == char.ToUpper(letterOnCube))
                    {
                        counter++;
                    }
                }
            }
        }

        if (counter == 4)
        {
            if (motFini != null) motFini.TriggerEvent();
            Debug.Log("<color=cyan>YOU WON GG !</color>");
        }
    }

    private IEnumerator RotateSingleWheelToLetter(int wheelIndex, char targetLetter)
    {
        Transform wheel = GetWheel(wheelIndex);
        Transform[] cubes = GetCubes(wheelIndex);
        int targetIndex = -1;

        // Recherche du cube qui porte la lettre à la FIN de son nom
        for (int j = 0; j < cubes.Length; j++)
        {
            if (cubes[j] != null)
            {
                string cubeName = cubes[j].name;
                char letterOnCube = cubeName[cubeName.Length - 1];

                if (char.ToUpper(letterOnCube) == char.ToUpper(targetLetter))
                {
                    targetIndex = j;
                    break;
                }
            }
        }

        if (targetIndex == -1)
        {
            Debug.LogError($"Lettre '{targetLetter}' non trouvée sur la roue {wheelIndex}");
            yield break;
        }

        int currentIndex = GetCurrentIndexForWheel(wheelIndex);

        // Rotation cran par cran jusqu'à l'index cible
        while (currentIndex != targetIndex)
        {
            Quaternion start = wheel.localRotation;
            Quaternion end = start * Quaternion.Euler(0, rotationStep, 0);

            float elapsed = 0;
            float stepDuration = rotationDuration / 4f;

            while (elapsed < stepDuration)
            {
                wheel.localRotation = Quaternion.Slerp(start, end, elapsed / stepDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            wheel.localRotation = end;

            currentIndex = (currentIndex + 1) % 8;
            SetCurrentIndexForWheel(wheelIndex, currentIndex);
        }
    }

    // --- Helpers pour gérer les index et les accès aux roues ---

    private int GetCurrentIndexForWheel(int wheelIndex)
    {
        switch (wheelIndex)
        {
            case 0: return currentIndexWheel1;
            case 1: return currentIndexWheel2;
            case 2: return currentIndexWheel3;
            case 3: return currentIndexWheel4;
            default: return 0;
        }
    }

    private void SetCurrentIndexForWheel(int wheelIndex, int value)
    {
        switch (wheelIndex)
        {
            case 0: currentIndexWheel1 = value; break;
            case 1: currentIndexWheel2 = value; break;
            case 2: currentIndexWheel3 = value; break;
            case 3: currentIndexWheel4 = value; break;
        }
    }

    private Transform[] GetCubes(int index)
    {
        if (index == 0) return cubes1;
        if (index == 1) return cubes2;
        if (index == 2) return cubes3;
        return cubes4;
    }

    private Transform GetWheel(int index)
    {
        if (index == 0) return wheel1;
        if (index == 1) return wheel2;
        if (index == 2) return wheel3;
        return wheel4;
    }

    void Update()
    {
        if (handleAnchor != null)
        {
            float currentAngle = handleAnchor.localEulerAngles.z; // On vérifie l'axe X selon tes nouveaux réglages
            if (currentAngle > 180) currentAngle -= 360;
            if (currentAngle >= 50f && !hasPulled && !isRotating)
            {
                hasPulled = true;
                RotateStep();
            }

            if (hasPulled && Mathf.Abs(currentAngle) < 5f)
            {
                hasPulled = false;
            }
        }
    }

    public void CheckIfWon(char lettre, int slot, bool removed)
    {
        if (removed)
            Debug.Log("lettre removed :" + lettre);
        else
            Debug.Log("lettre placed :" + lettre);
        Debug.Log("score : " + counter);
        if (slot < 0 || slot >= word.Length) return;

        // On compare en majuscule pour éviter les erreurs de casse
        if (char.ToUpper(word[slot]) == char.ToUpper(lettre))
        {
            if (removed) counter--;
            else counter++;
        }

        if (counter == 4)
        {
            if (motFini != null) motFini.TriggerEvent();
            Debug.Log("<color=cyan>YOU WON GG !</color>");
        }
    }
}