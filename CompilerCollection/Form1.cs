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

        private void btnNuevo_Click(object sender, EventArgs e)
        {

        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

        }

        private void btnGuardarComo_Click(object sender, EventArgs e)
        {

        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {

        }

        private void btnGenerarC3D_Click(object sender, EventArgs e)
        {
            List<ClaseJCode> clases = ParserJcode.generarAst("ArchivoX", rtbEntrada.Text);
            if (clases == null || clases.Count == 0)
            {
                rtbConsola.Text = "La cadena es invalida";
                return;
            }
            //Generación de tabla de símbolos
            TablaSimbolo tablaSimbolos = new TablaSimbolo();
            Padre padre;
            foreach (ClaseJCode clase in clases)
            {
                padre = Padre.crearDeClase(clase.archivo, clase.clase);
                tablaSimbolos.generar(padre, clase.clase);
            }
            String resultado = tablaSimbolos.generarReporte();
            rtbConsola.Text = "La cadena es válida\n";
            rtbConsola.Text += resultado + "\n";
        }

        private void btnOptimizaC3D_Click(object sender, EventArgs e)
        {

        }

        private void btnEjecutarC3D_Click(object sender, EventArgs e)
        {
            Interprete interprete = Interprete.ResetInstance();
            interprete.EjecutarC3D();
        }

        private void btnGenerar4P_Click(object sender, EventArgs e)
        {
            Generador gen = new Generador(true);
            gen.GenerarC3D();
        }

        private void btnEjecutar4P_Click(object sender, EventArgs e)
        {
            Interprete interprete = Interprete.ResetInstance();
            interprete.EjecutarC4P();
        }

        private void btnTablaSimbolos_Click(object sender, EventArgs e)
        {

        }

        private void btnLogError_Click(object sender, EventArgs e)
        {

        }

        private void btnAcercaDe_Click(object sender, EventArgs e)
        {

        }

    }
}
