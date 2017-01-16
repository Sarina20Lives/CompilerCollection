using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;
using CompilerCollection.CompilerCollection.JCode;
using CompilerCollection.CompilerCollection.Utilidades;
using CompilerCollection.CompilerCollection.General;

namespace CompilerCollection.CompilerCollection.Compilador
{
    class TablaSimbolo
    {
        private static List<Simbolo> simbolos = new List<Simbolo>();
        private static bool esOverride = false;

        public TablaSimbolo(){
            esOverride = false;
            simbolos = new List<Simbolo>();
        }

        public String generarReporte() {
            String simbolos = this.toHtml();
            return Utilidades.ManejadorArchivo.escribirTS(simbolos);
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
                if (esOverride)
                {
                    simbolo = Simbolo.resolverMetodo(padre, raiz);                    
                }
                else 
                {
                    simbolo = Simbolo.resolverConstructor(padre, raiz);
                }
                padre = Padre.crearDeMetodo(padre, raiz);
            }

            if (raiz.ToString().CompareTo(ConstantesJC.PRINCIPAL) == 0)
            {
                simbolo = Simbolo.resolverPrincipal(padre, raiz);
                padre = Padre.crearDePrincipal(padre, raiz);
            }

            if (raiz.ToString().CompareTo(ConstantesJC.METODO) == 0)
            {
                simbolo = Simbolo.resolverMetodo(padre, raiz);
                padre = Padre.crearDeMetodo(padre, raiz);
            }

            if (raiz.ToString().CompareTo(ConstantesJC.OVERRIDE) == 0 )
            {
                esOverride = true;
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
                    agregarVariable(simbolo, padre);
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
                    if (padre.pos == 0) {
                        padre.pos = 1;
                    }
                    simbolo.tamanio = padre.pos;                    
                    simbolos.Add(simbolo);
                }
            }

            if( raiz.ToString().CompareTo(ConstantesJC.CONSTRUCTOR) == 0 ||
                raiz.ToString().CompareTo(ConstantesJC.PRINCIPAL) == 0 ||
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
            if (simbolo == null) {
                return;
            }
            foreach (Simbolo sim in simbolos)
            {
                if (Simbolo.compararPorBloque(sim, simbolo))
                {
                    //TODO:ERROR:La sección de bloque es redundante
                    return;
                }
            }

            if (esOverride) {
                simbolo = obtenerTipoOverride(simbolo, padre.clase);
                esOverride = false;
                if (simbolo == null)
                {

                    return;
                }
            }
            
            simbolo.tamanio = padre.pos;
            simbolos.Add(simbolo);
        }

        public static Simbolo obtenerTipoOverride(Simbolo simbolo, String claseActual) {
            ClaseJCode actual = Compilador.obtenerClasePorNombre(claseActual);
            if (actual == null)
            {
                return null;
            }
            ParseTreeNode herencia = ParserJcode.obtenerHerencia(actual.clase);
            if (herencia == null || herencia.ChildNodes.Count == 0)
            {
                ManejadorErrores.General("La clase " + claseActual + " no posee ninguna herencia, por tanto no se puede realizar un override");
                return null;
            }
            ClaseJCode father = Compilador.obtenerClasePorNombre(herencia.ChildNodes.ElementAt(0).FindTokenAndGetText());
            if (father == null)
            {
                ManejadorErrores.General("La clase " + herencia.ChildNodes.ElementAt(0).FindTokenAndGetText() + " no existe");
                return null;
            }
            Simbolo metodo = buscarMetodo(father, simbolo);
            if (metodo == null)
            {
                ManejadorErrores.General("En la clase " + herencia.ChildNodes.ElementAt(0).FindTokenAndGetText() + " no existe un método que se desea sobreescribir " + simbolo.nombre);
                return null;
            }
            simbolo.tipo = metodo.tipo;
            return simbolo;
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

        public static Simbolo buscarClase(String archivo, String nombre) 
        {
            foreach (Simbolo simbolo in simbolos)
            {
                if (simbolo.archivo.CompareTo(archivo) == 0 
                    && simbolo.nombre.CompareTo(nombre)==0 
                    && simbolo.rol.Equals("Clase"))
                {
                    return simbolo;                    
                }
            
            }
            return null;        
        }

        public static Simbolo getSimbolo(Simbolo similar)
        {
            if (similar == null) 
            {
                return null;
            }
            foreach (Simbolo simbolo in simbolos) 
            {
                if (simbolo.archivo.CompareTo(similar.archivo) == 0 &&
                    simbolo.nombre.CompareTo(similar.nombre) == 0 &&
                    simbolo.rol.CompareTo(similar.rol) == 0 &&
                    simbolo.padre.CompareTo(similar.padre) == 0 &&
                    simbolo.parametros.CompareTo(similar.parametros) == 0 &&
                    simbolo.tipo == similar.tipo) 
                {
                    return simbolo;
                }
            }
            return null;
        }

        public static Simbolo buscarMetodo(ClaseJCode father, Simbolo metodo) {
            foreach (Simbolo simbolo in simbolos) {
                if (simbolo.rol.Equals(ConstantesJC.METODO, StringComparison.OrdinalIgnoreCase) &&
                    simbolo.nombre.Equals(metodo.nombre) && simbolo.parametros.Equals(metodo.parametros)) {
                        return simbolo;
                } 
            }
            return null;        
        }

        public static bool existeConstrutor(String nClase, String parametro){
            foreach (Simbolo simbolo in simbolos) {
                if (simbolo.rol.Equals(ConstantesJC.CONSTRUCTOR, StringComparison.OrdinalIgnoreCase) &&
                    simbolo.nombre.Equals(nClase) && simbolo.parametros.Equals(parametro, StringComparison.OrdinalIgnoreCase) &&
                    simbolo.padre.Equals(nClase))
                {
                    return true;
                } 
            }
            return false;
        }

        public static Simbolo buscarObj(String padre, String nombre) {
            foreach (Simbolo simbolo in simbolos) {
                if (simbolo.rol.Equals(Constantes.ROL_VAR) && simbolo.esGlobal && !simbolo.visibilidad.Equals("private", StringComparison.OrdinalIgnoreCase)
                    && simbolo.padre.Equals(padre, StringComparison.OrdinalIgnoreCase) && simbolo.nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)) {
                        return simbolo;                
                }
            }
            return null;
        }
    }
}
