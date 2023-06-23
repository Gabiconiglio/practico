using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using parcialt1.Data;
using parcialt1.Resultados.Docentes;
using System.Net;

namespace parcialt1.Business.Docentes
{
    public class GetDocenteByID
    {
        public class GetPersonaByIdComando : IRequest<ListadoDocentes>
        {
            public int IdPersona { get; set; }
        }
        public class EjecutaValidacion : AbstractValidator<GetPersonaByIdComando>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.IdPersona).NotEmpty();//anotacion blanda, buscar info
            }
        }
        public class Manejador : IRequestHandler<GetPersonaByIdComando, ListadoDocentes>
        {
            //propiedades
            private readonly ContextDb _contexto;

            private readonly IValidator<GetPersonaByIdComando> _validator;

            //constructor

            public Manejador(ContextDb contexto, IValidator<GetPersonaByIdComando> validator)
            {
                _contexto = contexto;
                _validator = validator;
            }
            //metodo que se encarga de resolver logica de negocio

            public async Task<ListadoDocentes> Handle(GetPersonaByIdComando request, CancellationToken cancellationToken)
            {
                var result = new ListadoDocentes();

                var validation = await _validator.ValidateAsync(request);
                if (!validation.IsValid)
                {
                    var errors = string.Join(Environment.NewLine, validation.Errors);
                    result.SetMensajeError(errors, HttpStatusCode.InternalServerError);
                    return result;
                }

                var persona = await _contexto.Docentes.Where(c => c.Id == request.IdPersona).Include(c => c.Categoria).FirstOrDefaultAsync();

                if (persona != null)
                {
                    var itemPersona = new ItemDocente
                    {
                        Apellido = persona.Apellido,
                        Id = persona.Id,
                        Nombre = persona.Nombre,
                       // NombreCategoria = persona.Categoria.Nombre
                    };

                    result.ListDocentes.Add(itemPersona);
                    return result;
                }

                var mensajeError = "Persona con " + request.IdPersona.ToString() + " no encontrada";
                result.SetMensajeError(mensajeError, HttpStatusCode.NotFound);

                return result;
            }


        }
    }
}
