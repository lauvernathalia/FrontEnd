<%@ Page Language="C#" AutoEventWireup="true" CodeFile="teste.aspx.cs" Inherits="teste" %>

<%@ Register TagPrefix="Portal" TagName="PageBottom" Src="rodapepadrao.ascx" %>
<%@ Register TagPrefix="Portal" TagName="PageLeft" Src="menupadrao.ascx" %>
<%@ Register TagPrefix="Portal" TagName="PageHeader" Src="topopadrao.ascx" %>
<%@ Register TagPrefix="Portal" TagName="PageRotina" Src="rotinaspadroes.ascx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta http-equiv="x-ua-compatible" content="ie=edge">


    <title>Radio com Ícone</title>

    <link rel="icon" type="image/x-icon" href="../images/favicon-96x96.png">

    <PORTAL:PAGEROTINA id="PageRotina1" title="Site Directory" runat="server" ModuleSource="rotinaspadroes.ascx"></PORTAL:PAGEROTINA>
    <link rel="stylesheet" href="../plugins/fullcalendar/main.min.css"/>
    <link rel="stylesheet" href="../plugins/fullcalendar-daygrid/main.min.css"/>
    <link rel="stylesheet" href="../plugins/fullcalendar-timegrid/main.min.css"/>
    <link rel="stylesheet" href="../plugins/fullcalendar-bootstrap/main.min.css"/>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.5.0/Chart.min.js"></script>

    <link href="https://fontawesome.com/icons/pix?f=brands&s=solid" rel="Stylesheet" />
    <script src="https://kit.fontawesome.com/91552b3746.js" crossorigin="anonymous"></script>

</head>
<body>
    <form id="form1" runat="server">



    <div class="row col-12">
        <button type="button" id="btnImportar">Iniciar Importação</button>

        <div class="progress" style="height:25px; margin-top:10px; width:300px;">
            <div id="barraProgresso" class="progress-bar" 
                 style="width:0%; background-color:green; text-align:center; color:white;">
                0%
            </div>
        </div>
    </div>

    <div>
      <div class="row col-12">
      <label>
        <input type="radio" name="opcao" value="1" hidden>
        <div class="option-card">
          <div class="option-header">
            <i class="fas fa-user text-success"></i>
            <input type="radio" name="opcao" value="1">
          </div>
          <div class="option-text text-success">CPF</div>
        </div>
      </label>

      <label>
        <input type="radio" name="opcao" value="2" hidden>
        <div class="option-card">
          <div class="option-header">
            <i class="fas fa-building text-success"></i>
            <input type="radio" name="opcao" value="2">
          </div>
          <div class="option-text text-success">CNPJ</div>
        </div>
      </label>



      <label>
        <input type="radio" name="teste" value="1" hidden>
        <div class="option-card">
          <div class="option-header">
            <i class="fas fa-credit-card text-success"></i>
            <input type="radio" name="teste" value="1">
          </div>
          <div class="option-text text-success">CARTÃO CRÉDITO</div>
        </div>
      </label>

      <label>
        <input type="radio" name="teste" value="2" hidden>
        <div class="option-card">
          <div class="option-header">
            <i class="fas fa-barcode text-success"></i>
            <input type="radio" name="teste" value="2">
          </div>
          <div class="option-text text-success">BOLETO</div>
        </div>
      </label>
      </div>
      <div class="row col-12">


        <div class="gauge-wrap">
          <div class="gauge">
            <svg viewBox="0 0 200 120" aria-hidden="true">
              <!-- Trilho cinza (todo o arco) -->
              <path id="arc"
                    d="M 10 110 A 100 100 0 0 1 190 110"
                    fill="none" stroke="#e6e6e6" stroke-width="5"
                    pathLength="1000"></path>

              <!-- Faixas (usam o mesmo path, fatiado por dasharray) -->
              <!-- 0–500: vermelho (500) -->
              <path d="M 10 110 A 100 100 0 0 1 190 110"
                    fill="none" stroke="red" stroke-width="5"
                    pathLength="1000" stroke-dasharray="500 1000" stroke-dashoffset="0"></path>

              <!-- 500–666: amarelo (166) -->
              <path d="M 10 110 A 100 100 0 0 1 190 110"
                    fill="none" stroke="orange" stroke-width="5"
                    pathLength="1000" stroke-dasharray="166 1000" stroke-dashoffset="-500"></path>

              <!-- 666–832: laranja (166) -->
              <path d="M 10 110 A 100 100 0 0 1 190 110"
                    fill="none" stroke="gold" stroke-width="5"
                    pathLength="1000" stroke-dasharray="166 1000" stroke-dashoffset="-666"></path>

              <!-- 832–1000: verde (168) -->
              <path d="M 10 110 A 100 100 0 0 1 190 110"
                    fill="none" stroke="green" stroke-width="5"
                    pathLength="1000" stroke-dasharray="168 1000" stroke-dashoffset="-832"></path>

              <!-- Bolinha indicadora -->
              <circle id="dot" r="7" cx="132.8" cy="29.5" fill="#111" />
            </svg>
          </div>

          <div class="readout" id="readout">735</div>
        </div>




      <script>
       function updateGauge(value) {
          // limita o valor
          value = Math.max(0, Math.min(1000, value));

          // atualiza número central
          document.getElementById("readout").textContent = value;

          // path do arco
          const path = document.getElementById("arc");
          const totalLength = path.getTotalLength();

          // posição exata da bolinha no arco
          const point = path.getPointAtLength(totalLength * (value / 1000));

          const dot = document.getElementById("dot");
          dot.setAttribute("cx", point.x.toFixed(2));
          dot.setAttribute("cy", point.y.toFixed(2));

          // define cor da bolinha de acordo com a faixa
          let color;
          if (value <= 500) color = "red";
          else if (value <= 666) color = "orange";
          else if (value <= 832) color = "gold";
          else color = "green";

          dot.setAttribute("fill", color);
        }

        // Teste: posiciona a bolinha em 735
        updateGauge(735);

        // Exemplo com slider para testar dinamicamente
        const slider = document.getElementById("slider");
        if(slider) {
          slider.addEventListener("input", e => updateGauge(Number(e.target.value)));
        }
        </script>


      
      </div>



    </div>
    </form>

<script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
<script type="text/javascript">
    var intervalo;

    $("#btnImportar").click(function () {
        $.ajax({
            type: "POST",
            url: "teste.aspx/IniciarImportacao",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (res) {
                alert(res.d);
                intervalo = setInterval(atualizarProgresso, 1000);
            }
        });
    });

    function atualizarProgresso() {
        $.ajax({
            type: "POST",
            url: "teste.aspx/GetProgresso",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (res) {
                var valor = res.d;
                $("#barraProgresso").css("width", valor + "%").text(valor + "%");

                if (valor >= 100) {
                    clearInterval(intervalo);
                }
            }
        });
    }
</script>
</body>
</html>
