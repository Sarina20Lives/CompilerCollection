using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Irony.Ast;
using Irony.Parsing;
using CompilerCollection.CompilerCollection.JCode;
using CompilerCollection.CompilerCollection.Compilador;
using CompilerCollection.CompilerCollection.Interprete;

namespace CompilerCollection
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void bAnalizar_Click(object sender, EventArgs e)
        {
            List<ClaseJCode> clases = ParserJcode.generarAst("ArchivoX", rtbEntrada.Text);
            if (clases==null || clases.Count==0)
            {
                rtbConsola.Text = "La cadena es invalida";
                return;
            }

            //Generación de tabla de símbolos
            TablaSimbolo ts = new TablaSimbolo();
            Padre padre;
            foreach (ClaseJCode clase in clases) {
                padre = Padre.crearDeClase(clase.archivo, clase.clase);
                ts.generar(padre, clase.clase);
            }
            String resultado = ts.generarReporte();

            rtbConsola.Text = "La cadena es valida\n";
            rtbConsola.Text += resultado + "\n";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Interprete interprete = new Interprete();
            interprete.Ejecutar();
        }
    }
}
