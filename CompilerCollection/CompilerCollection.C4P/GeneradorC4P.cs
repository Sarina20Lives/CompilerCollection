using CompilerCollection.CompilerCollection.Interprete;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerCollection.CompilerCollection.C4P
{
    class GeneradorC4P
    {
        private const string RUTA_C4P = "C:\\FilesCompilerCollection\\cuadruplos.txt";

        private ParseTreeNode programa;
        private string salida;
        public GeneradorC4P()
        {
            programa = GramaticaC3D.AnalizarC3D();
            salida = "";
        }

        public void GenerarCuadruplos() 
        {
            foreach (var metodo in programa.ChildNodes)
            {
                string idMetodo = metodo.ChildNodes[0].FindTokenAndGetText();
                salida += "begin, , , " + idMetodo +"\n";
                GenerarCuadruplos(metodo.ChildNodes[1]);
                salida += "end, , , " + idMetodo + "\n\n";
            }
            File.WriteAllText(RUTA_C4P, salida);
        }

        private void GenerarCuadruplos(ParseTreeNode cuerpo)
        {
            foreach (var sentencia in cuerpo.ChildNodes)
            {
                switch (sentencia.ToString())
                {
                    case "etiqueta":
                        //Ignorar etiqueta
                        salida += "\t" + GetString(sentencia, 0) + ":\n";
                        break;
                    case "llamada":
                        //Ejecutar llamada
                        salida += "\tcall, , , " + GetString(sentencia, 0) + "\n";
                        break;
                    case "acceso a stack":
                        //Ejecutar acceso a stack
                        salida += "\t=>, Stack, " + GetString(sentencia, 1) + ", " + GetString(sentencia, 0) +  "\n";
                        break;
                    case "asignación a stack":
                        //Ejecutar asigna a stack
                        salida += "\t<=, " + GetString(sentencia, 0) + ", " + GetString(sentencia, 1) + ", Stack\n";
                        break;
                    case "acceso a heap":
                        //Ejecutar acceso a heap
                        salida += "\t=>, Heap, " + GetString(sentencia, 1) + ", " + GetString(sentencia, 0) + "\n";
                        break;
                    case "asignación a heap":
                        //Ejecutar asigna a heap
                        salida += "\t<=, " + GetString(sentencia, 0) + ", " + GetString(sentencia, 1) + ", Heap\n";
                        break;
                    case "suma":
                        //Ejecutar suma
                        salida += "\t+, " + GetString(sentencia, 1) + ", " + GetString(sentencia, 2) + ", " + GetString(sentencia, 0) + "\n";
                        break;
                    case "resta":
                        //Ejecutar resta
                        salida += "\t-, " + GetString(sentencia, 1) + ", " + GetString(sentencia, 2) + ", " + GetString(sentencia, 0) + "\n";
                        break;
                    case "multiplicación":
                        //Ejecutar multiplicación
                        salida += "\t*, " + GetString(sentencia, 1) + ", " + GetString(sentencia, 2) + ", " + GetString(sentencia, 0) + "\n";
                        break;
                    case "división":
                        //Ejecutar división
                        salida += "\t/, " + GetString(sentencia, 1) + ", " + GetString(sentencia, 2) + ", " + GetString(sentencia, 0) + "\n";
                        break;
                    case "módulo":
                        //Ejecutar módulo
                        salida += "\t%, " + GetString(sentencia, 1) + ", " + GetString(sentencia, 2) + ", " + GetString(sentencia, 0) + "\n";
                        break;
                    case "potencia":
                        //Ejecutar potencia
                        salida += "\t^, " + GetString(sentencia, 1) + ", " + GetString(sentencia, 2) + ", " + GetString(sentencia, 0) + "\n";
                        break;
                    case "asignación":
                        //Ejecutar asignación
                        salida += "\t=, " + GetString(sentencia, 1) + ", , " + GetString(sentencia, 0) + "\n";
                        break;
                    case "salto":
                        //Ejecutar salto
                        salida += "\tjmp, , , " + GetString(sentencia, 0) + "\n";
                        break;
                    case "salto si igual":
                        //Ejecutar salto si igual
                        salida += "\tje, " + GetString(sentencia, 0) + ", " + GetString(sentencia, 1) + ", " + GetString(sentencia, 2) + "\n";
                        break;
                    case "salto si diferente":
                        //Ejecutar salto si diferente
                        salida += "\tjne, " + GetString(sentencia, 0) + ", " + GetString(sentencia, 1) + ", " + GetString(sentencia, 2) + "\n";
                        break;
                    case "salto si mayor o igual":
                        //Ejecutar salto si mayor o igual
                        salida += "\tjge, " + GetString(sentencia, 0) + ", " + GetString(sentencia, 1) + ", " + GetString(sentencia, 2) + "\n";
                        break;
                    case "salto si menor o igual":
                        //Ejecutar salto si menor o igual
                        salida += "\tjle, " + GetString(sentencia, 0) + ", " + GetString(sentencia, 1) + ", " + GetString(sentencia, 2) + "\n";
                        break;
                    case "salto si mayor":
                        //Ejecutar salto si mayor
                        salida += "\tjg, " + GetString(sentencia, 0) + ", " + GetString(sentencia, 1) + ", " + GetString(sentencia, 2) + "\n";
                        break;
                    case "salto si menor":
                        //Ejecutar salto si menor
                        salida += "\tjl, " + GetString(sentencia, 0) + ", " + GetString(sentencia, 1) + ", " + GetString(sentencia, 2) + "\n";
                        break;
                    case "imprime":
                        //Ejecutar función para imprimir a la salida
                        salida += "\tprintf, " + GetString(sentencia, 1) + ", " + GetString(sentencia, 0) + ",\n";
                        break;
                    case "nonsql":
                        //Ejecutar función nonsql
                        salida += "\tcall, , , " + GetString(sentencia, 0) + "\n";
                        break;
                    case "core":
                        //Ejecutar función del core
                        salida += "\tcall, , , " + GetString(sentencia, 0) + "\n";
                        break;
                    default:
                        break;
                }
            }
        }

        private string GetString(ParseTreeNode nodo, int hijo)
        {
            return nodo.ChildNodes[hijo].FindTokenAndGetText();
        }

    }
}
