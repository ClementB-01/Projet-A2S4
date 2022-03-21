# Projet-A2S4
## Traitement d'image BMP

### Structure de données
Dans le cadre de ce projet, nous avons créé trois classes en plus de la classe Program, et ajouté les classes correspondant à la correction d’erreur du QR code. 

Ces trois classes sont :
1. La classe Pixel qui est un type évolué ayant trois attributs, une composante rouge, une composante verte et une composante bleue. 
Ce type permettra ensuite de manipuler des matrices de pixels, sans avoir de matrices à trois dimensions et facilite la lisibilité du code.

2. La classe MyImage, qui possède comme attributs, le chemin d’accès à l’image que l’on souhaite traiter, son type (nous ne nous occupons que des images bitmap), un tableau de byte correspondant à l’intégralité des données de l’image, sa taille totale, la taille de son en-tête, sa hauteur et sa largeur, ainsi que l’étendue de sa gamme de couleur. On crée également un attribut qui sera la matrice de pixels évoquées plus tôt et qui servira dans tous les traitements ultérieurs à accéder et modifier les composantes de couleurs de l’image.

3. La classe QrCode, séparée par soucis de lisibilité et de meilleure compréhension du code, on estimait sinon que la classe MyImage perdrait en clarté. 
 Cette classe regroupe toutes les méthodes nécessaires à la création d’un QrCode mais n’a pas d’attributs spécifiques.
### Innovations
Pour répondre à la demande d’innovations, nous avons créé deux méthodes additionnelles dans la classe MyImage : Négatif et Luminosité. Comme leurs noms l’indique, ces méthodes permettent de voir le négatif d’une image et de modifier, à la hausse ou bien à la baisse la luminosité d’une image.
### Problèmes rencontrés
Nous n’avons pas rencontré de problèmes majeurs au cours de ce projet, cependant sur quelques étapes nous avons perdus du temps car nous ne parvenions pas toujours à trouver la source de nos erreurs, notamment lors de la manipulation d’objet complexes tels les fractales ou bien la stéganographie. Le QR code a demandé une réflexion poussée et un temps de développement long car il s’agissait de comprendre la construction d’une image complexe sans support de base.
