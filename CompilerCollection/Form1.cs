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
using CompilerCollection.CompilerCollection.Utilidades;
using CompilerCollection.CompilerCollection.C3D;

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
            //Iniciar registro de errores
            ManejadorErrores.Iniciar();
            
            //Iniciar análisis
            List<ClaseJCode> clases = ParserJcode.generarAst("ArchivoX", rtbEntrada.Text);
            if (clases==null || clases.Count==0)
            {
                rtbConsola.Text = "La cadena es invalida\n";
                return;
            }

            Compilador.repertorioClases = clases;
            rtbConsola.Text = "La cadena es valida\n";
            rtbConsola.Text += Compilador.generarTablaSimbolos() + "\n";
            if (ManejadorErrores.ExistenErrores()) {
                ManejadorErrores.GenerarReporte();
                rtbConsola.Text += "Existen errores, ver reporte... \n";
                return;
            }
            rtbConsola.Text += Compilador.generarC3d() + "\n";
           





        }
    }
}
