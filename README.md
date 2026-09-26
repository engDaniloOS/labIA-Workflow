# labIA-Workflow

> Acompanhamento de um curso de **LangGraph** (ministrado em Python), reimplementando cada
> exercício em **.NET/C#** usando o Microsoft Agent Framework (`Microsoft.Agents.AI.Workflows`)
> como equivalente ao LangGraph.

---

## 🎯 Objetivo

O curso é dado em Python, mas este repositório contém **apenas a versão .NET** dos exercícios —
não há código Python aqui. O curso em Python serve só de referência/material de estudo; o que é
versionado é o paralelo em .NET, para:

- Entender os conceitos de orquestração de agentes (grafos de estado, nós, edges condicionais,
  checkpoints, human-in-the-loop, multi-agente etc.) implementando-os fora do ecossistema Python;
- Comparar na prática LangGraph x Microsoft Agent Framework (`Microsoft.Agents.AI.Workflows`);
- Reaproveitar padrões já validados nos projetos [labIA](https://github.com/engDaniloOS/labIA) e
  [IncidentsAgent](https://github.com/engDaniloOS/IncidentsAgent).

---

## 📦 Estrutura do Projeto

Um único projeto .NET na raiz, que evolui conforme os conceitos do curso vão sendo incorporados:

```
labIA-Workflow/
└── labIA-Workflow/
    ├── Program.cs                 # loop de chat via console
    ├── Configs/
    │   ├── ChatClientFactory.cs   # client Gemini (GeminiDotnet.Extensions.AI)
    │   └── ChatSession.cs         # histórico de mensagens em memória
    └── Agents/
        └── GeneralAgent.cs        # definição de agentes (Microsoft.Agents.AI)
```

---

## 🛠️ Tecnologias

| Item | Stack |
|------|-------|
| **Curso (referência)** | LangGraph / LangChain / Python |
| **Implementação (este repo)** | .NET 10, `Microsoft.Agents.AI`, `Microsoft.Agents.AI.Workflows`, C# |
| **LLM** | Google Gemini por enquanto (`AI_API_KEY`, nome genérico pensando em trocar de provedor) |

---

## 🚀 Como executar

Execução direta via `dotnet run`, sem Docker:

```powershell
$env:AI_API_KEY = "sua-chave-aqui"
cd labIA-Workflow/labIA-Workflow
dotnet run
```

> A `AI_API_KEY` também pode ser definida em `Properties/launchSettings.json` para debug local pelo
> Visual Studio — esse arquivo está no `.gitignore` justamente para evitar commitar a chave por
> engano.

---

## 👨‍💻 Autor

**engDaniloOS**

- 🔗 GitHub: [@engDaniloOS](https://github.com/engDaniloOS)
