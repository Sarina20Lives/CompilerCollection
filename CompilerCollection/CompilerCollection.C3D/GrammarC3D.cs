using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace CompilerCollection.CompilerCollection.C3D
{
    class GrammarC3D : Grammar
    {
        public GrammarC3D() : base (caseSensitive: false)
        {
            #region ER
            StringLiteral cadena = new StringLiteral("cadena", "\"", StringOptions.IsTemplate);
            StringLiteral caracter = new StringLiteral("caracter", "\'", StringOptions.IsTemplate);
            IdentifierTerminal id = new IdentifierTerminal("id");
            RegexBasedTerminal numero = new RegexBasedTerminal("numero", "[0-9]+");
            #endregion

            #region Terminales
            #endregion

            #region No terminales
            #endregion

            #region Gramatica
            #endregion

            #region Preferencias
            #endregion
        }
    }
}
