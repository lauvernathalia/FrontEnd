<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dashboardpdl.aspx.cs" Inherits="dashboardpdl" %>

<!DOCTYPE html>
<html lang="pt-br">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Painel de Desenvolvimento de Líderes</title>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/admin-lte/3.1.0/css/adminlte.min.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css">
</head>
<body class="hold-transition sidebar-mini">
    <div class="wrapper">
        <!-- Navbar -->
        <nav class="main-header navbar navbar-expand navbar-white navbar-light">
            <ul class="navbar-nav">
                <li class="nav-item d-none d-sm-inline-block">
                    <a href="#" class="nav-link">Home</a>
                </li>
            </ul>
        </nav>

        <!-- Main content -->
        <div class="content-wrapper">
            <div class="content-header">
                <div class="container-fluid">
                    <div class="row mb-2">
                        <div class="col-sm-6">
                            <h1 class="m-0">Painel de Desenvolvimento de Líderes</h1>
                        </div>
                    </div>
                </div>
            </div>

            <div class="content">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-4 col-6">
                            <div class="small-box bg-info">
                                <div class="inner">
                                    <h3><asp:Label ID="lblLideres" runat="server" Text="42"></asp:Label></h3>
                                    <p>Líderes Cadastrados</p>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-4 col-6">
                            <div class="small-box bg-success">
                                <div class="inner">
                                    <h3><asp:Label ID="lblAvaliacoes" runat="server" Text="128"></asp:Label></h3>
                                    <p>Avaliações Realizadas</p>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-4 col-6">
                            <div class="small-box bg-warning">
                                <div class="inner">
                                    <h3><asp:Label ID="lblProgresso" runat="server" Text="75%"></asp:Label></h3>
                                    <p>Média de Desenvolvimento</p>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="card">
                        <div class="card-header">
                            <h3 class="card-title">Líderes em Desenvolvimento</h3>
                        </div>
                        <div class="card-body">
                            <table class="table table-bordered">
                                <thead>
                                    <tr>
                                        <th>Nome</th>
                                        <th>Cargo</th>
                                        <th>Avaliação</th>
                                        <th>Progresso</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>João Silva</td>
                                        <td>Gerente de Operações</td>
                                        <td>8.5</td>
                                        <td><div class="progress"><div class="progress-bar bg-success" style="width: 85%">85%</div></div></td>
                                    </tr>
                                    <tr>
                                        <td>Ana Souza</td>
                                        <td>Supervisora de Logística</td>
                                        <td>7.8</td>
                                        <td><div class="progress"><div class="progress-bar bg-warning" style="width: 78%">78%</div></div></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/admin-lte/3.1.0/js/adminlte.min.js"></script>
</body>
</html>
