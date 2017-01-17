using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace CompilerCollection.CompilerCollection.JCode
{
    class GrammarJcode : Grammar
    {
        public GrammarJcode() : base(caseSensitive: false)
        {
            #region ER
            StringLiteral __string = new StringLiteral("string", "\"", StringOptions.IsTemplate);
            StringLiteral __char = new StringLiteral("char", "\'", StringOptions.IsTemplate);
            IdentifierTerminal id = new IdentifierTerminal("id");
            RegexBasedTerminal __int = new RegexBasedTerminal("int", "[0-9]+");
            RegexBasedTerminal __double = new RegexBasedTerminal("double", "[0-9]+(\\.[0-9]+)");
            #endregion

            #region Comentarios
            CommentTerminal COMENTARIO_LINEA = new CommentTerminal("COMENTARIO_LINEA", "!--", "\n");
            CommentTerminal COMENTARIO_LINEAS = new CommentTerminal("COMENTARIO_LINEAS", "!!-", "-!!");
            base.NonGrammarTerminals.Add(COMENTARIO_LINEA);
            base.NonGrammarTerminals.Add(COMENTARIO_LINEAS);
            #endregion

            #region Terminales
            //Tipos:
            var _int = ToTerm("int");
            var _double = ToTerm("double");
            var _string = ToTerm("string");
            var _char = ToTerm("char");
            var _bool = ToTerm("bool");
            var _void = ToTerm("void");

            //Visibilidad:
            var _public = ToTerm("public");
            var _protected = ToTerm("protected");
            var _private = ToTerm("private");

            //Simbolos de agrupación:
            var _pabrir = ToTerm("(");
            var _pcerrar = ToTerm(")");
            var _cabrir = ToTerm("[");
            var _ccerrar = ToTerm("]");
            var _llabrir = ToTerm("{");
            var _llcerrar = ToTerm("}");
            
            //Simbolos de operación:
            var _inc = ToTerm("++");
            var _dec = ToTerm("--");
            var _sum = ToTerm("+");
            var _sub = ToTerm("-");
            var _mul = ToTerm("*");
            var _div = ToTerm("/");
            var _pow = ToTerm("^");
            var _mod = ToTerm("%");
            var _men = ToTerm("<");
            var _may = ToTerm(">");
            var _mene = ToTerm("<=");
            var _maye = ToTerm(">=");
            var _sie = ToTerm("==");
            var _noe = ToTerm("!=");
            var _or = ToTerm("||");
            var _and = ToTerm("&&");
            var _xor = ToTerm("??");
            var _not = ToTerm("!!");

            //Palabras reservadas:
            var _import = ToTerm("import");
            var _ext = ToTerm("jc");
            var _class = ToTerm("class");
            var _father = ToTerm("father");
            var _super = ToTerm("super");
            var _override = ToTerm("@override");
            var _main = ToTerm("main");
            var _new = ToTerm("new");
            var _null = ToTerm("null");
            var _rtrn = ToTerm("rtrn");
            var _break = ToTerm("break");
            var _continue = ToTerm("continue");
            var _create = ToTerm("create");
            var _select = ToTerm("select");
            var _insert = ToTerm("insert");
            var _update = ToTerm("update");
            var _delete = ToTerm("delete");
            var _drop = ToTerm("drop");
            var _cols = ToTerm("cols");
            var _val = ToTerm("val");
            var _from = ToTerm("from");
            var _outstring = ToTerm("out_string");
            var _outint = ToTerm("out_int");
            var _parseint = ToTerm("parseInt");
            var _parsedouble = ToTerm("parseDouble");
            var _inttostr = ToTerm("intToStr");
            var _doubletostr = ToTerm("doubleToStr");
            var _doubletoint = ToTerm("doubleToInt");
            var _copy = ToTerm("copy");
            var _length = ToTerm("length");
            var _concat = ToTerm("concat");
            var _substr = ToTerm("substr");
            var _loop = ToTerm("loop");
            var _for = ToTerm("for");
            var _while = ToTerm("while");
            var _do = ToTerm("do");
            var _repeat = ToTerm("repeat");
            var _until = ToTerm("until");
            var _case = ToTerm("case");
            var _ecse = ToTerm("ecse");
            var _of = ToTerm("of");
            var _otherwise = ToTerm("otherwise");
            var _else = ToTerm("else");
            var _if = ToTerm("if");
            var _this = ToTerm("this");
            var _true = ToTerm("true");
            var _false = ToTerm("false");

            #endregion

            #region No terminales
            NonTerminal inicio = new NonTerminal("inicio"),
            jcode = new NonTerminal("jcode"),
            imports = new NonTerminal("imports"),
            import = new NonTerminal("import"),
            clases = new NonTerminal("clases"),
            clase = new NonTerminal("clase"),
            visibilidad = new NonTerminal("visibilidad"),
            herencia = new NonTerminal("herencia"),
            elementos = new NonTerminal("decElementos"),
            lelementos = new NonTerminal("elementos"),
            elemento = new NonTerminal("elemento"),
            callconstructor = new NonTerminal("callConstructor"),
            sobreescribir = new NonTerminal("sobreescribir"),
            constructor = new NonTerminal("constructor"),
            metodo = new NonTerminal("metodo"),
            principal = new NonTerminal("principal"),
            decglobal = new NonTerminal("decGlobal"),
            asigparams = new NonTerminal("asigParametros"),
            decparams = new NonTerminal("decpParametros"),
            sentencias = new NonTerminal("decSentencias"),
            tipo = new NonTerminal("tipo"),
            tipomtd = new NonTerminal("tipo"),
            lcorch = new NonTerminal("corchetes"),
            inimatriz = new NonTerminal("iniciarMatriz"),
            inivar = new NonTerminal("iniciarVariable"),
            declocal = new NonTerminal("decLocal"),
            asigmatriz = new NonTerminal("asigMatriz"),
            asigvar = new NonTerminal("asigVariable"),
            dims = new NonTerminal("dimensiones"),
            lvalarr = new NonTerminal("{}"),
            valarr = new NonTerminal("valarr"),
            dim = new NonTerminal("dimension"),
            lexps = new NonTerminal("expresiones"),
            expresion = new NonTerminal("expresion"),
            corchetes = new NonTerminal("[]"),
            lparams = new NonTerminal("parametros"),
            param = new NonTerminal("parametro"),
            lsentencias = new NonTerminal("sentencias"),
            sentencia = new NonTerminal("sentencia"),
            asignacion = new NonTerminal("asignacion"),
            asig = new NonTerminal("tempAsig"),
            llamada = new NonTerminal("llamada"),
            scif = new NonTerminal("if"),
            scifotros = new NonTerminal("otros"),
            scifsimply = new NonTerminal("ifSimply"),
            scifnot = new NonTerminal("ifnot"),
            scswitch = new NonTerminal("switch"),
            scwhile = new NonTerminal("while"),
            scdowhile = new NonTerminal("dowhile"),
            scwhilex = new NonTerminal("whilex"),
            screpeat = new NonTerminal("repeat"),
            scfor = new NonTerminal("for"),
            scforpi = new NonTerminal("forparimpar"),
            scloop = new NonTerminal("loop"),
            retorno = new NonTerminal("retorno"),
            variacion = new NonTerminal("variacion"),
            outstring = new NonTerminal("outstring"),
            outint = new NonTerminal("outint"),
            parseint = new NonTerminal("parseint"),
            parsedouble = new NonTerminal("parsedouble"),
            inttostr = new NonTerminal("inttostr"),
            doubletostr = new NonTerminal("doubletostr"),
            doubletoint = new NonTerminal("doubletoint"),
            nosql = new NonTerminal("nosql"),
            obj = new NonTerminal("objeto"),
            subobj = new NonTerminal("subObjeto"),
            getobj = new NonTerminal("getObjeto"),
            getsubobj = new NonTerminal("getSubObjeto"),
            opobj = new NonTerminal("opObjeto"),
            decfor = new NonTerminal("decFor"),
            lcases = new NonTerminal("cases"),
            cases = new NonTerminal("cases"),
            sccase = new NonTerminal("case"),
            scotherwise = new NonTerminal("otherwise"),
            contenido = new NonTerminal("contenidoCase"),
            lelseif = new NonTerminal("elseIfs"),
            scelseif = new NonTerminal("elseIf"),
            scelse = new NonTerminal("else"),
            logica = new NonTerminal("logica"),
            relacional = new NonTerminal("relacional"),
            aritmetica = new NonTerminal("aritmetica"),
            operando = new NonTerminal("operando"),
            lacceso = new NonTerminal("accesos"),
            acceso = new NonTerminal("acceso"),
            negativo = new NonTerminal("negativo");
            #endregion

            #region Gramatica
            inicio.Rule =
                  jcode
                ;

            jcode.Rule =
                  imports + clases
                | clases
                ;


            #region Import
            imports.Rule = 
              MakePlusRule(imports, import)
            ;

            import.Rule =
              _import + _pabrir + id + "." + _ext + _pcerrar + ";"
            ;

            #endregion

            #region Clases
            clases.Rule = 
              MakePlusRule(clases, clase)
            ;

            clase.Rule =
              _class + id + visibilidad + herencia + elementos
            | _class + id + herencia + elementos
            ;

            #endregion

            #region Visibilidad
            visibilidad.Rule =
              _public
            | _protected
            | _private
            ;

            #endregion

            #region Herencia
            herencia.Rule =
              _father + id
            | Empty
            ;

            #endregion

            #region elementos
            elementos.Rule =
              _llabrir + lelementos + _llcerrar
            | _llabrir + _llcerrar
            ;

            lelementos.Rule = 
              MakePlusRule(lelementos, elemento)
            ;

            elemento.Rule =
              callconstructor
            | sobreescribir
            | constructor
            | metodo
            | principal
            | decglobal
            ;

            #endregion

            #region Constructor
            callconstructor.Rule =
              _super + asigparams + ";"
            ;

            sobreescribir.Rule =
              _override + constructor
            ;

            constructor.Rule =
              id + decparams + sentencias + ";"
            ;

            #endregion

            #region Metodo
            metodo.Rule =
              id + decparams + ":" + tipo + ":" + visibilidad + sentencias + ";"
            | id + decparams + ":" + tipo + sentencias + ";"
            ;
            #endregion

            #region Main
            principal.Rule =
              _main + _pabrir + _pcerrar + ":" + _void + sentencias + ";"
            ;

            #endregion

            #region Declaraciones
            decglobal.Rule =
              id + ":" + tipo + lcorch + visibilidad + inimatriz + ";"
            | id + ":" + tipo + visibilidad + inivar + ";"
            | declocal
            ;

            declocal.Rule =
              id + ":" + tipo + lcorch + inimatriz + ";"
            | id + ":" + tipo + inivar + ";"
            ;

            inimatriz.Rule =
              "=" + asigmatriz
            | Empty
            ;

            inivar.Rule =
              "=" + asigvar
            | Empty
            ;

            lcorch.Rule = 
              MakePlusRule(lcorch, corchetes)
            ;

            corchetes.Rule =
              _cabrir + _ccerrar
            ;

            asigmatriz.Rule =
              _new + tipo + dims
            | valarr
            ;

            dims.Rule = 
              MakePlusRule(dims, dim)
            ;

            dim.Rule =
              _cabrir + __int + _ccerrar
            | _cabrir + __int + "." + "." + __int + _ccerrar
            ;

            lvalarr.Rule =
              MakePlusRule(lvalarr, ToTerm(","), valarr)
            | lexps
            ;

            valarr.Rule =
              _llabrir + lvalarr + _llcerrar
            ;

            asigvar.Rule =
              expresion
            | _new + id + asigparams
            | _null
            ;

            #endregion

            #region Parametros
            decparams.Rule =
              _pabrir + lparams + _pcerrar
            | _pabrir + _pcerrar
            ;

            asigparams.Rule =
              _pabrir + lexps + _pcerrar
            | _pabrir + _pcerrar
            ;

            lparams.Rule =
              MakePlusRule(lparams, ToTerm(","), param)
            ;

            param.Rule =
              id + ":" + tipo
            ;

            #endregion

            #region Sentencias
            sentencias.Rule =
              _llabrir + lsentencias + _llcerrar
            | _llabrir + _llcerrar
            ;

            sentencias.ErrorRule =
              _llabrir + SyntaxError + _llcerrar
            ;

            lsentencias.Rule = 
              MakePlusRule(lsentencias, sentencia)
            ;

            sentencia.Rule =
              declocal
            | asignacion
            | llamada + ";"
            | retorno
            | scif
            | scifsimply
            | scifnot
            | scswitch
            | scwhile
            | scdowhile
            | scwhilex
            | screpeat
            | scfor
            | scforpi
            | scloop
            | variacion + ";"
            | _break + ";"
            | _continue + ";"
            | outstring + ";"
            | outint + ";"
            | nosql + ";"
            ;

            sentencia.ErrorRule =
              SyntaxError + ";"
            ;

            retorno.Rule =
              _rtrn + ";"
            | _rtrn + expresion + ";"
            ;
            #endregion

            #region Funciones No SQL
            nosql.Rule =
              _create + _pabrir + expresion + _pcerrar + "." + _cols + _pabrir + lexps + _pcerrar
            | _select + _pabrir + lexps + _pcerrar + "." + _from + _pabrir + expresion + _pcerrar
            | _insert + _pabrir + expresion + _pcerrar + "." + _cols + _pabrir + lexps + _pcerrar + "." + _val + asigparams
            | _update + _pabrir + expresion + _pcerrar + "." + _cols + _pabrir + expresion + _pcerrar + "." + _val + _pabrir + expresion + _pcerrar + "." + _new + _pabrir + expresion + _pcerrar
            | _delete + _pabrir + expresion + _pcerrar + "." + _cols + _pabrir + expresion + _pcerrar + "." + _val + _pabrir + expresion + _pcerrar
            | _drop + _pabrir + expresion + _pcerrar
            ;

            #endregion

            #region Funciones Propias del Lenguaje para Imprimir valores
            outstring.Rule =
              _outstring + _pabrir + expresion + _pcerrar
            ;

            outint.Rule =
              _outint + _pabrir + expresion + _pcerrar
            ;

            #endregion

            #region Funciones Propias del Lenguaje para Casteo
            parseint.Rule =
              _parseint + _pabrir + expresion + _pcerrar
            ;

            parsedouble.Rule =
              _parsedouble + _pabrir + expresion + _pcerrar
            ;

            inttostr.Rule =
              _inttostr + _pabrir + expresion + _pcerrar
            ;

            doubletostr.Rule =
              _doubletostr + _pabrir + expresion + _pcerrar
            ;

            doubletoint.Rule =
              _doubletoint + _pabrir + expresion + _pcerrar
            ;

            #endregion

            #region Variacion
            variacion.Rule =
              obj + _inc
            | obj + _dec
            ;

            #endregion

            #region Sentencia de Control Loop
            scloop.Rule =
              _loop + sentencias
            ;

            #endregion

            #region Sentencia de Control For y For Par-Impar (For PI)
            scfor.Rule =
              _for + _pabrir + decfor + expresion + ";" + variacion + _pcerrar + sentencias
            ;

            scforpi.Rule =
              _for + _pabrir + decfor + expresion + ";" + variacion + ";" + variacion + _pcerrar + sentencias
            ;

            decfor.Rule =
              declocal
            | obj + ";"
            ;

            #endregion

            #region Sentencia de Control While, do While, do Whilex, repeat until
            scwhile.Rule =
              _while + _pabrir + expresion + _pcerrar + sentencias
            ;

            scdowhile.Rule =
              _do + sentencias + _while + _pabrir + expresion + _pcerrar + ";"
            ;

            scwhilex.Rule =
              _do + sentencias + _while + _pabrir + expresion + "," + expresion + _pcerrar + ";"
            ;

            screpeat.Rule =
              _repeat + sentencias + _until + _pabrir + expresion + _pcerrar + ";"
            ;

            #endregion

            #region Sentencia de Control Switch
            scswitch.Rule =
              _case + _pabrir + expresion + _pcerrar + _of + lcases + _ecse
            ;

            lcases.Rule =
              MakePlusRule(lcases, cases)
            ;

            cases.Rule =
              sccase
            | scotherwise
            ;

            sccase.Rule =
              expresion + ":" + contenido
            ;

            scotherwise.Rule =
              _otherwise + ":" + contenido
            ;

            contenido.Rule =
              lsentencias
            | Empty
            ;

            #endregion

            #region Sentencia de control if simplificado
            scifsimply.Rule =
              obj + "=" + expresion + "?" + expresion + ":" + expresion + ";"
            ;
            #endregion

            #region Sentencia de Control If e IfNot
            scif.Rule =
              _if + ":" + _pabrir + expresion + _pcerrar + sentencias + scifotros
            ;

            scifotros.Rule =
              lelseif + scelse
            | lelseif
            | scelse
            | Empty
            ;
           
            lelseif.Rule =
              MakePlusRule(lelseif, scelseif)
            ;

            scelseif.Rule =
              _else + _if + _pabrir + expresion + _pcerrar + sentencias
            ;

            scelse.Rule =
              _else + sentencias
            ;

            scifnot.Rule =
              _if + "!" + ":" + _pabrir + expresion + _pcerrar + sentencias
            ;

            #endregion

            #region Asignación
            asignacion.Rule =
              obj + "=" + asig + ";"
            ;

            asig.Rule =
              asigmatriz
            | asigvar
            ;

            obj.Rule =
              _this + "." + id + subobj
            | id + subobj
            ;

            subobj.Rule =
              "." + id
            | lacceso
            | Empty
            ;

            #endregion

            #region Llamada
            llamada.Rule =
              _this + "." + id + "." + id + asigparams
            | id + "." + id + asigparams
            | id + asigparams
            ;

            #endregion

            #region Expresion
            lexps.Rule =
              MakePlusRule(lexps, ToTerm(","), expresion)
            ;

            expresion.Rule =
              logica
            ;

            logica.Rule =
              logica + _or + logica
            | logica + _xor + logica
            | logica + _and + logica
            | _not + logica
            | relacional
            ;

            relacional.Rule =
              relacional + _men + relacional
            | relacional + _may + relacional
            | relacional + _mene + relacional
            | relacional + _maye + relacional
            | relacional + _sie + relacional
            | relacional + _noe + relacional
            | aritmetica
            ;

            aritmetica.Rule =
              aritmetica + _sum + aritmetica
            | aritmetica + _sub + aritmetica
            | aritmetica + _mul + aritmetica
            | aritmetica + _div + aritmetica
            | aritmetica + _pow + aritmetica
            | aritmetica + _mod + aritmetica
            | operando
            ;

            operando.Rule =
              _pabrir + expresion + _pcerrar
            | _llabrir + expresion + _llcerrar
            | getobj
            | llamada
            | negativo
            | __int
            | __double
            | parseint
            | parsedouble
            | inttostr
            | doubletostr
            | doubletoint
            | _true
            | _false
            | __string
            | __char
            ;

            negativo.Rule =
              _sub + __int
            | _sub + __double
            ;

            getobj.Rule =
              _this + "." + id + getsubobj
            | id + getsubobj
            ;

            getsubobj.Rule =
              "." + id + opobj
            | lacceso + opobj
            | opobj
            ;

            opobj.Rule =
              "." + _copy + _pabrir + expresion + _pcerrar
            | "." + _length + _pabrir + _pcerrar
            | "." + _concat + _pabrir + expresion + _pcerrar
            | "." + _substr + _pabrir + expresion + "," + expresion + _pcerrar
            | Empty
            ;

            #endregion

            #region Acceso a Arreglos
            lacceso.Rule = 
              MakePlusRule(lacceso, acceso)
            ;

            acceso.Rule =
              _cabrir + expresion + _ccerrar
            ;

            #endregion

            #region Tipos
            tipo.Rule =
              _int
            | _double
            | _string
            | _char
            | _bool
            | id
            ;

            tipomtd.Rule =
              tipo
            | _void
            ;

            #endregion

            #endregion

            #region Preferencias
            this.Root = inicio;
            MarkPunctuation(_pabrir, _pcerrar, _llabrir, _llcerrar, _cabrir, _ccerrar, _case, _of, _ecse, _class, _import, _ext, _father, 
                _super, _override, _rtrn, _else, _if, _while, _do, _repeat, _until, _for, _loop, _parseint, _parsedouble, _inttostr, _doubletostr,
                _doubletoint, _outstring, _outint, _cols, _from, _val, _new, _main, _otherwise);
            MarkPunctuation(".", ";", ":", ",", "?", "!", "=");
            //MarkTransient(sentencia, conjunto, detalle, nounitarios, unitarios, tipo);
            MarkTransient(cases, sentencia, sentencias, elemento, elementos, inivar, decparams, asigparams, import, inimatriz, acceso, subobj,
                contenido, decfor, valarr, asig);
           
            //Precedencia y asociatividad
            RegisterOperators(10, Associativity.Left, _or, _xor);
            RegisterOperators(20, Associativity.Left, _and);
            RegisterOperators(30, _not);
            RegisterOperators(40, Associativity.Left, _sum, _sub);
            RegisterOperators(50, Associativity.Left, _mul, _div);
            RegisterOperators(60, Associativity.Left, _pow);
            #endregion

        }
    }
}
