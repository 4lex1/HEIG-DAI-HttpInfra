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

Les fichiers de configuration du serveur Apache se trouvent dans /etc/apache2 (sur la VM du container)

#### Utilisation
1) Cloner le repository (branche: fb-apache-static)
2) Se rendre dans le dossier /docker-images/apache-php-image.
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

Le tableau d'animaux contient la structure suivante :
```
{
   animal: "nom de l'animal",
   type: "type de l'animal, par exemple zoo ou ocean",
   firstSeen: "année à laquelle l'animal a été observé pour la première fois"
}
```

Comme indiqué, le serveur écoute sur le port 3000. Cependant, le script de lancement est préparé de manière à effectuer un mappage entre le port local 9090 et le port 3000 du container. Ainsi, pour ouvrir la page, il est nécessaire d'envoyer une requête sur le port 9090.

#### Utilisation
1) Cloner le repository (branche: fb-express-dynamic)
2) Se rendre dans le dossier /docker-images/express-image.
3) Construire le container en exécutant le script 'build-image.sh'.
4) Lancer le container en exécutant le script 'run-container.sh'.
5) Tester le fonctionnement en accédant au container via un navigateur à l'adresse http://<ip>:9090
6) (Optionnel) : utiliser Postman pour tester le fonctionnement en envoyant un GET à http://<ip>:9090

## Etape 3
Pour l'étape 3, la branche est fb-compose.
Cette étape rajoute le fichier /docker-images/docker-compose.yml.

Ce fichier permet de lancer simultanément le serveur statique (apache) et le serveur dynamique (node+express).

- Le serveur statique est mappé sur le port 9090
- Le serveur dynamique est mappé sur le port 9091

#### Utilisation
1) Cloner le repository (branche: fb-compose)
2) Se rendre dans le dossier /docker-images.
3) Exécuter le script 'run-compose.sh'.
4) Tester le fonctionnement en accédant au serveur statique via un navigateur à l'adresse http://<ip>:9090 et au serveur dynamique via un navigateur à l'adresse http://<ip>:9091

## Etape 3a
Pour l'étape 3, la branche est fb-proxy.
Dans cette étape, nous avons simplement modifié le fichier /docker-images/docker-compose.yml.

Cette fois-ci, deux instances de chaque serveur sont lancées ainsi qu'un reverse-proxy (Traefik).

Les points importants sont les suivants :
- "deploy:" suivi de "replicas: 2" indique à compose de lancer 2 instances.
- les "labels" sont utilisés par Traefik pour gérer les règles à appliquer aux containers.
- le label "traefik.http.routers.static.rule=Host(`localhost`)" crée une règle pour le serveur "static" et indique que celui-ci se trouve au endpoint 'localhost'.

- les labels :
- - "traefik.http.routers.dynamic.rule=Host(`localhost`) && PathPrefix(`/api`)"
- - "traefik.http.middlewares.dynamic-strip.stripprefix.prefixes=/api"
- - "traefik.http.routers.dynamic.middlewares=dynamic-strip@docker"

permettent, respectivement, de configurer la route du serveur dynamique vers le endpoint "localhost/api" puis de créer un middleware dont la fonction est de retirer le '/api' de l'URL (car le serveur n'est pas codé pour traiter ce qui vient sur le endpoint '/api' mais sur le endpoint '/') et finalement d'appliquer ce middleware au site dynamic.

Finalement, nous avons aussi ajouté un log dans le index.js du site dynamic afin, grâce à compose, de déterminer quelle instance du serveur a répondu.
Ainsi, on constate très rapidement en ouvrant n'importe lequel des deux sites et en rafraichissant la page plusieurs fois que chaque instance répond à tour de rôle et donc que le load balacing est bien configuré.

#### Utilisation
1) Cloner le repository (branche: fb-proxy)
2) Se rendre dans le dossier /docker-images.
3) Exécuter le script 'run-compose.sh'.
4) Tester le fonctionnement en accédant au serveur statique via un navigateur à l'adresse http://localhost et au serveur dynamique via un navigateur à l'adresse http://localhost/api
5) (Optionnel) Tester le load balacing en rafraichissant la page et en observant sur le terminal quel instance a traité la requête.