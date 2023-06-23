using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using parcialt1.Data;
using parcialt1.Models;
using parcialt1.Resultados.Docentes;
using System.Net;

namespace parcialt1.Business.Docentes
{
    public class PutDocenteBusiness
    {
        public class PutDocenteComando : IRequest<ListadoDocentes>
        {
            public int Id { get; set; }
            public string Nombre { get; set; } 
            public string Apellido { get; set; } 
            public int Edad { get; set; }
            public string Email { get; set; } 
            
        }

        public class EjecutaValidacion : AbstractValidator<PutDocenteComando>
        {
            public EjecutaValidacion()
            {
                RuleFor(d => d.Id).NotEmpty().WithMessage("El campo id debe ser obligatorio");
                RuleFor(d => d.Nombre).NotEmpty().WithMessage("el campo nombre debe ser obligatorio");
                RuleFor(d => d.Apellido).NotEmpty().WithMessage("el campo Apellido debe ser obligatorio");
                RuleFor(d => d.Edad).NotEmpty().WithMessage("el campo Edad debe ser obligatorio");
                RuleFor(d => d.Email).EmailAddress().WithMessage("el campo email debe ser un tipo valido");
            }
        }

        public class Manejador : IRequestHandler<PutDocenteComando, ListadoDocentes>
        {
            private readonly ContextDb _contextDb;
            private readonly IValidator<PutDocenteComando> _validator;
            public Manejador(ContextDb contextDb, IValidator<PutDocenteComando> validator)
            {
                _contextDb = contextDb;
                _validator = validator;
            }

            public async Task<ListadoDocentes> Handle(PutDocenteComando comando, CancellationToken cancellation)
            {
                var resultado = new ListadoDocentes();
                var validacion = await _validator.ValidateAsync(comando);
                if (!validacion.IsValid)
                {
                    var errores = string.Join(Environment.NewLine, validacion.Errors);
                    resultado.SetMensajeError(errores, HttpStatusCode.InternalServerError);
                    return resultado;
                }
                var docente = await _contextDb.Docentes.Where(d => d.Id == comando.Id).Include(n => n.IdNivelNavigation).FirstOrDefaultAsync();
                
                if (docente != null)
                {
                    docente.Apellido = comando.Apellido;
                    docente.Nombre = comando.Nombre;
                    docente.Email = comando.Email;
                    docente.Edad = comando.Edad;
                    docente.Id = comando.Id;

                    var nuevoLog = new Log
                    {
                        FechaLog = DateOnly.FromDateTime(DateTime.Now),
                        IdDocente = docente.Id,
                        Log1 = "actualizacion completada el dia de la fecha"
                    };
                    await _contextDb.Logs.AddAsync(nuevoLog);
                    _contextDb.Update(docente);
                    await _contextDb.SaveChangesAsync();
                    var itemDocente = new ItemDocente
                    {
                        Id = docente.Id,
                        Apellido = docente.Apellido,
                        Nombre = docente.Nombre,
                        Email = docente.Email,
                        Edad = docente.Edad,
                        Nivel = docente.IdNivelNavigation.Nombre
                    };

                    resultado.ListDocentes.Add(itemDocente);

                    return resultado;
                }
                var mensajeError = "Docentes no encontrados";
                resultado.SetMensajeError(mensajeError, HttpStatusCode.NotFound);
                return resultado;

            }
        }
    }
}
