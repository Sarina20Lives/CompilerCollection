using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerCollection.CompilerCollection.General
{
    public class Constantes
    {
        public static String ROL_VAR = "Variable";


        public static int ERROR = -1;
        //Tipos
        public static int T_VOID = 0;
        public static int T_BOOLEAN = 1;
        public static int T_INT = 2;
        public static int T_DOUBLE = 3;
        public static int T_CHAR = 4;
        public static int T_STRING = 5;
        public static int T_OBJETO = 6;
        public static String[] TIPOS= {"void", "boolean", "int", "double", "char", "string", "objeto"};

        public static int obtenerTipo(String tipo) {
            if (tipo.Equals("void", StringComparison.OrdinalIgnoreCase)) {
                return T_VOID;
            }
            if (tipo.Equals("boolean", StringComparison.OrdinalIgnoreCase))
            {
                return T_BOOLEAN;
            }
            if (tipo.Equals("int", StringComparison.OrdinalIgnoreCase))
            {
                return T_INT;
            }
            if (tipo.Equals("double", StringComparison.OrdinalIgnoreCase))
            {
                return T_DOUBLE;
            }
            if (tipo.Equals("char", StringComparison.OrdinalIgnoreCase))
            {
                return T_CHAR;
            }
            if (tipo.Equals("string", StringComparison.OrdinalIgnoreCase))
            {
                return T_STRING;
            }
            return T_OBJETO;        
        }

        //Errores
        public static int ERROR_DIV_CERO = 0;
        public static int ERROR_MOD_CERO = 0;



    }
}
