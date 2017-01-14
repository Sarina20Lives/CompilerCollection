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
        private static String RUTA_RAIZ = "C:\\Users\\sarina\\Desktop\\ArchivoJC\\";
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

    }
}
