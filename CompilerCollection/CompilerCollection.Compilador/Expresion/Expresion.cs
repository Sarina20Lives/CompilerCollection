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
using CompilerCollection.CompilerCollection.Utilidades;

namespace CompilerCollection.CompilerCollection.Compilador
{
    class Expresion
     {
        public Contexto ctxGlobal = null;
        public Contexto ctxLocal = null;
        public Simbolo ambito = null;
        public bool esInit = false;

        public Expresion() {
            this.ctxGlobal = new Contexto();
            this.ctxLocal = new Contexto();
            this.ambito = new Simbolo();
            this.esInit = false;
        }

        public Expresion(Contexto ctxG, Contexto ctxL, Simbolo ambito, bool esInit) {
            this.ctxGlobal = ctxG;
            this.ctxLocal = ctxL;
            this.ambito = ambito;
            this.esInit = esInit;
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
            // operando
            if (expresion.ChildNodes.Count() == 1)
            {
                return resolver(expresion.ChildNodes.ElementAt(0));
            }

            C3d op1;
            C3d op2;
            C3d resultado;

            // Not    
            if (expresion.ChildNodes.Count() == 2)
            {
                op1 = resolver(expresion.ChildNodes.ElementAt(1));
                if (esError(op1) || op1.tipo != Constantes.T_BOOLEAN)
                {
                    ManejadorErrores.Semantico("No se puede operar el tipo " + op1.tipo + " dentro de una operación relacional", expresion.FindToken().Location.Line, expresion.FindToken().Location.Column);
                    return op1;
                }

                op1 = C3d.verificarBoolean(op1, this.esInit);
                String tempEtq = op1.etqV;
                op1.etqV = op1.etqF;
                op1.etqF = tempEtq;
                return op1;
            }

            op1 = resolver(expresion.ChildNodes.ElementAt(0));
            if (esError(op1) || op1.tipo != Constantes.T_BOOLEAN)
            {
                ManejadorErrores.Semantico("No se puede operar el tipo " + op1.tipo + " dentro de una operación relacional", expresion.FindToken().Location.Line, expresion.FindToken().Location.Column);
                return op1;
            }

            op1 = C3d.verificarBoolean(op1, this.esInit);

            //And
            if (expresion.ChildNodes.ElementAt(1).FindTokenAndGetText().CompareTo("&&") == 0)
            {
                C3d.escribir(op1.etqV + ":", this.esInit);
                op2 = resolver(expresion.ChildNodes.ElementAt(2));
                if (esError(op2) || op2.tipo != Constantes.T_BOOLEAN)
                {
                    ManejadorErrores.Semantico("No se puede operar el tipo " + op2.tipo + " dentro de una operación relacional", expresion.FindToken().Location.Line, expresion.FindToken().Location.Column);
                    return op2;
                }
                op2 = C3d.verificarBoolean(op2, this.esInit);
                resultado = new C3d();
                resultado.tipo = Constantes.T_BOOLEAN;
                resultado.etqV = op2.etqV;
                resultado.etqF = C3d.addEtq(op1.etqF, op2.etqF);
                return resultado;
            }

            //Or
            if (expresion.ChildNodes.ElementAt(1).FindTokenAndGetText().CompareTo("||") == 0)
            {
                C3d.escribir(op1.etqF + ":", this.esInit);
                op2 = resolver(expresion.ChildNodes.ElementAt(2));
                if (esError(op2) || op2.tipo != Constantes.T_BOOLEAN)
                {
                    ManejadorErrores.Semantico("No se puede operar el tipo " + op2.tipo + " dentro de una operación relacional", expresion.FindToken().Location.Line, expresion.FindToken().Location.Column);
                    return op2;
                }
                op2 = C3d.verificarBoolean(op2, this.esInit);
                resultado = new C3d();
                resultado.tipo = Constantes.T_BOOLEAN;
                resultado.etqV = C3d.addEtq(op1.etqV, op2.etqV);
                resultado.etqF = op2.etqF;
                return resultado;
            }
            //Xor
            if (expresion.ChildNodes.ElementAt(1).FindTokenAndGetText().CompareTo("??") == 0)
            {
                C3d.escribir(op1.etqF + ":", this.esInit);
                op2 = resolver(expresion.ChildNodes.ElementAt(2));
                if (esError(op2) || op2.tipo != Constantes.T_BOOLEAN)
                {
                    ManejadorErrores.Semantico("No se puede operar el tipo " + op2.tipo + " dentro de una operación relacional", expresion.FindToken().Location.Line, expresion.FindToken().Location.Column);
                    return op2;
                }
                op2 = C3d.verificarBoolean(op2, this.esInit);
                C3d.escribir(op1.etqV + ":", this.esInit);
                C3d op3 = resolver(expresion.ChildNodes.ElementAt(2));
                if (esError(op3) || op3.tipo != Constantes.T_BOOLEAN)
                {
                    ManejadorErrores.Semantico("No se puede operar el tipo " + op3.tipo + " dentro de una operación relacional", expresion.FindToken().Location.Line, expresion.FindToken().Location.Column);
                    return op3;
                }
                op3 = C3d.verificarBoolean(op3, this.esInit);

                resultado = new C3d();
                resultado.tipo = Constantes.T_BOOLEAN;
                resultado.etqV = C3d.addEtq(op2.etqV, op3.etqF);
                resultado.etqF = C3d.addEtq(op2.etqF, op3.etqV);
                return resultado;
            }

            return new C3d();
        }

        public C3d resolverRelacional(ParseTreeNode expresion)
        {
            // operando
            if (expresion.ChildNodes.Count() == 1)
            {
                return resolver(expresion.ChildNodes.ElementAt(0));
            }

            C3d op1 = resolver(expresion.ChildNodes.ElementAt(0));
            if (esError(op1))
            {
                ManejadorErrores.Semantico("No se puede operar el tipo " + op1.tipo + " dentro de una operación relacional", expresion.FindToken().Location.Line, expresion.FindToken().Location.Column);
                return op1;
            }
            C3d op2 = resolver(expresion.ChildNodes.ElementAt(2));
            if (esError(op2))
            {
                ManejadorErrores.Semantico("No se puede operar el tipo " + op2.tipo + " dentro de una operación relacional", expresion.FindToken().Location.Line, expresion.FindToken().Location.Column);
                return op2;
            }

            if (op1.tipo == Constantes.T_BOOLEAN || op2.tipo == Constantes.T_BOOLEAN) {
                ManejadorErrores.Semantico("No se puede operar el tipo boolean dentro de una operación relacional", expresion.FindToken().Location.Line, expresion.FindToken().Location.Column);
                return new C3d();
            }

            if (op1.tipo == Constantes.T_STRING || op2.tipo ==Constantes.T_STRING) {
                if (op2.tipo == Constantes.T_INT || op2.tipo == Constantes.T_DOUBLE ||
                    op1.tipo == Constantes.T_INT || op1.tipo == Constantes.T_DOUBLE)
                {
                    ManejadorErrores.Semantico("No se puede operar un tipo string con uno numerico en una operación relacional", expresion.FindToken().Location.Line, expresion.FindToken().Location.Column);
                    return new C3d();                
                }
            }

            C3d resultado;
            C3d compTipo;
            String op = expresion.ChildNodes.ElementAt(1).FindTokenAndGetText();
            if ((op1.tipo == Constantes.T_INT || op1.tipo == Constantes.T_CHAR || op1.tipo == Constantes.T_DOUBLE) &&
                (op2.tipo == Constantes.T_INT || op2.tipo == Constantes.T_CHAR || op2.tipo == Constantes.T_DOUBLE)) {
                    resultado = new C3d();
                    resultado.tipo = Constantes.T_BOOLEAN;
                    resultado.etqV = C3d.generarEtq();
                    resultado.etqF = C3d.generarEtq();
                    C3d.escribirSaltoCond(op1.cad, op, op2.cad, resultado.etqV, this.esInit);
                    C3d.escribirSaltoIncond(resultado.etqF, this.esInit);
                    if(op.CompareTo("==")==0)
                    {
                        compTipo = new C3d();
                        compTipo.tipo = Constantes.T_BOOLEAN;
                        compTipo.etqV = C3d.generarEtq();
                        compTipo.etqF = resultado.etqF;

                        //And con la comprobación de tipo
                        C3d.escribir(resultado.etqV + ":", this.esInit);
                        C3d.escribirSaltoCond(op1.tipo.ToString(), "==", op2.tipo.ToString(), compTipo.etqV, this.esInit);
                        C3d.escribirSaltoIncond(compTipo.etqF, this.esInit);
                        return compTipo;
                    }
                    if (op.CompareTo("!=") == 0)
                    {
                        compTipo = new C3d();
                        compTipo.tipo = Constantes.T_BOOLEAN;
                        compTipo.etqV = resultado.etqV;
                        compTipo.etqF = C3d.generarEtq();

                        //Or con la comprobación de tipo
                        C3d.escribir(resultado.etqF + ":", this.esInit);
                        C3d.escribirSaltoCond(op1.tipo.ToString(), "!=", op2.tipo.ToString(), compTipo.etqV, this.esInit);
                        C3d.escribirSaltoIncond(compTipo.etqF, this.esInit);
                        return compTipo;
                    }
                    return resultado;
                }

            if (op1.tipo == Constantes.T_STRING && op2.tipo == Constantes.T_STRING) {
               resultado = new C3d();
               resultado.cad = C3d.compararStrings(op1.cad, op2.cad, this.ambito.tamanio, "compareStr()", this.esInit);
               C3d temp = new C3d();
               temp.tipo = Constantes.T_BOOLEAN;
               temp.etqV = C3d.generarEtq();
               temp.etqF = C3d.generarEtq();
               if (op.CompareTo("<") == 0) {
                   C3d.escribirSaltoCond(resultado.cad, "==", "-1", temp.etqV,this.esInit);
                   C3d.escribirSaltoIncond(temp.etqF, this.esInit);
                   return temp; 
               }
               if (op.CompareTo("<=") == 0) {
                   C3d.escribirSaltoCond(resultado.cad, "<=", "0", temp.etqV, this.esInit);
                   C3d.escribirSaltoIncond(temp.etqF, this.esInit);
                   return temp;
               }
               if (op.CompareTo(">") == 0) {
                   C3d.escribirSaltoCond(resultado.cad, "==", "1", temp.etqV, this.esInit);
                   C3d.escribirSaltoIncond(temp.etqF, this.esInit);
                   return temp;
               }
               if (op.CompareTo(">=") == 0) {
                   C3d.escribirSaltoCond(resultado.cad, ">=", "0", temp.etqV, this.esInit);
                   C3d.escribirSaltoIncond(temp.etqF, this.esInit);
                   return temp;
               }
               if (op.CompareTo("==") == 0) {
                   C3d.escribirSaltoCond(resultado.cad, "==", "0", temp.etqV, this.esInit);
                   C3d.escribirSaltoIncond(temp.etqF, this.esInit);
                   return temp;
               }
               if (op.CompareTo("!=") == 0) {
                   C3d.escribirSaltoCond(resultado.cad, "==", "0", temp.etqV, this.esInit);
                   C3d.escribirSaltoIncond(temp.etqF, this.esInit);
                   String tempEtq = temp.etqV;
                   temp.etqV = temp.etqF;
                   temp.etqF = tempEtq;
                   return temp;
               }

            }
            ManejadorErrores.Semantico("No se puede operar un tipo " + op1.tipo + " con un tipo "+ op2.tipo +" en una operación relacional", expresion.FindToken().Location.Line, expresion.FindToken().Location.Column);
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
                ManejadorErrores.Semantico("No se puede operar el tipo " + op1.tipo + " dentro de una operación aritmética", expresion.FindToken().Location.Line, expresion.FindToken().Location.Column); 
                return op1;
            }
            C3d op2 = resolver(expresion.ChildNodes.ElementAt(2));
            if (esError(op2))
            {
                ManejadorErrores.Semantico("No se puede operar el tipo " + op2.tipo + " dentro de una operación aritmética", expresion.FindToken().Location.Line, expresion.FindToken().Location.Column);
                return op2;
            }

            // aritmetica + _sum + aritmetica
            if (expresion.ChildNodes.ElementAt(1).FindTokenAndGetText().CompareTo("+") == 0)
            {
                int tipo = getTipoAritmetica(op1.tipo, op2.tipo, 0);
                if (tipo == Constantes.ERROR) 
                {
                    ManejadorErrores.Semantico("No se puede sumar los tipos " + op1.tipo + " y " + op2.tipo, expresion.FindToken().Location.Line, expresion.FindToken().Location.Column);
                    return new C3d();
                }
                return resolverSuma(op1, op2, tipo);
            }

            // aritmetica + _sub + aritmetica
            if (expresion.ChildNodes.ElementAt(1).FindTokenAndGetText().CompareTo("-") == 0)
            {
                int tipo = getTipoAritmetica(op1.tipo, op2.tipo, 1);
                if (tipo == Constantes.ERROR)
                {
                    ManejadorErrores.Semantico("No se puede restar los tipos " + op1.tipo + " y " + op2.tipo, expresion.FindToken().Location.Line, expresion.FindToken().Location.Column);
                    return new C3d();
                }
                return resolverResta(op1, op2, tipo);
            }

            // aritmetica + _mul + aritmetica
            if (expresion.ChildNodes.ElementAt(1).FindTokenAndGetText().CompareTo("*") == 0)
            {
                int tipo = getTipoAritmetica(op1.tipo, op2.tipo, 2);
                if (tipo == Constantes.ERROR)
                {
                    ManejadorErrores.Semantico("No se puede multiplicar los tipos " + op1.tipo + " y " + op2.tipo, expresion.FindToken().Location.Line, expresion.FindToken().Location.Column);
                    return new C3d();
                }
                return resolverMultiplicacion(op1, op2, tipo);
            }

            // aritmetica + _div + aritmetica
            if (expresion.ChildNodes.ElementAt(1).FindTokenAndGetText().CompareTo("/") == 0)
            {
                int tipo = getTipoAritmetica(op1.tipo, op2.tipo, 3);
                if (tipo == Constantes.ERROR)
                {
                    ManejadorErrores.Semantico("No se puede dividir los tipos " + op1.tipo + " y " + op2.tipo, expresion.FindToken().Location.Line, expresion.FindToken().Location.Column);
                    return new C3d();
                }
                return resolverDivision(op1, op2, tipo);
            }


            // aritmetica + _mod + aritmetica
            if (expresion.ChildNodes.ElementAt(1).FindTokenAndGetText().CompareTo("/") == 0)
            {
                int tipo = getTipoAritmetica(op1.tipo, op2.tipo, 4);
                if (tipo == Constantes.ERROR)
                {
                    ManejadorErrores.Semantico("No se puede obtener modulo de los tipos " + op1.tipo + " y " + op2.tipo, expresion.FindToken().Location.Line, expresion.FindToken().Location.Column);
                    return new C3d();
                }
                return resolverModulo(op1, op2, tipo);
            }

            // aritmetica + _pow + aritmetica
            if (expresion.ChildNodes.ElementAt(1).FindTokenAndGetText().CompareTo("^") == 0)
            {
                int tipo = getTipoAritmetica(op1.tipo, op2.tipo, 1);
                if (tipo == Constantes.ERROR)
                {
                    ManejadorErrores.Semantico("No se puede potenciar los tipos " + op1.tipo + " y " + op2.tipo, expresion.FindToken().Location.Line, expresion.FindToken().Location.Column);
                    return new C3d();
                }
                return resolverPotencia(op1, op2, tipo);
            }            
            return new C3d();
        }

        public C3d resolverSuma(C3d op1, C3d op2, int tipo) {
            op1 = castearA(op1, tipo);
            op2 = castearA(op2, tipo);
            C3d resultado = new C3d(tipo);
            //Concatenar
            if (tipo == Constantes.T_STRING)
            {
                resultado = C3d.concatenar(op1.cad, op2.cad, this.esInit);
                return resultado;
            }
            //Resolver Or
            if (tipo == Constantes.T_BOOLEAN)
            {
                C3d.escribirAsignacion(resultado.cad, "0", this.esInit);
                String etqV = C3d.generarEtq();
                String etqF1 = C3d.generarEtq();
                String etqF2 = C3d.generarEtq();
                C3d.escribirSaltoCond(op1.cad, "==", "1", etqV, this.esInit);
                C3d.escribirSaltoIncond(etqF1, this.esInit);
                C3d.escribir(etqF1 + ":", this.esInit);
                C3d.escribirSaltoCond(op2.cad, "==", "1", etqV, this.esInit);
                C3d.escribirSaltoIncond(etqF2, this.esInit);
                C3d.escribir(etqV + ":", this.esInit);
                C3d.escribirAsignacion(resultado.cad, "1", this.esInit);
                C3d.escribir(etqF2 + ":", this.esInit);
                return resultado;
            }
            //Resolver operacion
            C3d.escribirOperacion(resultado.cad, op1.cad, "+", op2.cad, this.esInit);
            return resultado;
        }

        public C3d resolverResta(C3d op1, C3d op2, int tipo) {
            op1 = castearA(op1, tipo);
            op2 = castearA(op2, tipo);
            C3d resultado = new C3d(tipo);
            C3d.escribirOperacion(resultado.cad, op1.cad, "-", op2.cad, this.esInit);
            return resultado;
        }

        public C3d resolverMultiplicacion(C3d op1, C3d op2, int tipo) {
            op1 = castearA(op1, tipo);
            op2 = castearA(op2, tipo);
            C3d resultado = new C3d( tipo);
            if (tipo == Constantes.T_BOOLEAN)
            {
                //Resolver And
                C3d.escribirAsignacion(resultado.cad, "0", this.esInit);
                String etqF = C3d.generarEtq();
                String etqV1 = C3d.generarEtq();
                String etqV2 = C3d.generarEtq();
                C3d.escribirSaltoCond(op1.cad, "==", "1", etqV1, this.esInit);
                C3d.escribirSaltoIncond(etqF, this.esInit);
                C3d.escribir(etqV1 + ":", this.esInit);
                C3d.escribirSaltoCond(op2.cad, "==", "1", etqV2, this.esInit);
                C3d.escribirSaltoIncond(etqF, this.esInit);
                C3d.escribir(etqV2 + ":", this.esInit);
                C3d.escribirAsignacion(resultado.cad, "1", this.esInit);
                C3d.escribir(etqF + ":", this.esInit);
                return resultado;
            }
            C3d.escribirOperacion(resultado.cad, op1.cad, "*", op2.cad, this.esInit);
            return resultado;
        }

        public C3d resolverDivision(C3d op1, C3d op2, int tipo) {
            op1 = castearA(op1, tipo);
            op2 = castearA(op2, tipo);
            C3d resultado = new C3d( tipo);
            //Controlar el error de división entre cero
            C3d.escribirAsignacion(resultado.cad, "0", this.esInit);
            String etqV = C3d.generarEtq();
            String etqF = C3d.generarEtq();
            C3d.escribirSaltoCond(op2.cad, "==", "0", etqV, this.esInit);
            C3d.escribirOperacion(resultado.cad, op1.cad, "/", op2.cad, this.esInit);
            C3d.escribirSaltoIncond(etqF, this.esInit);
            C3d.escribir(etqV + ":", this.esInit);
            C3d.generarError(Constantes.ERROR_DIV_CERO, this.ambito.tamanio, this.esInit);
            C3d.escribir(etqF + ":", this.esInit);
            return resultado;        
        }

        public C3d resolverModulo(C3d op1, C3d op2, int tipo)
        {
            op1 = castearA(op1, tipo);
            op2 = castearA(op2, tipo);
            C3d resultado = new C3d( tipo);
            //Controlar el error de división entre cero
            C3d.escribirAsignacion(resultado.cad, "0", this.esInit);
            String etqV = C3d.generarEtq();
            String etqF = C3d.generarEtq();
            C3d.escribirSaltoCond(op2.cad, "==", "0", etqV, this.esInit);
            C3d.escribirOperacion(resultado.cad, op1.cad, "%", op2.cad, this.esInit);
            C3d.escribirSaltoIncond(etqF, this.esInit);
            C3d.escribir(etqV + ":", this.esInit);
            C3d.generarError(Constantes.ERROR_MOD_CERO, this.ambito.tamanio, this.esInit);
            C3d.escribir(etqF + ":", this.esInit);
            return resultado;
        }

        public C3d resolverPotencia(C3d op1, C3d op2, int tipo) {
            op1 = castearA(op1, tipo);
            op2 = castearA(op2, tipo);
            C3d resultado = new C3d( tipo);
            C3d.escribirOperacion(resultado.cad, op1.cad, "^", op2.cad, this.esInit);
            return resultado;           
        }

        public int getTipoAritmetica(int tipo1, int tipo2, int operacion) 
        {
            switch (operacion) {
                //Suma:
                case 0:
                    if (tipo1 == Constantes.T_STRING || tipo2 == Constantes.T_STRING) {
                        if (tipo1 == Constantes.T_BOOLEAN || tipo2 == Constantes.T_BOOLEAN) {
                            return Constantes.ERROR;
                        }
                        return Constantes.T_STRING;                    
                    }
                    if (tipo1 == Constantes.T_DOUBLE || tipo2 == Constantes.T_DOUBLE) {
                        return Constantes.T_DOUBLE;
                    }
                    if (tipo1 == Constantes.T_INT || tipo2 == Constantes.T_INT) {
                        return Constantes.T_INT;
                    }
                    if (tipo1 == Constantes.T_BOOLEAN && tipo2 == Constantes.T_BOOLEAN) {
                        return Constantes.T_BOOLEAN;
                    }
                    break;
                //Resta y potencia:
                case 1:
                    if (tipo1 == Constantes.T_STRING || tipo2 == Constantes.T_STRING)
                    {
                        return Constantes.ERROR;
                    }
                    if (tipo1 == Constantes.T_DOUBLE || tipo2 == Constantes.T_DOUBLE) {
                        return Constantes.T_DOUBLE;
                    }
                    if (tipo1 == Constantes.T_INT || tipo2 == Constantes.T_INT) {
                        return Constantes.T_INT;
                    }
                    break;
                //Multiplicacion:
                case 2:
                    if (tipo1 == Constantes.T_STRING || tipo2 == Constantes.T_STRING)
                    {
                        return Constantes.ERROR;
                    }
                    if (tipo1 == Constantes.T_DOUBLE || tipo2 == Constantes.T_DOUBLE)
                    {
                        return Constantes.T_DOUBLE;
                    }
                    if (tipo1 == Constantes.T_INT || tipo2 == Constantes.T_INT)
                    {
                        return Constantes.T_INT;
                    }
                    if (tipo1 == Constantes.T_BOOLEAN && tipo2 == Constantes.T_BOOLEAN)
                    {
                        return Constantes.T_BOOLEAN;
                    }
                    break;
                //Division:
                case 3:
                    if (tipo1 == Constantes.T_STRING || tipo2 == Constantes.T_STRING)
                    {
                        return Constantes.ERROR;
                    }
                    if (tipo1 == Constantes.T_DOUBLE || tipo2 == Constantes.T_DOUBLE)
                    {
                        return Constantes.T_DOUBLE;
                    }
                    if (tipo1 == Constantes.T_INT || tipo2 == Constantes.T_INT)
                    {
                        return Constantes.T_DOUBLE;
                    }
                    break;
                //Modulo:
                case 4:
                    if (tipo1 == Constantes.T_STRING || tipo2 == Constantes.T_STRING)
                    {
                        return Constantes.ERROR;
                    }
                    if (tipo1 == Constantes.T_DOUBLE || tipo2 == Constantes.T_DOUBLE)
                    {
                        return Constantes.T_INT;
                    }
                    if (tipo1 == Constantes.T_INT || tipo2 == Constantes.T_INT)
                    {
                        return Constantes.T_INT;
                    }
                    break;
            }
            return Constantes.ERROR;
        } 

        public C3d resolverOperando(ParseTreeNode expresion)
        {
            
            //(expresion) || {expresion}
            if (expresion.ToString().CompareTo(ConstantesJC.EXPRESION) == 0) {
                return resolver(expresion);
            }

            C3d resultado;

            //ParseInt
            if (expresion.ToString().CompareTo(ConstantesJC.PARSEINT) == 0)
            {
                return GeneradorC3d.resolverParseInt(this.ambito, this.ctxGlobal, this.ctxLocal, expresion);
            }

            //ParseDouble
            if (expresion.ToString().CompareTo(ConstantesJC.PARSEDOUBLE) == 0)
            {
                return GeneradorC3d.resolverParseDouble(this.ambito, this.ctxGlobal, this.ctxLocal, expresion);
            }

            //IntToStr
            if (expresion.ToString().CompareTo(ConstantesJC.INTTOSTR) == 0)
            {
                return GeneradorC3d.resolverIntToStr(this.ambito, this.ctxGlobal, this.ctxLocal, expresion);
            }

            //DoubleToStr
            if (expresion.ToString().CompareTo(ConstantesJC.DOUBLETOSTR) == 0)
            {
                return GeneradorC3d.resolverDoubleToStr(this.ambito, this.ctxGlobal, this.ctxLocal, expresion);
            }

            //DoubleToInt
            if (expresion.ToString().CompareTo(ConstantesJC.DOUBLETOINT) == 0)
            {
                return GeneradorC3d.resolverDoubleToInt(this.ambito, this.ctxGlobal, this.ctxLocal, expresion);
            }

            //Get objeto
            if (expresion.ToString().CompareTo(ConstantesJC.GETOBJ) == 0) {
                resultado = getValObjeto(expresion);
                return resultado;
            }

            //Llamada
            if (expresion.ToString().CompareTo(ConstantesJC.LLAMADA) == 0)
            {
                resultado = GeneradorC3d.resolverLLamada(this.ambito, this.ctxGlobal, this.ctxLocal, expresion);
                return resultado;
            }


            //Negativos
            if(expresion.ToString().CompareTo(ConstantesJC.NEGATIVO)==0){
                if (expresion.ChildNodes.ElementAt(1).Term.ToString().CompareTo("int") == 0)
                {
                    resultado = C3d.crearNegativo(true, expresion.FindTokenAndGetText(), this.esInit);
                    return resultado;
                }
                resultado = C3d.crearNegativo(false, expresion.FindTokenAndGetText(), this.esInit);
                return resultado;
            }
            
            //Int
            if (expresion.Term.ToString().CompareTo("int") == 0)
            {
                resultado = C3d.crearInt(expresion.FindTokenAndGetText());
                return resultado;
            }
            
            //Double
            if (expresion.Term.ToString().CompareTo("double") == 0)
            {
                resultado = C3d.crearDouble(expresion.FindTokenAndGetText());
                return resultado;
            }
            
            //Char
            if (expresion.Term.ToString().CompareTo("char") == 0)
            {
                String str = expresion.FindTokenAndGetText();
                //Reemplazar
                resultado = C3d.crearChar(str.Substring(1, str.Length - 2));
                return resultado;
            }
            
            //String
            if (expresion.Term.ToString().CompareTo("string") == 0)
            {
                String str = expresion.FindTokenAndGetText();
                resultado = C3d.crearString(str.Substring(1, str.Length -2), this.esInit);
                return resultado;
            }

            //Boolean
            if (expresion.FindTokenAndGetText().Equals("true", StringComparison.OrdinalIgnoreCase) || expresion.FindTokenAndGetText().Equals("false", StringComparison.OrdinalIgnoreCase))
            {
                resultado = C3d.crearBoolean(expresion.FindTokenAndGetText());
                return resultado;
            }

            
            return new C3d();
        }

        public bool esError(C3d c3d) 
        {
            if (c3d == null) {
                return true;
            }
            if (c3d.tipo == Constantes.ERROR || c3d.tipo == Constantes.T_OBJETO || c3d.tipo == Constantes.T_VOID || c3d.esArr) 
            {
                return true;
            }
            return false;
        }

        public C3d castearA(C3d op, int tipo)
        {
            if (op.tipo == tipo)
            {
                return op;
            }

            C3d casteo = new C3d();
            if (tipo == Constantes.T_INT)
            {
                if (op.tipo == Constantes.T_CHAR)
                {
                    op.tipo = Constantes.T_INT;
                    return op;
                }
                if (op.tipo == Constantes.T_BOOLEAN)
                {
                    op.tipo = Constantes.T_INT;
                    return op;
                }
            }
            if (tipo == Constantes.T_DOUBLE)
            {
                if (op.tipo == Constantes.T_INT)
                {
                    op.tipo = Constantes.T_DOUBLE;
                    return op;
                }
                if (op.tipo == Constantes.T_CHAR)
                {
                    op.tipo = Constantes.T_DOUBLE;
                    return op;
                }
                if (op.tipo == Constantes.T_BOOLEAN)
                {
                    op.tipo = Constantes.T_DOUBLE;
                    return op;
                }
            }
            if (tipo == Constantes.T_STRING)
            {
                if (op.tipo == Constantes.T_INT)
                {
                    casteo = new C3d(Constantes.T_STRING);
                    casteo.cad = C3d.casteo(op.cad, this.ambito.tamanio, "intToStr()", this.esInit);
                    casteo.esTemp = true;
                    return casteo;
                }
                if (op.tipo == Constantes.T_DOUBLE)
                {
                    casteo = new C3d(Constantes.T_STRING);
                    casteo.cad = C3d.casteo(op.cad, this.ambito.tamanio, "doubleToStr()", this.esInit);
                    casteo.esTemp = true;
                    return casteo;
                }
                if (op.tipo == Constantes.T_CHAR)
                {
                    casteo = new C3d(Constantes.T_STRING);
                    casteo.cad = C3d.casteo(op.cad, this.ambito.tamanio, "charToStr()", this.esInit);
                    casteo.esTemp = true;
                    return casteo;
                }
            }
            return null;
        }


        public C3d getValObjeto(ParseTreeNode raiz) {
            Simbolo obj;
            String nObj = "";
            C3d resultado;
            if (raiz.ChildNodes.Count() == 2 && getSubObjEsVacio(raiz.ChildNodes.ElementAt(1))) { 
                //id
                nObj = raiz.ChildNodes.ElementAt(0).FindTokenAndGetText();
                obj = TablaSimbolo.buscarObj(this.ctxLocal, nObj);
                if (obj == null) {
                    obj =TablaSimbolo.buscarObj(this.ctxGlobal, nObj);
                }
                if (obj == null) {
                    ManejadorErrores.Semantico("No existe la variable a la cual se hace referencia", raiz.ChildNodes.ElementAt(0).Token.Location.Line, raiz.ChildNodes.ElementAt(0).Token.Location.Column);
                    return new C3d();
                }
                resultado = new C3d();
                resultado.esArr = obj.esArr;
                //Obtener el tipo del objeto
                if (obj.esObjeto())
                {
                    resultado.tipo = Constantes.T_OBJETO;
                    resultado.ntipo = obj.tipo;
                }
                else
                {
                    resultado.tipo = Constantes.obtenerTipo(obj.tipo);
                }
                String temp1, temp2 = "";
                if (obj.esGlobal) {
                    C3d.escribirComentario("Obteniendo valor de una variable global", this.esInit);
                    temp1 = C3d.generarTemp();
                    C3d.escribirComentario("Accediendo al this", this.esInit);
                    C3d.escribirOperacion(temp1, "P", "+", "1", this.esInit);
                    temp2 = C3d.leerDePila(temp1, this.esInit);
                    temp1 = C3d.generarTemp();
                    C3d.escribirComentario("Obteniendo su valor", this.esInit);
                    C3d.escribirOperacion(temp1, temp2, "+", obj.pos.ToString(), this.esInit);
                    resultado.cad = C3d.leerDeHeap(temp1, this.esInit);
                    return resultado;
                }
                C3d.escribirComentario("Obteniendo valor de una variable local", this.esInit);
                temp1 = C3d.generarTemp();
                C3d.escribirOperacion(temp1, "P", "+", obj.pos.ToString(), this.esInit);
                resultado.cad = C3d.leerDePila(temp1, this.esInit);
                return resultado;
            }
            if (raiz.ChildNodes.Count() == 3 && raiz.ChildNodes.ElementAt(0).FindTokenAndGetText().Equals("this",StringComparison.OrdinalIgnoreCase)
                && getSubObjEsVacio(raiz.ChildNodes.ElementAt(2)))
            {
                //this.id
                nObj = raiz.ChildNodes.ElementAt(1).FindTokenAndGetText();
                obj = TablaSimbolo.buscarObj(this.ctxGlobal, nObj);
                if (obj == null)
                {
                    ManejadorErrores.Semantico("No existe la variable a la cual se hace referencia", raiz.ChildNodes.ElementAt(1).Token.Location.Line, raiz.ChildNodes.ElementAt(1).Token.Location.Column);
                    return new C3d();
                }
                resultado = new C3d();
                resultado.esArr = obj.esArr;
                //Obtener el tipo del objeto
                if (obj.esObjeto())
                {
                    resultado.tipo = Constantes.T_OBJETO;
                    resultado.ntipo = obj.tipo;
                }
                else
                {
                    resultado.tipo = Constantes.obtenerTipo(obj.tipo);
                }

                String temp1, temp2 = "";
                C3d.escribirComentario("Obteniendo valor de una variable global", this.esInit);
                temp1 = C3d.generarTemp();
                C3d.escribirComentario("Accediendo al this", this.esInit);
                C3d.escribirOperacion(temp1, "P", "+", "1", this.esInit);
                temp2 = C3d.leerDePila(temp1, this.esInit);
                temp1 = C3d.generarTemp();
                C3d.escribirComentario("Obteniendo su valor", this.esInit);
                C3d.escribirOperacion(temp1, temp2, "+", obj.pos.ToString(), this.esInit);
                resultado.cad = C3d.leerDeHeap(temp1, this.esInit);
                return resultado;
            }
            return new C3d();
        }

        public static bool getSubObjEsVacio(ParseTreeNode getSubObj){
            if(getSubObj.ChildNodes.Count()==0){
                return true;
            }
            if (getSubObj.ChildNodes.Count() == 1 && getSubObj.ChildNodes.ElementAt(0).ChildNodes.Count() == 0) {
                return true;
            }
            return false;


        }

    }
}
