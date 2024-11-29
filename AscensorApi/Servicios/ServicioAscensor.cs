using AscensorApi.Entidades;

namespace AscensorApi.Servicios
{
    public class ServicioAscensor
    {
        private readonly Ascensor _ascensor = new Ascensor();

        public async Task<Ascensor> ObtenerEstado()
        {
            return await Task.FromResult(_ascensor);
        }

        public async Task LlamarAscensor(int piso)
        {
            await Task.Run(() =>
            {
                if (!_ascensor.Solicitudes.Contains(piso))
                    _ascensor.Solicitudes.Enqueue(piso);
            });
        }

        public async Task AbrirPuertas()
        {
            await Task.Run(() =>
            {
                if (!_ascensor.EnMovimiento)
                    _ascensor.PuertasAbiertas = true;
            });
        }

        public async Task CerrarPuertas()
        {
            await Task.Run( () =>
            {
                _ascensor.PuertasAbiertas = false;
            });
               
        }

        public async Task IniciarAscensor(int piso)
        {
            await Task.Run(async() =>
            {

                if (_ascensor.PuertasAbiertas) //validamos para cerrar las puertas antes de movernos
                    await CerrarPuertas();

                _ascensor.EnMovimiento = true;
                _ascensor.Solicitudes.Enqueue(piso);

                Thread.Sleep(500);

                await ProcesarSolicitudes();

            });
              
        }

        public async Task<int> DetenerAscensor()
        {
            return await Task.FromResult( await Task.Run( () =>
            {   
                _ascensor.EnMovimiento = false;
                return _ascensor.PisoActual;
            }));
        }

        private async Task ProcesarSolicitudes()
        {
            await Task.Run(async () => {


                while (_ascensor.Solicitudes.Count > 0 && _ascensor.EnMovimiento)
                {
                    int siguientePiso = _ascensor.Solicitudes.Dequeue();
                    await MoverAUnPiso(siguientePiso);
                }

            });

          
        }

        private async Task MoverAUnPiso(int piso)
        {

            await Task.Run( () =>
            {
                while (_ascensor.PisoActual != piso && _ascensor.EnMovimiento)
                {

                    _ascensor.PisoActual += _ascensor.PisoActual < piso ? 1 : -1;
                    Thread.Sleep(1000); // tiempo en que se tarda el ascensor en cambiar de un piso, para simular ascenso o descenso

                }

                _ascensor.EnMovimiento = false;
            });
                      
        }
    }
}
