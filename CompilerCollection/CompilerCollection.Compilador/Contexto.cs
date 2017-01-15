using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;
using CompilerCollection.CompilerCollection.JCode;
using CompilerCollection.CompilerCollection.Utilidades;

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


        
        public static void generarContextoGlobal(Simbolo ambito, Contexto ctx, Padre padre, ParseTreeNode raiz, bool esInclude) 
        { 
            Simbolo simbolo;
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
                                ctx.agregarVariable(ambito, simbolo, esInclude);
                            }
                        }
                    
                    }
                }
            }
        }

        


        private void agregarVariable(Simbolo ambito, Simbolo simbolo, bool EsInclude)
        {
            if (EsInclude && simbolo.visibilidad.CompareTo("private") == 0)
            {
                return;
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
                    return;
                }
            }
            this.simbolos.Add(simbolo);
        }


    }
}
