using System.IO;
using System.Linq;

namespace XmlToCode
{
    internal class ProjectAppender
    {
        public void AddFile(string className, string code)
        {
            // todo: Ensure folder is correct!
            string pathLevelAdjustment = "..\\..\\";
            string workingDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), pathLevelAdjustment));
            string generatedClassFileName = $"{className}.cs";
            string targetFilePath = Path.Combine(workingDirectory, generatedClassFileName);

            if (!File.Exists(targetFilePath)
                || !File.ReadAllText(targetFilePath).SequenceEqual(code))
            {
                File.WriteAllText(targetFilePath, code);

                // todo: 
                // Add cs file to csproj
                // https://stackoverflow.com/questions/18544354/how-to-programmatically-include-a-file-in-my-project
                // https://stackoverflow.com/questions/707107/parsing-visual-studio-solution-files

                AddFileToProject(workingDirectory, generatedClassFileName);
            }
        }

        private static void AddFileToProject(string workingDirectory, string generatedClassFileName)
        {
            // todo: get csproj file name dynamically
            // Error in Microsoft.Build: InternalErrorException: https://github.com/Microsoft/msbuild/issues/1889 --> Solution: Install-Package Microsoft.Build.Utilities.Core -Version 15.1.1012
            var project = new Microsoft.Build.Evaluation.Project(Path.Combine(workingDirectory, "XmlToCode.csproj"));
            if (project.Items.FirstOrDefault(i => i.EvaluatedInclude == generatedClassFileName) == null)
            {
                project.AddItem("Compile", generatedClassFileName);
                project.Save();
            }
        }
    }
}
