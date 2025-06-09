using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;

namespace zonaJuegos
{
    public partial class Videojuegos : System.Web.UI.Page
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
                CargarVideojuegos();
                CargarPlataformas();
            }
        }

        // Método para cargar videojuegos en el GridView
        private void CargarVideojuegos()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "EXEC sp_ObtenerVideojuegos";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvVideojuegos.DataSource = dt;
                gvVideojuegos.DataBind();
            }
        }

        // Método para cargar plataformas en el DropDownList
        private void CargarPlataformas()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "EXEC sp_ObtenerPlataformas";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlPlataformas.DataSource = dt;
                ddlPlataformas.DataTextField = "Nombre";
                ddlPlataformas.DataValueField = "ID";
                ddlPlataformas.DataBind();
            }
        }

        // Método de validación para agregar un videojuego
        public bool ValidarVideojuego()
        {
            lblMensajeVideojuego.Text = "";

            // Expresión regular para permitir solo letras, números y espacios
            string SoloLetrasYNumeros = @"^[A-Za-z0-9\s]+$";

            // Validar que el nombre no esté vacío
            if (string.IsNullOrWhiteSpace(txtNombreVideojuego.Text))
            {
                lblMensajeVideojuego.Text = "⚠ Error: El nombre del videojuego está vacío.";
                return false;
            }

            // Validar que el nombre solo contenga letras y números
            if (!Regex.IsMatch(txtNombreVideojuego.Text, SoloLetrasYNumeros))
            {
                lblMensajeVideojuego.Text = "⚠ Error: El nombre solo debe contener letras y números.";
                return false;
            }

            // Validar que el desarrollador no esté vacío
            if (string.IsNullOrWhiteSpace(txtDesarrollador.Text))
            {
                lblMensajeVideojuego.Text = "⚠ Error: El desarrollador no puede estar vacío.";
                return false;
            }

            // Validar que la fecha sea válida y no mayor a la actual
            if (!DateTime.TryParse(txtFechaLanzamiento.Text, out DateTime fecha) || fecha > DateTime.Now)
            {
                lblMensajeVideojuego.Text = "⚠ Error: La fecha de lanzamiento no puede ser mayor a la actual.";
                return false;
            }

            // Validar que el género no esté vacío
            if (string.IsNullOrWhiteSpace(txtGenero.Text))
            {
                lblMensajeVideojuego.Text = "⚠ Error: El género no puede estar vacío.";
                return false;
            }

            // Validar que la clasificación no esté vacía
            if (string.IsNullOrWhiteSpace(txtClasificacion.Text))
            {
                lblMensajeVideojuego.Text = "⚠ Error: La clasificación no puede estar vacía.";
                return false;
            }

            // Validar precio como número positivo
            if (string.IsNullOrWhiteSpace(txtPrecio.Text) || !decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
            {
                lblMensajeVideojuego.Text = "⚠ Error: El precio debe ser un número válido mayor a 0.";
                return false;
            }

            return true;
        }

        // Método para agregar un nuevo videojuego
        protected void btnAgregarVideojuego_Click(object sender, EventArgs e)
        {
            if (!ValidarVideojuego()) return; // Si la validación falla, detener el proceso

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "EXEC sp_InsertarVideojuego @Nombre, @Desarrollador, @FechaLanzamiento, @Genero, @Clasificacion, @Precio";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", txtNombreVideojuego.Text);
                cmd.Parameters.AddWithValue("@Desarrollador", txtDesarrollador.Text);
                cmd.Parameters.AddWithValue("@FechaLanzamiento", txtFechaLanzamiento.Text);
                cmd.Parameters.AddWithValue("@Genero", txtGenero.Text);
                cmd.Parameters.AddWithValue("@Clasificacion", txtClasificacion.Text);
                cmd.Parameters.AddWithValue("@Precio", txtPrecio.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                lblMensajeVideojuego.Text = "✅ ¡Videojuego agregado correctamente!";
                CargarVideojuegos();
            }
        }

        // Método para manejar las acciones en el GridView
        protected void gvVideojuegos_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int idVideojuego = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditarVideojuego")
            {
                txtNombreVideojuego.Text = ObtenerNombreVideojuego(idVideojuego);
                hdnVideojuegoID.Value = idVideojuego.ToString();
            }
            else if (e.CommandName == "EliminarVideojuego")
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "EXEC sp_EliminarVideojuego @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", idVideojuego);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    CargarVideojuegos();
                }
            }
        }

        // Método para obtener el nombre de un videojuego por su ID
        private string ObtenerNombreVideojuego(int idVideojuego)
        {
            string nombre = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Nombre FROM Videojuego WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", idVideojuego);

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
