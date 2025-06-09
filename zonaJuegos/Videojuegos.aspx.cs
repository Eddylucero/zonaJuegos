using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace zonaJuegos
{
    public partial class Videojuegos : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conexionejercicio"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarVideojuegos();
                CargarPlataformas(); // Opcional si decides incluir plataformas
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

        // Método para agregar un nuevo videojuego
        protected void btnAgregarVideojuego_Click(object sender, EventArgs e)
        {
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

                lblMensajeVideojuego.Text = "¡Videojuego agregado correctamente!";
                CargarVideojuegos(); // Refrescar la lista
            }
        }

        // Método para manejar las acciones en el GridView
        protected void gvVideojuegos_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int idVideojuego = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditarVideojuego")
            {
                // Aquí puedes cargar los datos del videojuego para editarlos
                // Por ejemplo, llenar los TextBox con los datos actuales
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
    }
}
