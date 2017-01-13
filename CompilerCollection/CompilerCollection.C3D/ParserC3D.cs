using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace CompilerCollection.CompilerCollection.C3D
{
    class ParserC3D : Grammar
    {
        public static ParseTreeNode generarAst(String cadena)
        {
            GrammarC3D gramatica = new GrammarC3D();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(cadena);
            ParseTreeNode raiz = arbol.Root;
            return raiz;
        }
    }
}
