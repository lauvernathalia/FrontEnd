# Guia prático para pedir implementação

Use este modelo sempre que quiser me pedir para criar ou alterar uma funcionalidade. Quanto mais completo o pedido, mais fielmente eu consigo seguir o padrão do projeto e implementar de primeira.

## Como me pedir

Escreva seu pedido com estes blocos:

1. **Objetivo**
   - O que a funcionalidade deve fazer.
   - Qual problema resolve.

2. **Contexto do negócio**
   - Em qual módulo/tela isso entra.
   - Quem usa.
   - Qual fluxo atual existe hoje.

3. **Regras de negócio**
   - Validações.
   - Permissões.
   - Regras por campo.
   - Regras de bloqueio, cálculo ou status.

4. **Tabelas / entidades envolvidas**
   - Nome das tabelas.
   - Campos relevantes.
   - Relacionamentos.
   - Chaves primárias e estrangeiras.

5. **Integração com API**
   - Endpoints existentes.
   - Métodos HTTP.
   - Payload de entrada e saída.
   - Se a API já existe ou se eu devo criar só o front.

6. **Tela / UX esperada**
   - Campos da tela.
   - Botões.
   - Comportamentos.
   - Mensagens.
   - Fluxo de navegação.

7. **Arquivos de referência**
   - Liste os arquivos que já existem e que devem ser usados como base.
   - Se houver documentação, inclua também.

8. **Critérios de aceite**
   - Como saber que terminou certo.
   - O que precisa funcionar no final.

---

## Modelo pronto para você copiar e preencher

```md
# Pedido de implementação

## 1. Objetivo
Descreva aqui o que deve ser feito.

## 2. Contexto do negócio
- Módulo/tela:
- Quem usa:
- Fluxo atual:

## 3. Regras de negócio
- Regra 1:
- Regra 2:
- Regra 3:

## 4. Tabelas / entidades envolvidas
- Tabela principal:
- Tabelas relacionadas:
- Campos importantes:
- Relacionamentos:

## 5. API / integração
- Endpoint(s):
- Método(s):
- Exemplo de payload:
- Observações:

## 6. Tela / UX esperada
- Campos:
- Botões:
- Validações:
- Mensagens:
- Fluxo:

## 7. Arquivos de referência
- [caminho/do/arquivo]
- [caminho/do/arquivo]
- [caminho/do/arquivo]

## 8. Critérios de aceite
- 
- 
- 
```

---

## Quais arquivos você deve me referenciar

Quando for pedir uma funcionalidade, o ideal é me passar estes tipos de arquivo, se existirem:

### 1. Documentação da funcionalidade
- `*.md` com regras de negócio, fluxo e telas.
- Diagramas, checklist ou fluxo da tela.
- Documentos SQL ou anotações do processo.

### 2. Arquivos Angular do domínio
- `src/app/<dominio>/<dominio>.module.ts`
- `src/app/<dominio>/<dominio>.route.ts`
- `src/app/<dominio>/<dominio>.app.component.ts`
- `src/app/<dominio>/services/*.ts`
- `src/app/<dominio>/models/*.ts`
- `src/app/<dominio>/**/lista/*.ts`
- `src/app/<dominio>/**/novo/*.ts`
- `src/app/<dominio>/**/editar/*.ts`
- `src/app/<dominio>/**/detalhes/*.ts`
- `src/app/<dominio>/**/excluir/*.ts`

### 3. Classes base e padrões compartilhados
- `src/app/base-components/form-base.component.ts`
- `src/app/services/base.service.ts`
- `src/app/services/base.guard.ts`
- `src/app/utils/generic-form-validation.ts`
- `src/app/utils/localstorage.ts`
- `src/app/services/error.handler.service.ts`

### 4. Arquivos de navegação e shell
- `src/app/app-routing.module.ts`
- `src/app/app.component.ts`
- `src/app/app.component.html`
- `src/app/navegacao/navegacao.module.ts`
- `src/app/navegacao/**`

### 5. Modelos de dados e contrato
- Interfaces e models do domínio.
- SQL das tabelas envolvidas.
- Contrato da API, se houver.
- Exemplo de request/response.

### 6. Arquivos que já servem de exemplo
- Uma feature parecida no projeto.
- Um service equivalente.
- Um form semelhante.
- Um guard ou resolver já usado.

---

## Como escolher os arquivos certos

Se a funcionalidade for nova, me passe:
- a documentação do processo
- o módulo semelhante mais próximo
- os models/tabelas
- o contrato da API, se existir

Se a funcionalidade for uma alteração, me passe:
- o arquivo atual da tela
- o service atual
- o model atual
- a documentação das mudanças
- os arquivos que mudam a regra

---

## Exemplo de pedido bom

```md
# Pedido de implementação

## 1. Objetivo
Criar cadastro de representantes com listagem, inclusão, edição, detalhes e exclusão.

## 2. Contexto do negócio
- Módulo/tela: Cadastro Comercial
- Quem usa: equipe comercial
- Fluxo atual: hoje a funcionalidade não existe no front

## 3. Regras de negócio
- Nome obrigatório
- Documento obrigatório
- Email válido
- Bloquear exclusão se houver vínculo

## 4. Tabelas / entidades envolvidas
- Tabela principal: representantes
- Tabelas relacionadas: empresas, usuários
- Campos importantes: id, nome, documento, email, ativo

## 5. API / integração
- GET /representantes
- GET /representantes/{id}
- POST /representantes
- PUT /representantes/{id}
- DELETE /representantes/{id}

## 6. Tela / UX esperada
- Lista com busca e paginação
- Formulário com nome, documento, email e status
- Botões salvar, cancelar, voltar
- Mensagens padrão do projeto

## 7. Arquivos de referência
- `FLUXO_CADASTRO_REPRESENTANTES.md`
- `SCRIPTS_SQL_E_INTEGRACAO_EMAIL.md`
- `src/app/produto/produto.module.ts`
- `src/app/produto/produto.route.ts`
- `src/app/produto/services/produto.service.ts`
- `src/app/base-components/form-base.component.ts`

## 8. Critérios de aceite
- Conseguir listar, cadastrar, editar, ver detalhes e excluir
- Validar todos os campos
- Seguir o padrão visual e estrutural do projeto
```
