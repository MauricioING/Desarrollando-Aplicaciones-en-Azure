using System;
using System.Collections.Generic;
using System.Text;

namespace Funciones.Funciones.OptimizadorGrupoDeImagenes
{
    public class ResultadoOptimizarGrupoDeImagenes
    {
        public Guid JobId { get; set; }
        public string[] Urls { get; set; } = [];
        public bool Exitoso { get; set; } = true;
        public string? MensajeDeError { get; set; }
        public static ResultadoOptimizarGrupoDeImagenes Error(string mensajeDeError) => 
            new ResultadoOptimizarGrupoDeImagenes { Exitoso = false, MensajeDeError = mensajeDeError };
    }
}
