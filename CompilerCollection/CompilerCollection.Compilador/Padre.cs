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
        public String nombre = "";
        public String visibilidad = "";
        public int pos = 2;

        public void aumentarPos() {
            this.pos = this.pos + 1;
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

        public static Padre crearDeClase(ParseTreeNode raiz) {
            Padre padre = new Padre();
            padre.nombre = raiz.ChildNodes.ElementAt(0).FindTokenAndGetText();
            padre.visibilidad = "Public";
            if (raiz.ChildNodes.ElementAt(1).ToString().CompareTo(ConstantesJC.VISIBILIDAD)==0) {
                padre.visibilidad = raiz.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).FindTokenAndGetText();
            }
            return padre;
        }

        public static Padre crearDeMetodo(Padre anterior, ParseTreeNode raiz)
        {
            Padre padre = new Padre();
            padre.nombre = anterior.nombre + "_" + raiz.ChildNodes.ElementAt(0).FindTokenAndGetText() + "_";
            padre.visibilidad = anterior.visibilidad;
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
            return padre;
        }
    }
}
