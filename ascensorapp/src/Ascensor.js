import React, { useState, useEffect } from "react";
import '../src/Ascensor.css'

const UrlApi = "https://localhost:7166/";


const ControlAscensor = () => {
    const [estado, setEstado] = useState({});
    const [piso, setPiso] = useState(0);
    const [espera, setEspera] = useState(0);
    const [mensaje, setMensaje] = useState("");
    const [llamando, setLlamando] = useState(false)

    useEffect(() => {

        const interval = setInterval(() => {
            obtenerEstado();

            if (llamando) {
                ValidarIniciarAutomaticamente();
            }

            if (piso == estado.pisoActual) {
                setMensaje("Ascensor llego a su destino");
            }

        }, 500); // Consultar cada medio segundo

        return () => clearInterval(interval);

    }, [espera, estado, mensaje,piso]);

    const obtenerEstado = async () => {
        const respuesta = await fetch(UrlApi + "api/ascensor/estado");
        const datos = await respuesta.json();
        setEstado(datos);
    };

    const ValidarIniciarAutomaticamente = async () => {

        if (!estado.enMovimiento && espera >= 5) {
            iniciarAscensor();
            setMensaje("debido a que se cumplio la espera el ascensor iniciara automaticamente");
            setLlamando(false);
        }
        else {
            setMensaje("el ascensor se encuentra en movimiento esperelo pacientemente");
        }
        setEspera(espera => espera + 1);
    };

    const llamarAscensor = async () => {

        if (!llamando) {

            setEspera(0);

            await fetch(UrlApi + `api/ascensor/llamar?piso=${piso}`, {
                method: "POST",
            });

            setLlamando(true);
            setMensaje("llamando al ascensor espere pacientemente");

        }
        else {
            setMensaje("ya se realizo un llamado espere pacientemente");
        }


    };

    const abrirPuertas = async () => {
        var result = await fetch(UrlApi + "api/ascensor/abrir-puertas", { method: "POST" });
        setMensaje("puertas abiertas");
    };

    const cerrarPuertas = async () => {
        await fetch(UrlApi + "api/ascensor/cerrar-puertas", { method: "POST" });
        setMensaje("puertas cerradas");
    };

    const iniciarAscensor = async () => {
        await fetch(UrlApi + `api/ascensor/iniciar?piso=${piso}`, { method: "POST" });
        setMensaje("ascensor moviendose al piso" + piso);
    };

    const detenerAscensor = async () => {
        setLlamando(false);
        const result = await fetch(UrlApi + "api/ascensor/detener", { method: "POST" });
        setMensaje("ascensor detenido en el piso" + piso);

    };

    return (
        <div className="contenedor-ascensor">
            <h1 className="titulo">Control de Ascensor</h1>
            <p className="texto-estado">Piso Actual: {estado.pisoActual}</p>
            <p className="texto-estado">Puertas Abiertas: {estado.puertasAbiertas ? "Sí" : "No"}</p>
            <p className="texto-estado">En Movimiento: {estado.enMovimiento ? "Sí" : "No"}</p>
            <p className="texto-estado">Espera: {espera}</p>
            <p className="mensaje">{mensaje}</p>
            
            <input
                type="number"
                className="input-piso"
                value={piso}
                onChange={(e) => setPiso(parseInt(e.target.value))}
                placeholder="Ingrese piso"
            />
            <div className="contenedor-botones">
                <button className="btn-accion" onClick={llamarAscensor}>Llamar Ascensor</button>
                <button className="btn-accion" onClick={abrirPuertas}>Abrir Puertas</button>
                <button className="btn-accion" onClick={cerrarPuertas}>Cerrar Puertas</button>
                <button className="btn-accion" onClick={iniciarAscensor}>Iniciar</button>
                <button className="btn-accion" onClick={detenerAscensor}>Detener</button>
            </div>
        </div>
    );
};

export default ControlAscensor;