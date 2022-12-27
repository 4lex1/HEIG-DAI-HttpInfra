# HEIG-DAI-HttpInfra

## Etape 1
Pour l'étape 1, la branche est fb-apache-static

Le contenu de cette étape se trouve dans le dossier /apache-php-image.

Le Dockerfile est relativement simple :
```
FROM php:7.2-apache

COPY content/ /var/www/html/
```

La première ligne "FROM..." permet de se baser sur une image de PHP compatible pour tourner sur un serveur Apache.
La seconde va copier le contenu du dossier 'content' de la machine qui va construire le container dans le dossier '/var/www/html' du container.

Ce dossier '/var/www/html' est le dossier de base du site web pour le serveur Apache. Celui-ci, lorsque l'on demande le site web, va automatiquement rechercher une page d'index (dans notre cas index.html) et la servir au client.

Le serveur Apache tourne sur le port 80 et est mappé depuis l'extérieur via le port 9090. Ce mappage est fait dans le script qui permet de lancer le container : 'run-container.sh'.

Les fichiers de configuration du serveur Apache se trouvent dans /etc/apache2

#### Utilisation
1) Cloner le repository
2) Se rendre dans le dossier /apache-php-image.
3) Construire le container en exécutant le script 'build-image.sh'.
4) Lancer le container en exécutant le script 'run-container.sh'.
5) Tester le fonctionnement en accédant au container via un navigateur à l'adresse http://<ip>:9090