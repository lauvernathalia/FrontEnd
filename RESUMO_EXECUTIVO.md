# RESUMO EXECUTIVO - FLUXO DE CADASTRO DE REPRESENTANTES

## 1. VISÃO GERAL DO PROJETO

### O que é?
Sistema completo de cadastro de representantes (agentes de vendas/revenda) com múltiplas integrações externas em uma arquitetura ASP.NET WebForms legada.

### Quem usa?
- **Licenciados**: Proprietários do sistema
- **Marketplaces**: Revendedores/agregadores
- **Representantes**: Agentes de vendas vinculados a marketplaces
- **Estabelecimentos**: Lojas/comerciantes finais

### Hierarquia
```
Licenciado
  ├─ Marketplace
  │   └─ Representante
  │       └─ Estabelecimento
  └─ Representante
      └─ Estabelecimento
```

---

## 2. FLUXO PRINCIPAL (Passo a Passo)

### Novo Cadastro
```
1. Usuario acessa: cad_representantes.aspx?id=0
   ↓
2. Preenche formulário com dados (PF ou PJ)
   ↓
3. Digita CEP → ViaCEP busca endereço automaticamente
   ↓
4. Clica "SALVAR"
   ↓
5. Sistema valida (email, senha, duplicidade)
   ↓
6. Envia código 2FA por email
   ↓
7. Usuario confirma código
   ↓
8. Sistema salva tudo no BD
   ↓
9. Usuario pode fazer login
```

### Edição
```
1. Usuario clica em representante existente
   ↓
2. Formulário carrega com dados (usuário não pode ser editado)
   ↓
3. Altera campos necessários
   ↓
4. Clica "SALVAR"
   ↓
5. 2FA de confirmação
   ↓
6. Dados atualizados
```

---

## 3. TECNOLOGIA ATUAL

### Backend
- **Linguagem**: C# (ASP.NET WebForms)
- **Padrão**: WebForms (monolítico)
- **Banco**: SQL Server
- **Comunicação**: ADO.NET + Stored Procedures

### Frontend
- **Tecnologia**: ASP.NET ASPX + HTML/CSS/JavaScript
- **Funcionalidades**: Formulários, validações básicas, modais
- **Máscaras**: jQuery InputMask

### Integrações Externas
1. **ViaCEP**: Busca endereço por CEP
2. **BigDataCorp**: Consulta dados de representante legal (PEP)
3. **HubCappta**: Sincroniza revendedor/marketplace/lojista
4. **Email**: Envia código 2FA

---

## 4. TABELAS PRINCIPAIS

### PESSOAS_FJ (Principal)
- ID, Tipo (R/E/M), Licenciado, Marketplace
- Dados: Razão Social, CNPJ/CPF, Telefone, Email
- Endereço: Rua, Número, Bairro, Cidade, UF, CEP
- Responsável: Nome, CPF, Email, Celular, etc
- Usuário: Login, Senha (encrypted)

### PESSOAS_FJ_CAPPTA
- Integração com HubCappta
- Token, Status, JSON completo da resposta

### PESSOAS_FJ_CONTAS_INS
- Dados bancários do representante
- Banco, Agência, Conta

### CONSULTAS_FICHA_CADASTRO
- Auditoria de consultas BigDataCorp
- Registra: CPF, Nome, Data, Origem

---

## 5. INTEGRAÇÕES DETALHADAS

### 5.1 ViaCEP
**Quando**: Usuario preenche CEP  
**O que faz**: Busca endereço  
**URL**: `https://viacep.com.br/ws/{CEP}/json`  
**Retorna**: Rua, Bairro, Cidade, UF

### 5.2 BigDataCorp
**Quando**: Ao salvar representante (PEP - Politicamente Exposto)  
**O que faz**: Consulta status do CPF/CNPJ  
**URL**: `https://plataforma.bigdatacorp.com.br/ondemand`  
**Retorna**: Situação, Status PEP, Renda, Patrimônio

### 5.3 HubCappta (CapPTA)
**Quando**: Ao processar transação com marketplace  
**O que faz**: Sincroniza revendedor, lojista, dados bancários  
**URL**: `https://api.posportal.com.br/api/hub`  
**Retorna**: Dados completos de revendedor/lojista

---

## 6. VALIDAÇÕES IMPLEMENTADAS

### Obrigatórias
- Tipo de Pessoa (PF/PJ)
- Razão Social/Nome
- Documento (CNPJ/CPF - deve ser único)
- Email (formato válido)
- Telefone
- Endereço completo (Rua, Número, Bairro, Cidade, UF, CEP)
- Dados do Responsável (Nome, CPF, Email)

### Especiais
**Senha**: 6-32 caracteres, 1 número, 1 maiúscula, 1 minúscula, 1 especial, sem espaço  
**Email**: Validação de formato  
**Duplicidade**: Mesmo CNPJ/CPF não pode existir para mesmo Licenciado

---

## 7. FLUXO 2FA (Two-Factor Authentication)

```
1. Usuario clica "SALVAR"
   ↓
2. Sistema valida dados
   ↓
3. Se OK: Gera código 6 dígitos
   ↓
4. Envia email para NOM_EMAIL_RESPONSAVEL
   ↓
5. Exibe modal para usuario digitar código
   ↓
6. Usuario digita código
   ↓
7. Sistema valida contra sessão
   ↓
8. Se válido: Salva representante + cria usuario
   ↓
9. Se inválido: Pede novo código
```

---

## 8. PROBLEMAS DO SISTEMA ATUAL

### Arquitetura
- ❌ Monolítico WebForms (acoplado)
- ❌ Sem separação de responsabilidades
- ❌ Difícil de testar
- ❌ Difícil de escalar

### Performance
- ⚠️ Múltiplas conexões ao BD
- ⚠️ Sem cache
- ⚠️ Integrações síncronas (usuário espera)
- ⚠️ Sem otimização de índices

### Manutenibilidade
- ⚠️ Código legado misturado
- ⚠️ Sem documentação clara
- ⚠️ Difícil adicionar novos campos/integrações
- ⚠️ Sem testes automatizados

### Segurança
- ⚠️ 2FA básico (sem rate limiting)
- ⚠️ Sem CSRF explícito
- ⚠️ Auditoria mínima
- ⚠️ Sem validação OWASP

---

## 9. OPORTUNIDADES DE MELHORIA

### Curto Prazo (1-3 meses)
- [ ] Adicionar testes unitários
- [ ] Melhorar logging/monitoramento
- [ ] Implementar rate limiting em 2FA
- [ ] Adicionar cache para MCCs/Marketplaces
- [ ] Documentar todas as integracoes

### Médio Prazo (3-6 meses)
- [ ] Migrar para ASP.NET Core (API REST)
- [ ] Implementar CQRS
- [ ] Integracoes assincronas (Job Queue)
- [ ] Melhorar performance do BD

### Longo Prazo (6-12 meses)
- [ ] Migrar frontend para Angular (já existe no projeto!)
- [ ] Implementar microserviços
- [ ] CI/CD completo
- [ ] Containerização (Docker/K8s)

---

## 10. ESTIMATIVA DE ESFORÇO - REESCRITA

| Fase | Duração | Esforço |
|------|---------|--------|
| Preparação | 1-2 sem | 40h |
| Modelagem de Domínio | 2-3 sem | 80h |
| Camada de Aplicação | 2-3 sem | 80h |
| Infraestrutura/BD | 3-4 sem | 120h |
| API/Controllers | 2-3 sem | 80h |
| Testes | 3-4 sem | 120h |
| Migração de Dados | 2-3 sem | 80h |
| Frontend Angular | 4-6 sem | 160h |
| Integração/Deploy | 2-3 sem | 80h |
| Validação/Go-Live | 1-2 sem | 40h |
| **TOTAL** | **5-8 meses** | **880-920 horas** |

**Equivalente**: 4-5 desenvolvedores por 6 meses

---

## 11. TECNOLOGIA ALVO (Stack Moderno)

```
FRONTEND:
├─ Angular 17+ ✓ (já existe no projeto!)
├─ TypeScript
├─ Material Design
├─ RxJS

BACKEND:
├─ ASP.NET Core 8
├─ C# 12
├─ Entity Framework Core
├─ MediatR (CQRS)
├─ FluentValidation

BANCO:
├─ SQL Server 2019+
├─ Redis (cache)

DEVOPS:
├─ Docker
├─ Kubernetes
├─ GitHub Actions
└─ Azure (ou similar)
```

---

## 12. BENEFÍCIOS DA REESCRITA

### Performance
- ⚡ 3-5x mais rápido (assincronismo)
- ⚡ Cache reduz queries
- ⚡ Integracoes em background

### Qualidade
- ✅ Testes automatizados (80%+ cobertura)
- ✅ Código limpo e testável
- ✅ Menos bugs em produção

### Manutenibilidade
- 📚 Documentação clara (OpenAPI/Swagger)
- 📚 Fácil adicionar features
- 📚 Fácil onboarding de novos devs

### Escalabilidade
- 📈 Arquitetura microserviços ready
- 📈 Suporta 10x mais usuarios
- 📈 Deploy independente

### Segurança
- 🔒 OWASP Top 10 implementado
- 🔒 Rate limiting
- 🔒 Audit trail completo
- 🔒 Secrets management

---

## 13. DOCUMENTOS RELACIONADOS

Todos os 3 documentos criados estão no repositório:

1. **[FLUXO_CADASTRO_REPRESENTANTES.md](FLUXO_CADASTRO_REPRESENTANTES.md)**
   - Fluxo completo detalhado
   - Estrutura de BD
   - Stored Procedures
   - Validações

2. **[DIAGRAMAS_TECNICO_APIS.md](DIAGRAMAS_TECNICO_APIS.md)**
   - Diagramas de fluxo
   - Detalhes de APIs externas
   - Arquitetura em camadas
   - Ciclo de vida de representante

3. **[GUIA_REESCRITA.md](GUIA_REESCRITA.md)**
   - Arquitetura alvo
   - 10 fases de implementação
   - Código de exemplo (C#/TypeScript)
   - Plano de migração
   - Testes de exemplo

---

## 14. PRÓXIMOS PASSOS

### Immediate (Esta semana)
- [ ] Reunião com stakeholders
- [ ] Confirmar escopo da reescrita
- [ ] Designar tech lead e arquiteto
- [ ] Criar roadmap detalhado

### Phase 1 (Próx 2 semanas)
- [ ] Setup inicial do projeto
- [ ] Criar estrutura de repos (GitHub)
- [ ] Setup de infraestrutura (Azure/AWS)
- [ ] Criar POC (Proof of Concept)

### Phase 2 (Próx 1 mês)
- [ ] Modelagem de domínio
- [ ] Design da API
- [ ] Começar implementação backend
- [ ] Setup de testes

---

## 15. MÉTRICAS DE SUCESSO

### Técnicas
- [ ] 80%+ cobertura de testes
- [ ] Tempo de resposta API < 200ms (p95)
- [ ] Zero erros críticos em produção
- [ ] 99.9% uptime

### Funcionais
- [ ] Todos os workflows da legacy replicados
- [ ] Integrações funcionando 100%
- [ ] Zero perda de dados na migração
- [ ] Usuários migrando sem problemas

### Negócio
- [ ] Redução de 50% em bugs
- [ ] Redução de 30% em tempo de deployment
- [ ] Aumento de 2x na velocidade de novos features
- [ ] ROI positivo em 12 meses

---

## 16. CONTATO E DÚVIDAS

Para mais informações, consulte os documentos detalhados:
- **Fluxo**: [FLUXO_CADASTRO_REPRESENTANTES.md](FLUXO_CADASTRO_REPRESENTANTES.md)
- **Técnico**: [DIAGRAMAS_TECNICO_APIS.md](DIAGRAMAS_TECNICO_APIS.md)  
- **Reescrita**: [GUIA_REESCRITA.md](GUIA_REESCRITA.md)

---

## Resumo em Uma Frase

**Sistema legado WebForms de cadastro de representantes com 3 integrações externas, pronto para ser reescrito em ASP.NET Core + Angular com arquitetura moderna, estimado em 5-8 meses, com ganho significativo em performance, manutenibilidade e escalabilidade.**

---

**Data**: 8 de Maio de 2026  
**Versão**: 1.0 (Final)  
**Status**: ✅ Análise Completa - Pronto para Execução
