using System;
using System.Linq;

namespace AsistenciaWpf.Dto
{
    public class Eventos
    {
        private bool isSelected;
        public bool IsSelected
        {
            get { return this.isSelected; }
            set { this.isSelected = value; }
        }

        private int idEvento;

        public int IdEvento
        {
            get
            {
                return this.idEvento;
            }
            set
            {
                this.idEvento = value;
            }
        }

        private String descripcion;
        public String Descripcion
        {
            get { return this.descripcion; }
            set { this.descripcion = value; }
        }

        private int idEmpleado;

        public int IdEmpleado
        {
            get
            {
                return this.idEmpleado;
            }
            set
            {
                this.idEmpleado = value;
            }
        }

        private int idIncidente;

        public int IdIncidente
        {
            get
            {
                return this.idIncidente;
            }
            set
            {
                this.idIncidente = value;
            }
        }

        private String justificante;

        public String Justificante
        {
            get
            {
                return this.justificante;
            }
            set
            {
                this.justificante = value;
            }
        }

        private String observaciones;

        public String Observaciones
        {
            get
            {
                return this.observaciones;
            }
            set
            {
                this.observaciones = value;
            }
        }

        private DateTime startDate;

        public DateTime StartDate
        {
            get
            {
                return this.startDate;
            }
            set
            {
                this.startDate = value;
            }
        }

        private DateTime endDate;

        public DateTime EndDate
        {
            get
            {
                return this.endDate;
            }
            set
            {
                this.endDate = value;
            }
        }
    }
}