setInterval(function() {
   fetch('http://localhost/api')
      .then((response) => response.json())
      .then((data) => loadAnimal(data));
}, 2000);

function loadAnimal(animals){
   let message = "Aucun";
   if (animals.length > 0){
      document.getElementById('animal').innerText = `${animals[0].animal} (première apparition: ${animals[0].firstSeen})`;
   }
}