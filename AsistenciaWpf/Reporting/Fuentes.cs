using System;
using System.Linq;
using iTextSharp.text;

namespace AsistenciaWpf.Reporting
{
    public class Fuentes
    {
        public static Font Encabezados
        {
            get
            {
                BaseColor black = new BaseColor(0, 0, 0);
                Font font = FontFactory.GetFont("Arial", 18, Font.BOLD, black);
                return font;
            }
        }

        public static Font NomExpd
        {
            get
            {
                BaseColor black = new BaseColor(0, 0, 0);
                Font font = FontFactory.GetFont("Arial", 12, Font.BOLD, black);
                return font;
            }
        }

        public static Font NomExpdUnder
        {
            get
            {
                BaseColor black = new BaseColor(0, 0, 0);
                Font font = FontFactory.GetFont("Arial", 12, Font.UNDERLINE, black);
                return font;
            }
        }

        public static Font EncabezadoColumna
        {
            get
            {
                BaseColor black = new BaseColor(0, 0, 0);
                Font font = FontFactory.GetFont("Arial", 13, Font.BOLD, black);
                return font;
            }
        }

        public static Font ContenidoCelda
        {
            get
            {
                BaseColor black = new BaseColor(0, 0, 0);
                Font font = FontFactory.GetFont("Arial", 9, Font.NORMAL, black);
                return font;
            }
        }

        /// <summary>
        /// Fuentes encabezado y Pie de Pagina
        /// </summary>

        public static Font Footer
        {
            get
            {
                BaseColor grey = new BaseColor(128, 128, 128);
                Font font = FontFactory.GetFont("Arial", 9, Font.NORMAL, grey);
                return font;
            }
        }

        public static Font Header
        {
            get
            {
                BaseColor grey = new BaseColor(255, 0, 0);
                Font font = FontFactory.GetFont("Arial", 16, Font.NORMAL, grey);
                return font;
            }
        }
    }
}
