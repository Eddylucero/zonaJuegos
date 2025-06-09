using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace zonaJuegos
{
    public partial class Plataforma : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conexionejercicio"]?.ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Error: La cadena de conexión 'conexionejercicio' no está definida en Web.config.");
            }

            if (!IsPostBack)
            {
                CargarPlataformas();
            }
        }

        // Método para cargar plataformas en el GridView
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

        // Método de validación antes de insertar o actualizar una plataforma
        public bool ValidarPlataforma()
        {
            lblMensajePlataforma.Text = "";

            // Expresión regular para validar solo letras y números
            string SoloLetrasYNumeros = @"^[A-Za-z0-9\s]+$";

            // Validar que el nombre no esté vacío
            if (string.IsNullOrWhiteSpace(txtNombrePlataforma.Text))
            {
                lblMensajePlataforma.Text = "⚠ Error: El nombre de la plataforma está vacío.";
                return false;
            }

            // Validar formato de nombre
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtNombrePlataforma.Text, SoloLetrasYNumeros))
            {
                lblMensajePlataforma.Text = "⚠ Error: El nombre solo debe contener letras y números.";
                return false;
            }

            return true;
        }

        // Método para agregar o editar una plataforma
        protected void btnGuardarPlataforma_Click(object sender, EventArgs e)
        {
            if (!ValidarPlataforma()) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query;

                if (string.IsNullOrEmpty(hdnPlataformaID.Value)) // Si no hay ID, es un nuevo registro
                {
                    query = "EXEC sp_InsertarPlataforma @Nombre";
                }
                else // Si hay un ID, estamos en modo edición
                {
                    query = "EXEC sp_ActualizarPlataforma @Id, @Nombre";
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", txtNombrePlataforma.Text);

                if (!string.IsNullOrEmpty(hdnPlataformaID.Value))
                {
                    cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(hdnPlataformaID.Value));
                }

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                lblMensajePlataforma.Text = "✅ ¡Plataforma guardada correctamente!";
                hdnPlataformaID.Value = ""; // Limpiar el campo de ID después de guardar
                txtNombrePlataforma.Text = ""; // Limpiar el campo de nombre
                lblFormularioTitulo.Text = "Agregar Plataforma"; // Volver al modo agregar
                btnGuardarPlataforma.Text = "Agregar Plataforma";

                CargarPlataformas();
            }
        }

        // Método para manejar las acciones en el GridView
        protected void gvPlataformas_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int idPlataforma = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditarPlataforma")
            {
                txtNombrePlataforma.Text = ObtenerNombrePlataforma(idPlataforma);
                hdnPlataformaID.Value = idPlataforma.ToString();

                lblFormularioTitulo.Text = "Editar Plataforma";
                btnGuardarPlataforma.Text = "Actualizar Plataforma";
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
                }
            }
        }

        // Método para obtener el nombre de una plataforma por su ID
        private string ObtenerNombrePlataforma(int idPlataforma)
        {
            string nombre = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Nombre FROM dbo.Plataforma WHERE Id = @Id";
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
