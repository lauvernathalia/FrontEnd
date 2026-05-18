# DIAGRAMAS E DETALHES TÉCNICOS - CADASTRO DE REPRESENTANTES

## 1. DIAGRAMA DE FLUXO - NOVO CADASTRO

```
┌─────────────────────────────────────────────────────────────────────┐
│                    NOVO CADASTRO DE REPRESENTANTE                   │
└─────────────────────────────────────────────────────────────────────┘

                                START
                                  │
                    ┌─────────────▼────────────────┐
                    │  Abre cad_representantes.aspx│
                    │     com ?id=0 (novo)         │
                    └─────────────┬────────────────┘
                                  │
                    ┌─────────────▼────────────────┐
                    │  Page_Load() executa:        │
                    │  ✓ Carrega MCCs              │
                    │  ✓ Carrega Marketplaces      │
                    │  ✓ Mostra seção de usuário   │
                    └─────────────┬────────────────┘
                                  │
                ┌─────────────────▼─────────────────┐
                │   Usuário Preenche Formulário     │
                │  ┌──────────────────────────────┐ │
                │  │ Tipo Pessoa (PF/PJ)          │ │
                │  │ Razão Social/Nome            │ │
                │  │ CNPJ/CPF                     │ │
                │  │ Telefone/Email               │ │
                │  │ Endereço + CEP               │ │
                │  │ Dados Responsável            │ │
                │  │ Usuário/Email/Senha          │ │
                │  └──────────────────────────────┘ │
                └──────────────────┬────────────────┘
                                   │
                        Preenche CEP │ Dispara evento
                                   │ txtCEP_TextChanged()
                                   │
        ┌──────────────────────────▼──────────────────────────┐
        │    INTEGRAÇÃO ViaCEP (Endereço)                     │
        │  ┌───────────────────────────────────────────────┐  │
        │  │ GET https://viacep.com.br/ws/{CEP}/json      │  │
        │  │                                               │  │
        │  │ Response:                                     │  │
        │  │ {                                             │  │
        │  │   "logradouro": "Av. Paulista",              │  │
        │  │   "bairro": "Bela Vista",                    │  │
        │  │   "localidade": "São Paulo",                 │  │
        │  │   "uf": "SP"                                 │  │
        │  │ }                                             │  │
        │  │                                               │  │
        │  │ Popula:                                       │  │
        │  │ - txtEndereco                                │  │
        │  │ - txtBairro                                  │  │
        │  │ - txtCidade                                  │  │
        │  │ - ddlEstado                                  │  │
        │  └───────────────────────────────────────────────┘  │
        └──────────────────────┬───────────────────────────────┘
                               │
                    ┌──────────▼──────────┐
                    │  Usuário clica      │
                    │  "SALVAR"           │
                    └──────────┬──────────┘
                               │
                    ┌──────────▼──────────────────────┐
                    │  btnSalvar_Click()               │
                    │  Validações:                     │
                    │  ✓ Nome de usuário (not empty)  │
                    │  ✓ Email (formato válido)       │
                    │  ✓ Senha (requisitos complexos) │
                    └──────────┬──────────────────────┘
                               │
                    ┌──────────▼──────────────────┐
                    │  Validações OK?              │
                    └─┬──────────────────────────┬─┘
                    SIM│                         │NÃO
                      │                    Exibe erro
                      │                         │
                      │                    Return
                      │
            ┌─────────▼─────────────────┐
            │  Envio 2FA (Email)         │
            │  Funcoes.Enviar2fa()       │
            │                            │
            │  - Gera código 6 dígitos  │
            │  - Envia por email         │
            │  - Exibe modal para código │
            └─────────┬─────────────────┘
                      │
            ┌─────────▼──────────────────┐
            │  Modal: Confirmar Código    │
            │  Usuário digita código      │
            └─────────┬──────────────────┘
                      │
            ┌─────────▼──────────────────┐
            │  Código Válido?             │
            └─┬──────────────────────────┬─┘
            SIM│                         │NÃO
              │                    Solicita novo
              │
        ┌─────▼────────────────────────────┐
        │  GravarDados()                     │
        │                                   │
        │  1. Verificar duplicidade:        │
        │     SP: stp_pessoas_fj_ins        │
        │     @flg_operacao = 'H'           │
        │     @NOM_CNPJ = [valor]          │
        │     @FLG_TIPO = 'R'              │
        │                                   │
        │  2. Existe? ──SIM──► Erro        │
        │     │                             │
        │     NÃO                           │
        │     │                             │
        │  3. Criptografa senha             │
        │                                   │
        │  4. Executa INSERT:               │
        │     SP: stp_pessoas_fj_ins        │
        │     @flg_operacao = 'I'           │
        │     + todos os parâmetros         │
        │                                   │
        │  5. Retorna novo COD_ID           │
        │                                   │
        │  6. Cria usuário de acesso        │
        │     (automático se FLG_INSERT_USUARIO='S')
        │                                   │
        │  7. OPCIONAL - Integração com:    │
        │     • BigDataCorp (PEP)           │
        │     • HubCappta (Marketplace)     │
        └─────┬────────────────────────────┘
              │
        ┌─────▼──────────────────────┐
        │  Salvo com sucesso          │
        │  alert("Dados gravados...")  │
        │  Fecha janela               │
        │  Recarrega listagem         │
        └─────┬──────────────────────┘
              │
            END
```

---

## 2. DIAGRAMA DE FLUXO - EDIÇÃO

```
┌─────────────────────────────────────────────────────────────────┐
│                 EDIÇÃO DE REPRESENTANTE EXISTENTE               │
└─────────────────────────────────────────────────────────────────┘

                            START
                              │
              ┌───────────────▼─────────────┐
              │ Usuário clica representante  │
              │ (lista de representantes)    │
              │ ?id=[encrypted_id]           │
              └───────────────┬─────────────┘
                              │
              ┌───────────────▼──────────────┐
              │ Page_Load() executa:         │
              │ ✓ Descriptografa ID          │
              │ ✓ sid_id = [decrypted]       │
              │ ✓ Verifica: sid_id > 0?     │
              │   └─ É edição! (SIM)         │
              │ ✓ ConsultaFicha()            │
              │ ✓ Oculta seção usuário       │
              └───────────────┬──────────────┘
                              │
              ┌───────────────▼──────────────┐
              │ ConsultaFicha()              │
              │                              │
              │ SP: stp_pessoas_fj_ins       │
              │ @flg_operacao = 'S'          │
              │ @COD_ID = [id decrypted]     │
              │                              │
              │ Popula TODOS os campos:      │
              │ - Dados empresa              │
              │ - Dados endereço             │
              │ - Dados responsável          │
              │ - Dados usuário (read-only)  │
              │ - Marketplace (locked)       │
              └───────────────┬──────────────┘
                              │
              ┌───────────────▼──────────────┐
              │ Usuário edita campos         │
              │ (sem alterar usuário)        │
              └───────────────┬──────────────┘
                              │
              ┌───────────────▼──────────────┐
              │ Usuário clica "SALVAR"       │
              └───────────────┬──────────────┘
                              │
              ┌───────────────▼──────────────────────┐
              │ btnSalvar_Click()                    │
              │ Mesmas validações de novo cadastro  │
              │ (mas NÃO valida duplicidade)         │
              └───────────────┬──────────────────────┘
                              │
              ┌───────────────▼──────────────┐
              │ Envio 2FA (confirmação)      │
              │ Exibe modal para código      │
              └───────────────┬──────────────┘
                              │
              ┌───────────────▼──────────────┐
              │ Usuário confirma código      │
              └───────────────┬──────────────┘
                              │
              ┌───────────────▼──────────────┐
              │ GravarDados()                │
              │                              │
              │ Verifica: sid_id > 0?        │
              │ └─ É Update (SIM)            │
              │                              │
              │ SP: stp_pessoas_fj_ins       │
              │ @flg_operacao = 'A'          │
              │ @COD_ID = [id do repr.]      │
              │ + todos os parâmetros        │
              │   atualizados                │
              │                              │
              │ (NÃO cria novo usuário)      │
              └───────────────┬──────────────┘
                              │
              ┌───────────────▼──────────────┐
              │ Atualizado com sucesso       │
              │ alert("Dados gravados...")   │
              │ Fecha janela                 │
              │ Recarrega listagem           │
              └───────────────┬──────────────┘
                              │
                            END
```

---

## 3. DIAGRAMA DE INTEGRAÇÕES

```
┌──────────────────────────────────────────────────────────────────────┐
│                  INTEGRAÇÕES COM APIS EXTERNAS                       │
└──────────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────────┐
│                                                                      │
│  ┌────────────────────────────────────────────────────────────────┐  │
│  │                  ViaCEP - Endereço                             │  │
│  │  ═══════════════════════════════════════════════════════════   │  │
│  │                                                                │  │
│  │  QUANDO: Usuário preenche CEP                                 │  │
│  │  EVENTO: txtCEP_TextChanged()                                 │  │
│  │                                                                │  │
│  │  REQUEST:                                                     │  │
│  │  ├─ Método: GET                                              │  │
│  │  ├─ URL: https://viacep.com.br/ws/{CEP}/json                │  │
│  │  └─ Headers: Accept: application/json                        │  │
│  │                                                                │  │
│  │  RESPONSE (JSON):                                             │  │
│  │  {                                                            │  │
│  │    "cep": "01310-100",                                       │  │
│  │    "logradouro": "Avenida Paulista",                         │  │
│  │    "complemento": "",                                        │  │
│  │    "bairro": "Bela Vista",                                   │  │
│  │    "localidade": "São Paulo",                                │  │
│  │    "uf": "SP",                                               │  │
│  │    "ibge": "3550308",                                        │  │
│  │    "gia": "",                                                │  │
│  │    "ddd": "11",                                              │  │
│  │    "siafi": "7107"                                           │  │
│  │  }                                                            │  │
│  │                                                                │  │
│  │  TRATAMENTO:                                                 │  │
│  │  ├─ Deserializa para class CEPInfo                          │  │
│  │  ├─ Popula campos:                                           │  │
│  │  │   • txtEndereco = logradouro                             │  │
│  │  │   • txtBairro = bairro                                   │  │
│  │  │   • txtCidade = localidade                               │  │
│  │  │   • ddlEstado = uf                                       │  │
│  │  └─ Tratamento de erro: try-catch                           │  │
│  │                                                                │  │
│  │  PROTOCOLO: HTTPS (TLS 1.0, 1.1, 1.2, SSL3)                 │  │
│  └────────────────────────────────────────────────────────────────┘  │
│                                                                      │
│  ┌────────────────────────────────────────────────────────────────┐  │
│  │              BigDataCorp - Dados Representante Legal           │  │
│  │  ═══════════════════════════════════════════════════════════   │  │
│  │                                                                │  │
│  │  QUANDO: Ao salvar representante (opcional)                   │  │
│  │  FUNÇÃO: bigdatacorp.ConsultaRepresentanteLegal()            │  │
│  │                                                                │  │
│  │  URL: https://plataforma.bigdatacorp.com.br/ondemand          │  │
│  │  MÉTODO: POST                                                 │  │
│  │  HEADERS:                                                     │  │
│  │  ├─ Content-Type: application/json                           │  │
│  │  ├─ AccessToken: [Token BigDataCorp]                        │  │
│  │  └─ TokenId: [ID BigDataCorp]                               │  │
│  │                                                                │  │
│  │  REQUEST (JSON):                                              │  │
│  │  {                                                            │  │
│  │    "cpf": "12345678900",                                     │  │
│  │    "tipoConsulta": "representante_legal"                    │  │
│  │  }                                                            │  │
│  │                                                                │  │
│  │  RESPONSE (JSON):                                             │  │
│  │  {                                                            │  │
│  │    "situacao": "ATIVA",                                      │  │
│  │    "nome": "João Silva",                                     │  │
│  │    "cpf": "12345678900",                                     │  │
│  │    "dataConsulta": "2024-05-08",                             │  │
│  │    "statusPEP": "NÃO_PEP",                                   │  │
│  │    "rendaAnual": 500000.00,                                 │  │
│  │    "patrimonio": 1000000.00                                  │  │
│  │  }                                                            │  │
│  │                                                                │  │
│  │  ARMAZENAMENTO:                                               │  │
│  │  └─ Tabela: CONSULTAS_FICHA_CADASTRO                        │  │
│  │     SP: stp_consulta_ficha_cadastro_ins                     │  │
│  │     ├─ @COD_ID_PESSOAS_FJ = [ID representante]             │  │
│  │     ├─ @NOM_DOCUMENTO = [CPF]                               │  │
│  │     ├─ @NOM_NOME = [Nome completo]                          │  │
│  │     ├─ @NOM_OPERACAO = "REPRESENTANTE_LEGAL"                │  │
│  │     ├─ @FLG_ORIGEM = "BIGDATACORP"                          │  │
│  │     └─ @DTA_DATA = [data/hora]                              │  │
│  │                                                                │  │
│  │  TOKEN (por Licenciado):                                      │  │
│  │  ├─ Tabela: PESSOAS_FJ_INTEGRACOES_CHAVES                  │  │
│  │  ├─ Coluna: NOM_TOKEN                                       │  │
│  │  ├─ Parâmetro: @COD_ID_INTEGRACOES = 3                      │  │
│  │  └─ Buscado por: @COD_ID_PESSOA_LICENCIADO                  │  │
│  │                                                                │  │
│  │  TRATAMENTO DE ERRO:                                          │  │
│  │  ├─ 400: Dados inválidos                                    │  │
│  │  ├─ 401: Erro de autenticação                               │  │
│  │  ├─ 500: Erro servidor                                      │  │
│  │  └─ Retorna: HttpResponseResult (statusCode, content)       │  │
│  └────────────────────────────────────────────────────────────────┘  │
│                                                                      │
│  ┌────────────────────────────────────────────────────────────────┐  │
│  │           HubCappta - Integração Marketplace/Revendedor        │  │
│  │  ═══════════════════════════════════════════════════════════   │  │
│  │                                                                │  │
│  │  QUANDO: Ao processar transação com marketplace               │  │
│  │  FUNÇÃO: hubcappta.VerificaMarketplace()                     │  │
│  │          hubcappta.VerificaEstabelecimento()                 │  │
│  │                                                                │  │
│  │  URL BASE: https://api.posportal.com.br/api/hub              │  │
│  │                                                                │  │
│  │  ENDPOINTS:                                                   │  │
│  │  ├─ GET  /onboarding/reseller                               │  │
│  │  ├─ GET  /onboarding/reseller/{document}                    │  │
│  │  ├─ POST /onboarding/reseller                               │  │
│  │  ├─ GET  /onboarding/merchant?resellerDocument={doc}       │  │
│  │  ├─ GET  /onboarding/merchant/{id}?resellerDocument={doc}  │  │
│  │  ├─ POST /onboarding/merchant                               │  │
│  │  ├─ GET  /plan                                              │  │
│  │  ├─ GET  /plan/{id}                                         │  │
│  │  ├─ GET  /pos/device                                        │  │
│  │  ├─ POST /pos/device                                        │  │
│  │  └─ GET  /payment?{filtros}                                 │  │
│  │                                                                │  │
│  │  HEADERS COMUNS:                                              │  │
│  │  ├─ Authorization: Bearer [Token Cappta]                     │  │
│  │  ├─ Content-Type: application/json                           │  │
│  │  └─ Accept: application/json                                 │  │
│  │                                                                │  │
│  │  TOKEN (por Licenciado):                                      │  │
│  │  ├─ Tabela: PESSOAS_FJ_INTEGRACOES_CHAVES                  │  │
│  │  ├─ Parâmetro: @COD_ID_INTEGRACOES = 5                      │  │
│  │  └─ Coluna: NOM_TOKEN                                       │  │
│  │                                                                │  │
│  │  FLUXO VerificaMarketplace():                                 │  │
│  │  ┌─ Verifica se marketplace já existe no BD                 │  │
│  │  │  └─ SP: stp_pessoas_fj_ins                               │  │
│  │  │     @flg_operacao = 'H'                                  │  │
│  │  │     @FLG_TIPO = 'M'                                      │  │
│  │  │                                                            │  │
│  │  ├─ Se não existe:                                            │  │
│  │  │  ├─ GET /onboarding/reseller/{document}                 │  │
│  │  │  ├─ Insere dados em PESSOAS_FJ (tipo='M')               │  │
│  │  │  │  └─ SP: stp_pessoas_fj_ins (@flg_operacao = 'I')     │  │
│  │  │  ├─ Insere dados Cappta em PESSOAS_FJ_CAPPTA            │  │
│  │  │  │  └─ SP: stp_pessoas_fj_cappta_ins                    │  │
│  │  │  └─ Insere dados bancários em PESSOAS_FJ_CONTAS_INS      │  │
│  │  │     └─ SP: stp_pessoas_fj_contas_ins                    │  │
│  │  │                                                            │  │
│  │  └─ Retorna: COD_ID do marketplace                          │  │
│  │                                                                │  │
│  │  RESPONSE /onboarding/reseller/{doc}:                         │  │
│  │  {                                                            │  │
│  │    "reseller": {                                             │  │
│  │      "document": "12345678901234",                           │  │
│  │      "companyName": "Empresa LTDA",                         │  │
│  │      "tradingName": "Nome Fantasia",                        │  │
│  │      "mccId": 5411,                                         │  │
│  │      "legalNatureId": 2062,                                 │  │
│  │      "status": "APPROVED"                                   │  │
│  │    },                                                        │  │
│  │    "responsible": {                                          │  │
│  │      "name": "João Silva",                                  │  │
│  │      "cpf": "12345678900",                                  │  │
│  │      "email": "joao@empresa.com",                           │  │
│  │      "phone": "1133334444",                                 │  │
│  │      "mobilePhone": "11999998888"                           │  │
│  │    },                                                        │  │
│  │    "address": {                                              │  │
│  │      "streetName": "Avenida Paulista",                      │  │
│  │      "houseNumber": "1000",                                 │  │
│  │      "complement": "Apto 1200",                             │  │
│  │      "neighborhood": "Bela Vista",                          │  │
│  │      "city": "São Paulo",                                   │  │
│  │      "state": "SP",                                         │  │
│  │      "postalCode": "01311100"                               │  │
│  │    },                                                        │  │
│  │    "bankAccount": {                                          │  │
│  │      "bankCode": "001",                                     │  │
│  │      "accountType": "1",                                    │  │
│  │      "branch": "0001",                                      │  │
│  │      "account": "123456"                                    │  │
│  │    },                                                        │  │
│  │    "statusDescription": "Aprovado"                          │  │
│  │  }                                                            │  │
│  │                                                                │  │
│  │  ARMAZENAMENTO:                                               │  │
│  │  ├─ PESSOAS_FJ                                              │  │
│  │  │  └─ FLG_TIPO = 'M'                                       │  │
│  │  ├─ PESSOAS_FJ_CAPPTA                                       │  │
│  │  │  ├─ NUM_TOKEN_CAPPTA = document                         │  │
│  │  │  ├─ FLG_STATUS_CAPPTA = statusDescription               │  │
│  │  │  ├─ DES_JSON_CAPPTA = resposta completa                 │  │
│  │  │  └─ COD_ID_NATUREZA_CAPPTA = legalNatureId              │  │
│  │  └─ PESSOAS_FJ_CONTAS_INS                                  │  │
│  │     ├─ NOM_CODIGO_BANCO = bankCode                         │  │
│  │     ├─ NOM_TIPO_BANCO = C/P                                │  │
│  │     ├─ NOM_NUMERO_AGENCIA_BANCO = branch                   │  │
│  │     ├─ NOM_NUMERO_CONTA_BANCO = account                    │  │
│  │     └─ FLG_PADRAO = 'S'                                     │  │
│  └────────────────────────────────────────────────────────────────┘  │
│                                                                      │
└──────────────────────────────────────────────────────────────────────┘
```

---

## 4. DIAGRAMA DE CAMADAS - ARQUITETURA

```
┌──────────────────────────────────────────────────────────────────────┐
│                        CAMADAS DA APLICAÇÃO                          │
└──────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────┐
│  CAMADA 1 - APRESENTAÇÃO (UI)                                       │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  Tecnologia: ASP.NET WebForms, JavaScript, jQuery, InputMask        │
│                                                                     │
│  Componentes:                                                       │
│  ├─ cad_representantes.aspx                                        │
│  │   ├─ Formulário HTML/ASPX                                       │
│  │   ├─ TextBox, DropDownList, Button                              │
│  │   ├─ Validação client-side (JavaScript)                         │
│  │   └─ Masks (CPF/CNPJ via InputMask)                            │
│  │                                                                  │
│  ├─ Eventos JavaScript:                                             │
│  │   ├─ ddlTipoFJ.change → Atualiza labels/máscaras               │
│  │   ├─ txtCEP.blur → Dispara busca ViaCEP                        │
│  │   ├─ btnSalvar.click → Validações e envio 2FA                  │
│  │   └─ btnConfirmar2FA.click → Confirma código                    │
│  │                                                                  │
│  └─ Modal:                                                          │
│     └─ mdConfirmar - Código 2FA                                    │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
                                  │
                    ┌─────────────▼─────────────┐
                    │   HTTP POST/GET Request   │
                    │   (ASP.NET ViewState)     │
                    └─────────────┬─────────────┘
                                  │
┌─────────────────────────────────▼─────────────────────────────────┐
│  CAMADA 2 - LÓGICA DE NEGÓCIO (Code-Behind)                        │
├──────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  Tecnologia: C#, ASP.NET WebForms                                   │
│                                                                     │
│  Arquivo: cad_representantes.aspx.cs                               │
│                                                                     │
│  Classes e Métodos:                                                │
│  ┌─ public partial class cad_representantes                        │
│  │                                                                  │
│  ├─ Properties:                                                     │
│  │   └─ sid_id (ID descriptografado)                              │
│  │                                                                  │
│  ├─ Event Handlers:                                                │
│  │   ├─ Page_Load()                                               │
│  │   ├─ btnSalvar_Click()                                         │
│  │   ├─ btnCancelar_Click()                                       │
│  │   ├─ txtCEP_TextChanged()                                      │
│  │   ├─ ddlTipoFJ_SelectedIndexChanged()                          │
│  │   └─ btnImportar_Click()                                       │
│  │                                                                  │
│  ├─ Private Methods:                                                │
│  │   ├─ ConsultaFicha()                                           │
│  │   └─ GravarDados()                                             │
│  │                                                                  │
│  └─ Validações:                                                     │
│      ├─ Validação de usuário                                      │
│      ├─ Validação de email                                        │
│      ├─ Validação de senha (requisitos complexos)                 │
│      └─ Verificação de duplicidade                                │
│                                                                     │
│  Integrações Chamadas:                                              │
│  ├─ Funcoes.strToInt(), Funcoes.strToDouble()                     │
│  ├─ Funcoes.IsEmail(), Funcoes.IsSenha()                          │
│  ├─ Funcoes.Encrypt(), Funcoes.Decrypt()                          │
│  ├─ Funcoes.Enviar2fa()                                           │
│  └─ Funcoes.conexao() - Connection string                         │
│                                                                     │
└──────────────────────────────────────────────────────────────────────┘
                                  │
                    ┌─────────────▼─────────────┐
                    │   SqlCommand, DataReader  │
                    │   (ADO.NET)               │
                    └─────────────┬─────────────┘
                                  │
┌─────────────────────────────────▼──────────────────────────────────┐
│  CAMADA 3 - ACESSO A DADOS (Data Access)                           │
├──────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  Tecnologia: SQL Server, ADO.NET, SqlClient                        │
│                                                                     │
│  Componentes:                                                       │
│  ├─ SqlConnection                                                  │
│  ├─ SqlCommand                                                     │
│  ├─ SqlDataReader / SqlDataAdapter                                 │
│  └─ SqlParameter                                                   │
│                                                                     │
│  Stored Procedures Chamadas:                                        │
│  ├─ dbo.stp_mcc_ins                                               │
│  ├─ dbo.stp_pessoas_fj_ins                                        │
│  ├─ dbo.stp_pessoas_fj_cappta_ins                                 │
│  ├─ dbo.stp_pessoas_fj_contas_ins                                 │
│  ├─ dbo.stp_pessoas_fj_integracoes_chaves_ins                     │
│  └─ dbo.stp_consulta_ficha_cadastro_ins                           │
│                                                                     │
│  Padrão de Execução:                                                │
│  1. Abre conexão: SqlConnection.Open()                             │
│  2. Define comando: SqlCommand com SP                              │
│  3. Adiciona parâmetros: Parameters.Add()                          │
│  4. Executa: ExecuteReader(), ExecuteNonQuery(), ExecuteScalar()   │
│  5. Fecha: Connection.Close() + Dispose()                          │
│                                                                     │
└──────────────────────────────────────────────────────────────────────┘
                                  │
                    ┌─────────────▼─────────────┐
                    │   SQL Server Database     │
                    │   (Tabelas + SP)          │
                    └─────────────┬─────────────┘
                                  │
┌─────────────────────────────────▼──────────────────────────────────┐
│  CAMADA 4 - BANCO DE DADOS (Database)                              │
├──────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  SQL Server (com SP)                                                │
│                                                                     │
│  Tabelas Principais:                                                │
│  ├─ PESSOAS_FJ                                                     │
│  ├─ PESSOAS_FJ_CAPPTA                                             │
│  ├─ PESSOAS_FJ_CONTAS_INS                                         │
│  ├─ PESSOAS_FJ_INTEGRACOES_CHAVES                                 │
│  ├─ EQUIPAMENTOS_MODELOS (MCC)                                    │
│  ├─ CONSULTAS_FICHA_CADASTRO                                     │
│  └─ (outras...)                                                    │
│                                                                     │
│  Índices para Performance:                                          │
│  ├─ PESSOAS_FJ.NOM_CNPJ (unique por licenciado)                  │
│  ├─ PESSOAS_FJ.COD_ID_PESSOA_LICENCIADO                          │
│  ├─ PESSOAS_FJ.FLG_TIPO                                           │
│  └─ PESSOAS_FJ_CAPPTA.COD_ID_PESSOAS_FJ                          │
│                                                                     │
└──────────────────────────────────────────────────────────────────────┘
                                  │
        ┌─────────────────────────┴──────────────────────────┐
        │                                                    │
┌───────▼────────────┐                         ┌────────────▼──────┐
│  APIS EXTERNAS 1   │                         │  APIS EXTERNAS 2  │
├────────────────────┤                         ├───────────────────┤
│                    │                         │                   │
│  ViaCEP            │                         │  BigDataCorp      │
│  ─────────         │                         │  ────────────     │
│  URL: viacep.com   │                         │  URL: plataforma. │
│  Busca endereço    │                         │  bigdatacorp.com  │
│  por CEP           │                         │  Consulta PEP,    │
│                    │                         │  dados legal       │
│                    │                         │                   │
└────────────────────┘                         └───────────────────┘

        ┌─────────────────────────┐
        │                         │
┌───────▼──────────────────┐  ┌───▼─────────────────────┐
│  APIS EXTERNAS 3         │  │  APIS EXTERNAS 4       │
├──────────────────────────┤  ├────────────────────────┤
│                          │  │                        │
│  HubCappta               │  │  Asaas                 │
│  ──────────              │  │  ─────                 │
│  URL: api.posportal.com  │  │  (Integração Bancária) │
│  Revendedor/Marketplace  │  │                        │
│  Lojista/Merchant        │  │                        │
│  Planos/POS              │  │                        │
│                          │  │                        │
└──────────────────────────┘  └────────────────────────┘
```

---

## 5. MAPEAMENTO DE FUNÇÕES

```
┌─────────────────────────────────────────────────────────────────────┐
│               FUNÇÃO → STORED PROCEDURE → TABELAS                   │
└─────────────────────────────────────────────────────────────────────┘

Page_Load()
│
├─→ Carrega MCCs
│   └─→ SP: stp_mcc_ins (@flg_operacao = 'C')
│       └─→ EQUIPAMENTOS_MODELOS (MCC)
│
├─→ Carrega Marketplaces
│   └─→ SP: stp_pessoas_fj_ins
│       ├─ @flg_operacao = 'L'
│       ├─ @FLG_TIPO = 'M'
│       └─ PESSOAS_FJ
│
├─→ Carrega Estabelecimentos (import)
│   └─→ SP: stp_pessoas_fj_ins
│       ├─ @flg_operacao = 'L'
│       ├─ @FLG_TIPO = 'E'
│       └─ PESSOAS_FJ
│
├─→ Verifica Integração Cappta
│   └─→ SP: stp_pessoas_fj_integracoes_chaves_ins
│       ├─ @COD_ID_INTEGRACOES = 5
│       └─ PESSOAS_FJ_INTEGRACOES_CHAVES
│
└─→ Se é edição: ConsultaFicha()
    └─→ SP: stp_pessoas_fj_ins (@flg_operacao = 'S')
        └─ PESSOAS_FJ (um registro)

ConsultaFicha()
│
└─→ SP: stp_pessoas_fj_ins (@flg_operacao = 'S')
    ├─ @COD_ID = [ID]
    └─ PESSOAS_FJ (popula campos)

btnSalvar_Click()
│
├─→ Funcoes.IsSenha() - Valida senha
├─→ Funcoes.IsEmail() - Valida email
├─→ Funcoes.Enviar2fa() - Envia código 2FA
│   └─ Email (SMTP)
│
└─→ Espera confirmação modal → GravarDados()

GravarDados()
│
├─→ Se novo cadastro:
│   ├─→ SP: stp_pessoas_fj_ins (@flg_operacao = 'H')
│   │   ├─ Verifica duplicidade CNPJ
│   │   └─ PESSOAS_FJ (Select com Where)
│   │
│   └─→ SP: stp_pessoas_fj_ins (@flg_operacao = 'I')
│       ├─ Insere novo representante
│       ├─ Cria usuário de acesso (se FLG_INSERT_USUARIO='S')
│       ├─ Retorna novo COD_ID
│       └─ PESSOAS_FJ (Insert + IDENTITY)
│
├─→ Se edição:
│   └─→ SP: stp_pessoas_fj_ins (@flg_operacao = 'A')
│       ├─ Atualiza representante existente
│       └─ PESSOAS_FJ (Update)
│
└─→ Integrações automáticas (optional):
    ├─→ hubcappta.VerificaMarketplace()
    │   └─→ HubCappta API + tabelas
    │
    └─→ bigdatacorp.ConsultaRepresentanteLegal()
        └─→ BigDataCorp API + CONSULTAS_FICHA_CADASTRO

txtCEP_TextChanged()
│
├─→ ServicePointManager.SecurityProtocol = (múltiplos protocolos)
├─→ HTTP GET para ViaCEP
│   └─ URL: https://viacep.com.br/ws/{CEP}/json
│
├─→ Deserializa JSON → CEPInfo
│
└─→ Popula campos:
    ├─ txtEndereco
    ├─ txtBairro
    ├─ txtCidade
    └─ ddlEstado

ddlTipoFJ_SelectedIndexChanged()
│
├─→ Se "PF":
│   ├─ Máscara: 999.999.999-99 (CPF)
│   ├─ Label "Nome Completo"
│   ├─ Label "CPF"
│   └─ Label "Data Início Atividade"
│
└─→ Se "PJ":
    ├─ Máscara: 99.999.999/9999-99 (CNPJ)
    ├─ Label "Razão Social"
    ├─ Label "CNPJ"
    └─ Label "Data Abertura Empresa"

btnImportar_Click()
│
└─→ Copia dados do Estabelecimento selecionado
    ├─ txtRazaoSocial
    ├─ txtFantasia
    ├─ txtCNPJ
    ├─ Dados endereço
    ├─ Dados responsável
    └─ (popula formulário para edição)
```

---

## 6. CICLO DE VIDA - REPRESENTANTE

```
┌─────────────────────────────────────────────────────────┐
│        CICLO DE VIDA DO CADASTRO DE REPRESENTANTE       │
└─────────────────────────────────────────────────────────┘

ESTADO 1: NÃO CADASTRADO
  │
  ├─ Ação: Clica em "Novo Representante"
  │
  ▼
ESTADO 2: EM PREENCHIMENTO
  │ Campos vazios, formulário aberto
  │
  ├─ Ação: Preenche dados e clica "Salvar"
  │
  ▼
ESTADO 3: VALIDANDO
  │ Email, Senha, Duplicidade
  │
  ├─ Se erro: Volta para ESTADO 2
  │
  ├─ Se OK: Envia 2FA
  │
  ▼
ESTADO 4: AGUARDANDO 2FA
  │ Código enviado por email
  │
  ├─ Se não confirma: Timeout e volta para ESTADO 2
  │
  ├─ Se confirma código: Grava dados
  │
  ▼
ESTADO 5: CADASTRADO (ATIVO)
  │ FLG_ATIVO = 'S'
  │ Usuário criado e pode fazer login
  │ Aparece em listas de representantes
  │
  ├─ Ação: Clica para editar
  │
  ▼
ESTADO 6: EM EDIÇÃO
  │ Formulário preenchido com dados existentes
  │ Usuário não pode ser alterado
  │ Marketplace fica locked
  │
  ├─ Ação: Altera dados e clica "Salvar"
  │
  ▼
ESTADO 7: VALIDANDO EDIÇÃO
  │ Mesmas validações (sem verificar duplicidade)
  │ Envia 2FA de confirmação
  │
  ├─ Se erro: Volta para ESTADO 6
  │
  ├─ Se OK: Atualiza dados
  │
  ▼
ESTADO 8: CADASTRADO (ATIVO) - Atualizado
  │ Dados sincronizados
  │
  ├─ Pode ser vinculado a Estabelecimentos
  │ ├─ Cada Representante pode ter N Estabelecimentos
  │ └─ COD_ID_REPRESENTANTE na tabela PESSOAS_FJ (tipo='E')
  │
  ├─ Pode ser vinculado a Marketplace
  │ └─ COD_ID_MARKETPLACE na tabela PESSOAS_FJ
  │
  ├─ Pode fazer transações
  │ └─ Integração com HubCappta
  │
  └─ Pode ser desativado
     └─ FLG_ATIVO = 'N' (soft delete)
```

---

## 7. MAPEAMENTO DE ERROS

```
┌──────────────────────────────────────────────────────────┐
│           ERROS E TRATAMENTOS                             │
└──────────────────────────────────────────────────────────┘

VALIDAÇÃO - NOME DE USUÁRIO VAZIO
├─ Mensagem: "O nome do usuário não pode ser deixado..."
├─ Alert: ClientScript.RegisterStartupScript()
├─ Ação: Return (não grava)
└─ Retry: Usuário preenche e tenta novamente

VALIDAÇÃO - EMAIL INVÁLIDO
├─ Função: Funcoes.IsEmail()
├─ Mensagem: "O e-mail digitado não é válido..."
├─ Alert: ClientScript.RegisterStartupScript()
├─ Ação: Return (não grava)
└─ Retry: Usuário corrige email

VALIDAÇÃO - SENHA INVÁLIDA
├─ Função: Funcoes.IsSenha()
├─ Requisitos não atendidos:
│  ├─ Menos de 6 caracteres
│  ├─ Mais de 32 caracteres
│  ├─ Sem número
│  ├─ Sem letra maiúscula
│  ├─ Sem letra minúscula
│  ├─ Sem caractere especial
│  └─ Contém espaço
│
├─ Mensagem principal: "A Senha digitada não é válida..."
├─ Mensagem detalhada: "ATENÇÃO! A senha deve atender..."
├─ Alert: Dois alerts (erro + requisitos)
├─ Ação: Return (não grava)
└─ Retry: Usuário corrige senha

DUPLICIDADE - CNPJ/CPF JÁ EXISTE
├─ Verificação: SP stp_pessoas_fj_ins (@flg_operacao = 'H')
├─ Condição: Mesmo CNPJ/CPF + mesmo Licenciado
├─ Mensagem: "Já existe um estabelecimento cadastrado..."
│            + ID do existente + Razão Social
├─ Alert: ClientScript.RegisterStartupScript()
├─ Ação: Return (não grava)
└─ Opção: Editar representante existente

ERRO 2FA - NÃO ENVIADO
├─ Função: Funcoes.Enviar2fa()
├─ Retorna: false
├─ Mensagem: "Ocorreu um erro ao tentar enviar..."
├─ Alert: ClientScript.RegisterStartupScript()
└─ Retry: Usuário pode tentar salvar novamente

ERRO CEP NÃO ENCONTRADO
├─ ViaCEP API retorna vazio
├─ Tratamento: try-catch
├─ Resultado: Campos não preenchidos
├─ Mensagem: Nenhuma (falha silenciosa)
└─ Retry: Usuário digita CEP manualmente

ERRO SEGURANÇA - USUÁRIO NÃO AUTENTICADO
├─ Verificação: HttpContext.Current.Session.Count <= 0
├─ Ação: Response.Redirect("login.aspx")
├─ FormsAuthentication.SignOut()
└─ Resultado: Redireciona para login

ERRO DATABASE - CONNECTION FAILED
├─ Tratamento: try-catch block (mínimo)
├─ SqlException pode ser capturada
├─ Mensagem genérica exibida
└─ Ação: Log do erro (sistema)

ERRO CAPPTA - MARKETPLACE NÃO ENCONTRADO
├─ HubCappta retorna null/empty
├─ Tratamento: catch block
├─ Atribuição: sMarketplace = "0"
├─ Resultado: Não sincroniza dados
└─ Impacto: Representante criado sem dados Cappta

ERRO BIGDATACORP - CONSULTA FALHA
├─ Retorna: HttpResponseResult (statusCode, errorJson)
├─ StatusCode 400: Dados inválidos
├─ StatusCode 401: Erro autenticação
├─ StatusCode 500: Erro servidor
├─ Tratamento: try-catch + WebException
├─ Resultado: Consulta não é registrada
└─ Impacto: Representante criado sem dados PEP
```

---

## 8. CONSIDERAÇÕES DE PERFORMANCE

```
BOTTLENECKS IDENTIFICADOS:

1. MÚLTIPLAS CONEXÕES AO BD
   Problema: Page_Load() abre 3-4 conexões sequenciais
   Solução: Usar connection pool ou consolidar queries
   
2. INTEGRAÇÃO CEP SÍNCRONA
   Problema: Usuário espera resposta ViaCEP (IO wait)
   Solução: Fazer assíncrono ou com callback
   
3. SEM CACHE DE MCCs/MARKETPLACES
   Problema: Carrega a cada Page_Load()
   Solução: Implementar cache em memória ou distribuído
   
4. BUSCA DUPLICIDADE ANTES DE INSERT
   Problema: Duas queries (H + I)
   Solução: Usar constraint UNIQUE no BD + tratamento exceção
   
5. INTEGRAÇÃO CAPPTA/BIGDATACORP SÍNCRONA
   Problema: Aguarda resposta de APIs externas
   Solução: Fila de processamento assincrono (Job)

RECOMENDAÇÕES:

├─ Implementar async/await
├─ Usar Entity Framework ou Dapper
├─ Cache distribuído (Redis)
├─ Fila para integrações (Azure Queue/RabbitMQ)
├─ Índices no BD (CNPJ, Licenciado)
├─ Paginação em listas grandes
├─ CDN para assets estáticos
└─ Monitoring e alertas
```

---

**Documento Complementar ao:** [FLUXO_CADASTRO_REPRESENTANTES.md](FLUXO_CADASTRO_REPRESENTANTES.md)

**Versão:** 2.0  
**Atualização:** 8 de Maio de 2026  
**Status:** Análise Técnica Completa
