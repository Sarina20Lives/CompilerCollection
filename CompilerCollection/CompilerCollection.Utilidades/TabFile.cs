using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompilerCollection.CompilerCollection.Utilidades
{
    class TabFile : TabPage
    {
        public String Path;

        public TabFile()
            : base()
        {
            Path = null;
            Text = "Sin-guardar*.jc";
            TextBox areaTexto = new TextBox();
            areaTexto.Multiline = true;
            areaTexto.Font = new Font("Courier New", 12);
            areaTexto.Dock = DockStyle.Fill;
            areaTexto.ScrollBars = ScrollBars.Vertical;
            areaTexto.AcceptsTab = true;
            this.Controls.Add(areaTexto);
        }

        public TabFile(String text)
            : base(text)
        {
            this.Path = null;
        }

        public void setPath(String path)
        {
            this.Path = path;
        }

        public string getText()
        {
            return Controls[0].Text;
        }

        public static TabFile abrir()
        { 
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Archivo JCode|*.jc";
            if (ofd.ShowDialog() != DialogResult.OK)
                return null;
            TabFile tab = new TabFile();
            tab.Path = ofd.FileName;
            tab.Controls[0].Text = File.ReadAllText(tab.Path);
            tab.ToolTipText = tab.Path;
            tab.Text = tab.Path.Split('\\').Last();
            return tab;
        }

        public void guardar()
        {
            if (this.Path == null)
            {
                guardarComo();
                return;
            }
            File.WriteAllText(Path, Controls[0].Text);
        }

        public void guardarComo()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Archivo JCode|*.jc";
            sfd.Title = "Guardar archivo";
            if (sfd.ShowDialog() != DialogResult.OK)
                return;
            Path = sfd.FileName;
            File.WriteAllText(Path, Controls[0].Text);
            ToolTipText = Path;
            Text = Path.Split('\\').Last();
        }

    }
}
