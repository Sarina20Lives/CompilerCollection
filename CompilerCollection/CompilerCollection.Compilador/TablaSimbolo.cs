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
    class TablaSimbolo
    {
        private static List<Simbolo> simbolos = new List<Simbolo>(); 

        public TablaSimbolo(){
            simbolos = new List<Simbolo>();
        }

        public void generar(Padre padre, ParseTreeNode raiz) {
            Simbolo simbolo;

            if (raiz.ToString().CompareTo(ConstantesJC.CLASE) == 0)
            {
                simbolo = Simbolo.resolverClase(raiz);
                simbolos.Add(simbolo);
            }

            if (raiz.ToString().CompareTo(ConstantesJC.CONSTRUCTOR) == 0)
            {
                simbolo = Simbolo.resolverConstructor(padre, raiz);
                if (simbolo != null) {
                    agregarBloque(simbolo);
                }
                padre = Padre.crearDeMetodo(padre, raiz);
            }

            if (raiz.ToString().CompareTo(ConstantesJC.PRINCIPAL) == 0)
            {
                simbolo = Simbolo.resolverPrincipal(padre, raiz);
                agregarBloque(simbolo);
                padre = Padre.crearDePrincipal(padre, raiz);
            }

            if (raiz.ToString().CompareTo(ConstantesJC.OVERRIDE) == 0 || raiz.ToString().CompareTo(ConstantesJC.METODO) == 0)
            {
                simbolo = Simbolo.resolverMetodo(padre, raiz);
                agregarBloque(simbolo);
                padre = Padre.crearDeMetodo(padre, raiz);
            }

            if (raiz.ToString().CompareTo(ConstantesJC.PARAMETRO) == 0)
            {
                simbolo = Simbolo.resolverParametro(padre, raiz);
                agregarVariable(simbolo);
            }

            if (raiz.ToString().CompareTo(ConstantesJC.DECGLOBAL) == 0)
            {
                simbolo = Simbolo.resolverDeclaracion(padre, raiz, true);
                if (simbolo != null)
                {
                    agregarVariable(simbolo);
                }
                return;
            }

            if (raiz.ToString().CompareTo(ConstantesJC.DECLOCAL) == 0)
            { 
                simbolo = Simbolo.resolverDeclaracion(padre, raiz, false);
                if (simbolo != null) {
                    simbolos.Add(simbolo);
                }
                return;
            }

            if (raiz.ChildNodes.Count <= 0) {
                return;
            }

            foreach (ParseTreeNode hijo in raiz.ChildNodes) {
                generar(padre, hijo);
            }            
        }


        public static void agregarBloque(Simbolo simbolo)
        {
            foreach (Simbolo sim in simbolos)
            {
                if (Simbolo.compararPorBloque(sim, simbolo))
                {
                    //TODO:ERROR:La sección de bloque es redundante
                    return;
                }
            }
            simbolos.Add(simbolo);
        }

        public static void agregarVariable(Simbolo simbolo)
        {
            foreach (Simbolo sim in simbolos)
            {
                if (Simbolo.compararPorVariable(sim, simbolo))
                {
                    //TODO:ERROR:La variable es redundante
                    return;
                }
            }
            simbolos.Add(simbolo);
        }

    
    }
}
