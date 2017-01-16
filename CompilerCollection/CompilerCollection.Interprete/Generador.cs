using CompilerCollection.CompilerCollection.Utilidades;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerCollection.CompilerCollection.Interprete
{
    class Generador
    {

        private ParseTreeNode programa;
        private string salida;
        private bool esOptimizado;

        public Generador(bool esOptimizado = false)
        {
            this.programa = null;
            this.salida = "";
            this.esOptimizado = esOptimizado;
        }

        public void GenerarC3D(ParseTreeNode programa)
        {
            this.programa = programa;
            Generar(true);
        }

        public void GenerarC3D()
        {
            this.programa = GramaticaC4P.AnalizarC4P();
            Generar(true);
        }

        public void GenerarCuadruplos(ParseTreeNode programa)
        {
            this.programa = programa;
            Generar(false);
        }

        public void GenerarCuadruplos()
        {
            this.programa = GramaticaC3D.AnalizarC3D();
            Generar(false);
        }

        private void Generar(bool esC3D)
        {
            if (programa == null)
            {
                System.Windows.Forms.MessageBox.Show(
                    (esC3D) ? "Imposible generar código de tres direcciones." : "Imposible generar cuádruplos.",
                    "Generador");
                return;
            }
            foreach (var metodo in programa.ChildNodes)
            {
                string idMetodo = metodo.ChildNodes[0].FindTokenAndGetText();
                salida += (esC3D) ? 
                    "void "+idMetodo+"(){\n" :
                    "begin, , , " + idMetodo + "\n" ;
                Generar(metodo.ChildNodes[1], esC3D);
                salida += (esC3D)?
                    "}\n\n" :
                    "end, , , " + idMetodo + "\n\n" ;
            }
            string ruta = (esC3D) ? 
                (esOptimizado) ? GramaticaC3D.RUTA_C3D_OPTIMIZADO : GramaticaC3D.RUTA_C3D :
                (esOptimizado) ? GramaticaC4P.RUTA_C4P_OPTIMIZADO : GramaticaC4P.RUTA_C4P ;
            File.WriteAllText(ruta, salida);
        }

        private void Generar(ParseTreeNode cuerpo, bool esC3D)
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
                        salida += (esC3D) ?
                            "\t" + GetString(sentencia, 0) + "();\n" :
                            "\tcall, , , " + GetString(sentencia, 0) + "\n" ;
                        break;
                    case "acceso a stack":
                        //Ejecutar acceso a stack
                        salida += (esC3D) ?
                            "\t" + GetString(sentencia, 0) + " = Stack[" + GetString(sentencia, 1) + "];\n" :
                            "\t=>, Stack, " + GetString(sentencia, 1) + ", " + GetString(sentencia, 0) + "\n" ;
                        break;
                    case "asignación a stack":
                        //Ejecutar asigna a stack
                        salida += (esC3D) ?
                            "\tStack[" + GetString(sentencia, 0) + "] = " + GetString(sentencia, 1) + ";\n" :
                            "\t<=, " + GetString(sentencia, 0) + ", " + GetString(sentencia, 1) + ", Stack\n" ;
                        break;
                    case "acceso a heap":
                        //Ejecutar acceso a heap
                        salida += (esC3D) ?
                            "\t" + GetString(sentencia, 0) + " = Heap[" + GetString(sentencia, 1) + "];\n" :
                            "\t=>, Heap, " + GetString(sentencia, 1) + ", " + GetString(sentencia, 0) + "\n" ;
                        break;
                    case "asignación a heap":
                        //Ejecutar asigna a heap
                        salida += (esC3D) ?
                            "\tHeap[" + GetString(sentencia, 0) + "] = " + GetString(sentencia, 1) + ";\n" :
                            "\t<=, " + GetString(sentencia, 0) + ", " + GetString(sentencia, 1) + ", Heap\n" ;
                        break;
                    case "suma":
                        //Ejecutar suma
                        salida += (esC3D) ?
                            "\t" + GetString(sentencia, 0) + " = " + GetString(sentencia, 1) + " + " + GetString(sentencia, 2) + ";\n" :
                            "\t+, " + GetString(sentencia, 1) + ", " + GetString(sentencia, 2) + ", " + GetString(sentencia, 0) + "\n" ;
                        break;
                    case "resta":
                        //Ejecutar resta
                        salida += (esC3D) ?
                            "\t" + GetString(sentencia, 0) + " = " + GetString(sentencia, 1) + " - " + GetString(sentencia, 2) + ";\n" :
                            "\t-, " + GetString(sentencia, 1) + ", " + GetString(sentencia, 2) + ", " + GetString(sentencia, 0) + "\n" ;
                        break;
                    case "multiplicación":
                        //Ejecutar multiplicación
                        salida += (esC3D) ?
                            "\t" + GetString(sentencia, 0) + " = " + GetString(sentencia, 1) + " * " + GetString(sentencia, 2) + ";\n" :
                            "\t*, " + GetString(sentencia, 1) + ", " + GetString(sentencia, 2) + ", " + GetString(sentencia, 0) + "\n" ;
                        break;
                    case "división":
                        //Ejecutar división
                        salida += (esC3D) ?
                            "\t" + GetString(sentencia, 0) + " = " + GetString(sentencia, 1) + " / " + GetString(sentencia, 2) + ";\n" :
                            "\t/, " + GetString(sentencia, 1) + ", " + GetString(sentencia, 2) + ", " + GetString(sentencia, 0) + "\n" ;
                        break;
                    case "módulo":
                        //Ejecutar módulo
                        salida += (esC3D) ?
                            "\t" + GetString(sentencia, 0) + " = " + GetString(sentencia, 1) + " % " + GetString(sentencia, 2) + ";\n" :
                            "\t%, " + GetString(sentencia, 1) + ", " + GetString(sentencia, 2) + ", " + GetString(sentencia, 0) + "\n" ;
                        break;
                    case "potencia":
                        //Ejecutar potencia
                        salida += (esC3D) ?
                            "\t" + GetString(sentencia, 0) + " = " + GetString(sentencia, 1) + " ^ " + GetString(sentencia, 2) + ";\n" :
                            "\t^, " + GetString(sentencia, 1) + ", " + GetString(sentencia, 2) + ", " + GetString(sentencia, 0) + "\n" ;
                        break;
                    case "asignación":
                        //Ejecutar asignación
                        salida += (esC3D) ?
                            "\t" + GetString(sentencia, 0) + " = " + GetString(sentencia, 1) + ";\n" :
                            "\t=, " + GetString(sentencia, 1) + ", , " + GetString(sentencia, 0) + "\n" ;
                        break;
                    case "salto":
                        //Ejecutar salto
                        salida += (esC3D) ?
                            "\tgoto " + GetString(sentencia, 0) + ";\n" :
                            "\tjmp, , , " + GetString(sentencia, 0) + "\n" ;
                        break;
                    case "salto si igual":
                        //Ejecutar salto si igual
                        salida += (esC3D) ?
                            "\tif(" + GetString(sentencia, 0) + " == " + GetString(sentencia, 1) + ") goto " + GetString(sentencia, 2) + ";\n" :
                            "\tje, " + GetString(sentencia, 0) + ", " + GetString(sentencia, 1) + ", " + GetString(sentencia, 2) + "\n" ;
                        break;
                    case "salto si diferente":
                        //Ejecutar salto si diferente
                        salida += (esC3D) ?
                            "\tif(" + GetString(sentencia, 0) + " != " + GetString(sentencia, 1) + ") goto " + GetString(sentencia, 2) + ";\n" :
                            "\tjne, " + GetString(sentencia, 0) + ", " + GetString(sentencia, 1) + ", " + GetString(sentencia, 2) + "\n" ;
                        break;
                    case "salto si mayor o igual":
                        //Ejecutar salto si mayor o igual
                        salida += (esC3D) ?
                            "\tif(" + GetString(sentencia, 0) + " >= " + GetString(sentencia, 1) + ") goto " + GetString(sentencia, 2) + ";\n" :
                            "\tjge, " + GetString(sentencia, 0) + ", " + GetString(sentencia, 1) + ", " + GetString(sentencia, 2) + "\n" ;
                        break;
                    case "salto si menor o igual":
                        //Ejecutar salto si menor o igual
                        salida += (esC3D) ?
                            "\tif(" + GetString(sentencia, 0) + " <= " + GetString(sentencia, 1) + ") goto " + GetString(sentencia, 2) + ";\n" :
                            "\tjle, " + GetString(sentencia, 0) + ", " + GetString(sentencia, 1) + ", " + GetString(sentencia, 2) + "\n" ;
                        break;
                    case "salto si mayor":
                        //Ejecutar salto si mayor
                        salida += (esC3D) ?
                            "\tif(" + GetString(sentencia, 0) + " > " + GetString(sentencia, 1) + ") goto " + GetString(sentencia, 2) + ";\n" :
                            "\tjg, " + GetString(sentencia, 0) + ", " + GetString(sentencia, 1) + ", " + GetString(sentencia, 2) + "\n" ;
                        break;
                    case "salto si menor":
                        //Ejecutar salto si menor
                        salida += (esC3D) ?
                            "\tif(" + GetString(sentencia, 0) + " < " + GetString(sentencia, 1) + ") goto " + GetString(sentencia, 2) + ";\n" :
                            "\tjl, " + GetString(sentencia, 0) + ", " + GetString(sentencia, 1) + ", " + GetString(sentencia, 2) + "\n" ;
                        break;
                    case "imprime":
                        //Ejecutar función para imprimir a la salida
                        salida += (esC3D) ? 
                            "\tprintf(" + GetString(sentencia, 0) + ", " + GetString(sentencia, 1) + ");\n" :
                            "\tprintf, " + GetString(sentencia, 1) + ", " + GetString(sentencia, 0) + ",\n" ;
                        break;
                    case "nonsql":
                        //Ejecutar función nonsql
                        salida += (esC3D) ?
                            "\t" + GetString(sentencia, 0) + "();\n" :
                            "\tcall, , , " + GetString(sentencia, 0) + "\n";
                        break;
                    case "core":
                        //Ejecutar función del core
                        salida += (esC3D) ?
                            "\t" + GetString(sentencia, 0) + "();\n" :
                            "\tcall, , , " + GetString(sentencia, 0) + "\n";
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
