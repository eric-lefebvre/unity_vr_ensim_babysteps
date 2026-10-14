
# Baby Steps

<p float="left">
    <img src="https://img.shields.io/badge/Unity-100000?style=for-the-badge&logo=unity&logoColor=white">
    <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white">
    <img src="https://img.shields.io/badge/Blender-E87D0D?style=for-the-badge&logo=blender&logoColor=white">
</p>

Un jeu en réalité virtuelle où vous incarnez un enfant dans une garderie transformée en centre de test, guidé par une intelligence artificielle.

Projet de 4e année réalisé à l'ENSIM dans le cadre de **RéaMix 2025-26** (Réalité Mixte pour la Formation et l'Ingénierie), par une équipe de six étudiants. Le rapport complet est disponible : [rapport_fin_projet.pdf](rapport_fin_projet.pdf).

## Captures d'écran

<p float="left">
  <img src="screenshots/tableau2.png" width="400" alt="Deuxième mini-jeu, où il faut trouver des cubes lettres et les placer dans une slot machine">
  <img src="screenshots/backroom_walls.png" width="400" alt="Seconde salle, similaire à des backrooms, dans un cul de sac avec le mot down écrit en rouge">
</p>

<p align="center">
    <img src="screenshots/whack_a_mole.gif" width="400" alt="Séquence tape-taupe">
</p>

## But du jeu

Baby Steps est un **jeu d'initiation à la réalité virtuelle**. Il s'adresse à des joueurs qui n'ont jamais (ou presque) porté de casque : chaque salle introduit une mécanique de base de la VR (se déplacer, attraper, lancer, manipuler, résoudre une énigme simple).

Le thème enfantin découle de cet objectif : comme le joueur apprend à utiliser son nouveau « corps virtuel », il incarne un tout-petit qui fait ses premiers pas.

### Scénario

Le joueur se réveille dans une série de chambres d'enfant qui sont en réalité des **salles de test**. Une IA, qui s'exprime par la voix et à travers une télévision, le guide d'épreuve en épreuve pour évaluer ses capacités cognitives, physiques et motrices. Son véritable but est de rendre les enfants obéissants. Le joueur n'est pas le seul enfant qu'elle entraîne, et un enfant trop désobéissant pourrait bien finir par la déranger. Libre au joueur d'écouter ses ordres ou non.

Les inspirations principales sont *Portal* (salles de test de plus en plus complexes), *The Stanley Parable* (narrateur omniscient, illusion du choix) et l'esthétique des *Backrooms*.

### Déroulement

**Salle 1 : la garderie**

1. **Le parc pour bébé** : le joueur apparaît dans un berceau et doit trouver comment ouvrir la porte coulissante pour en sortir.
2. **Lancer de balles** : des balles sortent une à une d'un tuyau dans le mur, il faut en lancer un certain nombre dans un panier.
3. **Slot machine** : en actionnant la manivelle, un mot aléatoire s'affiche sur les roues. Le joueur doit retrouver les cubes-lettres éparpillés dans la salle et les insérer dans la machine pour épeler le mot, ce qui déverrouille la porte suivante.
4. **Whack-a-Mole** : le marteau attend sur une petite table. Il faut taper les taupes qui sortent au hasard jusqu'à atteindre le score demandé.

**Salle 2 : les Backrooms**

5. **Le labyrinthe** : couloirs jaunes répétitifs, éclairage blafard, aucune instruction. Le joueur doit trouver la sortie par lui-même.
6. **L'ascenseur** : il mène à un étage lumineux et coloré, une aire de jeux pour enfants qui tranche avec le labyrinthe.
7. **Simon Says et Black Jack** : derniers jeux de la démo. Pour le Simon Says, le joueur reproduit une séquence de couleurs et de sons dans le bon ordre.

## Fonctionnalités

- Deux salles aux ambiances opposées : une garderie colorée et un labyrinthe inspiré des Backrooms
- Mini-jeux : lancer de balles, slot machine, Whack-a-Mole, Simon Says, Black Jack
- Déplacement fluide au joystick avec rotation par angle (*snap turn*) pour limiter le *motion sickness*
- Objets attrapables sous n'importe quel angle (sauf certains, comme le marteau, qui ont une prise prédéfinie)
- Assistance à l'insertion pour la slot machine : un hologramme montre l'emplacement et le cube s'y aimante (*snap*) une fois relâché
- Menus intégrés au monde 3D, utilisables avec un rayon depuis la manette, avec occultation de l'environnement (*fog of war*) pendant leur utilisation. Les boutons pressés tombent au sol et deviennent des objets du décor.
- Retours visuels (surbrillance des objets interactifs), sonores (sons 2D et 3D spatialisés) et haptiques (vibrations des manettes)
- Dialogues de l'IA déclenchés par les événements de jeu
- Tutoriels animés montrant les boutons de manette à utiliser et dessins explicatifs placés dans la salle

## Assets créés

Tous les modèles du jeu ont été **modélisés par l'équipe sous Blender**, les modèles disponibles en ligne ne correspondant pas à la direction artistique voulue. Le style est mi-cartoon, mi-réaliste : des couleurs vives pour l'aspect enfantin et des objets importants faciles à repérer, des proportions et un éclairage réalistes pour ne pas briser l'immersion. Les modèles sont volontairement *low-poly* pour tourner sur un casque autonome.

### Modèles 3D

Les fichiers `.fbx` sont dans [Assets/Imports](Assets/Imports), et les prefabs correspondants dans [Assets/Prefabs](Assets/Prefabs).

| Catégorie | Modèles |
| --- | --- |
| Personnage | Personnage principal (avatar humanoïde *low-poly*), mains du joueur |
| Bâtiment | Charpente du toit, plinthes, portes de séparation entre les salles (animées) |
| Salle 1 : mobilier | Berceau à porte coulissante, chaise en bois, chaise en plastique, table, table basse, bibliothèque, étagère, pupitre, canapé pour bébé, tableau |
| Salle 1 : jouets et déco | Balles de jonglage, planétarium (animé), veilleuse (animée), télévision (visage de l'IA) |
| Salle 1 : mini-jeux | Tuyau distributeur de balles et panier, slot machine avec manivelle, 26 cubes-lettres (A à Z), Whack-a-Mole (animé) avec ses taupes et son marteau |
| Salle 2 | Labyrinthe en blocs modulaires, ascenseur, escalier en colimaçon, barrière et dalles d'étage, lampes (enfant, plafond), tapis pour enfant, chaise excentrique |
| Salle 2 : mini-jeux | Table de Black Jack |
| Menus | Manettes VR animées (tutoriels), panneau de menu en carton |

### Animations

Dans [Assets/Animations](Assets/Animations) : portes, ascenseur, taupes du Whack-a-Mole, planétarium, veilleuse, mains, personnage et manettes VR des tutoriels. Les animations des portes et mécanismes ont été créées dans Blender puis importées dans Unity.

### Textures et matériaux

- Les matériaux Blender ont été **bakés en images** avant l'export, car Unity n'interprète pas les nœuds procéduraux de Blender (sans ça, les matériaux arrivaient manquants ou saturés)
- Dessins explicatifs affichés sur les murs pour guider le joueur ([Assets/Textures/Dessins](Assets/Textures/Dessins))
- Tags et inscriptions sur les murs du labyrinthe ([Assets/Textures/TagsMurs](Assets/Textures/TagsMurs))
- Écrans de la télévision ([Assets/Textures/TV](Assets/Textures/TV)), textures du bâtiment et des planètes
- Éclairage précalculé (*baked lighting*) pour la deuxième salle

### Son

- Dialogues de l'IA écrits par l'équipe et interprétés par Maya Hamdy ([Assets/Audio/Son/Dialogues](Assets/Audio/Son/Dialogues))
- Bruitages (bruits de pas, rebonds, « bonk » du marteau, « boop » à la saisie…) issus de banques de sons libres de droits

### Assets externes

Quelques ressources ne viennent pas de l'équipe et sont regroupées dans [Assets/AssetStore](Assets/AssetStore) :

- *Playing Cards* (cartes du Black Jack)
- *Low Poly Kids Playground*

Les dossiers `Samples`, `TextMesh Pro`, `XR` et `XRI` sont fournis par Unity et le XR Interaction Toolkit.

## Architecture technique

- **Moteur** : Unity 2022.3.9f1 avec XR Interaction Toolkit 2.5.4, OpenXR et Oculus XR
- **Casque** : Meta Quest, testé via Meta Quest Link
- **Système d'événements** : le jeu repose sur le patron observateur. Des `GameEvent` (ScriptableObjects dans [Assets/GameEvents](Assets/GameEvents)) sont déclenchés par les objets et écoutés par des `GameEventListener`. Par exemple, le panier émet « balle dans panier » sans connaître la télévision ni la porte, qui réagissent chacune de leur côté. Cela a permis à chacun de travailler en boîte noire, puis de brancher les dialogues sur les événements existants en quelques clics.
- **AudioSystem** : un objet unique centralise tous les sons et leurs paramètres (clip, volume, pitch).
- **Prefabs** : chaque objet est transformé en prefab pour limiter les modifications du fichier de scène et donc les conflits Git.

Les scripts sont dans [Assets/_Scripts](Assets/_Scripts), avec un dossier par mini-jeu ou système.

## Structure du dépôt

```
.
├── Assets
│   ├── Animations  # Animation controllers et fichiers d'animations
│   ├── AssetStore  # Ressources provenant du Unity Asset Store
│   ├── Audio       # Sons, dialogues et paramètres du mixer
│   ├── Fonts
│   ├── GameEvents  # Évènements utilisés dans l'éditeur et dans le code
│   ├── Imports     # Modèles fbx créés sous Blender
│   ├── Lighting    # Fichiers concernant la veilleuse (textures)
│   ├── Materials   # Fichiers matériaux définissant l'apparence des modèles
│   ├── Prefabs
│   ├── Samples     # Répertoire fourni par XR Interaction Toolkit
│   ├── Scenes      # Salle1 et Salle2
│   ├── _Scripts    # Scripts C# (un dossier par mini-jeu / système)
│   ├── Settings    # Fichiers paramètres pour la pipeline 3D
│   ├── TextMesh Pro
│   ├── Textures    # Images pour les modèles
│   ├── XR          # Fichiers paramètres pour la lib XR
│   └── XRI         # Fichiers paramètres pour la lib XR Interaction
├── Packages        # Fichiers répertoriant les dépendances utilisées
├── ProjectSettings # Fichiers paramètres du projet
├── README.md
├── rapport_fin_projet.pdf
└── screenshots     # Captures d'écran dans le README
```

## Exécuter localement

Clonez le projet

```bash
  git clone https://github.com/eric-lefebvre/unity_vr_ensim_babysteps.git
```

Puis ouvrez le projet sur Unity Hub avec l'éditeur en version **2022.3.9f1**. Lancez la scène `Assets/Scenes/Salle1.unity` avec un casque Meta Quest connecté via Meta Quest Link.

> En cas d'erreurs de compilation à l'ouverture, supprimez le dossier `Library` et laissez Unity le régénérer.

## Équipe

Eric-Nicolas Brieau, Maya Hamdy, Eric Lefebvre, Clément Moquereau, Matéo Olivier, Enzo Sarkissian

Tuteurs : Madeth May, Corentin Coupry (Le Mans Université)
