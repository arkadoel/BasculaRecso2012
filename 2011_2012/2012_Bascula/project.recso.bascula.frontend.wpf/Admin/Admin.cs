using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace project.recso.bascula.frontend.wpf.Admin
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
        }

        private void usuariosBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.usuariosBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.recso2011DBDataSet);

        }

        private void Admin_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'recso2011DBDataSet.Usuarios' table. You can move, or remove it, as needed.
            this.usuariosTableAdapter.Fill(this.recso2011DBDataSet.Usuarios);

            numericAlbaran.Value = logic.gestionConfiguracionApp.getUltimoAlbaran();

           
        }

        private void numericAlbaran_ValueChanged(object sender, EventArgs e)
        {
            logic.gestionConfiguracionApp.setUltimoAlbaran(long.Parse( numericAlbaran.Value.ToString()));
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //cambiar contraseña del usuario actual
            if (usuariosDataGridView.SelectedRows.Count > 0)
            {
                //coger loginname

                String loginName = usuariosDataGridView.SelectedRows[0].Cells[0].Value.ToString();
                String nuevoPass = Microsoft.VisualBasic.Interaction.InputBox("Escriba la nueva contraseña para el usuario: " + loginName, "Cambiar contraseña");
                String rePass = Microsoft.VisualBasic.Interaction.InputBox("Repita la nueva contraseña para el usuario: " + loginName, "Cambiar contraseña");

                if (nuevoPass == rePass)
                {
                   Boolean cambiado= logic.gestionUsuarios.cambiarPassword(loginName, nuevoPass);

                   if (cambiado == true)
                   {
                       MessageBox.Show("La contraseña se ha cambiado con exito", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   }
                }
                else MessageBox.Show("Las contraseñas no coinciden, intentelo de nuevo.", "Fallo al cambiar la contraseña.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
