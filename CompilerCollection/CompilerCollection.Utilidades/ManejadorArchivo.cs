using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CompilerCollection.CompilerCollection.Utilidades
{
    class ManejadorArchivo
    {
        private static String RUTA_RAIZ = "C:\\FilesCompilerCollection\\";

        public static String buscarContenidoArchivoImport(String nombre){
            String file = RUTA_RAIZ + nombre + ".jc";
            String contenido="";
            try
            {
                contenido = File.ReadAllText(file);
            }
            catch (IOException e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }   
            return contenido;
            
        }

        public static void escribirC3d(String cadena)
        {
            String file = RUTA_RAIZ + "c3d.txt";
            try
            {
                File.AppendAllText(file, cadena);
            }
            catch (IOException e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        public static String escribirTS(String simbolos)
        {
            String resultado = "Error al generar la tabla de Símbolos";
            String file = RUTA_RAIZ + "PlantillaTS.html";
            String contenido = "";
            try
            {
                contenido = File.ReadAllText(file);
            }
            catch (IOException e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            contenido = contenido.Replace("__BODY__", simbolos);
            String fileTS = RUTA_RAIZ + "TS.html";
            try
            {
                File.WriteAllText(fileTS, contenido);
                resultado = "Tabla de Símbolos Generada correctamente";
            }
            catch (IOException e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            return resultado;
        }

    }
}
