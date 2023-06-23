function guardarPersona(){
    let txtNombre = document.getElementById('txtNombre')
    let txtApellido = document.getElementById('txtApellido')
    let txtIdCategoria = document.getElementById('txtCategoria')

    if(txtNombre.value === ""){
        alert("El nombre es obligatorio")
        return false
    }

    if(txtApellido.value === ""){
        alert("El apellido es obligatorio")
        return false
    }

    if(txtIdCategoria.value === ""){
        alert("El id de la categoria es oblitaoria")
        return false
    }

    const url = 'https://localhost:7008/api/personas/postNuevaPersona'

    const request = {
        "nombre": txtNombre.value,
        "apellido": txtApellido.value,
        "idCategoria": txtIdCategoria.value
    }

    fetch(url,{
        body: JSON.stringify(request),
        method: 'post',
        headers: {
            'Content-Type':'application/json'
        }
    })
    .then(respuesta => respuesta.json())
    .then(respuesta => {
        if(respuesta.ok){
            alert("Persona agregada con éxito")
            localStorage.setItem("datoAMostrar", respuesta.listPersonas[0].nombre)
            window.location.replace('nuevo.html')
        }
        else{
            alert(respuesta.mensajeError)
        }
    })
    .catch(err => alert("ERROR: " + err))
}