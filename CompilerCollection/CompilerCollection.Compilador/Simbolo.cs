using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;
using CompilerCollection.CompilerCollection.JCode;
using CompilerCollection.CompilerCollection.General;
using CompilerCollection.CompilerCollection.Utilidades;
using CompilerCollection.CompilerCollection.C3D;


namespace CompilerCollection.CompilerCollection.Compilador
{
    class Simbolo
    {
        public String archivo = "";
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
        public String referencia = "";
        public bool esGlobal = false;
        public bool instancia = false;
        public List<Dimension> dimensiones = new List<Dimension>();
        public int nivel = -1;
        public int tamanio = 1;

        public static Simbolo resolverParametro(Padre padre, ParseTreeNode raiz) {
            Simbolo simbolo = new Simbolo();
            simbolo.nombre = raiz.ChildNodes.ElementAt(0).FindTokenAndGetText();
            simbolo.padre = padre.nombre;
            simbolo.rol = "Parametro";
            simbolo.tipo = raiz.ChildNodes.ElementAt(1).FindTokenAndGetText();
            simbolo.visibilidad = padre.visibilidad;
            simbolo.pos = padre.pos;
            simbolo.archivo = padre.archivo;
            return simbolo;
        }

        public static Simbolo resolverClase(String archivo, ParseTreeNode raiz) {
            Simbolo simbolo = new Simbolo();
            simbolo.nombre = raiz.ChildNodes.ElementAt(0).FindTokenAndGetText();
            simbolo.padre = "";
            simbolo.rol = "Clase";
            simbolo.tipo = simbolo.nombre;
            simbolo.visibilidad = "Public";
            simbolo.archivo = archivo;
            simbolo.referencia = simbolo.nombre;

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
            simbolo.archivo = padre.archivo;
            simbolo.referencia = padre.nombre + "_" + simbolo.nombre;
            simbolo.pos = 0;
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
            simbolo.archivo = padre.archivo;
            simbolo.referencia = padre.nombre + "_" + simbolo.nombre + "_";

            foreach (ParseTreeNode hijo in raiz.ChildNodes) { 
                if(hijo.ToString().CompareTo (ConstantesJC.PARAMETROS)==0){
                    simbolo.contParametros = hijo.ChildNodes.Count();
                    simbolo.parametros = tiposDeparametrosAString(hijo);
                    simbolo.referencia += simbolo.parametros;
                }
            }
            return simbolo;
        }

        public static Simbolo resolverMetodo(Padre padre, ParseTreeNode raiz)
        {
            Simbolo simbolo = new Simbolo();
            simbolo.nombre = raiz.ChildNodes.ElementAt(0).FindTokenAndGetText();
            simbolo.padre = padre.nombre;
            simbolo.rol = "Metodo";
            simbolo.visibilidad = padre.visibilidad;
            simbolo.archivo = padre.archivo;
            simbolo.referencia = padre.nombre + "_" + simbolo.nombre + "_";

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
                    simbolo.referencia += simbolo.parametros;
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
            simbolo.archivo = padre.archivo;
            foreach (ParseTreeNode hijo in declaracion.ChildNodes)
            { 
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
                    if (simbolo.contDims != simbolo.dimensiones.Count()) 
                    {
                        ManejadorErrores.General("Las dimensiones no corresponden para el arreglo " + simbolo.nombre + " en la clase " + padre.clase);
                        return null;
                    }
                    simbolo.instancia = true;
                }
            }

            if (simbolo.esObjeto()) {
                if (Contexto.comprobarPermisoTipoObjeto(simbolo.tipo, padre.clase, simbolo.archivo)) {
                    return simbolo;
                }
                ManejadorErrores.General("Se hace referencia a la clase " + simbolo.tipo + " y la clase no es accesible desde la clase actual " + padre.clase);
                return null;
            }

            return simbolo;
        }

        public bool esObjeto() {
            if (this.tipo.Equals("int", StringComparison.OrdinalIgnoreCase)) {
                return false;
            }
            if (this.tipo.Equals("double", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            if (this.tipo.Equals("char", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            if (this.tipo.Equals("string", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            if (this.tipo.Equals("bool", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return true;
        }

        public static List<Dimension> resolverDimensiones(int cont, String tipo, ParseTreeNode asig) {
            List<Dimension> dimensiones = new List<Dimension>();
            if (asig.ChildNodes.ElementAt(0).ToString().CompareTo(ConstantesJC.CORCHETE) == 0) {
                dimensiones = contarDimensiones(asig.ChildNodes.ElementAt(0));
                if (cont != dimensiones.Count) {
                    ManejadorErrores.General("Las dimensiones no corresponden en la asignacion");
                    return new List<Dimension>();
                }
                return dimensiones;
            }

            if (tipo.CompareTo(asig.ChildNodes.ElementAt(0).FindTokenAndGetText()) != 0) {
                ManejadorErrores.General("Los tipos asignados no concuerdan ");
                return new List<Dimension>();
            }

            if (cont != asig.ChildNodes.ElementAt(1).ChildNodes.Count()) {
                ManejadorErrores.General("La cantidad de dimensiones no concuerdan ");
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
                tipos = tipos + parametro.ChildNodes.ElementAt(1).FindTokenAndGetText() + "_";            
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


        public String toHtml() {
            String html = "<tr>\n";
                html = html + "\t<td>" + this.archivo + "</td>\n";
                html = html + "\t<td>" + this.nombre +"</td>\n";
                html = html + "\t<td>" + this.padre +"</td>\n";
                html = html + "\t<td>" + this.rol +"</td>\n";
                html = html + "\t<td>" + this.tipo +"</td>\n";
                html = html + "\t<td>" + this.visibilidad +"</td>\n";
                if (this.pos == -1)
                {
                    html = html + "\t<td></td>\n";
                }
                else 
                {
                    html = html + "\t<td>" + this.pos + "</td>\n";
                }
                html = html + "\t<td>" + Convert.ToString(this.esArr) +"</td>\n";
                html = html + "\t<td>" + this.contDims + "</td>\n";
                html = html + "\t<td>" + this.contParametros + "</td>\n";
                html = html + "\t<td>" + this.parametros + "</td>\n";
                html = html + "\t<td>" + Convert.ToString(this.esGlobal) + "</td>\n";
                html = html + "\t<td>" + Convert.ToString(this.instancia) + "</td>\n";
                html = html + "\t<td>" + this.tamanio + "</td>\n";
                return html + "</tr>\n";        

        }
    }
}
