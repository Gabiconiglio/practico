using MediatR;
using Microsoft.AspNetCore.Mvc;
using parcialt1.Business.Docentes;
using parcialt1.Comandos.Docentes;
using parcialt1.Data;
using parcialt1.Resultados.Docentes;

namespace parcialt1.Controllers
{
    [ApiController]
    public class DocenteController:ControllerBase
    {
        private readonly ContextDb _context;
        private readonly IMediator _mediator;
        public DocenteController(ContextDb context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("api/docentes/GetDocentes")]
        public async Task<ListadoDocentes> GetDocentes()
        {
            var resultado=await _mediator.Send(new GetDocenteBusiness.GetDocenteComando());
            return resultado;
        }

        [HttpPut]
        [Route("api/docentes/actualizarDocente")]
        public async Task<ListadoDocentes> ActualizarDocente([FromBody] UpdateDocenteComando comando)
        {
            var resultado = await _mediator.Send(new PutDocenteBusiness.PutDocenteComando
            {
                Id=comando.Id,
                Apellido=comando.Apellido,
                Nombre=comando.Nombre,
                Email=comando.Email,
                Edad=comando.Edad
            });

            return resultado;
        }

    }
}
