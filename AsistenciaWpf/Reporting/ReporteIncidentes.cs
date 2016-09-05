using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using AsistenciaWpf.Dto;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace AsistenciaWpf.Reporting
{
    public class ReporteIncidentes
    {
        private readonly List<Personal> empleados = null;
        private iTextSharp.text.Document myDocument;

        public ReporteIncidentes(List<Personal> empleados)
        {
            this.empleados = empleados;
        }


        public void GetReporteDeIncidentes()
        {
            myDocument = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 60);

            string filePath = Path.GetTempFileName() + ".pdf";

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(myDocument, new FileStream(filePath, FileMode.Create));
                HeaderFooter pdfPage = new HeaderFooter();
                writer.PageEvent = pdfPage;
                writer.ViewerPreferences = PdfWriter.PageModeUseOutlines;
                //Paragraph para;

                myDocument.Open();

                int consec = 1;

                foreach (Personal empleado in empleados)
                {

                    Chapter chapter1 = new Chapter(new Paragraph(empleado.NombreCompleto,Fuentes.Encabezados), consec);
                    chapter1.BookmarkOpen = true;
                    myDocument.Add(chapter1);

                    PdfPTable table = new PdfPTable(4);
                    //table.TotalWidth = 400;
                    table.WidthPercentage = 90;

                    float[] widths = new float[] { 1f, 1.5f, 1.5f, 2f };
                    table.SetWidths(widths);

                    table.SpacingBefore = 20f;
                    table.SpacingAfter = 30f;

                    string[] encabezado = { "Fecha", "Incidente", "Justificante", "Observaciones"};
                    PdfPCell cell;

                    foreach (string cabeza in encabezado)
                    {
                        cell = new PdfPCell(new Phrase(cabeza, Fuentes.EncabezadoColumna));
                        cell.Colspan = 0;
                        cell.Border = 0;
                        
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);
                    }

                    foreach (Eventos even in empleado.MyEventos)
                    {
                        string format = "dd/MM/yyyy";

                        cell = new PdfPCell(new Phrase(even.StartDate.ToString(format), Fuentes.ContenidoCelda));
                        cell.Colspan = 0;
                        cell.Border = 0;
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(even.Descripcion, Fuentes.ContenidoCelda));
                        cell.Colspan = 0;
                        cell.Border = 0;
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(even.Justificante, Fuentes.ContenidoCelda));
                        cell.Colspan = 0;
                        cell.Border = 0;
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(even.Observaciones, Fuentes.ContenidoCelda));
                        cell.Colspan = 0;
                        cell.Border = 0;
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);
                    }

                    myDocument.Add(table);
                    consec++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
            }
            finally
            {
                myDocument.Close();
                System.Diagnostics.Process.Start(filePath);
            }



        }


    }
}
