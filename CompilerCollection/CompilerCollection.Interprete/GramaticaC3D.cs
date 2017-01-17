using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerCollection.CompilerCollection.Interprete
{
    class GramaticaC3D : Grammar
    {
        public const string RUTA_C3D = "C:\\FilesCompilerCollection\\c3d.txt";
        public const string RUTA_C3D_OPTIMIZADO = "C:\\FilesCompilerCollection\\c3d(optimizado).txt";

        public static ParseTreeNode AnalizarC3D()
        {
            string c3d = File.ReadAllText(RUTA_C3D);
            GramaticaC3D gramatica = new GramaticaC3D();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(c3d);
            return arbol.Root;
        }

        public GramaticaC3D()
            : base(caseSensitive: false)
        {

            #region Terminales
            IdentifierTerminal 
                _id = new IdentifierTerminal("id", "$_");

            RegexBasedTerminal
                _etq = new RegexBasedTerminal("etiqueta", "L[0-9]+"),
                _temp = new RegexBasedTerminal("temporal", "t[0-9]+"),
                _entero = new RegexBasedTerminal("entero", "[0-9]+"),
                _decimal = new RegexBasedTerminal("decimal", "[0-9]+(\\.[0-9]+)");

            Terminal
                _main = ToTerm("main"),
                _void = ToTerm("void"),
                _goto = ToTerm("goto"),
                _if = ToTerm("if"),
                _stack = ToTerm("Stack"),
                _heap = ToTerm("Heap"),
                _P = ToTerm("P"),
                _H = ToTerm("H"),
                _NULL = ToTerm("NULL"),
                _printf = ToTerm("printf"),
                _create = ToTerm("create"),
                _select = ToTerm("select"),
                _insert = ToTerm("insert"),
                _delete = ToTerm("delete"),
                _update = ToTerm("update"),
                _drop = ToTerm("drop"),
                _intToStr = ToTerm("intToStr"),
                _doubleToStr = ToTerm("doubleToStr"),
                _charToStr = ToTerm("charToStr"),
                _boolToStr = ToTerm("boolToStr"),
                _compareStr = ToTerm("compareStr"),
                _outString = ToTerm("outString"),
                _error = ToTerm("error"),
                _breakpoint = ToTerm("breakpoint");

            Terminal
                _mas = ToTerm("+"),
                _men = ToTerm("-"),
                _por = ToTerm("*"),
                _div = ToTerm("/"),
                _mod = ToTerm("%"),
                _pot = ToTerm("^");

            Terminal
                _equ = ToTerm("=="),
                _dif = ToTerm("!="),
                _gte = ToTerm(">="),
                _lte = ToTerm("<="),
                _gt  = ToTerm(">"),
                _lt  = ToTerm("<");

            Terminal
                _fc = ToTerm("\"%c\""),
                _fd = ToTerm("\"%d\""),
                _ff = ToTerm("\"%f\"");
            #endregion

            #region No Terminales
            NonTerminal
                programa = new NonTerminal("programa"),
                elemento = new NonTerminal("elemento"),
                main = new NonTerminal("principal"),
                metodo = new NonTerminal("metodo"),
                cuerpo = new NonTerminal("cuerpo"),
                sentencia = new NonTerminal("sentencia"),

                etiqueta = new NonTerminal("etiqueta"),
                llamada = new NonTerminal("llamada"),
                acceso_stack = new NonTerminal("acceso a stack"),
                asigna_stack = new NonTerminal("asignación a stack"),
                acceso_heap = new NonTerminal("acceso a heap"),
                asigna_heap = new NonTerminal("asignación a heap"),
                suma = new NonTerminal("suma"),
                resta = new NonTerminal("resta"),
                multi = new NonTerminal("multiplicación"),
                divide = new NonTerminal("división"),
                modulo = new NonTerminal("módulo"),
                potencia = new NonTerminal("potencia"),
                asignacion = new NonTerminal("asignación"),
                salto = new NonTerminal("salto"),
                salto_equ = new NonTerminal("salto si igual"),
                salto_dif = new NonTerminal("salto si diferente"),
                salto_gte = new NonTerminal("salto si mayor o igual"),
                salto_lte = new NonTerminal("salto si menor o igual"),
                salto_gt = new NonTerminal("salto si mayor"),
                salto_lt = new NonTerminal("salto si menor"),
                printf = new NonTerminal("imprime"),
                nonsql = new NonTerminal("nonsql"),
                core = new NonTerminal("core"),
                breakpoint = new NonTerminal("breakpoint"),

                valor = new NonTerminal("valor"),
                destino = new NonTerminal("destino"),
                formatos = new NonTerminal("formatos");
            #endregion

            #region Gramática
            programa.Rule
                = MakeStarRule(programa, elemento)
            ;

            elemento.Rule
                = metodo 
                | main
            ;

            metodo.Rule
                = _void + _id + "(" + ")" + "{" + cuerpo + "}"
                
            ;

            main.Rule
                = _void + _main + "(" + ")" + "{" + cuerpo + "}" 
            ;

            cuerpo.Rule
                = MakePlusRule(cuerpo, sentencia)
            ;

            sentencia.Rule
                = etiqueta
                | llamada 
                | acceso_heap
                | acceso_stack
                | asigna_stack
                | asigna_heap
                | suma
                | resta
                | multi
                | divide
                | modulo
                | potencia
                | asignacion
                | salto
                | salto_equ
                | salto_dif
                | salto_gte
                | salto_lte
                | salto_gt 
                | salto_lt 
                | printf
                | nonsql
                | core
                | breakpoint
            ;

            breakpoint.Rule
                = _breakpoint
            ;

            etiqueta.Rule
                = _etq + ":"
            ;

            llamada.Rule
                = _id + "(" + ")" + ";"
            ;

            acceso_stack.Rule
                = destino + "=" + _stack + "[" + valor + "]" + ";"
            ;

            asigna_stack.Rule
                = _stack + "[" + valor + "]" + "=" + valor + ";"
            ;

            acceso_heap.Rule
                = destino + "=" + _heap + "[" + valor + "]" + ";"
            ;

            asigna_heap.Rule
                = _heap + "[" + valor + "]" + "=" + valor + ";"
            ;

            suma.Rule
                = destino + "=" + valor + _mas + valor + ";"
            ;

            resta.Rule
                = destino + "=" + valor + _men + valor + ";"
            ;

            multi.Rule
                = destino + "=" + valor + _por + valor + ";"
            ;

            divide.Rule
                = destino + "=" + valor + _div + valor + ";"
            ;

            modulo.Rule
                = destino + "=" + valor + _mod + valor + ";"
            ;

            potencia.Rule
                = destino + "=" + valor + _pot + valor + ";"
            ;

            asignacion.Rule
                = destino + "=" + valor + ";"
            ;

            salto.Rule
                = _goto + _etq + ";"
            ;

            salto_equ.Rule
                = _if + "(" + valor + _equ + valor + ")" + _goto + _etq + ";"
            ;

            salto_dif.Rule
                = _if + "(" + valor + _dif + valor + ")" + _goto + _etq + ";"
            ;

            salto_gte.Rule
	            = _if + "(" + valor + _gte + valor + ")" + _goto + _etq + ";"
            ;

            salto_lte.Rule
	            = _if + "(" + valor + _lte + valor + ")" + _goto + _etq + ";"
            ;

            salto_gt.Rule
	            = _if + "(" + valor + _gt + valor + ")" + _goto + _etq + ";"
            ;

            salto_lt.Rule
	            = _if + "(" + valor + _lt + valor + ")" + _goto + _etq + ";"
            ;

            printf.Rule 
                = _printf + "(" + formatos + "," + valor + ")" + ";"
            ;

            nonsql.Rule
                = _create + "(" + ")" + ";"
                | _select + "(" + ")" + ";"
                | _insert + "(" + ")" + ";"
                | _update + "(" + ")" + ";"
                | _delete + "(" + ")" + ";"
                | _drop + "(" + ")" + ";"
            ;

            core.Rule
                = _intToStr + "(" + ")" + ";"
                | _doubleToStr + "(" + ")" + ";"
                | _charToStr + "(" + ")" + ";"
                | _boolToStr + "(" + ")" + ";"
                | _compareStr + "(" + ")" + ";"
                | _outString + "(" + ")" + ";"
                | _error + "(" + ")" + ";"
            ;

            formatos.Rule = _fc | _fd | _ff ;

            valor.Rule
                = _entero
                | _decimal
                | _temp
                | _H
                | _P
                | _NULL
            ;

            destino.Rule
                = _temp
                | _H
                | _P
            ;
            #endregion

            #region Personalización
            CommentTerminal _comentario = new CommentTerminal("comentario", "//", "\n");
            NonGrammarTerminals.Add(_comentario);
            this.Root = programa;
            MarkPunctuation(";", ",", ":", "(", ")", "[", "]", "{", "}", "=");
            MarkPunctuation(_mas, _men, _por, _div, _mod, _pot);
            MarkPunctuation(_equ, _dif, _gte, _lte, _gt, _lt);
            MarkPunctuation(_void, _goto, _if, _printf, _stack, _heap);
            MarkTransient(elemento, formatos, valor, destino, sentencia);
            #endregion

        }
    }
}
