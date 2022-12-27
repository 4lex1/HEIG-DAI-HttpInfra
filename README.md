# HEIG-DAI-HttpInfra

## Etape 1
Pour l'étape 1, la branche est fb-apache-static
Le contenu de cette étape se trouve dans le dossier /docker-images/apache-php-image.
Le Dockerfile est relativement simple :
```
FROM php:7.2-apache

COPY content/ /var/www/html/
```

La première ligne "FROM..." permet de se baser sur une image de PHP compatible pour tourner sur un serveur Apache.
La seconde "COPY ..." va copier le contenu du dossier 'content' de la machine qui va construire le container dans le dossier '/var/www/html' du container.

Ce dossier '/var/www/html' est le dossier de base du site web pour le serveur Apache. Celui-ci, lorsque l'on demande le site web, va automatiquement rechercher une page d'index (dans notre cas index.html) et la servir au client.

Le serveur Apache tourne sur le port 80 et est mappé depuis l'extérieur via le port 9090. Ce mappage est fait dans le script qui permet de lancer le container : 'run-container.sh'.

Les fichiers de configuration du serveur Apache se trouvent dans /etc/apache2

#### Utilisation
1) Cloner le repository
2) Se rendre dans le dossier /apache-php-image.
3) Construire le container en exécutant le script 'build-image.sh'.
4) Lancer le container en exécutant le script 'run-container.sh'.
5) Tester le fonctionnement en accédant au container via un navigateur à l'adresse http://<ip>:9090

## Etape 2
Pour l'étape 2, la branche est fb-express-dynamic.
Le contenu de cette étape se trouve dans le dossier /docker-images/express-image.
Le Dockerfile est le suivant :
```
FROM node:18.12

COPY src /opt/app

CMD ["node", "/opt/app/index.js"]
```

L'instruction FROM charge une image de Node avec la dernière version LTS actuelle: 18.12.
L'instruction COPY permet de copier le contenu du dossier 'src' de la machine qui va construire le container dans le dossier /opt/app du container.
Finalement, l'instruction CMD permet d'indiquer au container que, lors de son exécution, il doit démarrer node avec comme paramètre le fichier index.js se trouvant dans le dossier de l'application.

Le serveur node fonctionne avec le framework express.js afin d'effectuer le routage. L'ensemble du code se trouve dans le fichier index.js.

Avec ce code, nous créeons un serveur web qui écoute sur le port 3000 et donc l'unique fonction est de rendre un tableau aléatoire d'animaux lorsqu'une requête est envoyée à la racine du serveur web (c'est à dire le endpoint '/').

Le tableau d'animal contient la structure suivante :
```
{
   animal: "nom de l'animal",
   type: "type de l'animal, par exemple zoo ou ocean",
   firstSeen: "année à laquelle l'animal a été observé pour la première fois"
}
```

Comme indiqué, le serveur écoute sur le port 3000. Cependant, l'image docker est préparée de manière à effectuer un mappage entre le port local 9090 et le port 3000 du container. Ainsi, pour ouvrir la page, il est nécessaire d'envoyer une requête sur le port 9090.

#### Utilisation
1) Cloner le repository
2) Se rendre dans le dossier /express-image.
3) Construire le container en exécutant le script 'build-image.sh'.
4) Lancer le container en exécutant le script 'run-container.sh'.
5) Tester le fonctionnement en accédant au container via un navigateur à l'adresse http://<ip>:9090
6) (Optionnel) : utiliser Postman pour tester le fonctionnement en envoyant un GET à http://<ip>:9090