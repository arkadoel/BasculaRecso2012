using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using project.recso.bascula.data;
using System.Windows;

namespace project.recso.bascula.frontend.wpf
{
    public class claseIntercambio
    {
        public static inicio maestra;
        public static Usuario usuarioActual;
        public static wpf.web.residuos.adminResiduos adminResiduosActual;
        public static wpf.web.Clientes.adminClientes adminClientesActual;
        public static wpf.web.Obras.adminObras adminObrasActual;
        public static wpf.web.Transitos.transito adminTransitos;

        //objeto para el transito actual
        public class transitoActual{
            public static String matricula { get; set; }
            public static String numAlbaran { get; set; }
            public static String fechaEntrada { get; set; }
            public static String fechaSalida { get; set; }

            public static String bruto { get; set; }
            public static String tara { get; set; }
            public static String neto { get; set; }
            public static String importeIVA { get; set; }
            public static String importeFinal { get; set; }
            public static String importeSinIVA { get; set; }
            public static Residuo residuoSeleccionado { get; set; }
            public static String IVAaplicado { get; set; }

            public static String tipoVehiculo { get; set; }
            public static String metrosCubicos { get; set; }
            public static String pesoMetrosCubicos { get; set; }

            //empresas
            public static Empresa empPoseedor { get; set; }
            public static Empresa empProductor { get; set; }
            public static Empresa empPlantaTransferencia { get; set; }
            public static Empresa empPlantaValorizacion { get; set; }
            private static Empresa empTransportista;

            //obra
            public static Obra obra { get; set; }

            public static Empresa EmpTransportista
            {
                get { return transitoActual.empTransportista; }
                set 
                { 
                    transitoActual.empTransportista = value;
                    /*
                    if (value != null)
                    {
                        Vehiculo veh = new Vehiculo();
                        veh.fechaAlta = "IGNORAR";
                        veh.matriculaVehiculo = claseIntercambio.transitoActual.matricula;
                        veh.recnumEmpresa = value.recnum;
                        veh.fechaBaja = "";
                        logic.gestionVehiculos.mergeOrCreate(veh);
                    }*/
                    
                }
            }
        }


        public static String comprobarSiDatosRellenados()
        {
            String r="";

            if (claseIntercambio.transitoActual.bruto != "")
            {
                if (claseIntercambio.transitoActual.empPoseedor != null && claseIntercambio.transitoActual.empPoseedor.nombre != "")
                {
                    if (claseIntercambio.transitoActual.empProductor != null && claseIntercambio.transitoActual.empProductor.nombre.Replace(" ","") !="")
                    {
                        if (claseIntercambio.transitoActual.EmpTransportista != null && claseIntercambio.transitoActual.EmpTransportista.nombre != "")
                        {
                            if (claseIntercambio.transitoActual.fechaEntrada != "")
                            {
                                if (claseIntercambio.transitoActual.neto != "")
                                {
                                    if (claseIntercambio.transitoActual.obra != null)
                                    {
                                        if (claseIntercambio.transitoActual.residuoSeleccionado != null)
                                        {
                                            if (claseIntercambio.transitoActual.tipoVehiculo != null)
                                            {
                                                if (claseIntercambio.transitoActual.matricula != "")
                                                {
                                                    return "";
                                                }
                                                else r = "Debe poner la matricula";
                                            }
                                            else r = "Revise el tipo de vehiculo";
                                        }
                                        else r = "Revise el residuo seleccionado.";
                                    }
                                    else r = "Revise la obra introducida";
                                }
                                else r = "Revise los pesos";

                            }
                            else r = "La fecha de entrada no es correcta";
                        }
                        else r = "Vuelva a introducir la matricula del vehiculo";
                    }
                    else r = "Debe seleccionar una empresa para el productor";
                }
                else r = "Debe seleccionar una empresa para el poseedor.";
            }
            else r = "Debe rellenar el peso bruto";

            return r;
        }
        public static long? empresaRecnum(Empresa _empresa)
        {
            if (_empresa == null) return null;
            else return _empresa.recnum;

        }

        public static TransitosActuale toTransitoDB()
        {
            TransitosActuale actual = new TransitosActuale();
            actual.bruto = transitoActual.bruto;
            actual.fechaEntrada = transitoActual.fechaEntrada;
            actual.importeFinal = transitoActual.importeFinal;
            actual.importeIVA = transitoActual.importeIVA;
            actual.importeSinIva = transitoActual.importeSinIVA;
            actual.IVAaplicado = transitoActual.IVAaplicado;
            actual.matricula = transitoActual.matricula;
            actual.neto = transitoActual.neto;
            actual.numAlbaran = transitoActual.numAlbaran;
            actual.obra = transitoActual.obra.recnum;
            actual.plantaTransferencia =empresaRecnum( transitoActual.empPlantaTransferencia);
            actual.plantaValorizacion = empresaRecnum(transitoActual.empPlantaValorizacion);
            actual.poseedor = empresaRecnum(transitoActual.empPoseedor);
            actual.productor = empresaRecnum(transitoActual.empProductor);
            actual.residuo = transitoActual.residuoSeleccionado.recnum;
            actual.tara = transitoActual.tara;
            actual.tipoVehiculo = transitoActual.tipoVehiculo;
            actual.transportista = empresaRecnum(transitoActual.EmpTransportista);

            return actual;
        }

        public static void limpiarDeDatos()
        {
            claseIntercambio.transitoActual.bruto = "";
            claseIntercambio.transitoActual.tara = "";
            claseIntercambio.transitoActual.neto = "";
            claseIntercambio.transitoActual.importeFinal = "";
            claseIntercambio.transitoActual.importeIVA = "";
            claseIntercambio.transitoActual.importeSinIVA = "";
            claseIntercambio.transitoActual.empPlantaTransferencia = new Empresa();
            claseIntercambio.transitoActual.empPlantaValorizacion = new Empresa();
            claseIntercambio.transitoActual.empPoseedor = new Empresa();
            claseIntercambio.transitoActual.empProductor = new Empresa();
            claseIntercambio.transitoActual.EmpTransportista = new Empresa();
            claseIntercambio.transitoActual.fechaEntrada = "";
            claseIntercambio.transitoActual.fechaSalida = "";
            claseIntercambio.transitoActual.residuoSeleccionado = new Residuo();
            claseIntercambio.transitoActual.matricula = "";
            claseIntercambio.transitoActual.numAlbaran = null;
            claseIntercambio.transitoActual.tipoVehiculo = "";
            claseIntercambio.transitoActual.obra = new Obra();

            GC.Collect();
        }

        

        public static HistoricoAlbarane toAlbaranHistorico()
        {
            HistoricoAlbarane a = new HistoricoAlbarane();
            try
            {
               

                a.numAlbaran = claseIntercambio.transitoActual.numAlbaran;
                a.fechaEntrada = claseIntercambio.transitoActual.fechaEntrada;
                a.fechaSalida = claseIntercambio.transitoActual.fechaSalida;

                //area factura
                a.bruto = claseIntercambio.transitoActual.bruto;
                a.tara = claseIntercambio.transitoActual.tara;
                a.neto = claseIntercambio.transitoActual.neto;
                a.precioResiduo = claseIntercambio.transitoActual.residuoSeleccionado.precio.Value.ToString("F2");
                a.ivaAplicado = claseIntercambio.transitoActual.IVAaplicado;
                a.importeSinIVA = claseIntercambio.transitoActual.importeSinIVA;
                a.importeIVA = claseIntercambio.transitoActual.importeIVA;
                a.residuo = claseIntercambio.transitoActual.residuoSeleccionado.codigoLER.Trim() + " " + claseIntercambio.transitoActual.residuoSeleccionado.nombre.Trim();
                a.tipoResiduo = claseIntercambio.transitoActual.residuoSeleccionado.tipoMaterial;
                a.milenaProducto = claseIntercambio.transitoActual.residuoSeleccionado.milena;

                a.importeFinal = claseIntercambio.transitoActual.importeFinal;
                a.metrosCubicos = claseIntercambio.transitoActual.metrosCubicos;
                a.PesoMCubicos = claseIntercambio.transitoActual.pesoMetrosCubicos;

                //transportista            
                a.matricula = claseIntercambio.transitoActual.matricula;
                a.NombreTransportista = claseIntercambio.transitoActual.EmpTransportista.nombre;
                a.Telefonos = claseIntercambio.transitoActual.EmpTransportista.telefono;
                a.TipoVehiculo = claseIntercambio.transitoActual.tipoVehiculo;

                //empresa poseedor-pagador
                a.EmpPagadorNombre = claseIntercambio.transitoActual.empPoseedor.nombre;
                a.EmpPagadorDireccion = claseIntercambio.transitoActual.empPoseedor.direccion + "" + claseIntercambio.transitoActual.empPoseedor.localidad;
                a.empClienteMilena = claseIntercambio.transitoActual.empPoseedor.codigoMilena.ToString();
                a.FormaPago = claseIntercambio.transitoActual.empPoseedor.tipoDePago;

                a.empProductor = claseIntercambio.transitoActual.empProductor.nombre;
                

                //empresa generadora
                a.EntidadGeneradora = claseIntercambio.transitoActual.empProductor.nombre;

                //obra
                a.obra = claseIntercambio.transitoActual.obra.denominacion;
                a.LugarGeneracion = claseIntercambio.transitoActual.obra.denominacion;
                a.milenaObra = claseIntercambio.transitoActual.obra.codigoMilena.ToString();

                //poseedor
                a.empPoseedor = claseIntercambio.transitoActual.empPoseedor.nombre;
            }
            catch (Exception ex)
            {

            }
                return a;
            
        }

        public static MessageBoxResult msg(String texto, String titulo, MessageBoxButton boton, MessageBoxImage imagen)
        {
            return Microsoft.Windows.Controls.MessageBox.Show(texto, titulo, boton, imagen);
        }

        public static String paraFiltros(String frase)
        {
            String res = frase.ToLower();
            res = res.Replace(" ", "");
            res = res.Replace("á", "a");
            res = res.Replace("é", "e");
            res = res.Replace("í", "i");
            res = res.Replace("ó", "o");
            res = res.Replace("ú", "u");
            return res;
        }

    }
}
