using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;
using CompilerCollection.CompilerCollection.JCode;
using CompilerCollection.CompilerCollection.Utilidades;
using CompilerCollection.CompilerCollection.C3D;
using CompilerCollection.CompilerCollection.General;

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

        //Generación de contexto local inicial(cuando exitan parametros)
        public static Contexto generarContextoLocal(Simbolo ambito, ParseTreeNode parametros) {
            Padre padre = Padre.crear(ambito);
            Contexto ctx = new Contexto();
            if (parametros == null || parametros.ChildNodes.Count() == 0) {
                return ctx;
            }

            Simbolo param;
            foreach (ParseTreeNode parametro in parametros.ChildNodes) {
                param = Simbolo.resolverParametro(padre, parametro);
                param = TablaSimbolo.getSimbolo(param);
                if (param != null) {
                    if (param.esObjeto())
                    {
                        if (comprobarPermisoTipoObjeto(param.tipo, padre.clase, padre.archivo)) {
                            ctx.agregarVariable(ambito, param, false);                                             
                        }
                    }
                    else
                    {
                        ctx.agregarVariable(ambito, param, false);                                             
                    }
                }
            }
            return ctx;
        }

        //Declaraciones locales....
        public void agregarAlContextoLocal(Simbolo ambito, ParseTreeNode declaracion, Contexto ctxG, bool esInit)
        {
            Padre padre = Padre.crear(ambito);
            bool resultado = false;
            Simbolo simbolo = TablaSimbolo.getSimbolo(Simbolo.resolverDeclaracion(padre, declaracion, false));
            if (simbolo == null)
            {
                return;
            }

            if (simbolo.esObjeto())
            {
                if (comprobarPermisoTipoObjeto(simbolo.tipo, padre.clase, padre.archivo))
                {
                    resultado = this.agregarVariable(ambito, simbolo, false);
                    if (resultado) {
                        iniciarObj(ambito, ctxG, this, simbolo, declaracion, false, esInit);
                    }
                }
            }
            else
            {
                resultado = this.agregarVariable(ambito, simbolo, false);
                if (resultado)
                {
                    iniciarObj(ambito, ctxG, this, simbolo, declaracion, false, esInit);
                }
            }
        }



        //Declaraciones Globales...
        public static Contexto crearContextoGlobal(String clase)
        {
            Contexto ctx = new Contexto();
            
            ClaseJCode cl = Compilador.obtenerClasePorNombre(clase);
            if (cl == null) 
            {
                return ctx;
            }
            Padre padre = Padre.crearDeClase(cl.archivo, cl.clase);
            Simbolo ambito = TablaSimbolo.buscarClase(padre.archivo, padre.nombre);
            generarContextoGlobal(ambito, ctx, padre, cl.clase, false);

            //Si tiene una clase padre, agregar los atributos
            ClaseJCode clasePadre;
            ParseTreeNode herencia = ParserJcode.obtenerHerencia(cl.clase);
            if (herencia == null || herencia.ChildNodes.Count == 0) {
                return ctx;            
            }
            clasePadre = Compilador.obtenerClasePorNombre(herencia.ChildNodes.ElementAt(0).FindTokenAndGetText());
            if (clasePadre == null) {
                ManejadorErrores.Semantico("La clase padre no existe o no es accesible " + herencia.FindTokenAndGetText(),
                    herencia.Token.Location.Line, herencia.Token.Location.Column);
                return ctx;
            }

            if (comprobarPermisoUso(clasePadre.archivo, cl)) {
                padre = Padre.crearDeClase(clasePadre.archivo, clasePadre.clase);
                generarContextoGlobal(ambito, ctx, padre, clasePadre.clase, true);
            }
            return ctx;
        }

        public static bool comprobarPermisoUso(String archivoClaseDeseada, ClaseJCode claseActual) {
            if (archivoClaseDeseada.CompareTo(claseActual.archivo) == 0) {
                return true;
            }
            ParseTreeNode imports = ParserJcode.obtenerImports(claseActual.clase);
            if (imports == null) {
                return false;
            }
            foreach (ParseTreeNode import in imports.ChildNodes) {
                if (import.FindTokenAndGetText().CompareTo(archivoClaseDeseada) == 0) {
                    return true;
                }
            }
            return false;
        }


        public static bool comprobarPermisoTipoObjeto(String claseDeseada, String claseActual, String archivoActual)
        {

            ClaseJCode cl = Compilador.obtenerClasePorNombre(claseDeseada);
            if (cl == null) {
                ManejadorErrores.General("No se ha encontrado una clase con nombre "+ claseDeseada);
                return false;
            }

            if(cl.archivo.CompareTo(archivoActual)==0){
                return true;
            }

            ClaseJCode actual = Compilador.obtenerClasePorNombre(claseActual);
            if (actual == null){
                return false;
            }

            ParseTreeNode imports = ParserJcode.obtenerImports(actual.clase);
            if (imports == null) {
                return false;
            }

            foreach (ParseTreeNode import in imports.ChildNodes) {
                if (import.FindTokenAndGetText().CompareTo(cl.archivo) == 0) {
                    return true;
                }
            }

            ParseTreeNode padre = ParserJcode.obtenerHerencia(actual.clase);
            if (padre == null || padre.ChildNodes.Count()==0) {
                return false;
            }

            if (padre.ChildNodes.ElementAt(0).FindTokenAndGetText().CompareTo(claseDeseada) == 0) {
                return true;
            }
            return false;
        }



        //Generación de contexto global para una clase recibida como ParseTreeNode raiz
        public static void generarContextoGlobal(Simbolo ambito, Contexto ctx, Padre padre, ParseTreeNode raiz, bool esInclude) 
        { 
            Simbolo simbolo;
            bool resultado = false;
            foreach (ParseTreeNode hijo in raiz.ChildNodes)
            {
                if (hijo.ToString().CompareTo(ConstantesJC.ELEMENTOS) == 0)
                {
                    foreach (ParseTreeNode sentencia in hijo.ChildNodes) 
                    { 
                        if(sentencia.ToString().CompareTo(ConstantesJC.DECGLOBAL)==0)
                        {
                            simbolo = TablaSimbolo.getSimbolo(Simbolo.resolverDeclaracion(padre, sentencia, true));
                            if (simbolo != null)
                            {
                                resultado = ctx.agregarVariable(ambito, simbolo, esInclude);

                            }
                        }
                    
                    }
                }
            }
        }

        

        //Verificación y actualización de variables en la tabla de símbolos
        private bool agregarVariable(Simbolo ambito, Simbolo simbolo, bool EsInclude)
        {
            if (EsInclude && simbolo.visibilidad.CompareTo("private") == 0)
            {
                return false;
            }

            if (EsInclude) {
                simbolo.pos = ambito.tamanio;
                ambito.tamanio += 1;
            }

            foreach (Simbolo sim in this.simbolos)
            {
                if (Simbolo.compararPorVariable(sim, simbolo))
                {
                    ManejadorErrores.General("La variable " + simbolo.nombre + " no se ha agregado, debido a que ya existe una variable con ese nombre");
                    return false;
                }
            }
            this.simbolos.Add(simbolo);
            return true;
        }

        //Método que verifica si existe una instancia inicial y escribe su c3d respectivo...
        public static void iniciarObj(Simbolo ambito, Contexto ctxG, Contexto ctxL, Simbolo obj, ParseTreeNode declaracion, bool esGlobal, bool esInit) {
            if (declaracion.ChildNodes.ElementAt(0).ToString().CompareTo(ConstantesJC.DECLOCAL) == 0) {
                iniciarObj(ambito, ctxG, ctxG, obj, declaracion.ChildNodes.ElementAt(0), esGlobal, esInit);
            }

            ParseTreeNode instancia = ParserJcode.obtenerInicializacion(declaracion);
            if (instancia == null || instancia.ChildNodes.Count()==0) {
                return;
            }


            //Resolver la asignacion
            C3d solucion = new C3d();
            if (instancia.ToString().CompareTo(ConstantesJC.ASIGVAR) == 0)
            {
                solucion = resolverAsigVar(ctxG, ctxL, ambito, obj, instancia, esInit);
                if (solucion == null) {
                    solucion = new C3d();
                    solucion.cad = "NULL";
                }
            }
            if (instancia.ToString().CompareTo(ConstantesJC.ASIGARR) == 0)
            {
                //solucion = resolverAsigArr();
            }

            //Obtener pos
            String posicion = "";
            if (esGlobal && obj.esGlobal)
            {
                String pthis = C3d.generarTemp();
                C3d.escribirOperacion(pthis, "P", "+", "1", esInit);  //Pos del this
                String vthis = C3d.leerDePila(pthis, esInit); //Valor del this
                posicion = C3d.generarTemp();
                C3d.escribirOperacion(posicion, vthis, "+", obj.pos.ToString(), esInit); //Pos real del objeto
                C3d.escribirEnHeap(posicion, solucion.cad, esInit); //Guardar en el heap el valor actualizado en solucion.cad
            }
            else
            {
                posicion = C3d.generarTemp();
                C3d.escribirOperacion(posicion, "P", "+", obj.pos.ToString(), esInit);
                C3d.escribirEnPila(posicion, solucion.cad, esInit);
            }
        }








        public static C3d resolverAsigVar(Contexto ctxG, Contexto ctxL, Simbolo ambito, Simbolo simbolo, ParseTreeNode instancia, bool esInit)
        {
            C3d solucion = new C3d();
            Expresion expresion = null;

            //null;_____________________________________________________________________________________________________
            if (instancia.ChildNodes.Count == 1 && instancia.ChildNodes.ElementAt(0).Term.ToString().Equals("null", StringComparison.OrdinalIgnoreCase)) {
                solucion.cad = "NULL";
                return solucion;
            }

            //Expresion;________________________________________________________________________________________________
            if (instancia.ChildNodes.ElementAt(0).ToString().CompareTo(ConstantesJC.EXPRESION) == 0)
            {
                expresion = new Expresion(ctxG, ctxL, ambito, esInit);
                solucion = expresion.resolver(instancia.ChildNodes.ElementAt(0));
                if (solucion == null)
                {
                    return null;
                }
                //(C3d op, int tipo, int tamanio, bool esInit)
                solucion = C3d.castearA(solucion, simbolo.tipo, ambito.tamanio, esInit);
                return solucion;
            }

            //New ID(parametros);_______________________________________________________________________________________
            //Comprobar tipo objeto
            if (!simbolo.esObjeto())
            {
                ManejadorErrores.General("La variable " + simbolo.nombre + " no es de tipo objeto, no se puede instanciar");
                return null; 
            }
            //Comprobar que las clases sean las mismas
            String nClase = instancia.ChildNodes.ElementAt(0).FindTokenAndGetText();
            if (!simbolo.tipo.Equals(nClase, StringComparison.OrdinalIgnoreCase)) {
                ManejadorErrores.General("No se hace referencia a la misma clase " + simbolo.tipo + "!="+ nClase);
                return null;             
            }
            //Comprobar la existencia de la clase            
            ClaseJCode clase = Compilador.obtenerClasePorNombre(nClase);
            if (clase == null) {
                ManejadorErrores.General("No existe una clase con nombre " + nClase);
                return null;
            }
            //Comprobar que es un tipo de objeto permitido
            if (!comprobarPermisoTipoObjeto(nClase, simbolo.tipo, simbolo.archivo)) {
                ManejadorErrores.General("La clase " + nClase + " no es accesible desde la clase actual");
                return null;
            }
            
            String parametro = "";

            if (instancia.ChildNodes.Count() == 2 && instancia.ChildNodes.ElementAt(1).ToString().Equals(ConstantesJC.EXPRESIONES)) {
                String tax = C3d.generarTemp();
                String tparam = C3d.generarTemp();
                C3d.escribirOperacion(tax, "P", "+", ambito.tamanio.ToString(), esInit);
                C3d valorParam;
                int cont = 2;
                foreach (ParseTreeNode exp in instancia.ChildNodes.ElementAt(1).ChildNodes) {
                    expresion = new Expresion(ctxG, ctxL, ambito, esInit);
                    valorParam = expresion.resolver(exp);
                    C3d.escribirOperacion(tparam, tax, "+", cont.ToString(), esInit);
                    if (valorParam == null || valorParam.tipo == Constantes.ERROR)
                    {
                        C3d.escribirEnPila(tparam, "NULL", esInit);
                        parametro += "NULL_";
                    }
                    else {
                        C3d.escribirEnPila(tparam, valorParam.cad, esInit);
                        parametro += Constantes.TIPOS[valorParam.tipo] + "_";
                    }
                    cont++;
                }            
            }

            if (TablaSimbolo.existeConstrutor(nClase, parametro)) {
                C3d.aumentarP(ambito.tamanio.ToString(), esInit);
                C3d.escribir(nClase + "_" + nClase + "_" + parametro + "();", esInit);
                String tx = C3d.generarTemp();
                C3d.escribirOperacion(tx, "P", "+", "0", esInit);
                solucion.cad = C3d.leerDePila(tx, esInit);
                C3d.disminuirP(ambito.tamanio.ToString(), esInit);
                return solucion;
            }

            return null;
        }

    }
}
