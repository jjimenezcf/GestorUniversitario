
function paginaInicial(controlador, accion, registrosPorleer) {
    alert(`https://../${controlador}/${accion}?regPorLeer=${registrosPorleer}`);
}

function paginaAnterior(controlador, accion, registrosPorleer, ultLeido) {
    alert(`https://../${controlador}/${accion}?regPorLeer=${registrosPorleer},ultLeido=${ultLeido}`);
}

function paginaSiguiente(controlador, accion, registrosPorleer, ultLeido) {
    alert(`https://../${controlador}/${accion}?regPorLeer=${registrosPorleer},ultLeido=${ultLeido}`);
}

function paginaUltima(controlador, accion, registrosPorleer, ultLeido) {
    alert(`https://../${controlador}/${accion}?regPorLeer=${registrosPorleer},ultLeido=${ultLeido}`);
}