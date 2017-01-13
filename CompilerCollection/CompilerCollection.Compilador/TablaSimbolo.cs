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

        public String generarReporte() {
            String simbolos = this.toHtml();
            return ManejadorArchivo.ManejadorArchivo.escribirTS(simbolos);
        }

        public String toHtml()
        {
            String html = "";
            foreach (Simbolo simbolo in simbolos) {
                html = html + simbolo.toHtml();
            }
            return html;
        }

        public void generar(Padre padre, ParseTreeNode raiz) {
            Simbolo simbolo = null;

            if (raiz.ToString().CompareTo(ConstantesJC.CLASE) == 0)
            {
                simbolo = Simbolo.resolverClase(padre.archivo, raiz);
            }

            if (raiz.ToString().CompareTo(ConstantesJC.CONSTRUCTOR) == 0)
            {
                simbolo = Simbolo.resolverConstructor(padre, raiz);
                padre = Padre.crearDeMetodo(padre, raiz);
            }

            if (raiz.ToString().CompareTo(ConstantesJC.PRINCIPAL) == 0)
            {
                simbolo = Simbolo.resolverPrincipal(padre, raiz);
                padre = Padre.crearDePrincipal(padre, raiz);
            }

            if (raiz.ToString().CompareTo(ConstantesJC.OVERRIDE) == 0 || raiz.ToString().CompareTo(ConstantesJC.METODO) == 0)
            {
                simbolo = Simbolo.resolverMetodo(padre, raiz);
                padre = Padre.crearDeMetodo(padre, raiz);
            }

            if (raiz.ToString().CompareTo(ConstantesJC.PARAMETRO) == 0)
            {
                simbolo = Simbolo.resolverParametro(padre, raiz);
                agregarVariable(simbolo, padre);
            }

            if (raiz.ToString().CompareTo(ConstantesJC.DECGLOBAL) == 0)
            {
                simbolo = Simbolo.resolverDeclaracion(padre, raiz, true);
                if (simbolo != null)
                {
                    agregarVariable(simbolo, padre);
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

            if (raiz.ToString().CompareTo(ConstantesJC.CLASE) == 0)
            {
                if (simbolo != null)
                {
                    simbolo.tamanio = padre.pos;
                    simbolos.Add(simbolo);
                }
            }

            if( raiz.ToString().CompareTo(ConstantesJC.CONSTRUCTOR) == 0 ||
                raiz.ToString().CompareTo(ConstantesJC.PRINCIPAL) == 0 ||
                raiz.ToString().CompareTo(ConstantesJC.OVERRIDE) == 0 || 
                raiz.ToString().CompareTo(ConstantesJC.METODO) == 0)
            {
                if (simbolo != null)
                {
                    agregarBloque(simbolo, padre);
                }
            }
        }


        public static void agregarBloque(Simbolo simbolo, Padre padre)
        {
            foreach (Simbolo sim in simbolos)
            {
                if (Simbolo.compararPorBloque(sim, simbolo))
                {
                    //TODO:ERROR:La sección de bloque es redundante
                    return;
                }
            }
            simbolo.tamanio = padre.pos;
            simbolos.Add(simbolo);
        }

        public static void agregarVariable(Simbolo simbolo, Padre padre)
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
            padre.aumentarPos();
        }

    
    }
}
