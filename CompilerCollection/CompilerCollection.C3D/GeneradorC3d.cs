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

        public static String final = "";
        public void iniciar()
        {
            final = "";
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
                    final = C3d.generarEtq();
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
                final = C3d.generarEtq();
            }

            if (raiz.ToString().CompareTo(ConstantesJC.METODO) == 0)
            {
                esInit = false;
                //Programar Cambio de Ambito
                Padre padre = Padre.crear(ambito);
                ambito = TablaSimbolo.getSimbolo(Simbolo.resolverMetodo(padre, raiz));
                C3d.escribir("void " + ambito.referencia + "(){", esInit);
                final = C3d.generarEtq();
                //Generar el ctx local si tiene parametros
                ctxL = Contexto.generarContextoLocal(ambito, ParserJcode.obtenerParametros(raiz));
            }

            if (raiz.ToString().CompareTo(ConstantesJC.DECLOCAL) == 0)
            {
                ctxL.agregarAlContextoLocal(ambito, raiz, ctxG, esInit);
                return;
            }

            if (raiz.ToString().CompareTo(ConstantesJC.ASIGNACION) == 0)
            {
                resolverAsignacion(ambito, ctxG, ctxL, raiz);
                return;
            }

            if (raiz.ToString().CompareTo(ConstantesJC.LLAMADA) == 0) 
            {
                resolverLLamada(ambito, ctxG, ctxL, raiz);
                return;
            }

            if (raiz.ToString().CompareTo(ConstantesJC.RETORNO) == 0)
            {
                resolverRetorno(ambito, ctxG, ctxL, raiz, esInit);
                return;
            }

            if (raiz.ToString().CompareTo(ConstantesJC.NOSQL) == 0)
            {
                resolverNoSql(ambito, ctxG, ctxL, raiz, esInit);
                return;
            }

            if (raiz.ToString().CompareTo(ConstantesJC.IF) == 0)
            {
                resolverIf(ambito, ctxG, ctxL, raiz);
                return;
            }

            if (raiz.ToString().CompareTo(ConstantesJC.IFNOT) == 0)
            {
                resolverIfNot(ambito, ctxG, ctxL, raiz);
                return;
            }

            if (raiz.ToString().CompareTo(ConstantesJC.WHILE) == 0)
            {
                resolverWhile(ambito, ctxG, ctxL, raiz);
                return;
            }

            if (raiz.ToString().CompareTo(ConstantesJC.DOWHILE) == 0)
            {
                resolverDoWhile(ambito, ctxG, ctxL, raiz);
                return;
            }

            if (raiz.ToString().CompareTo(ConstantesJC.WHILEX) == 0)
            {
                resolverWhilex(ambito, ctxG, ctxL, raiz);
                return;
            }

            if (raiz.ToString().CompareTo(ConstantesJC.REPEAT) == 0)
            {
                resolverRepeat(ambito, ctxG, ctxL, raiz);
                return;
            }

            if (raiz.ToString().CompareTo(ConstantesJC.OUTSTRING) == 0)
            {
                resolverOutString(ambito, ctxG, ctxL, raiz);
                return;
            }

            if (raiz.ToString().CompareTo(ConstantesJC.OUTINT) == 0)
            {
                resolverOutInt(ambito, ctxG, ctxL, raiz);
                return;
            }

            if (raiz.Token != null  && raiz.FindTokenAndGetText().Equals(ConstantesJC.BREAK, StringComparison.OrdinalIgnoreCase))
            {
                resolverEscape(false, raiz.Token.Location.Line, raiz.Token.Location.Column);
                return;
            }

            if (raiz.Token !=null && raiz.FindTokenAndGetText().Equals(ConstantesJC.CONTINUE, StringComparison.OrdinalIgnoreCase))
            {
                resolverEscape(true, raiz.Token.Location.Line, raiz.Token.Location.Column);
                return;
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


            if (raiz.ToString().CompareTo(ConstantesJC.CLASE) == 0){
                C3d.escribir("}\n", esInit);
            }
            if (raiz.ToString().CompareTo(ConstantesJC.CONSTRUCTOR) == 0 ){
                C3d.escribir(final + ":", esInit);
                C3d.escribir("}\n", esInit);
            }
            if (raiz.ToString().CompareTo(ConstantesJC.PRINCIPAL) == 0 ){
                C3d.escribir(final + ":", esInit);
                C3d.escribir("}\n", esInit);
            }
            if (raiz.ToString().CompareTo(ConstantesJC.METODO) == 0)
            {
                C3d.escribir(final+":", esInit);
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
            C3d.escribir("void " + referencia + "() {", false);
            final = C3d.generarEtq();
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
                sObj = TablaSimbolo.buscarObj(ctxL, nObj);
                if (sObj == null)
                    sObj = TablaSimbolo.buscarObj(ctxG, nObj);

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
                sObj = TablaSimbolo.buscarObj(ctxG, nObj);
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
                        ManejadorErrores.General("No se puede realizar la asignación por los tipos");
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
            sObj = TablaSimbolo.buscarObj(ctxL, nObj);
            if (sObj == null)
            {
                sObj = TablaSimbolo.buscarObj(ctxG, nObj);
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

        public static C3d resolverLLamada(Simbolo ambito, Contexto ctxG, Contexto ctxL, ParseTreeNode raiz){
            C3d resultado = new C3d();
            String tthis, temp1, temp2, lparams, nObj, nMtd, padre= "";
            ParseTreeNode parametros = null;
            C3d valorParam;
            Expresion expresion;
            int cont = 2;
            Simbolo sObj = null;
            Simbolo sMtd = null;
            bool esDeObjeto = true;
            
            if (raiz.ChildNodes.Count() == 1 ||
                (raiz.ChildNodes.Count() == 2 && raiz.ChildNodes.ElementAt(1).ToString().CompareTo(ConstantesJC.EXPRESIONES) == 0))
            {
                if (ambito.nombre.Equals("Main", StringComparison.OrdinalIgnoreCase))
                {
                    ManejadorErrores.General("Se requiere de una instancia para poder implementar la llamada desde el método main");
                    return null;
                }
                esDeObjeto = false;
                nMtd = raiz.ChildNodes.ElementAt(0).FindTokenAndGetText();
                padre = ambito.padre;
            }
            else if (raiz.ChildNodes.Count() == 2 ||
                raiz.ChildNodes.Count() == 3 && !raiz.ChildNodes.ElementAt(0).FindTokenAndGetText().Equals("this", StringComparison.OrdinalIgnoreCase))
            {
                nObj = raiz.ChildNodes.ElementAt(0).FindTokenAndGetText();
                nMtd = raiz.ChildNodes.ElementAt(1).FindTokenAndGetText();
                sObj = TablaSimbolo.buscarObj(ctxL, nObj);
                if (sObj == null || !sObj.esObjeto())
                {
                    if (ambito.nombre.Equals("Main", StringComparison.OrdinalIgnoreCase))
                    {
                        ManejadorErrores.General("Se requiere de una instancia para poder implementar la llamada desde el método main");
                        return null;
                    }
                    sObj = TablaSimbolo.buscarObj(ctxG, nObj);
                }
                if (sObj == null || !sObj.esObjeto())
                {
                    ManejadorErrores.General("No existe la variable en la clase actual que sea un objeto " + nObj);
                    return null;
                }
                padre = sObj.tipo;
            }
            //Obteniendo el objeto o variable inicial... debe ser global
            //this.id.mtd() ->Sin parametros
            //this.id.mtd(...) -> con parametros
            //id es el nombre del objeto
            else {
                if (ambito.nombre.Equals("Main", StringComparison.OrdinalIgnoreCase))
                {
                    ManejadorErrores.General("Se requiere de una instancia para poder implementar la llamada desde el método main");
                    return null;
                }
                nObj = raiz.ChildNodes.ElementAt(1).FindTokenAndGetText();
                sObj = TablaSimbolo.buscarObj(ctxG, nObj);
                nMtd = raiz.ChildNodes.ElementAt(2).FindTokenAndGetText();
                if (sObj == null || !sObj.esObjeto())
                {
                    ManejadorErrores.General("No existe la variable en la clase actual que sea un objeto " + nObj);
                }
                padre = sObj.tipo;
            }


            //El objeto es global
            if (sObj !=null && sObj.esGlobal && esDeObjeto)
            {
                //Obtener el this
                temp1 = C3d.generarTemp();
                C3d.escribirOperacion(temp1, "P", "+", "1", false);
                temp2 = C3d.leerDePila(temp1, false);
                temp1 = C3d.generarTemp();
                C3d.escribirOperacion(temp1, temp2, "+", sObj.pos.ToString(), false);
                tthis = C3d.leerDeHeap(temp1, false);
            }
            //El objeto es local
            else if (sObj !=null && !sObj.esGlobal && esDeObjeto)
            {
                temp1 = C3d.generarTemp();
                C3d.escribirOperacion(temp1, "P", "+", sObj.pos.ToString(), false);
                tthis = C3d.leerDePila(temp1, false);
            }
            //Es metodo de la clase y no de una instancia
            else 
            {
                temp1 = C3d.generarTemp();
                C3d.escribirOperacion(temp1, "P", "+", "1", false);
                tthis = C3d.leerDePila(temp1, false);            
            }


            //Cambio de ambito simulado
            temp1 = C3d.generarTemp();
            C3d.escribirOperacion(temp1, "P", "+", ambito.tamanio.ToString(), false);

            //Enviar this actual como parametro en el this del metodo a llamar
            temp2 = C3d.generarTemp();
            C3d.escribirOperacion(temp2, temp1, "+", "1", false);
            C3d.escribirEnPila(temp2, tthis, false);

            //Obtener parametros
            lparams = "";
            parametros = ParserJcode.obtenerExpresiones(raiz);
            if(parametros != null){
                    cont = 2;
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
            }

            //Buscar en la tabla de simbolo el método
            sMtd = TablaSimbolo.buscarMetodo(padre, nMtd, lparams, esDeObjeto);
            if (sMtd == null)
            {
                ManejadorErrores.General("El método " + nMtd + " no existe o no es accesible para ser usado");
                return resultado;
            }
            if (sMtd.esObjeto())
            {
                resultado.tipo = Constantes.T_OBJETO;
                resultado.ntipo = sMtd.tipo;
                resultado.esArr = sMtd.esArr;
            }
            else
            {
                resultado.tipo = Constantes.obtenerTipo(sMtd.tipo);
                resultado.esArr = sMtd.esArr;
            }

            //Aumentar P
            C3d.aumentarP(ambito.tamanio.ToString(), false);
            C3d.escribir(padre + "_" + nMtd + "_" + lparams + "();", false);
            //Guardando el return
            temp1 = C3d.generarTemp();
            C3d.escribirOperacion(temp1, "P", "+", "0", false);
            resultado.cad = C3d.leerDePila(temp1, false);
            C3d.disminuirP(ambito.tamanio.ToString(), false);
            return resultado;            
        }

        public void resolverRetorno(Simbolo ambito, Contexto ctxG, Contexto ctxL, ParseTreeNode raiz, bool esInit) {

            if (raiz.ChildNodes.Count() == 1)
            {
                Expresion exp = new Expresion(ctxG, ctxL, ambito, esInit);
                C3d solucion = exp.resolver(raiz.ChildNodes.ElementAt(0));
                solucion = C3d.castearA(solucion, ambito.tipo, ambito.tamanio, esInit);
                if (solucion == null)
                {
                    ManejadorErrores.General("El tipo de retorno no corresponde al tipo que se esperaba para el ambito " + ambito.nombre);
                    return;
                }
                String temp1 = C3d.generarTemp();
                C3d.escribirOperacion(temp1, "P", "+", "0", esInit);
                C3d.escribirEnPila(temp1, solucion.cad, esInit);
            }
            else
            {
                if (!ambito.tipo.Equals("void", StringComparison.OrdinalIgnoreCase))
                {
                    ManejadorErrores.General("El tipo de retorno no puede ser Void dado que no corresponde al tipo que se esperaba para el ambito " + ambito.nombre);
                    return;
                }
            }
            C3d.escribirSaltoIncond(final, esInit);

        }

        public void resolverNoSql(Simbolo ambito, Contexto ctxG, Contexto ctxL, ParseTreeNode raiz, bool esInit){
            // _create + expresion + lexps 
            if (raiz.ChildNodes.ElementAt(0).FindTokenAndGetText().Equals("create", StringComparison.OrdinalIgnoreCase))
            {
                resolverCreate(ambito, ctxG, ctxL, raiz, esInit);
                return;
            }
            // _select + lexps +  expresion 
            if(raiz.ChildNodes.ElementAt(0).FindTokenAndGetText().Equals("select",StringComparison.OrdinalIgnoreCase)){
                resolverSelect(ambito, ctxG, ctxL, raiz, esInit);
                return;
            }
            // _insert + expresion   + lexps   + asigparams
            if(raiz.ChildNodes.ElementAt(0).FindTokenAndGetText().Equals("insert",StringComparison.OrdinalIgnoreCase)){
                resolverInsert(ambito, ctxG, ctxL, raiz, esInit);
                return;
            }
            // _update + expresion   + expresion   +  expresion   +  expresion 
            if(raiz.ChildNodes.ElementAt(0).FindTokenAndGetText().Equals("update",StringComparison.OrdinalIgnoreCase)){
                resolverUpdate(ambito, ctxG, ctxL, raiz, esInit);
                return;
            }
            // _delete + expresion   + expresion   +  expresion 
            if(raiz.ChildNodes.ElementAt(0).FindTokenAndGetText().Equals("delete",StringComparison.OrdinalIgnoreCase)){
                resolverDelete(ambito, ctxG, ctxL, raiz, esInit);
                return;
            }
            // _drop + expresion 
            if(raiz.ChildNodes.ElementAt(0).FindTokenAndGetText().Equals("drop",StringComparison.OrdinalIgnoreCase)){
                resolverDrop(ambito, ctxG, ctxL, raiz, esInit);
                return;
            }
        }

        public void resolverCreate(Simbolo ambito, Contexto ctxG, Contexto ctxL, ParseTreeNode raiz, bool esInit) {
            /**
             * _create + expresion + lexps 
             * STACK
             * | 2 | -> PTR al nombre de la colección
             * | 3 | -> número de columnas
             * | 4 | -> PTR a la primera columna
             * | 5 | -> PTR a la segunda columna
             * |...| -> ...
             * | x | -> PTR a la n-ésima columna
             */

            C3d nCollection = null;
            C3d tExp = null;
            int cols = 0;
            List<C3d> soluciones = new List<C3d>();
            String temp1= "";
            Expresion exp = new Expresion(ctxG, ctxL, ambito, esInit);

            //Simular el cambio de ambito
            String cambio = C3d.generarTemp();
            C3d.escribirOperacion(cambio, "P", "+", ambito.tamanio.ToString(), false);
            
            //Resolviendo el nombre de la colección
            nCollection = exp.resolver(raiz.ChildNodes.ElementAt(1));
            nCollection = C3d.castearA(nCollection, "String", ambito.tamanio, esInit);
            if (nCollection == null)
            {
                ManejadorErrores.General("No se ha podido obtener el nombre de la colección " + ambito.padre + "-" + ambito.nombre);
                return;
            }
            //Resolviendo las columnas de la colección
            foreach (ParseTreeNode hijo in raiz.ChildNodes.ElementAt(2).ChildNodes)
            {
                tExp = exp.resolver(hijo);
                tExp = C3d.castearA(tExp, "String", ambito.tamanio, esInit);
                if (tExp == null)
                {
                    ManejadorErrores.General("No se ha podido obtener el valor de la columna " + cols.ToString() + " de la colección " + ambito.padre + "-" + ambito.nombre);
                    return;
                }
                soluciones.Add(tExp);
                cols += 1;
            }
            //Escribir los datos como parametros
            int pos = 2;
            temp1 = C3d.generarTemp();
            C3d.escribirOperacion(temp1, cambio, "+", pos.ToString(), esInit);
            C3d.escribirEnPila(temp1, nCollection.cad, esInit);
            //Escribir el numero de cols
            pos += 1;
            temp1 = C3d.generarTemp();
            C3d.escribirOperacion(temp1, cambio, "+", pos.ToString(), esInit);
            C3d.escribirEnPila(temp1, cols.ToString(), esInit);
            //Escribir los puntero de las columnas
            foreach (C3d parametro in soluciones)
            {
                pos += 1;
                temp1 = C3d.generarTemp();
                C3d.escribirOperacion(temp1, cambio, "+", pos.ToString(), esInit);
                C3d.escribirEnPila(temp1, parametro.cad, esInit);
            }
            //Generar la llamada para create();
            C3d.aumentarP(ambito.tamanio.ToString(), esInit);
            C3d.escribir("create();", esInit);
            C3d.disminuirP(ambito.tamanio.ToString(), esInit);
        
        }

        public void resolverSelect(Simbolo ambito, Contexto ctxG, Contexto ctxL, ParseTreeNode raiz, bool esInit)
        {
            /**
             * _select + lexps +  expresion 
             * STACK
             * | 0 | -> this
             * | 1 | -> return
             * | 2 | -> PTR al nombre de la colección
             * | 3 | -> número de columnas
             * | 4 | -> PTR a la primera columna
             * | 5 | -> PTR a la segunda columna
             * |...| -> ...
             * | x | -> PTR a la n-ésima columna
             */

            C3d nCollection = null;
            C3d tExp = null;
            int cols = 0;
            List<C3d> soluciones = new List<C3d>();
            String temp1 = "";
            Expresion exp = new Expresion(ctxG, ctxL, ambito, esInit);

            //Simular el cambio de ambito
            String cambio = C3d.generarTemp();
            C3d.escribirOperacion(cambio, "P", "+", ambito.tamanio.ToString(), false);

            //Resolviendo las columnas de la colección
            foreach (ParseTreeNode hijo in raiz.ChildNodes.ElementAt(1).ChildNodes)
            {
                tExp = exp.resolver(hijo);
                tExp = C3d.castearA(tExp, "String", ambito.tamanio, esInit);
                if (tExp == null)
                {
                    ManejadorErrores.General("No se ha podido obtener el valor de la columna " + cols.ToString() + " de la colección " + ambito.padre + "-" + ambito.nombre);
                    return;
                }
                soluciones.Add(tExp);
                cols += 1;
            }
            //Resolviendo el nombre de la colección
            nCollection = exp.resolver(raiz.ChildNodes.ElementAt(2));
            nCollection = C3d.castearA(nCollection, "String", ambito.tamanio, esInit);
            if (nCollection == null)
            {
                ManejadorErrores.General("No se ha podido obtener el nombre de la colección " + ambito.padre + "-" + ambito.nombre);
                return;
            }
            //Escribir los datos como parametros
            int pos = 2;
            temp1 = C3d.generarTemp();
            C3d.escribirOperacion(temp1, cambio, "+", pos.ToString(), esInit);
            C3d.escribirEnPila(temp1, nCollection.cad, esInit);
            //Escribir el numero de cols
            pos += 1;
            temp1 = C3d.generarTemp();
            C3d.escribirOperacion(temp1, cambio, "+", pos.ToString(), esInit);
            C3d.escribirEnPila(temp1, cols.ToString(), esInit);
            //Escribir los puntero de las columnas
            foreach (C3d parametro in soluciones)
            {
                pos += 1;
                temp1 = C3d.generarTemp();
                C3d.escribirOperacion(temp1, cambio, "+", pos.ToString(), esInit);
                C3d.escribirEnPila(temp1, parametro.cad, esInit);
            }
            //Generar la llamada para el select();
            C3d.aumentarP(ambito.tamanio.ToString(), esInit);
            C3d.escribir("select();", esInit);
            C3d.disminuirP(ambito.tamanio.ToString(), esInit);
        }

        public void resolverInsert(Simbolo ambito, Contexto ctxG, Contexto ctxL, ParseTreeNode raiz, bool esInit)
        {
            /**
             * _insert + expresion   + lexps   + asigparams
             * STACK
             * | 0 | -> this
             * | 1 | -> return
             * | 2 | -> PTR al nombre de la colección
             * | 3 | -> número de valores - columnas
             * | 4 | -> PTR a la primera columna
             * | 5 | -> PTR al primer valor
             * |...| -> ...
             * | x | -> PTR a la n-ésima columna
             * | y | -> PTR al n-ésimo valor
             */

            if (raiz.ChildNodes.Count() != 4) {
                ManejadorErrores.General("Existe un error en la cadena de entrada para realizar el insert");
            }

            C3d nCollection = null;
            C3d tExp = null;
            int cols = 0;
            int vals = 0;
            List<C3d> soluciones = new List<C3d>();
            List<C3d> valores = new List<C3d>();
            String temp1 = "";
            Expresion exp = new Expresion(ctxG, ctxL, ambito, esInit);

            //Simular el cambio de ambito
            String cambio = C3d.generarTemp();
            C3d.escribirOperacion(cambio, "P", "+", ambito.tamanio.ToString(), false);

            //Resolviendo el nombre de la colección
            nCollection = exp.resolver(raiz.ChildNodes.ElementAt(1));
            nCollection = C3d.castearA(nCollection, "String", ambito.tamanio, esInit);
            if (nCollection == null)
            {
                ManejadorErrores.General("No se ha podido obtener el nombre de la colección " + ambito.padre + "-" + ambito.nombre);
                return;
            }

            //Resolviendo las columnas de la colección
            foreach (ParseTreeNode hijo in raiz.ChildNodes.ElementAt(2).ChildNodes)
            {
                tExp = exp.resolver(hijo);
                tExp = C3d.castearA(tExp, "String", ambito.tamanio, esInit);
                if (tExp == null)
                {
                    ManejadorErrores.General("No se ha podido obtener el valor de la columna " + cols.ToString() + " de la colección " + ambito.padre + "-" + ambito.nombre);
                    return;
                }
                soluciones.Add(tExp);
                cols += 1;
            }

            //Resolviendo los valores de la colección
            foreach (ParseTreeNode hijo in raiz.ChildNodes.ElementAt(3).ChildNodes)
            {
                tExp = exp.resolver(hijo);
                tExp = C3d.castearA(tExp, "String", ambito.tamanio, esInit);
                if (tExp == null)
                {
                    ManejadorErrores.General("No se ha podido obtener el valor de la columna " + vals.ToString() + " de la colección " + ambito.padre + "-" + ambito.nombre);
                    return;
                }
                valores.Add(tExp);
                vals += 1;
            }


            if (cols != vals) {
                ManejadorErrores.General("No se han enviado igual cantidad de columnas y valores para el insert en la colección " + ambito.padre + "-" + ambito.nombre);
                return;
            }
            
            //Escribir los datos como parametros
            int pos = 2;
            temp1 = C3d.generarTemp();
            C3d.escribirOperacion(temp1, cambio, "+", pos.ToString(), esInit);
            C3d.escribirEnPila(temp1, nCollection.cad, esInit);
            //Escribir el numero de cols
            pos += 1;
            temp1 = C3d.generarTemp();
            C3d.escribirOperacion(temp1, cambio, "+", pos.ToString(), esInit);
            C3d.escribirEnPila(temp1, cols.ToString(), esInit);

            //Escribir los puntero de las columnas y los punteros de los valores
            int i = 0;
            while (i < cols) {
                pos += 1;
                temp1 = C3d.generarTemp();
                C3d.escribirOperacion(temp1, cambio, "+", pos.ToString(), esInit);
                C3d.escribirEnPila(temp1, soluciones[i].cad, esInit);
                pos += 1;
                temp1 = C3d.generarTemp();
                C3d.escribirOperacion(temp1, cambio, "+", pos.ToString(), esInit);
                C3d.escribirEnPila(temp1, valores[i].cad, esInit);
                i++;
            }

            //Generar la llamada para insert();
            C3d.aumentarP(ambito.tamanio.ToString(), esInit);
            C3d.escribir("insert();", esInit);
            C3d.disminuirP(ambito.tamanio.ToString(), esInit);
        }

        public void resolverUpdate(Simbolo ambito, Contexto ctxG, Contexto ctxL, ParseTreeNode raiz, bool esInit)
        {
            /**
             * _update + expresion   + expresion   +  expresion   +  expresion 
             * STACK
             * | 0 | -> this
             * | 1 | -> return
             * | 2 | -> PTR al nombre de la colección
             * | 3 | -> PTR al nombre de la columna
             * | 4 | -> PTR al valor para actualiza
             * | 5 | -> PTR al valor nuevo
             */

            C3d tExp = null;
            int cont = 2;
            String temp1 = "";
            Expresion exp = new Expresion(ctxG, ctxL, ambito, esInit);

            //Simular el cambio de ambito
            String cambio = C3d.generarTemp();
            C3d.escribirOperacion(cambio, "P", "+", ambito.tamanio.ToString(), false);

            //Resolviendo el nombre de la colección
            tExp = exp.resolver(raiz.ChildNodes.ElementAt(1));
            tExp = C3d.castearA(tExp, "String", ambito.tamanio, esInit);
            if (tExp == null)
            {
                ManejadorErrores.General("No se ha podido obtener el nombre de la colección " + ambito.padre + "-" + ambito.nombre);
                return;
            }
            //Escribiendo el puntero del nombre de la colección
            temp1 = C3d.generarTemp();
            C3d.escribirOperacion(temp1, cambio, "+", cont.ToString(), esInit);
            C3d.escribirEnPila(temp1, tExp.cad, esInit);
            cont++;

            //Resolviendo el valor de número de columna
            tExp = exp.resolver(raiz.ChildNodes.ElementAt(2));
            tExp = C3d.castearA(tExp, "String", ambito.tamanio, esInit);
            if (tExp == null)
            {
                ManejadorErrores.General("No se ha podido obtener el numero de columna que se quiere modificar " + ambito.padre + "-" + ambito.nombre);
                return;
            }
            //Escribiendo el puntero del numero de columna
            temp1 = C3d.generarTemp();
            C3d.escribirOperacion(temp1, cambio, "+", cont.ToString(), esInit);
            C3d.escribirEnPila(temp1, tExp.cad, esInit);
            cont++;


            //Resolviendo el valor actual de la columna
            tExp = exp.resolver(raiz.ChildNodes.ElementAt(3));
            tExp = C3d.castearA(tExp, "String", ambito.tamanio, esInit);
            if (tExp == null)
            {
                ManejadorErrores.General("No se ha podido obtener el valor que se desea modificar de la columna " + ambito.padre + "-" + ambito.nombre);
                return;
            }
            //Escribiendo el puntero del valor actual de la columna
            temp1 = C3d.generarTemp();
            C3d.escribirOperacion(temp1, cambio, "+", cont.ToString(), esInit);
            C3d.escribirEnPila(temp1, tExp.cad, esInit);
            cont++;

            //Resolviendo el valor nuevo de la columna
            tExp = exp.resolver(raiz.ChildNodes.ElementAt(4));
            tExp = C3d.castearA(tExp, "String", ambito.tamanio, esInit);
            if (tExp == null)
            {
                ManejadorErrores.General("No se ha podido obtener el valor nuevo que se desea modificar de la columna " + ambito.padre + "-" + ambito.nombre);
                return;
            }
            //Escribiendo el puntero del valor nuevo de la columna
            temp1 = C3d.generarTemp();
            C3d.escribirOperacion(temp1, cambio, "+", cont.ToString(), esInit);
            C3d.escribirEnPila(temp1, tExp.cad, esInit);

            //Generar la llamada para update();
            C3d.aumentarP(ambito.tamanio.ToString(), esInit);
            C3d.escribir("update();", esInit);
            C3d.disminuirP(ambito.tamanio.ToString(), esInit);
        }

        public void resolverDelete(Simbolo ambito, Contexto ctxG, Contexto ctxL, ParseTreeNode raiz, bool esInit)
        {
            /**
             * _delete + expresion   + expresion   +  expresion 
             * STACK
             * | 0 | -> this
             * | 1 | -> return
             * | 2 | -> PTR al nombre de la colección
             * | 3 | -> PTR al nombre de la columna
             * | 4 | -> PTR al valor para borrar
             */

            C3d tExp = null;
            int cont = 2;
            String temp1 = "";
            Expresion exp = new Expresion(ctxG, ctxL, ambito, esInit);

            //Simular el cambio de ambito
            String cambio = C3d.generarTemp();
            C3d.escribirOperacion(cambio, "P", "+", ambito.tamanio.ToString(), false);

            //Resolviendo el nombre de la colección
            tExp = exp.resolver(raiz.ChildNodes.ElementAt(1));
            tExp = C3d.castearA(tExp, "String", ambito.tamanio, esInit);
            if (tExp == null)
            {
                ManejadorErrores.General("No se ha podido obtener el nombre de la colección " + ambito.padre + "-" + ambito.nombre);
                return;
            }
            //Escribiendo el puntero del nombre de la colección
            temp1 = C3d.generarTemp();
            C3d.escribirOperacion(temp1, cambio, "+", cont.ToString(), esInit);
            C3d.escribirEnPila(temp1, tExp.cad, esInit);
            cont++;

            //Resolviendo el nombre de la columna
            tExp = exp.resolver(raiz.ChildNodes.ElementAt(2));
            tExp = C3d.castearA(tExp, "String", ambito.tamanio, esInit);
            if (tExp == null)
            {
                ManejadorErrores.General("No se ha podido obtener el nombre de columna que se quiere eliminar " + ambito.padre + "-" + ambito.nombre);
                return;
            }
            //Escribiendo el puntero del nombre de la columna
            temp1 = C3d.generarTemp();
            C3d.escribirOperacion(temp1, cambio, "+", cont.ToString(), esInit);
            C3d.escribirEnPila(temp1, tExp.cad, esInit);
            cont++;


            //Resolviendo el valor  de la columna
            tExp = exp.resolver(raiz.ChildNodes.ElementAt(3));
            tExp = C3d.castearA(tExp, "String", ambito.tamanio, esInit);
            if (tExp == null)
            {
                ManejadorErrores.General("No se ha podido obtener el valor que se desea eliminar de la columna " + ambito.padre + "-" + ambito.nombre);
                return;
            }
            //Escribiendo el puntero del valor actual de la columna
            temp1 = C3d.generarTemp();
            C3d.escribirOperacion(temp1, cambio, "+", cont.ToString(), esInit);
            C3d.escribirEnPila(temp1, tExp.cad, esInit);


            //Generar la llamada para delete();
            C3d.aumentarP(ambito.tamanio.ToString(), esInit);
            C3d.escribir("delete();", esInit);
            C3d.disminuirP(ambito.tamanio.ToString(), esInit);
        }

        public void resolverDrop(Simbolo ambito, Contexto ctxG, Contexto ctxL, ParseTreeNode raiz, bool esInit)
        {
            /**
             * _drop + expresion 
             * STACK
             * | 0 | -> this
             * | 1 | -> return
             * | 2 | -> PTR al nombre de la colección
             */

            C3d tExp = null;
            int cont = 2;
            String temp1 = "";
            Expresion exp = new Expresion(ctxG, ctxL, ambito, esInit);

            //Resolviendo el nombre de la colección
            tExp = exp.resolver(raiz.ChildNodes.ElementAt(1));
            tExp = C3d.castearA(tExp, "String", ambito.tamanio, esInit);
            if (tExp == null)
            {
                ManejadorErrores.General("No se ha podido obtener el nombre de la colección " + ambito.padre + "-" + ambito.nombre);
                return;
            }

            //Simular el cambio de ambito
            String cambio = C3d.generarTemp();
            C3d.escribirOperacion(cambio, "P", "+", ambito.tamanio.ToString(), false);
            
            //Escribiendo el puntero del nombre de la colección
            temp1 = C3d.generarTemp();
            C3d.escribirOperacion(temp1, cambio, "+", cont.ToString(), esInit);
            C3d.escribirEnPila(temp1, tExp.cad, esInit);
 
            //Generar la llamada para drop();
            C3d.aumentarP(ambito.tamanio.ToString(), esInit);
            C3d.escribir("drop();", esInit);
            C3d.disminuirP(ambito.tamanio.ToString(), esInit);
        }

        public void resolverEscape(bool tipo, int fila, int columna) {
            String etq = Compilador.Compilador.getEtqSalida(tipo);
            if (etq == null) {
                ManejadorErrores.Semantico("Sentencia de escape fuera de contexto", fila, columna);
                return;
            }
            C3d.escribirSaltoIncond(etq, false);        
        }

        public void resolverIf(Simbolo ambito, Contexto ctxG, Contexto ctxL, ParseTreeNode raiz)
        {
            ctxL.aumentarNivel();
            Compilador.Compilador.generarEtqsSalida();
            Expresion exp = new Expresion(ctxG, ctxL, ambito, false);
            //Linicio:
            C3d.escribir(Compilador.Compilador.getEtqSalida(true) + ":", false);
            //Generar condicion
            C3d condicion = exp.resolver(raiz.ChildNodes.ElementAt(0));
            condicion = C3d.castearA(condicion, "bool", ambito.tamanio, false);
            if (condicion == null) {
                ManejadorErrores.General("Se esperaba un tipo bool para la condición del if");
                ctxL.limpiarNivel();
                Compilador.Compilador.eliminarEtqsSalida();
                return;
            }
            C3d.verificarBoolean(condicion, false);
            //Lv:
            C3d.escribir(condicion.etqV + ":", false);
            //Verificar y resolver sentencias si existen
            ParseTreeNode sentencias = ParserJcode.obtenerSentencias(raiz);
            if (sentencias != null && sentencias.ChildNodes.Count()>0) {
                generar(ambito, ctxG, ctxL, sentencias, false);
            }
            //goto Lfin:
            C3d.escribirSaltoIncond(Compilador.Compilador.getEtqSalida(false), false);
            //Lf:
            C3d.escribir(condicion.etqF + ":", false);

            ParseTreeNode otrosIf = ParserJcode.obtenerOtrosIf(raiz);
            if (otrosIf != null && otrosIf.ChildNodes.Count() > 0) {
                resolverOtrosIf(ambito, ctxG, ctxL, otrosIf);
            }

            //Lfin:
            C3d.escribir(Compilador.Compilador.getEtqSalida(false) + ":", false);
            ctxL.limpiarNivel();
            Compilador.Compilador.eliminarEtqsSalida();
        }

        public void resolverOtrosIf(Simbolo ambito, Contexto ctxG, Contexto ctxL, ParseTreeNode raiz)
        {
            Expresion exp = new Expresion(ctxG, ctxL, ambito, false);
            C3d condicion;
            ParseTreeNode sentencias;
            //Lista de else if:
            if (raiz.ChildNodes.ElementAt(0).ToString().Equals(ConstantesJC.ELSEIFS, StringComparison.OrdinalIgnoreCase)) {
                foreach (ParseTreeNode elseif in raiz.ChildNodes.ElementAt(0).ChildNodes) {
                    //Generar condicion
                    condicion = exp.resolver(elseif.ChildNodes.ElementAt(0));
                    condicion = C3d.castearA(condicion, "bool", ambito.tamanio, false);
                    if (condicion == null)
                    {
                        ManejadorErrores.General("Se esperaba un tipo bool para la condición del elseif");
                        continue;
                    }
                    C3d.verificarBoolean(condicion, false);
                    //Lv:
                    C3d.escribir(condicion.etqV + ":", false);
                    //Verificar y resolver sentencias si existen
                    sentencias = ParserJcode.obtenerSentencias(elseif);
                    if (sentencias != null && sentencias.ChildNodes.Count() > 0)
                    {
                        generar(ambito, ctxG, ctxL, sentencias, false);
                    }
                    //goto Lfin:
                    C3d.escribirSaltoIncond(Compilador.Compilador.getEtqSalida(false), false);
                    //Lf:
                    C3d.escribir(condicion.etqF + ":", false);
                }            
            }

            //Generando las sentencias del else si existe
            ParseTreeNode nElse = ParserJcode.obtenerElse(raiz);
            if (nElse == null) {
                return;
            }
            sentencias = ParserJcode.obtenerSentencias(nElse);
            if (sentencias == null) {
                return;
            }
            generar(ambito, ctxG, ctxL, sentencias, false);
        }

        public void resolverIfNot(Simbolo ambito, Contexto ctxG, Contexto ctxL, ParseTreeNode raiz)
        {
            ctxL.aumentarNivel();
            Compilador.Compilador.generarEtqsSalida();
            Expresion exp = new Expresion(ctxG, ctxL, ambito, false);
            //Linicio:
            C3d.escribir(Compilador.Compilador.getEtqSalida(true) + ":", false);
            //Generar condicion
            C3d condicion = exp.resolver(raiz.ChildNodes.ElementAt(0));
            condicion = C3d.castearA(condicion, "bool", ambito.tamanio, false);
            if (condicion == null)
            {
                ManejadorErrores.General("Se esperaba un tipo bool para la condición del if");
                ctxL.limpiarNivel();
                Compilador.Compilador.eliminarEtqsSalida();
                return;
            }
            C3d.verificarBoolean(condicion, false);
            //Lf:
            C3d.escribir(condicion.etqF + ":", false);
            //Verificar y resolver sentencias si existen
            ParseTreeNode sentencias = ParserJcode.obtenerSentencias(raiz);
            if (sentencias != null && sentencias.ChildNodes.Count() > 0)
            {
                generar(ambito, ctxG, ctxL, sentencias, false);
            }
            //goto Lfin:
            C3d.escribirSaltoIncond(Compilador.Compilador.getEtqSalida(false), false);
            //Lv:
            C3d.escribir(condicion.etqV + ":", false);

        
            //Lfin:
            C3d.escribir(Compilador.Compilador.getEtqSalida(false) + ":", false);
            ctxL.limpiarNivel();
            Compilador.Compilador.eliminarEtqsSalida();
        }

        public void resolverWhile(Simbolo ambito, Contexto ctxG, Contexto ctxL, ParseTreeNode raiz)
        {
            ctxL.aumentarNivel();
            Compilador.Compilador.generarEtqsSalida();
            Expresion exp = new Expresion(ctxG, ctxL, ambito, false);
            //Linicio:
            C3d.escribir(Compilador.Compilador.getEtqSalida(true) + ":", false);
            //Generar condicion
            C3d condicion = exp.resolver(raiz.ChildNodes.ElementAt(0));
            condicion = C3d.castearA(condicion, "bool", ambito.tamanio, false);
            if (condicion == null)
            {
                ManejadorErrores.General("Se esperaba un tipo bool para la condición del if");
                ctxL.limpiarNivel();
                Compilador.Compilador.eliminarEtqsSalida();
                return;
            }
            C3d.verificarBoolean(condicion, false);
            //Lv:
            C3d.escribir(condicion.etqV + ":", false);
            //Verificar y resolver sentencias si existen
            ParseTreeNode sentencias = ParserJcode.obtenerSentencias(raiz);
            if (sentencias != null && sentencias.ChildNodes.Count() > 0)
            {
                generar(ambito, ctxG, ctxL, sentencias, false);
            }
            //goto Linicio:
            C3d.escribirSaltoIncond(Compilador.Compilador.getEtqSalida(true), false);
            //Lf:
            C3d.escribir(condicion.etqF + ":", false);
            //Lfin:
            C3d.escribir(Compilador.Compilador.getEtqSalida(false) + ":", false);
            ctxL.limpiarNivel();
            Compilador.Compilador.eliminarEtqsSalida();

        }

        public void resolverDoWhile(Simbolo ambito, Contexto ctxG, Contexto ctxL, ParseTreeNode raiz)
        {
        }

        public void resolverWhilex(Simbolo ambito, Contexto ctxG, Contexto ctxL, ParseTreeNode raiz)
        {
        }

        public void resolverRepeat(Simbolo ambito, Contexto ctxG, Contexto ctxL, ParseTreeNode raiz)
        {
        }

        public void resolverOutString(Simbolo ambito, Contexto ctxG, Contexto ctxL, ParseTreeNode raiz)
        {
            Expresion exp = new Expresion(ctxG, ctxL, ambito, false);
            C3d resultado = exp.resolver(raiz.ChildNodes.ElementAt(0));
            resultado = C3d.castearA(resultado, "string", ambito.tamanio, false);
            if (resultado == null) {
                ManejadorErrores.General("La expresión no posee un formato de string o el tipo no se puede castear");
                return;
            }
            String temp1 = C3d.generarTemp();
            C3d.escribirOperacion(temp1, "P", "+", ambito.tamanio.ToString(), false);
            String temp2 = C3d.generarTemp();
            C3d.escribirOperacion(temp2, temp1, "+", "0", false);
            C3d.escribirEnPila(temp2, resultado.cad, false);
            C3d.aumentarP(ambito.tamanio.ToString(), false);
            C3d.escribir("outString();", false);
            C3d.disminuirP(ambito.tamanio.ToString(), false);
        }

        public void resolverOutInt(Simbolo ambito, Contexto ctxG, Contexto ctxL, ParseTreeNode raiz)
        {
            Expresion exp = new Expresion(ctxG, ctxL, ambito, false);
            C3d resultado = exp.resolver(raiz.ChildNodes.ElementAt(0));
            resultado = C3d.castearA(resultado, "int", ambito.tamanio, false);
            if (resultado == null) {
                ManejadorErrores.General("La expresión no posee un formato de int");
                return;
            }
            C3d.escribir("printf(\"%d\"," + resultado.cad + ");", false);
        }

    }
}
