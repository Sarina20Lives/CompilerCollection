using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;
using CompilerCollection.CompilerCollection.JCode;

namespace CompilerCollection.CompilerCollection.Compilador
{
    class Contexto
    {
        private List<Simbolo> simbolos = new List<Simbolo>();
        private int nivel = 0;

        public void aumentarNivel() 
        {
            this.nivel = this.nivel + 1;
        }


        public void limpiarNivel() 
        {
            foreach (Simbolo simbolo in this.simbolos) 
            {
                if (simbolo.nivel == this.nivel) 
                {
                    this.simbolos.Remove(simbolo);
                }              
            }
            this.nivel = this.nivel - 1;
        }

        public static Contexto crearContextoGlobal(String clase)
        {
            Contexto ctx = new Contexto();
            ClaseJCode cl = Compilador.obtenerClasePorNombre(clase);
            if (cl == null) 
            {
                return ctx;
            }
            Padre padre = Padre.crearDeClase(cl.archivo, cl.clase);
            generarContextoGlobal(ctx, padre, cl.clase, false);

            ParseTreeNode imports = ParserJcode.obtenerImports(cl.clase);
            if (imports == null || imports.ChildNodes.Count <= 0) {
                return ctx;            
            }

            //Agregando el contexto global de las clases que se incluyen
            List<ClaseJCode> clsInclude;
            Padre padreInclude;
            foreach (ParseTreeNode hijo in imports.ChildNodes)
            {
                clsInclude = Compilador.obtenerClasePorArchivo(hijo.FindTokenAndGetText());
                if (clsInclude == null || clsInclude.Count() <= 0)
                {
                    return ctx;
                }
                foreach (ClaseJCode include in clsInclude) 
                {
                    padreInclude = Padre.crearDeClase(include.archivo, include.clase);
                    generarContextoGlobal(ctx, padreInclude, include.clase, true);
                }

            }
            return ctx;
        }



        public static void generarContextoGlobal(Contexto ctx, Padre padre, ParseTreeNode raiz, bool esInclude) 
        { 
            Simbolo simbolo;
            foreach (ParseTreeNode hijo in raiz.ChildNodes)
            {
                if (hijo.ToString().CompareTo(ConstantesJC.SENTENCIAS) == 0)
                {
                    foreach (ParseTreeNode sentencia in hijo.ChildNodes) 
                    { 
                        if(sentencia.ToString().CompareTo(ConstantesJC.DECGLOBAL)==0)
                        {
                            simbolo = Simbolo.resolverDeclaracion(padre, sentencia, true);
                            ctx.agregarVariable(simbolo, esInclude);
                        }
                    
                    }
                }
            }
        }



        private void agregarVariable(Simbolo simbolo, bool EsInclude)
        {
            if (EsInclude && simbolo.visibilidad.CompareTo("Public") != 0)
            {
                return;
            }

            foreach (Simbolo sim in this.simbolos)
            {
                if (Simbolo.compararPorVariable(sim, simbolo))
                {
                    //TODO:INFORMACION:La variable no se ha agregado por se redundante
                    return;
                }
            }
            this.simbolos.Add(simbolo);
        }


    }
}
