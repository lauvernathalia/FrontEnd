# GUIA DE REESCRITA - CADASTRO DE REPRESENTANTES

## 1. INTRODUÇÃO

Este documento fornece um roadmap completo para reescrever o sistema de cadastro de representantes, migrando de WebForms ASP.NET para uma arquitetura moderna com ASP.NET Core/Blazor e Angular.

---

## 2. ARQUITETURA ALVO

### 2.1 Stack de Tecnologia Recomendado

```
FRONTEND:
├─ Angular 17+
├─ TypeScript 5+
├─ Material Design / Tailwind CSS
├─ RxJS (Reactive Programming)
└─ Jest + Cypress (Testes)

BACKEND:
├─ ASP.NET Core 8.0 LTS
├─ C# 12
├─ Entity Framework Core 8
├─ AutoMapper (DTO mapping)
├─ FluentValidation (Validações)
├─ Serilog (Logging)
├─ MediatR (CQRS)
└─ xUnit + Moq (Testes)

INFRAESTRUTURA:
├─ SQL Server 2019+
├─ Redis (Cache)
├─ Azure Service Bus (Filas)
├─ Azure Key Vault (Secrets)
├─ Docker & Kubernetes
└─ GitHub Actions (CI/CD)

INTEGRACOES EXTERNAS:
├─ BigDataCorp
├─ HubCappta
├─ ViaCEP
└─ SendGrid (Email)
```

### 2.2 Padrões Arquiteturais

```
┌─────────────────────────────────────────────────────────┐
│                   ARQUITETURA LIMPA                     │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────────────────────────────────────────────┐  │
│  │  PRESENTATION LAYER (Controllers/Endpoints)      │  │
│  │  ├─ RepresentantesController                     │  │
│  │  │  ├─ POST /api/representantes                 │  │
│  │  │  ├─ GET /api/representantes/{id}            │  │
│  │  │  ├─ PUT /api/representantes/{id}            │  │
│  │  │  ├─ DELETE /api/representantes/{id}         │  │
│  │  │  └─ PATCH /api/representantes/{id}/confirm  │  │
│  │  └─ ValidationExceptionFilter                   │  │
│  │     └─ GlobalExceptionHandler                   │  │
│  └──────────────────────────────────────────────────┘  │
│                         ▲                               │
│                         │ Dependency Injection          │
│                         ▼                               │
│  ┌──────────────────────────────────────────────────┐  │
│  │  APPLICATION LAYER (Services/Use Cases)          │  │
│  │  ├─ CreateRepresentanteCommand                  │  │
│  │  ├─ UpdateRepresentanteCommand                  │  │
│  │  ├─ GetRepresentanteQuery                       │  │
│  │  ├─ ValidarDuplicidadeHandler                   │  │
│  │  ├─ Enviar2FAHandler                            │  │
│  │  ├─ IntegracaoCappataHandler                    │  │
│  │  └─ IntegracaoBigDatacorpHandler                │  │
│  └──────────────────────────────────────────────────┘  │
│                         ▲                               │
│                         │                               │
│                         ▼                               │
│  ┌──────────────────────────────────────────────────┐  │
│  │  DOMAIN LAYER (Entities/Rules)                   │  │
│  │  ├─ Representante (Entity)                      │  │
│  │  │  ├─ TipoPessoa (Value Object)               │  │
│  │  │  ├─ Documento (Value Object)                │  │
│  │  │  └─ Endereco (Value Object)                 │  │
│  │  ├─ RepresentanteRepositories (Interface)      │  │
│  │  └─ RepresentanteDomainRules                   │  │
│  └──────────────────────────────────────────────────┘  │
│                         ▲                               │
│                         │                               │
│                         ▼                               │
│  ┌──────────────────────────────────────────────────┐  │
│  │  INFRASTRUCTURE LAYER                            │  │
│  │  ├─ Repositories (Entity Framework)             │  │
│  │  │  └─ RepresentanteRepository                  │  │
│  │  ├─ Integrations                                │  │
│  │  │  ├─ ViaCEPService                            │  │
│  │  │  ├─ BigDatacorpService                       │  │
│  │  │  ├─ CapptaService                            │  │
│  │  │  └─ EmailService                             │  │
│  │  └─ Database Context                            │  │
│  │     └─ LegacybankDbContext                      │  │
│  └──────────────────────────────────────────────────┘  │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

---

## 3. FASE 1: PREPARAÇÃO (1-2 semanas)

### 3.1 Audit Completo do Sistema Atual

#### Checklist:

- [ ] Documentar todos os campos da tabela PESSOAS_FJ
- [ ] Listar todas as regras de validação
- [ ] Mapear todas as integrações (endpoints, formatos, erros)
- [ ] Identificar dados legados/inativos
- [ ] Documentar triggers/constraints do BD
- [ ] Listar todas as camadas de segurança atuais
- [ ] Criar testes de regressão (dados atuais)

#### Script de Auditoria:

```sql
-- Verificar estrutura da tabela
EXEC sp_help 'PESSOAS_FJ'

-- Verificar constraints
SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS 
WHERE TABLE_NAME = 'PESSOAS_FJ'

-- Verificar triggers
SELECT * FROM sys.triggers 
WHERE OBJECT_NAME(parent_id) = 'PESSOAS_FJ'

-- Contar registros
SELECT FLG_TIPO, COUNT(*) 
FROM PESSOAS_FJ 
GROUP BY FLG_TIPO

-- Verificar índices
EXEC sp_helpindex 'PESSOAS_FJ'
```

### 3.2 Criar Ambiente de Desenvolvimento

```bash
# Criar estrutura do projeto
mkdir lgc-representantes-api
cd lgc-representantes-api

# Criar soluções
dotnet new sln -n LGC.Representantes

# Criar projetos
dotnet new webapi -n LGC.Representantes.API
dotnet new classlib -n LGC.Representantes.Application
dotnet new classlib -n LGC.Representantes.Domain
dotnet new classlib -n LGC.Representantes.Infrastructure
dotnet new xunit -n LGC.Representantes.Tests

# Adicionar à solução
dotnet sln add LGC.Representantes.API
dotnet sln add LGC.Representantes.Application
dotnet sln add LGC.Representantes.Domain
dotnet sln add LGC.Representantes.Infrastructure
dotnet sln add LGC.Representantes.Tests
```

### 3.3 Setup de Dependências

```csharp
// LGC.Representantes.API.csproj
<ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="8.0.0" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.4.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
    <PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.0" />
    <PackageReference Include="FluentValidation.DependencyInjectionExtensions" Version="11.8.0" />
    <PackageReference Include="MediatR" Version="12.1.1" />
    <PackageReference Include="MediatR.Extensions.Microsoft.DependencyInjection" Version="11.0.0" />
    <PackageReference Include="Serilog.AspNetCore" Version="7.0.0" />
    <PackageReference Include="StackExchange.Redis" Version="2.6.121" />
</ItemGroup>
```

---

## 4. FASE 2: MODELAGEM DO DOMÍNIO (2-3 semanas)

### 4.1 Entities (Domain Layer)

```csharp
// Domain/Entities/Representante.cs
public class Representante : AggregateRoot
{
    public int Id { get; set; }
    public int LicenciadoId { get; set; }
    public int? MarketplaceId { get; set; }
    public int? UsuarioId { get; set; }
    
    // Dados Básicos
    public TipoPessoa TipoPessoa { get; set; }
    public bool VisitadoPresencialmente { get; set; }
    
    // Dados da Empresa
    public string RazaoSocial { get; set; }
    public string NomeFantasia { get; set; }
    public Documento Documento { get; set; } // CNPJ ou CPF
    public string TelefoneEmpresa { get; set; }
    public string EmailEmpresa { get; set; }
    public int MCCId { get; set; }
    public TipoEmpresa TipoEmpresa { get; set; }
    public decimal FaturamentoMensal { get; set; }
    public decimal PatrimonioLiquido { get; set; }
    public DateTime? DataAbertura { get; set; }
    
    // Endereço
    public Endereco Endereco { get; set; }
    
    // Responsável/Sócio
    public Responsavel Responsavel { get; set; }
    
    // Status
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    public int CriadoPor { get; set; }
    public int? AtualizadoPor { get; set; }
    
    // Métodos de Negócio
    public static Result<Representante> Criar(
        int licenciadoId,
        int? marketplaceId,
        TipoPessoa tipoPessoa,
        string razaoSocial,
        string nomeFantasia,
        string documento,
        Endereco endereco,
        Responsavel responsavel,
        int criadoPor)
    {
        // Validações de domínio
        var erros = new List<string>();
        
        if (string.IsNullOrWhiteSpace(razaoSocial))
            erros.Add("Razão Social é obrigatória");
            
        if (tipoPessoa == TipoPessoa.PessoaFisica && documento.Length != 11)
            erros.Add("CPF inválido");
            
        if (tipoPessoa == TipoPessoa.PessoaJuridica && documento.Length != 14)
            erros.Add("CNPJ inválido");
        
        if (erros.Any())
            return Result<Representante>.Fail(string.Join(", ", erros));
        
        var representante = new Representante
        {
            LicenciadoId = licenciadoId,
            MarketplaceId = marketplaceId,
            TipoPessoa = tipoPessoa,
            RazaoSocial = razaoSocial,
            NomeFantasia = nomeFantasia,
            Documento = Documento.Criar(tipoPessoa, documento).Value,
            Endereco = endereco,
            Responsavel = responsavel,
            Ativo = true,
            DataCriacao = DateTime.UtcNow,
            CriadoPor = criadoPor,
            VisitadoPresencialmente = false
        };
        
        return Result<Representante>.Ok(representante);
    }
    
    public Result Atualizar(
        string razaoSocial,
        string nomeFantasia,
        Endereco endereco,
        Responsavel responsavel,
        int atualizadoPor)
    {
        // Validações...
        
        RazaoSocial = razaoSocial;
        NomeFantasia = nomeFantasia;
        Endereco = endereco;
        Responsavel = responsavel;
        DataAtualizacao = DateTime.UtcNow;
        AtualizadoPor = atualizadoPor;
        
        return Result.Ok();
    }
    
    public Result Desativar(int desativadoPor)
    {
        Ativo = false;
        DataAtualizacao = DateTime.UtcNow;
        AtualizadoPor = desativadoPor;
        
        return Result.Ok();
    }
}

// Domain/ValueObjects/Documento.cs
public class Documento : ValueObject
{
    public TipoPessoa Tipo { get; }
    public string Valor { get; }
    
    public static Result<Documento> Criar(TipoPessoa tipo, string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            return Result<Documento>.Fail("Documento não pode estar vazio");
        
        var documentoLimpo = valor.Replace(".", "").Replace("/", "").Replace("-", "");
        
        if (tipo == TipoPessoa.PessoaFisica)
        {
            if (!ValidarCPF(documentoLimpo))
                return Result<Documento>.Fail("CPF inválido");
        }
        else if (tipo == TipoPessoa.PessoaJuridica)
        {
            if (!ValidarCNPJ(documentoLimpo))
                return Result<Documento>.Fail("CNPJ inválido");
        }
        
        return Result<Documento>.Ok(new Documento(tipo, documentoLimpo));
    }
    
    private Documento(TipoPessoa tipo, string valor)
    {
        Tipo = tipo;
        Valor = valor;
    }
    
    private static bool ValidarCPF(string cpf)
    {
        // Implementar algoritmo de validação CPF
        return cpf.Length == 11;
    }
    
    private static bool ValidarCNPJ(string cnpj)
    {
        // Implementar algoritmo de validação CNPJ
        return cnpj.Length == 14;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Tipo;
        yield return Valor;
    }
}

// Domain/ValueObjects/Endereco.cs
public class Endereco : ValueObject
{
    public string Rua { get; }
    public string Numero { get; }
    public string Complemento { get; }
    public string Bairro { get; }
    public string Cidade { get; }
    public string Estado { get; }
    public string CEP { get; }
    public string Pais { get; } = "BR";
    
    private Endereco(string rua, string numero, string complemento, 
        string bairro, string cidade, string estado, string cep)
    {
        Rua = rua;
        Numero = numero;
        Complemento = complemento;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        CEP = cep;
    }
    
    public static Result<Endereco> Criar(
        string rua, string numero, string complemento,
        string bairro, string cidade, string estado, string cep)
    {
        var erros = new List<string>();
        
        if (string.IsNullOrWhiteSpace(rua))
            erros.Add("Rua é obrigatória");
        if (string.IsNullOrWhiteSpace(numero))
            erros.Add("Número é obrigatório");
        if (string.IsNullOrWhiteSpace(bairro))
            erros.Add("Bairro é obrigatório");
        if (string.IsNullOrWhiteSpace(cidade))
            erros.Add("Cidade é obrigatória");
        
        if (erros.Any())
            return Result<Endereco>.Fail(string.Join(", ", erros));
        
        return Result<Endereco>.Ok(
            new Endereco(rua, numero, complemento, bairro, cidade, estado, cep));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Rua;
        yield return Numero;
        yield return Bairro;
        yield return Cidade;
        yield return CEP;
    }
}

// Domain/ValueObjects/Responsavel.cs
public class Responsavel : ValueObject
{
    public string Nome { get; }
    public string Sobrenome { get; }
    public string CPF { get; }
    public DateTime? DataNascimento { get; }
    public string NomeMae { get; }
    public decimal RendaMensal { get; }
    public string Email { get; }
    public string Celular { get; }
    public bool PoliticamenteExposto { get; }
    
    private Responsavel(string nome, string sobrenome, string cpf,
        DateTime? dataNascimento, string nomeMae, decimal rendaMensal,
        string email, string celular, bool politicamenteExposto)
    {
        Nome = nome;
        Sobrenome = sobrenome;
        CPF = cpf;
        DataNascimento = dataNascimento;
        NomeMae = nomeMae;
        RendaMensal = rendaMensal;
        Email = email;
        Celular = celular;
        PoliticamenteExposto = politicamenteExposto;
    }
    
    public static Result<Responsavel> Criar(
        string nome, string sobrenome, string cpf,
        DateTime? dataNascimento, string nomeMae, decimal rendaMensal,
        string email, string celular, bool politicamenteExposto)
    {
        var erros = new List<string>();
        
        if (string.IsNullOrWhiteSpace(nome))
            erros.Add("Nome é obrigatório");
        if (string.IsNullOrWhiteSpace(cpf))
            erros.Add("CPF é obrigatório");
        if (string.IsNullOrWhiteSpace(email))
            erros.Add("Email é obrigatório");
        
        if (erros.Any())
            return Result<Responsavel>.Fail(string.Join(", ", erros));
        
        return Result<Responsavel>.Ok(
            new Responsavel(nome, sobrenome, cpf, dataNascimento,
                nomeMae, rendaMensal, email, celular, politicamenteExposto));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CPF;
        yield return Email;
    }
}
```

### 4.2 Enums

```csharp
// Domain/Enums/TipoPessoa.cs
public enum TipoPessoa
{
    PessoaFisica = 0,
    PessoaJuridica = 1
}

// Domain/Enums/TipoEmpresa.cs
public enum TipoEmpresa
{
    MEI = "MEI",
    PJ = "PJ",
    Cooperativa = "COOPERATIVA"
}
```

### 4.3 Interfaces de Repositório

```csharp
// Domain/Repositories/IRepresentanteRepository.cs
public interface IRepresentanteRepository
{
    Task<Representante> GetByIdAsync(int id, CancellationToken ct = default);
    
    Task<Representante> GetByDocumentoAsync(
        string documento, int licenciadoId, CancellationToken ct = default);
    
    Task<IEnumerable<Representante>> GetByLicenciadoAsync(
        int licenciadoId, CancellationToken ct = default);
    
    Task<IEnumerable<Representante>> GetByMarketplaceAsync(
        int marketplaceId, CancellationToken ct = default);
    
    Task<Representante> AddAsync(Representante representante, CancellationToken ct = default);
    
    Task<Representante> UpdateAsync(Representante representante, CancellationToken ct = default);
    
    Task DeleteAsync(int id, CancellationToken ct = default);
    
    Task<bool> ExisteDocumentoAsync(
        string documento, int licenciadoId, CancellationToken ct = default);
}
```

---

## 5. FASE 3: CAMADA DE APLICAÇÃO (2-3 semanas)

### 5.1 Commands (CQRS)

```csharp
// Application/Commands/CreateRepresentante/CreateRepresentanteCommand.cs
public record CreateRepresentanteCommand(
    int LicenciadoId,
    int? MarketplaceId,
    string TipoPessoa,
    string RazaoSocial,
    string NomeFantasia,
    string Documento,
    string Telefone,
    string EmailEmpresa,
    int MCCId,
    string TipoEmpresa,
    decimal Faturamento,
    decimal Patrimonio,
    DateTime? DataAbertura,
    
    // Endereço
    string Rua,
    string Numero,
    string Complemento,
    string Bairro,
    string Cidade,
    string Estado,
    string CEP,
    
    // Responsável
    string NomeResponsavel,
    string SobrenomeResponsavel,
    string CPFResponsavel,
    DateTime? DataNascimento,
    string NomeMae,
    decimal RendaMensal,
    string EmailResponsavel,
    string Celular,
    bool PoliticamenteExposto,
    
    // Usuário
    string NomeUsuario,
    string EmailUsuario,
    string Senha) : IRequest<Result<CreateRepresentanteResponse>>;

// Application/Commands/CreateRepresentante/CreateRepresentanteCommandHandler.cs
public class CreateRepresentanteCommandHandler
    : IRequestHandler<CreateRepresentanteCommand, Result<CreateRepresentanteResponse>>
{
    private readonly IRepresentanteRepository _repository;
    private readonly IEnviar2FAService _enviar2FA;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateRepresentanteCommandHandler> _logger;
    
    public async Task<Result<CreateRepresentanteResponse>> Handle(
        CreateRepresentanteCommand command, CancellationToken ct)
    {
        try
        {
            // 1. Validar existência de documento
            var existe = await _repository.ExisteDocumentoAsync(
                command.Documento, command.LicenciadoId, ct);
            
            if (existe)
                return Result<CreateRepresentanteResponse>.Fail(
                    $"Documento {command.Documento} já cadastrado");
            
            // 2. Criar Value Objects
            var documentoResult = Documento.Criar(
                Enum.Parse<TipoPessoa>(command.TipoPessoa),
                command.Documento);
            
            if (!documentoResult.IsSuccess)
                return Result<CreateRepresentanteResponse>.Fail(documentoResult.Error);
            
            var enderecoResult = Endereco.Criar(
                command.Rua, command.Numero, command.Complemento,
                command.Bairro, command.Cidade, command.Estado, command.CEP);
            
            if (!enderecoResult.IsSuccess)
                return Result<CreateRepresentanteResponse>.Fail(enderecoResult.Error);
            
            var responsavelResult = Responsavel.Criar(
                command.NomeResponsavel, command.SobrenomeResponsavel,
                command.CPFResponsavel, command.DataNascimento,
                command.NomeMae, command.RendaMensal,
                command.EmailResponsavel, command.Celular,
                command.PoliticamenteExposto);
            
            if (!responsavelResult.IsSuccess)
                return Result<CreateRepresentanteResponse>.Fail(responsavelResult.Error);
            
            // 3. Criar Representante (aggregate)
            var representanteResult = Representante.Criar(
                command.LicenciadoId,
                command.MarketplaceId,
                Enum.Parse<TipoPessoa>(command.TipoPessoa),
                command.RazaoSocial,
                command.NomeFantasia,
                command.Documento,
                enderecoResult.Value,
                responsavelResult.Value,
                command.LicenciadoId);
            
            if (!representanteResult.IsSuccess)
                return Result<CreateRepresentanteResponse>.Fail(representanteResult.Error);
            
            // 4. Salvar no repositório
            var representante = await _repository.AddAsync(
                representanteResult.Value, ct);
            
            // 5. Enviar 2FA
            var codigo2FA = GerarCodigo2FA();
            await _enviar2FA.EnviarAsync(
                command.EmailUsuario, codigo2FA, ct);
            
            // 6. Retornar resposta
            var response = new CreateRepresentanteResponse(
                Id: representante.Id,
                Codigo2FAEnviado: true,
                Mensagem: "Código de confirmação enviado por email");
            
            _logger.LogInformation(
                "Representante {Id} criado por {Usuario}",
                representante.Id, command.LicenciadoId);
            
            return Result<CreateRepresentanteResponse>.Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar representante");
            return Result<CreateRepresentanteResponse>.Fail("Erro ao processar requisição");
        }
    }
    
    private string GerarCodigo2FA()
    {
        return Random.Shared.Next(100000, 999999).ToString();
    }
}

// Application/Commands/CreateRepresentante/CreateRepresentanteValidator.cs
public class CreateRepresentanteValidator : AbstractValidator<CreateRepresentanteCommand>
{
    public CreateRepresentanteValidator()
    {
        RuleFor(x => x.RazaoSocial)
            .NotEmpty().WithMessage("Razão Social é obrigatória")
            .MaximumLength(255).WithMessage("Razão Social não pode exceder 255 caracteres");
        
        RuleFor(x => x.Documento)
            .NotEmpty().WithMessage("Documento é obrigatório")
            .Must(x => ValidarDocumento(x))
            .WithMessage("Documento inválido");
        
        RuleFor(x => x.EmailResponsavel)
            .NotEmpty().WithMessage("Email é obrigatório")
            .EmailAddress().WithMessage("Email inválido");
        
        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Senha é obrigatória")
            .Must(x => ValidarSenha(x))
            .WithMessage("Senha não atende aos requisitos de segurança");
        
        RuleFor(x => x.TipoPessoa)
            .NotEmpty().WithMessage("Tipo de Pessoa é obrigatório")
            .Must(x => x == "PF" || x == "PJ")
            .WithMessage("Tipo de Pessoa deve ser PF ou PJ");
    }
    
    private static bool ValidarDocumento(string documento)
    {
        // Implementar validação CPF/CNPJ
        return !string.IsNullOrEmpty(documento);
    }
    
    private static bool ValidarSenha(string senha)
    {
        if (string.IsNullOrEmpty(senha) || senha.Length < 6 || senha.Length > 32)
            return false;
        
        var temNumero = senha.Any(char.IsDigit);
        var temMaiuscula = senha.Any(char.IsUpper);
        var temMinuscula = senha.Any(char.IsLower);
        var temEspecial = senha.Any(x => !char.IsLetterOrDigit(x));
        var temEspaco = senha.Contains(' ');
        
        return temNumero && temMaiuscula && temMinuscula && temEspecial && !temEspaco;
    }
}
```

### 5.2 Queries

```csharp
// Application/Queries/GetRepresentante/GetRepresentanteQuery.cs
public record GetRepresentanteQuery(int Id) : IRequest<Result<GetRepresentanteResponse>>;

// Application/Queries/GetRepresentante/GetRepresentanteQueryHandler.cs
public class GetRepresentanteQueryHandler
    : IRequestHandler<GetRepresentanteQuery, Result<GetRepresentanteResponse>>
{
    private readonly IRepresentanteRepository _repository;
    private readonly IMapper _mapper;
    
    public async Task<Result<GetRepresentanteResponse>> Handle(
        GetRepresentanteQuery query, CancellationToken ct)
    {
        var representante = await _repository.GetByIdAsync(query.Id, ct);
        
        if (representante == null)
            return Result<GetRepresentanteResponse>.Fail("Representante não encontrado");
        
        var response = _mapper.Map<GetRepresentanteResponse>(representante);
        
        return Result<GetRepresentanteResponse>.Ok(response);
    }
}
```

### 5.3 Handlers de Integrações

```csharp
// Application/Services/IntegracaoBigDatacorpHandler.cs
public interface IIntegracaoBigDatacorpService
{
    Task<BigDatacorpRepresentanteResponse> ConsultarRepresentanteLegalAsync(
        string cpf, CancellationToken ct = default);
}

public class IntegracaoBigDatacorpService : IIntegracaoBigDatacorpService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<IntegracaoBigDatacorpService> _logger;
    private readonly string _token;
    private readonly string _tokenId;
    
    public async Task<BigDatacorpRepresentanteResponse> ConsultarRepresentanteLegalAsync(
        string cpf, CancellationToken ct = default)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/ondemand")
            {
                Content = JsonContent.Create(new { cpf, tipoConsulta = "representante_legal" })
            };
            
            request.Headers.Add("AccessToken", _token);
            request.Headers.Add("TokenId", _tokenId);
            
            var response = await _httpClient.SendAsync(request, ct);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Erro ao consultar BigDatacorp: {StatusCode}",
                    response.StatusCode);
                return null;
            }
            
            var content = await response.Content.ReadAsStringAsync(ct);
            var resultado = JsonSerializer.Deserialize<BigDatacorpRepresentanteResponse>(content);
            
            return resultado;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao integrar com BigDatacorp");
            throw;
        }
    }
}

// Application/Services/IntegracaoCappataService.cs
public interface IIntegracaoCappataService
{
    Task<CapptaRevendedorResponse> ConsultarRevendedorAsync(
        string documento, CancellationToken ct = default);
    
    Task<CapptaRevendedorResponse> CadastrarRevendedorAsync(
        CadastrarRevendedorRequest request, CancellationToken ct = default);
}

public class IntegracaoCappataService : IIntegracaoCappataService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<IntegracaoCappataService> _logger;
    private readonly string _token;
    
    public async Task<CapptaRevendedorResponse> ConsultarRevendedorAsync(
        string documento, CancellationToken ct = default)
    {
        try
        {
            var url = $"/onboarding/reseller/{documento}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", $"Bearer {_token}");
            
            var response = await _httpClient.SendAsync(request, ct);
            
            if (!response.IsSuccessStatusCode)
                return null;
            
            var content = await response.Content.ReadAsStringAsync(ct);
            var resultado = JsonSerializer.Deserialize<CapptaRevendedorResponse>(content);
            
            return resultado;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao integrar com Cappta");
            throw;
        }
    }
    
    // Implementar outros métodos...
}

// Application/Services/IntegracaoViaCEPService.cs
public interface IIntegracaoViaCEPService
{
    Task<ViaCEPResponse> BuscarEnderecoByCEPAsync(
        string cep, CancellationToken ct = default);
}

public class IntegracaoViaCEPService : IIntegracaoViaCEPService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<IntegracaoViaCEPService> _logger;
    
    public async Task<ViaCEPResponse> BuscarEnderecoByCEPAsync(
        string cep, CancellationToken ct = default)
    {
        try
        {
            var url = $"https://viacep.com.br/ws/{cep}/json";
            var response = await _httpClient.GetAsync(url, ct);
            
            if (!response.IsSuccessStatusCode)
                return null;
            
            var content = await response.Content.ReadAsStringAsync(ct);
            var resultado = JsonSerializer.Deserialize<ViaCEPResponse>(content);
            
            return resultado;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar CEP em ViaCEP");
            throw;
        }
    }
}
```

---

## 6. FASE 4: INFRAESTRUTURA (3-4 semanas)

### 6.1 Entity Framework Configuration

```csharp
// Infrastructure/Persistence/LegacybankDbContext.cs
public class LegacybankDbContext : DbContext
{
    public DbSet<Representante> Representantes { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Marketplace> Marketplaces { get; set; }
    
    public LegacybankDbContext(DbContextOptions<LegacybankDbContext> options)
        : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Aplicar todas as configurações
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(LegacybankDbContext).Assembly);
    }
}

// Infrastructure/Persistence/Configurations/RepresentanteConfiguration.cs
public class RepresentanteConfiguration : IEntityTypeConfiguration<Representante>
{
    public void Configure(EntityTypeBuilder<Representante> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.RazaoSocial).IsRequired().HasMaxLength(255);
        builder.Property(x => x.NomeFantasia).IsRequired().HasMaxLength(255);
        
        // Value Objects
        builder.OwnsOne(x => x.Documento, e =>
        {
            e.Property(p => p.Tipo).HasColumnName("TipoPessoa");
            e.Property(p => p.Valor).HasColumnName("Documento");
        });
        
        builder.OwnsOne(x => x.Endereco, e =>
        {
            e.Property(p => p.Rua).HasColumnName("Rua").HasMaxLength(255);
            e.Property(p => p.Numero).HasColumnName("Numero").HasMaxLength(10);
            e.Property(p => p.Bairro).HasColumnName("Bairro").HasMaxLength(255);
            e.Property(p => p.Cidade).HasColumnName("Cidade").HasMaxLength(255);
        });
        
        builder.OwnsOne(x => x.Responsavel, e =>
        {
            e.Property(p => p.Nome).HasColumnName("NomeResponsavel").HasMaxLength(255);
            e.Property(p => p.Email).HasColumnName("EmailResponsavel").HasMaxLength(255);
        });
        
        // Índices
        builder.HasIndex(x => new { x.LicenciadoId, x.Documento }).IsUnique();
        builder.HasIndex(x => x.MarketplaceId);
        builder.HasIndex(x => x.Ativo);
        
        // Relacionamentos
        builder.HasOne<Marketplace>()
            .WithMany()
            .HasForeignKey(x => x.MarketplaceId)
            .IsRequired(false);
    }
}
```

### 6.2 Repository Implementation

```csharp
// Infrastructure/Repositories/RepresentanteRepository.cs
public class RepresentanteRepository : IRepresentanteRepository
{
    private readonly LegacybankDbContext _context;
    private readonly ILogger<RepresentanteRepository> _logger;
    
    public async Task<Representante> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Representantes
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }
    
    public async Task<bool> ExisteDocumentoAsync(
        string documento, int licenciadoId, CancellationToken ct = default)
    {
        return await _context.Representantes
            .AnyAsync(x => 
                x.LicenciadoId == licenciadoId &&
                x.Documento.Valor == documento &&
                x.Ativo,
            ct);
    }
    
    public async Task<Representante> AddAsync(
        Representante representante, CancellationToken ct = default)
    {
        await _context.Representantes.AddAsync(representante, ct);
        await _context.SaveChangesAsync(ct);
        
        _logger.LogInformation("Representante {Id} adicionado", representante.Id);
        
        return representante;
    }
    
    // Implementar outros métodos...
}
```

---

## 7. FASE 5: API E CONTROLLERS (2-3 semanas)

### 7.1 Controllers

```csharp
// API/Controllers/RepresentantesController.cs
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RepresentantesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<RepresentantesController> _logger;
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateRepresentanteResponse>> Criar(
        CreateRepresentanteCommand command,
        CancellationToken ct)
    {
        var resultado = await _mediator.Send(command, ct);
        
        if (!resultado.IsSuccess)
            return BadRequest(resultado.Error);
        
        return CreatedAtAction(
            nameof(Obter),
            new { id = resultado.Value.Id },
            resultado.Value);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetRepresentanteResponse>> Obter(
        int id,
        CancellationToken ct)
    {
        var query = new GetRepresentanteQuery(id);
        var resultado = await _mediator.Send(query, ct);
        
        if (!resultado.IsSuccess)
            return NotFound(resultado.Error);
        
        return Ok(resultado.Value);
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateRepresentanteResponse>> Atualizar(
        int id,
        UpdateRepresentanteCommand command,
        CancellationToken ct)
    {
        // Validar que ID da rota corresponde ao comando
        if (id != command.Id)
            return BadRequest("ID não corresponde");
        
        var resultado = await _mediator.Send(command, ct);
        
        if (!resultado.IsSuccess)
            return BadRequest(resultado.Error);
        
        return Ok(resultado.Value);
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deletar(int id, CancellationToken ct)
    {
        var command = new DeleteRepresentanteCommand(id);
        var resultado = await _mediator.Send(command, ct);
        
        if (!resultado.IsSuccess)
            return NotFound(resultado.Error);
        
        return NoContent();
    }
}
```

### 7.2 Startup Configuration

```csharp
// API/Program.cs
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();

// API/Extensions/ServiceCollectionExtensions.cs
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateRepresentanteCommand).Assembly));
        
        services.AddValidatorsFromAssembly(
            typeof(CreateRepresentanteValidator).Assembly);
        
        services.AddAutoMapper(typeof(RepresentanteProfile).Assembly);
        
        return services;
    }
    
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LegacybankDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IRepresentanteRepository, RepresentanteRepository>();
        
        services.AddHttpClient<IIntegracaoViaCEPService, IntegracaoViaCEPService>();
        services.AddHttpClient<IIntegracaoBigDatacorpService, IntegracaoBigDatacorpService>();
        services.AddHttpClient<IIntegracaoCappataService, IntegracaoCappataService>();
        
        services.AddSingleton<ICache, RedisCache>();
        services.AddSingleton<IEnviar2FAService, Enviar2FAService>();
        
        return services;
    }
    
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "LGC Representantes API",
                Version = "v1"
            });
        });
        
        return services;
    }
}
```

---

## 8. FASE 6: TESTES (3-4 semanas)

### 8.1 Testes Unitários

```csharp
// Tests/Application/CreateRepresentanteCommandHandlerTests.cs
public class CreateRepresentanteCommandHandlerTests
{
    private readonly Mock<IRepresentanteRepository> _mockRepository;
    private readonly Mock<IEnviar2FAService> _mockEnviar2FA;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreateRepresentanteCommandHandler _handler;
    
    public CreateRepresentanteCommandHandlerTests()
    {
        _mockRepository = new Mock<IRepresentanteRepository>();
        _mockEnviar2FA = new Mock<IEnviar2FAService>();
        _mockMapper = new Mock<IMapper>();
        
        _handler = new CreateRepresentanteCommandHandler(
            _mockRepository.Object,
            _mockEnviar2FA.Object,
            _mockMapper.Object,
            Mock.Of<ILogger<CreateRepresentanteCommandHandler>>());
    }
    
    [Fact]
    public async Task Handle_ComDocumentoExistente_RetornaErro()
    {
        // Arrange
        var command = new CreateRepresentanteCommand(
            LicenciadoId: 1,
            MarketplaceId: null,
            TipoPessoa: "PJ",
            RazaoSocial: "Empresa LTDA",
            // ... outros parâmetros
            Documento: "12345678901234");
        
        _mockRepository
            .Setup(x => x.ExisteDocumentoAsync(
                command.Documento, command.LicenciadoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("já cadastrado", result.Error);
        _mockRepository.Verify(
            x => x.AddAsync(It.IsAny<Representante>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
    
    [Fact]
    public async Task Handle_ComDadosValidos_CriaRepresentante()
    {
        // Arrange
        var command = new CreateRepresentanteCommand(
            LicenciadoId: 1,
            MarketplaceId: null,
            TipoPessoa: "PJ",
            RazaoSocial: "Empresa LTDA",
            // ... outros parâmetros
            Documento: "12345678901234");
        
        var representante = new Representante();
        
        _mockRepository
            .Setup(x => x.ExisteDocumentoAsync(
                command.Documento, command.LicenciadoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        _mockRepository
            .Setup(x => x.AddAsync(It.IsAny<Representante>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(representante);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsSuccess);
        _mockRepository.Verify(
            x => x.AddAsync(It.IsAny<Representante>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
```

### 8.2 Testes de Integração

```csharp
// Tests/Integration/RepresentantesControllerTests.cs
public class RepresentantesControllerTests : IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory;
    private HttpClient _client;
    private LegacybankDbContext _dbContext;
    
    public async Task InitializeAsync()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remover DbContext original
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<LegacybankDbContext>));
                    
                    if (descriptor != null)
                        services.Remove(descriptor);
                    
                    // Adicionar DbContext com banco em memória
                    services.AddDbContext<LegacybankDbContext>(options =>
                        options.UseInMemoryDatabase("TestDb"));
                });
            });
        
        _client = _factory.CreateClient();
        _dbContext = _factory.Services.GetRequiredService<LegacybankDbContext>();
        
        await _dbContext.Database.EnsureCreatedAsync();
    }
    
    [Fact]
    public async Task Post_ComDadosValidos_Retorna201()
    {
        // Arrange
        var command = new CreateRepresentanteCommand(
            LicenciadoId: 1,
            // ... outros parâmetros
        );
        
        var content = JsonContent.Create(command);
        
        // Act
        var response = await _client.PostAsync("/api/representantes", content);
        
        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
    
    public async Task DisposeAsync()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        _client.Dispose();
        _factory.Dispose();
    }
}
```

---

## 9. FASE 7: MIGRAÇÃO DE DADOS (2-3 semanas)

### 9.1 Script de Migração

```sql
-- Scripts/MigrationScript.sql

-- 1. Criar tabelas novas (EF Core gera automaticamente)
-- 2. Copiar dados antigos para novas (com transformação)

INSERT INTO dbo.Representantes (
    LicenciadoId,
    MarketplaceId,
    TipoPessoa,
    RazaoSocial,
    NomeFantasia,
    Documento_Tipo,
    Documento_Valor,
    -- ... outros campos
    Ativo,
    DataCriacao
)
SELECT
    pf.COD_ID_PESSOA_LICENCIADO,
    pf.COD_ID_MARKETPLACE,
    CASE pf.FLG_TIPO_PESSOA WHEN 'PF' THEN 0 ELSE 1 END,
    pf.NOM_RAZAOSOCIAL,
    pf.NOM_FANTASIA,
    CASE pf.FLG_TIPO_PESSOA WHEN 'PF' THEN 0 ELSE 1 END,
    REPLACE(REPLACE(REPLACE(pf.NOM_CNPJ, '.', ''), '/', ''), '-', ''),
    -- ... outros campos
    CASE pf.FLG_ATIVO WHEN 'S' THEN 1 ELSE 0 END,
    pf.DTA_DATA
FROM PESSOAS_FJ pf
WHERE pf.FLG_TIPO = 'R'
  AND pf.FLG_ATIVO = 'S'

-- 3. Validar contagem
SELECT COUNT(*) FROM dbo.Representantes

-- 4. Atualizar sequência de identidade
DBCC CHECKIDENT ('dbo.Representantes', RESEED, (SELECT MAX(Id) FROM dbo.Representantes))
```

### 9.2 Plano de Rollback

```
1. Backup completo do banco antigo ANTES de qualquer mudança
2. Manter schema antigo por 30 dias
3. Criar view de compatibilidade apontando para novas tabelas
4. Testar todos os reports/dashboards antes do cutover
5. Preparar script de reverse migration em caso de falha
```

---

## 10. FASE 8: FRONTEND ANGULAR (4-6 semanas)

### 10.1 Estrutura do Projeto

```
src/app/
├── representantes/
│   ├── models/
│   │   ├── representante.model.ts
│   │   ├── criar-representante.dto.ts
│   │   └── atualizar-representante.dto.ts
│   ├── services/
│   │   ├── representantes.service.ts
│   │   ├── viacep-integration.service.ts
│   │   └── dois-fa.service.ts
│   ├── components/
│   │   ├── lista-representantes/
│   │   ├── form-representante/
│   │   ├── detalhe-representante/
│   │   └── modal-confirmacao-2fa/
│   ├── pages/
│   │   ├── representantes-page/
│   │   └── detalhe-representante-page/
│   ├── state/
│   │   └── representantes.store.ts (NgRx)
│   └── representantes.module.ts
```

### 10.2 Services

```typescript
// representantes.service.ts
@Injectable({ providedIn: 'root' })
export class RepresentantesService {
  private apiUrl = '/api/representantes';
  
  constructor(private http: HttpClient) {}
  
  criar(comando: CriarRepresentanteCommand): Observable<CriarRepresentanteResponse> {
    return this.http.post<CriarRepresentanteResponse>(this.apiUrl, comando);
  }
  
  obter(id: number): Observable<GetRepresentanteResponse> {
    return this.http.get<GetRepresentanteResponse>(`${this.apiUrl}/${id}`);
  }
  
  atualizar(id: number, comando: AtualizarRepresentanteCommand): 
    Observable<AtualizarRepresentanteResponse> {
    return this.http.put<AtualizarRepresentanteResponse>(
      `${this.apiUrl}/${id}`, comando);
  }
  
  deletar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
  
  listar(): Observable<GetRepresentanteResponse[]> {
    return this.http.get<GetRepresentanteResponse[]>(this.apiUrl);
  }
}

// viacep-integration.service.ts
@Injectable({ providedIn: 'root' })
export class ViaCEPIntegrationService {
  private apiUrl = 'https://viacep.com.br/ws';
  
  constructor(private http: HttpClient) {}
  
  buscarEnderecoByCEP(cep: string): Observable<ViaCEPResponse> {
    const cepLimpo = cep.replace(/\D/g, '');
    return this.http.get<ViaCEPResponse>(
      `${this.apiUrl}/${cepLimpo}/json`).pipe(
        retry(1),
        catchError(error => {
          console.error('Erro ao buscar CEP', error);
          return throwError(() => new Error('CEP não encontrado'));
        })
      );
  }
}
```

### 10.3 Components

```typescript
// form-representante.component.ts
@Component({
  selector: 'app-form-representante',
  templateUrl: './form-representante.component.html',
  styleUrls: ['./form-representante.component.scss']
})
export class FormRepresentanteComponent implements OnInit {
  form: FormGroup;
  isLoading = false;
  showModal2FA = false;
  
  constructor(
    private fb: FormBuilder,
    private service: RepresentantesService,
    private viacepService: ViaCEPIntegrationService,
    private snackBar: MatSnackBar
  ) {
    this.form = this.createForm();
  }
  
  ngOnInit(): void {
    this.setupForm();
  }
  
  private createForm(): FormGroup {
    return this.fb.group({
      tipoPessoa: ['PJ', Validators.required],
      razaoSocial: ['', [Validators.required, Validators.maxLength(255)]],
      nomeFantasia: ['', [Validators.required, Validators.maxLength(255)]],
      documento: ['', [Validators.required, this.validarDocumento()]],
      // ... outros campos
      cep: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      senha: ['', [Validators.required, this.validarSenha()]],
    });
  }
  
  private setupForm(): void {
    // Atualizar labels quando tipo de pessoa muda
    this.form.get('tipoPessoa').valueChanges.subscribe(tipo => {
      this.atualizarLabels(tipo);
      this.atualizarMascaras(tipo);
    });
    
    // Buscar endereço quando CEP perde foco
    this.form.get('cep').statusChanges
      .pipe(
        filter(status => status === 'VALID'),
        debounceTime(300),
        distinctUntilChanged(),
        switchMap(() => this.viacepService.buscarEnderecoByCEP(
          this.form.get('cep').value)),
        catchError(error => {
          this.snackBar.open('CEP não encontrado', 'OK', { duration: 3000 });
          return of(null);
        })
      )
      .subscribe(endereco => {
        if (endereco) {
          this.form.patchValue({
            rua: endereco.logradouro,
            bairro: endereco.bairro,
            cidade: endereco.localidade,
            estado: endereco.uf
          });
        }
      });
  }
  
  onSubmit(): void {
    if (this.form.invalid) {
      this.snackBar.open('Formulário inválido', 'OK', { duration: 3000 });
      return;
    }
    
    this.isLoading = true;
    
    const comando = this.mapFormToCommand();
    
    this.service.criar(comando)
      .pipe(finalize(() => this.isLoading = false))
      .subscribe({
        next: (response) => {
          this.showModal2FA = true;
          // Modal pede código 2FA
        },
        error: (error) => {
          this.snackBar.open('Erro ao criar representante', 'OK');
        }
      });
  }
  
  private atualizarLabels(tipo: string): void {
    // Atualizar labels dinamicamente
  }
  
  private atualizarMascaras(tipo: string): void {
    const documentoControl = this.form.get('documento');
    if (tipo === 'PF') {
      // Aplicar máscara CPF
    } else {
      // Aplicar máscara CNPJ
    }
  }
  
  private validarDocumento(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) return null;
      
      const tipoPessoa = this.form?.get('tipoPessoa')?.value;
      const documento = control.value.replace(/\D/g, '');
      
      if (tipoPessoa === 'PF' && documento.length !== 11) {
        return { cpfInvalido: true };
      }
      
      if (tipoPessoa === 'PJ' && documento.length !== 14) {
        return { cnpjInvalido: true };
      }
      
      return null;
    };
  }
  
  private validarSenha(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) return null;
      
      const senha = control.value;
      const erros: any = {};
      
      if (senha.length < 6 || senha.length > 32) {
        erros.tamanho = true;
      }
      
      if (!/\d/.test(senha)) {
        erros.numero = true;
      }
      
      if (!/[A-Z]/.test(senha)) {
        erros.maiuscula = true;
      }
      
      if (!/[a-z]/.test(senha)) {
        erros.minuscula = true;
      }
      
      if (!/[!@#$%^&*]/.test(senha)) {
        erros.especial = true;
      }
      
      if (/ /.test(senha)) {
        erros.espaco = true;
      }
      
      return Object.keys(erros).length > 0 ? erros : null;
    };
  }
  
  private mapFormToCommand(): CriarRepresentanteCommand {
    return this.form.value;
  }
}
```

---

## 11. CRONOGRAMA ESTIMADO

```
FASE 1: Preparação               1-2 semanas
FASE 2: Modelagem do Domínio     2-3 semanas
FASE 3: Camada de Aplicação      2-3 semanas
FASE 4: Infraestrutura           3-4 semanas
FASE 5: API e Controllers        2-3 semanas
FASE 6: Testes                   3-4 semanas
FASE 7: Migração de Dados        2-3 semanas
FASE 8: Frontend Angular         4-6 semanas
FASE 9: Integração e Deploy      2-3 semanas
FASE 10: Validação e Go-Live     1-2 semanas

TOTAL:                           22-34 semanas (5-8 meses)
```

---

## 12. RISKS E MITIGAÇÕES

| Risk | Impacto | Probabilidade | Mitigação |
|------|---------|---------------|-----------|
| Perda de dados na migração | Alto | Média | Backup completo, validação de contagem |
| Performance da API | Alto | Média | Testes de carga, índices otimizados |
| Integração com APIs externas falha | Alto | Média | Fallback, retry policy, logging |
| Equipe sem experiência com stack | Médio | Média | Treinamento, pair programming |
| Requisitos não documentados | Médio | Média | Audit completo, reuniões com stakeholders |
| Delay em aprovações | Médio | Alta | Comunicação proativa, roadmap claro |

---

## 13. CHECKLIST DE GO-LIVE

- [ ] Todos os testes passando (unitários, integração, carga)
- [ ] Documentação completa (API docs, runbooks, troubleshooting)
- [ ] Backup do banco antigo realizado e testado
- [ ] Plano de rollback validado
- [ ] Treinamento da equipe completo
- [ ] Monitoramento e alertas configurados
- [ ] Load balancing testado
- [ ] Segurança validada (OWASP Top 10)
- [ ] Performance validada (SLA definido)
- [ ] Compliance e auditoria validados
- [ ] Comunicação aos usuários realizada
- [ ] Suporte 24/7 disponível no go-live

---

**Documento Final do Guia de Reescrita**  
**Versão:** 1.0  
**Data:** 8 de Maio de 2026  
**Status:** Completo e Pronto para Execução
