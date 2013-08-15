using System;
using System.Collections.Generic;
using System.Linq;
using AsistenciaWpf.Dto;
using AsistenciaWpf.Model;

namespace AsistenciaWpf.Singletons
{
    public class AreasSingleton
    {
        private static List<Areas> compartidos;

        private AreasSingleton() { }

        public static List<Areas> Compartidos
            {
                get
                {
                    if (compartidos == null)
                        compartidos = new AreasModel().GetListaAreas();

                    return compartidos;
                }
            }
    }
}
