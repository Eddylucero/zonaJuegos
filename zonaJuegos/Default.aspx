<%@ Page Title="Zona Juegos - Inicio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="zonaJuegos._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    
    <div class="jumbotron text-center">

        <h1>Bienvenido a Zona Juegos</h1>
        <p>Gestiona videojuegos y plataformas de manera eficiente y organizada.</p>

        <div class="d-flex justify-content-center gap-3">
            <a class="btn btn-primary btn-lg" runat="server" href="~/Videojuegos"><i class="fas fa-gamepad"></i> Gestionar Videojuegos</a>
            <a class="btn btn-warning btn-lg" runat="server" href="~/Plataforma"><i class="fas fa-dice"></i> Gestionar Plataformas</a>
        </div>
    </div>

    <div class="container text-center mt-4">
        <h2>¿Cómo funciona?</h2>
        <p>Administra videojuegos, organiza plataformas y optimiza la gestión con nuestro sistema.</p>
        <div class="row mt-4">
            <div class="col-md-6">
                <i class="fas fa-list-alt fa-3x text-primary mb-3"></i>
                <h4>Organización Eficiente</h4>
                <p>Clasifica y gestiona videojuegos en una interfaz intuitiva.</p>
            </div>
            <div class="col-md-6">
                <i class="fas fa-cogs fa-3x text-warning mb-3"></i>
                <h4>Plataformas Conectadas</h4>
                <p>Administra tus plataformas favoritas y mejora la gestión.</p>
            </div>
        </div>
    </div>

    <style>
        .jumbotron {
            background-color: #f8f9fa;
            padding: 2rem 1rem;
            margin-bottom: 2rem;
            border-radius: 12px;
            box-shadow: 0 0 20px rgba(0,0,0,0.15);
        }
        .rounded-img {
            border-radius: 15px;
            max-width: 20%;
            height: auto;
        }
        .btn-primary, .btn-warning {
            padding: 15px 25px;
            font-size: 1.2rem;
            display: flex;
            align-items: center;
        }
        .btn-primary i, .btn-warning i {
            margin-right: 10px;
        }
        .container {
            max-width: 60rem;
        }
        .fa-3x {
            display: block;
            margin-bottom: 15px;
        }
    </style>

</asp:Content>
