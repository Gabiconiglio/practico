using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using parcialt1.Business.Docentes;
using parcialt1.Comandos.Docentes;
using parcialt1.Data;
using parcialt1.Models;
using parcialt1.Resultados.Docentes;
using System.Net;

namespace parcialt1.Controllers
{
    public class Controller : ControllerBase
    {
        private readonly ContextDb _contexto;
        private readonly IMediator _mediator;
        public Controller(ContextDb contexto, IMediator mediator)
        {
            _contexto = contexto;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("api/personas/getPersonas")]
        public async Task<ListadoDocentes> GetPersonas()
        {
            var result = new ListadoDocentes();
            var personas = await _contexto.Docentes.ToListAsync();

            if (personas != null)
            {
                foreach (var item in personas)
                {
                    var itemPersona = new ItemDocente();
                    itemPersona.Id = item.Id;
                    itemPersona.Apellido = item.Apellido;
                    itemPersona.Nombre = item.Nombre;

                    result.ListDocentes.Add(itemPersona);
                }

                return result;
            }

            var mensajeError = "Personas no encontradas";
            result.SetMensajeError(mensajeError, HttpStatusCode.NotFound);
            return result;
        }

        [HttpGet]
        [Route("api/personas/getPersonaById/{id}")]
        public async Task<ListadoDocentes> GetPersonaById(int id)
        {
            //var result = new ListadoPersonas();

            //if (id == null || id <= 0)
            //{
            //    result.SetMensajeError("El parametro id es obligatorio", HttpStatusCode.BadRequest);
            //    return result;
            //}

            //var persona = await _contexto.Personas.Where(c => c.Id == id).Include(c=>c.Categoria).FirstOrDefaultAsync();

            //if (persona != null)
            //{
            //    var itemPersona = new ItemPersona
            //    {
            //        Apellido = persona.Apellido,
            //        Id = persona.Id,
            //        Nombre = persona.Nombre,
            //        NombreCategoria = persona.Categoria.Nombre
            //    };

            //    result.ListPersonas.Add(itemPersona);
            //    return result;
            //}

            //var mensajeError = "Persona con " + id.ToString() + " no encontrada";
            //result.SetMensajeError(mensajeError, HttpStatusCode.NotFound);

            //return result;

            return await _mediator.Send(new GetDocenteByID.GetPersonaByIdComando //crear comando
            {
                IdPersona = id
            });

        }

        [HttpPost]
        [Route("api/personas/postNuevaPersona")]
        public async Task<ListadoDocentes> PostNuevaPersona([FromBody] UpdateDocenteComando comando)//crear comando
        {
            var result = new ListadoDocentes();
            // validaciones TODO

            var persona = new Docente
            {
                Apellido = comando.Apellido,
                Nombre = comando.Nombre,
                //FechaAlta = DateTime.Now,
                //IdCategoria = comando.IdCategoria
            };

            await _contexto.Docentes.AddAsync(persona);
            await _contexto.SaveChangesAsync();

            var personaItem = new ItemDocente
            {
                Apellido = persona.Apellido,
                Nombre = persona.Nombre,
                Id = persona.Id
            };

            result.ListDocentes.Add(personaItem);

            return result;
            //return await _mediator.Send(new post_business.PostPersona
            //{
            //    Apellido = comando.Apellido,
            //    Nombre = comando.Nombre,
            //    CategoriaId = comando.IdCategoria
            //});
        }

        [HttpPut]
        [Route("api/personas/putPersona")]
        public async Task<ListadoDocentes> PutPersona([FromBody] UpdateDocenteComando comando) //crear comando
        {
            var result = new ListadoDocentes();
            // validaciones TODO

            var persona = await _contexto.Docentes.FirstOrDefaultAsync(c => c.Id == comando.Id);

            if (persona != null)
            {
                persona.Nombre = comando.Nombre;
                persona.Apellido = comando.Apellido;
               // persona.IdCategoria = comando.IdCategoria;
               // persona.FechaModificacion = DateTime.Now;

                _contexto.Update(persona);
                await _contexto.SaveChangesAsync();

                var personaItem = new ItemDocente
                {
                    Apellido = persona.Apellido,
                    Nombre = persona.Nombre,
                    Id = persona.Id
                };

                result.ListDocentes.Add(personaItem);

                return result;
            }
            else
            {
                result.SetMensajeError("persona no encontrada", HttpStatusCode.NotFound);
                return result;
            }


        }
    }
}
}
