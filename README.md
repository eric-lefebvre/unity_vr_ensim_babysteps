<!--Rappel push 07/02 sur la distance de grab. Le paramètre c'est XR Interaction Setup/XR Origin/Left Controller/Ray Interactor/XR Ray Interactor/Raycast Configuration/Max Raycast Distance-->

# Baby Steps

<p float="left">
    <img src="https://img.shields.io/badge/Unity-100000?style=for-the-badge&logo=unity&logoColor=white">
    <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white">
</p>

Un jeu en réalité virtuelle où vous incarnez un enfant dans une garderie transformée en centre de test, guidé par une intelligence artificielle.

## Captures d'écran

<p float="left">
  <img src="screenshots/tableau2.png" width="400" alt="Deuxième mini-jeu, où il faut trouver des cubes lettres et les placer dans une slot machine">
  <img src="screenshots/backroom_walls.png" width="400" alt="Seconde salle, similaire à des backrooms, dans un cul de sac avec le mot down écrit en rouge">
</p>

<p align="center">
    <img src="screenshots/whack_a_mole.gif" width="400" alt="Séquence tape-taupe">
</p>

## Fonctionnalités

- Deux salles
- Cinq mini-jeux

## Structure du dépôt

```
.
├── Assets
│   ├── Animations  # Animation controllers et fichiers d'animations
│   ├── AssetStore  # Ressources provenant du Unity Asset Store
│   ├── Audio       # Sons et paramètres du mixer
│   ├── Fonts
│   ├── GameEvents  # Évènements utilisés dans l'éditeur et dans le code 
│   ├── Imports     # Fichiers fbx
│   ├── Lighting    # Fichiers concernant la veilleuse (textures)
│   ├── Materials   # Fichiers matériaux définissant l'apparence des modèles
│   ├── Prefabs
│   ├── Samples     # Répertoire fourni par XR Interaction Toolkit
│   ├── Scenes
│   ├── _Scripts
│   ├── Settings    # Fichiers paramètres pour la pipeline 3D
│   ├── TextMesh Pro
│   ├── Textures    # Images pour les modèles
│   ├── XR          # Fichiers paramètres pour la lib XR
│   └── XRI         # Fichiers paramètres pour la lib XR Interaction
├── Packages        # Fichiers répertoriant les dépendances utilisées
├── ProjectSettings # Fichiers paramètres du projet
├── README.md
└── screenshots     # Captures d'écran dans le README
```

## Exécuter localement

Clonez le projet

```bash
  git clone https://github.com/pgmtx/reamix_2025/
```

Puis ouvrez le projet sur Unity Hub avec l'éditeur en version **2022.3.9f1**
