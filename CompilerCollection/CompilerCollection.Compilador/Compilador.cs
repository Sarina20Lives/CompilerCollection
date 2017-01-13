using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;
using CompilerCollection.CompilerCollection.JCode;
using CompilerCollection.CompilerCollection.C3D;

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



    }
}
