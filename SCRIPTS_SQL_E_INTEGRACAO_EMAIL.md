# 🗂️ SCRIPTS SQL E INTEGRAÇÃO DE EMAIL - REPRESENTANTES

## 📋 Índice
1. Scripts SQL Completos
2. Sistema de Envio de Email (2FA)
3. URLs de POST/GET das Integrações
4. Fluxo Completo de Conclusão do Cadastro
5. Exemplos Práticos

---

## 🔧 SCRIPTS SQL COMPLETOS

### ✅ STORED PROCEDURE: stp_pessoas_fj_ins (CRUD Principal)

Esta SP gerencia todas as operações de CRUD na tabela PESSOAS_FJ (Representantes, Estabelecimentos, Marketplaces).

#### OPERAÇÕES SUPORTADAS:

| Op | Descrição | Tipo |
|----|-----------|------|
| **C** | Listar pessoas (Count) | SELECT |
| **L** | Listar todas | SELECT |
| **S** | Selecionar por ID | SELECT |
| **H** | Verificar se existe (Has) | SELECT |
| **I** | Inserir (Insert) | INSERT |
| **A** | Atualizar (Alter) | UPDATE |
| **D** | Deletar (Delete) | DELETE |

---

#### SQL SCRIPT - CREATE PROCEDURE:

```sql
CREATE PROCEDURE [dbo].[stp_pessoas_fj_ins]
    @flg_operacao       VARCHAR(1),
    @COD_ID             INT = NULL,
    @COD_ID_PESSOA_LICENCIADO INT,
    @COD_ID_MARKETPLACE INT = NULL,
    @COD_ID_REPRESENTANTE INT = NULL,
    @FLG_TIPO           CHAR(1),  -- R=Representante, E=Estabelecimento, M=Marketplace
    @NOM_RAZAOSOCIAL    VARCHAR(255),
    @NOM_FANTASIA       VARCHAR(255) = NULL,
    @NOM_CNPJ           VARCHAR(20),  -- Para PJ
    @NUM_TELEFONE       VARCHAR(20) = NULL,
    @NOM_EMAIL_EMPRESA  VARCHAR(255) = NULL,
    @NOM_ENDERECO       VARCHAR(255),
    @NOM_NUMERO         VARCHAR(10),
    @NOM_COMPLEMENTO    VARCHAR(255) = NULL,
    @NOM_BAIRRO         VARCHAR(100),
    @NOM_CIDADE         VARCHAR(100),
    @NOM_CEP            VARCHAR(10),
    @NOM_UF             CHAR(2),
    @COD_ID_MCC         INT = NULL,
    @FLG_TIPO_PESSOA    CHAR(2),  -- PF=Pessoa Física, PJ=Pessoa Jurídica
    @NOM_NOME           VARCHAR(255),
    @NOM_SOBRENOME      VARCHAR(255) = NULL,
    @NOM_CPF            VARCHAR(20) = NULL,
    @DTA_ANIVERSARIO    DATETIME = NULL,
    @NOM_MAE            VARCHAR(255) = NULL,
    @NUM_RENDA_MENSAL   DECIMAL(18,2) = NULL,
    @NOM_EMAIL          VARCHAR(255) = NULL,
    @NOM_CELULAR        VARCHAR(20) = NULL,
    @NOM_NOME_USUARIO   VARCHAR(255) = NULL,
    @NOM_LOGIN          VARCHAR(255) = NULL,  -- Email para login
    @NOM_SENHA          VARCHAR(255) = NULL,  -- Criptografada
    @FLG_ATIVO          CHAR(1) = 'S',
    @FLG_PRESENCIAL     CHAR(1) = 'N',
    @FLG_POLITICAMENTE  CHAR(1) = 'N',
    @COD_ID_USUARIO_CRIACAO INT = NULL,
    @DTA_DATA_CRIACAO   DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- ===================================================================
        -- OPERAÇÃO: C = Count (contar registros)
        -- ===================================================================
        IF @flg_operacao = 'C'
        BEGIN
            SELECT COUNT(*) AS TOTAL
            FROM PESSOAS_FJ
            WHERE COD_ID_PESSOA_LICENCIADO = @COD_ID_PESSOA_LICENCIADO
              AND FLG_TIPO = @FLG_TIPO
              AND FLG_ATIVO = 'S'
        END

        -- ===================================================================
        -- OPERAÇÃO: L = Listar todas
        -- ===================================================================
        ELSE IF @flg_operacao = 'L'
        BEGIN
            SELECT 
                COD_ID,
                COD_ID_PESSOA_LICENCIADO,
                COD_ID_MARKETPLACE,
                FLG_TIPO,
                NOM_RAZAOSOCIAL,
                NOM_FANTASIA,
                NOM_CNPJ,
                NUM_TELEFONE,
                NOM_EMAIL_EMPRESA,
                NOM_ENDERECO,
                NOM_NUMERO,
                NOM_COMPLEMENTO,
                NOM_BAIRRO,
                NOM_CIDADE,
                NOM_CEP,
                NOM_UF,
                NOM_NOME,
                NOM_CPF,
                NOM_EMAIL,
                NOM_CELULAR,
                FLG_ATIVO,
                DTA_DATA
            FROM PESSOAS_FJ
            WHERE COD_ID_PESSOA_LICENCIADO = @COD_ID_PESSOA_LICENCIADO
              AND FLG_TIPO = @FLG_TIPO
              AND FLG_ATIVO = 'S'
            ORDER BY NOM_RAZAOSOCIAL ASC
        END

        -- ===================================================================
        -- OPERAÇÃO: S = Selecionar por ID
        -- ===================================================================
        ELSE IF @flg_operacao = 'S'
        BEGIN
            SELECT 
                COD_ID,
                COD_ID_PESSOA_LICENCIADO,
                COD_ID_MARKETPLACE,
                COD_ID_REPRESENTANTE,
                FLG_TIPO,
                NOM_RAZAOSOCIAL,
                NOM_FANTASIA,
                NOM_CNPJ,
                NUM_TELEFONE,
                NOM_EMAIL_EMPRESA,
                NOM_ENDERECO,
                NOM_NUMERO,
                NOM_COMPLEMENTO,
                NOM_BAIRRO,
                NOM_CIDADE,
                NOM_CEP,
                NOM_UF,
                COD_ID_MCC,
                FLG_TIPO_PESSOA,
                NOM_NOME,
                NOM_SOBRENOME,
                NOM_CPF,
                DTA_ANIVERSARIO,
                NOM_MAE,
                NUM_RENDA_MENSAL,
                NOM_EMAIL,
                NOM_CELULAR,
                NOM_NOME_USUARIO,
                NOM_LOGIN,
                -- NOM_SENHA não retorna (segurança)
                FLG_ATIVO,
                FLG_PRESENCIAL,
                FLG_POLITICAMENTE,
                DTA_DATA,
                DTA_DATA_ULT_ALT
            FROM PESSOAS_FJ
            WHERE COD_ID = @COD_ID
              AND COD_ID_PESSOA_LICENCIADO = @COD_ID_PESSOA_LICENCIADO
        END

        -- ===================================================================
        -- OPERAÇÃO: H = Verificar Existência (Has) - Verificar Duplicidade
        -- ===================================================================
        ELSE IF @flg_operacao = 'H'
        BEGIN
            IF @FLG_TIPO_PESSOA = 'PJ'
            BEGIN
                -- Para PJ: verificar CNPJ único por Licenciado
                SELECT COUNT(*) AS EXISTE
                FROM PESSOAS_FJ
                WHERE COD_ID_PESSOA_LICENCIADO = @COD_ID_PESSOA_LICENCIADO
                  AND NOM_CNPJ = @NOM_CNPJ
                  AND FLG_ATIVO = 'S'
                  AND COD_ID <> ISNULL(@COD_ID, 0)  -- Excluir edit do mesmo registro
            END
            ELSE IF @FLG_TIPO_PESSOA = 'PF'
            BEGIN
                -- Para PF: verificar CPF único por Licenciado
                SELECT COUNT(*) AS EXISTE
                FROM PESSOAS_FJ
                WHERE COD_ID_PESSOA_LICENCIADO = @COD_ID_PESSOA_LICENCIADO
                  AND NOM_CPF = @NOM_CPF
                  AND FLG_ATIVO = 'S'
                  AND COD_ID <> ISNULL(@COD_ID, 0)
            END
        END

        -- ===================================================================
        -- OPERAÇÃO: I = Inserir (Insert)
        -- ===================================================================
        ELSE IF @flg_operacao = 'I'
        BEGIN
            INSERT INTO PESSOAS_FJ (
                COD_ID_PESSOA_LICENCIADO,
                COD_ID_MARKETPLACE,
                COD_ID_REPRESENTANTE,
                FLG_TIPO,
                NOM_RAZAOSOCIAL,
                NOM_FANTASIA,
                NOM_CNPJ,
                NUM_TELEFONE,
                NOM_EMAIL_EMPRESA,
                NOM_ENDERECO,
                NOM_NUMERO,
                NOM_COMPLEMENTO,
                NOM_BAIRRO,
                NOM_CIDADE,
                NOM_CEP,
                NOM_UF,
                COD_ID_MCC,
                FLG_TIPO_PESSOA,
                NOM_NOME,
                NOM_SOBRENOME,
                NOM_CPF,
                DTA_ANIVERSARIO,
                NOM_MAE,
                NUM_RENDA_MENSAL,
                NOM_EMAIL,
                NOM_CELULAR,
                NOM_NOME_USUARIO,
                NOM_LOGIN,
                NOM_SENHA,
                FLG_ATIVO,
                FLG_PRESENCIAL,
                FLG_POLITICAMENTE,
                DTA_DATA,
                DTA_DATA_ULT_ALT,
                COD_ID_USUARIO_CRIACAO
            ) VALUES (
                @COD_ID_PESSOA_LICENCIADO,
                @COD_ID_MARKETPLACE,
                @COD_ID_REPRESENTANTE,
                @FLG_TIPO,
                @NOM_RAZAOSOCIAL,
                @NOM_FANTASIA,
                @NOM_CNPJ,
                @NUM_TELEFONE,
                @NOM_EMAIL_EMPRESA,
                @NOM_ENDERECO,
                @NOM_NUMERO,
                @NOM_COMPLEMENTO,
                @NOM_BAIRRO,
                @NOM_CIDADE,
                @NOM_CEP,
                @NOM_UF,
                @COD_ID_MCC,
                @FLG_TIPO_PESSOA,
                @NOM_NOME,
                @NOM_SOBRENOME,
                @NOM_CPF,
                @DTA_ANIVERSARIO,
                @NOM_MAE,
                @NUM_RENDA_MENSAL,
                @NOM_EMAIL,
                @NOM_CELULAR,
                @NOM_NOME_USUARIO,
                @NOM_LOGIN,
                @NOM_SENHA,
                @FLG_ATIVO,
                @FLG_PRESENCIAL,
                @FLG_POLITICAMENTE,
                GETDATE(),  -- DTA_DATA = agora
                GETDATE(),  -- DTA_DATA_ULT_ALT = agora
                @COD_ID_USUARIO_CRIACAO
            )

            -- Retornar ID do novo registro
            SELECT SCOPE_IDENTITY() AS COD_ID
        END

        -- ===================================================================
        -- OPERAÇÃO: A = Atualizar (Alter/Update)
        -- ===================================================================
        ELSE IF @flg_operacao = 'A'
        BEGIN
            UPDATE PESSOAS_FJ
            SET
                NOM_RAZAOSOCIAL = ISNULL(@NOM_RAZAOSOCIAL, NOM_RAZAOSOCIAL),
                NOM_FANTASIA = ISNULL(@NOM_FANTASIA, NOM_FANTASIA),
                NUM_TELEFONE = ISNULL(@NUM_TELEFONE, NUM_TELEFONE),
                NOM_EMAIL_EMPRESA = ISNULL(@NOM_EMAIL_EMPRESA, NOM_EMAIL_EMPRESA),
                NOM_ENDERECO = ISNULL(@NOM_ENDERECO, NOM_ENDERECO),
                NOM_NUMERO = ISNULL(@NOM_NUMERO, NOM_NUMERO),
                NOM_COMPLEMENTO = ISNULL(@NOM_COMPLEMENTO, NOM_COMPLEMENTO),
                NOM_BAIRRO = ISNULL(@NOM_BAIRRO, NOM_BAIRRO),
                NOM_CIDADE = ISNULL(@NOM_CIDADE, NOM_CIDADE),
                NOM_CEP = ISNULL(@NOM_CEP, NOM_CEP),
                NOM_UF = ISNULL(@NOM_UF, NOM_UF),
                COD_ID_MCC = ISNULL(@COD_ID_MCC, COD_ID_MCC),
                NOM_NOME = ISNULL(@NOM_NOME, NOM_NOME),
                NOM_SOBRENOME = ISNULL(@NOM_SOBRENOME, NOM_SOBRENOME),
                DTA_ANIVERSARIO = ISNULL(@DTA_ANIVERSARIO, DTA_ANIVERSARIO),
                NOM_MAE = ISNULL(@NOM_MAE, NOM_MAE),
                NUM_RENDA_MENSAL = ISNULL(@NUM_RENDA_MENSAL, NUM_RENDA_MENSAL),
                NOM_EMAIL = ISNULL(@NOM_EMAIL, NOM_EMAIL),
                NOM_CELULAR = ISNULL(@NOM_CELULAR, NOM_CELULAR),
                NOM_NOME_USUARIO = ISNULL(@NOM_NOME_USUARIO, NOM_NOME_USUARIO),
                NOM_SENHA = ISNULL(@NOM_SENHA, NOM_SENHA),
                FLG_ATIVO = ISNULL(@FLG_ATIVO, FLG_ATIVO),
                FLG_PRESENCIAL = ISNULL(@FLG_PRESENCIAL, FLG_PRESENCIAL),
                FLG_POLITICAMENTE = ISNULL(@FLG_POLITICAMENTE, FLG_POLITICAMENTE),
                DTA_DATA_ULT_ALT = GETDATE()
            WHERE COD_ID = @COD_ID
              AND COD_ID_PESSOA_LICENCIADO = @COD_ID_PESSOA_LICENCIADO

            SELECT @@ROWCOUNT AS REGISTROS_ATUALIZADOS
        END

        -- ===================================================================
        -- OPERAÇÃO: D = Deletar (Delete - Soft Delete)
        -- ===================================================================
        ELSE IF @flg_operacao = 'D'
        BEGIN
            -- Soft delete: apenas marca como inativo
            UPDATE PESSOAS_FJ
            SET
                FLG_ATIVO = 'N',
                DTA_DATA_ULT_ALT = GETDATE()
            WHERE COD_ID = @COD_ID
              AND COD_ID_PESSOA_LICENCIADO = @COD_ID_PESSOA_LICENCIADO

            SELECT @@ROWCOUNT AS REGISTROS_DELETADOS
        END

    END TRY
    BEGIN CATCH
        -- Retornar erro
        DECLARE @ErroMsg NVARCHAR(4000)
        DECLARE @ErroSeveridade INT

        SELECT @ErroMsg = ERROR_MESSAGE(), @ErroSeveridade = ERROR_SEVERITY()

        RAISERROR(@ErroMsg, @ErroSeveridade, 1)
    END CATCH
END
GO
```

---

### 🗂️ SPs Complementares

#### stp_pessoas_fj_cappta_ins (Dados de Integração CapPTA)

```sql
CREATE PROCEDURE [dbo].[stp_pessoas_fj_cappta_ins]
    @flg_operacao           VARCHAR(1),  -- I=Insert, U=Update, S=Select
    @COD_ID                 INT = NULL,
    @COD_ID_PESSOAS_FJ      INT,
    @NUM_TOKEN_CAPPTA       VARCHAR(255) = NULL,
    @FLG_STATUS_CAPPTA      CHAR(1) = 'P',  -- P=Pendente, A=Ativo, I=Inativo
    @DES_JSON_CAPPTA        TEXT = NULL,
    @COD_ID_NATUREZA_CAPPTA INT = NULL
AS
BEGIN
    SET NOCOUNT ON

    -- Inserir dados Cappta
    IF @flg_operacao = 'I'
    BEGIN
        INSERT INTO PESSOAS_FJ_CAPPTA (
            COD_ID_PESSOAS_FJ,
            NUM_TOKEN_CAPPTA,
            FLG_STATUS_CAPPTA,
            DES_JSON_CAPPTA,
            COD_ID_NATUREZA_CAPPTA,
            DTA_DATA
        ) VALUES (
            @COD_ID_PESSOAS_FJ,
            @NUM_TOKEN_CAPPTA,
            @FLG_STATUS_CAPPTA,
            @DES_JSON_CAPPTA,
            @COD_ID_NATUREZA_CAPPTA,
            GETDATE()
        )

        SELECT SCOPE_IDENTITY() AS COD_ID
    END

    -- Atualizar dados Cappta
    ELSE IF @flg_operacao = 'U'
    BEGIN
        UPDATE PESSOAS_FJ_CAPPTA
        SET
            NUM_TOKEN_CAPPTA = ISNULL(@NUM_TOKEN_CAPPTA, NUM_TOKEN_CAPPTA),
            FLG_STATUS_CAPPTA = ISNULL(@FLG_STATUS_CAPPTA, FLG_STATUS_CAPPTA),
            DES_JSON_CAPPTA = ISNULL(@DES_JSON_CAPPTA, DES_JSON_CAPPTA),
            COD_ID_NATUREZA_CAPPTA = ISNULL(@COD_ID_NATUREZA_CAPPTA, COD_ID_NATUREZA_CAPPTA)
        WHERE COD_ID = @COD_ID
          AND COD_ID_PESSOAS_FJ = @COD_ID_PESSOAS_FJ

        SELECT @@ROWCOUNT AS REGISTROS_ATUALIZADOS
    END

    -- Selecionar dados Cappta
    ELSE IF @flg_operacao = 'S'
    BEGIN
        SELECT *
        FROM PESSOAS_FJ_CAPPTA
        WHERE COD_ID_PESSOAS_FJ = @COD_ID_PESSOAS_FJ
        ORDER BY DTA_DATA DESC
    END
END
GO
```

#### stp_pessoas_fj_contas_ins (Dados Bancários)

```sql
CREATE PROCEDURE [dbo].[stp_pessoas_fj_contas_ins]
    @flg_operacao               VARCHAR(1),
    @COD_ID                     INT = NULL,
    @COD_ID_PESSOAS_FJ          INT,
    @NOM_CODIGO_BANCO           VARCHAR(10),
    @NOM_NUMERO_AGENCIA_BANCO   VARCHAR(10),
    @NOM_NUMERO_CONTA_BANCO     VARCHAR(20),
    @NOM_TIPO_CONTA             VARCHAR(20) = 'CC'  -- CC=Corrente, PP=Poupança
AS
BEGIN
    SET NOCOUNT ON

    -- Inserir conta bancária
    IF @flg_operacao = 'I'
    BEGIN
        INSERT INTO PESSOAS_FJ_CONTAS_INS (
            COD_ID_PESSOAS_FJ,
            NOM_CODIGO_BANCO,
            NOM_NUMERO_AGENCIA_BANCO,
            NOM_NUMERO_CONTA_BANCO,
            NOM_TIPO_CONTA,
            DTA_DATA
        ) VALUES (
            @COD_ID_PESSOAS_FJ,
            @NOM_CODIGO_BANCO,
            @NOM_NUMERO_AGENCIA_BANCO,
            @NOM_NUMERO_CONTA_BANCO,
            @NOM_TIPO_CONTA,
            GETDATE()
        )

        SELECT SCOPE_IDENTITY() AS COD_ID
    END

    -- Atualizar conta bancária
    ELSE IF @flg_operacao = 'U'
    BEGIN
        UPDATE PESSOAS_FJ_CONTAS_INS
        SET
            NOM_CODIGO_BANCO = ISNULL(@NOM_CODIGO_BANCO, NOM_CODIGO_BANCO),
            NOM_NUMERO_AGENCIA_BANCO = ISNULL(@NOM_NUMERO_AGENCIA_BANCO, NOM_NUMERO_AGENCIA_BANCO),
            NOM_NUMERO_CONTA_BANCO = ISNULL(@NOM_NUMERO_CONTA_BANCO, NOM_NUMERO_CONTA_BANCO),
            NOM_TIPO_CONTA = ISNULL(@NOM_TIPO_CONTA, NOM_TIPO_CONTA)
        WHERE COD_ID = @COD_ID
          AND COD_ID_PESSOAS_FJ = @COD_ID_PESSOAS_FJ

        SELECT @@ROWCOUNT AS REGISTROS_ATUALIZADOS
    END

    -- Selecionar
    ELSE IF @flg_operacao = 'S'
    BEGIN
        SELECT *
        FROM PESSOAS_FJ_CONTAS_INS
        WHERE COD_ID_PESSOAS_FJ = @COD_ID_PESSOAS_FJ
        ORDER BY DTA_DATA DESC
    END
END
GO
```

#### stp_consulta_ficha_cadastro_ins (Auditoria)

```sql
CREATE PROCEDURE [dbo].[stp_consulta_ficha_cadastro_ins]
    @flg_operacao       VARCHAR(1),
    @NOM_DOCUMENTO      VARCHAR(20),
    @NOM_NOME           VARCHAR(255),
    @NOM_OPERACAO       VARCHAR(50),  -- Incluir, Alterar, Consultar, Deletar
    @FLG_ORIGEM         VARCHAR(50),  -- BigDataCorp, HubCappta, Sistema, etc
    @DES_DETALHES       TEXT = NULL,
    @COD_ID_USUARIO     INT
AS
BEGIN
    SET NOCOUNT ON

    -- Registrar operação (auditoria)
    IF @flg_operacao = 'I'
    BEGIN
        INSERT INTO CONSULTAS_FICHA_CADASTRO (
            NOM_DOCUMENTO,
            NOM_NOME,
            NOM_OPERACAO,
            FLG_ORIGEM,
            DES_DETALHES,
            COD_ID_USUARIO,
            DTA_DATA
        ) VALUES (
            @NOM_DOCUMENTO,
            @NOM_NOME,
            @NOM_OPERACAO,
            @FLG_ORIGEM,
            @DES_DETALHES,
            @COD_ID_USUARIO,
            GETDATE()
        )

        SELECT SCOPE_IDENTITY() AS COD_ID
    END
END
GO
```

---

## 📧 SISTEMA DE ENVIO DE EMAIL (2FA)

### Arquivo: funcoes.cs - Função de Envio de Email

```csharp
/// <summary>
/// Envia 2FA por email
/// </summary>
public static bool Enviar2fa(string email, string codigo, string nome)
{
    try
    {
        // Configuração SMTP
        SmtpClient client = new SmtpClient();
        
        // Ler configurações de email do banco de dados
        SqlConnection conn = new SqlConnection(Funcoes.conexao());
        conn.Open();
        
        SqlCommand cmd = new SqlCommand(@"
            SELECT TOP 1
                NUM_PORTA_SMTP,
                NOM_SERVIDOR_SMTP,
                NOM_EMAIL_ENVIO,
                NOM_SENHA_EMAIL,
                FLG_SSL_TLS
            FROM SERVIDORES_EMAIL
            WHERE COD_ID_LICENCIADO = @LICENCIADO
            AND FLG_ATIVO = 'S'
        ", conn);
        
        cmd.Parameters.Add("@LICENCIADO", SqlDbType.Int).Value = 
            Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        
        SqlDataReader dr = cmd.ExecuteReader();
        
        if (!dr.HasRows)
        {
            conn.Close();
            return false;
        }

        dr.Read();
        
        string smtpServer = dr["NOM_SERVIDOR_SMTP"].ToString();
        int smtpPort = Funcoes.strToInt(dr["NUM_PORTA_SMTP"].ToString());
        string emailOrigem = dr["NOM_EMAIL_ENVIO"].ToString();
        string senhaEmail = dr["NOM_SENHA_EMAIL"].ToString();
        bool usarSSL = dr["FLG_SSL_TLS"].ToString() == "S";

        conn.Close();

        // Configurar cliente SMTP
        client.Host = smtpServer;
        client.Port = smtpPort;
        client.EnableSsl = usarSSL;
        
        // Credenciais
        NetworkCredential credentials = new NetworkCredential(emailOrigem, senhaEmail);
        client.Credentials = credentials;

        // Criar mensagem de email
        MailMessage mensagem = new MailMessage();
        mensagem.From = new MailAddress(emailOrigem, "LegacyBank");
        mensagem.To.Add(email);
        
        // Assunto e Corpo
        mensagem.Subject = "Código de Confirmação - LegacyBank";
        
        // HTML do Email
        string htmlBody = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <style>
                    body {{ font-family: Arial, sans-serif; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background-color: #003366; color: white; padding: 20px; text-align: center; }}
                    .content {{ padding: 20px; border: 1px solid #ddd; }}
                    .codigo {{ 
                        background-color: #f0f0f0; 
                        padding: 15px; 
                        text-align: center; 
                        font-size: 24px; 
                        font-weight: bold; 
                        color: #003366;
                        letter-spacing: 2px;
                    }}
                    .rodape {{ text-align: center; color: #666; font-size: 12px; margin-top: 20px; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>LegacyBank</h1>
                        <p>Sistema de Confirmação</p>
                    </div>
                    
                    <div class='content'>
                        <p>Olá <strong>{nome}</strong>,</p>
                        
                        <p>Você solicitou um código de confirmação para completar seu cadastro.</p>
                        
                        <p><strong>Seu código é:</strong></p>
                        <div class='codigo'>{codigo}</div>
                        
                        <p>Este código é válido por <strong>10 minutos</strong>.</p>
                        
                        <p>Se você não solicitou este código, ignore este email.</p>
                        
                        <hr>
                        
                        <p><strong>Atenção:</strong> Nunca compartilhe este código com outras pessoas.</p>
                    </div>
                    
                    <div class='rodape'>
                        <p>© 2026 LegacyBank - Todos os direitos reservados</p>
                        <p>Email enviado automaticamente - não responda</p>
                    </div>
                </div>
            </body>
            </html>
        ";

        mensagem.Body = htmlBody;
        mensagem.IsBodyHtml = true;

        // Enviar
        client.Send(mensagem);
        
        mensagem.Dispose();
        client.Dispose();

        return true;
    }
    catch (Exception ex)
    {
        // Log do erro
        Funcoes.LogarErro("Enviar2fa", ex.Message);
        return false;
    }
}
```

### Validação do Código 2FA - Função no Backend

```csharp
/// <summary>
/// Valida código 2FA recebido pelo usuário
/// </summary>
public static bool ValidarCodigo2FA(string emailUsuario, string codigoRecebido)
{
    try
    {
        // Buscar código no Session/Cache
        string chaveSessao = $"2FA_{emailUsuario}";
        
        if (HttpContext.Current.Session[chaveSessao] == null)
        {
            return false; // Código expirou
        }

        string codigo2FAArmazenado = HttpContext.Current.Session[chaveSessao].ToString();
        
        // Comparar
        if (codigo2FAArmazenado == codigoRecebido)
        {
            // Remover da sessão após validação
            HttpContext.Current.Session.Remove(chaveSessao);
            return true;
        }

        return false;
    }
    catch
    {
        return false;
    }
}
```

### Geração do Código 2FA

```csharp
/// <summary>
/// Gera código 2FA aleatório (6 dígitos)
/// </summary>
public static string GerarCodigo2FA()
{
    Random random = new Random();
    int codigo = random.Next(100000, 999999);
    return codigo.ToString();
}
```

### Armazenar Código na Sessão

```csharp
/// <summary>
/// Armazena código 2FA na sessão com timeout de 10 minutos
/// </summary>
public static void ArmazenarCodigo2FA(string emailUsuario, string codigo)
{
    string chaveSessao = $"2FA_{emailUsuario}";
    
    // Armazenar na sessão
    HttpContext.Current.Session[chaveSessao] = codigo;
    
    // Timeout: 10 minutos = 600 segundos
    HttpContext.Current.Session.Timeout = 10;
}
```

---

## 🌐 URLs DE POST/GET DAS INTEGRAÇÕES

### 1️⃣ ViaCEP - Consulta de Endereço

**Tipo:** GET (Síncrono)

```
URL:        https://viacep.com.br/ws/{CEP}/json
MÉTODO:     GET
TIMEOUT:    ~200ms
FORMATO:    JSON
```

**Exemplo de Requisição:**
```
GET https://viacep.com.br/ws/01310100/json HTTP/1.1
Host: viacep.com.br
Connection: close
```

**Resposta (200 OK):**
```json
{
    "cep": "01310-100",
    "logradouro": "Avenida Paulista",
    "complemento": "lado par",
    "bairro": "Bela Vista",
    "localidade": "São Paulo",
    "uf": "SP",
    "ibge": "3550308",
    "gia": "",
    "ddd": "11",
    "siafi": "7107"
}
```

**Resposta (CEP não encontrado):**
```json
{
    "erro": true
}
```

**Código C# de Integração:**

```csharp
public static bool ConsultarCEP(string cep, ref string rua, ref string bairro, 
    ref string cidade, ref string uf)
{
    try
    {
        string url = $"https://viacep.com.br/ws/{cep}/json";
        
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.Timeout = 5000;
        
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        
        if (response.StatusCode == HttpStatusCode.OK)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string responseText = reader.ReadToEnd();
            
            // Parse JSON
            dynamic json = JsonConvert.DeserializeObject(responseText);
            
            if (json["erro"] == null)  // Se não há erro
            {
                rua = json["logradouro"];
                bairro = json["bairro"];
                cidade = json["localidade"];
                uf = json["uf"];
                
                return true;
            }
        }

        return false;
    }
    catch (Exception ex)
    {
        LogarErro("ConsultarCEP", ex.Message);
        return false;
    }
}
```

---

### 2️⃣ BigDataCorp - Consulta de PEP

**Tipo:** POST (Síncrono)

```
URL:         https://plataforma.bigdatacorp.com.br/ondemand
MÉTODO:      POST
TIMEOUT:     ~2000ms
FORMATO:     JSON
AUTENTICAÇÃO: Bearer Token (JWT)
```

**Headers Obrigatórios:**
```
Content-Type: application/json
Authorization: Bearer {TOKEN}
AccessToken: {TOKEN_ID}
```

**Corpo da Requisição (JSON):**
```json
{
    "documento": "12345678901234",  // CNPJ ou CPF
    "tipoDocumento": "CNPJ",         // ou CPF
    "nome": "Empresa LTDA"
}
```

**Resposta (200 OK):**
```json
{
    "situacao": "ATIVA",
    "nome": "Empresa LTDA",
    "documento": "12345678901234",
    "statusPEP": "NAO_PEP",
    "renda_mensal": "150000.00",
    "patrimonio_liquido": "500000.00"
}
```

**Resposta (400 - Erro):**
```json
{
    "erro": true,
    "mensagem": "Documento inválido"
}
```

**Código C# de Integração:**

```csharp
public static HttpResponseResult ConsultaRepresentanteLegal(string documento, string nome)
{
    try
    {
        // Obter token do banco
        string token = ObterTokenBigDataCorp();
        
        if (string.IsNullOrEmpty(token))
        {
            return new HttpResponseResult 
            { 
                statusCode = 401, 
                content = "Token não disponível" 
            };
        }

        string url = "https://plataforma.bigdatacorp.com.br/ondemand";
        
        // Preparar payload
        dynamic payload = new ExpandoObject();
        payload.documento = documento;
        payload.tipoDocumento = documento.Length == 11 ? "CPF" : "CNPJ";
        payload.nome = nome;

        string json = JsonConvert.SerializeObject(payload);

        // Criar requisição
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/json";
        request.Timeout = 5000;
        
        // Headers
        request.Headers.Add("Authorization", $"Bearer {token}");
        request.Headers.Add("AccessToken", token);

        // Enviar payload
        byte[] data = Encoding.UTF8.GetBytes(json);
        request.ContentLength = data.Length;

        using (Stream stream = request.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }

        // Receber resposta
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string responseText = reader.ReadToEnd();

        return new HttpResponseResult
        {
            statusCode = (int)response.StatusCode,
            content = responseText
        };
    }
    catch (WebException ex)
    {
        HttpWebResponse response = (HttpWebResponse)ex.Response;
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string erro = reader.ReadToEnd();

        return new HttpResponseResult
        {
            statusCode = (int)response.StatusCode,
            content = erro
        };
    }
    catch (Exception ex)
    {
        LogarErro("ConsultaRepresentanteLegal", ex.Message);
        
        return new HttpResponseResult
        {
            statusCode = 500,
            content = ex.Message
        };
    }
}
```

---

### 3️⃣ HubCappta - Sincronização de Marketplace

**Tipo:** GET/POST (Assíncrono recomendado)

```
URL BASE:    https://api.posportal.com.br/api/hub
MÉTODO:      GET, POST
TIMEOUT:     ~3000ms
FORMATO:     JSON
AUTENTICAÇÃO: Bearer Token
```

**Headers Obrigatórios:**
```
Content-Type: application/json
Authorization: Bearer {TOKEN}
```

#### Endpoint: GET /onboarding/reseller/{document}

**Requisição:**
```
GET /onboarding/reseller/12345678901234 HTTP/1.1
Host: api.posportal.com.br
Authorization: Bearer {TOKEN}
```

**Resposta (200 OK):**
```json
{
    "id": "resel_12345",
    "document": "12345678901234",
    "legalRepresentative": {
        "name": "João Silva",
        "cpf": "12345678901"
    },
    "address": {
        "street": "Rua A",
        "number": "100",
        "city": "São Paulo",
        "state": "SP",
        "postalCode": "01311100"
    },
    "bankAccount": {
        "bank": "001",
        "agency": "0001",
        "account": "123456789"
    }
}
```

#### Endpoint: POST /onboarding/merchant

**Requisição:**
```json
POST /onboarding/merchant HTTP/1.1
Host: api.posportal.com.br
Authorization: Bearer {TOKEN}
Content-Type: application/json

{
    "resellerDocument": "12345678901234",
    "name": "Loja XYZ",
    "document": "98765432100001",
    "address": {
        "street": "Rua B",
        "number": "200",
        "city": "São Paulo",
        "state": "SP",
        "postalCode": "01311100"
    }
}
```

**Resposta (201 Created):**
```json
{
    "id": "merchant_98765",
    "status": "ACTIVE"
}
```

**Código C# de Integração:**

```csharp
public static bool VerificaMarketplace(string documento, ref string tokenCappta)
{
    try
    {
        // Obter token
        string token = ObterTokenHubCappta();
        
        if (string.IsNullEmpty(token))
            return false;

        string url = $"https://api.posportal.com.br/api/hub/onboarding/reseller/{documento}";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.Timeout = 5000;
        request.Headers.Add("Authorization", $"Bearer {token}");

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        
        if (response.StatusCode == HttpStatusCode.OK)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string responseText = reader.ReadToEnd();

            dynamic json = JsonConvert.DeserializeObject(responseText);
            tokenCappta = json["id"];

            // Salvar no BD
            SalvarTokenCappta(documento, json.ToString());

            return true;
        }

        return false;
    }
    catch
    {
        return false;
    }
}
```

---

## 🏁 FLUXO COMPLETO DE CONCLUSÃO DO CADASTRO

### PASSO 1: Validação Inicial (Client Side)

```javascript
// JavaScript - Validação
function ValidarFormulario() {
    // Email
    if (!ValidarEmail(document.getElementById('email').value)) {
        alert("Email inválido");
        return false;
    }

    // Senha
    if (!ValidarSenha(document.getElementById('senha').value)) {
        alert("Senha não atende aos requisitos");
        return false;
    }

    return true;
}

function ValidarEmail(email) {
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return regex.test(email);
}

function ValidarSenha(senha) {
    // Mínimo 6, máximo 32
    if (senha.length < 6 || senha.length > 32) return false;
    
    // Número
    if (!/\d/.test(senha)) return false;
    
    // Maiúscula
    if (!/[A-Z]/.test(senha)) return false;
    
    // Minúscula
    if (!/[a-z]/.test(senha)) return false;
    
    // Especial
    if (!/[!@#$%^&*]/.test(senha)) return false;
    
    // Sem espaço
    if (/\s/.test(senha)) return false;

    return true;
}
```

### PASSO 2: Submissão para Backend

```csharp
// C# - cad_representantes.aspx.cs
protected void btnSalvar_Click(object sender, EventArgs e)
{
    try
    {
        // 1. Validar email
        if (!Funcoes.IsEmail(txtEmailResponsavel.Text))
        {
            Page.ClientScript.RegisterClientScriptBlock(
                this.GetType(), "erro", 
                "alert('Email inválido');", true);
            return;
        }

        // 2. Validar senha
        if (!Funcoes.IsSenha(txtSenha.Text))
        {
            Page.ClientScript.RegisterClientScriptBlock(
                this.GetType(), "erro", 
                "alert('Senha não atende aos requisitos');", true);
            return;
        }

        // 3. Validar nome usuário
        if (string.IsNullOrWhiteSpace(txtNomeUsuario.Text))
        {
            Page.ClientScript.RegisterClientScriptBlock(
                this.GetType(), "erro", 
                "alert('Nome de usuário é obrigatório');", true);
            return;
        }

        // 4. Gerar código 2FA
        string codigo2FA = Funcoes.GerarCodigo2FA();
        
        // 5. Armazenar na sessão
        Funcoes.ArmazenarCodigo2FA(txtEmailResponsavel.Text, codigo2FA);

        // 6. Enviar email
        bool emailEnviado = Funcoes.Enviar2fa(
            txtEmailResponsavel.Text, 
            codigo2FA, 
            txtNome.Text);

        if (!emailEnviado)
        {
            Page.ClientScript.RegisterClientScriptBlock(
                this.GetType(), "erro", 
                "alert('Erro ao enviar email');", true);
            return;
        }

        // 7. Mostrar modal para digitar código
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
            "showModal", "document.getElementById('modal2FA').style.display='block';", true);
    }
    catch (Exception ex)
    {
        Funcoes.LogarErro("btnSalvar_Click", ex.Message);
    }
}
```

### PASSO 3: Validação de Código 2FA

```csharp
// C# - Confirmação do código 2FA
protected void btnConfirmar2FA_Click(object sender, EventArgs e)
{
    try
    {
        string emailUsuario = txtEmailResponsavel.Text;
        string codigoDigitado = txtCodigo2FA.Text;

        // 1. Validar código
        if (!Funcoes.ValidarCodigo2FA(emailUsuario, codigoDigitado))
        {
            Page.ClientScript.RegisterClientScriptBlock(
                this.GetType(), "erro", 
                "alert('Código inválido ou expirado');", true);
            return;
        }

        // 2. Chamar GravarDados para salvar no BD
        GravarDados();
    }
    catch (Exception ex)
    {
        Funcoes.LogarErro("btnConfirmar2FA_Click", ex.Message);
    }
}
```

### PASSO 4: Salvar no Banco de Dados

```csharp
// C# - Salvar dados
private void GravarDados()
{
    try
    {
        // 1. Verificar duplicidade (SP: H)
        SqlConnection myDup = new SqlConnection(Funcoes.conexao());
        myDup.Open();
        SqlCommand cmdDup = new SqlCommand("dbo.stp_pessoas_fj_ins", myDup);
        cmdDup.CommandType = CommandType.StoredProcedure;
        cmdDup.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "H";
        cmdDup.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = 
            Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdDup.Parameters.Add("@FLG_TIPO_PESSOA", SqlDbType.Char).Value = 
            ddlTipoFJ.SelectedValue; // PF ou PJ
        
        if (ddlTipoFJ.SelectedValue == "PJ")
            cmdDup.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = txtCNPJ.Text;
        else
            cmdDup.Parameters.Add("@NOM_CPF", SqlDbType.VarChar).Value = txtCPF.Text;

        SqlDataAdapter drDup = new SqlDataAdapter();
        drDup.SelectCommand = cmdDup;
        DataSet dsDup = new DataSet();
        drDup.Fill(dsDup, "DUP");

        if (dsDup.Tables["DUP"].Rows[0]["EXISTE"].ToString() == "1")
        {
            myDup.Close();
            Page.ClientScript.RegisterClientScriptBlock(
                this.GetType(), "erro", 
                "alert('Este documento já está cadastrado');", true);
            return;
        }

        myDup.Close();

        // 2. Criptografar senha
        string senhaCriptografada = Funcoes.Encrypt(txtSenha.Text);

        // 3. Inserir representante (SP: I)
        SqlConnection myIns = new SqlConnection(Funcoes.conexao());
        myIns.Open();
        SqlCommand cmdIns = new SqlCommand("dbo.stp_pessoas_fj_ins", myIns);
        cmdIns.CommandType = CommandType.StoredProcedure;
        cmdIns.CommandTimeout = 30;
        cmdIns.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "I";
        cmdIns.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = 
            Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdIns.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = 
            string.IsNullOrEmpty(ddlMarketplace.SelectedValue) ? 
            (object)DBNull.Value : Funcoes.strToInt(ddlMarketplace.SelectedValue);
        
        cmdIns.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "R"; // R = Representante
        cmdIns.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = txtRazaoSocial.Text;
        cmdIns.Parameters.Add("@NOM_FANTASIA", SqlDbType.VarChar).Value = txtNomeFantasia.Text;
        cmdIns.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = 
            ddlTipoFJ.SelectedValue == "PJ" ? txtCNPJ.Text : "";
        cmdIns.Parameters.Add("@NUM_TELEFONE", SqlDbType.VarChar).Value = txtTelefone.Text;
        cmdIns.Parameters.Add("@NOM_EMAIL_EMPRESA", SqlDbType.VarChar).Value = txtEmailEmpresa.Text;
        
        // Endereço
        cmdIns.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = txtEndereco.Text;
        cmdIns.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = txtNumero.Text;
        cmdIns.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = txtComplemento.Text;
        cmdIns.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = txtBairro.Text;
        cmdIns.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = txtCidade.Text;
        cmdIns.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = txtCEP.Text;
        cmdIns.Parameters.Add("@NOM_UF", SqlDbType.Char).Value = ddlUF.SelectedValue;
        
        // Responsável
        cmdIns.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNome.Text;
        cmdIns.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = txtSobrenome.Text;
        cmdIns.Parameters.Add("@NOM_CPF", SqlDbType.VarChar).Value = 
            ddlTipoFJ.SelectedValue == "PF" ? txtCPF.Text : "";
        cmdIns.Parameters.Add("@DTA_ANIVERSARIO", SqlDbType.DateTime).Value = 
            string.IsNullOrEmpty(txtDataNascimento.Text) ? 
            (object)DBNull.Value : Convert.ToDateTime(txtDataNascimento.Text);
        cmdIns.Parameters.Add("@NOM_MAE", SqlDbType.VarChar).Value = txtMae.Text;
        cmdIns.Parameters.Add("@NUM_RENDA_MENSAL", SqlDbType.Decimal).Value = 
            string.IsNullOrEmpty(txtRenda.Text) ? 
            (object)DBNull.Value : Convert.ToDecimal(txtRenda.Text);
        cmdIns.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmailResponsavel.Text;
        cmdIns.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = txtCelular.Text;
        
        // Usuário
        cmdIns.Parameters.Add("@NOM_NOME_USUARIO", SqlDbType.VarChar).Value = txtNomeUsuario.Text;
        cmdIns.Parameters.Add("@NOM_LOGIN", SqlDbType.VarChar).Value = txtEmailResponsavel.Text;
        cmdIns.Parameters.Add("@NOM_SENHA", SqlDbType.VarChar).Value = senhaCriptografada;
        
        // Flags
        cmdIns.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
        cmdIns.Parameters.Add("@FLG_PRESENCIAL", SqlDbType.Char).Value = "N";
        cmdIns.Parameters.Add("@FLG_POLITICAMENTE", SqlDbType.Char).Value = "N";
        cmdIns.Parameters.Add("@FLG_TIPO_PESSOA", SqlDbType.Char).Value = ddlTipoFJ.SelectedValue;
        cmdIns.Parameters.Add("@COD_ID_USUARIO_CRIACAO", SqlDbType.Int).Value = 
            Funcoes.strToInt(HttpContext.Current.Session["USUARIO"].ToString());

        object result = cmdIns.ExecuteScalar();
        int novoID = Funcoes.strToInt(result.ToString());

        myIns.Close();

        // 4. Registrar auditoria
        RegistrarAuditoria(txtCPF.Text, txtNome.Text, "Incluir", "Sistema");

        // 5. Iniciar integrações (assincronas)
        // BigDataCorp
        Task.Run(() => {
            BigDataCorp bdc = new BigDataCorp();
            bdc.ConsultaRepresentanteLegal(
                ddlTipoFJ.SelectedValue == "PJ" ? txtCNPJ.Text : txtCPF.Text,
                txtNome.Text);
        });

        // HubCappta
        if (!string.IsNullOrEmpty(ddlMarketplace.SelectedValue))
        {
            Task.Run(() => {
                HubCappta hc = new HubCappta();
                hc.VerificaMarketplace(ddlMarketplace.SelectedValue);
            });
        }

        // 6. Mensagem de sucesso
        Page.ClientScript.RegisterClientScriptBlock(
            this.GetType(), "sucesso", 
            "alert('Representante cadastrado com sucesso!'); window.close();", true);
    }
    catch (Exception ex)
    {
        Funcoes.LogarErro("GravarDados", ex.Message);
        Page.ClientScript.RegisterClientScriptBlock(
            this.GetType(), "erro", 
            $"alert('Erro ao salvar: {ex.Message}');", true);
    }
}
```

### PASSO 5: Integrações Assincronas (Background)

```csharp
// BigDataCorp (async)
Task.Run(async () => {
    try
    {
        BigDataCorp bdc = new BigDataCorp();
        var resultado = bdc.ConsultaRepresentanteLegal(documento, nome);
        
        // Salvar resultado no BD (CONSULTAS_FICHA_CADASTRO)
        RegistrarAuditoria(documento, nome, "Consultar", "BigDataCorp", resultado.content);
    }
    catch (Exception ex)
    {
        LogarErro("Integracao_BigDataCorp", ex.Message);
    }
});

// HubCappta (async)
Task.Run(async () => {
    try
    {
        HubCappta hc = new HubCappta();
        bool verificado = hc.VerificaMarketplace(marketplaceDoc);
        
        if (verificado)
        {
            // Salvar token no BD
            RegistrarAuditoria(marketplaceDoc, marketplace, "Sincronizar", "HubCappta");
        }
    }
    catch (Exception ex)
    {
        LogarErro("Integracao_HubCappta", ex.Message);
    }
});
```

---

## 📊 RESUMO VISUAL DO FLUXO COMPLETO

```
┌─────────────────────────────────────────────────────────┐
│ 1. FORMULÁRIO PREENCHIDO                                │
│    - Email, Senha, Documento, Endereço, etc             │
└──────────────────┬──────────────────────────────────────┘
                   ▼
┌─────────────────────────────────────────────────────────┐
│ 2. VALIDAÇÕES (Client + Server)                         │
│    - Email válido ✓                                      │
│    - Senha requisitos ✓                                 │
│    - Campos obrigatórios ✓                              │
└──────────────────┬──────────────────────────────────────┘
                   ▼
┌─────────────────────────────────────────────────────────┐
│ 3. GERAR E ENVIAR 2FA                                   │
│    - Código 6 dígitos gerado                            │
│    - Email SMTP enviado                                 │
│    - Código na sessão (10 min timeout)                  │
│    - Modal de confirmação exibida                       │
└──────────────────┬──────────────────────────────────────┘
                   ▼
┌─────────────────────────────────────────────────────────┐
│ 4. USUÁRIO DIGITA CÓDIGO 2FA                            │
│    - Recebe email com código                            │
│    - Digita no modal                                    │
│    - Clica confirmar                                    │
└──────────────────┬──────────────────────────────────────┘
                   ▼
┌─────────────────────────────────────────────────────────┐
│ 5. VALIDAR CÓDIGO (Backend)                             │
│    - Comparar com sessão                                │
│    - Se válido → prosseguir                             │
│    - Se inválido → exibir erro                          │
└──────────────────┬──────────────────────────────────────┘
                   ▼
┌─────────────────────────────────────────────────────────┐
│ 6. VERIFICAR DUPLICIDADE (SP: H)                        │
│    - SELECT COUNT WHERE CNPJ/CPF = ?                    │
│    - Se existe → erro                                   │
│    - Se não existe → continuar                          │
└──────────────────┬──────────────────────────────────────┘
                   ▼
┌─────────────────────────────────────────────────────────┐
│ 7. CRIPTOGRAFAR SENHA                                   │
│    - Usar função Encrypt()                              │
│    - Salvar hash, nunca plaintext                        │
└──────────────────┬──────────────────────────────────────┘
                   ▼
┌─────────────────────────────────────────────────────────┐
│ 8. INSERT NO BD (SP: I)                                 │
│    - INSERT INTO PESSOAS_FJ                             │
│    - Todos os campos preenchidos                        │
│    - FLG_ATIVO = 'S'                                    │
│    - Retorna novo COD_ID                                │
└──────────────────┬──────────────────────────────────────┘
                   ▼
┌─────────────────────────────────────────────────────────┐
│ 9. REGISTRAR AUDITORIA                                  │
│    - INSERT CONSULTAS_FICHA_CADASTRO                    │
│    - NOM_DOCUMENTO, NOM_OPERACAO, FLG_ORIGEM            │
│    - Timestamp = GETDATE()                              │
└──────────────────┬──────────────────────────────────────┘
                   ▼
┌─────────────────────────────────────────────────────────┐
│ 10. INTEGRAÇÕES (Async - Background)                    │
│    ├─ BigDataCorp: Consultar PEP                        │
│    │  └─ POST https://plataforma.bigdatacorp.br/        │
│    ├─ HubCappta: Sincronizar                            │
│    │  └─ GET https://api.posportal.com.br/              │
│    └─ Resultados salvos em CONSULTAS_FICHA_CADASTRO     │
└──────────────────┬──────────────────────────────────────┘
                   ▼
┌─────────────────────────────────────────────────────────┐
│ 11. SUCESSO                                              │
│    - "Representante cadastrado com sucesso!"            │
│    - Fechar janela / Redirecionar                       │
│    - Atualizar lista de representantes                  │
└─────────────────────────────────────────────────────────┘
```

---

## ✨ Próximos Passos

Este documento fornece:
- ✅ Scripts SQL completos para INSERT, UPDATE, DELETE
- ✅ Sistema de envio de email 2FA com template HTML
- ✅ Todas as URLs de integração (ViaCEP, BigDataCorp, HubCappta)
- ✅ Fluxo completo de conclusão do cadastro
- ✅ Código C# pronto para usar

**Para implementar a reescrita**, use este documento como referência para:
1. Criar as Stored Procedures no SQL Server
2. Implementar classes de integração (BigDataCorp, HubCappta, ViaCEP)
3. Criar serviço de Email SMTP
4. Estruturar o fluxo em ASP.NET Core/Entity Framework
5. Testar cada etapa em isolamento

---

**Data:** 8 de Maio de 2026  
**Versão:** 1.0  
**Status:** ✅ COMPLETO
