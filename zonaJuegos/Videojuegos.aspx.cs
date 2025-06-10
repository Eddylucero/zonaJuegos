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
