using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;
using CompilerCollection.CompilerCollection.JCode;
using CompilerCollection.CompilerCollection.General;


namespace CompilerCollection.CompilerCollection.Compilador
{
    class Simbolo
    {
        public String nombre = "";
        public String padre = "";
        public String rol = "";
        public String tipo = "No definido";
        public String visibilidad = "";
        public int pos = -1;
        public bool esRef = false;
        public bool esArr = false;
        public int contDims = 0;
        public int contParametros = 0;
        public String parametros = "";
        public bool esGlobal = false;
        public bool instancia = false;
        public List<Dimension> dimensiones = new List<Dimension>();
        public int nivel = -1;

        public static Simbolo resolverParametro(Padre padre, ParseTreeNode raiz) {
            Simbolo simbolo = new Simbolo();
            simbolo.nombre = raiz.ChildNodes.ElementAt(0).FindTokenAndGetText();
            simbolo.padre = padre.nombre;
            simbolo.rol = "Parametro";
            simbolo.tipo = raiz.ChildNodes.ElementAt(1).FindTokenAndGetText();
            simbolo.visibilidad = padre.visibilidad;
            simbolo.pos = padre.pos;
            padre.aumentarPos();
            return simbolo;
        }

        public static Simbolo resolverClase(ParseTreeNode raiz) {
            Simbolo simbolo = new Simbolo();
            simbolo.nombre = raiz.ChildNodes.ElementAt(0).FindTokenAndGetText();
            simbolo.padre = "";
            simbolo.rol = "Clase";
            simbolo.tipo = simbolo.nombre;
            simbolo.visibilidad = "Public";

            foreach (ParseTreeNode hijo in raiz.ChildNodes)
            {
                if (hijo.ToString().CompareTo(ConstantesJC.VISIBILIDAD) == 0)
                {
                    simbolo.visibilidad = hijo.ChildNodes.ElementAt(0).FindTokenAndGetText();
                }
                if (hijo.ToString().CompareTo(ConstantesJC.HERENCIA) == 0 && hijo.ChildNodes.Count > 0)
                {
                    simbolo.padre = hijo.ChildNodes.ElementAt(0).FindTokenAndGetText();
                }

            }
            return simbolo;        
        }

        public static Simbolo resolverPrincipal(Padre padre, ParseTreeNode raiz) 
        {
            Simbolo simbolo = new Simbolo();
            simbolo.nombre = "Main";
            simbolo.padre = padre.nombre;
            simbolo.rol = "Main";
            simbolo.tipo = "Void";
            simbolo.visibilidad = padre.visibilidad;
            return simbolo;
        }

        public static Simbolo resolverConstructor(Padre padre, ParseTreeNode raiz)
        {
            Simbolo simbolo = new Simbolo();
            if (padre.nombre.CompareTo(raiz.ChildNodes.ElementAt(0).FindTokenAndGetText()) != 0) { 
                //TODO:ERROR-El constructor no hace referencia a su clase padre.
                return null;
            }
            simbolo.nombre = padre.nombre;
            simbolo.padre = padre.nombre;
            simbolo.rol = "Constructor";
            simbolo.tipo = padre.nombre;
            simbolo.visibilidad = "Public";

            foreach (ParseTreeNode hijo in raiz.ChildNodes) { 
                if(hijo.ToString().CompareTo (ConstantesJC.PARAMETROS)==0){
                    simbolo.contParametros = hijo.ChildNodes.Count();
                    simbolo.parametros = tiposDeparametrosAString(hijo);
                }
            }
            return simbolo;
        }

        public static Simbolo resolverMetodo(Padre padre, ParseTreeNode raiz)
        {
            if(raiz.ToString().CompareTo(ConstantesJC.OVERRIDE)==0){
                return resolverMetodo(padre, raiz.ChildNodes.ElementAt(0));
            }
            Simbolo simbolo = new Simbolo();
            simbolo.nombre = raiz.ChildNodes.ElementAt(0).FindTokenAndGetText();
            simbolo.padre = padre.nombre;
            simbolo.rol = "Metodo";
            simbolo.visibilidad = padre.visibilidad;

            foreach (ParseTreeNode hijo in raiz.ChildNodes)
            {
                if(hijo.ToString().CompareTo(ConstantesJC.TIPO)==0)
                {
                    simbolo.tipo = hijo.ChildNodes.ElementAt(0).FindTokenAndGetText();
                }

                if (hijo.ToString().CompareTo(ConstantesJC.VISIBILIDAD) == 0)
                {
                    simbolo.visibilidad = hijo.ChildNodes.ElementAt(0).FindTokenAndGetText();
                }

                if (hijo.ToString().CompareTo(ConstantesJC.PARAMETROS) == 0)
                {
                    simbolo.contParametros = hijo.ChildNodes.Count();
                    simbolo.parametros = tiposDeparametrosAString(hijo);
                }
            }
            return simbolo;
        }


        public static Simbolo resolverDeclaracion(Padre padre, ParseTreeNode declaracion, bool esGlobal) {
            Simbolo simbolo = new Simbolo();
            if (declaracion.ChildNodes.ElementAt(0).ToString().CompareTo(ConstantesJC.DECLOCAL) == 0) {
                return resolverDeclaracion(padre, declaracion.ChildNodes.ElementAt(0), esGlobal);
            }
            simbolo.nombre = declaracion.ChildNodes.ElementAt(0).FindTokenAndGetText();
            simbolo.padre = padre.nombre;
            simbolo.rol = Constantes.ROL_VAR;
            simbolo.tipo = declaracion.ChildNodes.ElementAt(1).FindTokenAndGetText();
            simbolo.visibilidad = padre.visibilidad;
            simbolo.pos = padre.pos;
            simbolo.esGlobal = esGlobal;
            padre.aumentarPos();
            foreach (ParseTreeNode hijo in declaracion.ChildNodes) { 
                if(hijo.ToString().CompareTo(ConstantesJC.VISIBILIDAD)==0){
                    simbolo.visibilidad = hijo.ChildNodes.ElementAt(0).FindTokenAndGetText();                
                }
                if (hijo.ToString().CompareTo(ConstantesJC.CORCHETES) == 0) {
                    simbolo.esArr = true;
                    simbolo.contDims = hijo.ChildNodes.Count();
                }
                if (hijo.ToString().CompareTo(ConstantesJC.ASIGVAR) == 0) {
                    if (hijo.ChildNodes.Count > 0 && (hijo.ChildNodes.ElementAt(0)).FindTokenAndGetText().CompareTo("null") != 0) {
                        simbolo.instancia = true;
                    }
                }
                if (hijo.ToString().CompareTo(ConstantesJC.ASIGARR) == 0) {
                    simbolo.dimensiones = resolverDimensiones(simbolo.contDims, simbolo.tipo,  hijo);
                    simbolo.instancia = true;
                }
            }
            return simbolo;
        }


        public static List<Dimension> resolverDimensiones(int cont, String tipo, ParseTreeNode asig) {
            List<Dimension> dimensiones = new List<Dimension>();
            if (asig.ChildNodes.ElementAt(0).ToString().CompareTo(ConstantesJC.CORCHETE) == 0) {
                dimensiones = contarDimensiones(asig.ChildNodes.ElementAt(0));
                if (cont != dimensiones.Count) { 
                    //TODO:ERROR:LA CANTIDAD DE DIMENSIONES NO CONCUERDA
                    return new List<Dimension>();
                }
                return dimensiones;
            }

            if (tipo.CompareTo(asig.ChildNodes.ElementAt(0).FindTokenAndGetText()) != 0) { 
                //TODO:ERROR:LOS TIPOS ASIGNADOS NO CONCUERDAN
                return new List<Dimension>();
            }

            if (cont != asig.ChildNodes.ElementAt(1).ChildNodes.Count()) { 
                //TODO:ERROR:LA CANTIDAD DE DIMENSIONES NO CONCUERDA
                return new List<Dimension>();
            }

            Dimension dimension;
            foreach (ParseTreeNode dim in asig.ChildNodes.ElementAt(1).ChildNodes) {
                dimension = Dimension.crear(dim);
                if (dimension == null) {
                    return new List<Dimension>();
                }
                dimensiones.Add(dimension);
            }
            return dimensiones;
        }


        public static List<Dimension> contarDimensiones(ParseTreeNode dims) {
            List<Dimension> dimensiones = new List<Dimension>();
            Dimension dimension;
            if (dims.ChildNodes.Count == 1) {
                if (dims.ChildNodes.ElementAt(0).ToString().CompareTo(ConstantesJC.EXPRESIONES)==0) {
                    dimension = Dimension.crear(dims.ChildNodes.ElementAt(0).ChildNodes.Count);
                    if (dimension == null) 
                    {
                        return new List<Dimension>();
                    }
                    dimensiones.Add(dimension);
                    return dimensiones;
                }
            }
            dimension = Dimension.crear(dims.ChildNodes.Count);
            if (dimension == null)
            {
                return new List<Dimension>();
            }
            dimensiones.Add(dimension);
            dimensiones.AddRange(contarDimensiones(dims.ChildNodes.ElementAt(0)));
            return dimensiones;
        }


        public static String tiposDeparametrosAString(ParseTreeNode raiz) {
            String tipos = "";
            foreach (ParseTreeNode parametro in raiz.ChildNodes) {
                tipos = tipos + "_" + parametro.ChildNodes.ElementAt(1).FindTokenAndGetText();            
            }
            return tipos;
        }

        public static bool compararPorBloque(Simbolo uno, Simbolo dos)
        {
            if (uno.nombre.CompareTo(dos.nombre) == 0 &&
                uno.parametros.CompareTo(dos.parametros) == 0 &&
                uno.padre.CompareTo(dos.padre) == 0)
            {
                return true;
            }
            return false;
        }

        public static bool compararPorVariable(Simbolo uno, Simbolo dos)
        {
            if (uno.nombre.CompareTo(dos.nombre) == 0 &&
                uno.padre.CompareTo(dos.padre) == 0)
            {
                return true;
            }
            return false;
        }

    }
}
