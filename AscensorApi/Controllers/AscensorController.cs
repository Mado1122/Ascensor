using AscensorApi.Entidades;
using AscensorApi.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AscensorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AscensorController : ControllerBase
    {
        private static ServicioAscensor _servicioAscensor = new ServicioAscensor();


        [HttpGet("estado")]
        public  async Task<ActionResult<Ascensor>> ObtenerEstado()
        {
            var result = await _servicioAscensor.ObtenerEstado();

            return Ok(result);
        }

        [HttpPost("llamar")]
        public async Task<IActionResult> LlamarAscensor([FromQuery] int piso)
        {   
            await _servicioAscensor.LlamarAscensor(piso);

            return Ok($"Ascensor llamado al piso {piso}");
        }

        [HttpPost("abrir-puertas")]
        public async Task<IActionResult> AbrirPuertas()
        {   
            await _servicioAscensor.AbrirPuertas();
            return Ok("Puertas abiertas.");
        }

        [HttpPost("cerrar-puertas")]
        public async Task<IActionResult> CerrarPuertas()
        {
            await _servicioAscensor.CerrarPuertas();
            return Ok("Puertas cerradas.");
        }

        [HttpPost("iniciar")]
        public async Task<IActionResult> IniciarAscensor([FromQuery] int piso)
        {
            await _servicioAscensor.IniciarAscensor(piso);
            return Ok("Ascensor iniciado moviendose al piso " + piso);
        }

        [HttpPost("detener")]
        public async Task<IActionResult> DetenerAscensor()
        {
           var piso = await _servicioAscensor.DetenerAscensor();
            return Ok("Ascensor detenido en el piso " + piso);
        }
    }
}
