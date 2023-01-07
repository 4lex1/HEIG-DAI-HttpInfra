setInterval(function() {
   fetch('/api')
      .then((response) => response.json())
      .then((data) => loadAnimal(data));
}, 2000);

function loadAnimal(animals){
   let message = "Aucun";   
   if (animals.length > 0){
      message = `${animals[0].animal} (première apparition: ${animals[0].firstSeen})`;
   }
   document.getElementById('animal').innerText = message;
}