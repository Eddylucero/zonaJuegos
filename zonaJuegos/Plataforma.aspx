<%@ Page Title="Gestión de Plataformas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Plataforma.aspx.cs" Inherits="zonaJuegos.Plataforma" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script>
        function confirmarEliminacion(btn) {
            event.preventDefault();

            Swal.fire({
                title: "¿Estás seguro?",
                text: "Esta acción no se puede deshacer.",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminar",
                cancelButtonText: "Cancelar"
            }).then((result) => {
                if (result.isConfirmed) {
                    __doPostBack(btn.name, '');
                }
            });

            return false;
        }
    </script>

    <h1 class="text-center">
        <asp:Label ID="lblTitulo" runat="server" Text="Gestión de Plataformas"></asp:Label>
    </h1>

    <div class="container mt-4 text-center">
        <asp:HiddenField ID="hdnPlataformaID" runat="server" />
    </div>

    <div class="container d-flex justify-content-center mt-4" style="max-width: 500px;">
        <div>
            <h3 class="text-center">
                <asp:Label ID="lblFormularioTitulo" runat="server" Text="Agregar Plataforma"></asp:Label>
            </h3>

            <label>Nombre de la plataforma:</label>
            <asp:TextBox ID="txtNombrePlataforma" runat="server" CssClass="form-control" />

            <label>Fabricante:</label>
            <asp:TextBox ID="txtFabricante" runat="server" CssClass="form-control mt-2" />

            <label>Año de Lanzamiento:</label>
            <asp:TextBox ID="txtAnioLanzamiento" runat="server" CssClass="form-control mt-2" TextMode="Number" />

            <label>Tipo de Plataforma:</label>
            <asp:TextBox ID="txtTipo" runat="server" CssClass="form-control mt-2" />

            <label>Región Disponible:</label>
            <asp:TextBox ID="txtRegionDisponible" runat="server" CssClass="form-control mt-2" />

            <asp:Button ID="btnGuardarPlataforma" runat="server" Text="Agregar Plataforma" CssClass="btn btn-primary mt-3 w-100" OnClick="btnGuardarPlataforma_Click" />

            <br />
            <asp:Label ID="lblMensajePlataforma" runat="server" CssClass="text-error mt-2"></asp:Label>
        </div>
    </div>

    <div class="container mt-5">
        <h3 class="text-center">Lista de Plataformas</h3>

        <asp:GridView ID="gvPlataformas" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" DataKeyNames="ID" OnRowCommand="gvPlataformas_RowCommand">
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="ID" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Fabricante" HeaderText="Fabricante" />
                <asp:BoundField DataField="AnioLanzamiento" HeaderText="Año de Lanzamiento" />
                <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                <asp:BoundField DataField="RegionDisponible" HeaderText="Región Disponible" />

                <asp:TemplateField HeaderText="Opciones">
                    <ItemTemplate>
                        <asp:Button runat="server" Text="✏️ Editar" CssClass="btn form-control-sm btn-primary" CommandName="EditarPlataforma" CommandArgument='<%# Eval("ID") %>' />
                        <asp:Button runat="server" Text="🗑️ Eliminar" CssClass="btn form-control-sm btn-danger"
                            CommandName="EliminarPlataforma" CommandArgument='<%# Eval("ID") %>' 
                            OnClientClick="return confirmarEliminacion(this);" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
