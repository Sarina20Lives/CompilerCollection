using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerCollection.CompilerCollection.Interprete
{
    class Optimizador
    {

        private ParseTreeNode programa;

        public Optimizador()
        {
            programa = null;
        }

        public void OptimizarC3D()
        {
            programa = GramaticaC3D.AnalizarC3D();
            if (programa == null)
            {
                System.Windows.Forms.MessageBox.Show("Imposible realizar la optimización.", "Optimizando C3D");
                return;
            }
            Optimizar();
        }

        public void OptimizarC4P()
        {
            programa = GramaticaC4P.AnalizarC4P();
            if (programa == null)
            {
                System.Windows.Forms.MessageBox.Show("Imposible realizar la optimización.", "Optimizando Cuádruplos");
                return;
            }
            Optimizar();
        }

        private void Optimizar()
        {

        }
    }
}
