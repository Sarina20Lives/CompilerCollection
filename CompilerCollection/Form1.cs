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
using CompilerCollection.CompilerCollection.Interprete;
using CompilerCollection.CompilerCollection.C3D;

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
            //Iniciar registro de errores
            ManejadorErrores.Iniciar();
            
            //Iniciar análisis
            List<ClaseJCode> clases = ParserJcode.generarAst("ArchivoX", rtbEntrada.Text);
            if (clases == null || clases.Count == 0)
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
            rtbConsola.Text += Compilador.generarC3d()+"\n";
            ManejadorArchivo.agregarInit();
            if (ManejadorErrores.ExistenErrores())
            {
                ManejadorErrores.GenerarReporte();
                rtbConsola.Text += "Existen errores, el C3d generado no es legitimo, ver reporte... \n";
                return;
            }
        }

        private void btnOptimizaC3D_Click(object sender, EventArgs e)
        {
            Optimizador opt = new Optimizador();
            opt.OptimizarC3D();
            labelLogOptimizacion.Text = opt.GetReporte();
            int i = 1;
            foreach (var log in opt.GetLog())
            {
                dataLogOptimizacion.Rows.Add(log.GetData(i++));
            }
            tabControl1.SelectedIndex = 2;
        }

        private void btnEjecutarC3D_Click(object sender, EventArgs e)
        {
            Interprete interprete = Interprete.ResetInstance();
            interprete.EjecutarC3D();
            textSalida.Text = String.Format("Salida generada: {0}\r\n\r\n{1}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), interprete.GetSalida().Replace("\n","\r\n"));
        }

        private void btnGenerar4P_Click(object sender, EventArgs e)
        {
            Generador gen = new Generador(true);
            gen.GenerarCuadruplos();
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
