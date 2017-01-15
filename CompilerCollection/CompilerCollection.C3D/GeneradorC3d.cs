using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;
using CompilerCollection.CompilerCollection.Compilador;
using CompilerCollection.CompilerCollection.JCode;

namespace CompilerCollection.CompilerCollection.C3D
{
    class GeneradorC3d
    {
        public static String init = "";

        public void iniciar()
        {
            init = "";
            Utilidades.ManejadorArchivo.iniciarC3d();
        }

        public void limpiarInit() {
            init = "";
        }

        public void generar(Simbolo ambito, Contexto ctxG, Contexto ctxL, ParseTreeNode raiz)
        {
            if (raiz.ToString().CompareTo(ConstantesJC.CLASE) == 0)
            {
                //Escribir clase_init
                iniciarInit(ambito.nombre, ambito.tamanio);
            }

            if (raiz.ToString().CompareTo(ConstantesJC.CONSTRUCTOR) == 0)
            {
                //Programar Cambio de Ambito
                Padre padre = Padre.crear(ambito);
                ambito = TablaSimbolo.getSimbolo( Simbolo.resolverConstructor(padre, raiz));
                if (ambito == null)
                {
                    ambito = Simbolo.resolverMetodo(padre, raiz);
                    ambito = TablaSimbolo.obtenerTipoOverride(ambito, padre.clase);
                    ambito = TablaSimbolo.getSimbolo(ambito);
                }
                if (ambito.rol.Equals(ConstantesJC.CONSTRUCTOR, StringComparison.OrdinalIgnoreCase))
                {
                    //Iniciar Constructor
                    iniciarConstructor(ambito.padre, ambito.referencia, ambito.tamanio);
                    //Generar el ctx local si tiene parametros
                    ctxL = Contexto.generarContextoLocal(ambito, ParserJcode.obtenerParametros(raiz));
                }else{
                    //Escribir encabezado de método
                    C3d.escribir("Void " + ambito.referencia + "(){");
                    //Generar el ctx local si tiene parametros
                    ctxL = Contexto.generarContextoLocal(ambito, ParserJcode.obtenerParametros(raiz));
                }
            }

            if (raiz.ToString().CompareTo(ConstantesJC.PRINCIPAL) == 0)
            {
                //Programar Cambio de Ambito
                Padre padre = Padre.crear(ambito);
                ambito = TablaSimbolo.getSimbolo(Simbolo.resolverPrincipal(padre, raiz));
                C3d.escribir("Void " + ambito.referencia + "(){");
            }

            if (raiz.ToString().CompareTo(ConstantesJC.METODO) == 0)
            {
                //Programar Cambio de Ambito
                Padre padre = Padre.crear(ambito);
                ambito = TablaSimbolo.getSimbolo(Simbolo.resolverMetodo(padre, raiz));
                C3d.escribir("Void " + ambito.referencia + "(){");
                //Generar el ctx local si tiene parametros
                ctxL = Contexto.generarContextoLocal(ambito, ParserJcode.obtenerParametros(raiz));
            }

            if (raiz.ToString().CompareTo(ConstantesJC.DECLOCAL) == 0)
            {
                ctxL.agregarAlContextoLocal(ambito, raiz);
            }

            if (raiz.ChildNodes.Count <= 0)
            {
                return;
            }

            foreach (ParseTreeNode hijo in raiz.ChildNodes)
            {
                if(hijo.ToString().CompareTo(ConstantesJC.DECGLOBAL)==0){
                    continue;
                }
                generar(ambito, ctxG, ctxL, hijo);
            }

            if (raiz.ToString().CompareTo(ConstantesJC.CLASE) == 0) {
                //Escribir el init
                C3d.escribir(init);
                C3d.escribir("}\n");
            }
           
            if (raiz.ToString().CompareTo(ConstantesJC.CONSTRUCTOR) == 0 ||
                raiz.ToString().CompareTo(ConstantesJC.PRINCIPAL) == 0 ||
                raiz.ToString().CompareTo(ConstantesJC.METODO) == 0)
            {
                //Colocar el fin del bloque
                C3d.escribir("}\n");
            }
        }

        public void iniciarInit(String nombre, int tamanio)
        {
            init = "Void " + nombre + "_" + "$init(){\n";
            //Creando el this
            String tthis = C3d.generarTemp();
            init += tthis + " = H;\n";
            init += "H = H + " + tamanio.ToString()+ ";\n";
            //Guardando el return
            String temp = C3d.generarTemp();
            init += temp + " = P + 0;\n";
            init += "Stack[" + temp + "] = " + tthis + ";\n";
            //Guardando el this
            temp = C3d.generarTemp();
            init += temp + " = P + 1;\n";
            init += "Stack[" + temp + "] = " + tthis + ";";
        }


        public void iniciarConstructor(String clase, String referencia, int tamanio) 
        {
            String t1 = C3d.generarTemp();
            //Iniciar el constructor
            C3d.escribir(referencia + "{");
            C3d.aumentarP(tamanio.ToString());
            C3d.escribir(clase + "_$init();");
            //Obteniendo el return de init
            C3d.escribirOperacion(t1, "P", "+", "0");
            String t2 = C3d.leerDePila(t1);
            C3d.disminuirP(tamanio.ToString());
            //Guardando el return
            String t3 = C3d.generarTemp();
            C3d.escribirOperacion(t3, "P", "+", "0");
            C3d.escribirEnPila(t3, t2);
            //Guardando el this
            t3 = C3d.generarTemp();
            C3d.escribirOperacion(t3, "P", "+", "1");
            C3d.escribirEnPila(t3, t2);
        }


    }
}
