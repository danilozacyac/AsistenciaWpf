using System;
using System.Linq;

namespace AsistenciaWpf.Dto
{
    public class Areas
    {
        private int idArea;
        public int IdArea
        {
            get { return this.idArea; }
            set { this.idArea = value; }
        }

        private String descripcion;
        public String Descripcion
        {
            get { return this.descripcion; }
            set { this.descripcion = value; }
        }

        private String descMay;
        public String DescMay
        {
            get { return this.descMay; }
            set { this.descMay = value; }
        }

        private String abreviatura;
        public String Abreviatura
        {
            get { return abreviatura; }
            set { this.abreviatura = value; }
        }
    }
}
