# FLUXO COMPLETO DE CADASTRO DE REPRESENTANTES

## 1. VISÃO GERAL

O sistema LegacyBank implementa um fluxo completo de cadastro de representantes com integrações com múltiplas APIs externas. Os representantes podem ser Pessoa Física (PF) ou Pessoa Jurídica (PJ) e estão vinculados a Marketplaces.

### Hierarquia do Sistema:
```
LICENCIADO (Proprietário)
    ├── MARKETPLACE (Vendedor/Revenda)
    │   ├── REPRESENTANTE (Agente de Vendas/Representação)
    │   │   └── ESTABELECIMENTO (Loja/Comerciante)
    │   └── ESTABELECIMENTO
    └── REPRESENTANTE (Direto ao Licenciado)
        └── ESTABELECIMENTO
```

---

## 2. FLUXO DO USUÁRIO - FRONT-END

### 2.1 Página de Cadastro
**Arquivo:** `[cad_representantes.aspx](cad_representantes.aspx)`

#### Seções do Formulário:

1. **1. Dados do Representante**
   - ID (readonly - auto gerado)
   - Tipo de Pessoa (PF/PJ) - combo
   - Visitou presencialmente? (S/N)
   - Marketplace Responsável - combo (carrega marketplaces ativos)
   - Importar dados do estabelecimento selecionado (apenas para Admin/Marketplace)

2. **2. Dados da Empresa**
   - Nome / Razão Social (obrigatório)
   - Apelido / Nome Fantasia (obrigatório)
   - CNPJ/CPF (obrigatório)
   - Telefone Empresa
   - Email Empresa
   - Tipo de Empresa - combo
   - Atividade Econômica (MCC) - combo
   - Faturamento Mensal
   - Patrimônio Líquido
   - Data Abertura / Data Início Atividade

3. **3. Endereço**
   - Endereço (obrigatório)
   - Número (obrigatório)
   - Complemento
   - Bairro (obrigatório)
   - Cidade (obrigatório)
   - Estado/UF (obrigatório)
   - CEP (obrigatório - com integração ViaCEP)
   - País (fixo: BR)

4. **4. Dados do Responsável/Sócio**
   - Nome (obrigatório)
   - Sobrenome (obrigatório)
   - CPF (obrigatório)
   - Data de Nascimento
   - Nome da Mãe
   - Renda Mensal (PF)
   - Email Responsável (obrigatório)
   - Celular (obrigatório)
   - Politicamente Exposto? (S/N)

5. **5. Dados de Usuário (Novo Cadastro Apenas)**
   - Nome de Usuário (obrigatório)
   - Email/Login (obrigatório - validado)
   - Senha (obrigatório - validações específicas)

### 2.2 Validações Front-End

```typescript
// Validações de Senha
- Mínimo 6 caracteres, máximo 32
- Pelo menos 1 número
- Pelo menos 1 letra maiúscula
- Pelo menos 1 letra minúscula
- Pelo menos 1 caractere especial
- Sem espaços

// Validações de Email
- Formato válido de email

// Validações Adicionais
- Tipo de Pessoa obrigatório
- Marketplace obrigatório
- Verificação de duplicidade por CNPJ/CPF (validação 'H' na SP)
```

### 2.3 Dinâmica de Campos

Quando altera **Tipo de Pessoa** (PF/PJ):
- Labels mudam dinamicamente
- Máscaras de input mudam (CPF ou CNPJ)
- Placeholders atualizam
- Validações mudam

Exemplo:
```
PF = Pessoa Física
├── Máscara CPF: 000.000.000-00
├── Label: "Nome Completo" (não "Razão Social")
├── Label: "CPF" (não "CNPJ")
└── Label: "Data Início Atividade" (não "Data Abertura")

PJ = Pessoa Jurídica
├── Máscara CNPJ: 00.000.000/0000-00
├── Label: "Razão Social"
├── Label: "CNPJ"
└── Label: "Data Abertura Empresa"
```

### 2.4 Integração ViaCEP

```csharp
// Quando usuário preenche CEP e sai do campo
// Faz requisição HTTPS para: https://viacep.com.br/ws/{CEP}/json

API Response:
{
  "logradouro": "Avenida...",
  "bairro": "Centro",
  "localidade": "São Paulo",
  "uf": "SP"
}

// Campos preenchidos automaticamente:
txtEndereco.Text = logradouro
txtBairro.Text = bairro
txtCidade.Text = localidade
ddlEstado.SelectedValue = uf
```

---

## 3. PROCESSAMENTO BACK-END

### 3.1 Arquivo de Processamento
**Arquivo:** `[cad_representantes.aspx.cs](cad_representantes.aspx.cs)`

#### Métodos Principais:

#### 3.1.1 `Page_Load()`
```csharp
// 1. Verifica autenticação do usuário
if (HttpContext.Current.Session.Count <= 0)
    Response.Redirect("login.aspx");

// 2. Carrega MCCs (Merchant Category Code)
// SP: dbo.stp_mcc_ins (operação: 'L')

// 3. Carrega Marketplaces Ativos
// SP: dbo.stp_pessoas_fj_ins
// Parâmetros:
//   - @flg_operacao = 'L'
//   - @COD_ID_PESSOA_LICENCIADO = [da sessão]
//   - @FLG_ATIVO = 'S'
//   - @FLG_TIPO = 'M' (Marketplace)

// 4. Se é edição (sid_id > 0)
//   Chama ConsultaFicha() para carregar dados existentes

// 5. Se é novo cadastro (sid_id = 0)
//   Mostra seção de usuário (dvUsuario.Visible = true)

// 6. Se é Admin ou Marketplace
//   Carrega lista de Estabelecimentos para importação
//   @FLG_TIPO = 'E' (Estabelecimento)
```

#### 3.1.2 `ConsultaFicha()`
```csharp
// Busca dados do representante para EDIÇÃO
// SP: dbo.stp_pessoas_fj_ins
// Parâmetros:
//   - @flg_operacao = 'S' (Select)
//   - @COD_ID = [ID do representante]
//   - @COD_ID_PESSOA_LICENCIADO = [da sessão]

// Popula todos os campos do formulário com os dados retornados
```

#### 3.1.3 `btnSalvar_Click()`
```csharp
// 1. Se é novo cadastro (sid_id <= 0)
//    - Valida Nome de Usuário (não pode ser vazio)
//    - Valida Email (deve ser válido)
//    - Valida Senha (atende requisitos complexos)

// 2. Envia código 2FA
// Função: Funcoes.Enviar2fa()
// Exibe modal de confirmação

// 3. Se confirmado no modal
//    Chama GravarDados()
```

#### 3.1.4 `GravarDados()`
```csharp
// 1. Se novo cadastro (sid_id == 0)
//    - Verifica duplicidade por CNPJ/CPF
//    - SP: dbo.stp_pessoas_fj_ins (@flg_operacao = 'H')
//    - Se existe: retorna erro

// 2. Executa SP com operação:
//    - 'I' = Insert (novo)
//    - 'A' = Update (edição)

// 3. SP: dbo.stp_pessoas_fj_ins
//    Parâmetros salvos:
//    - Dados da Empresa
//    - Dados do Endereço
//    - Dados do Responsável
//    - Dados de Usuário (novo cadastro)
//    - Marketplace
//    - Flags de Status
```

#### 3.1.5 `txtCEP_TextChanged()`
```csharp
// Integração com ViaCEP
// URL: https://viacep.com.br/ws/{CEP}/json
// Método: GET
// Protocolo: HTTPS (TLS 1.0, 1.1, 1.2, SSL 3)

// Response deserializado para classe CEPInfo
public class CEPInfo {
    logradouro
    bairro
    localidade
    uf
}

// Campos preenchidos automaticamente
```

#### 3.1.6 `ddlTipoFJ_SelectedIndexChanged()`
```csharp
// Atualiza labels e máscaras dinamicamente
// Se PF: CPF, Nome, Apelido, Data Início
// Se PJ: CNPJ, Razão Social, Nome Fantasia, Data Abertura
```

#### 3.1.7 `btnImportar_Click()`
```csharp
// Copia dados de um Estabelecimento selecionado
// Para preencher formulário de novo Representante
```

---

## 4. INTEGRAÇÕES COM APIS EXTERNAS

### 4.1 ViaCEP (Busca de Endereço)

**Arquivo:** `[cad_representantes.aspx.cs](cad_representantes.aspx.cs)` - método `txtCEP_TextChanged()`

```csharp
// URL: https://viacep.com.br/ws/{CEP}/json
// Método: GET
// Resposta: JSON

Request:
GET https://viacep.com.br/ws/01310100/json

Response:
{
  "cep": "01310-100",
  "logradouro": "Avenida Paulista",
  "complemento": "",
  "bairro": "Bela Vista",
  "localidade": "São Paulo",
  "uf": "SP",
  "ibge": "3550308",
  "gia": "",
  "ddd": "11",
  "siafi": "7107"
}
```

### 4.2 BigDataCorp (Dados de Representante Legal)

**Arquivo:** `[App_Code/bigdatacorp.cs](App_Code/bigdatacorp.cs)`

**Função:** `ConsultaRepresentanteLegal(string json)`

```csharp
// URL: https://plataforma.bigdatacorp.com.br/ondemand
// Método: POST
// Headers:
//   - Content-Type: application/json
//   - AccessToken: [Token BigDataCorp]
//   - TokenId: [ID BigDataCorp]

// Request JSON exemplo:
{
  "cpf": "12345678900",
  "tipoConsulta": "representante_legal"
}

// Response JSON:
{
  "situacao": "ATIVA",
  "nome": "João Silva",
  "cpf": "12345678900",
  "dataConsulta": "2024-05-08",
  "statusPEP": "NÃO_PEP"
}

// Armazenamento: Tabela CONSULTAS_FICHA_CADASTRO
// SP: dbo.stp_consulta_ficha_cadastro_ins
```

**Tokens (por Licenciado):**
- Armazenados em: `PESSOAS_FJ_INTEGRACOES_CHAVES`
- SP de acesso: `dbo.stp_pessoas_fj_integracoes_chaves_ins`
- Parâmetros: `@COD_ID_INTEGRACOES = 3` (BigDataCorp)

### 4.3 HubCappta (Integração de Revendedor/Marketplace)

**Arquivo:** `[App_Code/hubcappta.cs](App_Code/hubcappta.cs)`

**Função:** `VerificaMarketplace(string sMarketplace)`

Quando um Marketplace é consultado:

```csharp
// 1. Verifica se já existe no BD
// SP: dbo.stp_pessoas_fj_ins (@flg_operacao = 'H')
//     @NOM_CNPJ = sMarketplace
//     @FLG_TIPO = 'M'

// 2. Se não existe, consulta HubCappta
// Endpoint: GET /onboarding/reseller/{document}
// URL: https://api.posportal.com.br/api/hub/onboarding/reseller/{document}

// 3. Se encontrado, insere no BD
// SP: dbo.stp_pessoas_fj_ins (INSERT)
// Com dados retornados da API

// 4. Insere dados complementares
// SP: dbo.stp_pessoas_fj_cappta_ins (Dados Cappta)
// SP: dbo.stp_pessoas_fj_contas_ins (Dados Bancários)
```

**HubCappta API Endpoints para Representantes:**

```
GET  /onboarding/reseller                    # Lista revendedores
GET  /onboarding/reseller/{document}         # Consulta revendedor
POST /onboarding/reseller                    # Cadastra revendedor

GET  /onboarding/merchant                    # Lista lojas por revendedor
POST /onboarding/merchant                    # Cadastra loja
```

**Response Reseller:**
```json
{
  "reseller": {
    "document": "12345678901234",
    "companyName": "Empresa LTDA",
    "tradingName": "Nome Fantasia",
    "mccId": 5411,
    "legalNatureId": 2062,
    "status": "APPROVED"
  },
  "responsible": {
    "name": "João Silva",
    "cpf": "12345678900",
    "email": "joao@empresa.com",
    "phone": "1133334444",
    "mobilePhone": "11999998888"
  },
  "address": {
    "streetName": "Avenida Paulista",
    "houseNumber": "1000",
    "complement": "Apto 1200",
    "neighborhood": "Bela Vista",
    "city": "São Paulo",
    "state": "SP",
    "postalCode": "01311100"
  },
  "bankAccount": {
    "bankCode": "001",
    "accountType": "1",
    "branch": "0001",
    "account": "123456"
  },
  "statusDescription": "Aprovado"
}
```

---

## 5. ESTRUTURA DE BANCO DE DADOS

### 5.1 Tabelas Principais

#### PESSOAS_FJ (Tabela Mestre)
```sql
COD_ID                              INT PRIMARY KEY IDENTITY
FLG_TIPO_PESSOA                     CHAR (PF/PJ)
FLG_PRESENCIAL                      CHAR (S/N)
FLG_TIPO                            CHAR (R=Representante, E=Estabelecimento, M=Marketplace)
FLG_ATIVO                           CHAR (S/N)
FLG_INSERT_USUARIO                  CHAR (S/N) - Cria usuário automaticamente
FLG_POLITICAMENTE                   CHAR (S/N) - PEP

-- Dados da Empresa
NOM_RAZAOSOCIAL                     VARCHAR(255)
NOM_FANTASIA                        VARCHAR(255)
NOM_CNPJ                            VARCHAR(20)
NUM_TELEFONE                        VARCHAR(20)
NOM_EMAIL_EMPRESA                   VARCHAR(255)
COD_ID_MCC                          INT - Atividade Econômica
NOM_TIPO_EMPRESA                    VARCHAR(50)
NUM_FATURAMENTO                     FLOAT
NUM_PATRIMONIO                      FLOAT
DTA_ABERTURA                        DATETIME

-- Endereço
NOM_ENDERECO                        VARCHAR(255)
NOM_NUMERO                          VARCHAR(10)
NOM_COMPLEMENTO                     VARCHAR(255)
NOM_BAIRRO                          VARCHAR(255)
NOM_CIDADE                          VARCHAR(255)
NOM_UF                              VARCHAR(2)
NOM_PAIS                            VARCHAR(3)
NOM_CEP                             VARCHAR(10)

-- Dados do Responsável/Sócio
NOM_NOME                            VARCHAR(255)
NOM_SOBRENOME                       VARCHAR(255)
NOM_CPF                             VARCHAR(15)
DTA_ANIVERSARIO                     DATETIME
NOM_MAE                             VARCHAR(255)
NUM_RENDA_MENSAL                    FLOAT
NOM_EMAIL                           VARCHAR(255)
NOM_CELULAR                         VARCHAR(20)

-- Dados de Usuário
NOM_NOME_USUARIO                    VARCHAR(100)
NOM_LOGIN                           VARCHAR(255) - Email
NOM_SENHA                           VARCHAR(255) - Criptografada

-- Relacionamentos
COD_ID_PESSOA_LICENCIADO            INT - FK Licenciado
COD_ID_MARKETPLACE                  INT - FK Marketplace (se aplicável)

-- Auditoria
DTA_DATA                            DATETIME - Data criação/edição
```

#### PESSOAS_FJ_CAPPTA (Integração HubCappta)
```sql
COD_ID                              INT PRIMARY KEY IDENTITY
COD_ID_PESSOAS_FJ                   INT FK
COD_ID_PESSOA_LICENCIADO            INT FK
NUM_TOKEN_CAPPTA                    VARCHAR(20) - Document (CNPJ/CPF)
FLG_STATUS_CAPPTA                   VARCHAR(50) - Status da integração
FLG_CAPPTA                          CHAR (S/N) - Integrado com Cappta
DES_JSON_CAPPTA                     TEXT - Resposta JSON completa da API
COD_ID_NATUREZA_CAPPTA              INT - Legal Nature ID
DTA_DATA                            DATETIME
```

#### PESSOAS_FJ_CONTAS_INS (Dados Bancários)
```sql
COD_ID                              INT PRIMARY KEY IDENTITY
COD_ID_PESSOAS_FJ                   INT FK
COD_ID_PESSOA_LICENCIADO            INT FK
NOM_CODIGO_BANCO                    VARCHAR(10)
NOM_TIPO_BANCO                      CHAR (C=Corrente, P=Poupança)
NOM_NUMERO_AGENCIA_BANCO            VARCHAR(10)
NOM_NUMERO_DIGITO_AGENCIA_BANCO     VARCHAR(2)
NOM_NUMERO_CONTA_BANCO              VARCHAR(20)
NOM_NUMERO_DIGITO_CONTA_BANCO       VARCHAR(2)
FLG_PADRAO                          CHAR (S/N)
DTA_DATA                            DATETIME
```

#### PESSOAS_FJ_INTEGRACOES_CHAVES (Tokens de Integração)
```sql
COD_ID                              INT PRIMARY KEY IDENTITY
COD_ID_INTEGRACOES                  INT (1=?, 3=BigDataCorp, 5=Cappta)
COD_ID_PESSOA_LICENCIADO            INT FK
NOM_TOKEN                           VARCHAR(MAX)
NOM_ID                              VARCHAR(255)
FLG_ATIVO                           CHAR (S/N)
DTA_DATA                            DATETIME
```

#### CONSULTAS_FICHA_CADASTRO (Auditoria de Consultas)
```sql
COD_ID                              INT PRIMARY KEY IDENTITY
COD_ID_PESSOA_LICENCIADO            INT FK
COD_ID_PESSOAS_FJ                   INT FK
COD_ID_USUARIO                      INT FK
NOM_OPERACAO                        VARCHAR(50) - Ex: "REPRESENTANTE_LEGAL"
NOM_DOCUMENTO                       VARCHAR(20)
NOM_NOME                            VARCHAR(255)
FLG_ORIGEM                          VARCHAR(50) - Ex: "BIGDATACORP"
DTA_DATA                            DATETIME
```

### 5.2 Stored Procedures Principais

#### stp_pessoas_fj_ins
**Operações:**
- `'L'` = List (filtrado por licenciado/tipo)
- `'S'` = Select (um registro)
- `'I'` = Insert (novo)
- `'A'` = Update (edição)
- `'H'` = Exists/Hash (verifica duplicidade por CNPJ/CPF)
- `'D'` = Delete (soft delete)

**Exemplo Insert:**
```sql
EXEC dbo.stp_pessoas_fj_ins
    @flg_operacao = 'I',
    @COD_ID_PESSOA_LICENCIADO = 1,
    @FLG_TIPO = 'R',
    @FLG_TIPO_PESSOA = 'PJ',
    @FLG_PRESENCIAL = 'S',
    @NOM_RAZAOSOCIAL = 'Empresa LTDA',
    @NOM_CNPJ = '12345678901234',
    @NOM_EMAIL = 'email@empresa.com',
    @NOM_LOGIN = 'email@empresa.com',
    @NOM_SENHA = '[encrypted]',
    @NOM_NOME_USUARIO = 'usuario',
    @FLG_INSERT_USUARIO = 'S'
    -- ... outros parâmetros
```

#### stp_pessoas_fj_cappta_ins
**Operações:**
- `'I'` = Insert
- `'U'` = Update
- `'S'` = Select

#### stp_consulta_ficha_cadastro_ins
**Operações:**
- `'I'` = Insert (registra consulta realizada)

#### stp_mcc_ins
**Operações:**
- `'C'` = Lista todos MCCs (Merchant Category Codes)

---

## 6. FLUXO DETALHADO - PASSO A PASSO

### 6.1 Novo Cadastro de Representante (PJ)

```
1. USUÁRIO ACESSA PÁGINA
   ↓
2. PAGE_LOAD() executa:
   - Carrega MCCs
   - Carrega Marketplaces
   - Verifica se é novo (mostra seção usuário)
   - Carrega lista de Estabelecimentos para importação
   ↓
3. USUÁRIO PREENCHE FORMULÁRIO
   ├── Seleciona Tipo de Pessoa = "PJ"
   │   └── Atualiza labels/máscaras (CNPJ)
   ├── Digita CEP
   │   └── txtCEP_TextChanged()
   │       ├── Chamada HTTPS ViaCEP
   │       └── Popula Endereco, Bairro, Cidade, UF
   └── Preenche demais campos
   ↓
4. USUÁRIO CLICA "SALVAR"
   ↓
5. btnSalvar_Click() executa:
   ├── Valida Nome de Usuário (obrigatório)
   ├── Valida Email (formato)
   ├── Valida Senha (requisitos complexos)
   ├── Chamada Funcoes.Enviar2fa()
   │   └── Envia código por email
   └── Exibe Modal com campo para código
   ↓
6. USUÁRIO CONFIRMA 2FA
   ↓
7. GravarDados() executa:
   ├── Verifica duplicidade por CNPJ
   │   └── SP: stp_pessoas_fj_ins (@flg_operacao = 'H')
   │       └── Se existe: return erro
   ├── Criptografa senha
   ├── Executa SP: stp_pessoas_fj_ins (@flg_operacao = 'I')
   │   ├── Insere dados empresa
   │   ├── Insere dados endereço
   │   ├── Insere dados responsável
   │   ├── Insere dados usuário
   │   └── Retorna COD_ID (novo ID)
   └── Exibe mensagem sucesso + fecha janela
   ↓
8. TELA ANTERIOR RECARREGA
   └── Listagem atualizada com novo representante
```

### 6.2 Edição de Representante Existente

```
1. USUÁRIO CLICA NO REPRESENTANTE
   └── Abre janela com ?id=[encrypted_id]
   ↓
2. PAGE_LOAD() executa:
   ├── Descriptografa ID
   ├── sid_id = [decrypted_id]
   ├── Verifica se sid_id > 0 → É edição
   ├── Carrega dados via ConsultaFicha()
   └── Oculta seção usuário (dvUsuario.Visible = false)
   ↓
3. ConsultaFicha() executa:
   ├── SP: stp_pessoas_fj_ins (@flg_operacao = 'S')
   ├── @COD_ID = [id decrypted]
   └── Popula todos os campos
   ↓
4. USUÁRIO EDITA CAMPOS
   ↓
5. USUÁRIO CLICA "SALVAR"
   ├── btnSalvar_Click() - mesmas validações
   ├── Envia 2FA (confirmação de alteração)
   └── Exibe Modal
   ↓
6. USUÁRIO CONFIRMA 2FA
   ↓
7. GravarDados() executa:
   ├── Verifica sid_id > 0 → É Update
   ├── Criptografa senha (se alterada)
   ├── Executa SP: stp_pessoas_fj_ins (@flg_operacao = 'A')
   │   ├── @COD_ID = [id do representante]
   │   └── Atualiza todos os campos
   └── Exibe mensagem sucesso + fecha janela
```

### 6.3 Integração com HubCappta (Automática)

```
1. SISTEMA PROCESSA ESTABELECIMENTO
   └── Identifica marketplace CNPJ
   ↓
2. hubcappta.VerificaMarketplace() é chamado
   ├── Busca no BD se marketplace já existe
   │   └── SP: stp_pessoas_fj_ins (@flg_operacao = 'H')
   │       @FLG_TIPO = 'M'
   ├── Se não existe:
   │   └── Chamada API HubCappta
   │       ├── GET /onboarding/reseller/{document}
   │       └── URL: https://api.posportal.com.br/api/hub/...
   │
   ├── Se encontrado na API:
   │   ├── Insere PESSOAS_FJ (tipo='M')
   │   │   └── SP: stp_pessoas_fj_ins (@flg_operacao = 'I')
   │   ├── Insere PESSOAS_FJ_CAPPTA
   │   │   └── SP: stp_pessoas_fj_cappta_ins (@flg_operacao = 'I')
   │   └── Insere PESSOAS_FJ_CONTAS_INS
   │       └── SP: stp_pessoas_fj_contas_ins (@flg_operacao = 'I')
   └── Retorna COD_ID do marketplace
   ↓
3. SISTEMA CONTINUA FLUXO
   └── Marketplace agora está disponível para relacionamentos
```

---

## 7. FLUXO DE INTEGRAÇÕES SIMULTÂNEAS

### 7.1 Consulta BigDataCorp (Representante Legal)

```
QUANDO: Ao salvar representante com CPF/CNPJ específico

1. Sistema chama bigdatacorp.ConsultaRepresentanteLegal()
   ↓
2. Monta JSON da consulta:
   {
     "documento": "12345678900",
     "tipo": "REPRESENTANTE_LEGAL"
   }
   ↓
3. Faz POST para BigDataCorp:
   - URL: https://plataforma.bigdatacorp.com.br/ondemand
   - Headers: AccessToken, TokenId
   ↓
4. Recebe resposta com dados de representante legal:
   - Status cadastral
   - Dados pessoais
   - Status PEP (Politicamente Exposto)
   ↓
5. Armazena em CONSULTAS_FICHA_CADASTRO:
   - SP: stp_consulta_ficha_cadastro_ins
   - Registra: documento, nome, operação, origem, data
   ↓
6. Valida dados contra o cadastro:
   - Verifica consistências
   - Alerta se há discrepâncias
```

### 7.2 Sincronização com HubCappta (Assincrono)

```
QUANDO: Representante é vinculado a um Marketplace que usa Cappta

1. Sistema identifica marketplace CNPJ
   ↓
2. Verifica se marketplace está em PESSOAS_FJ_CAPPTA
   ├── Se sim: Pula para passo 5
   └── Se não: Continua
   ↓
3. Consulta HubCappta API:
   GET /onboarding/reseller/{marketplace_document}
   ↓
4. Recebe dados: contatos, banco, documentos
   ↓
5. Armazena dados relacionados:
   - PESSOAS_FJ_CAPPTA: Status e JSON
   - PESSOAS_FJ_CONTAS_INS: Dados bancários
   ↓
6. Disponibiliza para:
   - Processamento de transações
   - Integração com POS
   - Gestão financeira
```

---

## 8. ESTRUTURA DE DIRETÓRIOS RELACIONADOS

```
legacybank-main/
├── cad_representantes.aspx          # Página HTML
├── cad_representantes.aspx.cs       # Code-Behind C#
│
├── App_Code/
│   ├── bigdatacorp.cs               # Integração BigDataCorp
│   ├── hubcappta.cs                 # Integração HubCappta
│   ├── funcoes.cs                   # Funções utilitárias
│   ├── tabelas.cs                   # Consultas de tabelas
│   ├── asaas.cs                     # Integração Asaas
│   ├── erp.cs                       # Integração ERP
│   ├── dados.cs                     # Classes de dados
│   └── ... (outras integrações)
│
└── (Outras páginas relacionadas)
    ├── cad_estabelecimentos.aspx    # Cadastro de Estabelecimentos
    ├── cad_estabelecimentos_*.aspx  # Variações (API, Assinatura, etc)
    └── ...
```

---

## 9. VALIDAÇÕES E REGRAS DE NEGÓCIO

### 9.1 Validações Obrigatórias

| Campo | Validação | Tipo |
|-------|-----------|------|
| Tipo de Pessoa | Seleção obrigatória | Combo |
| Nome/Razão Social | Não pode estar vazio | Text |
| CNPJ/CPF | Deve ser único no licenciado | Text |
| Email | Formato válido | Email |
| Senha (novo) | Ver requisitos complexos | Password |
| Marketplace | Seleção obrigatória | Combo |
| Presencial | Seleção obrigatória (S/N) | Combo |

### 9.2 Validação de Senha

```
REQUISITOS OBRIGATÓRIOS:
✓ Mínimo 6 caracteres
✓ Máximo 32 caracteres
✓ Pelo menos 1 número (0-9)
✓ Pelo menos 1 letra maiúscula (A-Z)
✓ Pelo menos 1 letra minúscula (a-z)
✓ Pelo menos 1 caractere especial (!@#$%^&*)
✗ Não permitir espaços

FUNÇÃO: Funcoes.IsSenha(string senha)
```

### 9.3 Validação de Email

```
FUNÇÃO: Funcoes.IsEmail(string email)

REGRA: Deve conter @ e domínio válido
```

### 9.4 Regras de Duplicidade

```
VERIFICAÇÃO: Mesmo CNPJ/CPF já existe?

POR LICENCIADO:
- Cada licenciado pode ter representantes com mesmo CNPJ
- Mas não pode ter DOIS representantes com mesmo CNPJ

VERIFICAÇÃO: SP stp_pessoas_fj_ins
  @flg_operacao = 'H'  (Hash/Exists)
  @NOM_CNPJ = [valor]
  @FLG_TIPO = 'R'
  @COD_ID_PESSOA_LICENCIADO = [sessão]
```

### 9.5 Regras de Marketplace

```
SE Usuário é MARKETPLACE:
├── Não pode alterar marketplace selecionado
├── Marketplace fica locked na combo
└── Marketplace do representante = Marketplace logado

SE Usuário é ADMIN:
├── Pode selecionar qualquer marketplace
└── Pode criar representante sem marketplace específico
```

---

## 10. FLUXO DE 2FA (Two-Factor Authentication)

### 10.1 Envio do Código

```csharp
// Função: Funcoes.Enviar2fa()

PROCESSO:
1. Gera código aleatório (ex: 6 dígitos)
2. Armazena código na sessão com timeout
3. Envia email para NOM_EMAIL_RESPONSAVEL com código
4. Retorna true se sucesso, false se falha
5. Exibe modal para usuário digitar código
```

### 10.2 Confirmação do Código

```
1. Modal solicita código
2. Usuário digita código recebido por email
3. Clica "Confirmar"
4. Valida código contra sessão
5. Se válido:
   ├── Executa GravarDados()
   └── Salva representante no BD
6. Se inválido:
   └── Exibe erro e solicita novo código
```

---

## 11. TABELAS RELACIONADAS (Referências)

### Tabelas que Consultam Representantes

| Tabela | Relação | Campo |
|--------|---------|-------|
| PESSOAS_FJ_CAPPTA | 1:1 | COD_ID_PESSOAS_FJ |
| PESSOAS_FJ_CONTAS_INS | 1:N | COD_ID_PESSOAS_FJ |
| PESSOAS_FJ_DOCUMENTOS | 1:N | COD_ID_PESSOAS_FJ |
| PESSOAS_FJ_ASSINATURAS | 1:N | COD_ID_PESSOAS_FJ |
| PESSOAS_FJ_BAAS | 1:1 | COD_ID_PESSOAS_FJ |
| PESSOAS_FJ_ERP | 1:1 | COD_ID_PESSOAS_FJ |
| ESTABELECIMENTOS | 1:N | COD_ID_REPRESENTANTE |
| EQUIPAMENTOS | N:M | COD_ID_REPRESENTANTE |
| CONSULTAS_FICHA_CADASTRO | 1:N | COD_ID_PESSOAS_FJ |

---

## 12. PLANO DE MIGRAÇÃO / REESCRITA

### 12.1 Oportunidades de Refatoração

**Áreas para Melhoria:**

1. **Separação de Responsabilidades**
   - Movér lógica de validação para classe separada
   - Movér chamadas de API para camada de integração
   - Implementar Repository Pattern para acesso a dados

2. **Tratamento de Erros**
   - Adicionar try-catch mais específicos
   - Implementar logging centralizado
   - Criar custom exceptions

3. **Segurança**
   - Implementar rate limiting no 2FA
   - Adicionar validação de CSRF
   - Audit trail mais detalhado

4. **Performance**
   - Adicionar cache para MCCs e Marketplaces
   - Implementar lazy loading de dados
   - Adicionar índices no BD

5. **Testes**
   - Criar testes unitários para validações
   - Testes de integração com APIs externas
   - Testes de banco de dados

6. **Modernização**
   - Migrar de WebForms para ASP.NET Core/Blazor
   - Integração com Angular frontend existente
   - API RESTful separada

### 12.2 Dependências para Reescrita

```
Antes de reescrever, garantir:
✓ Backup completo dos dados
✓ Scripts de migração testados
✓ Documentação das integrações
✓ Testes com dados reais
✓ Rollback plan definido
✓ Aprovação de stakeholders
```

---

## 13. SUMÁRIO EXECUTIVO

### Fluxo Simplificado

```
USUÁRIO → FORMULÁRIO (Front-End)
         ↓
      VALIDAÇÕES
         ↓
      2FA EMAIL
         ↓
   BACKEND (C#)
   ├── Verifica duplicidade
   ├── Criptografa dados
   ├── Insere no BD
   └── Cria usuário
         ↓
   INTEGRAÇÕES (Assíncronas)
   ├── BigDataCorp (PEP)
   ├── HubCappta (Marketplace)
   └── ViaCEP (Endereço)
         ↓
   RESULTADO
   ├── Representante criado
   ├── Usuário de acesso criado
   └── Dados sincronizados
```

### Tecnologias Utilizadas

```
Backend:        ASP.NET WebForms, C#
Banco de Dados: SQL Server
APIs Externas:  BigDataCorp, HubCappta/CapPTA, ViaCEP
Frontend:       Angular, TypeScript, HTML/CSS
Segurança:      2FA, Criptografia, Validações
```

---

**Última Atualização:** 8 de Maio de 2026  
**Status:** Análise Completa - Pronto para Reescrita
