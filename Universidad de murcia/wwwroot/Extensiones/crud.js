function Leer(controlador) {
    realizarPeticion(`/${controlador}/Leer?cantidad=10,posicion=0`, procesarRespuesta);
}

function LeerAnteriores(controlador) {
    alert(`/${controlador}/Leer?cantidad=${0},posicion${0}`);
}

function LeerSiguientes(controlador) {
    alert(`/${controlador}/Leer?cantidad=${0},posicion${0}`);
}

function LeerUltimos(controlador) {
    alert(`/${controlador}/Leer?cantidad=${0},posicion${0}`);
}