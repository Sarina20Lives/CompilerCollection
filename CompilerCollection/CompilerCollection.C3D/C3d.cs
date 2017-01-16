using CompilerCollection.CompilerCollection.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilerCollection.CompilerCollection.Utilidades;
using CompilerCollection.CompilerCollection.Compilador;

namespace CompilerCollection.CompilerCollection.C3D
{
    public class C3d
    {
        public String cad = "";
        public int tipo = Constantes.ERROR;
        public bool esArr = false;
        public bool esTemp = false;
        public String etqV = "";
        public String etqF = "";
        public static int contTemp = 0;
        public static int contEtq = 0;

        public static void iniciarC3d()
        {
            contTemp = 0;
            contEtq = 0;
        }
        public C3d() {
            this.cad = "";
            this.tipo = Constantes.ERROR;
            this.esArr = false;
            this.esTemp = false;
            this.etqV = "";
            this.etqF = "";
        }
        public C3d(int tipo) {
            this.cad = generarTemp();
            this.esTemp = true;
            this.tipo = tipo;
            this.esArr = false;
            this.etqV = "";
            this.etqF = "";
        }
        public static String generarEtq()
        {
            String etq = "L" + contEtq;
            contEtq = contEtq + 1;
            return etq;
        }
        public static String addEtq(String antes, String nueva) {
            String etq = antes + ":" + nueva;
            return etq;
        }
        public static String generarTemp()
        {
            String temp = "t" + contTemp;
            contTemp = contTemp + 1;
            return temp;
        }
        public static C3d crearBoolean(String valor)
        {
            C3d c3d = new C3d(Constantes.T_BOOLEAN);
            if (valor.Equals("true", StringComparison.OrdinalIgnoreCase) || valor.Equals("1"))
            {
                c3d.cad = "1";
            }
            if (valor.Equals("false", StringComparison.OrdinalIgnoreCase) || valor.Equals("0"))
            {
                c3d.cad = "0";
            }
            return c3d;
        }
        public static C3d crearInt(String valor) 
        {
            try
            {
                int i = Convert.ToInt32(valor);
            }
            catch (OverflowException e) 
            {
                //TODO:ERROR:ENTERO FUERA DE LOS LÍMITES PERMITIDOS
                Console.WriteLine("Entero definido fuera de los límites permitidos " + e.Message);
                valor = "0";
            }
            C3d c3d = new C3d();
            c3d.cad = valor;
            c3d.tipo = Constantes.T_INT;
            return c3d;
        }
        public static C3d crearDouble(String valor)
        {
            try
            {
                double i = Convert.ToDouble(valor);
            }
            catch (InvalidCastException e) {
                //TODO:ERROR:CASTEO INVALIDO PARA DOUBLE
                Console.WriteLine("Casteo invalido para el double recibido " + e.Message);
                valor = "0.0";
            }
            C3d c3d = new C3d();
            c3d.cad = valor;
            c3d.tipo = Constantes.T_DOUBLE;
            return c3d;
        }
        public static C3d crearChar(String valor) 
        {
            try
            {
                if (valor.Length != 1)
                {
                    valor = "0";
                }
                char i = Convert.ToChar(valor);
            }
            catch (InvalidCastException e)
            {
                //TODO:ERROR:CASTEO INVALIDO PARA DOUBLE
                Console.WriteLine("Casteo invalido para el char recibido " + e.Message);
                valor = "0";
            }
            C3d c3d = new C3d();
            c3d.cad = valor;
            c3d.tipo = Constantes.T_CHAR;
            return c3d;
        }
        public static C3d crearString(String valor, bool esInit) 
        {
            C3d c3d = new C3d();
            //Inicio de la cadena
            c3d.cad = generarTemp();
            c3d.esTemp = true;
            escribirAsignacion(c3d.cad, "H", esInit);
            aumentarH("1", esInit);
            escribirEnHeap(c3d.cad, "H", esInit);

            //Escribir la cadena
            int i;
            String temp;
            foreach (var caracter in valor)
            {
                temp = generarTemp();
                escribirAsignacion(temp, "H", esInit);
                escribirEnHeap(temp, ((Int32)caracter).ToString(), esInit);
                aumentarH("1", esInit);
            }

            //Fin de la cadena
            temp = generarTemp();
            escribirAsignacion(temp, "H", esInit);
            escribirEnHeap(temp, "0", esInit);
            aumentarH("1", esInit);

            c3d.tipo = Constantes.T_STRING;
            return c3d;
        }
        public static C3d concatenar(String inicio1, String inicio2, bool esInit)
        {
            C3d c3d = new C3d(Constantes.T_STRING);
            //Inicio de la cadena
            c3d.cad = generarTemp();
            c3d.esTemp = true;
            escribirAsignacion(c3d.cad, "H", esInit);
            aumentarH("1", esInit);
            escribirEnHeap(c3d.cad, "H", esInit);

            //Escribir la primera cadena
            String linicio1 = generarEtq();
            String lf1 = generarEtq();
            String temp = generarTemp();
            String cad1 = leerDeHeap(inicio1, esInit);              //ta = heap[inicio1]; pos del primer caracter de la cadena 1
            escribir(linicio1 + ":", esInit);                       //Linicio:
            String caracter = leerDeHeap(cad1, esInit);             //tb = heap[ta]; valor del caracter en la posicion ta
            escribirSaltoCond(caracter, "==", "0", lf1, esInit);    //if tb == 0 goto lf; //Preguntar si es el fin de cadena
            escribirAsignacion(temp, "H", esInit);                  //temp = H;
            escribirEnHeap(temp, caracter, esInit);                 //Heap[temp] = tb;
            aumentarH("1", esInit);                                 //H = H+1;
            escribirOperacion(cad1, cad1, "+", "1", esInit);        //ta = ta + 1; Aumentando la posicion
            escribirSaltoIncond(linicio1, esInit);                  //goto linicio;
            escribir(lf1 + ":", esInit);                            //lf:

            String linicio2 = generarEtq();
            String lf2 = generarEtq();
            String cad2 = leerDeHeap(inicio2, esInit);              //ta = heap[inicio2]; pos del primer caracter de la cadena 2
            escribir(linicio2 + ":", esInit);                       //Linicio:
            String caracter2 = leerDeHeap(cad2, esInit);            //tb = heap[ta]; valor del caracter en la posicion ta
            escribirSaltoCond(caracter2, "==", "0", lf2, esInit);   //if tb == 0 goto lf; //Preguntar si es el fin de cadena
            escribirAsignacion(temp, "H", esInit);                  //temp = H;
            escribirEnHeap(temp, caracter2, esInit);                //Heap[temp] = tb;
            aumentarH("1", esInit);                                 //H = H+1;
            escribirOperacion(cad2, cad2, "+", "1", esInit);        //ta = ta + 1; Aumentando la posicion
            escribirSaltoIncond(linicio2, esInit);                  //goto linicio;
            escribir(lf2 + ":", esInit);                            //lf:

            //Fin de la cadena
            escribirAsignacion(temp, "H", esInit);
            escribirEnHeap(temp, "0", esInit);
            aumentarH("1", esInit);
            return c3d;
        }
        public static C3d crearNegativo(bool esInt, String valor, bool esInit) 
        {
            C3d c3d;
            if (esInt)
            {
                c3d = crearInt(valor);
            }
            else
            {
                c3d = crearDouble(valor);
            }

            String temp = c3d.cad;
            c3d.cad = generarTemp();
            c3d.esTemp = true;
            escribirOperacion(c3d.cad, "", "-", temp, esInit);
            return c3d;
        }


        public static void generarError(int tipoError, int tamanio, bool esInit) {
            String temp1 = generarTemp();
            escribirOperacion(temp1, "P", "+", tamanio.ToString(), esInit);
            String temp2 = generarTemp();
            escribirOperacion(temp2, temp1, "+", "0", esInit);
            escribirEnPila(temp2, tipoError.ToString(), esInit);
            aumentarP(tamanio.ToString(), esInit);
            escribir("Error();", esInit);
            disminuirP(tamanio.ToString(), esInit);
        }
        public static C3d verificarBoolean(C3d boolean, bool esInit) {
            if (boolean.cad.CompareTo("1") == 0 || boolean.cad.CompareTo("0") == 0)
            {
                boolean.etqV = C3d.generarEtq();
                boolean.etqF = C3d.generarEtq();
                C3d.escribirSaltoCond("1", "==", boolean.cad, boolean.etqV, esInit);
                C3d.escribirSaltoIncond(boolean.etqF, esInit);
                boolean.cad = "";
            }
            return boolean;
        }
        public static String casteo(String cad, int tamanio, String funcion, bool esInit)
        {
            String temp1 = generarTemp();
            escribirOperacion(temp1, "P", "+", tamanio.ToString(), esInit);
            String temp2 = generarTemp();
            escribirOperacion(temp2, temp1, "+", "1", esInit);
            escribirEnPila(temp2, cad, esInit);
            aumentarP(tamanio.ToString(), esInit);
            escribir(funcion, esInit);
            String temp3 = generarTemp();
            escribirOperacion(temp3, "P", "+", "0", esInit);
            String casteo = leerDePila(temp3, esInit);
            disminuirP(tamanio.ToString(), esInit);
            return casteo;
        }
        public static String compararStrings(String cad1, String cad2, int tamanio, String funcion, bool esInit)
        {
            String temp1 = generarTemp();
            escribirOperacion(temp1, "P", "+", tamanio.ToString(), esInit);
            String temp2 = generarTemp();
            escribirOperacion(temp2, temp1, "+", "1", esInit);
            escribirEnPila(temp2, cad1, esInit);
            String temp3 = generarTemp();
            escribirOperacion(temp3, temp1, "+", "2", esInit);
            escribirEnPila(temp3, cad2, esInit);
            aumentarP(tamanio.ToString(), esInit);
            escribir(funcion, esInit);
            String temp4 = generarTemp();
            escribirOperacion(temp4, "P", "+", "1", esInit);
            String casteo = leerDePila(temp4, esInit);
            disminuirP(tamanio.ToString(), esInit);
            return casteo;
        }


        public static void escribirSaltoCond(String op1, String op, String op2, String etq, bool esInit)
        {
            escribir("if " + op1 + op + op2 + " goto " + etq + ";",esInit);
        }
        public static void escribirSaltoIncond(String etq, bool esInit)
        {
            escribir("goto " + etq + ";", esInit);
        }
        public static void escribirAsignacion(String destino, String valor, bool esInit) 
        {
            escribir(destino + " = " + valor + ";", esInit);
        }
        public static void escribirOperacion(String destino, String val1, String op, String val2, bool esInit)
        {
            escribir(destino + " = " + val1 + op + val2 + ";", esInit);
        }
        public static void aumentarH(String cantidad, bool esInit)
        {
            escribir("H = H + " + cantidad + ";", esInit);
        }
        public static void disminuirH(String cantidad, bool esInit)
        {
            escribir("H = H - " + cantidad + ";", esInit);
        }
        public static void aumentarP(String cantidad, bool esInit)
        {
            escribir("P = P + " + cantidad + ";", esInit);
        }
        public static void disminuirP(String cantidad, bool esInit)
        {
            escribir("P = P - " + cantidad + ";", esInit);
        }
        public static void escribirEnHeap(String pos, String valor, bool esInit)
        {
            escribir("Heap[" + pos + "] = " + valor + ";", esInit);
        }
        public static String leerDeHeap(String pos, bool esInit)
        {
            String cad = generarTemp();
            escribir(cad + " = Heap[" + pos + "];", esInit);
            return cad;
        }
        public static void escribirEnPila(String pos, String valor, bool esInit)
        {
            escribir("Stack[" + pos + "] = " + valor + ";", esInit);
        }
        public static String leerDePila(String pos, bool esInit)
        {
            String temp = generarTemp();
            escribir(temp + " = Stack[" + pos + "];", esInit);
            return temp;
        }
        public static void escribir(String cadena, bool esInit)
        {
            Utilidades.ManejadorArchivo.escribirC3d(cadena + "\n", esInit);
        }
        public static void escribirComentario(String cadena, bool esInit)
        {
            Utilidades.ManejadorArchivo.escribirC3d("\n//"+cadena + "\n", esInit);
        }

        public static C3d castearA(C3d op, String tipo, int tamanio, bool esInit)
        {
            if (op.tipo == Constantes.ERROR) {
                return null;
            }
            if (tipo.Equals(Constantes.TIPOS[op.tipo], StringComparison.OrdinalIgnoreCase))
            {
                return op;
            }

            C3d casteo = new C3d();
            if (tipo.Equals(Constantes.TIPOS[Constantes.T_INT], StringComparison.OrdinalIgnoreCase))
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
            if (tipo.Equals(Constantes.TIPOS[Constantes.T_DOUBLE], StringComparison.OrdinalIgnoreCase))
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
            if (tipo.Equals(Constantes.TIPOS[Constantes.T_STRING], StringComparison.OrdinalIgnoreCase))
            {
                if (op.tipo == Constantes.T_INT)
                {
                    casteo = new C3d(Constantes.T_STRING);
                    casteo.cad = C3d.casteo(op.cad, tamanio, "intToStr()", esInit);
                    casteo.esTemp = true;
                    return casteo;
                }
                if (op.tipo == Constantes.T_DOUBLE)
                {
                    casteo = new C3d(Constantes.T_STRING);
                    casteo.cad = C3d.casteo(op.cad, tamanio, "doubleToStr()", esInit);
                    casteo.esTemp = true;
                    return casteo;
                }
                if (op.tipo == Constantes.T_CHAR)
                {
                    casteo = new C3d(Constantes.T_STRING);
                    casteo.cad = C3d.casteo(op.cad, tamanio, "charToStr()", esInit);
                    casteo.esTemp = true;
                    return casteo;
                }
            }
            return null;
        }


    }
}
