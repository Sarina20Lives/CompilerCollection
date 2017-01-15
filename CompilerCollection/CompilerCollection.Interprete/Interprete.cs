using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Irony.Parsing;
using System.Collections.Generic;

namespace CompilerCollection.CompilerCollection.Interprete
{
    class Interprete
    {
        private int P;
        private int H;
        private double [] Heap;
        private double [] Stack;
        private Dictionary<string, double> Temps;
        private string salida;
        private const Double NULL = -51233905.20302;
        private const string RUTA_C3D = "C:\\FilesCompilerCollection\\c3d.txt";
        private ParseTreeNode programa = null;

        public Interprete()
        {
            int i; this.P = 0; H = 0;
            Heap = new Double[10000];
            i = 0;
            while (i < Heap.Length)
                Heap[i++] = NULL;
            Stack = new Double[10000];
            i = 0;
            while (i < Stack.Length)
                Stack[i++] = NULL;
            salida = "";
            Temps = new Dictionary<string, double>();
        }

        public void Ejecutar()
        {
            string c3d = File.ReadAllText(RUTA_C3D);
            GramaticaC3D gramatica = new GramaticaC3D();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(c3d);
            if (arbol.Root == null)
            {
                MessageBox.Show("Ejecutando C3D", "Existen errores en el código 3D generado.");
                return;
            }
            this.programa = arbol.Root;
            MessageBox.Show("Ejecutando C3D", "Inicia la ejecución del código 3D.");
            ParseTreeNode principal = null;
            foreach (var metodo in programa.ChildNodes)
            {
                if (metodo.ToString() == "principal")
                {
                    principal = metodo;
                    break;
                }
            }
            if (principal == null)
            {
                MessageBox.Show("Ejecutando C3D", "No existe ningún método principal.");
                return;
            }
            Ejecutar(principal);
            MessageBox.Show("Ejecutando C3D", "Finaliza la ejecución del código 3D.");
        }

        public void Ejecutar(ParseTreeNode metodo)
        {
            ParseTreeNode cuerpo = metodo.ChildNodes[1];
            for (int i = 0; i < cuerpo.ChildNodes.Count; i++)
            {
                var sentencia = cuerpo.ChildNodes[i];
                switch (sentencia.ToString())
                {
                    case "etiqueta":
                        //Ignorar etiqueta
                        break;
                    case "llamada":
                        //Ejecutar llamada
                        EjecutarLlamada(sentencia);
                        break;
                    case "acceso a stack":
                        //Ejecutar acceso a stack
                        EjecutarAccesoStack(sentencia);
                        break;
                    case "asigna a stack":
                        //Ejecutar asigna a stack
                        EjecutarAsignaStack(sentencia);
                        break;
                    case "acceso a heap":
                        //Ejecutar acceso a heap
                        EjecutarAccesoHeap(sentencia);
                        break;
                    case "asigna a heap":
                        //Ejecutar asigna a heap
                        EjecutarAsignaHeap(sentencia);
                        break;
                    case "suma":
                        //Ejecutar suma
                        EjecutarAritmetica(sentencia, '+');
                        break;
                    case "resta":
                        //Ejecutar resta
                        EjecutarAritmetica(sentencia, '-');
                        break;
                    case "multiplicación":
                        //Ejecutar multiplicación
                        EjecutarAritmetica(sentencia, '*');
                        break;
                    case "división":
                        //Ejecutar división
                        EjecutarAritmetica(sentencia, '/');
                        break;
                    case "módulo":
                        //Ejecutar módulo
                        EjecutarAritmetica(sentencia, '%');
                        break;
                    case "potencia":
                        //Ejecutar potencia
                        EjecutarAritmetica(sentencia, '^');
                        break;
                    case "asignación":
                        //Ejecutar asignación
                        EjecutarAsignacion(sentencia);
                        break;
                    case "salto":
                        //Ejecutar salto
                        i = EjecutarSalto(sentencia, cuerpo);
                        break;
                    case "salto si igual":
                        //Ejecutar salto si igual
                        i = EjecutarSaltoCondicional(sentencia, cuerpo, i, "==");
                        break;
                    case "salto si diferente":
                        //Ejecutar salto si diferente
                        i = EjecutarSaltoCondicional(sentencia, cuerpo, i, "!=");
                        break;
                    case "salto si mayor o igual":
                        //Ejecutar salto si mayor o igual
                        i = EjecutarSaltoCondicional(sentencia, cuerpo, i, ">=");
                        break;
                    case "salto si menor o igual":
                        //Ejecutar salto si menor o igual
                        i = EjecutarSaltoCondicional(sentencia, cuerpo, i, "<=");
                        break;
                    case "salto si mayor":
                        //Ejecutar salto si mayor
                        i = EjecutarSaltoCondicional(sentencia, cuerpo, i, ">");
                        break;
                    case "salto si menor":
                        //Ejecutar salto si menor
                        i = EjecutarSaltoCondicional(sentencia, cuerpo, i, "<");
                        break;
                    case "imprime":
                        //Ejecutar función para imprimir a la salida
                        EjecutarImprime(sentencia);
                        break;
                    case "nonsql":
                        //Ejecutar función nonsql
                        EjecutarNonSQL(sentencia);
                        break;
                    case "core":
                        //Ejecutar función del core
                        EjecutarCore(sentencia);
                        break;
                    default:
                        break;
                }
            }
        }

        private void EjecutarCore(ParseTreeNode sentencia)
        {
            switch (sentencia.ChildNodes[0].Term.ToString())
            {
                case "intToStr":
                    break;
                case "doubleToStr":
                    break;
                case "charToStr":
                    break;
                case "boolToStr":
                    break;
                case "compareStr":
                    break;
                case "error":
                    break;
            }
        }

        private void EjecutarNonSQL(ParseTreeNode sentencia)
        {
            switch (sentencia.ChildNodes[0].Term.ToString())
            {
                case "create":
                    break;
                case "drop":
                    break;
                case "insert":
                    break;
                case "update":
                    break;
                case "delete":
                    break;
                case "select":
                    break;
            }
        }

        private void EjecutarImprime(ParseTreeNode sentencia)
        {
            string formato = sentencia.ChildNodes[1].FindTokenAndGetText();
            double valor = GetValor(sentencia.ChildNodes[2]);
            switch (formato)
            {
                case "\"%f\"":
                    salida += valor;
                    break;
                case "\"%d\"":
                    salida += (int)valor;
                    break;
                case "\"%c\"":
                    salida += (char)valor;
                    break;
            }
        }

        private int EjecutarSaltoCondicional(ParseTreeNode sentencia, ParseTreeNode cuerpo, int i, string p)
        {
            double valIzq = GetValor(sentencia.ChildNodes[1]);
            double valDer = GetValor(sentencia.ChildNodes[2]);
            string etiqueta = sentencia.ChildNodes[3].FindTokenAndGetText();
            switch (p)
            {
                case "==":
                    if (valIzq == valDer)
                        i = BuscarEtiqueta(etiqueta, cuerpo);
                    break;
                case "!=":
                    if (valIzq != valDer)
                        i = BuscarEtiqueta(etiqueta, cuerpo);
                    break;
                case ">=":
                    if (valIzq >= valDer)
                        i = BuscarEtiqueta(etiqueta, cuerpo);
                    break;
                case "<=":
                    if (valIzq <= valDer)
                        i = BuscarEtiqueta(etiqueta, cuerpo);
                    break;
                case ">":
                    if (valIzq > valDer)
                        i = BuscarEtiqueta(etiqueta, cuerpo);
                    break;
                case "<":
                    if (valIzq < valDer)
                        i = BuscarEtiqueta(etiqueta, cuerpo);
                    break;
            }
            return i;
        }

        private int EjecutarSalto(ParseTreeNode sentencia, ParseTreeNode cuerpo)
        {
            string etiqueta = sentencia.ChildNodes[0].FindTokenAndGetText();
            return BuscarEtiqueta(etiqueta, cuerpo);
        }

        private void EjecutarAsignacion(ParseTreeNode sentencia)
        {
            double valor = GetValor(sentencia.ChildNodes[1]);
            SetValor(sentencia.ChildNodes[0], valor);
        }

        private void EjecutarAritmetica(ParseTreeNode sentencia, char p)
        {
            double valIzq = GetValor(sentencia.ChildNodes[1]);
            double valDer = GetValor(sentencia.ChildNodes[2]);
            switch (p)
            {
                case '+':
                    valIzq += valDer;
                    break;
                case '-':
                    valIzq -= valDer;
                    break;
                case '*':
                    valIzq *= valDer;
                    break;
                case '/':
                    valIzq /= valDer;
                    break;
                case '%':              
                    valIzq %= valDer;
                    break;
                case '^':
                    valIzq = Math.Pow(valIzq, valDer);
                    break;
            }
            SetValor(sentencia.ChildNodes[0], valIzq);
        }

        private void EjecutarAsignaHeap(ParseTreeNode sentencia)
        {
            int indice = (int)GetValor(sentencia.ChildNodes[1]);
            double valor = GetValor(sentencia.ChildNodes[2]);
            Heap[indice] = valor;
        }

        private void EjecutarAccesoHeap(ParseTreeNode sentencia)
        {
            int indice = (int)GetValor(sentencia.ChildNodes[2]);
            SetValor(sentencia.ChildNodes[0], Heap[indice]);
        }

        private void EjecutarAsignaStack(ParseTreeNode sentencia)
        {
            int indice = (int)GetValor(sentencia.ChildNodes[1]);
            double valor = GetValor(sentencia.ChildNodes[2]);
            Stack[indice] = valor;
        }

        private void EjecutarAccesoStack(ParseTreeNode sentencia)
        {
            int indice = (int)GetValor(sentencia.ChildNodes[2]);
            SetValor(sentencia.ChildNodes[0], Stack[indice]);
        }

        private void EjecutarLlamada(ParseTreeNode nodo)
        {
            string id = nodo.FindTokenAndGetText();
            ParseTreeNode metodo = BuscarMetodo(id);
            if (metodo == null)
            {
                //Error(No existe el método... id)
                return;
            }
            Ejecutar(metodo);
        }

        private void SetValor(ParseTreeNode destino, double valor)
        {
            string tipoTerminal = destino.Term.ToString();
            switch (tipoTerminal)
            {
                case "temporal":
                    Temps[destino.FindTokenAndGetText()] = valor;
                    break;
                case "H":
                    H = (int)valor;
                    break;
                case "P":
                    P = (int)valor;
                    break;
            }
        }

        private double GetValor(ParseTreeNode nodo)
        {
            string tipoTerminal = nodo.Term.ToString();
            double valor = 0;
            switch (tipoTerminal)
            {
                case "temporal":
                    valor = Temps[nodo.FindTokenAndGetText()];
                    break;
                case "entero":
                case "decimal":
                    valor = Convert.ToDouble(nodo.FindTokenAndGetText());
                    break;
                case "H":
                    return H;
                case "P":
                    return P;
            }
            return valor;
        }

        private ParseTreeNode BuscarMetodo(string identificador)
        {
            foreach (var metodo in programa.ChildNodes)
            {
                if (metodo.ChildNodes[0].FindTokenAndGetText() == identificador)
                {
                    return metodo;
                }
            }
            return null;
        }

        private int BuscarEtiqueta(string etiqueta, ParseTreeNode cuerpo)
        {
            int i = 0;
            foreach (var nodo in cuerpo.ChildNodes)
            {
                if (nodo.ToString() == "etiqueta")
                    if (nodo.FindTokenAndGetText() == etiqueta)
                        return i;
                i++;
            }
            return i;
        }

    }
}
