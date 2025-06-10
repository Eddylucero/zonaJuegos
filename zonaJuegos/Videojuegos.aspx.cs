using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

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
                ddlPlataformas.DataValueField = "Id";
                ddlPlataformas.DataBind();
            }
        }

        protected void gvVideojuegos_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int idVideojuego = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditarVideojuego")
            {
                CargarDatosVideojuego(idVideojuego);
                hdnVideojuegoID.Value = idVideojuego.ToString();

                lblFormularioTitulo.Text = "Editar Videojuego";
                btnGuardarVideojuego.Text = "Actualizar Videojuego";
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

                    ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert",
                        "Swal.fire({ title: '¡Eliminado!', text: 'VIDEOJUEGO ELIMINADO CORRECTAMENTE', icon: 'success' });", true);

                    CargarVideojuegos();
                }
            }
        }

        private void CargarDatosVideojuego(int idVideojuego)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Videojuego WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", idVideojuego);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtTitulo.Text = reader["Titulo"].ToString();
                    txtGenero.Text = reader["Genero"].ToString();
                    txtDesarrollador.Text = reader["Desarrollador"].ToString();
                    txtFechaLanzamiento.Text = Convert.ToDateTime(reader["FechaLanzamiento"]).ToString("yyyy-MM-dd");
                    txtClasificacion.Text = reader["Clasificacion"].ToString();
                    ddlPlataformas.SelectedValue = reader["PlataformaId"].ToString();
                }

                conn.Close();
            }
        }
        public bool ValidarVideojuego()
        {
            lblMensajeVideojuego.Text = "";

            // Validar que el título no esté vacío
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                lblMensajeVideojuego.Text = "⚠ Error: El título del videojuego está vacío.";
                return false;
            }

            // Expresión regular para validar solo letras y números en el título
            string SoloLetrasYNumeros = @"^[A-Za-z0-9\s]+$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtTitulo.Text, SoloLetrasYNumeros))
            {
                lblMensajeVideojuego.Text = "⚠ Error: El título solo debe contener letras y números.";
                return false;
            }

            // Validar género
            if (string.IsNullOrWhiteSpace(txtGenero.Text))
            {
                lblMensajeVideojuego.Text = "⚠ Error: Debes ingresar un género.";
                return false;
            }

            // Validar desarrollador
            if (string.IsNullOrWhiteSpace(txtDesarrollador.Text))
            {
                lblMensajeVideojuego.Text = "⚠ Error: Debes ingresar el desarrollador.";
                return false;
            }

            // Validar fecha de lanzamiento
            if (!DateTime.TryParse(txtFechaLanzamiento.Text, out DateTime fecha) || fecha > DateTime.Now)
            {
                lblMensajeVideojuego.Text = "⚠ Error: La fecha de lanzamiento no puede ser mayor a la actual.";
                return false;
            }

            // Validar clasificación
            if (string.IsNullOrWhiteSpace(txtClasificacion.Text))
            {
                lblMensajeVideojuego.Text = "⚠ Error: Debes ingresar la clasificación.";
                return false;
            }

            // Validar selección de plataforma
            if (ddlPlataformas.SelectedIndex == -1)
            {
                lblMensajeVideojuego.Text = "⚠ Error: Debes seleccionar una plataforma.";
                return false;
            }

            return true;
        }
        protected void btnGuardarVideojuego_Click(object sender, EventArgs e)
        {
            if (!ValidarVideojuego()) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query;
                bool esNuevo = string.IsNullOrEmpty(hdnVideojuegoID.Value);

                if (esNuevo)
                {
                    query = "EXEC sp_InsertarVideojuego @Titulo, @Genero, @Desarrollador, @FechaLanzamiento, @Clasificacion, @PlataformaId";
                }
                else
                {
                    query = "EXEC sp_ActualizarVideojuego @Id, @Titulo, @Genero, @Desarrollador, @FechaLanzamiento, @Clasificacion, @PlataformaId";
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Titulo", txtTitulo.Text);
                cmd.Parameters.AddWithValue("@Genero", txtGenero.Text);
                cmd.Parameters.AddWithValue("@Desarrollador", txtDesarrollador.Text);
                cmd.Parameters.AddWithValue("@FechaLanzamiento", txtFechaLanzamiento.Text);
                cmd.Parameters.AddWithValue("@Clasificacion", txtClasificacion.Text);
                cmd.Parameters.AddWithValue("@PlataformaId", ddlPlataformas.SelectedValue);

                if (!esNuevo)
                {
                    cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(hdnVideojuegoID.Value));
                }

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                string mensaje = esNuevo ? "VIDEOJUEGO AGREGADO CORRECTAMENTE" : "VIDEOJUEGO EDITADO CORRECTAMENTE";
                ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert",
                    $"Swal.fire({{ title: '¡Éxito!', text: '{mensaje}', icon: 'success' }});", true);

                LimpiarFormulario();
                CargarVideojuegos();
            }

        }

        private void LimpiarFormulario()
        {
            hdnVideojuegoID.Value = "";
            txtTitulo.Text = "";
            txtGenero.Text = "";
            txtDesarrollador.Text = "";
            txtFechaLanzamiento.Text = "";
            txtClasificacion.Text = "";
            ddlPlataformas.SelectedIndex = 0;

            lblFormularioTitulo.Text = "Agregar Videojuego";
            btnGuardarVideojuego.Text = "Agregar Videojuego";
        }

        
    }
}
