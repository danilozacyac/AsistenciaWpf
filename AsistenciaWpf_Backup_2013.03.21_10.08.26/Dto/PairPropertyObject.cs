using System;
using System.Linq;

namespace AsistenciaWpf.Dto
{
    public class PairPropertyObject
    {
        private int identificador;
        public int Identificador
        {
            get { return this.identificador; }
            set { this.identificador = value; }
        }

        private String descripcion;
        public String Descripcion
        {
            get { return this.descripcion; }
            set { this.descripcion = value; }
        }


        public PairPropertyObject(int identificador, String descripcion)
        {
            this.identificador = identificador;
            this.descripcion = descripcion;
        }

    }
}
