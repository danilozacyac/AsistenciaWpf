using System;
using System.Collections.Generic;
using System.Linq;
using AsistenciaWpf.Dto;
using AsistenciaWpf.Model;

namespace AsistenciaWpf.Singletons
{
    public class MesesSingleton
    {
        private static List<PairPropertyObject> compartidos;

        private MesesSingleton()
        {
        }
        
        public static List<PairPropertyObject> Compartidos
        {
            get
            {
                if (compartidos == null)
                    compartidos = new MesesModel().GetListaMeses();

                return compartidos;
            }
        }
    }
}