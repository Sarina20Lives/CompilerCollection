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

        private static Interprete singleton = null;

        public static Interprete GetInstance()
        {
            if (singleton == null)
                singleton = new Interprete();
            return singleton;
        }

        public static Interprete ResetInstance()
        {
            singleton = new Interprete();
            return singleton;
        }

        private int P;
        private int H;
        private double[] Heap;
        private double[] Stack;
        private string Salida;
        private const Double NULL = -51233905.20302;
        private ParseTreeNode programa = null;

        private Interprete()
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
            Salida = "";
        }

        public void EjecutarC3D()
        {
            ParseTreeNode nodo = GramaticaC3D.AnalizarC3D();
            if (nodo == null)
            {
                MessageBox.Show("Existen errores en el código 3D generado.", "Ejecutando C3D");
                return;
            }
            Ejecutar(nodo, "Código 3 direcciones");
        }

        public void EjecutarC4P()
        {
            ParseTreeNode nodo = GramaticaC4P.AnalizarC4P();
            if (nodo == null)
            {
                MessageBox.Show("Existen errores en los cuádruplos generados.", "Ejecutando C4P");
                return;
            }
            Ejecutar(nodo, "Cuádruplos");
        }

        private void Ejecutar(ParseTreeNode nodo, string titulo)
        {
            this.programa = nodo;
            MessageBox.Show("Inicia la ejecución.", titulo);
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
                MessageBox.Show("No existe ningún método principal.", titulo);
                return;
            }
            Ejecutar(principal, new Dictionary<string, double>());
            MessageBox.Show("Salida:\n" + Salida, titulo);
        }

        public void Ejecutar(ParseTreeNode metodo, Dictionary<string, double> temps)
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
                        EjecutarAccesoStack(sentencia, temps);
                        break;
                    case "asignación a stack":
                        //Ejecutar asigna a stack
                        EjecutarAsignaStack(sentencia, temps);
                        break;
                    case "acceso a heap":
                        //Ejecutar acceso a heap
                        EjecutarAccesoHeap(sentencia, temps);
                        break;
                    case "asignación a heap":
                        //Ejecutar asigna a heap
                        EjecutarAsignaHeap(sentencia, temps);
                        break;
                    case "suma":
                        //Ejecutar suma
                        EjecutarAritmetica(sentencia, temps, '+');
                        break;
                    case "resta":
                        //Ejecutar resta
                        EjecutarAritmetica(sentencia, temps, '-');
                        break;
                    case "multiplicación":
                        //Ejecutar multiplicación
                        EjecutarAritmetica(sentencia, temps, '*');
                        break;
                    case "división":
                        //Ejecutar división
                        EjecutarAritmetica(sentencia, temps, '/');
                        break;
                    case "módulo":
                        //Ejecutar módulo
                        EjecutarAritmetica(sentencia, temps, '%');
                        break;
                    case "potencia":
                        //Ejecutar potencia
                        EjecutarAritmetica(sentencia, temps, '^');
                        break;
                    case "asignación":
                        //Ejecutar asignación
                        EjecutarAsignacion(sentencia, temps);
                        break;
                    case "salto":
                        //Ejecutar salto
                        i = EjecutarSalto(sentencia, cuerpo);
                        break;
                    case "salto si igual":
                        //Ejecutar salto si igual
                        i = EjecutarSaltoCondicional(sentencia, temps, cuerpo, i, "==");
                        break;
                    case "salto si diferente":
                        //Ejecutar salto si diferente
                        i = EjecutarSaltoCondicional(sentencia, temps, cuerpo, i, "!=");
                        break;
                    case "salto si mayor o igual":
                        //Ejecutar salto si mayor o igual
                        i = EjecutarSaltoCondicional(sentencia, temps, cuerpo, i, ">=");
                        break;
                    case "salto si menor o igual":
                        //Ejecutar salto si menor o igual
                        i = EjecutarSaltoCondicional(sentencia, temps, cuerpo, i, "<=");
                        break;
                    case "salto si mayor":
                        //Ejecutar salto si mayor
                        i = EjecutarSaltoCondicional(sentencia, temps, cuerpo, i, ">");
                        break;
                    case "salto si menor":
                        //Ejecutar salto si menor
                        i = EjecutarSaltoCondicional(sentencia, temps, cuerpo, i, "<");
                        break;
                    case "imprime":
                        //Ejecutar función para imprimir a la salida
                        EjecutarImprime(sentencia, temps);
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
            int retorno = 0;
            switch (sentencia.ChildNodes[0].Term.ToString())
            {
                case "intToStr":
                    Int32 i = (Int32)Stack[P + 1];
                    retorno = SetString(i.ToString());
                    break;
                case "doubleToStr":
                    Double d = Stack[P + 1];
                    retorno = SetString(d.ToString());
                    break;
                case "charToStr":
                    Char c = (Char)Stack[P + 1];
                    retorno = SetString(c.ToString());
                    break;
                case "boolToStr":
                    retorno = (Stack[P + 1] == 0) ? SetString("true") : SetString("false");
                    break;
                case "compareStr":
                    retorno = GetString((int)Stack[P + 1]).CompareTo(GetString((int)Stack[P + 2]));
                    break;
                case "outString":
                    Salida += GetString((int)Stack[P + 0]);
                    break;
                case "error":
                    //Error de ejecución controlado... código en Stack[P + 0]
                    //retorno = (int)Stack[P + 0];
                    break;
            }
            Stack[P + 0] = retorno;
        }

        private void EjecutarNonSQL(ParseTreeNode sentencia)
        {
            switch (sentencia.ChildNodes[0].Term.ToString())
            {
                case "create":
                    create();
                    break;
                case "drop":
                    drop();
                    break;
                case "insert":
                    insert();
                    break;
                case "update":
                    update();
                    break;
                case "delete":
                    delete();
                    break;
                case "select":
                    select();
                    break;
            }
        }

        private void EjecutarImprime(ParseTreeNode sentencia, Dictionary<string, double> temps)
        {
            string formato = sentencia.ChildNodes[0].FindTokenAndGetText();
            double valor = GetValor(sentencia.ChildNodes[1], temps);
            switch (formato)
            {
                case "\"%f\"":
                    Salida += valor;
                    break;
                case "\"%d\"":
                    Salida += (int)valor;
                    break;
                case "\"%c\"":
                    Salida += (char)valor;
                    break;
            }
        }

        private int EjecutarSaltoCondicional(ParseTreeNode sentencia, Dictionary<string, double> temps, ParseTreeNode cuerpo, int i, string p)
        {
            double valIzq = GetValor(sentencia.ChildNodes[0], temps);
            double valDer = GetValor(sentencia.ChildNodes[1], temps);
            string etiqueta = sentencia.ChildNodes[2].FindTokenAndGetText();
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

        private void EjecutarAsignacion(ParseTreeNode sentencia, Dictionary<string, double> temps)
        {
            double valor = GetValor(sentencia.ChildNodes[1], temps);
            SetValor(sentencia.ChildNodes[0], temps, valor);
        }

        private void EjecutarAritmetica(ParseTreeNode sentencia, Dictionary<string, double> temps, char p)
        {
            double valIzq = GetValor(sentencia.ChildNodes[1], temps);
            double valDer = GetValor(sentencia.ChildNodes[2], temps);
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
            SetValor(sentencia.ChildNodes[0], temps, valIzq);
        }

        private void EjecutarAsignaHeap(ParseTreeNode sentencia, Dictionary<string, double> temps)
        {
            int indice = (int)GetValor(sentencia.ChildNodes[0], temps);
            double valor = GetValor(sentencia.ChildNodes[1], temps);
            Heap[indice] = valor;
        }

        private void EjecutarAccesoHeap(ParseTreeNode sentencia, Dictionary<string, double> temps)
        {
            int indice = (int)GetValor(sentencia.ChildNodes[1], temps);
            SetValor(sentencia.ChildNodes[0], temps, Heap[indice]);
        }

        private void EjecutarAsignaStack(ParseTreeNode sentencia, Dictionary<string, double> temps)
        {
            int indice = (int)GetValor(sentencia.ChildNodes[0], temps);
            double valor = GetValor(sentencia.ChildNodes[1], temps);
            Stack[indice] = valor;
        }

        private void EjecutarAccesoStack(ParseTreeNode sentencia, Dictionary<string, double> temps)
        {
            int indice = (int)GetValor(sentencia.ChildNodes[1], temps);
            SetValor(sentencia.ChildNodes[0], temps, Stack[indice]);
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
            Ejecutar(metodo, new Dictionary<string, double>());
        }

        private ParseTreeNode BuscarMetodo(string identificador)
        {
            foreach (var metodo in programa.ChildNodes)
                if (metodo.ChildNodes[0].FindTokenAndGetText() == identificador)
                    return metodo;
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

        private void SetValor(ParseTreeNode destino, Dictionary<string, double> temps, double valor)
        {
            string tipoTerminal = destino.Term.ToString();
            switch (tipoTerminal)
            {
                case "temporal":
                    temps[destino.FindTokenAndGetText()] = valor;
                    break;
                case "H":
                    H = (int)valor;
                    break;
                case "P":
                    P = (int)valor;
                    break;
            }
        }

        private double GetValor(ParseTreeNode nodo, Dictionary<string, double> temps)
        {
            string tipoTerminal = nodo.Term.ToString();
            double valor = 0;
            switch (tipoTerminal)
            {
                case "temporal":
                    valor = temps[nodo.FindTokenAndGetText()];
                    break;
                case "entero":
                case "decimal":
                    valor = Convert.ToDouble(nodo.FindTokenAndGetText());
                    break;
                case "H":
                    return H;
                case "P":
                    return P;
                case "NULL":
                    return NULL;
            }
            return valor;
        }

        private string GetString(int ptr)
        {
            string str = "";
            if (Heap[ptr] == NULL)
            {
                //Error, null pointer exception
                return "";
            }
            int indice = (int)Heap[ptr];
            char c = (char)Heap[indice];
            while (c != 0)
            {
                str += c;
                c = (char)Heap[++indice];
            }
            return str;
        }

        private int SetString(string str)
        {
            int ptr = H;
            Heap[ptr] = ++H;
            foreach (char c in str)
                Heap[H++] = c;
            Heap[H++] = 0;
            return ptr;
        }

        /**
         * STACK
         * | 0 | -> this
         * | 1 | -> return
         * | 2 | -> PTR al nombre de la colección
         * | 3 | -> número de columnas
         * | 4 | -> PTR a la primera columna
         * | 5 | -> PTR a la segunda columna
         * |...| -> ...
         * | x | -> PTR a la n-ésima columna
         */
        private void create()
        {
            string nombre = GetString((int)Stack[P + 2]);
            int nocolumnas = (int)Stack[P + 3];
            string[] columnas = new string[nocolumnas];
            int paux = P + 4;
            for (int i = 0; i < nocolumnas; i++)
                columnas[i] = GetString((int)Stack[paux + i]);
            CollectionNonSQL.create(nombre, columnas);
        }

        /**
         * STACK
         * | 0 | -> this
         * | 1 | -> return
         * | 2 | -> PTR al nombre de la colección
         */
        private void drop()
        {
            string nombre = GetString((int)Stack[P + 2]);
            CollectionNonSQL.drop(nombre);
        }

        /**
         * STACK
         * | 0 | -> this
         * | 1 | -> return
         * | 2 | -> PTR al nombre de la colección
         * | 3 | -> número de valores - columnas
         * | 4 | -> PTR a la primera columna
         * |...| -> ...
         * | x | -> PTR a la n-ésima columna
         * |x+1| -> PTR al primer valor
         * |...| -> ...
         * | y | -> PTR al n-ésimo valor
         */
        private void insert()
        {
            string nombre = GetString((int)Stack[P + 2]);
            int noColumnas = (int)Stack[P + 3];
            string[] columnas = new string[noColumnas];
            string[] valores = new string[noColumnas];
            int indiceAux = P + 4;
            for (int i = 0; i < noColumnas; i++)
            {
                indiceAux += i;
                columnas[i] = GetString((int)Stack[indiceAux]);
                valores[i] = GetString((int)Stack[indiceAux + noColumnas]);
            }
            CollectionNonSQL.insert(nombre, columnas, valores);
        }

        /**
         * STACK
         * | 0 | -> this
         * | 1 | -> return
         * | 2 | -> PTR al nombre de la colección
         * | 3 | -> PTR al nombre de la columna
         * | 4 | -> PTR al valor para borrar
         */
        private void delete()
        {
            string nombre = GetString((int)Stack[P + 2]);
            string columna = GetString((int)Stack[P + 3]);
            string valor = GetString((int)Stack[P + 4]);
            CollectionNonSQL.delete(nombre, columna, valor);
        }

        /**
         * STACK
         * | 0 | -> this
         * | 1 | -> return
         * | 2 | -> PTR al nombre de la colección
         * | 3 | -> PTR al nombre de la columna
         * | 4 | -> PTR al valor para actualiza
         * | 5 | -> PTR al valor nuevo
         */
        private void update()
        {
            string nombre = GetString((int)Stack[P + 2]);
            string columna = GetString((int)Stack[P + 3]);
            string valor = GetString((int)Stack[P + 4]);
            string nuevo = GetString((int)Stack[P + 5]);
            CollectionNonSQL.update(nombre, columna, valor, nuevo);
        }

        /**
         * STACK
         * | 0 | -> this
         * | 1 | -> return
         * | 2 | -> PTR al nombre de la colección
         * | 3 | -> número de columnas
         * | 4 | -> PTR a la primera columna
         * | 5 | -> PTR a la segunda columna
         * |...| -> ...
         * | x | -> PTR a la n-ésima columna
         */
        private void select()
        {
            string nombre = GetString((int)Stack[P + 2]);
            int nocolumnas = (int)Stack[P + 3];
            string[] columnas = new string[nocolumnas];
            int paux = P + 4;
            for (int i = 0; i < nocolumnas; i++)
                columnas[i] = GetString((int)Stack[paux + i]);
            string texto = CollectionNonSQL.select(nombre, columnas);
            Salida += "\n" + texto + "\n";
        }

        public string GetSalida()
        {
            return Salida;
        }

    }
}
