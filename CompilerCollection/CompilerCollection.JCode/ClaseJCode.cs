using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace CompilerCollection.CompilerCollection.JCode
{
    public class ClaseJCode
    {
        public String archivo = "";
        public bool esPrincipal = false;
        public ParseTreeNode clase = null;

        public static ClaseJCode crearClase(String archivo, bool esPrincipal, ParseTreeNode clase) 
        {
            ClaseJCode cl = new ClaseJCode();
            cl.archivo = archivo;
            cl.esPrincipal = esPrincipal;
            cl.clase = clase;
            return cl;
        }
    
        

    }
}
