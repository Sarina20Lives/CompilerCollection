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
            this.bAnalizar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtbEntrada
            // 
            this.rtbEntrada.Location = new System.Drawing.Point(12, 13);
            this.rtbEntrada.Name = "rtbEntrada";
            this.rtbEntrada.Size = new System.Drawing.Size(545, 356);
            this.rtbEntrada.TabIndex = 0;
            this.rtbEntrada.Text = "";
            // 
            // rtbConsola
            // 
            this.rtbConsola.Location = new System.Drawing.Point(-2, 385);
            this.rtbConsola.Name = "rtbConsola";
            this.rtbConsola.Size = new System.Drawing.Size(694, 61);
            this.rtbConsola.TabIndex = 1;
            this.rtbConsola.Text = "";
            // 
            // bAnalizar
            // 
            this.bAnalizar.Location = new System.Drawing.Point(563, 13);
            this.bAnalizar.Name = "bAnalizar";
            this.bAnalizar.Size = new System.Drawing.Size(115, 23);
            this.bAnalizar.TabIndex = 2;
            this.bAnalizar.Text = "Analizar";
            this.bAnalizar.UseVisualStyleBackColor = true;
            this.bAnalizar.Click += new System.EventHandler(this.bAnalizar_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 445);
            this.Controls.Add(this.bAnalizar);
            this.Controls.Add(this.rtbConsola);
            this.Controls.Add(this.rtbEntrada);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbEntrada;
        private System.Windows.Forms.RichTextBox rtbConsola;
        private System.Windows.Forms.Button bAnalizar;
    }
}

