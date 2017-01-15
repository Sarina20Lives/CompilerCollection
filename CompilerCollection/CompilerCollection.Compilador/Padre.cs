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
    class Padre
    {
        public String archivo = "";
        public String nombre = "";
        public String visibilidad = "";
        public String clase = "";
        public int pos = 2;

        public void aumentarPos() {
            this.pos = this.pos + 1;
        }

        public static Padre crear(Simbolo simbolo) {
            Padre padre = new Padre();
            padre.nombre = simbolo.referencia;
            padre.visibilidad = simbolo.visibilidad;
            padre.archivo = simbolo.archivo;
            padre.clase = simbolo.padre;
            if (simbolo.rol.CompareTo("Clase") == 0)
            {
                padre.clase = padre.nombre;
                padre.pos = 0;
            }            
            return padre;
        }

        public static Padre crear(String nombre) {
            Padre padre = new Padre();
            padre.nombre = nombre;
            padre.visibilidad = "public";
            padre.pos = 2;
            return padre;        
        }

        public static Padre crear(String nombre, String visibilidad)
        {
            Padre padre = new Padre();
            padre.nombre = nombre;
            padre.visibilidad = visibilidad;
            padre.pos = 2;
            return padre;
        }

        public static Padre crearDeClase(String archivo, ParseTreeNode raiz) {
            Padre padre = new Padre();
            padre.nombre = raiz.ChildNodes.ElementAt(0).FindTokenAndGetText();
            padre.visibilidad = "Public";
            padre.archivo = archivo;
            padre.clase = padre.nombre;
            padre.pos = 0;
            if (raiz.ChildNodes.ElementAt(1).ToString().CompareTo(ConstantesJC.VISIBILIDAD) == 0)
            {
                padre.visibilidad = raiz.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).FindTokenAndGetText();
            }
            return padre;
        }

        public static Padre crearDeMetodo(Padre anterior, ParseTreeNode raiz)
        {
            Padre padre = new Padre();
            padre.nombre = anterior.nombre + "_" + raiz.ChildNodes.ElementAt(0).FindTokenAndGetText() + "_";
            padre.visibilidad = anterior.visibilidad;
            padre.archivo = anterior.archivo;
            padre.clase = anterior.nombre;
            foreach (ParseTreeNode hijo in raiz.ChildNodes)
            {
                if (hijo.ToString().CompareTo(ConstantesJC.VISIBILIDAD) == 0)
                {
                    padre.visibilidad = hijo.ChildNodes.ElementAt(0).FindTokenAndGetText();
                }
                if (hijo.ToString().CompareTo(ConstantesJC.PARAMETROS) == 0) 
                {
                    padre.nombre = padre.nombre + Simbolo.tiposDeparametrosAString(hijo);
                }
            }
            return padre;
        }

        public static Padre crearDePrincipal(Padre anterior, ParseTreeNode raiz)
        {
            Padre padre = new Padre();
            padre.nombre = anterior.nombre + "_Main";
            padre.visibilidad = anterior.visibilidad;
            padre.archivo = anterior.archivo;
            padre.clase = anterior.nombre;
            return padre;
        }
    }
}
