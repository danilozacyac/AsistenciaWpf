using System;
using System.Collections.Generic;
using System.Linq;
using AsistenciaWpf.Model;

namespace AsistenciaWpf.Singletons
{
    public class ExpedientesSingleton
    {
        private static List<int> compartidos;

        private ExpedientesSingleton() { }

        public static List<int> Compartidos
            {
                get
                {
                    if (compartidos == null)
                        compartidos = new EmpleadosModel().GetExpedientesEmpleados();

                    return compartidos;
                }
            }

        public static void AddExpediente(int expediente)
        {
            compartidos.Add(expediente);
        }
    }
}
