# Modelo de Pedido de Implementação

Use este arquivo sempre que quiser me pedir para implementar uma funcionalidade no projeto. Quanto mais completo você preencher, mais fielmente eu consigo seguir o padrão existente.

## 1. Resumo da funcionalidade

- O que deve ser feito?
- Qual problema isso resolve?
- Qual é o objetivo final?

## 2. Contexto do módulo

- Em qual módulo ou domínio isso entra?
- A funcionalidade é nova ou altera algo existente?
- Quem usa essa funcionalidade?

## 3. Estrutura Angular esperada

- Qual pasta/módulo deve receber a implementação?
- Deve seguir o padrão de `lista / novo / editar / detalhes / excluir`?
- Precisa de `route`, `guard`, `resolver` ou `service` novos?
- Deve reutilizar alguma classe base existente?

## 4. Regras de negócio

- Liste as validações.
- Informe campos obrigatórios.
- Informe limites, formatos e dependências entre campos.
- Informe permissões de acesso, se houver.

## 5. Tela e UX

- Descreva os campos da tela.
- Informe botões e ações esperadas.
- Descreva mensagens, estados de carregamento e comportamento desejado.

## 6. Integração com API

- Existe endpoint já pronto?
- Quais são as rotas da API?
- Quais métodos serão usados?
- Quais dados entram e saem?
- Se não existir API, diga se devo montar apenas o front.

## 7. Arquivos que podem ser afetados

- Liste os arquivos que você já sabe que devem mudar.
- Se não souber, eu posso identificar os arquivos necessários.

## 8. Critérios de aceite

- Como saber que a implementação ficou correta?
- O que precisa funcionar ao final?

## 9. Observações adicionais

- Observações técnicas.
- Dependências externas.
- Regras de navegação.
- Casos especiais.

---

## Modelo rápido para copiar e preencher

```md
# Pedido de implementação

## Resumo

## Módulo / domínio

## Estrutura Angular esperada
- [ ] Novo módulo
- [ ] Nova rota
- [ ] Novo service
- [ ] Novo guard
- [ ] Novo resolver
- [ ] Novo componente de lista
- [ ] Novo componente de formulário

## Regras de negócio

## Tela / UX

## Integração com API

## Arquivos que podem ser afetados

## Critérios de aceite

## Observações
```

---

## Exemplo de preenchimento

### Resumo
Criar cadastro de representantes com listagem, inclusão, edição e exclusão.

### Módulo / domínio
Módulo de cadastro comercial.

### Estrutura Angular esperada
- Novo módulo lazy loaded
- Rotas filhas
- Guard para permissões
- Resolver para detalhes/edição
- Componentes de lista e formulário

### Regras de negócio
- Nome obrigatório
- Documento obrigatório
- Email válido
- Bloquear exclusão se houver vínculo

### Tela / UX
- Campo nome
- Campo documento
- Campo email
- Botões salvar, cancelar e voltar

### Integração com API
- `GET /representantes`
- `POST /representantes`
- `PUT /representantes/{id}`
- `DELETE /representantes/{id}`

### Critérios de aceite
- Conseguir listar, cadastrar, editar e excluir representantes
- Validar campos obrigatórios
- Exibir mensagens de erro corretamente
