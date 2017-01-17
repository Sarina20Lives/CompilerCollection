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
        private const String filtroJC = "Archivo JCode|*.jc", filtroACF = "Archivo ACode|*.acf";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            tabArchivos.TabPages.Add(new TabFile());
            tabArchivos.SelectedIndex = tabArchivos.TabPages.Count - 1;
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            TabFile abierta = TabFile.abrir();
            if (abierta != null)
            {
                tabArchivos.TabPages.Add(abierta);
                tabArchivos.SelectedIndex = tabArchivos.TabPages.Count - 1;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (tabArchivos.SelectedIndex >= 0)
                ((TabFile)tabArchivos.SelectedTab).guardar();
        }

        private void btnGuardarComo_Click(object sender, EventArgs e)
        {
            if (tabArchivos.SelectedIndex >= 0)
                ((TabFile)tabArchivos.SelectedTab).guardarComo();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            int i = tabArchivos.SelectedIndex;
            if (i >= 0)
                tabArchivos.TabPages.RemoveAt(i);
        }

        private void btnGenerarC3D_Click(object sender, EventArgs e)
        {
            if (tabArchivos.SelectedIndex < 0)
            {
                MessageBox.Show("No existe ninguna pestaña para compilar.", "Compilando");
                return;
            }
            reiniciarConsola();
            TabFile tab = (TabFile)tabArchivos.SelectedTab;
            if (tab.Path != null)
            {
                string path = tab.Path;
                ManejadorArchivo.RUTA_IMPORTS = path.Substring(0, path.LastIndexOf("\\") + 1);
            }

            //Iniciar registro de errores
            ManejadorErrores.Iniciar();

            //Iniciar análisis
            List<ClaseJCode> clases = ParserJcode.generarAst(tab.Text, tab.getText());
            if (clases == null || clases.Count == 0)
            {
                logConsola("La cadena de entrada no es válida.");
                return;
            }

            Compilador.repertorioClases = clases;
            logConsola("Cadena de entrada válida.");
            logConsola(Compilador.generarTablaSimbolos());
            if (ManejadorErrores.ExistenErrores()) {
                ManejadorErrores.GenerarReporte();
                logConsola("Existen errores de compilación, por favor vea el reporte.");
                return;
            }

            logConsola("Generando código intermedio...");
            logConsola(Compilador.generarC3d());
            ManejadorArchivo.agregarInit();
            if (ManejadorErrores.ExistenErrores())
            {
                ManejadorErrores.GenerarReporte();
                logConsola("Existen errores, el C3d generado no es legitimo, por favor vea el reporte.");
                return;
            }
            ManejadorErrores.GenerarReporte();
            logConsola("Proceso de compilación terminado.");
        }

        private void reiniciarConsola()
        {
            textConsola.Text = "";
        }

        private void logConsola(string texto)
        {
            textConsola.Text += texto + "\n";
        }

        private void btnOptimizaC3D_Click(object sender, EventArgs e)
        {
            Optimizador opt = new Optimizador();
            opt.OptimizarC3D();
            labelLogOptimizacion.Text = opt.GetReporte();
            int i = 1;
            foreach (var log in opt.GetLog())
                dataLogOptimizacion.Rows.Add(log.GetData(i++));
            tabPrincipal.SelectedIndex = 2;
        }

        private void btnEjecutarC3D_Click(object sender, EventArgs e)
        {
            Interprete interprete = Interprete.ResetInstance();
            interprete.EjecutarC3D();
            textSalida.Text = String.Format("Salida C3D: {0}\r\n\r\n{1}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), interprete.GetSalida().Replace("\n","\r\n"));
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
            textSalida.Text = String.Format("Salida Cuádruplos: {0}\r\n\r\n{1}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), interprete.GetSalida().Replace("\n", "\r\n"));
        }

        private void btnTablaSimbolos_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("C:\\FilesCompilerCollection\\Reportes\\TS.html");
        }

        private void btnLogError_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("C:\\FilesCompilerCollection\\Reportes\\Errores.html");
        }

        private void btnAcercaDe_Click(object sender, EventArgs e)
        {
            
        }


    }
}
