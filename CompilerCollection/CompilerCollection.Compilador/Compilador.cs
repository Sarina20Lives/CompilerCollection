using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;
using CompilerCollection.CompilerCollection.JCode;
using CompilerCollection.CompilerCollection.C3D;
using CompilerCollection.CompilerCollection.Utilidades;

namespace CompilerCollection.CompilerCollection.Compilador
{
    public class Compilador
    {
        public static List<ClaseJCode> repertorioClases = null;
        public static Stack<Entorno> display = null;

        public static void iniciarCompilacion(){
            repertorioClases = new List<ClaseJCode>();
            display = new Stack<Entorno>();
            C3d.iniciarC3d();
        }

        public static String getEtqSalida(bool tipo) {
            if (display==null || display.Count() == 0) {
                return null;
            }
            if (tipo) {
                return display.Last().etqInicio;
            }
            return display.Last().etqFinal;
        }

        public static void generarEtqsSalida() {
            Entorno entorno = new Entorno();
            entorno.etqInicio = C3d.generarEtq();
            entorno.etqFinal = C3d.generarEtq();
            if (display == null)
            {
                display = new Stack<Entorno>();
            }
            display.Push(entorno);
        }

        public static void eliminarEtqsSalida() {
            if (display == null) {
                return;
            }
            display.Pop();
        }

        public static List<ClaseJCode> obtenerClasePorArchivo(String archivo)
        {
            List<ClaseJCode> cls = new List<ClaseJCode>();
            foreach (ClaseJCode cl in repertorioClases)
            {
                if (cl.archivo.CompareTo(archivo) == 0)
                {
                    cls.Add(cl);
                }
            }
            return cls;
        }

        public static ClaseJCode obtenerClasePorNombre(String nombre)
        {
            foreach (ClaseJCode cl in repertorioClases)
            {
                if (cl.clase.ChildNodes.ElementAt(0).FindTokenAndGetText().CompareTo(nombre) == 0)
                {
                    return cl;
                }
            }
            return null;
        }

        public static String generarTablaSimbolos() {
            //Generación de tabla de símbolos
            TablaSimbolo ts = new TablaSimbolo();
            Padre padre;
            foreach (ClaseJCode clase in repertorioClases)
            {
                padre = Padre.crearDeClase(clase.archivo, clase.clase);
                ts.generar(padre, clase.clase);
            }
            String resultado = ts.generarReporte();
            return resultado;
        }

        public static String generarC3d() {
            //Generar C3d
            Simbolo ambito;
            GeneradorC3d generadorc3d = new GeneradorC3d();
            Contexto ctxGlobal = new Contexto();
            Contexto ctxLocal = new Contexto();
            generadorc3d.iniciar();
            foreach (ClaseJCode clase in repertorioClases)
            {
                String nombre = clase.clase.ChildNodes.ElementAt(0).FindTokenAndGetText();
                ambito = TablaSimbolo.buscarClase(clase.archivo, nombre);
                ambito.tamanio = 2;
                if (ambito == null)
                {
                    ManejadorErrores.General("Comprueba la generación de tabla de símbolos, no se encontro la clase " + nombre + " en el archivo " + clase.archivo);
                    continue;
                }
                //Escribir clase_init
                generadorc3d.iniciarInit(ambito.nombre, ambito.tamanio);
                ctxGlobal = Contexto.crearContextoGlobal(nombre);
                generadorc3d.generar(ambito, ctxGlobal, ctxLocal, clase.clase, true);
            }
            return "Escritura de C3d finalizada \n";
        }

    }
}
