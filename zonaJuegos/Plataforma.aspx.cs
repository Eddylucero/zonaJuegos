using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;

namespace zonaJuegos
{
    public partial class Plataforma : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conexionejercicio"]?.ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("❌ Error: La cadena de conexión 'conexionejercicio' no está definida en Web.config.");
            }

            if (!IsPostBack)
            {
                CargarPlataformas();
                ReiniciarFormulario();
            }
        }

        // Cargar todas las plataformas en el GridView
        private void CargarPlataformas()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "EXEC sp_ObtenerPlataformas";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvPlataformas.DataSource = dt;
                gvPlataformas.DataBind();
            }
        }

        // Validación de nombre de plataforma
        private bool ValidarPlataforma()
        {
            lblMensajePlataforma.Text = "";

            string soloLetrasYNumeros = @"^[A-Za-z0-9\s]+$";

            if (string.IsNullOrWhiteSpace(txtNombrePlataforma.Text))
            {
                lblMensajePlataforma.Text = "⚠ Error: El nombre de la plataforma está vacío.";
                return false;
            }

            if (!Regex.IsMatch(txtNombrePlataforma.Text, soloLetrasYNumeros))
            {
                lblMensajePlataforma.Text = "⚠ Error: El nombre solo debe contener letras y números.";
                return false;
            }

            return true;
        }

        // Botón para agregar o actualizar una plataforma
        protected void btnGuardarPlataforma_Click(object sender, EventArgs e)
        {
            if (!ValidarPlataforma()) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query;
                SqlCommand cmd;

                if (string.IsNullOrEmpty(hdnPlataformaID.Value))
                {
                    // Insertar nueva plataforma
                    query = "EXEC sp_InsertarPlataforma @Nombre";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nombre", txtNombrePlataforma.Text);
                }
                else
                {
                    // Actualizar plataforma existente
                    query = "EXEC sp_ActualizarPlataforma @Id, @Nombre";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(hdnPlataformaID.Value));
                    cmd.Parameters.AddWithValue("@Nombre", txtNombrePlataforma.Text);
                }

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                lblMensajePlataforma.Text = "✅ Plataforma guardada correctamente.";
                ReiniciarFormulario();
                CargarPlataformas();
            }
        }

        // Reinicia los controles del formulario
        private void ReiniciarFormulario()
        {
            hdnPlataformaID.Value = "";
            txtNombrePlataforma.Text = "";
            lblFormularioTitulo.Text = "Agregar Plataforma";
            btnGuardarPlataforma.Text = "Agregar Plataforma";
        }

        // Acciones del GridView (editar o eliminar)
        protected void gvPlataformas_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int idPlataforma = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditarPlataforma")
            {
                string nombre = ObtenerNombrePlataforma(idPlataforma);
                txtNombrePlataforma.Text = nombre;
                hdnPlataformaID.Value = idPlataforma.ToString();

                lblFormularioTitulo.Text = "Editar Plataforma";
                btnGuardarPlataforma.Text = "Actualizar Plataforma";
                lblMensajePlataforma.Text = "";
            }
            else if (e.CommandName == "EliminarPlataforma")
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "EXEC sp_EliminarPlataforma @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", idPlataforma);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    CargarPlataformas();
                    lblMensajePlataforma.Text = "✅ Plataforma eliminada correctamente.";
                    ReiniciarFormulario();
                }
            }
        }

        // Obtener el nombre de una plataforma por su ID
        private string ObtenerNombrePlataforma(int idPlataforma)
        {
            string nombre = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Nombre FROM Plataformas WHERE ID = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", idPlataforma);

                conn.Open();
                object result = cmd.ExecuteScalar();
                conn.Close();

                if (result != null)
                {
                    nombre = result.ToString();
                }
            }
            return nombre;
        }
    }
}
