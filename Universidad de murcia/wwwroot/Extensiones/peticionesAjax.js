
function procesarRespuesta(respuesta)
{
    console.log(respuesta);
}

function realizarPeticion(url, funcionDeRespuesta)
{

    function respuestaCorrecta(){
        if (req.status>=200 && req.status<400){
            funcionDeRespuesta(req.responseText);
        }
        else {
            console.log(req.status + ' ' + req.statusText);
        }
    }
    
    function respuestaErronea() {
        console.log('Error de conexión');
    }

  var req=new XMLHttpRequest();
  req.open('GET',url,true);
  req.addEventListener("load",respuestaCorrecta);
  req.addEventListener("error", respuestaErronea);
  req.send();
}

realizarPeticion('http://localhost:3000/imagenes',procesarRespuesta);