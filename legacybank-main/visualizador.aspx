<%@ Page Language="C#" AutoEventWireup="true" CodeFile="visualizador.aspx.cs" Inherits="visualizador" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    

<!-- MAIN CONTENT
================================================== -->
<section class="miolo">


  <? include('../template/breadcrumb.php'); ?>
<section class="middle">


  <!-- HEADER -->
  <div class="header">
    <div class="container-fluid">

      <!-- Body -->
      <div class="header-body">
        <div class="row align-items-end">
          <div class="col">

            <!-- Pretitle -->
            <h6 class="header-pretitle">
              Gerencie as informações do usuário
            </h6>

            <!-- Title -->
            <h1 class="header-title">
              <?=(($usuario) ? 'Usuário '.$usuario['seller_user_login'] : 'Adicionar Usuário')?>
            </h1>
          </div>
          <div class="col-auto">
            <a class="btn btn-outline-secondary lift" href="/gestor/usuarios">Voltar</a>
          </div>
        </div> <!-- / .row -->
      </div> <!-- / .header-body -->
    </div>
  </div> <!-- / .header -->

  <!-- CARDS -->
  <div class="container-fluid">
    <div class="row">
      <div class="col-12 col-lg-12 col-xl">

        <? if ($_SESSION['success']) { ?>
          <div class='alert alert-success'>
            <?=$_SESSION['success']?>
          </div>
        <? unset($_SESSION['success']);  } ?>
        <? if ($_SESSION['error']) { ?>
          <div class='alert alert-danger'>
            <?=$_SESSION['error']?>
          </div>
        <? unset($_SESSION['error']); } ?>

        <div class='card card-resumo'>
          <div class='card-body'>
            <form class="needs-validation" novalidate method="post" action="/router/action-gestor/usuarios-salvar">
              <input type="hidden" name="id" id="id" value="<?=(($usuario) ? md5($usuario['id']) : '0')?>">
              <div class="form-row">
                <div class="col-md-12 mb-3">
                  <label for="type">Perfil de Acesso</label>
                  <select class="form-control" id="seller_is_panel" name="seller_is_panel" required data-toggle="select">
                    <option value="0" <?=(($usuario) && ('0'==strtolower($usuario['seller_is_panel']))) ? "selected='selected'" : "" ?>>Administrador BANCO DIGITAL</option>
                    <? if ($_SESSION['marketplace']['id'] != 1) { ?>
                      <option value="1" <?=(($usuario) && ('1'==strtolower($usuario['seller_is_panel']))) ? "selected='selected'" : "" ?>>Administrador ESTABELECIMENTO COMERCIAL</option>
                    <? } else if ($_SESSION['marketplace']['id'] == 1) { ?>
                      <option value="1" <?=(($usuario) && ('1'==strtolower($usuario['seller_is_panel']))) ? "selected='selected'" : "" ?>>Compliance - Análise de Documentos</option>
                      <option value="2" <?=(($usuario) && ('1'==strtolower($usuario['seller_is_panel']))) ? "selected='selected'" : "" ?>>Atendimento - Atendimento ao Cliente</option>
                    <? } ?>
                  </select>
                  <div class="invalid-tooltip invalid-select">
                    <i class='fa fa-exclamation'></i>
                  </div>
                </div>
              </div>
              <div class="form-row">
                <div class="col-md-6 mb-3">
                  <label for="validationTooltip02">Usuário</label>
                  <input type="text" class="form-control" id="manager_login" name="manager_login" placeholder="Usuário de acesso" value="<?=(($usuario) ? (($usuario['marketplace_id'] == 1) ? $usuario['manager_login'] : $usuario['seller_user_login']) : '')?>" <?=(($usuario) ? 'disabled' : 'required')?> >
                </div>
                <div class="col-md-6 mb-3">
                  <label for="validationTooltipUsername">Senha</label>
                  <input type="password" class="form-control" id="manager_password" name="manager_password" value="<?=(($usuario) ? (($usuario['marketplace_id'] == 1) ? $usuario['manager_password'] : $usuario['seller_user_password']) : '')?>" placeholder="Senha de acesso" autocomplete='new-password' aria-describedby="validationTooltipUsernamePrepend" required>
                </div>
              </div>
              <button class="btn btn-primary" type="submit">Salvar Informações</button>
              <? if (($usuario) && ($usuario['seller_user_login'] != '')) { ?>
                <? if (($usuario) && ($usuario['seller_is_removable'] == 1) && ($_SESSION['marketplace']['whoami'] != $usuario['id'])) { ?>
                  <a href='/router/action-gestor/usuarios-remover-normal?id=<?=md5($usuario['id'])?>' class="btn btn-secondary">Remover Usuário</a>
                <? } ?>
              <? } else { ?>
                <? if (($usuario) && ($usuario['manager_is_removable'] == 1) && ($_SESSION['marketplace']['whoami'] != $usuario['id'])) { ?>
                  <a href='/router/action-gestor/usuarios-remover?id=<?=md5($usuario['id'])?>' class="btn btn-secondary">Remover Usuário</a>
                <? } ?>
              <? } ?>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>
<? include('../template/footer.php')?>
    </div>
    </form>
</body>
</html>
