using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class con_venda_step : System.Web.UI.Page
{
protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            multiViewWizard.ActiveViewIndex = 0; // Começa no primeiro passo
        }
    }

    protected void btnNext1_Click(object sender, EventArgs e)
    {
        multiViewWizard.ActiveViewIndex = 1; // Vai para o passo 2
    }

    protected void btnBack1_Click(object sender, EventArgs e)
    {
        multiViewWizard.ActiveViewIndex = 0; // Volta para o passo 1
    }

    protected void btnNext2_Click(object sender, EventArgs e)
    {
        lblResumo.Text = txtNome.Text.ToString() + " " + txtEmail.Text.ToString(); // Preenche o resumo
        multiViewWizard.ActiveViewIndex = 2; // Vai para o passo 3
    }

    protected void btnBack2_Click(object sender, EventArgs e)
    {
        multiViewWizard.ActiveViewIndex = 1; // Volta para o passo 2
    }

    protected void btnFinish_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Cadastro finalizado com sucesso!');", true);
    }

    // Retorna a classe CSS correta para os círculos numerados
    protected string GetStepClass(int step)
    {
        return multiViewWizard.ActiveViewIndex >= step ? "step active" : "step";
    }

    // Retorna a classe CSS correta para as linhas entre os círculos
    protected string GetLineClass(int step)
    {
        return multiViewWizard.ActiveViewIndex > step ? "step-line active" : "step-line";
    }
    protected void btnBack3_Click(object sender, EventArgs e)
    {
        multiViewWizard.ActiveViewIndex = 2; // Volta para o passo 2

    }
    protected void btnNext3_Click(object sender, EventArgs e)
    {
        multiViewWizard.ActiveViewIndex = 3; // Vai para o passo 4

    }
}
