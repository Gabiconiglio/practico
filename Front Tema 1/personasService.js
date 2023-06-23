const API_PERSONAS_URL = 'https://localhost:7008/api/personas/getPersonas'

function listarPersonas(){
    fetch(API_PERSONAS_URL)
    .then((respuesta) => respuesta.json())
    .then((respuesta) => {
        if(!respuesta.ok){
            alert("Error!!!")
            return;
        }

        const cuerpoTabla = document.querySelector('tbody')

        respuesta.listPersonas.forEach((per) => {
            const fila = document.createElement('tr')
            fila.innerHTML += `<td>${per.id}</td>`
            fila.innerHTML += `<td>${per.nombre}</td>`
            fila.innerHTML += `<td>${per.apellido}</td>`

            cuerpoTabla.append(fila)
        });
    }).catch((err) => {
        alert("Algo salio mal!!")
    })
}