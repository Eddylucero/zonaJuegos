<%@ Page Title="Gestión de Videojuegos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Videojuegos.aspx.cs" Inherits="zonaJuegos.Videojuegos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <script>
        function confirmarEliminacion(id) {
            Swal.fire({
                title: "¿Estás seguro?",
                text: "Esta acción no se puede deshacer.",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminar",
                cancelButtonText: "Cancelar"
            }).then((result) => {
                if (result.isConfirmed) {
                    __doPostBack('EliminarVideojuego', id);  // Debe coincidir con CommandName
                }
            });
        }
    </script>

    <h1 class="text-center">
        <asp:Label ID="lblTitulo" runat="server" Text="Gestión de Videojuegos"></asp:Label>
    </h1>

    <div class="container mt-4 text-center">
        <asp:HiddenField ID="hdnVideojuegoID" runat="server" />
    </div>

    <div class="container d-flex justify-content-center mt-4" style="max-width: 500px;">
        <div>
            <h3 class="text-center">
                <asp:Label ID="lblFormularioTitulo" runat="server" Text="Agregar Videojuego"></asp:Label>
            </h3>

            <asp:TextBox ID="txtTitulo" runat="server" CssClass="form-control" placeholder="Título del videojuego" />
            <asp:TextBox ID="txtGenero" runat="server" CssClass="form-control mt-2" placeholder="Género" />
            <asp:TextBox ID="txtDesarrollador" runat="server" CssClass="form-control mt-2" placeholder="Desarrollador" />
            <asp:TextBox ID="txtFechaLanzamiento" runat="server" CssClass="form-control mt-2" placeholder="Fecha de Lanzamiento (YYYY-MM-DD)" TextMode="Date" />
            <asp:TextBox ID="txtClasificacion" runat="server" CssClass="form-control mt-2" placeholder="Clasificación" />

            <label class="mt-2">Plataforma</label>
            <asp:DropDownList ID="ddlPlataformas" runat="server" CssClass="form-control"></asp:DropDownList>

            <asp:Button ID="btnGuardarVideojuego" runat="server" Text="Agregar Videojuego" CssClass="btn btn-primary mt-3 w-100" OnClick="btnGuardarVideojuego_Click" />

            <br />
            <asp:Label ID="lblMensajeVideojuego" runat="server" CssClass="text-error mt-2"></asp:Label>
        </div>
    </div>

    <div class="container mt-5">
        <h3 class="text-center">Lista de Videojuegos</h3>

        <asp:GridView ID="gvVideojuegos" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" DataKeyNames="ID" OnRowCommand="gvVideojuegos_RowCommand">
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="ID" />
                <asp:BoundField DataField="Titulo" HeaderText="Título" />
                <asp:BoundField DataField="Genero" HeaderText="Género" />
                <asp:BoundField DataField="Desarrollador" HeaderText="Desarrollador" />
                <asp:BoundField DataField="FechaLanzamiento" HeaderText="Fecha de Lanzamiento" />
                <asp:BoundField DataField="Clasificacion" HeaderText="Clasificación" />
                <asp:BoundField DataField="Plataforma" HeaderText="Plataforma" />

                <asp:TemplateField HeaderText="Opciones">
                    <ItemTemplate>
                        <asp:Button runat="server" Text="✏️ Editar" CssClass="btn form-control-sm btn-primary" CommandName="EditarVideojuego" CommandArgument='<%# Eval("ID") %>' />
                        <asp:Button runat="server" Text="🗑️ Eliminar" CssClass="btn form-control-sm btn-danger"
                            CommandName="EliminarVideojuego" CommandArgument='<%# Eval("ID") %>' 
                            OnClientClick='<%# "confirmarEliminacion(" + Eval("ID") + "); return false;" %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
