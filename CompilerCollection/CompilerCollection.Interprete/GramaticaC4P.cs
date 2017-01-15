using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerCollection.CompilerCollection.Interprete
{
    class GramaticaC4P : Grammar
    {
        private const string RUTA_C4P = "C:\\FilesCompilerCollection\\cuadruplos.txt";

        public static ParseTreeNode AnalizarC4P()
        {
            string c4p = File.ReadAllText(RUTA_C4P);
            GramaticaC4P gramatica = new GramaticaC4P();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(c4p);
            return RefactorizarAST(arbol.Root);
        }

        private static ParseTreeNode RefactorizarAST(ParseTreeNode programa)
        {
            if (programa == null)
                return null;
            for (int i = 0; i < programa.ChildNodes.Count; i++ )
            {
                var metodo = programa.ChildNodes[i];
                metodo.ChildNodes.RemoveAt(2);
                metodo.ChildNodes[1] = RefactorizarMetodo(metodo.ChildNodes[1]);
                programa.ChildNodes[i] = metodo;
            }
            return programa;
        }

        private static ParseTreeNode RefactorizarMetodo(ParseTreeNode metodo)
        {
            for (int i = 0; i < metodo.ChildNodes.Count; i++)
            {
                var sentencia = metodo.ChildNodes[i];
                switch (sentencia.ToString())
                {
                    case "acceso a stack":
                        //Ejecutar acceso a stack
                        sentencia = RefactorizarAccesoArreglo(sentencia);
                        break;
                    case "acceso a heap":
                        //Ejecutar acceso a heap
                        sentencia = RefactorizarAccesoArreglo(sentencia);
                        break;
                    case "suma":
                        //Ejecutar suma
                        sentencia = RefactorizarAritmetica(sentencia);
                        break;
                    case "resta":
                        //Ejecutar resta
                        sentencia = RefactorizarAritmetica(sentencia);
                        break;
                    case "multiplicación":
                        //Ejecutar multiplicación
                        sentencia = RefactorizarAritmetica(sentencia);
                        break;
                    case "división":
                        //Ejecutar división
                        sentencia = RefactorizarAritmetica(sentencia);
                        break;
                    case "módulo":
                        //Ejecutar módulo
                        sentencia = RefactorizarAritmetica(sentencia);
                        break;
                    case "potencia":
                        //Ejecutar potencia
                        sentencia = RefactorizarAritmetica(sentencia);
                        break;
                    case "asignación":
                        //Ejecutar asignación
                        sentencia = RefactorizarAsignacion(sentencia);
                        break;
                    case "imprime":
                        //Ejecutar función para imprimir a la salida
                        sentencia = RefactorizarImprime(sentencia);
                        break;
                    case "etiqueta":
                    case "llamada":
                    case "asignación a stack":
                    case "asignación a heap":
                    case "nonsql":
                    case "core":
                    case "salto":
                    case "salto si igual":
                    case "salto si diferente":
                    case "salto si mayor o igual":
                    case "salto si menor o igual":
                    case "salto si mayor":
                    case "salto si menor":
                        break;
                    default:
                        break;
                }
                metodo.ChildNodes[i] = sentencia;
            }
            return metodo;
        }

        private static ParseTreeNode RefactorizarImprime(ParseTreeNode sentencia)
        {
            var formato = sentencia.ChildNodes[1];
            var valor = sentencia.ChildNodes[0];
            sentencia.ChildNodes[0] = formato;
            sentencia.ChildNodes[1] = valor;
            return sentencia;
        }

        private static ParseTreeNode RefactorizarAsignacion(ParseTreeNode sentencia)
        {
            var destino = sentencia.ChildNodes[1];
            var valor = sentencia.ChildNodes[0];
            sentencia.ChildNodes[0] = destino;
            sentencia.ChildNodes[1] = valor;
            return sentencia;
        }

        private static ParseTreeNode RefactorizarAritmetica(ParseTreeNode sentencia)
        {
            var destino = sentencia.ChildNodes[2];
            var valorIzq = sentencia.ChildNodes[0];
            var valorDer = sentencia.ChildNodes[1];
            sentencia.ChildNodes[0] = destino;
            sentencia.ChildNodes[1] = valorIzq;
            sentencia.ChildNodes[2] = valorDer;
            return sentencia;
        }

        private static ParseTreeNode RefactorizarAccesoArreglo(ParseTreeNode sentencia)
        {
            var destino = sentencia.ChildNodes[1];
            var indice = sentencia.ChildNodes[0];
            sentencia.ChildNodes[0] = destino;
            sentencia.ChildNodes[1] = indice;
            return sentencia;
        }

        public GramaticaC4P()
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
                _begin = ToTerm("begin"),
                _end = ToTerm("end"),
                _jmp = ToTerm("jmp"),
                _call = ToTerm("call"),
                _stack = ToTerm("Stack"),
                _heap = ToTerm("Heap"),
                _P = ToTerm("P"),
                _H = ToTerm("H"),
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
                _error = ToTerm("error");

            Terminal
                _mas = ToTerm("+"),
                _men = ToTerm("-"),
                _por = ToTerm("*"),
                _div = ToTerm("/"),
                _mod = ToTerm("%"),
                _pot = ToTerm("^"),
                _izq = ToTerm("=>"),
                _der = ToTerm("<="),
                _asig = ToTerm("=");

            Terminal
                _equ = ToTerm("je"),
                _dif = ToTerm("jne"),
                _gte = ToTerm("jge"),
                _lte = ToTerm("jle"),
                _gt = ToTerm("jg"),
                _lt = ToTerm("jl");

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
                = _begin + "," + "," + "," + _id 
                    + cuerpo +
                  _end + "," + "," + "," + _id 

            ;

            main.Rule
                = _begin + "," + "," + "," + _main
                    + cuerpo +
                  _end + "," + "," + "," + _main
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
                | acceso_heap
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
            ;

            etiqueta.Rule
                = _etq + ":"
            ;

            llamada.Rule
                = _call + "," + "," + "," + _id
            ;

            acceso_stack.Rule
                = _izq + "," + _stack + "," + valor + "," + destino
            ;

            asigna_stack.Rule
                = _der + "," + valor + "," + valor + "," + _stack
            ;

            acceso_heap.Rule
                = _izq + "," + _heap + "," + valor + "," + destino
            ;

            asigna_heap.Rule
                = _der + "," + valor + "," + valor + "," + _heap
            ;

            suma.Rule
                = _mas + "," + valor + "," + valor + "," + destino
            ;

            resta.Rule
                = _men + "," + valor + "," + valor + "," + destino
            ;

            multi.Rule
                = _por + "," + valor + "," + valor + "," + destino
            ;

            divide.Rule
                = _div + "," + valor + "," + valor + "," + destino
            ;

            modulo.Rule
                = _mod + "," + valor + "," + valor + "," + destino
            ;

            potencia.Rule
                = _pot + "," + valor + "," + valor + "," + destino
            ;

            asignacion.Rule
                = _asig + "," + valor + "," + "," + destino
            ;

            salto.Rule
                = _jmp + "," + "," + "," + _etq 
            ;

            salto_equ.Rule
                = _equ + "," + valor + "," + valor + "," + _etq
            ;

            salto_dif.Rule
                = _dif + "," + valor + "," + valor + "," + _etq
            ;

            salto_gte.Rule
                = _gte + "," + valor + "," + valor + "," + _etq
            ;

            salto_lte.Rule
                = _lte + "," + valor + "," + valor + "," + _etq
            ;

            salto_gt.Rule
                = _gt + "," + valor + "," + valor + "," + _etq
            ;

            salto_lt.Rule
                = _lt + "," + valor + "," + valor + "," + _etq
            ;

            printf.Rule
                = _printf + "," + valor + "," + formatos + ","
            ;

            nonsql.Rule
                = _call + "," + "," + "," + _create
                | _call + "," + "," + "," + _select
                | _call + "," + "," + "," + _insert
                | _call + "," + "," + "," + _update
                | _call + "," + "," + "," + _delete
                | _call + "," + "," + "," + _drop
            ;

            core.Rule
                = _call + "," + "," + "," + _intToStr
                | _call + "," + "," + "," + _doubleToStr
                | _call + "," + "," + "," + _charToStr
                | _call + "," + "," + "," + _boolToStr
                | _call + "," + "," + "," + _compareStr
                | _call + "," + "," + "," + _outString
                | _call + "," + "," + "," + _error
            ;

            formatos.Rule = _fc | _fd | _ff;

            valor.Rule
                = _entero
                | _decimal
                | _temp
                | _H
                | _P
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
            MarkPunctuation(",", ":");
            MarkPunctuation(_mas, _men, _por, _div, _mod, _pot);
            MarkPunctuation(_equ, _dif, _gte, _lte, _gt, _lt);
            MarkPunctuation(_asig, _izq, _der);
            MarkPunctuation(_begin, _end, _call, _jmp, _printf, _stack, _heap);
            MarkTransient(elemento, formatos, valor, destino, sentencia);
            #endregion

        }

    }
}
