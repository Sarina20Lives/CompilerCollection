namespace CompilerCollection
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.rtbEntrada = new System.Windows.Forms.RichTextBox();
            this.rtbConsola = new System.Windows.Forms.RichTextBox();
            this.toolAcciones = new System.Windows.Forms.ToolStrip();
            this.btnNuevo = new System.Windows.Forms.ToolStripButton();
            this.btnAbrir = new System.Windows.Forms.ToolStripButton();
            this.btnGuardar = new System.Windows.Forms.ToolStripButton();
            this.btnGuardarComo = new System.Windows.Forms.ToolStripButton();
            this.btnCerrar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnGenerarC3D = new System.Windows.Forms.ToolStripButton();
            this.btnOptimizaC3D = new System.Windows.Forms.ToolStripButton();
            this.btnEjecutarC3D = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnGenerar4P = new System.Windows.Forms.ToolStripButton();
            this.btnEjecutar4P = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnTablaSimbolos = new System.Windows.Forms.ToolStripButton();
            this.btnLogError = new System.Windows.Forms.ToolStripButton();
            this.btnAcercaDe = new System.Windows.Forms.ToolStripButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dataLogOptimizacion = new System.Windows.Forms.DataGridView();
            this.labelLogOptimizacion = new System.Windows.Forms.Label();
            this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Regla = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Codigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Optimizado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ambito = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textSalida = new System.Windows.Forms.TextBox();
            this.toolAcciones.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataLogOptimizacion)).BeginInit();
            this.SuspendLayout();
            // 
            // rtbEntrada
            // 
            this.rtbEntrada.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbEntrada.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbEntrada.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbEntrada.Location = new System.Drawing.Point(3, 3);
            this.rtbEntrada.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.rtbEntrada.Name = "rtbEntrada";
            this.rtbEntrada.Size = new System.Drawing.Size(886, 244);
            this.rtbEntrada.TabIndex = 0;
            this.rtbEntrada.Text = "";
            // 
            // rtbConsola
            // 
            this.rtbConsola.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rtbConsola.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbConsola.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtbConsola.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbConsola.Location = new System.Drawing.Point(3, 247);
            this.rtbConsola.Name = "rtbConsola";
            this.rtbConsola.ReadOnly = true;
            this.rtbConsola.Size = new System.Drawing.Size(886, 141);
            this.rtbConsola.TabIndex = 1;
            this.rtbConsola.TabStop = false;
            this.rtbConsola.Text = "";
            // 
            // toolAcciones
            // 
            this.toolAcciones.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolAcciones.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNuevo,
            this.btnAbrir,
            this.btnGuardar,
            this.btnGuardarComo,
            this.btnCerrar,
            this.toolStripSeparator1,
            this.btnGenerarC3D,
            this.btnOptimizaC3D,
            this.btnEjecutarC3D,
            this.toolStripSeparator2,
            this.btnGenerar4P,
            this.btnEjecutar4P,
            this.toolStripSeparator3,
            this.btnTablaSimbolos,
            this.btnLogError,
            this.btnAcercaDe});
            this.toolAcciones.Location = new System.Drawing.Point(0, 0);
            this.toolAcciones.Name = "toolAcciones";
            this.toolAcciones.Size = new System.Drawing.Size(900, 52);
            this.toolAcciones.TabIndex = 10;
            this.toolAcciones.Text = "toolStrip1";
            // 
            // btnNuevo
            // 
            this.btnNuevo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNuevo.Image = global::CompilerCollection.Properties.Resources.icono_nuevo;
            this.btnNuevo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnNuevo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(44, 49);
            this.btnNuevo.Text = "toolStripButton1";
            this.btnNuevo.ToolTipText = "Nuevo archivo";
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            // 
            // btnAbrir
            // 
            this.btnAbrir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAbrir.Image = global::CompilerCollection.Properties.Resources.icono_folder_abrir;
            this.btnAbrir.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnAbrir.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAbrir.Name = "btnAbrir";
            this.btnAbrir.Size = new System.Drawing.Size(49, 49);
            this.btnAbrir.Text = "toolStripButton2";
            this.btnAbrir.ToolTipText = "Abrir archivo";
            this.btnAbrir.Click += new System.EventHandler(this.btnAbrir_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnGuardar.Image = global::CompilerCollection.Properties.Resources.icono_guardar;
            this.btnGuardar.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnGuardar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(44, 49);
            this.btnGuardar.Text = "toolStripButton3";
            this.btnGuardar.ToolTipText = "Guardar archivo";
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnGuardarComo
            // 
            this.btnGuardarComo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnGuardarComo.Image = global::CompilerCollection.Properties.Resources.icono_guardarComo;
            this.btnGuardarComo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnGuardarComo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGuardarComo.Name = "btnGuardarComo";
            this.btnGuardarComo.Size = new System.Drawing.Size(44, 49);
            this.btnGuardarComo.Text = "toolStripButton4";
            this.btnGuardarComo.ToolTipText = "Guardar archivo como...";
            this.btnGuardarComo.Click += new System.EventHandler(this.btnGuardarComo_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCerrar.Image = global::CompilerCollection.Properties.Resources.icono_eliminar;
            this.btnCerrar.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnCerrar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(44, 49);
            this.btnCerrar.Text = "toolStripButton12";
            this.btnCerrar.ToolTipText = "Cerrar archivo";
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 52);
            // 
            // btnGenerarC3D
            // 
            this.btnGenerarC3D.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnGenerarC3D.Image = global::CompilerCollection.Properties.Resources.icono_3D_generar;
            this.btnGenerarC3D.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnGenerarC3D.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGenerarC3D.Name = "btnGenerarC3D";
            this.btnGenerarC3D.Size = new System.Drawing.Size(44, 49);
            this.btnGenerarC3D.Text = "toolStripButton5";
            this.btnGenerarC3D.ToolTipText = "Generar código de tres direcciones";
            this.btnGenerarC3D.Click += new System.EventHandler(this.btnGenerarC3D_Click);
            // 
            // btnOptimizaC3D
            // 
            this.btnOptimizaC3D.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOptimizaC3D.Image = global::CompilerCollection.Properties.Resources.icono_3D_optimizar;
            this.btnOptimizaC3D.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnOptimizaC3D.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOptimizaC3D.Name = "btnOptimizaC3D";
            this.btnOptimizaC3D.Size = new System.Drawing.Size(44, 49);
            this.btnOptimizaC3D.Text = "toolStripButton6";
            this.btnOptimizaC3D.ToolTipText = "Optimizar código de tres direcciones";
            this.btnOptimizaC3D.Click += new System.EventHandler(this.btnOptimizaC3D_Click);
            // 
            // btnEjecutarC3D
            // 
            this.btnEjecutarC3D.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEjecutarC3D.Image = global::CompilerCollection.Properties.Resources.icono_3D_ejecutar;
            this.btnEjecutarC3D.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnEjecutarC3D.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEjecutarC3D.Name = "btnEjecutarC3D";
            this.btnEjecutarC3D.Size = new System.Drawing.Size(44, 49);
            this.btnEjecutarC3D.Text = "toolStripButton7";
            this.btnEjecutarC3D.ToolTipText = "Ejecutar código de tres direcciones";
            this.btnEjecutarC3D.Click += new System.EventHandler(this.btnEjecutarC3D_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 52);
            // 
            // btnGenerar4P
            // 
            this.btnGenerar4P.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnGenerar4P.Image = global::CompilerCollection.Properties.Resources.icono_4P_generar;
            this.btnGenerar4P.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnGenerar4P.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGenerar4P.Name = "btnGenerar4P";
            this.btnGenerar4P.Size = new System.Drawing.Size(44, 49);
            this.btnGenerar4P.Text = "toolStripButton8";
            this.btnGenerar4P.ToolTipText = "Generar cuádruplos";
            this.btnGenerar4P.Click += new System.EventHandler(this.btnGenerar4P_Click);
            // 
            // btnEjecutar4P
            // 
            this.btnEjecutar4P.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEjecutar4P.Image = global::CompilerCollection.Properties.Resources.icono_4P_ejecutar;
            this.btnEjecutar4P.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnEjecutar4P.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEjecutar4P.Name = "btnEjecutar4P";
            this.btnEjecutar4P.Size = new System.Drawing.Size(44, 49);
            this.btnEjecutar4P.Text = "toolStripButton9";
            this.btnEjecutar4P.ToolTipText = "Ejecutar cuádruplos";
            this.btnEjecutar4P.Click += new System.EventHandler(this.btnEjecutar4P_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 52);
            // 
            // btnTablaSimbolos
            // 
            this.btnTablaSimbolos.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnTablaSimbolos.Image = global::CompilerCollection.Properties.Resources.icono_tabla;
            this.btnTablaSimbolos.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnTablaSimbolos.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTablaSimbolos.Name = "btnTablaSimbolos";
            this.btnTablaSimbolos.Size = new System.Drawing.Size(44, 49);
            this.btnTablaSimbolos.Text = "toolStripButton10";
            this.btnTablaSimbolos.ToolTipText = "Abrir tabla de símbolos";
            this.btnTablaSimbolos.Click += new System.EventHandler(this.btnTablaSimbolos_Click);
            // 
            // btnLogError
            // 
            this.btnLogError.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLogError.Image = global::CompilerCollection.Properties.Resources.icono_logerror;
            this.btnLogError.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnLogError.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLogError.Name = "btnLogError";
            this.btnLogError.Size = new System.Drawing.Size(44, 49);
            this.btnLogError.Text = "toolStripButton11";
            this.btnLogError.ToolTipText = "Log de errores";
            this.btnLogError.Click += new System.EventHandler(this.btnLogError_Click);
            // 
            // btnAcercaDe
            // 
            this.btnAcercaDe.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAcercaDe.Image = global::CompilerCollection.Properties.Resources.miniSOL;
            this.btnAcercaDe.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnAcercaDe.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAcercaDe.Name = "btnAcercaDe";
            this.btnAcercaDe.Size = new System.Drawing.Size(40, 49);
            this.btnAcercaDe.Text = "toolStripButton13";
            this.btnAcercaDe.ToolTipText = "SOL";
            this.btnAcercaDe.Click += new System.EventHandler(this.btnAcercaDe_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(0, 55);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(900, 417);
            this.tabControl1.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage1.Controls.Add(this.rtbEntrada);
            this.tabPage1.Controls.Add(this.rtbConsola);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(892, 391);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Área de edición";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textSalida);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(892, 391);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Consola";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dataLogOptimizacion);
            this.tabPage3.Controls.Add(this.labelLogOptimizacion);
            this.tabPage3.Location = new System.Drawing.Point(4, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(892, 391);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Log optimización";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dataLogOptimizacion
            // 
            this.dataLogOptimizacion.AllowUserToAddRows = false;
            this.dataLogOptimizacion.AllowUserToDeleteRows = false;
            this.dataLogOptimizacion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataLogOptimizacion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.No,
            this.Regla,
            this.Codigo,
            this.Optimizado,
            this.Ambito});
            this.dataLogOptimizacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLogOptimizacion.Location = new System.Drawing.Point(3, 39);
            this.dataLogOptimizacion.Name = "dataLogOptimizacion";
            this.dataLogOptimizacion.ReadOnly = true;
            this.dataLogOptimizacion.Size = new System.Drawing.Size(886, 349);
            this.dataLogOptimizacion.TabIndex = 1;
            // 
            // labelLogOptimizacion
            // 
            this.labelLogOptimizacion.AutoSize = true;
            this.labelLogOptimizacion.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelLogOptimizacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLogOptimizacion.Location = new System.Drawing.Point(3, 3);
            this.labelLogOptimizacion.Name = "labelLogOptimizacion";
            this.labelLogOptimizacion.Size = new System.Drawing.Size(245, 36);
            this.labelLogOptimizacion.TabIndex = 0;
            this.labelLogOptimizacion.Text = "No hay optimizaciones por mostrar.\r\n\r\n";
            // 
            // No
            // 
            this.No.Frozen = true;
            this.No.HeaderText = "No.";
            this.No.Name = "No";
            this.No.ReadOnly = true;
            // 
            // Regla
            // 
            this.Regla.Frozen = true;
            this.Regla.HeaderText = "Regla";
            this.Regla.Name = "Regla";
            this.Regla.ReadOnly = true;
            // 
            // Codigo
            // 
            this.Codigo.HeaderText = "Código original";
            this.Codigo.Name = "Codigo";
            this.Codigo.ReadOnly = true;
            this.Codigo.Width = 120;
            // 
            // Optimizado
            // 
            this.Optimizado.HeaderText = "Código optimizado";
            this.Optimizado.Name = "Optimizado";
            this.Optimizado.ReadOnly = true;
            this.Optimizado.Width = 120;
            // 
            // Ambito
            // 
            this.Ambito.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Ambito.HeaderText = "Ámbito (método)";
            this.Ambito.Name = "Ambito";
            this.Ambito.ReadOnly = true;
            // 
            // textSalida
            // 
            this.textSalida.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textSalida.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textSalida.Location = new System.Drawing.Point(3, 3);
            this.textSalida.Multiline = true;
            this.textSalida.Name = "textSalida";
            this.textSalida.Size = new System.Drawing.Size(886, 385);
            this.textSalida.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(900, 484);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolAcciones);
            this.Name = "Form1";
            this.Text = "Compiler Collection - By Sarina20Lives";
            this.toolAcciones.ResumeLayout(false);
            this.toolAcciones.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataLogOptimizacion)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbEntrada;
        private System.Windows.Forms.RichTextBox rtbConsola;
        private System.Windows.Forms.ToolStrip toolAcciones;
        private System.Windows.Forms.ToolStripButton btnNuevo;
        private System.Windows.Forms.ToolStripButton btnAbrir;
        private System.Windows.Forms.ToolStripButton btnGuardar;
        private System.Windows.Forms.ToolStripButton btnGuardarComo;
        private System.Windows.Forms.ToolStripButton btnCerrar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnGenerarC3D;
        private System.Windows.Forms.ToolStripButton btnOptimizaC3D;
        private System.Windows.Forms.ToolStripButton btnEjecutarC3D;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnGenerar4P;
        private System.Windows.Forms.ToolStripButton btnEjecutar4P;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnTablaSimbolos;
        private System.Windows.Forms.ToolStripButton btnLogError;
        private System.Windows.Forms.ToolStripButton btnAcercaDe;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dataLogOptimizacion;
        private System.Windows.Forms.Label labelLogOptimizacion;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn Regla;
        private System.Windows.Forms.DataGridViewTextBoxColumn Codigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Optimizado;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ambito;
        private System.Windows.Forms.TextBox textSalida;
    }
}

