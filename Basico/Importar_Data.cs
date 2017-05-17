using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;
namespace Basico
{
   public class Importar_Data
    {       
        #region <IMPORTAR DATOS AL SQL>
        public static void ejecutatarea(ref string verror,ref string verror_procesos)
        {
            DataSet dstienda = null;
            DataTable dttienda = null;
            DataTable dtrutadbf = null;
            DataTable dttipocarpeta = null;
            try
            {

                verror = "";
                dstienda = dsgettienda(ref verror);
                dttienda = dstienda.Tables[0];
                dtrutadbf = dstienda.Tables[1];
                dttipocarpeta = dstienda.Tables[2];
                if (dstienda != null)
                {
                    string ruta_wx = dttipocarpeta.Rows[0]["carpeta"].ToString();
                    if (dttienda.Rows.Count > 0)
                    {

                        string carpetadbf = dtrutadbf.Rows[0]["RUTADBF"].ToString();
                        for (Int32 itienda = 0; itienda < dttienda.Rows.Count; ++itienda)
                        {
                            string carpetatd = carpetadbf + dttienda.Rows[itienda]["CARPETAT"].ToString();
                            //verror_procesos = carpetatd;
                            if ((Directory.Exists(@carpetatd)))
                            {
                                //verror_procesos = carpetatd;
                                //System.IO.Directory.CreateDirectory(@carpetatd);

                                string carpetatienda = carpetadbf + dttienda.Rows[itienda]["CARPETAT"].ToString() + "\\" + ruta_wx;

                                string carpetatemp = carpetatienda + "\\Temp";

                                //si existe carpeta temporal verificamos los archivos
                                if (Directory.Exists(carpetatemp))
                                {
                                    string[] filestemp;
                                    filestemp = Directory.GetFiles(carpetatemp,"*.*");
                                    if (filestemp.Length > 0)
                                    {
                                        for (Int32 t = 0; t < filestemp.Length; ++t)
                                        {
                                            string name_temp = System.IO.Path.GetFileNameWithoutExtension(@filestemp[t].ToString());
                                            string nombrearchivo_temp = System.IO.Path.GetFileName(@filestemp[t].ToString());
                                            string _rutatempfile = carpetatienda + "\\" + nombrearchivo_temp;

                                            if (!File.Exists(_rutatempfile))
                                            {
                                                File.Move(@filestemp[t].ToString(), _rutatempfile);
                                            }
                                        }
                                    }

                                }

                                //string carpetacopy = carpetatienda + "\\BkDbf\\";

                                string centidad = dttienda.Rows[itienda]["CODIGOENT"].ToString();
                                string[] filesrar;
                                filesrar = System.IO.Directory.GetFiles(@carpetatienda, "*.*");
                                if (filesrar.Length > 0)
                                {
                                    carpetatienda = carpetatienda + "\\DBF";
                                    if (!(Directory.Exists(@carpetatienda)))
                                    {
                                        System.IO.Directory.CreateDirectory(@carpetatienda);
                                    }
                                    for (Int32 irar = 0; irar < filesrar.Length; ++irar)
                                    {                                      

                                        verror = "";
                                        string name = System.IO.Path.GetFileNameWithoutExtension(@filesrar[irar].ToString());
                                        string nombrearchivo = System.IO.Path.GetFileName(@filesrar[irar].ToString());
                                        //seleccionar la carpeta dbf para borrar
                                        string[] filesborrar;
                                        filesborrar = System.IO.Directory.GetFiles(@carpetatienda, "*.*");

                                        //borrar archivo de la carpeta dbf de td
                                        for (Int32 iborrar = 0; iborrar < filesborrar.Length; ++iborrar)
                                        {
                                            System.IO.File.Delete(@filesborrar[iborrar].ToString());
                                        }
                                        //ahora descomprimimos los archivos rar                                      
                                        verror = descomprimir(@filesrar[irar].ToString(), @carpetatienda);
                                        
                                        if (verror.Length==0)
                                        {
                                            string _archivo_fac = carpetatienda  +"\\FFACTC.DBF";

                                            //en este verifico si el dbf existe
                                            if (System.IO.File.Exists(@_archivo_fac))
                                            {

                                                if (centidad == "50541")
                                                {
                                                    actualizardata(carpetatienda, centidad, name, ref verror);
                                                }

                                            }

                                            //borrar el archivo zip
                                            if (verror.Length==0)
                                            {
                                                if (centidad == "50541")
                                                {
                                                    if (System.IO.File.Exists(@filesrar[irar].ToString()))
                                                    {
                                                        System.IO.File.Delete(@filesrar[irar].ToString());
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            
                                        }

                                        verror_procesos += verror;
                                    }
                                    //byte[] xml = File.ReadAllBytes(filesrar[irar].ToString());
                                    //Boolean validarar = DescromprimirZip(filesrar[irar].ToString(), carpetatienda);
                                    //UnzipFile(filesrar[irar].ToString(),)
                                        //if (!(validarar))
                                        //{                                        
                                        //    verror = "Error al descomprimir el archivo " + filesrar[irar].ToString();
                                        //    //sb_archivoserror(name, filesrar[irar].ToString(), 1);
                                        //    vejecuta_usp = 0;
                                        //    //return;
                                        //}
                                        //else
                                        //{
                                        //    actualizardata(carpetatd, centidad, name, ref verror);
                                        //    //sb_archivoserror(name, filesrar[irar].ToString(), 2);
                                        //    vejecuta_usp = 1;
                                        //}
                                        //if (verror.Length > 0)
                                        //{
                                        //    if (vejecuta_usp == 1)
                                        //    {
                                        //        //return;
                                        //    }                                       
                                        //}
                                        //else
                                        //{
                                        //    //if (!(Directory.Exists(carpetacopy)))
                                        //    //{
                                        //    //    System.IO.Directory.CreateDirectory(@carpetacopy);
                                        //    //}
                                        //    //string vrutacopy = carpetacopy + nombrearchivo;
                                        //    //if (System.IO.File.Exists(vrutacopy))
                                        //    //{
                                        //    //    System.IO.File.Delete(vrutacopy);
                                        //    //}
                                        //    //System.IO.File.Move(filesrar[irar].ToString(), vrutacopy);
                                        //    //System.IO.File.Delete(filesrar[irar].ToString());
                                        //}                                   
                                    }                              
                            }
                            else
                            {
                                //verror_procesos = carpetatd;
                            }

                        }
                    }
                }


            }
            catch (Exception ex)
            {

                verror_procesos = ex.Message;

            }
        }
        private static string descomprimir(string _rutazip,string _destino)
        {
            string _error = "";
            try
            { 
                FastZip fZip = new FastZip();
                fZip.ExtractZip(@_rutazip, @_destino, "");      
            }
            catch(Exception exc)
            {
                _error = exc.Message;
            }
            return _error;
        }
        private static string UnzipFile(string sourcePath, byte[] gzip)
        {
            string xmlString = "";
            FileStream streamWriter = null;
            try
            {
                Stream stream1 = new MemoryStream(gzip);
                using (ZipInputStream s = new ZipInputStream(stream1))
                {
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {

                        streamWriter = File.OpenWrite(sourcePath);

                        int size = 8192;
                        byte[] data = new byte[8192];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;

                            }
                        }
                        //xmlString = System.Text.ASCIIEncoding.ASCII.GetString(data);
                        streamWriter.Close();

                    }
                }
                //return XElement.Parse(xmlString);
                return xmlString;
            }
            catch (Exception ex)
            {
                streamWriter.Close();
                throw;
            }
        }
        private static DataSet dsgettienda(ref string verror)
        {
            SqlConnection cn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataSet ds = null;
            string sql = "USP_Leer_Tiendas";
            try
            {
                cn = new SqlConnection(conexion);
                cmd = new SqlCommand(sql, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                verror = ex.Message;
                ds = null;
            }
            if (cn.State==ConnectionState.Open) cn.Close();
            return ds;
        }

        private static void actualizardata(string carpeta, string centidad, string name, ref string verror)
        {
            string sqlquery = "[USP_Actualizar_Bata_Parameter]";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataTable dt = null;
            try
            {
                cn = new SqlConnection(conexion);
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CARPETA", carpeta);
                cmd.Parameters.AddWithValue("@C_ENTID", centidad);
                cmd.Parameters.AddWithValue("@NAME", name);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                verror = ex.Message;

                if (verror.Trim().Length == 0) verror = "error del archivo dbf==>" + carpeta + "<==>" + centidad + "<==>" + name;

            }
            if (cn.State==ConnectionState.Open) cn.Close();        
        }

       public static Int32 _get_estado_servicio()
        {
            Int32 _estado_servicio = 0;
            string sqlquery = "USP_Leer_Servicio_Estado";
            SqlConnection cn = null;
            SqlCommand cmd = null;
           try
           {
               cn = new SqlConnection(conexion);
               if (cn.State==0) cn.Open();
               cmd = new SqlCommand(sqlquery, cn);
               cmd.CommandTimeout = 0;
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.Add("@estado_service", SqlDbType.Int);
               cmd.Parameters["@estado_service"].Direction = ParameterDirection.Output;
               cmd.ExecuteNonQuery();
               _estado_servicio = Convert.ToInt32(cmd.Parameters["@estado_service"].Value);
           }
           catch
           {
               _estado_servicio = 0;
           }
           if (cn.State == ConnectionState.Open) cn.Close();
           return _estado_servicio;
        }
       public static void insertar_error_service(string _error)
       {
           string sqlquery = "USP_Insertar_Errores_Service";
           SqlConnection cn = null;
           SqlCommand cmd = null;
           try
           {
               cn = new SqlConnection(conexion);
               if (cn.State==0) cn.Open();
               cmd = new SqlCommand(sqlquery, cn);
               cmd.CommandTimeout = 0;
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.AddWithValue("@error",_error);
               cmd.ExecuteNonQuery();
           }
           catch
           {

           }
           if (cn.State==ConnectionState.Open) cn.Close();
       }
       public static void actualiza_servicio(Int32 _valor_service)
        {
            string sqlquery = "USP_Actualizar_ServiceWin";
            SqlConnection cn = null;
            SqlCommand cmd = null;
           try
           {
               cn = new SqlConnection(conexion);
               if (cn.State==0) cn.Open();
               cmd = new SqlCommand(sqlquery, cn);
               cmd.CommandTimeout = 0;
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.AddWithValue("@valor_service",_valor_service);
               cmd.ExecuteNonQuery();
           }
           catch
           {
           }
           if (cn.State==ConnectionState.Open) cn.Close();
        }
        #endregion
        #region<METODO DE CONEXION>
        static  string conexion = "Server=10.10.10.208;Database=BdTienda;UID=sa;Password=Bata2013";
        #endregion

        


    }
}
