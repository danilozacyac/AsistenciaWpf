using System;
using System.Collections.Generic;
using System.Linq;
using AsistenciaWpf.Dto;
using AsistenciaWpf.Model;

namespace AsistenciaWpf.Singletons
{
    public class JustificacionesSingleton
    {
        private static List<PairPropertyObject> compartidos;

        private JustificacionesSingleton()
        {
        }

        public static List<PairPropertyObject> Compartidos
        {
            get
            {
                if (compartidos == null)
                    compartidos = new JustificacionesModel().GetListaJustificaciones();

                return compartidos;
            }
        }
    }
}
