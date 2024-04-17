using Octokit;

namespace DailyPushAutomator.Processes;

public class Automator
{
    // private readonly string _githubToken = "your_github_token";
    // private readonly string _owner = "your_github_username";
    // private readonly string _repo = "your_repository_name";
    private readonly  IConfiguration _configuration;
    
    public Automator(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task PushToRepoAsync()
    {
        var githubToken = _configuration["GithubSettings:repoToken"];
        var repo = _configuration["GithubSettings:repo"];
        var owner = _configuration["GithubSettings:owner"];
        try
        {
            var github = new GitHubClient(new ProductHeaderValue("DailyPushAutomator"))
            {
                Credentials = new Credentials(githubToken)
            };

            // Get the repository
            var repository = await github.Repository.Get(owner, repo);

            // Try to get the file
            RepositoryContent file;
            try
            {
                file = (await github.Repository.Content.GetAllContentsByRef(owner, repo, "Ahercode.txt", "main")).First();
            }
            catch (NotFoundException)
            {
                // If the file does not exist, create it
                await github.Repository.Content.CreateFile(owner, repo, "Ahercode.txt",
                    new CreateFileRequest("Initial commit", "Initial content"));
                file = (await github.Repository.Content.GetAllContentsByRef(owner, repo, "Ahercode.txt", "main")).First();
            }

            // Update the file content with the current time
            var updatedContent = $"Last updated at: {DateTime.Now}";
            if (file.Content != updatedContent)
            {
                var updateChangeSet = await github.Repository.Content.UpdateFile(
                    owner,
                    repo,
                    "Ahercode.txt",
                    new UpdateFileRequest("New update added!!", updatedContent, file.Sha, "main"));

                Console.WriteLine($"Updated file at: {DateTime.Now}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}