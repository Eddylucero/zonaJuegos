<%@ Page Title="Videojuegos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Videojuegos.aspx.cs" Inherits="zonaJuegos.Videojuegos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h1 class="text-center">Gestión de Videojuegos</h1>

    <div class="container mt-4 text-center">
        <asp:HiddenField ID="hdnVideojuegoID" runat="server" />
    </div>

    <div class="container d-flex justify-content-center mt-4" style="max-width: 500px;">
        <div>
            <h3 class="text-center">Agregar Videojuego</h3>
            <asp:TextBox ID="txtNombreVideojuego" runat="server" CssClass="form-control" placeholder="Nombre del videojuego" />
            <asp:TextBox ID="txtDesarrollador" runat="server" CssClass="form-control mt-2" placeholder="Desarrollador" />
            <asp:TextBox ID="txtFechaLanzamiento" runat="server" CssClass="form-control mt-2" placeholder="Fecha de Lanzamiento" TextMode="Date" />
            <asp:TextBox ID="txtGenero" runat="server" CssClass="form-control mt-2" placeholder="Género" />
            <asp:TextBox ID="txtClasificacion" runat="server" CssClass="form-control mt-2" placeholder="Clasificación" />
            <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control mt-2" placeholder="Precio" TextMode="Number" />

            <asp:DropDownList ID="ddlPlataformas" runat="server" CssClass="form-control mt-2" Visible="true" />

            <asp:Button ID="btnAgregarVideojuego" runat="server" Text="Agregar Videojuego" CssClass="btn btn-primary mt-3 w-100" OnClick="btnAgregarVideojuego_Click" />
        
            <br />
            <asp:Label ID="lblMensajeVideojuego" runat="server" CssClass="text-error mt-2"></asp:Label>
        </div>
    </div>

    <div class="container mt-5">
        <h3 class="text-center">Lista de Videojuegos</h3>
    
        <asp:GridView ID="gvVideojuegos" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" DataKeyNames="ID" OnRowCommand="gvVideojuegos_RowCommand">
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="ID" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Desarrollador" HeaderText="Desarrollador" />
                <asp:BoundField DataField="FechaLanzamiento" HeaderText="Fecha de Lanzamiento" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="Genero" HeaderText="Género" />
                <asp:BoundField DataField="Clasificacion" HeaderText="Clasificación" />
                <asp:BoundField DataField="Precio" HeaderText="Precio" />

                <asp:TemplateField HeaderText="Opciones">
                    <ItemTemplate>
                        <asp:Button runat="server" Text="✏️" CssClass="btn form-control-sm btn-primary" CommandName="EditarVideojuego" CommandArgument='<%# Eval("ID") %>' />
                        <asp:Button runat="server" Text="🗑️" CssClass="btn form-control-sm btn-danger" CommandName="EliminarVideojuego" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('¿Estás seguro de que quieres eliminar este videojuego?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
