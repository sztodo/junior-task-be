using System.Xml.Linq;
using FluentAssertions;

namespace Domain.Tests;

[TestClass]
public class LayerDependencyTests
{
    private static string FindSolutionRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !dir.GetFiles("*.slnx").Any())
            dir = dir.Parent;

        return dir?.FullName
            ?? throw new InvalidOperationException(
                "Could not find solution root. Is the .slnx file present?");
    }

    private static IEnumerable<string> GetProjectReferences(string csprojPath)
    {
        var doc = XDocument.Load(csprojPath);
        return doc.Descendants("ProjectReference")
                  .Select(e => e.Attribute("Include")?.Value ?? string.Empty)
                  .Where(v => !string.IsNullOrWhiteSpace(v));
    }

    [TestMethod]
    public void Domain_HasNoProjectReferences_IsCompletelyIsolated()
    {
        var root = FindSolutionRoot();
        var csprojPath = Path.Combine(
            root, "src", "Domain", "Domain.csproj");

        var references = GetProjectReferences(csprojPath).ToList();

        references.Should().BeEmpty(
            because: "Domain is the innermost layer and must have zero dependencies. " +
                     $"Found: {string.Join(", ", references)}");
    }

    [TestMethod]
    public void Application_ReferencesOnlyDomain()
    {
        var root = FindSolutionRoot();
        var csprojPath = Path.Combine(
            root, "src", "Application", "Application.csproj");

        var references = GetProjectReferences(csprojPath).ToList();

        references.Should().HaveCount(1,
            because: "Application should only reference Domain");

        references[0].Should().Contain("Domain",
            because: "the only allowed reference from Application is Domain");
    }

    [TestMethod]
    public void Application_DoesNotReferenceInfrastructure()
    {
        var root = FindSolutionRoot();
        var csprojPath = Path.Combine(
            root, "src", "Application", "Application.csproj");

        var references = GetProjectReferences(csprojPath).ToList();

        references.Should().NotContain(r => r.Contains("Infrastructure"),
            because: "Application must never depend on Infrastructure");
    }

    [TestMethod]
    public void Application_DoesNotReferenceApi()
    {
        var root = FindSolutionRoot();
        var csprojPath = Path.Combine(
            root, "src", "Application", "Application.csproj");

        var references = GetProjectReferences(csprojPath).ToList();

        references.Should().NotContain(r => r.Contains("Api"),
            because: "Application must never depend on the Api layer");
    }

    [TestMethod]
    public void Infrastructure_ReferencesOnlyDomainAndApplication()
    {
        var root = FindSolutionRoot();
        var csprojPath = Path.Combine(
            root, "src", "Infrastructure", "Infrastructure.csproj");

        var references = GetProjectReferences(csprojPath).ToList();

        references.Should().AllSatisfy(r =>
            (r.Contains("Domain") || r.Contains("Application"))
                .Should().BeTrue(because:
                    $"Infrastructure may only reference Domain or Application, found: {r}"));
    }

    [TestMethod]
    public void Infrastructure_DoesNotReferenceApi()
    {
        var root = FindSolutionRoot();
        var csprojPath = Path.Combine(
            root, "src", "Infrastructure", "Infrastructure.csproj");

        var references = GetProjectReferences(csprojPath).ToList();

        references.Should().NotContain(r => r.Contains("Api"),
            because: "Infrastructure must not reference the Api layer");
    }

    [TestMethod]
    public void Domain_DoesNotReferenceApplication()
    {
        var root = FindSolutionRoot();
        var csprojPath = Path.Combine(
            root, "src", "Domain", "Domain.csproj");

        var references = GetProjectReferences(csprojPath).ToList();

        references.Should().NotContain(r => r.Contains("Application"),
            because: "circular reference Domain → Application is forbidden");
    }
}
