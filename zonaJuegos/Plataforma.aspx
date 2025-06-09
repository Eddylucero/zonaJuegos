<%@ Page Title="Plataformas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Plataforma.aspx.cs" Inherits="zonaJuegos.Plataforma" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h1 class="text-center">Gestión de Plataformas</h1>

    <div class="container mt-4 text-center">
        <asp:HiddenField ID="hdnPlataformaID" runat="server" />
    </div>

    <div class="container d-flex justify-content-center mt-4" style="max-width: 500px;">
        <div>
            <h3 class="text-center">Agregar Plataforma</h3>
            <asp:TextBox ID="txtNombrePlataforma" runat="server" CssClass="form-control" placeholder="Nombre de la plataforma" />

            <asp:Button ID="btnAgregarPlataforma" runat="server" Text="Agregar Plataforma" CssClass="btn btn-primary mt-3 w-100" OnClick="btnAgregarPlataforma_Click" />
        
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

                <asp:TemplateField HeaderText="Opciones">
                    <ItemTemplate>
                        <asp:Button runat="server" Text="✏️ Editar" CssClass="btn form-control-sm btn-primary" CommandName="EditarPlataforma" CommandArgument='<%# Eval("ID") %>' />
                        <asp:Button runat="server" Text="🗑️ Eliminar" CssClass="btn form-control-sm btn-danger" CommandName="EliminarPlataforma" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('¿Estás seguro de que quieres eliminar esta plataforma?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
