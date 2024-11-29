namespace AscensorApi.Entidades
{
    public class Ascensor
    {
        public int PisoActual { get; set; } = 1;
        public bool PuertasAbiertas { get; set; } = false;
        public bool EnMovimiento { get; set; } = false;
        public Queue<int> Solicitudes { get; set; } = new Queue<int>();
    }
}
