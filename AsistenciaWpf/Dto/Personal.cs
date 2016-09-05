using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AsistenciaWpf.Dto
{
    public class Personal
    {
        private int expediente;
        private int idEmpleado;
        private string nombre;
        private string apellidos;
        private string nombreCompleto;
        private int idArea;
        private int numEventos;
        private ObservableCollection<Eventos> myEventos = new ObservableCollection<Eventos>();

        public int Expediente
        {
            get
            {
                return this.expediente;
            }
            set
            {
                this.expediente = value;
            }
        }

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

        public string Nombre
        {
            get
            {
                return this.nombre;
            }
            set
            {
                this.nombre = value;
            }
        }

        public string Apellidos
        {
            get
            {
                return this.apellidos;
            }
            set
            {
                this.apellidos = value;
            }
        }

        public string NombreCompleto
        {
            get
            {
                return this.nombreCompleto;
            }
            set
            {
                this.nombreCompleto = value;
            }
        }

        public int IdArea
        {
            get
            {
                return this.idArea;
            }
            set
            {
                this.idArea = value;
            }
        }

        public int NumEventos
        {
            get
            {
                return this.numEventos;
            }
            set
            {
                this.numEventos = value;
            }
        }

        public ObservableCollection<Eventos> MyEventos
        {
            get
            {
                return this.myEventos;
            }
            set
            {
                this.myEventos = value;
            }
        }

        public void SetEventos(Eventos evento)
        {
            myEventos.Add(evento);
            numEventos++;
        }

        public void DeleteSelectedEvent(Eventos eventos)
        {
            this.myEventos.Remove(eventos);
            numEventos--;
        }
    }
}
