using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace CompilerCollection.CompilerCollection.Compilador
{
    class Dimension
    {
        private int inferior = 0;
        private int superior = 0;
        private int capacidad = 0;


        public static Dimension crear(int cont) {
            Dimension dimension = new Dimension();
            dimension.superior = cont;
            dimension.capacidad = dimension.superior - dimension.inferior;
            return dimension;
        }
        
        /**
         * Crear una dimensión a partir de un ParseTreeNode que contenga la dimensión
         */ 
        public static Dimension crear(ParseTreeNode dimension){
           int inf = -1;
           int sup = -1;
           switch(dimension.ChildNodes.Count()){
               case 1:
                   inf = 0;
                   sup = Convert.ToInt32(dimension.ChildNodes.ElementAt(0).FindTokenAndGetText());
                   break;
               case 2:
                   inf = Convert.ToInt32(dimension.ChildNodes.ElementAt(0).FindTokenAndGetText());
                   sup = Convert.ToInt32(dimension.ChildNodes.ElementAt(1).FindTokenAndGetText());
                   break;
               default:
                   //TODO:ERROR-EN LA DEFINICIÓN DE DIMENSION
                   return null;
           }

            if(inf < 0 || sup < 0 ){
                //TODO:ERROR-EN LA DEFINICIÓN DE DIMENSION
                return null;
            }

            if (sup <= inf) {
                //TODO:ERROR-EN LA DEFINICIÓN DE DIMENSION
                return null;
            }

            Dimension dim = new Dimension();
            dim.inferior = inf;
            dim.superior = sup;
            dim.capacidad = sup - inf;
            return dim;
        }
    }
}
