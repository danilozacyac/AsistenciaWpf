using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace AsistenciaWpf.Dto
{
    public class Empleados : IDataErrorInfo, INotifyPropertyChanged
    {
        private int numEventos;

        public int NumEventos
        {
            get
            {
                return this.numEventos;
            }
            set
            {
                this.numEventos = value;
                OnPropertyChanged("NumEventos");
            }
        }

        private int expediente;

        public int Expediente
        {
            get { return this.expediente; }
            set { this.expediente = value; }
        }

        private int idEmpleado;

        public int IdEmpleado
        {
            get { return idEmpleado; }
            set { this.idEmpleado = value; }
        }

        private String nombre;
        public String Nombre
        {
            get { return this.nombre; }
            set { this.nombre = value; }
        }

        private String apellidos;
        public String Apellidos
        {
            get { return this.apellidos; }
            set { this.apellidos = value; }
        }

        private String nombreCompleto;
        public String NombreCompleto
        {
            get { return this.nombreCompleto; }
            set { this.nombreCompleto = value; }
        }

        private int idArea;
        public int IdArea
        {
            get { return this.idArea; }
            set { this.idArea = value; }
        }

        private ObservableCollection<Eventos> myEventos = new ObservableCollection<Eventos>();
        public ObservableCollection<Eventos> MyEventos
        {
            get { return this.myEventos; }
            set { this.myEventos = value; }
        }


        public void SetEventos(Eventos evento)
        {
            myEventos.Add(evento);
            numEventos++;
        }

        public ObservableCollection<Eventos> GetEventos()
        {
            return this.myEventos; 
        }

        public void DeleteSelectedEvent(Eventos eventos)
        {
            this.myEventos.Remove(eventos);
            numEventos--;
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == "Nombre")
                {
                    if (string.IsNullOrEmpty(Nombre) || Nombre.Length < 3)
                        result = "Ingrese el nombre";
                }
                if (columnName == "Apellidos")
                {
                    if (string.IsNullOrEmpty(Apellidos) || Apellidos.Length < 3)
                        result = "Ingrese los apellidos";
                }
                if (columnName == "Expediente")
                {
                    if (Expediente <= 1000 || Expediente >= 1000000)
                        result = "Ingrese un número de expediente valido";
                }
                return result;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
