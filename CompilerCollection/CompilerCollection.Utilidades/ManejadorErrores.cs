using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerCollection.CompilerCollection.Utilidades
{
    class ManejadorErrores
    {
        private const string RUTA_GENERAL = "C:\\FilesCompilerCollection\\Reportes\\";
        private const string RUTA_PLANTILLA = RUTA_GENERAL + "PlantillaErrores.html";
        private const string RUTA_REPORTE = RUTA_GENERAL + "Errores.html";

        private static ManejadorErrores singleton = null;
        
        public static void iniciar()
        {
            singleton = new ManejadorErrores();
        }

        public static void General(string descripcion, int linea, int columna, string archivo)
        {
            Error error = new Error(descripcion, TipoError.General, linea, columna);
            error.SetArchivo(archivo);
            singleton.AddError(error);
        }

        public static void General(string descripcion, int linea, int columna)
        {
            General(descripcion, linea, columna, Error.DEFAULT);
        }


        public static void Semantico(string descripcion, int linea, int columna, string archivo)
        {
            Error error = new Error(descripcion, TipoError.Semantico, linea, columna);
            error.SetArchivo(archivo);
            singleton.AddError(error);
        }

        public static void Semantico(string descripcion, int linea, int columna)
        {
            Semantico(descripcion, linea, columna, Error.DEFAULT);
        }

        public static void Sintactico(string descripcion, int linea, int columna, string archivo)
        {
            Error error = new Error(descripcion, TipoError.Sintactico, linea, columna);
            error.SetArchivo(archivo);
            singleton.AddError(error);
        }

        public static void Sintactico(string descripcion, int linea, int columna)
        {
            Sintactico(descripcion, linea, columna, Error.DEFAULT);
        }

        public static void Lexico(string descripcion, int linea, int columna, string archivo)
        {
            Error error = new Error(descripcion, TipoError.Lexico, linea, columna);
            error.SetArchivo(archivo);
            singleton.AddError(error);
        }

        public static void Lexico(string descripcion, int linea, int columna)
        {
            Lexico(descripcion, linea, columna, Error.DEFAULT);
        }

        public static void GenerarReporte()
        {
            if(singleton==null)
                singleton = new ManejadorErrores();
            string reporte = File.ReadAllText(RUTA_PLANTILLA);
            reporte = reporte.Replace("__DATE__", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            string body = "";
            foreach(Error err in singleton.errores)
            {
                body += err.getHTML();
            }
            reporte = reporte.Replace("__BODY__", body);
            File.WriteAllText(RUTA_REPORTE, reporte);
        }

        private List<Error> errores;
        private string archivo { get; set; }

        private ManejadorErrores()
        {
            this.errores = new List<Error>();
        }

        private void AddError(Error error)
        {
            this.errores.Add(error);
        }

    }

    class Error
    {
        TipoError tipo { get; set; }
        string descripcion { get; set; }
        string archivo { get; set; }
        int linea { get; set; }
        int columna { get; set; }
        public const string DEFAULT = "---";

        public Error(string descripcion, TipoError tipo, int linea, int columna)
        {
            this.descripcion = descripcion;
            this.tipo = tipo;
            this.linea = linea;
            this.columna = columna;
            this.archivo = DEFAULT;
        }

        public void SetArchivo(string archivo)
        {
            this.archivo = archivo;
        }

        public string getHTML()
        {
            string html = "<tr>\n";
            html += "\t<td>" + tipo.ToString() + "</td>\n";
            html += "\t<td>" + descripcion + "</td>\n";
            html += "\t<td>" + archivo + "</td>\n";
            html += "\t<td>" + linea + "</td>\n";
            html += "\t<td>" + columna + "</td>\n";
            html += "</tr>\n";
            return html;
        }

    }

    enum TipoError 
    {
        Lexico, Sintactico, Semantico, General
    }
}
