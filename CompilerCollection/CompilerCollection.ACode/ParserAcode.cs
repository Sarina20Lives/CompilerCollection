using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace CompilerCollection.CompilerCollection.ACode
{
    class ParserAcode : Grammar
    {
        public static ParseTreeNode generarAst(String cadena) {
            GrammarAcode gramatica = new GrammarAcode();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(cadena);
            ParseTreeNode raiz = arbol.Root;
            return raiz;        
        }
    }
}
