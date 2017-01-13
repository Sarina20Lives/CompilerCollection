using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;
using CompilerCollection.CompilerCollection.JCode;
using CompilerCollection.CompilerCollection.C3D;
using CompilerCollection.CompilerCollection.General;

namespace CompilerCollection.CompilerCollection.Compilador.Expresion
{
    class Expresion
    {

        public List<String> temporales = null;
        public Contexto ctxGlobal = null;
        public Contexto ctxLocal = null;


        public Expresion() {
            this.temporales = new List<String>();
            this.ctxGlobal = new Contexto();
            this.ctxLocal = new Contexto();
        }

        public Expresion(List<String> temps, Contexto ctxG, Contexto ctxL) {
            this.temporales = temps;
            this.ctxGlobal = ctxG;
            this.ctxLocal = ctxL;
        }

        public C3d resolver(ParseTreeNode expresion) {
            if (expresion.ToString().CompareTo(ConstantesJC.EXPRESION) == 0)
            {
                return resolver(expresion.ChildNodes.ElementAt(0));            
            }
        
            if (expresion.ToString().CompareTo(ConstantesJC.LOGICA) == 0)
            {
                return resolverLogica(expresion);
            }
            
            if (expresion.ToString().CompareTo(ConstantesJC.RELACIONAL) == 0)
            {
                return resolverRelacional(expresion);
            }
            
            if (expresion.ToString().CompareTo(ConstantesJC.ARITMETICA) == 0)
            {
                return resolverAritmetica(expresion);
            }
            
            if (expresion.ToString().CompareTo(ConstantesJC.OPERANDO) == 0)
            {
                return resolverOperando(expresion.ChildNodes.ElementAt(0));
            }

            return new C3d();
        }

        public C3d resolverLogica(ParseTreeNode expresion)
        {
            return new C3d();
        }

        public C3d resolverRelacional(ParseTreeNode expresion)
        {
            return new C3d();
        }


        public C3d resolverAritmetica(ParseTreeNode expresion)
        {
            // operando
            if (expresion.ChildNodes.Count() == 1)
            {
                return resolver(expresion.ChildNodes.ElementAt(0));
            }

            C3d op1 = resolver(expresion.ChildNodes.ElementAt(0));
            if (esError(op1)) 
            {
                //TODO:ERROR:NO SE PUEDE OPERAR EL TIPO DENTRO DE UNA OPERACIÓN ARITMÉTICA
                return op1;
            }
            C3d op2 = resolver(expresion.ChildNodes.ElementAt(2));
            if (esError(op2))
            {
                //TODO:ERROR:NO SE PUEDE OPERAR EL TIPO DENTRO DE UNA OPERACIÓN ARITMÉTICA
                return op2;
            }


            // aritmetica + _sum + aritmetica
            if (expresion.ChildNodes.ElementAt(1).FindTokenAndGetText().CompareTo("+") == 0)
            {
                                       
            }

            // aritmetica + _sub + aritmetica
            if (expresion.ChildNodes.ElementAt(1).FindTokenAndGetText().CompareTo("-") == 0)
            {
            }

            // aritmetica + _mul + aritmetica
            if (expresion.ChildNodes.ElementAt(1).FindTokenAndGetText().CompareTo("*") == 0)
            {
            }

            // aritmetica + _div + aritmetica
            if (expresion.ChildNodes.ElementAt(1).FindTokenAndGetText().CompareTo("/") == 0)
            {
            }

            // aritmetica + _pow + aritmetica
            if (expresion.ChildNodes.ElementAt(1).FindTokenAndGetText().CompareTo("^") == 0)
            {
            }
            
            return new C3d();
        }

        public C3d resolverOperando(ParseTreeNode expresion)
        {
            //(expresion) || {expresion}
            if (expresion.ToString().CompareTo(ConstantesJC.EXPRESION) == 0) {
                return resolver(expresion);
            }

            C3d resultado;

            //Negativos
            if(expresion.ToString().CompareTo(ConstantesJC.NEGATIVO)==0){
                if (expresion.ChildNodes.ElementAt(1).Term.ToString().CompareTo("int") == 0)
                {
                    resultado = C3d.crearNegativo(true, this.temporales, expresion.ChildNodes.ElementAt(1).FindTokenAndGetText());
                    return resultado;
                }
                resultado = C3d.crearNegativo(false, this.temporales, expresion.ChildNodes.ElementAt(1).FindTokenAndGetText());
                return resultado;
            }
            
            //Int
            if (expresion.Term.ToString().CompareTo("int") == 0)
            {
                resultado = C3d.crearInt(expresion.ChildNodes.ElementAt(0).FindTokenAndGetText());
                return resultado;
            }
            
            //Double
            if (expresion.Term.ToString().CompareTo("double") == 0)
            {
                resultado = C3d.crearDouble(expresion.ChildNodes.ElementAt(0).FindTokenAndGetText());
                return resultado;
            }
            
            //Char
            if (expresion.Term.ToString().CompareTo("char") == 0)
            {
                resultado = C3d.crearChar(expresion.ChildNodes.ElementAt(0).FindTokenAndGetText());
                return resultado;
            }
            
            //String
            if (expresion.Term.ToString().CompareTo("string") == 0)
            {
                resultado = C3d.crearChar(expresion.ChildNodes.ElementAt(0).FindTokenAndGetText());
                return resultado;
            }

            //Boolean
            if (expresion.FindTokenAndGetText().Equals("true", StringComparison.OrdinalIgnoreCase) || expresion.FindTokenAndGetText().Equals("false", StringComparison.OrdinalIgnoreCase))
            {
                resultado = C3d.crearBoolean(expresion.FindTokenAndGetText());       
            }

            return new C3d();
        }

        public bool esError(C3d c3d) 
        { 
            if (c3d.tipo == Constantes.ERROR || c3d.tipo == Constantes.T_OBJETO || c3d.tipo == Constantes.T_VOID || c3d.esArr) 
            {
                return true;
            }
            return false;
        }

    }
}
