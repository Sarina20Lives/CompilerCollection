using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace CompilerCollection.CompilerCollection.ACode
{
    class GrammarAcode : Grammar
    {

        public GrammarAcode() : base (caseSensitive: false)
        {
            #region ER
            StringLiteral cadena = new StringLiteral("cadena", "\"", StringOptions.IsTemplate);
            StringLiteral caracter = new StringLiteral("caracter", "\'", StringOptions.IsTemplate);
            IdentifierTerminal id = new IdentifierTerminal("id");
            RegexBasedTerminal numero = new RegexBasedTerminal("numero", "[0-9]+");
            #endregion

            #region Comentarios
            CommentTerminal COMENTARIO_LINEA = new CommentTerminal("COMENTARIO_LINEA", "!--", "\n");
            CommentTerminal COMENTARIO_LINEAS = new CommentTerminal("COMENTARIO_LINEAS", "!!-", "-!!");
            base.NonGrammarTerminals.Add(COMENTARIO_LINEA);
            base.NonGrammarTerminals.Add(COMENTARIO_LINEAS);
            #endregion

            #region Terminales
            var etiqueta = ToTerm("%%");

            #endregion

            #region No terminales
            NonTerminal inicio = new NonTerminal("inicio");
            
            #endregion

            #region Gramatica
            #endregion

            #region Preferencias
            #endregion
        }



    }
}
