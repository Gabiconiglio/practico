using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using parcialt1.Data;
using parcialt1.Resultados.Docentes;
using System.Net;

namespace parcialt1.Business.Docentes
{
    public class GetDocenteBusiness
    {
        public class GetDocenteComando : IRequest<ListadoDocentes>
        {
            
        }

        public class EjecutaValidacion : AbstractValidator<GetDocenteComando>
        {
            public EjecutaValidacion()
            {
                
            }
        }

        public class Manejador : IRequestHandler<GetDocenteComando, ListadoDocentes>
        {
            private readonly ContextDb _contextDb;
            private readonly IValidator<GetDocenteComando> _validator;
            public Manejador(ContextDb contextDb, IValidator<GetDocenteComando> validator)
            {
                _contextDb = contextDb;
                _validator = validator;
            }

            public async Task<ListadoDocentes>Handle(GetDocenteComando comando,CancellationToken cancellation)
            {
                var resultado = new ListadoDocentes();
                var validacion = await _validator.ValidateAsync(comando);
                if (!validacion.IsValid)
                {
                    var errores = string.Join(Environment.NewLine, validacion.Errors);
                    resultado.SetMensajeError(errores, HttpStatusCode.InternalServerError);
                    return resultado;
                }
                var docentes = await _contextDb.Docentes
                    .Include(d => d.IdNivelNavigation)
                    .Where(c => c.Edad >= 30 && c.Edad <= 40 && c.Email.Contains("outlook"))
                    .Where(d => d.IdNivelNavigation.Nombre.Contains("Secundario"))
                    .FirstOrDefaultAsync();

                if (docentes != null)
                {

                    var itemDoc = new ItemDocente
                    {
                        Id = docentes.Id,
                        Nombre = docentes.Nombre,
                        Apellido = docentes.Apellido,
                        Edad = docentes.Edad,
                        Email = docentes.Email,
                        Nivel = docentes.IdNivelNavigation.Nombre
                    };
                    resultado.ListDocentes.Add(itemDoc);
                    return resultado;
                }
                var mensajeError = "Docentes no encontrados";
                resultado.SetMensajeError(mensajeError, HttpStatusCode.NotFound);
                return resultado;

            }
        }

    }
}
