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

        public static void agregarInit() {
            String fileC3d = RUTA_RAIZ + "c3d.txt";
            String fileInit = RUTA_RAIZ + "init.txt";
            String contenido = "";
            try
            {
                contenido = leer(fileInit);
                File.AppendAllText(fileC3d, contenido);
            }
            catch (IOException e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        
        }

        public static String leer(String file)
        {
            String contenido = "";
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

        public static void escribirC3d(String cadena, bool esInit)
        {
            String file = RUTA_RAIZ + "c3d.txt";
            if (esInit) {
                file = RUTA_RAIZ + "init.txt";
            }
            try
            {
                File.AppendAllText(file, cadena);
            }
            catch (IOException e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        public static void iniciarC3d()
        {
            String fileC3d = RUTA_RAIZ + "c3d.txt";
            String fileInit = RUTA_RAIZ + "init.txt";
            try
            {
                File.WriteAllText(fileC3d, "");
                File.WriteAllText(fileInit, "");
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
            contenido = contenido.Replace("__DATE__", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
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
