




function actualizarDocente() {
    let nombreInput = document.getElementById('txtNombre');
    let apellidoInput = document.getElementById('txtApellido');
    let emailInput = document.getElementById('txtEmail');
    let edadInput = document.getElementById('txtEdad');
    let nivelInput = document.getElementById('txtNivel');
  
    let nombre = nombreInput.value;
    let apellido = apellidoInput.value;
    let email = emailInput.value;
    let edad = parseInt(edadInput.value);
    let nivel = nivelInput.value;
    
  
    if (nombre === "") {
      alert('El campo nombre es obligatorio');
      return false;
    }
    if (apellido === "") {
      alert('El campo apellido es obligatorio');
      return false;
    }
    if (email === "") {
      alert('El campo email es obligatorio');
      return false;
    }
    if (edad <= 0 || edad >= 65 || isNaN(edad)) {
      alert('El docente no puede ser jubilado');
      return false;
    }
    
  
    const PUT_DOCENTE_URL = 'https://localhost:7005/api/docentes/actualizarDocente';
  
    const request = {
      "id":132 ,
      "nombre": nombre,
      "apellido": apellido,
      "email": email,
      "edad": edad
    };
  
    fetch(PUT_DOCENTE_URL, {
      body: JSON.stringify(request),
      method: 'put',
      headers: {
        'Content-Type': 'application/json'
      }
    })
      .then(response => response.json())
      .then(data => {
        if (data.ok) {
          alert('La persona se actualizó correctamente');
          localStorage.setItem('DatoAMostrar', data.listDocentes[0].nombre);
        } else {
          alert(data.mensajeError);
        }
      })
      .catch(err => alert('Error: ' + err));
  }