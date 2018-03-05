using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MLS.Agent.Tools;
using Recipes;
using Workspace = MLS.Agent.Tools.Workspace;

namespace WorkspaceServer.Tests
{
    public static class Create
    {
        public static async Task<Workspace> ConsoleWorkspace([CallerMemberName] string testName = null)
        {
            var workspace = Workspace.Copy(
                await Default.ConsoleWorkspace,
                testName);

            await workspace.EnsureBuilt();

            return workspace;
        }

        public static async Task<Workspace> WebApiWorkspace([CallerMemberName] string testName = null)
        {
            var workspace = Workspace.Copy(
                await Default.WebApiWorkspace,
                testName);

            await workspace.EnsurePublished();

            return workspace;
        }

        public static Workspace EmptyWorkspace([CallerMemberName] string testName = null, IWorkspaceInitializer initializer = null) =>
            new Workspace(Workspace.CreateDirectory(testName), initializer: initializer);

        public static Models.Execution.Workspace SimpleRunRequest(
            string consoleOutput = "Hello!",
            string workspaceType = null) =>
            SimpleWorkspace(consoleOutput, workspaceType);

        public static Models.Execution.Workspace SimpleWorkspace(
            string consoleOutput = "Hello!",
            string workspaceType = null) =>
            new Models.Execution.Workspace(SimpleConsoleAppCodeWithoutNamespaces(consoleOutput), workspaceType: workspaceType);

        public static string SimpleWorkspaceAsJson(
            string consoleOutput = "Hello!",
            string workspaceType = null) =>
            new
            {
                buffer = SimpleConsoleAppCodeWithoutNamespaces(consoleOutput),
                workspaceType
            }.ToJson();

        public static string SimpleConsoleAppCodeWithoutNamespaces(string consoleOutput)
        {
            return $@"
using System;

public static class Hello
{{
    public static void Main()
    {{
        Console.WriteLine(""{consoleOutput}"");
    }}
}}";
        }
    }
}
