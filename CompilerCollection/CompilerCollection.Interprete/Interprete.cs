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
                    salida += GetString((int)Stack[P + 0]);
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
        public void drop()
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
         * | 5 | -> PTR al primer valor
         * |...| -> ...
         * | x | -> PTR a la n-ésima columna
         * | y | -> PTR al n-ésimo valor
         */
        public void insert()
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
                valores[i]  = GetString((int)Stack[indiceAux + noColumnas]);
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
        public void delete()
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
        public void update()
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
        public void select()
        {
            string nombre = GetString((int)Stack[P + 2]);
            int nocolumnas = (int)Stack[P + 3];
            string[] columnas = new string[nocolumnas];
            int paux = P + 4;
            for (int i = 0; i < nocolumnas; i++)
                columnas[i] = GetString((int)Stack[paux + i]);
            string texto = CollectionNonSQL.select(nombre, columnas);
            salida += "\n" + texto + "\n";
        }

    }
}
