using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace CompilerCollection.CompilerCollection.JCode
{
    class ParserJcode : Grammar
    {
        public static List<ClaseJCode> generarAst(String archivo, String cadena)
        {
            GrammarJcode gramatica = new GrammarJcode();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(cadena);
            ParseTreeNode principal = arbol.Root;
            //No se ha realizado el análisis
            if (principal == null) {
                return null;
            }
            List<ClaseJCode> clases = obtenerClases(archivo, principal, true);
            obtenerImportadas(clases, principal);
            return clases;
        }

        private static void obtenerImportadas(List<ClaseJCode> clases, ParseTreeNode raiz) {
            ParseTreeNode imports = obtenerImports(raiz);
            if (imports == null) {
                return;
            }

            String cadena = "";
            String nombre = "";
            List<ClaseJCode> nuevas = new List<ClaseJCode>();
            foreach (ParseTreeNode import in imports.ChildNodes) {
                nombre = import.FindTokenAndGetText();
                cadena = Utilidades.ManejadorArchivo.buscarContenidoArchivoImport(nombre);
                nuevas = generarAst(nombre, cadena);
                if (nuevas == null)
                {
                    //Error en el archivo de entrada con nombre
                }
                else
                {
                    agregarClasesImport(clases, nuevas);
                }
            }
        }

        private static void agregarClasesImport(List<ClaseJCode> clases, List<ClaseJCode> nuevas) {          
            foreach (ClaseJCode nueva in nuevas) { 
                if(!existeClase(clases, nueva)){
                    clases.Add(nueva);
                }
            }
        }

        private static List<ClaseJCode> obtenerClases(String archivo, ParseTreeNode raiz, bool esPrincipal){
            ParseTreeNode imports = obtenerImports(raiz);
            ParseTreeNode raizClases = obtenerRaizClases(raiz);
            List<ClaseJCode> clases = new List<ClaseJCode>();
            ClaseJCode cl = new ClaseJCode();
            foreach (ParseTreeNode clase in raizClases.ChildNodes) {
                if (imports != null) {
                    clase.ChildNodes.Add(imports);
                }
                cl = ClaseJCode.crearClase(archivo, esPrincipal, clase);
                if (!existeClase(clases, cl))
                {
                    clases.Add(cl);
                }
            }
            return clases;        
        }

        private static bool existeClase(List<ClaseJCode> clases, ClaseJCode clase) {
            foreach (ClaseJCode temp in clases) {
                if (temp.clase.ChildNodes.ElementAt(0).ToString().CompareTo(clase.clase.ChildNodes.ElementAt(0).ToString())==0) {
                    //TODO:ERROR-Existe redundancia en las clases...                                        
                    return true;
                }
            }
            return false;
        }

        private static ParseTreeNode obtenerRaizClases(ParseTreeNode raiz){
            foreach (ParseTreeNode hijo in raiz.ChildNodes) {
                if (hijo.ToString() == "jcode")
                {
                    return obtenerRaizClases(hijo);
                }
                if (hijo.ToString() == "clases")
                {
                    return hijo;
                }
            }
            return null;
        }

        public static ParseTreeNode obtenerImports(ParseTreeNode raiz){
           
            foreach (ParseTreeNode hijo in raiz.ChildNodes) {
                if (hijo.ToString() == "jcode") {
                    return obtenerImports(hijo);
                }
                if (hijo.ToString() == "imports") {
                    return hijo;
                }
            }
            return null;
        }
    }
}
