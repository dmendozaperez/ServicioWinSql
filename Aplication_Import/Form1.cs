using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Basico;
namespace Aplication_Import
{
    public partial class Form1 : Form
    {
        private Int32 _valida_service = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string _error_tarea = "";
            Int32 _valor = 0;
            try
            {
                //verificarsi es el servicio se esta ejeutando
                //if (Importar_Data._get_estado_servicio()==0)
                //{
                if (_valida_service == 0)
                {
                    _valor = 1;
                    _valida_service = 1;
                    //activar servicio
                    //Importar_Data.actualiza_servicio(1);                   
                    string _error = "";
                    //ejecutar la importacion de data
                    Cursor.Current = Cursors.WaitCursor;
                    Importar_Data.ejecutatarea(ref _error, ref _error_tarea);

                    MessageBox.Show(_error_tarea, "aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Cursor.Current = Cursors.Default;
                    //********************
                    //si es que hay un error entonces grabamos el error en tabla del sql
                    if (_error_tarea.Trim().Length > 0)
                    {
                        //Importar_Data.insertar_error_service(_error_tarea);
                    }

                    //una vez se haya realizado las importaciones
                    //setear la tabla en cero
                    _valida_service = 0;
                    //Importar_Data.actualiza_servicio(0);                   
                }
                //****************************************************************************

            }
            catch (Exception ex)
            {
                _valida_service = 0;
                _error_tarea += "===>>" + ex.Message;
                if (_error_tarea.Trim().Length > 0)
                {
                    //Importar_Data.insertar_error_service(_error_tarea);
                }

                if (_valor == 1)
                {
                    //if (System.IO.File.Exists(varchivov))
                    //{
                    _valida_service = 0;
                    //System.IO.File.Delete(varchivov);
                    //}   
                }
                //Importar_Data.actualiza_servicio(0);                   
            }
        }
    }
}
