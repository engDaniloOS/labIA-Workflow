# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository purpose

This repo tracks a LangGraph course. The course itself is taught in Python, but **this repo
contains only the .NET reimplementation** of each exercise — there is no Python code here. Every
concept taught with LangGraph (state graphs, nodes, conditional edges, checkpoints,
human-in-the-loop, multi-agent, etc.) is reimplemented in .NET/C# using the Microsoft Agent
Framework (`Microsoft.Agents.AI` + `Microsoft.Agents.AI.Workflows`), the closest .NET analog to
LangGraph's graph/state orchestration model. Treat the Python/LangGraph course material as
reference only, never as something to add to this repo.

The repo is a single evolving .NET project at the root (`labIA-Workflow/labIA-Workflow/`), not a
collection of per-module folders — course concepts are added as new files/folders inside this one
project (e.g. `Agents/`, `Configs/`) as they come up, rather than as separate module projects. Keep
this file in sync with the structure that actually emerges (don't let it drift into aspirational
documentation).

## Sibling projects (not part of this repo, but the reference pattern)

Other repos under `C:\Users\User\Documents\GitHub` already build .NET agents and are the source of
truth for conventions to reuse here:

- **labIA** — closest analog to this repo's .NET side. Uses `Microsoft.Agents.AI.Workflows` on
  .NET 10 to build sequential and decision-branching workflows, plus a RAG example
  (`Microsoft.Extensions.VectorData` + `Microsoft.SemanticKernel.Text.TextChunker`). Chat/LLM
  client wrapper lives in a shared `Comum` project. Uses Gemini via `GEMINI_API_KEY` env var.
- **IncidentsAgent** — multi-service example (ServiceNow mock + agent API) showing
  `Microsoft.Agents.AI` wired into an ASP.NET Core API, run via `docker compose up --build`.
- **labIA-Docker** — Dockerized single-project agent (`DocChat`) example.

When scaffolding the .NET side of a module here, prefer matching these repos' package choices
(`Microsoft.Agents.AI`, `Microsoft.Agents.AI.Workflows`, target framework `net10.0`,
`ImplicitUsings`/`Nullable` enabled) unless the course material calls for something else on the
Python/LangGraph side that has no equivalent.

This repo currently uses **Google Gemini** as the LLM provider, but — unlike the sibling repos,
which name the env var `GEMINI_API_KEY` — the env var here is deliberately named **`AI_API_KEY`**
(provider-agnostic), since a different LLM may be swapped in later. Unlike `IncidentsAgent` and
`labIA-Docker`, there is no Docker/docker-compose setup here: the app runs directly via `dotnet
run`, no containers.

## Structure

```
labIA-Workflow/
└── labIA-Workflow/              # single .csproj, root of the whole project
    ├── Program.cs                # entry point — console chat loop
    ├── Configs/
    │   ├── ChatClientFactory.cs  # builds the IChatClient (Gemini, via GeminiDotnet.Extensions.AI)
    │   └── ChatSession.cs        # in-memory message history with a size-capped window
    └── Agents/
        └── GeneralAgent.cs       # AIAgent definitions (Microsoft.Agents.AI)
```

Do not add Python code/projects to this repo; the course's Python material stays external,
referenced only in commit messages/notes for the concept mapping when relevant.

## Configuration

LLM provider is currently **Google Gemini**, via the `AI_API_KEY` environment variable (read in
`ChatClientFactory.GetApiKey()`, which throws if unset — fails fast instead of sending an empty key
to the API). The var is named generically, not `GEMINI_API_KEY`, so the provider can be swapped
later without a rename. Set it in your shell before `dotnet run`, or in
`Properties/launchSettings.json` locally — that file is gitignored
(`**/Properties/launchSettings.json`) specifically because it's an easy place to accidentally
commit a real API key; never remove it from `.gitignore`.

## Commands

```powershell
cd labIA-Workflow/labIA-Workflow
$env:AI_API_KEY = "sua-chave-aqui"
dotnet run
```

No docker, no test project yet — `dotnet build` from the same folder is the only other command in
use so far.
