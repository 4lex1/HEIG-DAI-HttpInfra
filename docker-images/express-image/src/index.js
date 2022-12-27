var Chance = require('chance');
var chance = new Chance();

var express = require('express');
var app = express();

app.get('/', function(req, res) {
   res.send(generateAnimals());
});

app.listen(3000, function() {
   console.log('Accepting HTTP requests on port 3000');
});

function generateAnimals(){
   var numberOfAnimals = chance.integer({min:5, max:20});
   var animalTypes = ["zoo", "ocean", "desert", "grassland", "forest", "farm", "pet"];
   
   var animals = [];
   for (var i = 0; i < numberOfAnimals; i++){
      var type = animalTypes[chance.integer({min:0, max:animalTypes.length -1})];
      animals.push({
         animal: chance.animal({type: type}),
         type: type,
         firstSeen: chance.year({min:1500, max:2023})
      });
   }
   
   return animals;
}