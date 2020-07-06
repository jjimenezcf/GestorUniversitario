namespace Seguridad {

    export class CrudMntPuestosDeUnUsuario extends Crud.CrudMnt {
        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt);
            this.crudDeCreacion = new CrudCreacionPuestoDeUnUsuario(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionPuestoDeUnUsuario(this, idPanelEdicion);
            this.idModalBorrar = idModalBorrar;

            let estado: string = sessionStorage.getItem(`${ValorStorage.entrada}-${idPanelMnt}`);
            if (!IsNullOrEmpty(estado)) {
                var valores = estado.split('#');
                sessionStorage.setItem(Restrictor.idUsuario, valores[0].split('==')[1]);
                sessionStorage.setItem(Parametros.Usuario, valores[1].split('==')[1]);
                sessionStorage.removeItem(`${ValorStorage.entrada}-${idPanelMnt}`);
            }

            if (!NumeroMayorDeCero(sessionStorage[Restrictor.idUsuario])) {
                console.log();
                document.location.href = document.referrer;
            }
            else {

                this.MapearRestrictorDeFiltro(Restrictor.idUsuario, sessionStorage[Restrictor.idUsuario].Numero(), sessionStorage[Parametros.Usuario]);
                this.crudDeCreacion.MaperaRestrictorDeCreacion(Restrictor.idUsuario, sessionStorage[Restrictor.idUsuario].Numero(), sessionStorage[Parametros.Usuario]);
                this.crudDeEdicion.MaperaRestrictorDeEdicion(Restrictor.idUsuario, sessionStorage[Restrictor.idUsuario].Numero(), sessionStorage[Parametros.Usuario]);

                this.Estado[Restrictor.idUsuario] = sessionStorage[Restrictor.idUsuario];
                this.Estado[Parametros.Usuario] = sessionStorage[Parametros.Usuario]
                
                sessionStorage.removeItem(Restrictor.idUsuario);
                sessionStorage.removeItem(Parametros.Usuario);
            }
        }

        public IrARelacionar(parametrosDeEntrada: string) {
            let partes = parametrosDeEntrada.split('#');
            let idForm = partes[0].split('==')[1];
            let relacionarCon = partes[1].split('==')[1];

            try {
                if (this.InfoSelector.Cantidad != 1) {
                    throw new Error("Debe seleccionar solo un usuario");
                }

                switch (relacionarCon) {
                    case Relaciones.roles: {
                        let id: number = this.InfoSelector.LeerElemento(0).Id;
                        let filtro: string = this.DefinirFiltroPorRestrictor(Restrictor.idPuesto, id);
                        let orden: string = "rol";

                        let idPuesto: string = this.obtenerValorDeLaFilaParaLaPropiedad(id, "idpuesto");
                        let puesto: string = this.obtenerValorDeLaFilaParaLaPropiedad(id, "puesto");

                        sessionStorage[Restrictor.idPuesto] = idPuesto;
                        sessionStorage[Parametros.Puesto] = puesto;

                        parametrosDeEntrada = `${idForm}#${filtro}#${orden}`;

                        break;
                    }
                }

                super.IrARelacionar(parametrosDeEntrada);
            }
            catch (error) {
                Mensaje(TipoMensaje.Error, error);
                return;
            }
        }

    }

    export class CrudCreacionPuestoDeUnUsuario extends Crud.CrudCreacion {

        constructor(crud: Crud.CrudMnt, idPanelCreacion: string) {
            super(crud, idPanelCreacion);
        }

    }

    export class CrudEdicionPuestoDeUnUsuario extends Crud.CrudEdicion {

        constructor(crud: Crud.CrudMnt, idPanelEdicion: string) {
            super(crud, idPanelEdicion);
        }
    }
}