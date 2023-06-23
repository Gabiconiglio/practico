using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using parcialt1.Data;
using parcialt1.Models;
using parcialt1.Resultados.Docentes;
using System.Net;

namespace parcialt1.Business.Docentes
{
    public class PosDocenteController
    {
        public class PostPersona : IRequest<ListadoDocentes>
        {
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public int CategoriaId { get; set; }
        }
        public class EjecutaValidacion : AbstractValidator<PostPersona>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Nombre).NotEmpty().WithMessage("Error nombre");//anotacion blanda, buscar info
                RuleFor(x => x.Apellido).NotEmpty().WithMessage("Error Apellido");//anotacion blanda, buscar info
                RuleFor(x => x.CategoriaId).NotEmpty().WithMessage("Error Fecha Alta");//anotacion blanda, buscar info

            }
        }
        public class Manejador : IRequestHandler<PostPersona, ListadoDocentes>
        {
            //propiedades
            private readonly ContextDb _contexto;

            //constructor

            public Manejador(ContextDb contexto)
            {
                _contexto = contexto;
            }
            //metodo que se encarga de resolver logica de negocio

            public async Task<ListadoDocentes> Handle(PostPersona request, CancellationToken cancellationToken)
            {
                var result = new ListadoDocentes();


                var persona = new Docente
                {
                    Apellido = request.Apellido,
                    Nombre = request.Nombre,
                    //IdCategoria = request.CategoriaId
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
            }
        }
    }
}
