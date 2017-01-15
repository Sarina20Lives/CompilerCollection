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
        public void iniciar()
        {
            Utilidades.ManejadorArchivo.iniciarC3d();
        }


        public void generar(Simbolo ambito, Contexto ctxG, Contexto ctxL, ParseTreeNode raiz, bool esInit)
        {
            if (raiz.ToString().CompareTo(ConstantesJC.CONSTRUCTOR) == 0)
            {
                esInit = false;
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
                    C3d.escribir("void " + ambito.referencia + "(){", esInit);
                    //Generar el ctx local si tiene parametros
                    ctxL = Contexto.generarContextoLocal(ambito, ParserJcode.obtenerParametros(raiz));
                }
            }

            if (raiz.ToString().CompareTo(ConstantesJC.PRINCIPAL) == 0)
            {
                esInit = false;
                //Programar Cambio de Ambito
                Padre padre = Padre.crear(ambito);
                ambito = TablaSimbolo.getSimbolo(Simbolo.resolverPrincipal(padre, raiz));
                C3d.escribir("void main(){", esInit);
            }

            if (raiz.ToString().CompareTo(ConstantesJC.METODO) == 0)
            {
                esInit = false;
                //Programar Cambio de Ambito
                Padre padre = Padre.crear(ambito);
                ambito = TablaSimbolo.getSimbolo(Simbolo.resolverMetodo(padre, raiz));
                C3d.escribir("void " + ambito.referencia + "(){", esInit);
                //Generar el ctx local si tiene parametros
                ctxL = Contexto.generarContextoLocal(ambito, ParserJcode.obtenerParametros(raiz));
            }

            if (raiz.ToString().CompareTo(ConstantesJC.DECLOCAL) == 0)
            {
                ctxL.agregarAlContextoLocal(ambito, raiz, ctxG, esInit);
            }

            if (raiz.ChildNodes.Count <= 0)
            {
                return;
            }

            foreach (ParseTreeNode hijo in raiz.ChildNodes)
            {
                if(hijo.ToString().CompareTo(ConstantesJC.DECGLOBAL)==0){
                    Simbolo var = TablaSimbolo.getSimbolo(Simbolo.resolverDeclaracion(Padre.crear(ambito), hijo, true));
                    Contexto.iniciarObj(ambito, ctxG, ctxL, var, hijo, true, true);
                    continue;
                }
                generar(ambito, ctxG, ctxL, hijo, esInit);
            }

            if (raiz.ToString().CompareTo(ConstantesJC.CLASE) == 0 ||
                raiz.ToString().CompareTo(ConstantesJC.CONSTRUCTOR) == 0 ||
                raiz.ToString().CompareTo(ConstantesJC.PRINCIPAL) == 0 ||
                raiz.ToString().CompareTo(ConstantesJC.METODO) == 0)
            {
                //Colocar el fin del bloque
                C3d.escribir("}\n", esInit);
            }
        }

        public void iniciarInit(String nombre, int tamanio)
        {
            C3d.escribir("void " + nombre + "_" + "$init(){", true);
            //Creando el this
            String tthis = C3d.generarTemp();
            C3d.escribirAsignacion(tthis, "H", true);
            C3d.aumentarH(tamanio.ToString(), true);
            //Guardando el return
            String temp = C3d.generarTemp();
            C3d.escribirOperacion(temp, "P", "+", "0", true);
            C3d.escribirEnPila(temp, tthis, true);
            //Guardando el this
            temp = C3d.generarTemp();
            C3d.escribirOperacion(temp, "P", "+", "1", true);
            C3d.escribirEnPila(temp, tthis, true);
        }

        public void iniciarConstructor(String clase, String referencia, int tamanio) 
        {
            String t1 = C3d.generarTemp();
            //Iniciar el constructor
            C3d.escribir(referencia + "{", false);
            C3d.aumentarP(tamanio.ToString(), false);
            C3d.escribir(clase + "_$init();", false);
            //Obteniendo el return de init
            C3d.escribirOperacion(t1, "P", "+", "0", false);
            String t2 = C3d.leerDePila(t1, false);
            C3d.disminuirP(tamanio.ToString(), false);
            //Guardando el return
            String t3 = C3d.generarTemp();
            C3d.escribirOperacion(t3, "P", "+", "0", false);
            C3d.escribirEnPila(t3, t2, false);
            //Guardando el this
            t3 = C3d.generarTemp();
            C3d.escribirOperacion(t3, "P", "+", "1", false);
            C3d.escribirEnPila(t3, t2, false);
        }


        public void resolverObj(Simbolo ambito, Contexto ctxG, Contexto ctxL, ParseTreeNode obj) { 
        

        }

    }
}
