using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerCollection.CompilerCollection.Interprete
{
    class Optimizacion
    {
        int regla;
        int iteracion;
        string ambito;
        string codigo;
        string optimizado;

        public Optimizacion(int regla, int iteracion, string ambito, string codigo)
        {
            this.regla = regla;
            this.iteracion = iteracion;
            this.ambito = ambito;
            this.codigo = codigo;
            this.optimizado = "";
        }

        public void SetOptimizado(string codigo)
        {
            this.optimizado = codigo;
        }

        public string[] GetData(int i) 
        {
            string[] data = { i.ToString(), regla.ToString(), codigo, optimizado, ambito + " : " + iteracion.ToString() };
            return data;
                
        }

    }

    class Optimizador
    {

        private ParseTreeNode programa;
        private string ambito;
        private int optimizaciones;
        private int iteraciones;
        private int eliminadas;
        private List<Optimizacion> log;

        public Optimizador()
        {
            programa = null;
            ambito = "";
            optimizaciones = 0;
            iteraciones = 0;
            eliminadas = 0;
            log = new List<Optimizacion>();
        }

        public string GetReporte()
        {
            string reporte = "";
            reporte += String.Format("Optimización: {0}\n", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            reporte += String.Format("\tFueron necesarias {0} iteraciones\n", iteraciones);
            reporte += String.Format("\tpara realizar {0} optimizaciones\n", log.Count);
            reporte += String.Format("\tque eliminaron {0} líneas de código.\n\n", eliminadas);
            return reporte;
        }

        public List<Optimizacion> GetLog()
        {
            return log;
        }

        public void OptimizarC3D()
        {
            programa = GramaticaC3D.AnalizarC3D();
            if (programa == null)
            {
                System.Windows.Forms.MessageBox.Show("Imposible realizar la optimización.", "Optimizando C3D");
                return;
            }
            do
            {
                iteraciones++;
                Optimizar();
            } while (optimizaciones > 0);
            Generador gen = new Generador(true);
            gen.GenerarC3D(programa);
        }

        private void Optimizar()
        {
            optimizaciones = 0;
            for (int i = 0; i < programa.ChildNodes.Count; i++)
            {
                var metodo = programa.ChildNodes[i];
                var cuerpo = metodo.ChildNodes[1];
                this.ambito = metodo.ChildNodes[0].FindTokenAndGetText();
                metodo.ChildNodes[1] = OptimizarMetodo(cuerpo);
                programa.ChildNodes[i] = metodo;
            }
        }

        private ParseTreeNode OptimizarMetodo(ParseTreeNode cuerpo)
        {
            ParseTreeNodeList optimizado = new ParseTreeNodeList();
            int i = 0;
            while (i < cuerpo.ChildNodes.Count)
            {
                int m = 0;
                ParseTreeNodeList mirilla = new ParseTreeNodeList();
                while (m < 20 && i < cuerpo.ChildNodes.Count)
                {
                    mirilla.Add(cuerpo.ChildNodes[i]);
                    i++;
                }
                if (mirilla.Count == 0)
                    break;
                optimizado.AddRange(OptimizarMirilla(mirilla));
            }
            cuerpo.ChildNodes.Clear();
            cuerpo.ChildNodes.AddRange(optimizado);
            return cuerpo;
        }

        private ParseTreeNodeList OptimizarMirilla(ParseTreeNodeList mirilla)
        {
            ParseTreeNodeList mirillaOptimizada = new ParseTreeNodeList();
            foreach(var sentencia in mirilla)
            {
                var opt = Reglas8y12(sentencia);
                if (opt == null)
                    continue;
                opt = Reglas9y13(opt);
                if (opt == null)
                    continue;
                opt = Reglas10y14(opt);
                if (opt == null)
                    continue;
                opt = Reglas11y15(opt);
                if (opt == null)
                    continue;
                opt = Regla16(opt);
                if (opt == null)
                    continue;
                opt = Reglas17y18(opt);
                if (opt == null)
                    continue;
                opt = Regla19(opt);
                if (opt == null)
                    continue;
                mirillaOptimizada.Add(opt);
            }
            return mirillaOptimizada;
        }

        private ParseTreeNode Reglas8y12(ParseTreeNode sentencia)
        {
            Optimizacion nueva = null;
            if(sentencia.ToString() == "suma")
            {
                if(EsIgual(sentencia.ChildNodes[1], 0))
                {
                    if (SonIguales( sentencia.ChildNodes[0], sentencia.ChildNodes[2]))
                    {
                        nueva = new Optimizacion(8, iteraciones, ambito, GetCodigo(sentencia));
                        sentencia = null;
                    }
                    else
                    {
                        nueva = new Optimizacion(12, iteraciones, ambito, GetCodigo(sentencia));
                        sentencia = NuevaAsignacion(sentencia.ChildNodes[0], sentencia.ChildNodes[2]);
                    }
                }
                else if(EsIgual(sentencia.ChildNodes[2], 0))
                {
                    if (SonIguales(sentencia.ChildNodes[0], sentencia.ChildNodes[1]))
                    {
                        nueva = new Optimizacion(8, iteraciones, ambito, GetCodigo(sentencia));
                        sentencia = null;
                    }
                    else
                    {
                        nueva = new Optimizacion(12, iteraciones, ambito, GetCodigo(sentencia));
                        sentencia = NuevaAsignacion(sentencia.ChildNodes[0], sentencia.ChildNodes[1]);
                    }
                }
            }
            return AddOptimizacion(nueva, sentencia);
        }

        private ParseTreeNode Reglas9y13(ParseTreeNode sentencia)
        {
            Optimizacion nueva = null;
            if (sentencia.ToString() == "resta")
            {
                if (EsIgual(sentencia.ChildNodes[2], 0))
                {
                    if (SonIguales(sentencia.ChildNodes[0], sentencia.ChildNodes[1]))
                    {
                        nueva = new Optimizacion(9, iteraciones, ambito, GetCodigo(sentencia));
                        sentencia = null;
                    }
                    else
                    {
                        nueva = new Optimizacion(13, iteraciones, ambito, GetCodigo(sentencia));
                        sentencia = NuevaAsignacion(sentencia.ChildNodes[0], sentencia.ChildNodes[1]);
                    }
                }
            }
            return AddOptimizacion(nueva, sentencia);
        }

        private ParseTreeNode Reglas10y14(ParseTreeNode sentencia)
        {
            Optimizacion nueva = null;
            if (sentencia.ToString() == "multiplicación")
            {
                if (EsIgual(sentencia.ChildNodes[1], 1))
                {
                    if (SonIguales(sentencia.ChildNodes[0], sentencia.ChildNodes[2]))
                    {
                        nueva = new Optimizacion(10, iteraciones, ambito, GetCodigo(sentencia));
                        sentencia = null;
                    }
                    else
                    {
                        nueva = new Optimizacion(14, iteraciones, ambito, GetCodigo(sentencia));
                        sentencia = NuevaAsignacion(sentencia.ChildNodes[0], sentencia.ChildNodes[2]);
                    }
                }
                else if (EsIgual(sentencia.ChildNodes[2], 1))
                {
                    if (SonIguales(sentencia.ChildNodes[0], sentencia.ChildNodes[1]))
                    {
                        nueva = new Optimizacion(10, iteraciones, ambito, GetCodigo(sentencia));
                        sentencia = null;
                    }
                    else
                    {
                        nueva = new Optimizacion(14, iteraciones, ambito, GetCodigo(sentencia));
                        sentencia = NuevaAsignacion(sentencia.ChildNodes[0], sentencia.ChildNodes[1]);
                    }
                }
            }
            return AddOptimizacion(nueva, sentencia);
        }

        private ParseTreeNode Reglas11y15(ParseTreeNode sentencia)
        {
            Optimizacion nueva = null;
            if (sentencia.ToString() == "división")
            {
                if (EsIgual(sentencia.ChildNodes[2], 1))
                {
                    if (SonIguales(sentencia.ChildNodes[0], sentencia.ChildNodes[1]))
                    {
                        nueva = new Optimizacion(11, iteraciones, ambito, GetCodigo(sentencia));
                        sentencia = null;
                    }
                    else
                    {
                        nueva = new Optimizacion(15, iteraciones, ambito, GetCodigo(sentencia));
                        sentencia = NuevaAsignacion(sentencia.ChildNodes[0], sentencia.ChildNodes[1]);
                    }
                }
            }
            return AddOptimizacion(nueva, sentencia);
        }

        private ParseTreeNode Regla16(ParseTreeNode sentencia)
        {
            Optimizacion nueva = null;
            if (sentencia.ToString() == "potencia")
            {
                if (EsIgual(sentencia.ChildNodes[2], 2))
                {
                    nueva = new Optimizacion(16, iteraciones, ambito, GetCodigo(sentencia));
                    sentencia = NuevaOperacion("multiplicación", sentencia.ChildNodes[0], sentencia.ChildNodes[1], sentencia.ChildNodes[1]);
                }
            }
            return AddOptimizacion(nueva, sentencia);
        }

        private ParseTreeNode Reglas17y18(ParseTreeNode sentencia)
        {
            Optimizacion nueva = null;
            if (sentencia.ToString() == "multiplicación")
            {
                if (EsIgual(sentencia.ChildNodes[1], 0))
                {
                    nueva = new Optimizacion(18, iteraciones, ambito, GetCodigo(sentencia));
                    sentencia = NuevaAsignacion(sentencia.ChildNodes[0], sentencia.ChildNodes[1]);
                }
                else if (EsIgual(sentencia.ChildNodes[2], 0))
                {
                    nueva = new Optimizacion(18, iteraciones, ambito, GetCodigo(sentencia));
                    sentencia = NuevaAsignacion(sentencia.ChildNodes[0], sentencia.ChildNodes[2]);
                }
                else if (EsIgual(sentencia.ChildNodes[1], 2))
                {
                    nueva = new Optimizacion(17, iteraciones, ambito, GetCodigo(sentencia));
                    sentencia = NuevaOperacion("suma", sentencia.ChildNodes[0], sentencia.ChildNodes[2], sentencia.ChildNodes[2]);
                }
                else if (EsIgual(sentencia.ChildNodes[2], 2))
                {
                    nueva = new Optimizacion(17, iteraciones, ambito, GetCodigo(sentencia));
                    sentencia = NuevaOperacion("suma", sentencia.ChildNodes[0], sentencia.ChildNodes[1], sentencia.ChildNodes[1]);
                }
            }
            return AddOptimizacion(nueva, sentencia);
        }

        private ParseTreeNode Regla19(ParseTreeNode sentencia)
        {
            Optimizacion nueva = null;
            if (sentencia.ToString() == "división")
            {
                if (EsIgual(sentencia.ChildNodes[1], 0))
                {
                    nueva = new Optimizacion(19, iteraciones, ambito, GetCodigo(sentencia));
                    sentencia = NuevaAsignacion(sentencia.ChildNodes[0], sentencia.ChildNodes[1]);
                }
            }
            return AddOptimizacion(nueva, sentencia);
        }

        private bool EsIgual(ParseTreeNode target, Double valor)
        {
            double val = -1;
            if(target.Term.ToString() == "entero" || target.Term.ToString() == "decimal")
                val = Double.Parse(target.FindTokenAndGetText());
            return val == valor;
        }

        private bool SonIguales(ParseTreeNode uno, ParseTreeNode dos)
        {
            return uno.FindTokenAndGetText() == dos.FindTokenAndGetText();
        }

        private ParseTreeNode AddOptimizacion(Optimizacion nueva, ParseTreeNode sentencia)
        {
            if (nueva != null)
            {
                optimizaciones++;
                nueva.SetOptimizado(GetCodigo(sentencia));
                log.Add(nueva);
            }
            return sentencia;
        }

        private ParseTreeNode NuevaOperacion(string operacion, ParseTreeNode destino, ParseTreeNode izq, ParseTreeNode der)
        {
            var sentencia = new ParseTreeNode(new NonTerminal(operacion), new SourceSpan());
            sentencia.ChildNodes.Add(destino);
            sentencia.ChildNodes.Add(izq);
            sentencia.ChildNodes.Add(der);
            return sentencia;
        }

        private ParseTreeNode NuevaAsignacion(ParseTreeNode destino, ParseTreeNode valor)
        {
            var sentencia = new ParseTreeNode(new NonTerminal("asignación"), new SourceSpan());
            sentencia.ChildNodes.Add(destino);
            sentencia.ChildNodes.Add(valor);
            return sentencia;
        }

        private string GetCodigo(ParseTreeNode nodo)
        {
            if (nodo == null)
            {
                eliminadas++;
                return "//Instrucción eliminada";
            }
            var str = nodo.ToString();
            if (str == "asignación")
                return nodo.ChildNodes[0].FindTokenAndGetText() + " = " + nodo.ChildNodes[1].FindTokenAndGetText() + ";\n";
            string operacion = nodo.ChildNodes[0].FindTokenAndGetText() + " = " + nodo.ChildNodes[1].FindTokenAndGetText() + " {0} " + nodo.ChildNodes[2].FindTokenAndGetText() + ";\n" ;
            switch (nodo.ToString())
            {
                case "suma":
                    return String.Format(operacion, "+");
                case "resta":
                    return String.Format(operacion, "-");
                case "multiplicación":
                    return String.Format(operacion, "*");
                case "división":
                    return String.Format(operacion, "/");
                case "potencia":
                    return String.Format(operacion, "^");
                default:
                    break;
            }
            
            //suma
            //asignación
            //resta
            //null
            return "";
        }

    }
}
