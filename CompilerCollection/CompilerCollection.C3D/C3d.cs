using CompilerCollection.CompilerCollection.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilerCollection.CompilerCollection.Utilidades;

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
        public C3d(List<String> temps, int tipo) {
            this.cad = generarTemp(temps);
            this.esTemp = true;
            this.tipo = tipo;
            this.esArr = false;
            this.etqV = "";
            this.etqF = "";
        }
        public static String generarTemp(List<String> temps)
        {
            String temp = "t" + contTemp;
            contTemp = contTemp + 1;
            temps.Add(temp);
            return temp;
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
        public String getCad(List<String> temps) 
        {
            if (this.esTemp) 
            {
                temps.Remove(this.cad);
            }
            return this.cad;
        }
        public static C3d crearBoolean(String valor)
        {
            C3d c3d = new C3d();
            if (valor.Equals("true", StringComparison.OrdinalIgnoreCase) || valor.Equals("1"))
            {
                c3d.cad = "1";
            }
            if (valor.Equals("false", StringComparison.OrdinalIgnoreCase) || valor.Equals("0"))
            {
                c3d.cad = "0";
            }
            c3d.tipo = Constantes.T_BOOLEAN;
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
        public static C3d crearString(List<String> temps, String valor) 
        {
            C3d c3d = new C3d();
            //Inicio de la cadena
            c3d.cad = generarTemp(temps);
            c3d.esTemp = true;
            escribirAsignacion(c3d.cad, "H");
            aumentarH("1");
            escribirEnHeap(c3d.cad, "H");

            //Escribir la cadena
            int i;
            String temp;
            foreach (var caracter in valor)
            {
                temp = generarTemp();
                escribirAsignacion(temp, "H");
                escribirEnHeap(temp, caracter.ToString());
                aumentarH("1");
            }

            //Fin de la cadena
            temp = generarTemp();
            escribirAsignacion(temp, "H");
            escribirEnHeap(temp, "0");
            aumentarH("1");

            c3d.tipo = Constantes.T_STRING;
            return c3d;
        }
        public static C3d concatenar(String inicio1, String inicio2, List<String> temps)
        {
            C3d c3d = new C3d();
            //Inicio de la cadena
            c3d.cad = generarTemp(temps);
            c3d.esTemp = true;
            escribirAsignacion(c3d.cad, "H");
            aumentarH("1");
            escribirEnHeap(c3d.cad, "H");

            //Escribir la primera cadena
            String linicio1 = generarEtq();
            String lf1 = generarEtq();
            String temp = generarTemp();
            C3d cad1 = leerDeHeap(inicio1, temps);              //ta = heap[inicio1]; pos del primer caracter de la cadena 1
            escribir(linicio1 + ":");                           //Linicio:
            C3d caracter = leerDeHeap(cad1.cad, temps);         //tb = heap[ta]; valor del caracter en la posicion ta
            escribirSaltoCond(caracter.cad, "==", "0", lf1);    //if tb == 0 goto lf; //Preguntar si es el fin de cadena
            escribirAsignacion(temp, "H");                      //temp = H;
            escribirEnHeap(temp, caracter.cad);                 //Heap[temp] = tb;
            aumentarH("1");                                     //H = H+1;
            escribirOperacion(cad1.cad, cad1.cad, "+", "1");    //ta = ta + 1; Aumentando la posicion
            escribirSaltoIncond(linicio1);                      //goto linicio;
            escribir(lf1 + ":");                                //lf:

            String linicio2 = generarEtq();
            String lf2 = generarEtq();
            C3d cad2 = leerDeHeap(inicio2, temps);              //ta = heap[inicio2]; pos del primer caracter de la cadena 2
            escribir(linicio2 + ":");                           //Linicio:
            C3d caracter2 = leerDeHeap(cad2.cad, temps);        //tb = heap[ta]; valor del caracter en la posicion ta
            escribirSaltoCond(caracter2.cad, "==", "0", lf2);   //if tb == 0 goto lf; //Preguntar si es el fin de cadena
            escribirAsignacion(temp, "H");                      //temp = H;
            escribirEnHeap(temp, caracter2.cad);                //Heap[temp] = tb;
            aumentarH("1");                                     //H = H+1;
            escribirOperacion(cad2.cad, cad2.cad, "+", "1");    //ta = ta + 1; Aumentando la posicion
            escribirSaltoIncond(linicio2);                      //goto linicio;
            escribir(lf2 + ":");                                //lf:

            //Fin de la cadena
            escribirAsignacion(temp, "H");
            escribirEnHeap(temp, "0");
            aumentarH("1");
            c3d.tipo = Constantes.T_STRING;
            return c3d;
        }
        public static C3d crearNegativo(bool esInt, List<String> temps, String valor) 
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
            c3d.cad = generarTemp(temps);
            c3d.esTemp = true;
            escribirOperacion(c3d.cad, "", "-", temp);
            return c3d;
        }


        public static void generarError(int tipoError, int tamanio) {
            String temp1 = generarTemp();
            escribirOperacion(temp1, "P", "+", tamanio.ToString());
            String temp2 = generarTemp();
            escribirOperacion(temp2, temp1, "+", "0");
            escribirEnPila(temp2, tipoError.ToString());
            aumentarP(tamanio.ToString());
            escribir("Error();");
            disminuirP(tamanio.ToString());
        }
        public static C3d verificarBoolean(C3d boolean) {
            if (boolean.cad.CompareTo("1") == 0 || boolean.cad.CompareTo("0") == 0)
            {
                boolean.etqV = C3d.generarEtq();
                boolean.etqF = C3d.generarEtq();
                C3d.escribirSaltoCond("1", "==", boolean.cad, boolean.etqV);
                C3d.escribirSaltoIncond(boolean.etqF);
                boolean.cad = "";
            }
            return boolean;
        }

        public static C3d casteo(String cad, int tamanio, String funcion, List<String> temps)
        {
            String temp1 = generarTemp();
            escribirOperacion(temp1, "P", "+", tamanio.ToString());
            String temp2 = generarTemp();
            escribirOperacion(temp2, temp1, "+", "1");
            escribirEnPila(temp2, cad);
            aumentarP(tamanio.ToString());
            escribir(funcion);
            String temp3 = generarTemp();
            escribirOperacion(temp3, "P", "+", "0");
            C3d casteo = leerDePila(temp3, temps);
            disminuirP(tamanio.ToString());
            return casteo;
        }

        public static C3d compararStrings(String cad1, String cad2, int tamanio, String funcion, List<String> temps)
        {
            String temp1 = generarTemp();
            escribirOperacion(temp1, "P", "+", tamanio.ToString());
            String temp2 = generarTemp();
            escribirOperacion(temp2, temp1, "+", "2");
            escribirEnPila(temp2, cad1);
            String temp3 = generarTemp();
            escribirOperacion(temp3, temp1, "+", "3");
            escribirEnPila(temp3, cad2);
            aumentarP(tamanio.ToString());
            escribir(funcion);
            String temp4 = generarTemp();
            escribirOperacion(temp4, "P", "+", "1");
            C3d casteo = leerDePila(temp4, temps);
            disminuirP(tamanio.ToString());
            return casteo;
        }


        public static void escribirSaltoCond(String op1, String op, String op2, String etq)
        {
            escribir("if " + op1 + op + op2 + " goto " + etq + ";");
        }
        public static void escribirSaltoIncond(String etq)
        {
            escribir("goto " + etq + ";");
        }
        public static void escribirAsignacion(String destino, String valor) 
        {
            escribir(destino + " = " + valor + ";");
        }
        public static void escribirOperacion(String destino, String val1, String op, String val2)
        {
            escribir(destino + " = " + val1 + op + val2 +";");
        }
        public static void aumentarH(String cantidad)
        {
            escribir("H = H + " + cantidad + ";");
        }
        public static void disminuirH(String cantidad)
        {
            escribir("H = H - " + cantidad + ";");
        }
        public static void aumentarP(String cantidad)
        {
            escribir("P = P + " + cantidad + ";");
        }
        public static void disminuirP(String cantidad)
        {
            escribir("P = P - " + cantidad + ";");
        }
        public static void escribirEnHeap(String pos, String valor)
        {
            escribir("Heap["+ pos +"] = " + valor + ";");
        }
        public static C3d leerDeHeap(String pos, List<String> temps)
        {
            C3d c3d = new C3d();
            c3d.cad = generarTemp(temps);
            c3d.esTemp = true;
            escribir(c3d.cad + " = Heap[" + pos + "];");
            return c3d;
        }
        public static void escribirEnPila(String pos, String valor)
        {
            escribir("Pila[" + pos + "] = " + valor + ";");
        }
        public static C3d leerDePila(String pos, List<String> temps)
        {
            C3d c3d = new C3d();
            c3d.cad = generarTemp(temps);
            c3d.esTemp = true;
            escribir(c3d.cad + " = Pila[" + pos + "];");
            return c3d;
        }
        public static String leerDePila(String pos)
        {
            String temp = generarTemp();
            escribir(temp + " = Pila[" + pos + "];");
            return temp;
        }
        public static void escribir(String cadena)
        {
            Utilidades.ManejadorArchivo.escribirC3d(cadena + "\n");
        }



    }
}
