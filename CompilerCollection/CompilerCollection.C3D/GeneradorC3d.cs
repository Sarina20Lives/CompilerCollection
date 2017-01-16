using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;
using CompilerCollection.CompilerCollection.Compilador;
using CompilerCollection.CompilerCollection.JCode;
using CompilerCollection.CompilerCollection.Utilidades;
using CompilerCollection.CompilerCollection.General;

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

            if (raiz.ToString().CompareTo(ConstantesJC.LLAMADA) == 0) 
            {
                resolverLLamada(ambito, ctxG, ctxL, raiz);
            }

            if (raiz.ToString().CompareTo(ConstantesJC.ASIGNACION) == 0) 
            {
                resolverAsignacion(ambito, ctxG, ctxL, raiz);
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


        public void resolverAsignacion(Simbolo ambito, Contexto ctxG, Contexto ctxL, ParseTreeNode raiz) {
            C3d.escribirComentario("Asignacion de variable", false);                    
            Expresion expresion = new Expresion(ctxG, ctxL, ambito, false);
            C3d solucion = new C3d();
            ParseTreeNode nodoAsig = raiz.ChildNodes.ElementAt(1);
            if (nodoAsig.ToString().CompareTo(ConstantesJC.ASIGVAR) == 0) {
                if (nodoAsig.ChildNodes.ElementAt(0).ToString().CompareTo(ConstantesJC.EXPRESION) == 0) {
                    solucion = expresion.resolver(nodoAsig.ChildNodes.ElementAt(0));
                    if (solucion == null) {
                        return;
                    }
                    asignarObj(ambito, ctxG, ctxL, raiz.ChildNodes.ElementAt(0), solucion);
                }
            }

        }

        public void asignarObj(Simbolo ambito, Contexto ctxG, Contexto ctxL, ParseTreeNode obj, C3d eCad)
        {
            String nObj = "";
            Simbolo sObj;
            Simbolo sSubObj;
            String nSubObj = "";
            //id
            if (obj.ChildNodes.Count() == 1)
            {
                nObj = obj.ChildNodes.ElementAt(0).FindTokenAndGetText();
                sObj = buscarObj(ctxL, nObj);
                if (sObj == null)
                    sObj = buscarObj(ctxG, nObj);

                //Objeto no encontrado
                if (sObj == null)
                {
                    ManejadorErrores.General("La variable " + nObj + " no existe o no es accesible");
                    return;
                }

                eCad = C3d.castearA(eCad, sObj.tipo, ambito.tamanio, false);
                if (eCad == null) { 
                    ManejadorErrores.General("No se puede realizar la asignación por los tipos");
                    return;
                }

                String temp1, temp2 = "";
                if (sObj.esGlobal) {
                    C3d.escribirComentario("Accediendo al this", false);
                    temp1 = C3d.generarTemp();
                    C3d.escribirOperacion(temp1, "P", "+", "1", false); //Pos del this
                    temp2 = C3d.leerDePila(temp1, false); //Val del this
                    temp1 = C3d.generarTemp();
                    C3d.escribirComentario("Obteniendo posicion y asignando valor", false);
                    C3d.escribirOperacion(temp1, temp2, "+", sObj.pos.ToString(), false); //Sumando la pos del objeto
                    C3d.escribirEnHeap(temp1, eCad.cad, false); //Escribiendo en el heap el valor de eCad 
                    return;
                }

                temp1 = C3d.generarTemp();
                C3d.escribirOperacion(temp1, "P", "+", sObj.pos.ToString(), false); //Pos del objeto dentro del Stack
                temp2 = C3d.generarTemp();
                C3d.escribirEnPila(temp1, eCad.cad, false);
                return;
            }


            if (obj.ChildNodes.ElementAt(0).FindTokenAndGetText().Equals("this", StringComparison.OrdinalIgnoreCase))
            {
                nObj = obj.ChildNodes.ElementAt(1).FindTokenAndGetText();
                sObj = buscarObj(ctxG, nObj);
                //No encontrado
                if (sObj == null)
                {
                    return;
                }
            
                //this.id
                if (obj.ChildNodes.Count() == 2)
                {
                    eCad = C3d.castearA(eCad, sObj.tipo, ambito.tamanio, false);
                    if (eCad == null)
                    {
                        ManejadorErrores.General("No se puede realizar la asignación por los tipos, E.tipo =" + eCad.tipo + " y Obj.tipo=" + sObj.tipo);
                        return;
                    }

                    String temp1, temp2 = "";
                    if (sObj.esGlobal)
                    {
                        temp1 = C3d.generarTemp();
                        C3d.escribirOperacion(temp1, "P", "+", "1", false); //Pos del this
                        temp2 = C3d.leerDePila(temp1, false); //Val del this
                        temp1 = C3d.generarTemp();
                        C3d.escribirOperacion(temp1, temp2, "+", sObj.pos.ToString(), false); //Sumando la pos del objeto
                        C3d.escribirEnHeap(temp1, eCad.cad, false); //Escribiendo en el heap el valor de eCad 
                        return;
                    }

                    temp1 = C3d.generarTemp();
                    C3d.escribirOperacion(temp1, "P", "+", sObj.pos.ToString(), false); //Pos del objeto dentro del Stack
                    temp2 = C3d.generarTemp();
                    C3d.escribirEnPila(temp1, eCad.cad, false);
                    return;                
                }
                
                //this.id[]...
                if (obj.ChildNodes.ElementAt(2).ToString().CompareTo(ConstantesJC.ACCESOS) == 0)
                {
                    if (sObj.esArr)
                        return;
                    return;
                }

                //this.id.id...
                nSubObj = obj.ChildNodes.ElementAt(2).ChildNodes.ElementAt(0).FindTokenAndGetText();
                sSubObj = TablaSimbolo.buscarObj(sObj.tipo, nSubObj);
                return;
            }

            nObj = obj.ChildNodes.ElementAt(0).FindTokenAndGetText();
            sObj = buscarObj(ctxL, nObj);
            if (sObj == null)
            {
                sObj = buscarObj(ctxG, nObj);
                //No se encontro
                if (sObj == null)
                {
                    ManejadorErrores.General("La variable "+nObj+" solicitada no fue encontrada");
                    return;
                }
            }
            //id[][][]
            if (obj.ChildNodes.ElementAt(1).ToString().CompareTo(ConstantesJC.ACCESOS) == 0)
            {
                if (sObj.esArr)
                    return;
                return;
            }
            //id.id...
            nSubObj = obj.ChildNodes.ElementAt(2).ChildNodes.ElementAt(0).FindTokenAndGetText();
            sSubObj = TablaSimbolo.buscarObj(sObj.tipo, nSubObj);
            return;
        }

        public static Simbolo buscarObj(Contexto ctx, String nombre) {
            foreach (Simbolo simbolo in ctx.simbolos) {
                if (simbolo.nombre.CompareTo(nombre) == 0) {
                    return simbolo;
                }
            }
            return null;
        }

        public static String resolverLLamada(Simbolo ambito, Contexto ctxG, Contexto ctxL, ParseTreeNode raiz){
            //mtd() ->Sin parametros
            if (raiz.ChildNodes.Count() == 1) { 
                //Obtener el this
                String temp1, temp2 = "";
                temp1 = C3d.generarTemp();
                C3d.escribirOperacion(temp1, "P", "+", "1", false);
                String tthis = C3d.leerDePila(temp1, false);

                //Cambio de ambito simulado
                temp1 = C3d.generarTemp();
                C3d.escribirOperacion(temp1, "P", "+", ambito.tamanio.ToString(), false);

                //Enviar this actual como parametro en el this del metodo a llamar
                temp2 = C3d.generarTemp();
                C3d.escribirOperacion(temp2, temp1, "+", "1", false);
                C3d.escribirEnPila(temp2, tthis, false);
                
                //Aumentar P
                C3d.aumentarP(ambito.tamanio.ToString(), false);
                C3d.escribir(ambito.padre + "_" + raiz.ChildNodes.ElementAt(0).FindTokenAndGetText() + "_();", false);
                //Guardando el return
                temp1 = C3d.generarTemp();
                C3d.escribirOperacion(temp1, "P", "+", "0", false);
                temp2 = C3d.leerDePila(temp1, false);
                C3d.disminuirP(ambito.tamanio.ToString(), false);
                return temp2;
            }

            //mtd(...) ->Con parametros
            if (raiz.ChildNodes.Count() == 2 && raiz.ChildNodes.ElementAt(1).ToString().CompareTo(ConstantesJC.EXPRESIONES)==0)
            {
                //Obtener el this
                String temp1, temp2 = "";
                temp1 = C3d.generarTemp();
                C3d.escribirOperacion(temp1, "P", "+", "1", false);
                String tthis = C3d.leerDePila(temp1, false);

                //Cambio de ambito simulado
                temp1 = C3d.generarTemp();
                C3d.escribirOperacion(temp1, "P", "+", ambito.tamanio.ToString(), false);

                //Enviar this actual como parametro en el this del metodo a llamar
                temp2 = C3d.generarTemp();
                C3d.escribirOperacion(temp2, temp1, "+", "1", false);
                C3d.escribirEnPila(temp2, tthis, false);
                String lparams = "";
                //Enviar parametros
                ParseTreeNode parametros = raiz.ChildNodes.ElementAt(1);
                C3d valorParam;
                Expresion expresion;
                int cont = 2;
                foreach (ParseTreeNode exp in parametros.ChildNodes)
                {
                    expresion = new Expresion(ctxG, ctxL, ambito, false);
                    valorParam = expresion.resolver(exp);
                    temp2 = C3d.generarTemp();
                    C3d.escribirOperacion(temp2, temp1, "+", cont.ToString(), false);
                    if (valorParam == null || valorParam.tipo == Constantes.ERROR)
                    {
                        C3d.escribirEnPila(temp2, "NULL", false);
                        lparams += "NULL_";
                    }
                    else
                    {
                        C3d.escribirEnPila(temp2, valorParam.cad, false);
                        lparams += Constantes.TIPOS[valorParam.tipo] + "_";
                    }
                    cont++;
                }
 
                //Aumentar P
                C3d.aumentarP(ambito.tamanio.ToString(), false);
                C3d.escribir(ambito.padre + "_" + raiz.ChildNodes.ElementAt(0).FindTokenAndGetText() + "_"+lparams+"();", false);
                //Guardando el return
                temp1 = C3d.generarTemp();
                C3d.escribirOperacion(temp1, "P", "+", "0", false);
                temp2 = C3d.leerDePila(temp1, false);
                C3d.disminuirP(ambito.tamanio.ToString(), false);
                return temp2;
            }
            return "";
        }

    }
}
