using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

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

            // Expresión regular para validar solo letras y números en el nombre
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

            // Validar que el fabricante no esté vacío
            if (string.IsNullOrWhiteSpace(txtFabricante.Text))
            {
                lblMensajePlataforma.Text = "⚠ Error: Debes ingresar un fabricante.";
                return false;
            }

            // Validar que el año de lanzamiento sea válido
            if (!int.TryParse(txtAnioLanzamiento.Text, out int anio) || anio < 1950 || anio > DateTime.Now.Year)
            {
                lblMensajePlataforma.Text = "⚠ Error: El año de lanzamiento no es válido.";
                return false;
            }

            // Validar que el tipo de plataforma no esté vacío
            if (string.IsNullOrWhiteSpace(txtTipo.Text))
            {
                lblMensajePlataforma.Text = "⚠ Error: Debes ingresar el tipo de plataforma.";
                return false;
            }

            // Validar que la región disponible no esté vacía
            if (string.IsNullOrWhiteSpace(txtRegionDisponible.Text))
            {
                lblMensajePlataforma.Text = "⚠ Error: Debes ingresar la región donde está disponible.";
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
                    query = "EXEC sp_InsertarPlataforma @Nombre, @Fabricante, @AnioLanzamiento, @Tipo, @RegionDisponible";
                }
                else // Si hay un ID, estamos en modo edición
                {
                    query = "EXEC sp_ActualizarPlataforma @Id, @Nombre, @Fabricante, @AnioLanzamiento, @Tipo, @RegionDisponible";
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", txtNombrePlataforma.Text);
                cmd.Parameters.AddWithValue("@Fabricante", txtFabricante.Text);
                cmd.Parameters.AddWithValue("@AnioLanzamiento", txtAnioLanzamiento.Text);
                cmd.Parameters.AddWithValue("@Tipo", txtTipo.Text);
                cmd.Parameters.AddWithValue("@RegionDisponible", txtRegionDisponible.Text);

                if (!string.IsNullOrEmpty(hdnPlataformaID.Value))
                {
                    cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(hdnPlataformaID.Value));
                }

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert",
                    "Swal.fire({ title: '¡Éxito!', text: 'Videojuego guardado correctamente.', icon: 'success' });", true);
                hdnPlataformaID.Value = ""; // Limpiar el campo de ID después de guardar
                txtNombrePlataforma.Text = ""; // Limpiar el campo de nombre
                txtFabricante.Text = "";
                txtAnioLanzamiento.Text = "";
                txtTipo.Text = "";
                txtRegionDisponible.Text = "";

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
                CargarDatosPlataforma(idPlataforma);
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

        // Método para cargar los datos de una plataforma seleccionada
        private void CargarDatosPlataforma(int idPlataforma)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Plataforma WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", idPlataforma);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtNombrePlataforma.Text = reader["Nombre"].ToString();
                    txtFabricante.Text = reader["Fabricante"].ToString();
                    txtAnioLanzamiento.Text = reader["AnioLanzamiento"].ToString();
                    txtTipo.Text = reader["Tipo"].ToString();
                    txtRegionDisponible.Text = reader["RegionDisponible"].ToString();
                }

                conn.Close();
            }
        }
    }
}
