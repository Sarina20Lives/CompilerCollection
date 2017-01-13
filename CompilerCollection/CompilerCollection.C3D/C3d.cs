using CompilerCollection.CompilerCollection.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static String generarTemp(List<String> temps)
        {
            String temp = "t" + contTemp;
            contTemp = contTemp + 1;
            temps.Add(temp);
            return temp;
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


        public void castIntToString() {
            C3d c3d = new C3d();
            


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


        public static void escribir(String cadena)
        {
            cadena = cadena + "\n";
            ManejadorArchivo.ManejadorArchivo.escribirC3d(cadena);
        }


    }
}
