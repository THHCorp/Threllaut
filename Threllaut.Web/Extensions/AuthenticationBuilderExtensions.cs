namespace Microsoft.AspNetCore.Authentication;

public static class AuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddExternalLogins(this AuthenticationBuilder builder, IConfiguration config)
    {
        if (config["Authentication:Microsoft:ClientId"] is string microsoftClientId)
            builder = builder.AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = microsoftClientId;
                microsoftOptions.ClientSecret = config["Authentication:Microsoft:ClientSecret"]
                    ?? throw new InvalidOperationException("Missing Microsoft Secret ID.");
            });
        if (config["Authentication:Google:ClientId"] is string googleClientId)
            builder = builder.AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = googleClientId;
                googleOptions.ClientSecret = config["Authentication:Google:ClientSecret"]
                    ?? throw new InvalidOperationException("Missing Google Client Secret.");
            });
        if (config["Authentication:Github:ClientId"] is string githubClientId)
            builder = builder.AddGitHub(githubOptions =>
            {
                githubOptions.ClientId = githubClientId;
                githubOptions.ClientSecret = config["Authentication:Github:ClientSecret"]
                    ?? throw new InvalidOperationException("Missing Github Client secret.");
            });
        if (config["Authentication:GitLab:ClientId"] is string gitlabClientId)
            builder = builder.AddGitLab(gitlabOptions =>
            {
                gitlabOptions.ClientId = gitlabClientId;
                gitlabOptions.ClientSecret = config["Authentication:GitLab:ClientSecret"]
                    ?? throw new InvalidOperationException("Missing GitLab Secret.");
            });
        return builder;
    }
}
