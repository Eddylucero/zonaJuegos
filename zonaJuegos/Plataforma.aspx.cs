using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace zonaJuegos
{
    public partial class Plataforma : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conexionejercicio"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
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

        // Método para agregar una nueva plataforma
        protected void btnAgregarPlataforma_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "EXEC sp_InsertarPlataforma @Nombre";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", txtNombrePlataforma.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                lblMensajePlataforma.Text = "¡Plataforma agregada correctamente!";
                CargarPlataformas(); // Refrescar lista
            }
        }

        // Método para manejar las acciones en el GridView
        protected void gvPlataformas_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int idPlataforma = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditarPlataforma")
            {
                // Aquí podrías cargar la información de la plataforma en un TextBox para editarla
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
    }
}
